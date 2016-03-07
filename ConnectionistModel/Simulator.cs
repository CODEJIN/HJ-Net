using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ConnectionistModel
{
    class Simulator
    {   
        Random random;        

        public Simulator(Random random)
        {
            this.random = random;

            LayerDictionary = new Dictionary<string, Layer>();
            ConnectionDictionary = new Dictionary<string, Connection>();
            StimuliPackDictionary = new Dictionary<string, StimuliPack>();
            ProcessDictionary = new Dictionary<string, ConnectionistModel.Process>();
            TestDataList = new List<TestData>();
            WeightDataDictionary = new Dictionary<int, Dictionary<string, Matrix<double>>>();

            //Initial Value(Temporal)
            Momentum = 0.9;
            ActivationCriterion = 0.6;
            InactivationCriterion = 0.4;
            DecayRate = 0.000001;
            WeightRange = 1.0;
            LearningRate = 0.1;

        }

        public void LayerMaking(string layerName, int unitCount, int cleanupUnitCount)
        {
            Layer newLayer = new Layer(random, layerName, unitCount, cleanupUnitCount, WeightRange);
            LayerDictionary[layerName] = newLayer;
        }
        public void LayerMaking(string layerName, int unitAmount, int cleanUpUnitAmount, int tick)
        {
            BPTTLayer newLayer = new BPTTLayer(random, layerName, unitAmount, cleanUpUnitAmount, WeightRange, tick);
            LayerDictionary[layerName] = newLayer;
        }

        public void ConnectionMaking(string connectionName, string sendLayerName, string receiveLayerName)
        {
            Connection newConnection = new Connection(this.random, connectionName, LayerDictionary[sendLayerName], LayerDictionary[receiveLayerName], WeightRange);
            ConnectionDictionary[connectionName] = newConnection;
        }

        public void SimulatorRenewal()
        {
            foreach (string key in LayerDictionary.Keys) LayerDictionary[key].InitialWeightSetting(WeightRange);
            foreach (string key in ConnectionDictionary.Keys) ConnectionDictionary[key].InitialWeightSetting(WeightRange);
        }

        public bool StimuliImport(string packName, string fileName)
        {
            StimuliPack newStimuliPack = new StimuliPack();
            newStimuliPack.Name = packName;

            StreamReader streamReader = new StreamReader(fileName);

            try
            {
                string[] nameReadData = streamReader.ReadLine().Split('\t');
                for (int i = 3; i < nameReadData.Length; i++) newStimuliPack.PatternNameList.Add(nameReadData[i]);

                while (!streamReader.EndOfStream)
                {
                    string[] readData = streamReader.ReadLine().Split('\t');

                    StimulusData newStimulusData = new StimulusData();
                    newStimulusData.PackName = packName;
                    newStimulusData.Name = readData[0];
                    newStimulusData.Possibility = double.Parse(readData[1]);
                    newStimulusData.TimeStamp = int.Parse(readData[2]);
                    
                    for (int i = 3; i < readData.Length; i++)
                    {
                        newStimulusData[newStimuliPack.PatternNameList[i - 3]] = ActivationPatternMaker.Maker(readData[i]);
                    }
                    newStimuliPack.Add(newStimulusData);
                }
                if (newStimuliPack.RegularityCheck())
                {
                    StimuliPackDictionary[newStimuliPack.Name] = newStimuliPack;
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                streamReader.Close();
            }
        }
        
        public void Process(TrialInformation trialInformation, int epoch)
        {
            Process process = trialInformation.Process;
            StimuliMatrix stimuliMatrixData = trialInformation.StimuliMatrix;
            Dictionary<int, string> patternSetup = trialInformation.PatternSetup;

            foreach (string key in LayerDictionary.Keys) LayerDictionary[key].Initialize(process.LayerStateDictionary[key], process.LayerDamagedSDDictionary[key], stimuliMatrixData.StimuliCount);
            foreach (string key in ConnectionDictionary.Keys) ConnectionDictionary[key].Initialize(process.ConnectionStateDictionary[key], process.ConnectionDamagedSDDictionary[key]);

            int timeStamp = 0;

            for (int i = 0; i < process.Count; i++)
            {
                Matrix<double> targetPattern;
                TestData newTestData;
                switch (process[i].Code)
                {
                    case OrderCode.ActivationInput:
                        LayerDictionary[process[i].Layer1Name].ActivationInput(stimuliMatrixData[patternSetup[i]]);
                        break;
                    case OrderCode.ActivationCalculate_Sigmoid:
                        LayerDictionary[process[i].Layer1Name].CalculateActivation_Sigmoid(this.Momentum);
                        break;
                    case OrderCode.ActivationCalculate_Softmax:
                        LayerDictionary[process[i].Layer1Name].CalculateActivation_Softmax();
                        break;
                    case OrderCode.ActivationSend:
                        LayerDictionary[process[i].Layer1Name].SendActivation();
                        break;
                    case OrderCode.OutputErrorCalculate_for_Sigmoid:
                        LayerDictionary[process[i].Layer1Name].ErrorCalculate_Sigmoid(stimuliMatrixData[patternSetup[i]], this.Momentum);
                        break;
                    case OrderCode.OutputErrorCalculate_for_Softmax:
                        LayerDictionary[process[i].Layer1Name].ErrorCalculate_Softmax(stimuliMatrixData[patternSetup[i]]);
                        break;
                    case OrderCode.HiddenErrorCalculate_for_Sigmoid:
                        LayerDictionary[process[i].Layer1Name].ErrorCalculate_Sigmoid(this.Momentum);
                        break;
                    case OrderCode.HiddenErrorCalculate_for_Softmax:
                        LayerDictionary[process[i].Layer1Name].ErrorCalculate_Softmax();
                        break;
                    case OrderCode.Interact: //Inner Unit Interact
                        LayerDictionary[process[i].Layer1Name].Interact();
                        break;
                    case OrderCode.InterconnectionWeightRenewal: //Inner Unit Weight Change
                        LayerDictionary[process[i].Layer1Name].InterConnectionWeightRenewal(this.LearningRate, this.DecayRate);
                        break;
                    case OrderCode.LayerDuplicate: //Layer Duplicate                        
                        LayerDictionary[process[i].Layer1Name].Duplicate(LayerDictionary[process[i].Layer2Name]);
                        break;
                    case OrderCode.ConnectionDuplicate: //Connection Duplicate                        
                        ConnectionDictionary[process[i].Connection1Name].Duplicate(ConnectionDictionary[process[i].Connection2Name]);
                        break;
                    case OrderCode.TransposedConnectionDuplicate:
                        ConnectionDictionary[process[i].Connection1Name].TransposedDuplicate(ConnectionDictionary[process[i].Connection2Name]);
                        break;
                    case OrderCode.BiasRenewal:
                        LayerDictionary[process[i].Layer1Name].BiasRenewal(LearningRate, DecayRate);
                        break;
                    case OrderCode.WeightRenewal: //Weight Renewal                        
                        ConnectionDictionary[process[i].Connection1Name].WeightRenewal(this.LearningRate, this.DecayRate);
                        break;
                    case OrderCode.TestValueStore:
                        List<double> meanSEList;
                        List<double> meanSSList;
                        List<double> meanCEList;
                        List<bool> correctnessList;
                        List<double> meanActivationList;
                        List<double> meanAUActivationList;
                        List<double> meanIUActivationList;

                        Layer testLayer = LayerDictionary[process[i].Layer1Name];
                        testLayer.Test(stimuliMatrixData[patternSetup[i]], ActivationCriterion, InactivationCriterion,
                            out meanSEList, out meanSSList, out meanCEList, out correctnessList,
                            out meanActivationList, out meanAUActivationList, out meanIUActivationList);

                        for(int index=0;index<stimuliMatrixData.StimuliCount;index++)
                        {
                            newTestData = new TestData();
                            newTestData.ProcessName = process.Name;
                            newTestData.StimuliPackName = trialInformation.StimuliMatrix.PackName;
                            newTestData.Name = stimuliMatrixData.NameList[index];
                            newTestData.Epoch = epoch;
                            newTestData.TimeStamp = timeStamp;

                            newTestData.MeanSquredError = meanSEList[index];
                            newTestData.Correctness = correctnessList[index];
                            newTestData.MeanSemanticStress = meanSSList[index];
                            newTestData.MeanCrossEntropy = meanCEList[index];
                            newTestData.MeanActivation = meanActivationList[index];
                            newTestData.MeanActiveUnitActivation = meanAUActivationList[index];
                            newTestData.MeanInactiveUnitActivation = meanIUActivationList[index];
                            newTestData.TargetPattern = stimuliMatrixData[patternSetup[i]].Row(index).ToRowMatrix();
                            newTestData.OutputActivation = testLayer.LayerActivationMatrix.Row(index).ToRowMatrix();

                            if (UseActivationInformation) newTestData.LayerActivationInforamtion = ActivationInforamtionReader(index);

                            TestDataList.Add(newTestData);
                        }
                        
                        timeStamp++;
                        break;
                    case OrderCode.LayerInitialize:
                        LayerDictionary[process[i].Layer1Name].Initialize(process.LayerStateDictionary[process[i].Layer1Name], process.LayerDamagedSDDictionary[process[i].Layer1Name], stimuliMatrixData.StimuliCount);
                        break;
                    case OrderCode.EndInitialize:
                        //실제론 아무것도 하지 않으나, 반드시 뒤에 어떤 Order도 올 수없도록 할 것
                        break;
                    case OrderCode.LayerToCleanUpForwardProcess:
                        LayerDictionary[process[i].Layer1Name].LayerToCleanUpForwardProcess(Momentum);
                        break;
                    case OrderCode.CleanUpToLayerForwardProcess:
                        LayerDictionary[process[i].Layer1Name].CleanUpToLayerForwardProcess();
                        break;
                    case OrderCode.CleanUpBackwardProcess:
                        LayerDictionary[process[i].Layer1Name].CleanUpBackwordProcess(Momentum, LearningRate, DecayRate);
                        break;
                    case OrderCode.TickIn:
                        ((BPTTLayer)LayerDictionary[process[i].Layer1Name]).TickIn(Momentum, stimuliMatrixData.StimuliCount);
                        break;
                    case OrderCode.TickProgress:
                        ((BPTTLayer)LayerDictionary[process[i].Layer1Name]).TickProgress(Momentum, stimuliMatrixData.StimuliCount);
                        break;
                    case OrderCode.TickOut:
                        ((BPTTLayer)LayerDictionary[process[i].Layer1Name]).TickOut();
                        break;
                    case OrderCode.BaseWeightRenewal:
                        ((BPTTLayer)LayerDictionary[process[i].Layer1Name]).WeightRenewal(Momentum, LearningRate, DecayRate);
                        break;
                    case OrderCode.SRNTraining:
                        for (int cycle = 0; cycle < stimuliMatrixData.TimeStamp; cycle++)
                        {
                            LayerDictionary[process[i].SRNInputLayerName].ActivationInput(stimuliMatrixData[process[i].SRNInputPatternName + cycle.ToString()]);
                            LayerDictionary[process[i].SRNInputLayerName].SendActivation();
                            LayerDictionary[process[i].SRNContextLayerName].SendActivation();

                            LayerDictionary[process[i].SRNHiddenLayerName].CalculateActivation_Sigmoid(Momentum);
                            LayerDictionary[process[i].SRNHiddenLayerName].SendActivation();

                            if (process[i].SRNErrorCalculation == "Sigmoid")
                            {
                                LayerDictionary[process[i].SRNOutputLayerName].CalculateActivation_Sigmoid(Momentum);
                                LayerDictionary[process[i].SRNOutputLayerName].ErrorCalculate_Sigmoid(stimuliMatrixData[process[i].SRNOutputPatternName + cycle.ToString()], Momentum);
                            }
                            else if (process[i].SRNErrorCalculation == "Softmax")
                            {
                                LayerDictionary[process[i].SRNOutputLayerName].CalculateActivation_Softmax();
                                LayerDictionary[process[i].SRNOutputLayerName].ErrorCalculate_Softmax(stimuliMatrixData[process[i].SRNOutputPatternName + cycle.ToString()]);
                            }
                                                        
                            LayerDictionary[process[i].SRNHiddenLayerName].ErrorCalculate_Sigmoid(Momentum);

                            ConnectionDictionary[process[i].SRNIHConnectionName].WeightRenewal(LearningRate, DecayRate);
                            ConnectionDictionary[process[i].SRNCHConnectionName].WeightRenewal(LearningRate, DecayRate);
                            ConnectionDictionary[process[i].SRNHOConnectionName].WeightRenewal(LearningRate, DecayRate);

                            LayerDictionary[process[i].SRNOutputLayerName].BiasRenewal(LearningRate, DecayRate);
                            LayerDictionary[process[i].SRNHiddenLayerName].BiasRenewal(LearningRate, DecayRate);

                            LayerDictionary[process[i].SRNHiddenLayerName].Duplicate(LayerDictionary[process[i].SRNContextLayerName]);

                            LayerDictionary[process[i].SRNInputLayerName].Initialize(process.LayerStateDictionary[process[i].SRNInputLayerName], process.LayerDamagedSDDictionary[process[i].SRNInputLayerName], stimuliMatrixData.StimuliCount);
                            LayerDictionary[process[i].SRNHiddenLayerName].Initialize(process.LayerStateDictionary[process[i].SRNHiddenLayerName], process.LayerDamagedSDDictionary[process[i].SRNHiddenLayerName], stimuliMatrixData.StimuliCount);
                            LayerDictionary[process[i].SRNOutputLayerName].Initialize(process.LayerStateDictionary[process[i].SRNOutputLayerName], process.LayerDamagedSDDictionary[process[i].SRNOutputLayerName], stimuliMatrixData.StimuliCount);
                        }
                        break;
                    case OrderCode.SRNTest:
                        for (int cycle = 0; cycle < stimuliMatrixData.TimeStamp; cycle++)
                        {
                            targetPattern = stimuliMatrixData[process[i].SRNOutputPatternName + cycle.ToString()];

                            LayerDictionary[process[i].SRNInputLayerName].ActivationInput(stimuliMatrixData[process[i].SRNInputPatternName + cycle.ToString()]);
                            LayerDictionary[process[i].SRNInputLayerName].SendActivation();
                            LayerDictionary[process[i].SRNContextLayerName].SendActivation();

                            LayerDictionary[process[i].SRNHiddenLayerName].CalculateActivation_Sigmoid(Momentum);
                            LayerDictionary[process[i].SRNHiddenLayerName].SendActivation();

                            if (process[i].SRNErrorCalculation == "Sigmoid") LayerDictionary[process[i].SRNOutputLayerName].CalculateActivation_Sigmoid(Momentum);
                            else if (process[i].SRNErrorCalculation == "Softmax") LayerDictionary[process[i].SRNOutputLayerName].CalculateActivation_Softmax();


                            List<double> srnMeanSEList;
                            List<double> srnMeanSSList;
                            List<double> srnMeanCEList;
                            List<bool> srnCorrectnessList;
                            List<double> srnMeanActivationList;
                            List<double> srnMeanAUActivationList;
                            List<double> srnMeanIUActivationList;

                            Layer srnTestLayer = LayerDictionary[process[i].SRNOutputLayerName];
                            srnTestLayer.Test(targetPattern, ActivationCriterion, InactivationCriterion,
                                out srnMeanSEList, out srnMeanSSList, out srnMeanCEList, out srnCorrectnessList,
                                out srnMeanActivationList, out srnMeanAUActivationList, out srnMeanIUActivationList);

                            for (int index = 0; index < stimuliMatrixData.StimuliCount; index++)
                            {
                                newTestData = new TestData();
                                newTestData.ProcessName = process.Name;
                                newTestData.StimuliPackName = trialInformation.StimuliMatrix.PackName;
                                newTestData.Name = stimuliMatrixData.NameList[index];
                                newTestData.Epoch = epoch;
                                newTestData.TimeStamp = timeStamp;

                                newTestData.MeanSquredError = srnMeanSEList[index];
                                newTestData.Correctness = srnCorrectnessList[index];
                                newTestData.MeanSemanticStress = srnMeanSSList[index];
                                newTestData.MeanCrossEntropy = srnMeanCEList[index];
                                newTestData.MeanActivation = srnMeanActivationList[index];
                                newTestData.MeanActiveUnitActivation = srnMeanAUActivationList[index];
                                newTestData.MeanInactiveUnitActivation = srnMeanIUActivationList[index];
                                newTestData.TargetPattern = targetPattern.Row(index).ToRowMatrix();
                                newTestData.OutputActivation = srnTestLayer.LayerActivationMatrix.Row(index).ToRowMatrix();

                                if (UseActivationInformation) newTestData.LayerActivationInforamtion = ActivationInforamtionReader(index);

                                TestDataList.Add(newTestData);
                            }
                            timeStamp++;

                            LayerDictionary[process[i].SRNHiddenLayerName].Duplicate(LayerDictionary[process[i].SRNContextLayerName]);

                            LayerDictionary[process[i].SRNInputLayerName].Initialize(process.LayerStateDictionary[process[i].SRNInputLayerName], process.LayerDamagedSDDictionary[process[i].SRNInputLayerName], stimuliMatrixData.StimuliCount);
                            LayerDictionary[process[i].SRNHiddenLayerName].Initialize(process.LayerStateDictionary[process[i].SRNHiddenLayerName], process.LayerDamagedSDDictionary[process[i].SRNHiddenLayerName], stimuliMatrixData.StimuliCount);
                            LayerDictionary[process[i].SRNOutputLayerName].Initialize(process.LayerStateDictionary[process[i].SRNOutputLayerName], process.LayerDamagedSDDictionary[process[i].SRNOutputLayerName], stimuliMatrixData.StimuliCount);
                        }
                        break;
                }
            }
        }

        public string OutputResult()
        {
            string resultPath = Application.StartupPath + "\\" + String.Format("{0:yyyy MM dd HH mm ss}", DateTime.Now) + "\\";
            if (!Directory.Exists(resultPath)) Directory.CreateDirectory(resultPath);

            InformationDataSave(resultPath + "Inforamtion.txt");
            RawDataSave(resultPath + "RawData.txt");
            SummarySave(resultPath);
            if (UseActivationInformation) ActivationInformationWriter(resultPath + "ActivationData.txt");
            if (UseWeightInformation) WeightInformationWriter(resultPath + "WeightData.txt");

            return resultPath;
        }
        private void InformationDataSave(string fileName)
        {
            StreamWriter informationStreamWriter = new StreamWriter(fileName);
            StringBuilder informationData = new StringBuilder();
            informationData.AppendLine("Momentum: " + Momentum);
            informationData.AppendLine("Activation Criterion: " + ActivationCriterion);
            informationData.AppendLine("Inactivation Criterion: " + InactivationCriterion);
            informationData.AppendLine("Decay Rate: " + DecayRate);
            informationData.AppendLine("Initial Weight Range: " + WeightRange);
            informationData.AppendLine("Learning Rate: " + LearningRate);            
            informationData.AppendLine();

            informationData.AppendLine("Layer Information");
            foreach (string key in LayerDictionary.Keys)
            {
                informationData.Append("-" + LayerDictionary[key].Name + "(");
                informationData.Append(LayerDictionary[key].LayerType + ", " + LayerDictionary[key].UnitCount + ", " + LayerDictionary[key].CleanupUnitCount);
                if (LayerDictionary[key].LayerType == LayerType.BPTTLayer) informationData.Append(", " + ((BPTTLayer)LayerDictionary[key]).Tick);
                informationData.AppendLine(")");
            }

            informationData.AppendLine();
            informationData.AppendLine("Connection Information");
            foreach (string key in ConnectionDictionary.Keys)
            {
                informationData.Append("-" + ConnectionDictionary[key].Name + "(");
                informationData.Append(ConnectionDictionary[key].SendLayer.Name + " -> " + ConnectionDictionary[key].ReceiveLayer.Name);
                informationData.AppendLine(")");
            }

            informationData.AppendLine();
            informationData.AppendLine("Layer Status Information");
            foreach (string key in ProcessDictionary.Keys)
            {
                informationData.AppendLine("-" + ProcessDictionary[key].Name);
                foreach(string layerKey in ProcessDictionary[key].LayerStateDictionary.Keys)
                {
                    switch(ProcessDictionary[key].LayerStateDictionary[layerKey])
                    {
                        case LayerState.On:
                        case LayerState.Off:
                            informationData.AppendLine("--" + key + ": "+ ProcessDictionary[key].LayerStateDictionary[layerKey].ToString());
                            break;
                        case LayerState.Damaged:
                            informationData.Append("--" + key + ": " + ProcessDictionary[key].LayerStateDictionary[layerKey].ToString() + "(");
                            informationData.Append(ProcessDictionary[key].LayerDamagedSDDictionary[layerKey]);
                            informationData.AppendLine(")");
                            break;
                    }
                }
            }

            informationData.AppendLine();
            informationData.AppendLine("Connection Status Information");
            foreach (string key in ProcessDictionary.Keys)
            {
                informationData.AppendLine("-" + ProcessDictionary[key].Name);
                foreach (string connectionKey in ProcessDictionary[key].ConnectionStateDictionary.Keys)
                {
                    switch (ProcessDictionary[key].ConnectionStateDictionary[connectionKey])
                    {
                        case ConnectionState.On:
                        case ConnectionState.Off:
                            informationData.AppendLine("--" + key + ": " + ProcessDictionary[key].ConnectionStateDictionary[connectionKey].ToString());
                            break;
                        case ConnectionState.Damaged:
                            informationData.Append("--" + key + ": " + ProcessDictionary[key].ConnectionStateDictionary[connectionKey].ToString() + "(");
                            informationData.Append(ProcessDictionary[key].ConnectionDamagedSDDictionary[connectionKey]);
                            informationData.AppendLine(")");
                            break;
                    }
                }
            }

            informationStreamWriter.Write(informationData);
            informationStreamWriter.Flush();
            informationStreamWriter.Close();
        }
        private void RawDataSave(string fileName)
        {
            StreamWriter rawStreamWriter = new StreamWriter(fileName);
            rawStreamWriter.WriteLine("ProcessName\tStimuliPackName\tName\tEpoch\tTimeStamp\tMeanSquredError\tMeanSemanticStress\tMeanCrossEntropy\tCorrectness\tCorrectUnit\tIncorrectActUnit\tIncorrectInactUnit\tMeanActiveUnitActivation\tMeanInactiveUnitActivation\tUnit");
            foreach (TestData testData in TestDataList)
            {
                int incorrectAct = 0;
                int incorrectInact = 0;
                int correct = 0;
                StringBuilder unitData = new StringBuilder();
                for (int unitIndex = 0; unitIndex < testData.OutputActivation.ColumnCount; unitIndex++)
                {
                    if (testData.TargetPattern[0, unitIndex] > ActivationCriterion && testData.OutputActivation[0, unitIndex] < ActivationCriterion)
                    {
                        unitData.Append("▲");
                        incorrectAct++;

                    }
                    else if (testData.TargetPattern[0, unitIndex] < InactivationCriterion && testData.OutputActivation[0, unitIndex] > InactivationCriterion)
                    {
                        unitData.Append("▼");
                        incorrectInact++;
                    }
                    else
                    {
                        unitData.Append("◎");
                        correct++;
                    }
                    unitData.Append(testData.OutputActivation[0, unitIndex] + "\t");
                }

                rawStreamWriter.Write(testData.ProcessName + "\t");
                rawStreamWriter.Write(testData.StimuliPackName + "\t");
                rawStreamWriter.Write(testData.Name + "\t");
                rawStreamWriter.Write(testData.Epoch + "\t");
                rawStreamWriter.Write(testData.TimeStamp + "\t");
                rawStreamWriter.Write(testData.MeanSquredError + "\t");
                rawStreamWriter.Write(testData.MeanSemanticStress + "\t");
                rawStreamWriter.Write(testData.MeanCrossEntropy + "\t");
                rawStreamWriter.Write(testData.Correctness.ToString() + "\t");
                rawStreamWriter.Write(correct.ToString() + "\t");
                rawStreamWriter.Write(incorrectAct.ToString() + "\t");
                rawStreamWriter.Write(incorrectInact.ToString() + "\t");
                rawStreamWriter.Write(testData.MeanActiveUnitActivation + "\t");
                rawStreamWriter.Write(testData.MeanInactiveUnitActivation + "\t");
                rawStreamWriter.Write(unitData);
                rawStreamWriter.WriteLine();
            }

            rawStreamWriter.Flush();
            rawStreamWriter.Close();
        }
        private void SummarySave(string resultPath)
        {
            StreamWriter seStreamWriter = new StreamWriter(resultPath + "SimulationResult-SquaredError.txt");
            StreamWriter ssStreamWriter = new StreamWriter(resultPath + "SimulationResult-SemanticStress.txt");
            StreamWriter ceStreamWriter = new StreamWriter(resultPath + "SimulationResult-CrossEntropy.txt");
            StreamWriter accStreamWriter = new StreamWriter(resultPath + "SimulationResult-Accuracy.txt");
            StreamWriter actUnitStreamWriter = new StreamWriter(resultPath + "SimulationResult-ActivateUnit.txt");
            StreamWriter inactUnitStreamWriter = new StreamWriter(resultPath + "SimulationResult-InactivateUnit.txt");


            seStreamWriter.WriteLine("ProcessName\tStimuliPackName\tName\tTimeStamp\tTransition");
            ssStreamWriter.WriteLine("ProcessName\tStimuliPackName\tName\tTimeStamp\tTransition");
            ceStreamWriter.WriteLine("ProcessName\tStimuliPackName\tName\tTimeStamp\tTransition");
            accStreamWriter.WriteLine("ProcessName\tStimuliPackName\tName\tTimeStamp\tTransition");
            actUnitStreamWriter.WriteLine("ProcessName\tStimuliPackName\tName\tTimeStamp\tTransition");
            inactUnitStreamWriter.WriteLine("ProcessName\tStimuliPackName\tName\tTimeStamp\tTransition");

            List<TestDataList> testDataListList = new List<TestDataList>();
            foreach (TestData testData in TestDataList)
            {
                bool isExist = false;
                foreach (TestDataList testDataList in testDataListList)
                {
                    if (testData.ProcessName == testDataList.ProcessName && testData.StimuliPackName == testDataList.StimuliPackName && testData.Name == testDataList.Name && testData.TimeStamp == testDataList.TimeStamp)
                    {
                        testDataList.Add(testData);
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    TestDataList newTestDataList = new TestDataList();
                    newTestDataList.ProcessName = testData.ProcessName;
                    newTestDataList.StimuliPackName = testData.StimuliPackName;
                    newTestDataList.Name = testData.Name;
                    newTestDataList.TimeStamp = testData.TimeStamp;
                    newTestDataList.Add(testData);
                    testDataListList.Add(newTestDataList);
                }
            }

            foreach (TestDataList testDataList in testDataListList) testDataList.Sort((x, y) => x.Epoch.CompareTo(y.Epoch));


            foreach (TestDataList testDataList in testDataListList)
            {
                seStreamWriter.Write(testDataList.ProcessName + "\t");
                ssStreamWriter.Write(testDataList.ProcessName + "\t");
                ceStreamWriter.Write(testDataList.ProcessName + "\t");
                accStreamWriter.Write(testDataList.ProcessName + "\t");
                actUnitStreamWriter.Write(testDataList.ProcessName + "\t");
                inactUnitStreamWriter.Write(testDataList.ProcessName + "\t");

                seStreamWriter.Write(testDataList.StimuliPackName + "\t");
                ssStreamWriter.Write(testDataList.StimuliPackName + "\t");
                ceStreamWriter.Write(testDataList.StimuliPackName + "\t");
                accStreamWriter.Write(testDataList.StimuliPackName + "\t");
                actUnitStreamWriter.Write(testDataList.StimuliPackName + "\t");
                inactUnitStreamWriter.Write(testDataList.StimuliPackName + "\t");

                seStreamWriter.Write(testDataList.Name + "\t");
                ssStreamWriter.Write(testDataList.Name + "\t");
                ceStreamWriter.Write(testDataList.Name + "\t");
                accStreamWriter.Write(testDataList.Name + "\t");
                actUnitStreamWriter.Write(testDataList.Name + "\t");
                inactUnitStreamWriter.Write(testDataList.Name + "\t");

                seStreamWriter.Write(testDataList.TimeStamp + "\t");
                ssStreamWriter.Write(testDataList.TimeStamp + "\t");
                ceStreamWriter.Write(testDataList.TimeStamp + "\t");
                accStreamWriter.Write(testDataList.TimeStamp + "\t");
                actUnitStreamWriter.Write(testDataList.TimeStamp + "\t");
                inactUnitStreamWriter.Write(testDataList.TimeStamp + "\t");

                foreach (TestData testData in testDataList)
                {
                    seStreamWriter.Write(testData.MeanSquredError + "\t");
                    ssStreamWriter.Write(testData.MeanSemanticStress + "\t");
                    ceStreamWriter.Write(testData.MeanCrossEntropy + "\t");
                    if (testData.Correctness) accStreamWriter.Write("1" + "\t");
                    else accStreamWriter.Write("0" + "\t");
                    actUnitStreamWriter.Write(testData.MeanActiveUnitActivation + "\t");
                    inactUnitStreamWriter.Write(testData.MeanInactiveUnitActivation + "\t");
                }

                seStreamWriter.WriteLine();
                ssStreamWriter.WriteLine();
                ceStreamWriter.WriteLine();
                accStreamWriter.WriteLine();
                actUnitStreamWriter.WriteLine();
                inactUnitStreamWriter.WriteLine();
            }

            seStreamWriter.Flush();
            seStreamWriter.Close();

            ssStreamWriter.Flush();
            ssStreamWriter.Close();

            ceStreamWriter.Flush();
            ceStreamWriter.Close();

            accStreamWriter.Flush();
            accStreamWriter.Close();

            actUnitStreamWriter.Flush();
            actUnitStreamWriter.Close();

            inactUnitStreamWriter.Flush();
            inactUnitStreamWriter.Close();
        }
        public void WeightSave(string fileName)
        {
            StreamWriter streamWriter = new StreamWriter(fileName);

            //Connection-WeightMatrix

            foreach (string key in ConnectionDictionary.Keys)
            {
                streamWriter.WriteLine("!Connection");
                streamWriter.WriteLine("@" + key);
                streamWriter.WriteLine("#" + ConnectionDictionary[key].SendLayer.UnitCount);
                streamWriter.WriteLine("$" + ConnectionDictionary[key].ReceiveLayer.UnitCount);
                for (int sendIndex =0; sendIndex< ConnectionDictionary[key].SendLayer.UnitCount;sendIndex++)
                {
                    StringBuilder newLine = new StringBuilder();
                    for (int receiveIndex = 0; receiveIndex < ConnectionDictionary[key].ReceiveLayer.UnitCount; receiveIndex++)
                    {
                        newLine.Append(ConnectionDictionary[key].WeightMatrix[sendIndex, receiveIndex]);
                        newLine.Append("\t");
                    }
                    streamWriter.WriteLine(newLine.Remove(newLine.Length - 1, 1).ToString()); //Tap이 들어가는지 체크
                }
                streamWriter.WriteLine("%");
                streamWriter.WriteLine();
            }

            foreach (string key in LayerDictionary.Keys)
            {
                streamWriter.WriteLine("!Layer_Bias");
                streamWriter.WriteLine("@" + key);
                streamWriter.WriteLine("#" + 1);
                streamWriter.WriteLine("$" + LayerDictionary[key].UnitCount);
                StringBuilder newLine = new StringBuilder();
                for (int unitIndex = 0; unitIndex < LayerDictionary[key].UnitCount; unitIndex++)
                {   
                    newLine.Append(LayerDictionary[key].BiasMatrix[0, unitIndex]);
                    newLine.Append("\t");
                }
                streamWriter.WriteLine(newLine.Remove(newLine.Length - 1, 1).ToString());
                streamWriter.WriteLine("%");
                streamWriter.WriteLine();
            }

            foreach (string key in LayerDictionary.Keys)
            {
                if (LayerDictionary[key].CleanupUnitCount == 0) continue;
                streamWriter.WriteLine("!Layer_LayerToCleanup");
                streamWriter.WriteLine("@" + key);
                streamWriter.WriteLine("#" + LayerDictionary[key].UnitCount);
                streamWriter.WriteLine("$" + LayerDictionary[key].CleanupUnitCount);                
                for (int sendIndex = 0; sendIndex < LayerDictionary[key].UnitCount; sendIndex++)
                {
                    StringBuilder newLine = new StringBuilder();
                    for (int receiveIndex = 0; receiveIndex < LayerDictionary[key].CleanupUnitCount; receiveIndex++)
                    {
                        newLine.Append(LayerDictionary[key].LayerToCleanupWeightMatrix[sendIndex, receiveIndex]);
                        newLine.Append("\t");
                    }
                    streamWriter.WriteLine(newLine.Remove(newLine.Length - 1, 1).ToString());
                }
                streamWriter.WriteLine("%");
                streamWriter.WriteLine();
            }

            foreach (string key in LayerDictionary.Keys)
            {
                if (LayerDictionary[key].CleanupUnitCount == 0) continue;
                streamWriter.WriteLine("!Layer_CleanupToLayer");
                streamWriter.WriteLine("@" + key);
                streamWriter.WriteLine("#" + LayerDictionary[key].CleanupUnitCount);
                streamWriter.WriteLine("$" + LayerDictionary[key].UnitCount);
                for (int sendIndex = 0; sendIndex < LayerDictionary[key].CleanupUnitCount; sendIndex++)
                {
                    StringBuilder newLine = new StringBuilder();
                    for (int receiveIndex = 0; receiveIndex < LayerDictionary[key].UnitCount; receiveIndex++)
                    {
                        newLine.Append(LayerDictionary[key].CleanupToLayerWeightMatrix[sendIndex, receiveIndex]);
                        newLine.Append("\t");
                    }
                    streamWriter.WriteLine(newLine.Remove(newLine.Length - 1, 1).ToString());
                }
                streamWriter.WriteLine("%");
                streamWriter.WriteLine();
            }

            foreach (string key in LayerDictionary.Keys)
            {
                if (LayerDictionary[key].CleanupUnitCount == 0) continue;
                streamWriter.WriteLine("!Layer_CleanupBias");
                streamWriter.WriteLine("@" + key);
                streamWriter.WriteLine("#" + 1);
                streamWriter.WriteLine("$" + LayerDictionary[key].CleanupUnitCount);
                StringBuilder newLine = new StringBuilder();
                for (int unitIndex = 0; unitIndex < LayerDictionary[key].CleanupUnitCount; unitIndex++)
                {
                    newLine.Append(LayerDictionary[key].CleanupBiasMatrix[0, unitIndex]);
                    newLine.Append("\t");
                }
                streamWriter.WriteLine(newLine.Remove(newLine.Length - 1, 1).ToString());
                streamWriter.WriteLine("%");
                streamWriter.WriteLine();
            }

            foreach (string key in LayerDictionary.Keys)
            {
                streamWriter.WriteLine("!Layer_Interconnection");
                streamWriter.WriteLine("@" + key);
                streamWriter.WriteLine("#" + LayerDictionary[key].UnitCount);
                streamWriter.WriteLine("$" + LayerDictionary[key].UnitCount);
                for (int sendIndex = 0; sendIndex < LayerDictionary[key].UnitCount; sendIndex++)
                {
                    StringBuilder newLine = new StringBuilder();
                    for (int receiveIndex = 0; receiveIndex < LayerDictionary[key].UnitCount; receiveIndex++)
                    {
                        newLine.Append(LayerDictionary[key].InterConnectionMatrix[sendIndex, receiveIndex]);
                        newLine.Append("\t");
                    }
                    streamWriter.WriteLine(newLine.Remove(newLine.Length - 1, 1).ToString());
                }
                streamWriter.WriteLine("%");
                streamWriter.WriteLine();
            }

            foreach (string key in LayerDictionary.Keys)
            {
                if (LayerDictionary[key].LayerType != LayerType.BPTTLayer) continue;
                streamWriter.WriteLine("!BPTTLayer_BaseHideConnection");
                streamWriter.WriteLine("@" + key);
                streamWriter.WriteLine("#" + LayerDictionary[key].UnitCount);
                streamWriter.WriteLine("$" + LayerDictionary[key].UnitCount);                
                for (int sendIndex = 0; sendIndex < LayerDictionary[key].UnitCount; sendIndex++)
                {
                    StringBuilder newLine = new StringBuilder();
                    for (int receiveIndex = 0; receiveIndex < LayerDictionary[key].UnitCount; receiveIndex++)
                    {
                        newLine.Append(((BPTTLayer)LayerDictionary[key]).BaseHideWeightMatrix[sendIndex, receiveIndex]);
                        newLine.Append("\t");
                    }
                    streamWriter.WriteLine(newLine.Remove(newLine.Length - 1, 1).ToString());
                }
                streamWriter.WriteLine("%");
                streamWriter.WriteLine();
            }

            foreach (string key in LayerDictionary.Keys)
            {
                if (LayerDictionary[key].LayerType != LayerType.BPTTLayer) continue;
                streamWriter.WriteLine("!BPTTLayer_BaseHideBias");
                streamWriter.WriteLine("@" + key);
                streamWriter.WriteLine("#" + 1);
                streamWriter.WriteLine("$" + LayerDictionary[key].UnitCount);
                StringBuilder newLine = new StringBuilder();
                for (int unitIndex = 0; unitIndex < LayerDictionary[key].UnitCount; unitIndex++)
                {
                    newLine.Append(((BPTTLayer)LayerDictionary[key]).BaseHideBiasMatrix[0, unitIndex]);
                    newLine.Append("\t");
                }
                streamWriter.WriteLine(newLine.Remove(newLine.Length - 1, 1).ToString());
                streamWriter.WriteLine("%");
                streamWriter.WriteLine();
            }
            
            streamWriter.Flush();
            streamWriter.Close();
        }
        public void WeightLoad(string fileName)
        {
            StreamReader streamReader = new StreamReader(fileName);

            while(!streamReader.EndOfStream)
            {
                string typeCheck = streamReader.ReadLine();
                if (typeCheck.Trim() == "") continue;
                else if (typeCheck[0] != '!') continue;

                string type = typeCheck.Substring(1);
                string name = streamReader.ReadLine().Substring(1);
                int sendUnit = int.Parse(streamReader.ReadLine().Substring(1));
                int receiveUnit = int.Parse(streamReader.ReadLine().Substring(1));

                switch (type)
                {
                    case "Connection":                        
                        ConnectionDictionary[name].WeightMatrix = DenseMatrix.Create(sendUnit, receiveUnit,0);
                        for (int sendIndex = 0; sendIndex < sendUnit; sendIndex++)
                        {
                            string[] readLine = streamReader.ReadLine().Split('\t');
                            for (int receiveIndex = 0; receiveIndex < receiveUnit; receiveIndex++)
                            {
                                ConnectionDictionary[name].WeightMatrix[sendIndex, receiveIndex] = double.Parse(readLine[receiveIndex]);
                            }
                        }
                        break;
                    case "Layer_Bias":
                        LayerDictionary[name].BiasMatrix = DenseMatrix.Create(sendUnit, receiveUnit,0);
                        for (int sendIndex = 0; sendIndex < sendUnit; sendIndex++)
                        {
                            string[] readLine = streamReader.ReadLine().Split('\t');
                            for (int receiveIndex = 0; receiveIndex < receiveUnit; receiveIndex++)
                            {
                                LayerDictionary[name].BiasMatrix[sendIndex, receiveIndex] = double.Parse(readLine[receiveIndex]);
                            }
                        }
                        break;
                    case "Layer_LayerToCleanup":
                        LayerDictionary[name].LayerToCleanupWeightMatrix = DenseMatrix.Create(sendUnit, receiveUnit,0);
                        for (int sendIndex = 0; sendIndex < sendUnit; sendIndex++)
                        {
                            string[] readLine = streamReader.ReadLine().Split('\t');
                            for (int receiveIndex = 0; receiveIndex < receiveUnit; receiveIndex++)
                            {
                                LayerDictionary[name].LayerToCleanupWeightMatrix[sendIndex, receiveIndex] = double.Parse(readLine[receiveIndex]);
                            }
                        }
                        break;
                    case "Layer_CleanupToLayer":
                        LayerDictionary[name].CleanupToLayerWeightMatrix = DenseMatrix.Create(sendUnit, receiveUnit,0);
                        for (int sendIndex = 0; sendIndex < sendUnit; sendIndex++)
                        {
                            string[] readLine = streamReader.ReadLine().Split('\t');
                            for (int receiveIndex = 0; receiveIndex < receiveUnit; receiveIndex++)
                            {
                                LayerDictionary[name].CleanupToLayerWeightMatrix[sendIndex, receiveIndex] = double.Parse(readLine[receiveIndex]);
                            }
                        }
                        break;
                    case "Layer_CleanupBias":
                        LayerDictionary[name].CleanupBiasMatrix = DenseMatrix.Create(sendUnit, receiveUnit,0);
                        for (int sendIndex = 0; sendIndex < sendUnit; sendIndex++)
                        {
                            string[] readLine = streamReader.ReadLine().Split('\t');
                            for (int receiveIndex = 0; receiveIndex < receiveUnit; receiveIndex++)
                            {
                                LayerDictionary[name].CleanupBiasMatrix[sendIndex, receiveIndex] = double.Parse(readLine[receiveIndex]);
                            }
                        }
                        break;
                    case "Layer_Interconnection":
                        LayerDictionary[name].InterConnectionMatrix = DenseMatrix.Create(sendUnit, receiveUnit,0);
                        for (int sendIndex = 0; sendIndex < sendUnit; sendIndex++)
                        {
                            string[] readLine = streamReader.ReadLine().Split('\t');
                            for (int receiveIndex = 0; receiveIndex < receiveUnit; receiveIndex++)
                            {
                                LayerDictionary[name].InterConnectionMatrix[sendIndex, receiveIndex] = double.Parse(readLine[receiveIndex]);
                            }
                        }
                        break;
                    case "BPTTLayer_BaseHideConnection":
                        ((BPTTLayer)LayerDictionary[name]).BaseHideWeightMatrix = DenseMatrix.Create(sendUnit, receiveUnit,0);
                        for (int sendIndex = 0; sendIndex < sendUnit; sendIndex++)
                        {
                            string[] readLine = streamReader.ReadLine().Split('\t');
                            for (int receiveIndex = 0; receiveIndex < receiveUnit; receiveIndex++)
                            {
                                ((BPTTLayer)LayerDictionary[name]).BaseHideWeightMatrix[sendIndex, receiveIndex] = double.Parse(readLine[receiveIndex]);
                            }
                        }
                        break;
                    case "BPTTLayer_BaseHideBias":
                        ((BPTTLayer)LayerDictionary[name]).BaseHideBiasMatrix = DenseMatrix.Create(sendUnit, receiveUnit, 0);
                        for (int sendIndex = 0; sendIndex < sendUnit; sendIndex++)
                        {
                            string[] readLine = streamReader.ReadLine().Split('\t');
                            for (int receiveIndex = 0; receiveIndex < receiveUnit; receiveIndex++)
                            {
                                ((BPTTLayer)LayerDictionary[name]).BaseHideBiasMatrix[sendIndex, receiveIndex] = double.Parse(readLine[receiveIndex]);
                            }
                        }
                        break;
                }
            }

            streamReader.Close();
        }
                
        public Dictionary<string, Matrix<double>> ActivationInforamtionReader(int index)
        {   
            Dictionary<string, Matrix<double>> currentActivationData = new Dictionary<string, Matrix<double>>();
            
            foreach (string key in LayerDictionary.Keys)
            {
                currentActivationData.Add("LayerActvation_" + key, LayerDictionary[key].LayerActivationMatrix.Row(index).ToRowMatrix());
                if (LayerDictionary[key].CleanupUnitCount > 0)
                {
                    currentActivationData.Add("CleanupActivation_" + key, LayerDictionary[key].CleanupLayerActivationMatrix.Row(index).ToRowMatrix());
                }
                if (LayerDictionary[key].LayerType == LayerType.BPTTLayer)
                {
                    for (int layerIndex = 0; layerIndex < ((BPTTLayer)LayerDictionary[key]).HideLayerList.Count; layerIndex++)
                    {
                        currentActivationData.Add("BPTT_HideLayer_" + key + "_" + layerIndex, ((BPTTLayer)LayerDictionary[key]).HideLayerList[layerIndex].LayerActivationMatrix.Row(index).ToRowMatrix());
                    }
                }
            }

            return currentActivationData;
        }
        public void ActivationInformationWriter(string fileName)
        {
            StreamWriter streamWriter = new StreamWriter(fileName);

            StringBuilder newLine = new StringBuilder();
            newLine.Append("Name\tEpoch\tTimeStamp\t");

            foreach (string key in LayerDictionary.Keys)
            {
                for(int index=0;index< LayerDictionary[key].UnitCount; index++)
                {
                    newLine.Append("LayerActvation_" + key + "[" + index + "]");
                    newLine.Append("\t");
                }

                if (LayerDictionary[key].CleanupUnitCount > 0)
                {
                    for (int index = 0; index < LayerDictionary[key].CleanupUnitCount; index++)
                    {
                        newLine.Append("CleanupActivation_" + key + "[" + index + "]");
                        newLine.Append("\t");
                    }
                }

                if (LayerDictionary[key].LayerType == LayerType.BPTTLayer)
                {
                    for (int layerIndex = 0; layerIndex < ((BPTTLayer)LayerDictionary[key]).HideLayerList.Count; layerIndex++)
                    {
                        for (int index = 0; index < LayerDictionary[key].UnitCount; index++)
                        {
                            newLine.Append("BPTT_HideLayer_" + key + "_" + layerIndex + "[" + index + "]");
                            newLine.Append("\t");
                        }
                    }
                }
            }
            newLine.Length--;
            streamWriter.WriteLine(newLine);

            foreach(TestData testData in TestDataList)
            {
                newLine.Clear();
                newLine.Append(testData.Name + "\t");
                newLine.Append(testData.Epoch + "\t");
                newLine.Append(testData.TimeStamp + "\t");

                foreach (string key in LayerDictionary.Keys)
                {
                    for (int index = 0; index < LayerDictionary[key].UnitCount; index++)
                    {
                        newLine.Append(testData.LayerActivationInforamtion["LayerActvation_" + key][0, index]);
                        newLine.Append("\t");
                    }

                    if (LayerDictionary[key].CleanupUnitCount > 0)
                    {
                        for (int index = 0; index < LayerDictionary[key].CleanupUnitCount; index++)
                        {
                            newLine.Append(testData.LayerActivationInforamtion["CleanupActivation_" + key][0, index]);
                            newLine.Append("\t");
                        }
                    }

                    if (LayerDictionary[key].LayerType == LayerType.BPTTLayer)
                    {
                        for (int layerIndex = 0; layerIndex < ((BPTTLayer)LayerDictionary[key]).HideLayerList.Count; layerIndex++)
                        {
                            for (int index = 0; index < LayerDictionary[key].UnitCount; index++)
                            {
                                newLine.Append(testData.LayerActivationInforamtion["BPTT_HideLayer_" + key + "_" + layerIndex][0, index]);
                                newLine.Append("\t");
                            }
                        }
                    }
                }
                newLine.Length--;
                streamWriter.WriteLine(newLine);
            }

            streamWriter.Flush();
            streamWriter.Close();
        }
        public void WeightInformationReader(int epoch)
        {
            foreach (string key in LayerDictionary.Keys) LayerDictionary[key].Initialize(LayerState.On, 0, 1);
            foreach (string key in ConnectionDictionary.Keys) ConnectionDictionary[key].Initialize(ConnectionState.On, 0);

            Dictionary<string, Matrix<double>> currentWeightData = new Dictionary<string, Matrix<double>>();
                
            foreach (string key in ConnectionDictionary.Keys)
            {
                currentWeightData.Add("Connection_" + key, ConnectionDictionary[key].WeightMatrix);
            }

            foreach (string key in LayerDictionary.Keys)
            {
                currentWeightData.Add("Layer_Bias_" + key, LayerDictionary[key].BiasMatrix);
                if (LayerDictionary[key].CleanupUnitCount > 0)
                {
                    currentWeightData.Add("Layer_LayerToCleanup_" + key, LayerDictionary[key].LayerToCleanupWeightMatrix);
                    currentWeightData.Add("Layer_CleanupToLayer_" + key, LayerDictionary[key].CleanupToLayerWeightMatrix);
                    currentWeightData.Add("Layer_CleanupBias_" + key, LayerDictionary[key].CleanupBiasMatrix);
                }
                currentWeightData.Add("Layer_Interconnection_" + key, LayerDictionary[key].InterConnectionMatrix);
                if (LayerDictionary[key].LayerType == LayerType.BPTTLayer)
                {
                    currentWeightData.Add("BPTTLayer_BaseHideConnection_" + key, ((BPTTLayer)LayerDictionary[key]).BaseHideWeightMatrix);
                    currentWeightData.Add("BPTTLayer_BaseHideBias_" + key, ((BPTTLayer)LayerDictionary[key]).BaseHideBiasMatrix);
                }
            }

            WeightDataDictionary.Add(epoch, currentWeightData);
        }
        public void WeightInformationWriter(string fileName)
        {
            StreamWriter streamWriter = new StreamWriter(fileName);

            StringBuilder newLine = new StringBuilder();
            newLine.Append("Epoch\t");

            foreach (string key in ConnectionDictionary.Keys)
            {
                for (int fromIndex = 0; fromIndex < ConnectionDictionary[key].SendLayer.UnitCount; fromIndex++)
                {
                    for (int toIndex = 0; toIndex < ConnectionDictionary[key].ReceiveLayer.UnitCount; toIndex++)
                    {
                        newLine.Append("Connection_" + key + "[" + fromIndex + ", " + toIndex + "]");
                        newLine.Append("\t");
                    }
                }
            }

            foreach (string key in LayerDictionary.Keys)
            {
                for (int index = 0; index < LayerDictionary[key].UnitCount; index++)
                {
                    newLine.Append("Layer_Bias_" + key + "[" + index + "]");
                    newLine.Append("\t");
                }

                if (LayerDictionary[key].CleanupUnitCount > 0)
                {
                    for (int fromIndex = 0; fromIndex < LayerDictionary[key].UnitCount; fromIndex++)
                    {
                        for (int toIndex = 0; toIndex < LayerDictionary[key].CleanupUnitCount; toIndex++)
                        {
                            newLine.Append("Layer_LayerToCleanup_" + key + "[" + fromIndex + ", " + toIndex + "]");
                            newLine.Append("\t");
                        }
                    }

                    for (int fromIndex = 0; fromIndex < LayerDictionary[key].CleanupUnitCount; fromIndex++)
                    {
                        for (int toIndex = 0; toIndex < LayerDictionary[key].UnitCount; toIndex++)
                        {
                            newLine.Append("Layer_CleanupToLayer_" + key + "[" + fromIndex + ", " + toIndex + "]");
                            newLine.Append("\t");
                        }
                    }

                    for (int index = 0; index < LayerDictionary[key].CleanupUnitCount; index++)
                    {
                        newLine.Append("Layer_CleanupBias_" + key + "[" + index + "]");
                        newLine.Append("\t");
                    }
                }

                for (int fromIndex = 0; fromIndex < LayerDictionary[key].UnitCount; fromIndex++)
                {
                    for (int toIndex = 0; toIndex < LayerDictionary[key].UnitCount; toIndex++)
                    {
                        newLine.Append("Layer_Interconnection_" + key + "[" + fromIndex + ", " + toIndex + "]");
                        newLine.Append("\t");
                    }
                }

                if (LayerDictionary[key].LayerType == LayerType.BPTTLayer)
                {
                    for (int fromIndex = 0; fromIndex < LayerDictionary[key].UnitCount; fromIndex++)
                    {
                        for (int toIndex = 0; toIndex < LayerDictionary[key].UnitCount; toIndex++)
                        {
                            newLine.Append("BPTTLayer_BaseHideConnection_" + key + "[" + fromIndex + ", " + toIndex + "]");
                            newLine.Append("\t");
                        }
                    }

                    for (int Index = 0; Index < LayerDictionary[key].UnitCount; Index++)
                    {
                        newLine.Append("BPTTLayer_BaseHideBias_" + key + "[" + Index + "]");
                        newLine.Append("\t");
                    }
                }
            }
            newLine.Length--;
            streamWriter.WriteLine(newLine);

            foreach (int epochKey in WeightDataDictionary.Keys)
            {
                newLine.Clear();
                newLine.Append(epochKey + "\t");

                foreach (string key in ConnectionDictionary.Keys)
                {
                    for (int fromIndex = 0; fromIndex < ConnectionDictionary[key].SendLayer.UnitCount; fromIndex++)
                    {
                        for (int toIndex = 0; toIndex < ConnectionDictionary[key].ReceiveLayer.UnitCount; toIndex++)
                        {
                            newLine.Append(WeightDataDictionary[epochKey]["Connection_" + key][fromIndex, toIndex]);
                            newLine.Append("\t");
                        }
                    }
                }

                foreach (string key in LayerDictionary.Keys)
                {
                    for (int index = 0; index < LayerDictionary[key].UnitCount; index++)
                    {
                        newLine.Append(WeightDataDictionary[epochKey]["Layer_Bias_" + key][0, index]);
                        newLine.Append("\t");
                    }

                    if (LayerDictionary[key].CleanupUnitCount > 0)
                    {
                        for (int fromIndex = 0; fromIndex < LayerDictionary[key].UnitCount; fromIndex++)
                        {
                            for (int toIndex = 0; toIndex < LayerDictionary[key].CleanupUnitCount; toIndex++)
                            {
                                newLine.Append(WeightDataDictionary[epochKey]["Layer_LayerToCleanup_" + key][fromIndex, toIndex]);
                                newLine.Append("\t");
                            }
                        }

                        for (int fromIndex = 0; fromIndex < LayerDictionary[key].CleanupUnitCount; fromIndex++)
                        {
                            for (int toIndex = 0; toIndex < LayerDictionary[key].UnitCount; toIndex++)
                            {
                                newLine.Append(WeightDataDictionary[epochKey]["Layer_CleanupToLayer_" + key][fromIndex, toIndex]);
                                newLine.Append("\t");
                            }
                        }

                        for (int index = 0; index < LayerDictionary[key].CleanupUnitCount; index++)
                        {
                            newLine.Append(WeightDataDictionary[epochKey]["Layer_CleanupBias_" + key][0, index]);
                            newLine.Append("\t");
                        }
                    }

                    for (int fromIndex = 0; fromIndex < LayerDictionary[key].UnitCount; fromIndex++)
                    {
                        for (int toIndex = 0; toIndex < LayerDictionary[key].UnitCount; toIndex++)
                        {
                            newLine.Append(WeightDataDictionary[epochKey]["Layer_Interconnection_" + key][fromIndex, toIndex]);
                            newLine.Append("\t");
                        }
                    }

                    if (LayerDictionary[key].LayerType == LayerType.BPTTLayer)
                    {
                        for (int fromIndex = 0; fromIndex < LayerDictionary[key].UnitCount; fromIndex++)
                        {
                            for (int toIndex = 0; toIndex < LayerDictionary[key].UnitCount; toIndex++)
                            {
                                newLine.Append(WeightDataDictionary[epochKey]["BPTTLayer_BaseHideConnection_" + key][fromIndex, toIndex]);
                                newLine.Append("\t");
                            }
                        }

                        for (int index = 0; index < LayerDictionary[key].UnitCount; index++)
                        {
                            newLine.Append(WeightDataDictionary[epochKey]["BPTTLayer_BaseHideBias_" + key][0, index]);
                            newLine.Append("\t");
                        }
                    }
                }
                newLine.Length--;
                streamWriter.WriteLine(newLine);
            }

            streamWriter.Flush();
            streamWriter.Close();
        }

        public double Momentum
        {
            get;
            set;
        }
        public double ActivationCriterion
        {
            get;
            set;
        }
        public double InactivationCriterion
        {
            get;
            set;
        }
        public double DecayRate
        {
            get;
            set;
        }

        private double weightRange;
        public double WeightRange
        {
            get
            {
                return weightRange;
            }
            set
            {
                this.weightRange = value;
                foreach (string key in ConnectionDictionary.Keys) ConnectionDictionary[key].InitialWeightSetting(value);
                foreach (string key in LayerDictionary.Keys) LayerDictionary[key].InitialWeightSetting(value);
            }
        }

        public double LearningRate
        {
            get;
            set;
        }
        
        public bool IsSet
        {
            get;
            set;
        }

        public Dictionary<string, Layer> LayerDictionary
        {
            get;
            private set;
        }        
        public Dictionary<string, Connection> ConnectionDictionary
        {
            get;
            private set;
        }

        public Dictionary<string, StimuliPack> StimuliPackDictionary
        {
            get;
            private set;
        }
        public Dictionary<string, Process> ProcessDictionary
        {
            get;
            private set;
        }
        public List<TestData> TestDataList
        {
            get;
            private set;
        }
        public Dictionary<int, Dictionary<string, Matrix<double>>> WeightDataDictionary
        {
            get;
            set;
        }

        public bool UseActivationInformation
        {
            get;
            set;
        }
        public bool UseWeightInformation
        {
            get;
            set;
        }


        public void Architecture_Save(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            XmlNode rootNode = xmlDocument.CreateElement("", "Root", "");
            xmlDocument.AppendChild(rootNode);

            XmlElement configElement = xmlDocument.CreateElement("Config");
            configElement.SetAttribute("Momentum", Momentum.ToString());
            configElement.SetAttribute("ActivationCriterion", ActivationCriterion.ToString());
            configElement.SetAttribute("InactivationCriterion", InactivationCriterion.ToString());
            configElement.SetAttribute("DecayRate", DecayRate.ToString());
            configElement.SetAttribute("WeightRange", WeightRange.ToString());
            configElement.SetAttribute("LearningRate", LearningRate.ToString());
            rootNode.AppendChild(configElement);

            XmlNode layerListNode = xmlDocument.CreateElement("LayerList");
            foreach (string key in LayerDictionary.Keys)
            {
                XmlElement layerElement = xmlDocument.CreateElement("Layer");
                layerElement.SetAttribute("Name", LayerDictionary[key].Name);
                layerElement.SetAttribute("UnitCount", LayerDictionary[key].UnitCount.ToString());
                layerElement.SetAttribute("CleanUpUnitCount", LayerDictionary[key].CleanupUnitCount.ToString());
                if (LayerDictionary[key].LayerType == LayerType.NormalLayer) layerElement.SetAttribute("BPTTUse", "0");
                if (LayerDictionary[key].LayerType == LayerType.BPTTLayer)
                {
                    layerElement.SetAttribute("Tick", ((BPTTLayer)LayerDictionary[key]).Tick.ToString());
                    layerElement.SetAttribute("BPTTUse", "1");
                }
                layerListNode.AppendChild(layerElement);
            }
            rootNode.AppendChild(layerListNode);

            XmlNode connectionListNode = xmlDocument.CreateElement("ConnectionList");
            foreach (string key in ConnectionDictionary.Keys)
            {
                XmlElement connectionElement = xmlDocument.CreateElement("Connection");
                connectionElement.SetAttribute("Name", ConnectionDictionary[key].Name);
                connectionElement.SetAttribute("SendLayerName", ConnectionDictionary[key].SendLayer.Name);
                connectionElement.SetAttribute("ReceiveLayerName", ConnectionDictionary[key].ReceiveLayer.Name);
                connectionListNode.AppendChild(connectionElement);
            }
            rootNode.AppendChild(connectionListNode);

            xmlDocument.Save(fileName);
        }
        public void Architecture_Load(string fileName)
        {
            LayerDictionary.Clear();
            ConnectionDictionary.Clear();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlNode config = xmlDocument.SelectSingleNode("/Root/Config");
            Momentum = double.Parse(config.Attributes["Momentum"].Value);
            ActivationCriterion = double.Parse(config.Attributes["ActivationCriterion"].Value);
            InactivationCriterion = double.Parse(config.Attributes["InactivationCriterion"].Value);
            DecayRate = double.Parse(config.Attributes["DecayRate"].Value);
            WeightRange = double.Parse(config.Attributes["WeightRange"].Value);
            LearningRate = double.Parse(config.Attributes["LearningRate"].Value);

            XmlNodeList layerNodeList = xmlDocument.SelectNodes("/Root/LayerList/Layer");
            foreach (XmlNode layerNode in layerNodeList)
            {
                if (layerNode.Attributes["BPTTUse"].Value == "0") LayerMaking(layerNode.Attributes["Name"].Value, int.Parse(layerNode.Attributes["UnitCount"].Value), int.Parse(layerNode.Attributes["CleanUpUnitCount"].Value));
                else if (layerNode.Attributes["BPTTUse"].Value == "1") LayerMaking(layerNode.Attributes["Name"].Value, int.Parse(layerNode.Attributes["UnitCount"].Value), int.Parse(layerNode.Attributes["CleanUpUnitCount"].Value), int.Parse(layerNode.Attributes["Tick"].Value));
            }

            XmlNodeList connectionNodeList = xmlDocument.SelectNodes("/Root/ConnectionList/Connection");
            foreach (XmlNode connectionNode in connectionNodeList)
            {
                ConnectionMaking(connectionNode.Attributes["Name"].Value, connectionNode.Attributes["SendLayerName"].Value, connectionNode.Attributes["ReceiveLayerName"].Value);
            }

            IsSet = true;
        }

        public void Process_Save(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            XmlNode rootNode = xmlDocument.CreateElement("", "Root", "");
            xmlDocument.AppendChild(rootNode);

            XmlNode processListNode = xmlDocument.CreateElement("ProcessList");
            foreach (string key in ProcessDictionary.Keys)
            {
                XmlElement processElement = xmlDocument.CreateElement("Process");
                processElement.SetAttribute("Name", ProcessDictionary[key].Name);

                XmlElement layerStateDictionaryElement = xmlDocument.CreateElement("LayerStateDictionary");
                foreach (string layerName in ProcessDictionary[key].LayerStateDictionary.Keys)
                {
                    XmlElement layerStateElement = xmlDocument.CreateElement("LayerState");
                    layerStateElement.SetAttribute("Name", layerName);
                    layerStateElement.SetAttribute("State", ProcessDictionary[key].LayerStateDictionary[layerName].ToString());
                    layerStateElement.SetAttribute("SD", ProcessDictionary[key].LayerDamagedSDDictionary[layerName].ToString());
                    layerStateDictionaryElement.AppendChild(layerStateElement);
                }
                processElement.AppendChild(layerStateDictionaryElement);

                XmlElement connectionStateDictionaryElement = xmlDocument.CreateElement("ConnectionStateDictionary");
                foreach (string connectionName in ProcessDictionary[key].ConnectionStateDictionary.Keys)
                {
                    XmlElement connectionStateElement = xmlDocument.CreateElement("ConnectionState");
                    connectionStateElement.SetAttribute("Name", connectionName);
                    connectionStateElement.SetAttribute("State", ProcessDictionary[key].ConnectionStateDictionary[connectionName].ToString());
                    connectionStateElement.SetAttribute("SD", ProcessDictionary[key].ConnectionDamagedSDDictionary[connectionName].ToString());
                    connectionStateDictionaryElement.AppendChild(connectionStateElement);
                }
                processElement.AppendChild(connectionStateDictionaryElement);

                XmlElement orderListElement = xmlDocument.CreateElement("OrderList");
                foreach (Order order in ProcessDictionary[key])
                {
                    XmlElement orderElement = xmlDocument.CreateElement("Order");
                    orderElement.SetAttribute("Code", order.Code.ToString());
                    orderElement.SetAttribute("Layer1", order.Layer1Name);
                    orderElement.SetAttribute("Layer2", order.Layer2Name);
                    orderElement.SetAttribute("Connection1", order.Connection1Name);
                    orderElement.SetAttribute("Connection2", order.Connection2Name);

                    switch (order.Code)
                    {
                        case OrderCode.SRNTraining:
                        case OrderCode.SRNTest:
                            orderElement.SetAttribute("SRNInputLayerName", order.SRNInputLayerName);
                            orderElement.SetAttribute("SRNContextLayerName", order.SRNContextLayerName);
                            orderElement.SetAttribute("SRNHiddenLayerName", order.SRNHiddenLayerName);
                            orderElement.SetAttribute("SRNOutputLayerName", order.SRNOutputLayerName);
                            orderElement.SetAttribute("SRNIHConnectionName", order.SRNIHConnectionName);
                            orderElement.SetAttribute("SRNCHConnectionName", order.SRNCHConnectionName);
                            orderElement.SetAttribute("SRNHOConnectionName", order.SRNHOConnectionName);
                            orderElement.SetAttribute("SRNInputPatternName", order.SRNInputPatternName);
                            orderElement.SetAttribute("SRNOutputPatternName", order.SRNOutputPatternName);
                            orderElement.SetAttribute("SRNErrorCalculation", order.SRNErrorCalculation);
                            break;
                    }
                    orderListElement.AppendChild(orderElement);
                }
                processElement.AppendChild(orderListElement);

                processListNode.AppendChild(processElement);
            }
            rootNode.AppendChild(processListNode);

            xmlDocument.Save(fileName);
        }
        public void Process_Load(string fileName)
        {
            ProcessDictionary.Clear();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlNodeList processNodeList = xmlDocument.SelectNodes("/Root/ProcessList/Process");
            foreach (XmlNode processNode in processNodeList)
            {
                Process loadProcess = new Process();

                loadProcess.Name = processNode.Attributes["Name"].Value;
                foreach (XmlNode layerStateNode in processNode.SelectNodes("LayerStateDictionary/LayerState"))
                {
                    switch (layerStateNode.Attributes["State"].Value)
                    {
                        case "On":
                            loadProcess.LayerStateDictionary[layerStateNode.Attributes["Name"].Value] = LayerState.On;
                            loadProcess.LayerDamagedSDDictionary[layerStateNode.Attributes["Name"].Value] = 0;
                            break;
                        case "Off":
                            loadProcess.LayerStateDictionary[layerStateNode.Attributes["Name"].Value] = LayerState.Off;
                            loadProcess.LayerDamagedSDDictionary[layerStateNode.Attributes["Name"].Value] = 0;
                            break;
                        case "Damaged":
                            loadProcess.LayerStateDictionary[layerStateNode.Attributes["Name"].Value] = LayerState.Damaged;
                            loadProcess.LayerDamagedSDDictionary[layerStateNode.Attributes["Name"].Value] = double.Parse(layerStateNode.Attributes["SD"].Value);
                            break;
                    }
                }
                foreach (XmlNode connectionStateNode in processNode.SelectNodes("ConnectionStateDictionary/ConnectionState"))
                {
                    switch (connectionStateNode.Attributes["State"].Value)
                    {
                        case "On":
                            loadProcess.ConnectionStateDictionary[connectionStateNode.Attributes["Name"].Value] = ConnectionState.On;
                            loadProcess.ConnectionDamagedSDDictionary[connectionStateNode.Attributes["Name"].Value] = 0;
                            break;
                        case "Off":
                            loadProcess.ConnectionStateDictionary[connectionStateNode.Attributes["Name"].Value] = ConnectionState.Off;
                            loadProcess.ConnectionDamagedSDDictionary[connectionStateNode.Attributes["Name"].Value] = 0;
                            break;
                        case "Damaged":
                            loadProcess.ConnectionStateDictionary[connectionStateNode.Attributes["Name"].Value] = ConnectionState.Damaged;
                            loadProcess.ConnectionDamagedSDDictionary[connectionStateNode.Attributes["Name"].Value] = double.Parse(connectionStateNode.Attributes["SD"].Value);
                            break;
                    }
                }

                foreach (XmlNode orderNode in processNode.SelectNodes("OrderList/Order"))
                {
                    Order loadOrder = new Order();
                    switch (orderNode.Attributes["Code"].Value)
                    {
                        case "ActivationInput":
                            loadOrder.Code = OrderCode.ActivationInput;
                            break;
                        case "ActivationCalculate_Sigmoid":
                            loadOrder.Code = OrderCode.ActivationCalculate_Sigmoid;
                            break;
                        case "ActivationCalculate_Softmax":
                            loadOrder.Code = OrderCode.ActivationCalculate_Sigmoid;
                            break;
                        case "ActivationSend":
                            loadOrder.Code = OrderCode.ActivationSend;
                            break;
                        case "OutputErrorCalculate_for_Sigmoid":
                            loadOrder.Code = OrderCode.OutputErrorCalculate_for_Sigmoid;
                            break;
                        case "OutputErrorCalculate_for_Softmax":
                            loadOrder.Code = OrderCode.OutputErrorCalculate_for_Softmax;
                            break;
                        case "HiddenErrorCalculate_for_Sigmoid":
                            loadOrder.Code = OrderCode.HiddenErrorCalculate_for_Sigmoid;
                            break;
                        case "HiddenErrorCalculate_for_Softmax":
                            loadOrder.Code = OrderCode.HiddenErrorCalculate_for_Softmax;
                            break;
                        case "Interact":
                            loadOrder.Code = OrderCode.Interact;
                            break;
                        case "InterconnectionWeightRenewal":
                            loadOrder.Code = OrderCode.InterconnectionWeightRenewal;
                            break;
                        case "LayerDuplicate":
                            loadOrder.Code = OrderCode.LayerDuplicate;
                            break;
                        case "ConnectionDuplicate":
                            loadOrder.Code = OrderCode.ConnectionDuplicate;
                            break;
                        case "TransposedConnectionDuplicate":
                            loadOrder.Code = OrderCode.TransposedConnectionDuplicate;
                            break;
                        case "BiasRenewal":
                            loadOrder.Code = OrderCode.BiasRenewal;
                            break;
                        case "WeightRenewal":
                            loadOrder.Code = OrderCode.WeightRenewal;
                            break;
                        case "TestValueStore":
                            loadOrder.Code = OrderCode.TestValueStore;
                            break;
                        case "LayerInitialize":
                            loadOrder.Code = OrderCode.LayerInitialize;
                            break;
                        case "ConnectionInitialize":
                            loadOrder.Code = OrderCode.ConnectionInitialize;
                            break;
                        case "EndInitialize":
                            loadOrder.Code = OrderCode.EndInitialize;
                            break;
                        case "LayerToCleanUpForwardProcess":
                            loadOrder.Code = OrderCode.LayerToCleanUpForwardProcess;
                            break;
                        case "CleanUpToLayerForwardProcess":
                            loadOrder.Code = OrderCode.CleanUpToLayerForwardProcess;
                            break;
                        case "CleanUpBackwardProcess":
                            loadOrder.Code = OrderCode.CleanUpBackwardProcess;
                            break;
                        case "TickIn":
                            loadOrder.Code = OrderCode.TickIn;
                            break;
                        case "TickProgress":
                            loadOrder.Code = OrderCode.TickProgress;
                            break;
                        case "TickOut":
                            loadOrder.Code = OrderCode.TickOut;
                            break;
                        case "BaseWeightRenewal":
                            loadOrder.Code = OrderCode.BaseWeightRenewal;
                            break;
                        case "SRNTraining":
                            loadOrder.Code = OrderCode.SRNTraining;
                            break;
                        case "SRNTest":
                            loadOrder.Code = OrderCode.SRNTest;
                            break;
                    }
                    loadOrder.Layer1Name = orderNode.Attributes["Layer1"].Value;
                    loadOrder.Layer2Name = orderNode.Attributes["Layer2"].Value;
                    loadOrder.Connection1Name = orderNode.Attributes["Connection1"].Value;
                    loadOrder.Connection2Name = orderNode.Attributes["Connection2"].Value;

                    switch (orderNode.Attributes["Code"].Value)
                    {                        
                        case "SRNTraining":
                        case "SRNTest":
                            loadOrder.SRNInputLayerName = orderNode.Attributes["SRNInputLayerName"].Value;
                            loadOrder.SRNContextLayerName = orderNode.Attributes["SRNContextLayerName"].Value;
                            loadOrder.SRNHiddenLayerName = orderNode.Attributes["SRNHiddenLayerName"].Value;
                            loadOrder.SRNOutputLayerName = orderNode.Attributes["SRNOutputLayerName"].Value;
                            loadOrder.SRNIHConnectionName = orderNode.Attributes["SRNIHConnectionName"].Value;
                            loadOrder.SRNCHConnectionName = orderNode.Attributes["SRNCHConnectionName"].Value;
                            loadOrder.SRNHOConnectionName = orderNode.Attributes["SRNHOConnectionName"].Value;
                            loadOrder.SRNInputPatternName = orderNode.Attributes["SRNInputPatternName"].Value;
                            loadOrder.SRNOutputPatternName = orderNode.Attributes["SRNOutputPatternName"].Value;
                            loadOrder.SRNErrorCalculation = orderNode.Attributes["SRNErrorCalculation"].Value;
                            break;
                    }

                    loadProcess.Add(loadOrder);
                }

                ProcessDictionary[loadProcess.Name] = loadProcess;
            }
        }
    }

    class StimulusData : Dictionary<string, Matrix<double>>
    {
        public string PackName
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public double Possibility
        {
            get;
            set;
        }
        public int TimeStamp
        {
            get;
            set;
        }
    }    
    class StimuliPack : List<StimulusData>
    {
        public StimuliPack()
        {
            PatternNameList = new List<string>();            
        }

        public String Name
        {
            get;
            set;
        }
        public List<string> PatternNameList
        {
            get;
            set;
        }
        public int PatternCount()
        {
            return PatternNameList.Count;
        }
        public int RepresentationSize(string name)
        {
            return this[0][name].ColumnCount;
        }

        public bool RegularityCheck()
        {
            foreach(string patternName in PatternNameList)
            {
                for(int j=0;j<this.Count;j++)
                {
                    if (this[j][patternName].ColumnCount != this[0][patternName].ColumnCount) return false;
                }
            }

            return true;
        }
        
        public List<StimuliMatrix> GetMatrixStimuliData(bool useProbability, bool useRandomize, int miniBatchSize)
        {
            List<StimuliMatrix> stimuliMatrixList = new List<StimuliMatrix>();

            List<StimulusData> stimulusDataList = new List<StimulusData>();
            if (useProbability)
            {
                foreach (StimulusData stimulusData in this) if (SimulatorAccessor.random.NextDouble() < stimulusData.Possibility) stimulusDataList.Add(stimulusData);
            }
            else stimulusDataList = this.ToList();

            if (useRandomize) stimulusDataList = stimulusDataList.OrderBy(x => (SimulatorAccessor.random.Next())).ToList();

            int listSize = 0;
            if (stimulusDataList.Count % miniBatchSize == 0) listSize = stimulusDataList.Count / miniBatchSize;
            else listSize = stimulusDataList.Count / miniBatchSize + 1;

            for (int stimulusMatrixIndex=0;stimulusMatrixIndex<listSize;stimulusMatrixIndex++)
            {
                List<StimulusData> miniStimulusDataList = stimulusDataList.Skip(stimulusMatrixIndex * miniBatchSize).Take(miniBatchSize).ToList();

                StimuliMatrix newStimuliMatrix = new StimuliMatrix();
                newStimuliMatrix.PackName = Name;
                if(miniBatchSize >  1) newStimuliMatrix.TimeStamp = this[0].TimeStamp;
                else newStimuliMatrix.TimeStamp = miniStimulusDataList[0].TimeStamp;

                foreach (string patternName in PatternNameList) newStimuliMatrix[patternName] = DenseMatrix.Create(miniStimulusDataList.Count, RepresentationSize(patternName), 0);

                for (int index = 0; index < miniStimulusDataList.Count; index++)
                {
                    newStimuliMatrix.NameList.Add(miniStimulusDataList[index].Name);
                    foreach (string patternName in PatternNameList)
                    {
                        newStimuliMatrix[patternName].SetRow(index, miniStimulusDataList[index][patternName].Row(0));
                    }
                }

                stimuliMatrixList.Add(newStimuliMatrix);
            }

            return stimuliMatrixList;
        }
    }
    class StimuliMatrix : Dictionary<string, Matrix<double>>
    {
        public StimuliMatrix()
        {   
            NameList = new List<string>();
        }

        public string PackName
        {
            get;
            set;
        }
        public List<string> NameList
        {
            get;
            set;
        }
        public int TimeStamp
        {
            get;
            set;
        }
        public int StimuliCount
        {
            get
            {
                return NameList.Count;
            }
        }
    }

    class Order
    {
        public OrderCode Code
        {
            get;
            set;
        }
        public string Layer1Name
        {
            get;
            set;
        }
        public string Layer2Name
        {
            get;
            set;
        }
        public string Connection1Name
        {
            get;
            set;
        }
        public string Connection2Name
        {
            get;
            set;
        }

        public string SRNInputLayerName
        {
            get;
            set;
        }        
        public string SRNContextLayerName
        {
            get;
            set;
        }
        public string SRNHiddenLayerName
        {
            get;
            set;
        }        
        public string SRNOutputLayerName
        {
            get;
            set;
        }
        public string SRNIHConnectionName
        {
            get;
            set;
        }
        public string SRNCHConnectionName
        {
            get;
            set;
        }
        public string SRNHOConnectionName
        {
            get;
            set;
        }
        public string SRNInputPatternName
        {
            get;
            set;
        }
        public string SRNOutputPatternName
        {
            get;
            set;
        }
        public string SRNErrorCalculation
        {
            get;
            set;
        }
    }
    public enum OrderCode
    {
        ActivationInput,
        ActivationCalculate_Sigmoid,
        ActivationCalculate_Softmax,
        ActivationSend,
        OutputErrorCalculate_for_Sigmoid,
        OutputErrorCalculate_for_Softmax,
        HiddenErrorCalculate_for_Sigmoid,
        HiddenErrorCalculate_for_Softmax,
        Interact,
        InterconnectionWeightRenewal,
        LayerDuplicate,
        BiasRenewal,
        WeightRenewal,
        TestValueStore,
        EndInitialize,
        LayerToCleanUpForwardProcess,
        CleanUpToLayerForwardProcess,
        CleanUpBackwardProcess,
        TickIn,
        TickProgress,        
        TickOut,
        BaseWeightRenewal,
        LayerInitialize,
        ConnectionInitialize,
        ConnectionDuplicate,
        TransposedConnectionDuplicate,
        SRNTraining,
        SRNTest,
    }
    
    class Process : List<Order>
    {
        public Process()
        {
            LayerStateDictionary = new Dictionary<string, LayerState>();
            LayerDamagedSDDictionary = new Dictionary<string, double>();
            ConnectionStateDictionary = new Dictionary<string, ConnectionState>();
            ConnectionDamagedSDDictionary = new Dictionary<string, double>();
        }

        public string Name
        {
            get;
            set;
        }

        public Dictionary<string, LayerState> LayerStateDictionary
        {
            get;
            set;
        }
        public Dictionary<string, double> LayerDamagedSDDictionary
        {
            get;
            set;
        }
        public Dictionary<string, ConnectionState> ConnectionStateDictionary
        {
            get;
            set;
        }
        public Dictionary<string, double> ConnectionDamagedSDDictionary
        {
            get;
            set;
        }
    }

    class LearningSetup
    {
        public LearningSetup()
        {
            TrainingMatchingInformationList = new List<MatchingInformation>();
            TestMatchingInformationList = new List<MatchingInformation>();
        }

        public List<MatchingInformation> TrainingMatchingInformationList
        {
            get;
            set;
        }
        public List<MatchingInformation> TestMatchingInformationList
        {
            get;
            set;
        }
        
        public int TrainingEpoch
        {
            get;
            set;
        }        
        public int TestTiming
        {
            get;
            set;
        }
        public int MatrixCalculationSize
        {
            get;
            set;
        }
        public ProcessMode TrainingProcessMode
        {
            get;
            set;
        }
    }
    public enum ProcessMode
    {        
        RandomAll,
        RandomInStimuliPack,
        SequentialAll,
        SequentialinStimuliPack,
    }
    class MatchingInformation
    {
        public MatchingInformation()
        {
            PatternSetup = new Dictionary<int, string>();
        }

        public string StimuliPackName
        {
            get;
            set;
        }
        public string ProcessName
        {
            get;
            set;
        }
        public Dictionary<int, string> PatternSetup
        {
            get;
            set;
        }        
    }
    class TrialInformation
    {
        public TrialInformation()
        {
            PatternSetup = new Dictionary<int, string>();
        }
        public StimuliMatrix StimuliMatrix
        {
            get;
            set;
        }
        public Process Process
        {
            get;
            set;
        }
        public Dictionary<int, string> PatternSetup
        {
            get;
            set;
        }
    }

    class TestData
    {
        public string ProcessName
        {
            get;
            set;
        }
        public string StimuliPackName
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public int Epoch
        {
            get;
            set;
        }
        public int TimeStamp
        {
            get;
            set;
        }
        public double MeanSquredError
        {
            get;
            set;
        }
        public double MeanSemanticStress
        {
            get;
            set;
        }
        public double MeanCrossEntropy
        {
            get;
            set;
        }
        public double MeanActivation
        {
            get;
            set;
        }
        public double MeanActiveUnitActivation
        {
            get;
            set;
        }
        public double MeanInactiveUnitActivation
        {
            get;
            set;
        }
        public bool Correctness
        {
            get;
            set;
        }

        public Matrix<double> TargetPattern
        {
            get;
            set;
        }
        public Matrix<double> OutputActivation
        {
            get;
            set;
        }

        public Dictionary<string, Matrix<double>> LayerActivationInforamtion
        {
            get;
            set;
        }

    }    
    class TestDataList : List<TestData>
    {
        public string ProcessName
        {
            get;
            set;
        }
        public string StimuliPackName
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public int TimeStamp
        {
            get;
            set;
        }
    }    
}