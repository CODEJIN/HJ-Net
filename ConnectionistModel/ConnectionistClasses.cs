using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ConnectionistModel
{   
    public class Layer
    {
        protected Random random;
        
        public Layer(Random random, string name, int unitCount, int cleanupUnitCount, double initialWeightRange)
        {
            this.random = random;
            this.UnitCount = unitCount;
            this.CleanupUnitCount = cleanupUnitCount;
            this.Name = name;
            this.LayerType = LayerType.NormalLayer;

            this.SendConnectionDictionary = new Dictionary<string, Connection>();
            this.ReceiveConnectionDictionary = new Dictionary<string, Connection>();

            BiasMatrix = DenseMatrix.Create(1,UnitCount, 0);
            InterConnectionMatrix = DenseMatrix.Create(UnitCount, UnitCount, 0);
            if (cleanupUnitCount > 0)
            {
                CleanupBiasMatrix = DenseMatrix.Create(1, CleanupUnitCount, 0);
                LayerToCleanupWeightMatrix = DenseMatrix.Create(UnitCount, CleanupUnitCount, 0);
                CleanupToLayerWeightMatrix = DenseMatrix.Create(CleanupUnitCount, UnitCount, 0);
            }

            InitialWeightSetting(initialWeightRange);
        }
        public virtual void InitialWeightSetting(double initialWeightRange)
        {
            for (int unitIndex = 0; unitIndex < UnitCount; unitIndex++)
            {
                BiasMatrix[0, unitIndex] = random.NextDouble() * initialWeightRange * 2 - initialWeightRange;
                for (int toUnitIndex = 0; toUnitIndex < UnitCount; toUnitIndex++)
                {
                    InterConnectionMatrix[unitIndex, toUnitIndex] = random.NextDouble() * initialWeightRange * 2 - initialWeightRange;
                }
            }
            for (int cleanupUnitIndex = 0; cleanupUnitIndex < CleanupUnitCount; cleanupUnitIndex++)
            {
                CleanupBiasMatrix[0, cleanupUnitIndex] = random.NextDouble() * initialWeightRange * 2 - initialWeightRange;
                for (int layerUnitIndex = 0; layerUnitIndex < UnitCount; layerUnitIndex++)
                {
                    LayerToCleanupWeightMatrix[layerUnitIndex, cleanupUnitIndex] = random.NextDouble() * initialWeightRange * 2 - initialWeightRange;
                    CleanupToLayerWeightMatrix[cleanupUnitIndex, layerUnitIndex] = random.NextDouble() * initialWeightRange * 2 - initialWeightRange;
                }
            }
        }

        public virtual void ActivationInput(Matrix<double> inputPattern)
        {
            layerActivationMatrix = inputPattern; 
        }
        public void CalculateActivation_Sigmoid(double momentum)
        {             
            layerActivationMatrix = 1 / (1 + (-1 * momentum * (LayerStroageMatrix + BiasMatrixForMatrixCalculation)).PointwiseExp());
        }
        public void CalculateActivation_Softmax()
        {
            Vector<double> sumVector = (LayerStroageMatrix + BiasMatrixForMatrixCalculation).PointwiseExp().RowSums();
            Matrix<double> sumMatrix = DenseMatrix.Create(LayerStroageMatrix.RowCount, LayerStroageMatrix.ColumnCount, 0);
            for (int columnIndex = 0; columnIndex < LayerStroageMatrix.ColumnCount; columnIndex++) sumMatrix.SetColumn(columnIndex, sumVector);
            layerActivationMatrix = (LayerStroageMatrix + BiasMatrixForMatrixCalculation).PointwiseExp().PointwiseDivide(sumMatrix);


        }
        public void SendActivation()
        {
            foreach(string key in SendConnectionDictionary.Keys)
            {                
                SendConnectionDictionary[key].ReceiveLayer.LayerStroageMatrix += LayerActivationMatrix * SendConnectionDictionary[key].WeightMatrix;
            }
        }
        public virtual void ErrorRateCalculate_Sigmoid(Matrix<double> targetPattern, double momentum)
        {
            LayerErrorMatrix = (targetPattern - layerActivationMatrix).PointwiseMultiply(momentum * layerActivationMatrix.PointwiseMultiply(1 - layerActivationMatrix));
        }
        public virtual void ErrorRateCalculate_Softmax(Matrix<double> targetPattern)
        {
            LayerErrorMatrix = targetPattern - layerActivationMatrix;
        }
        public virtual void ErrorRateCalculate_Sigmoid(double momentum)
        {
            foreach (string key in SendConnectionDictionary.Keys)
            {
                LayerErrorMatrix += (SendConnectionDictionary[key].ReceiveLayer.LayerErrorMatrix * SendConnectionDictionary[key].WeightMatrix.Transpose()).PointwiseMultiply(momentum * layerActivationMatrix.PointwiseMultiply(1 - layerActivationMatrix));
            }
        }
        public virtual void ErrorRateCalculate_Softmax()
        {
            foreach (string key in SendConnectionDictionary.Keys)
            {
                LayerErrorMatrix += SendConnectionDictionary[key].ReceiveLayer.LayerErrorMatrix * SendConnectionDictionary[key].WeightMatrix.Transpose() ;
            }
        }
        public void Interact()
        {
            LayerStroageMatrix = LayerStroageMatrix + (LayerActivationMatrix * InterConnectionMatrix);
        }
        public void InterConnectionWeightRenewal(double learningRate, double decayRate)
        {
            InterConnectionMatrix += LayerActivationMatrix.Transpose() *LayerErrorMatrix *learningRate;
            Parallel.For(0, InterConnectionMatrix.RowCount, index =>
            {   
                InterConnectionMatrix[index, index] = 0;
            });
            if (decayRate > 0)
            {
                Parallel.For(0, InterConnectionMatrix.RowCount, rowIndex =>
                {
                    for (int columnIndex = 0; columnIndex < InterConnectionMatrix.ColumnCount; columnIndex++)
                    {
                        if (InterConnectionMatrix[rowIndex, columnIndex] > 0) InterConnectionMatrix[rowIndex, columnIndex] -= decayRate;
                        else if (InterConnectionMatrix[rowIndex, columnIndex] < 0) InterConnectionMatrix[rowIndex, columnIndex] += decayRate;
                    }
                });

                InterConnectionMatrix.CoerceZero(decayRate);
            }
        }
        public void LayerToCleanUpForwardProcess(double momentum)
        {
            CleanupLayerStroageMatrix = LayerActivationMatrix * LayerToCleanupWeightMatrix;
            CleanupLayerActivationMatrix = 1 / (1 + (-1 * momentum * (CleanupLayerStroageMatrix + CleanupBiasMatrixForMatrixCalculation)).PointwiseExp());
        }
        public void CleanUpToLayerForwardProcess()
        {   
            LayerStroageMatrix += CleanupLayerActivationMatrix * CleanupToLayerWeightMatrix;
        }                
        public void CleanUpBackwordProcess(double momentum, double learningRate, double decayRate)
        {
            CleanupLayerErrorMatrix = (LayerErrorMatrix * CleanupToLayerWeightMatrix.Transpose())
                .PointwiseMultiply(CleanupLayerActivationMatrix.PointwiseMultiply(1 - CleanupLayerActivationMatrix));

            CleanupBiasMatrix += CleanupLayerErrorMatrix.ColumnSums().ToRowMatrix() * learningRate;

            CleanupToLayerWeightMatrix += CleanupLayerActivationMatrix.Transpose() * LayerErrorMatrix * learningRate;
            LayerToCleanupWeightMatrix += LayerActivationMatrix.Transpose() * CleanupLayerErrorMatrix * learningRate;

            if (decayRate > 0)
            {
                Parallel.For(0, CleanupBiasMatrix.ColumnCount, columnIndex =>
                {
                    if (CleanupBiasMatrix[0, columnIndex] > 0) CleanupBiasMatrix[0, columnIndex] -= decayRate;
                    else if (CleanupBiasMatrix[0, columnIndex] < 0) CleanupBiasMatrix[0, columnIndex] += decayRate;

                });
                CleanupBiasMatrix.CoerceZero(decayRate);

                Parallel.For(0, CleanupToLayerWeightMatrix.RowCount, rowIndex =>
                {
                    for (int columnIndex = 0; columnIndex < CleanupToLayerWeightMatrix.ColumnCount; columnIndex++)
                    {
                        if (CleanupToLayerWeightMatrix[rowIndex, columnIndex] > 0) CleanupToLayerWeightMatrix[rowIndex, columnIndex] -= decayRate;
                        else if (CleanupToLayerWeightMatrix[rowIndex, columnIndex] < 0) CleanupToLayerWeightMatrix[rowIndex, columnIndex] += decayRate;
                    }
                });
                CleanupToLayerWeightMatrix.CoerceZero(decayRate);

                Parallel.For(0, LayerToCleanupWeightMatrix.RowCount, rowIndex =>
                {
                    for (int columnIndex = 0; columnIndex < LayerToCleanupWeightMatrix.ColumnCount; columnIndex++)
                    {
                        if (LayerToCleanupWeightMatrix[rowIndex, columnIndex] > 0) LayerToCleanupWeightMatrix[rowIndex, columnIndex] -= decayRate;
                        else if (LayerToCleanupWeightMatrix[rowIndex, columnIndex] < 0) LayerToCleanupWeightMatrix[rowIndex, columnIndex] += decayRate;
                    }
                });
                LayerToCleanupWeightMatrix.CoerceZero(decayRate);
            }
        }

        public virtual void BiasRenewal(double learningRate, double decayRate)
        {   
            BiasMatrix += LayerErrorMatrix.ColumnSums().ToRowMatrix() * learningRate;
            
            if (decayRate > 0)
            {
                Parallel.For(0, BiasMatrix.ColumnCount, columnIndex =>
                {
                    if (BiasMatrix[0, columnIndex] > 0) BiasMatrix[0, columnIndex] -= decayRate;
                    else if (BiasMatrix[0, columnIndex] < 0) BiasMatrix[0, columnIndex] += decayRate;

                });
                BiasMatrix.CoerceZero(decayRate);
            }
        }
        
        public void Test(Matrix<double> targetPattern, double activationCriterion, double inactivationCriterion, out List<double> meanSEList, out List<double> meanSSList, out List<double> meanCEList, out List<bool> correctnessList, out List<double> meanActivationList, out List<double> meanAUActivationList, out List<double> meanIUActivationList)
        {
            meanSEList = ((targetPattern - LayerActivationMatrix).PointwisePower(2).RowSums() / 2.0 / UnitCount).ToList();
            meanSSList = ((((LayerActivationMatrix.PointwiseMultiply(LayerActivationMatrix.PointwiseLog()) + ((1 - LayerActivationMatrix).PointwiseMultiply((1 - LayerActivationMatrix).PointwiseLog()))) / Math.Log(2)) + 1).RowSums() / UnitCount).ToList();
            meanCEList = (((targetPattern.PointwiseMultiply(LayerActivationMatrix.PointwiseLog()) + (1 - targetPattern).PointwiseMultiply((1 - LayerActivationMatrix).PointwiseLog())) / Math.Log(Math.E)).RowSums() * -1 / UnitCount).ToList();            
            meanActivationList = (LayerActivationMatrix.RowSums() / UnitCount).ToList();

            correctnessList = new List<bool>(LayerActivationMatrix.RowCount);
            meanAUActivationList = new List<double>(LayerActivationMatrix.RowCount);
            meanIUActivationList = new List<double>(LayerActivationMatrix.RowCount);
            for (int index = 0; index < LayerActivationMatrix.RowCount; index++)
            {
                correctnessList.Add(true);
                meanAUActivationList.Add(0);
                meanIUActivationList.Add(0);
            }

            Matrix<double> activeUnitMatrix = DenseMatrix.Create(LayerActivationMatrix.RowCount, LayerActivationMatrix.ColumnCount, 0);
            Matrix<double> inactiveUnitMatrix = DenseMatrix.Create(LayerActivationMatrix.RowCount, LayerActivationMatrix.ColumnCount, 0);
            Matrix<double> countActive = DenseMatrix.Create(LayerActivationMatrix.RowCount, 1, 0);
            Matrix<double> countInactive = DenseMatrix.Create(LayerActivationMatrix.RowCount, 1, 0);
            for (int rowIndex = 0;rowIndex<LayerActivationMatrix.RowCount;rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < LayerActivationMatrix.ColumnCount; columnIndex++)
                {   
                    if (targetPattern[rowIndex, columnIndex] > activationCriterion)
                    {
                        if(LayerActivationMatrix[rowIndex, columnIndex] < activationCriterion) correctnessList[rowIndex] = false;
                        activeUnitMatrix[rowIndex, columnIndex] = 1;
                        countActive[rowIndex, 0]++;
                    }
                    if (targetPattern[rowIndex, columnIndex] < inactivationCriterion)
                    {
                        if (LayerActivationMatrix[rowIndex, columnIndex] > activationCriterion) correctnessList[rowIndex] = false;
                        inactiveUnitMatrix[rowIndex, columnIndex] = 1;
                        countInactive[rowIndex, 0]++;
                    }
                }
            }
            meanAUActivationList = LayerActivationMatrix.PointwiseMultiply(activeUnitMatrix).RowSums().ToList();
            meanIUActivationList = LayerActivationMatrix.PointwiseMultiply(inactiveUnitMatrix).RowSums().ToList();
            for (int rowIndex = 0; rowIndex < LayerActivationMatrix.RowCount; rowIndex++)
            {
                if (countActive[rowIndex, 0] > 0) meanAUActivationList[rowIndex] = meanAUActivationList[rowIndex] / countActive[rowIndex,0];
                else
                {
                    meanAUActivationList[rowIndex] = double.NaN;
                }
                if (countInactive[rowIndex, 0] > 0) meanIUActivationList[rowIndex] = meanIUActivationList[rowIndex] / countInactive[rowIndex,0];
                else
                {
                    meanIUActivationList[rowIndex] = double.NaN;
                }
            }
        }

        public virtual void Initialize(LayerState layerState, double damagedSD, int rowCount)
        {
            this.SetState(layerState, damagedSD, rowCount);

            LayerStroageMatrix = DenseMatrix.Create(rowCount, UnitCount, 0);
            LayerActivationMatrix = DenseMatrix.Create(rowCount, UnitCount, 0);
            LayerErrorMatrix = DenseMatrix.Create(rowCount, UnitCount, 0);

            if (CleanupUnitCount > 0)
            {
                CleanupLayerStroageMatrix = DenseMatrix.Create(rowCount, CleanupUnitCount, 0);
                CleanupLayerActivationMatrix = DenseMatrix.Create(rowCount, CleanupUnitCount, 0);
                CleanupLayerErrorMatrix = DenseMatrix.Create(rowCount, CleanupUnitCount, 0);
            }
        }

        public virtual void Duplicate(Layer cloneLayer)
        {
            cloneLayer.LayerStroageMatrix = LayerStroageMatrix.Clone();
            cloneLayer.LayerActivationMatrix = LayerActivationMatrix.Clone();
            cloneLayer.LayerErrorMatrix = LayerErrorMatrix.Clone();
            cloneLayer.BiasMatrix = BiasMatrix.Clone();
            cloneLayer.InterConnectionMatrix = InterConnectionMatrix.Clone();
        }
        
        public string Name
        {
            get;
            protected set;
        }
        public LayerType LayerType
        {
            get;
            protected set;
        }
        public int UnitCount
        {
            get;
            set;
        }
        public int CleanupUnitCount
        {
            get;
            set;
        }

        public Matrix<double> LayerStroageMatrix
        {
            get;
            set;
        }
        private Matrix<double> layerActivationMatrix;
        public Matrix<double> LayerActivationMatrix
        {
            get
            {
                switch(state)
                {
                    case LayerState.On:
                        return layerActivationMatrix;
                    case LayerState.Off:
                        return stateMatrix;
                    case LayerState.Damaged:
                        Matrix<double> returnMatrix = layerActivationMatrix + stateMatrix;
                        //This code is too bad.                        
                        for(int columnIndex=0;columnIndex<returnMatrix.ColumnCount;columnIndex++)
                        {
                            if (returnMatrix[0, columnIndex] > 1) returnMatrix[0, columnIndex] = 1;
                            else if (returnMatrix[0, columnIndex] < 0) returnMatrix[0, columnIndex] = 0;
                        }
                        return returnMatrix;                                                
                    default:
                        return layerActivationMatrix;
                }                              
            }
            set
            {
                layerActivationMatrix = value;
            }
        }
        public Matrix<double> LayerErrorMatrix
        {
            get;
            set;
        }

        public Matrix<double> BiasMatrix
        {
            get;
            set;
        }
        private Matrix<double> BiasMatrixForMatrixCalculation
        {
            get
            {
                Matrix<double> matrixBiasMatrix = DenseMatrix.Create(LayerStroageMatrix.RowCount, LayerStroageMatrix.ColumnCount, 0);
                Parallel.For(0, LayerStroageMatrix.RowCount, rowIndex =>
                {
                    matrixBiasMatrix.SetRow(rowIndex, BiasMatrix.Row(0));
                });
                return matrixBiasMatrix;
            }
        }

        public Matrix<double> InterConnectionMatrix
        {
            get;
            set;
        }
        
        public Matrix<double> CleanupLayerStroageMatrix
        {
            get;
            protected set;
        }        
        public Matrix<double> CleanupLayerActivationMatrix
        {
            get;
            set;
        }
        public Matrix<double> CleanupLayerErrorMatrix
        {
            get;
            protected set;
        }
        public Matrix<double> LayerToCleanupWeightMatrix
        {
            get;
            set;
        }
        public Matrix<double> CleanupToLayerWeightMatrix
        {
            get;
            set;
        }

        public Matrix<double> CleanupBiasMatrix
        {
            get;
            set;
        }
        private Matrix<double> CleanupBiasMatrixForMatrixCalculation
        {
            get
            {
                Matrix<double> matrixCleanupBiasMatrix = DenseMatrix.Create(CleanupLayerStroageMatrix.RowCount, CleanupLayerStroageMatrix.ColumnCount, 0);
                Parallel.For(0, CleanupLayerStroageMatrix.RowCount, rowIndex =>
                {
                    matrixCleanupBiasMatrix.SetRow(rowIndex, CleanupBiasMatrix.Row(0));
                });
                return matrixCleanupBiasMatrix;
            }
        }

        public Dictionary<string, Connection> SendConnectionDictionary
        {
            get;
            set;
        }
        public Dictionary<string, Connection> ReceiveConnectionDictionary
        {
            get;
            set;
        }
        
        private LayerState state;
        private Matrix<double> stateMatrix;        
        public void SetState(LayerState layerState, double damagedSD, int rowCount)
        {
            state = layerState;
            switch(layerState)
            {
                case LayerState.On:
                    stateMatrix = DenseMatrix.Create(rowCount, UnitCount, 1);
                    break;
                case LayerState.Off:
                    stateMatrix = DenseMatrix.Create(rowCount, UnitCount, 0);
                    break;
                case LayerState.Damaged:
                    stateMatrix = RandomizeGaussianMatrixMaker(damagedSD, rowCount);
                    break;
            }
            
        }

        private Matrix<double> RandomizeGaussianMatrixMaker(double damagedSD, int rowCount)
        {
            MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, damagedSD);
            Matrix<double> newMatrix = DenseMatrix.CreateRandom(rowCount, UnitCount, normalDist);

            return newMatrix;
        }
    }

    public enum LayerState
    {
        On,
        Off,
        Damaged,
    }    

    public class BPTTLayer : Layer
    {
        public BPTTLayer(Random random, string name, int unitCount, int cleanupUnitCount, double initialWeightRange, int tick) : base(random, name, unitCount, cleanupUnitCount, initialWeightRange)
        {   
            this.LayerType = LayerType.BPTTLayer;
            this.Tick = tick;
            this.CurrentTick = 0;
            
            InitialWeightSetting(initialWeightRange);
        }

        public override void InitialWeightSetting(double initialWeightRange)
        {
            BaseHideWeightMatrix = DenseMatrix.Create(UnitCount, UnitCount, 0);
            BaseHideBiasMatrix = DenseMatrix.Create(1, UnitCount, 0);

            for (int unitIndex = 0; unitIndex < UnitCount; unitIndex++)
            {
                BaseHideBiasMatrix[0, unitIndex] = random.NextDouble() * initialWeightRange * 2 - initialWeightRange;
                for (int toUnitIndex = 0; toUnitIndex < UnitCount; toUnitIndex++)
                {
                    BaseHideWeightMatrix[unitIndex, toUnitIndex] = random.NextDouble() * initialWeightRange * 2 - initialWeightRange;
                }
            }
        }
        
        public void TickIn(double momentum, int rowCount)
        {
            HideLayerList[0].LayerStroageMatrix = (Matrix<double>)LayerStroageMatrix.Clone();
            HideLayerList[0].CalculateActivation_Sigmoid(momentum);
            LayerStroageMatrix = DenseMatrix.Create(rowCount, UnitCount, 0);
        }        
        public void TickProgress(double momentum, int rowCount)
        {
            HideLayerList[CurrentTick + 1].LayerStroageMatrix = (Matrix<double>)LayerStroageMatrix.Clone();
            HideLayerList[CurrentTick].SendActivation();
            HideLayerList[CurrentTick + 1].CalculateActivation_Sigmoid(momentum);
            CurrentTick++;

            LayerStroageMatrix = DenseMatrix.Create(rowCount, UnitCount, 0);
        }
        public void TickOut()
        {
            LayerActivationMatrix = (Matrix<double>)HideLayerList[CurrentTick].LayerActivationMatrix.Clone();
        }
        public override void Duplicate(Layer cloneLayer)
        {
            if (cloneLayer.LayerType != LayerType.BPTTLayer) throw new Exception();
            else if (((BPTTLayer)cloneLayer).Tick != Tick) throw new Exception();
            else
            {
                base.Duplicate(cloneLayer);

                for (int hideLayerIndex = 0; hideLayerIndex < HideLayerList.Count; hideLayerIndex++)
                {
                    ((BPTTLayer)cloneLayer).HideLayerList[hideLayerIndex].LayerStroageMatrix = HideLayerList[hideLayerIndex].LayerStroageMatrix.Clone();
                    ((BPTTLayer)cloneLayer).HideLayerList[hideLayerIndex].LayerActivationMatrix = HideLayerList[hideLayerIndex].LayerActivationMatrix.Clone();
                    ((BPTTLayer)cloneLayer).HideLayerList[hideLayerIndex].LayerErrorMatrix = HideLayerList[hideLayerIndex].LayerErrorMatrix.Clone();
                    ((BPTTLayer)cloneLayer).HideLayerList[hideLayerIndex].BiasMatrix = HideLayerList[hideLayerIndex].BiasMatrix.Clone();
                    ((BPTTLayer)cloneLayer).HideLayerList[hideLayerIndex].InterConnectionMatrix = HideLayerList[hideLayerIndex].InterConnectionMatrix.Clone();
                }
                for (int hideConnectionIndex = 0; hideConnectionIndex < HideConnectionList.Count; hideConnectionIndex++)
                {
                    ((BPTTLayer)cloneLayer).HideConnectionList[hideConnectionIndex].WeightMatrix = HideConnectionList[hideConnectionIndex].WeightMatrix;
                }
                ((BPTTLayer)cloneLayer).BaseHideWeightMatrix = BaseHideWeightMatrix.Clone();
                ((BPTTLayer)cloneLayer).BaseHideBiasMatrix = BaseHideBiasMatrix.Clone();
            }
        }

        public void WeightRenewal(double momentum, double learningRate, double decayRate)
        {
            base.ErrorRateCalculate_Sigmoid(momentum);
            HideLayerList[CurrentTick].LayerErrorMatrix = (Matrix<double>)LayerErrorMatrix.Clone();
            for (int i = CurrentTick - 1; i >= 0; i--) HideLayerList[i].ErrorRateCalculate_Sigmoid(momentum); //Boden (2001)

            Matrix<double> hideWeightDeltaMatrix = DenseMatrix.Create(UnitCount, UnitCount, 0);
            Matrix<double> hideBiasDeltaMatrix = DenseMatrix.Create(1, UnitCount, 0);
                        
            foreach (Layer layer in HideLayerList) hideBiasDeltaMatrix += layer.LayerErrorMatrix.ColumnSums().ToRowMatrix() * learningRate;
            foreach (Connection conneciton in HideConnectionList) hideWeightDeltaMatrix += conneciton.SendLayer.LayerActivationMatrix.Transpose() * conneciton.ReceiveLayer.LayerErrorMatrix * learningRate;
            //for (int layerIndex = 0; layerIndex < CurrentTick - 1; layerIndex++)
            //{
            //    hideWeightDeltaMatrix += HideLayerList[layerIndex].LayerActivationMatrix.Transpose() * HideLayerList[layerIndex + 1].LayerErrorMatrix * learningRate;
            //}

            BaseHideBiasMatrix += hideBiasDeltaMatrix / CurrentTick;
            BaseHideWeightMatrix += hideWeightDeltaMatrix / CurrentTick;

            if (decayRate > 0)
            {
                Parallel.For(0, BaseHideBiasMatrix.ColumnCount, columnIndex =>
                {
                    if (BaseHideBiasMatrix[0, columnIndex] > 0) BaseHideBiasMatrix[0, columnIndex] -= decayRate;
                    else if (BaseHideBiasMatrix[0, columnIndex] < 0) BaseHideBiasMatrix[0, columnIndex] += decayRate;

                });
                BaseHideBiasMatrix.CoerceZero(decayRate);

                Parallel.For(0, BaseHideWeightMatrix.RowCount, rowIndex =>
                {
                    for (int columnIndex = 0; columnIndex < BaseHideWeightMatrix.ColumnCount; columnIndex++)
                    {
                        if (BaseHideWeightMatrix[rowIndex, columnIndex] > 0) BaseHideWeightMatrix[rowIndex, columnIndex] -= decayRate;
                        else if (BaseHideWeightMatrix[rowIndex, columnIndex] < 0) BaseHideWeightMatrix[rowIndex, columnIndex] += decayRate;
                    }
                });
                BaseHideWeightMatrix.CoerceZero(decayRate);
            }

            //BPTT Layer에 Sending하는 Layer 및 Connection의 값을 구하기 위함
            Matrix<double> hideLayerErrorMatrixSum = DenseMatrix.Create(HideLayerList[0].LayerErrorMatrix.RowCount, UnitCount, 0);
            for (int i = CurrentTick; i >= 0; i--) hideLayerErrorMatrixSum += HideLayerList[i].LayerErrorMatrix;
            LayerErrorMatrix = hideLayerErrorMatrixSum / CurrentTick;
        }

        public override void Initialize(LayerState layerState, double damagedSD, int rowCount)
        {
            base.Initialize(layerState, damagedSD, rowCount);
            CurrentTick = 0;
            HideLayerList = new List<Layer>();            
            for (int layerIndex = 0; layerIndex <= Tick; layerIndex++)
            {
                Layer newLayer = new Layer(random, Name + "-Hide" + layerIndex, UnitCount, 0, 0);
                newLayer.BiasMatrix = (Matrix<double>)BaseHideBiasMatrix.Clone();
                newLayer.Initialize(layerState, damagedSD, rowCount);
                HideLayerList.Add(newLayer);

            }
            HideConnectionList = new List<Connection>();
            for (int connectionIndex = 0; connectionIndex < Tick; connectionIndex++)
            {
                Connection newConnection = new Connection(random, Name + "-Hide" + connectionIndex, HideLayerList[connectionIndex], HideLayerList[connectionIndex + 1], 0);
                newConnection.WeightMatrix = (Matrix<double>)BaseHideWeightMatrix.Clone();
                newConnection.Initialize(ConnectionState.On, 0);                
                HideConnectionList.Add(newConnection);
            }
        }

        public List<Layer> HideLayerList
        {
            get;
            set;
        }
        public List<Connection> HideConnectionList
        {
            get;
            set;
        }

        public Matrix<double> BaseHideWeightMatrix
        {
            get;
            set;
        }
        public Matrix<double> BaseHideBiasMatrix
        {
            get;
            set;
        }


        public int Tick
        {
            get;
            private set;
        }
        public int CurrentTick
        {
            private set;
            get;
        }
    }

    public enum LayerType
    {
        NoLayer = 1,
        NormalLayer = 2,
        BPTTLayer = 3,
    }

    public class Connection
    {
        protected Random random;
        
        public Connection(Random random, string name, Layer sendLayer, Layer receiveLayer, double initialWeightRange)
        {
            this.random = random;

            this.Name = name;
            this.SendLayer = sendLayer;
            this.ReceiveLayer = receiveLayer;
            
            sendLayer.SendConnectionDictionary[name] = this;
            receiveLayer.ReceiveConnectionDictionary[name] = this;

            weightMatrix = DenseMatrix.Create(sendLayer.UnitCount, receiveLayer.UnitCount,0);

            InitialWeightSetting(initialWeightRange);
        }        
        public void InitialWeightSetting(double initialWeightRange)
        {
            for (int sendIndex = 0; sendIndex < SendLayer.UnitCount; sendIndex++)
            {   
                for (int receiveIndex = 0; receiveIndex < ReceiveLayer.UnitCount; receiveIndex++)
                {
                    weightMatrix[sendIndex, receiveIndex] = random.NextDouble() * initialWeightRange * 2 - initialWeightRange;
                }
            }
        }
        
        public void WeightRenewal(double learningRate, double decayRate)
        {
            weightMatrix += SendLayer.LayerActivationMatrix.Transpose() * ReceiveLayer.LayerErrorMatrix * learningRate;
            if (decayRate > 0)
            {
                Parallel.For(0, weightMatrix.RowCount, rowIndex =>
                {
                    for (int columnIndex = 0; columnIndex < weightMatrix.ColumnCount; columnIndex++)
                    {
                        if (weightMatrix[rowIndex, columnIndex] > 0) weightMatrix[rowIndex, columnIndex] -= decayRate;
                        else if (weightMatrix[rowIndex, columnIndex] < 0) weightMatrix[rowIndex, columnIndex] += decayRate;
                    }
                });

                weightMatrix.CoerceZero(decayRate);
            }
        }

        public void Initialize(ConnectionState connectionState, double damagedSD)
        {
            this.SetState(connectionState, damagedSD);
        }        

        public void Duplicate(Connection cloneConnection)
        {
            cloneConnection.WeightMatrix = WeightMatrix;
        }

        public void TransposedDuplicate(Connection cloneConnection)
        {
            cloneConnection.WeightMatrix = WeightMatrix.Transpose();
        }

        public Layer SendLayer
        {
            get;
            protected set;
        }

        public Layer ReceiveLayer
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        private Matrix<double> weightMatrix;
        public Matrix<double> WeightMatrix
        {
            get
            {
                switch(state)
                {
                    case ConnectionState.On:
                        return weightMatrix;
                    case ConnectionState.Off:
                        return stateMatrix;
                    case ConnectionState.Damaged:
                        return weightMatrix + stateMatrix;
                    default:
                        return weightMatrix;
                }
            }
            set
            {
                weightMatrix = value;
            }
        }
        
        private ConnectionState state;
        private Matrix<double> stateMatrix;
        public void SetState(ConnectionState connectionState, double damagedSD)
        {
            state = connectionState;
            switch (connectionState)
            {
                case ConnectionState.On:
                    stateMatrix = DenseMatrix.Create(SendLayer.UnitCount, ReceiveLayer.UnitCount, 1);
                    break;
                case ConnectionState.Off:
                    stateMatrix = DenseMatrix.Create(SendLayer.UnitCount, ReceiveLayer.UnitCount, 0);
                    break;
                case ConnectionState.Damaged:
                    stateMatrix = RandomizeGaussianMatrixMaker(damagedSD);
                    break;
            }
            
        }

        private Matrix<double> RandomizeGaussianMatrixMaker(double damagedSD)
        {
            MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, damagedSD);
            Matrix<double> newMatrix = DenseMatrix.CreateRandom(SendLayer.UnitCount, ReceiveLayer.UnitCount, normalDist);

            return newMatrix;
        }
    }

    public enum ConnectionState
    {
        On,
        Off,
        Damaged,
    }

    public class ActivationPatternMaker
    {
        public static Matrix<double> Maker(string pattern)
        {
            string[] patternArray = pattern.Split(' ');

            Matrix<double> newPattern = DenseMatrix.Create(1, patternArray.Length,0);

            for (int index = 0; index < patternArray.Length; index++) newPattern[0, index] = double.Parse(patternArray[index]);
            return newPattern;
        }
    }
}
