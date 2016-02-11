using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace KoreanRepresentationMakerBasedYou2015
{
    public partial class KorWordPatternMaker : Form
    {
        public KorWordPatternMaker()
        {
            InitializeComponent();
        }

        private void txtFileBrowserButton_Click(object sender, EventArgs e)
        {
            txtFileDialog.Multiselect = false;
            txtFileDialog.Filter = "Txt File (*.txt)|*.txt";
            txtFileDialog.InitialDirectory = Application.StartupPath;

            if (txtFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fileNameTextBox.Text = txtFileDialog.FileName;
            }
        }

        private void makingButton_Click(object sender, EventArgs e)
        {
            Making(fileNameTextBox.Text);
        }

        private void Making(string loadFileName)
        {
            List<WordData> wordDataList = new List<WordData>();

            StreamReader streamReader = new StreamReader(loadFileName, System.Text.Encoding.GetEncoding("ks_c_5601-1987"), true);
            while(!streamReader.EndOfStream)
            {
                WordData newWordData = new WordData();
                newWordData.Word = streamReader.ReadLine();                
                wordDataList.Add(newWordData);
            }
            streamReader.Close();

            int maxSyllable = 0;
            foreach (WordData wordData in wordDataList) if (wordData.Word.Length > maxSyllable) maxSyllable = wordData.Word.Length;

            foreach (WordData wordData in wordDataList)
            {
                wordData.IndexList = WordIndexMaking(wordData.Word, nonLocationFeatureUseCheckBox.Checked);
                wordData.Pattern = PatternMaking(wordData.IndexList, maxSyllable, nonLocationFeatureUseCheckBox.Checked);
            }

            StreamWriter streamWriter = new StreamWriter("Pattern.txt");

            streamWriter.WriteLine("\t\tKoreanWord");
            foreach(WordData wordData in wordDataList)
            {
                streamWriter.Write(wordData.Word + "\t");
                streamWriter.Write("\t");
                streamWriter.Write(wordData.Pattern);
                streamWriter.WriteLine();
            }

            streamWriter.Flush();
            streamWriter.Close();

            MessageBox.Show("Finished");
        }

        private void SyllableToLetterMaking(char syllable, out char onset, out char nucleus, out char coda)
        {
            onset = ' ';
            nucleus = ' ';
            coda = ' ';

            if (syllable < '까') onset = 'ㄱ';
            else if (syllable < '나') onset = 'ㄲ';
            else if (syllable < '다') onset = 'ㄴ';
            else if (syllable < '따') onset = 'ㄷ';
            else if (syllable < '라') onset = 'ㄸ';
            else if (syllable < '마') onset = 'ㄹ';
            else if (syllable < '바') onset = 'ㅁ';
            else if (syllable < '빠') onset = 'ㅂ';
            else if (syllable < '사') onset = 'ㅃ';
            else if (syllable < '싸') onset = 'ㅅ';
            else if (syllable < '아') onset = 'ㅆ';
            else if (syllable < '자') onset = 'ㅇ';
            else if (syllable < '짜') onset = 'ㅈ';
            else if (syllable < '차') onset = 'ㅉ';
            else if (syllable < '카') onset = 'ㅊ';
            else if (syllable < '타') onset = 'ㅋ';
            else if (syllable < '파') onset = 'ㅌ';
            else if (syllable < '하') onset = 'ㅍ';
            else if (syllable < '\uD7A4') onset = 'ㅎ';

            if (((syllable - '가') % 588) < 28 * 1) nucleus = 'ㅏ';
            else if (((syllable - '가') % 588) < 28 * 2) nucleus = 'ㅐ';
            else if (((syllable - '가') % 588) < 28 * 3) nucleus = 'ㅑ';
            else if (((syllable - '가') % 588) < 28 * 4) nucleus = 'ㅒ';
            else if (((syllable - '가') % 588) < 28 * 5) nucleus = 'ㅓ';
            else if (((syllable - '가') % 588) < 28 * 6) nucleus = 'ㅔ';
            else if (((syllable - '가') % 588) < 28 * 7) nucleus = 'ㅕ';
            else if (((syllable - '가') % 588) < 28 * 8) nucleus = 'ㅖ';
            else if (((syllable - '가') % 588) < 28 * 9) nucleus = 'ㅗ';
            else if (((syllable - '가') % 588) < 28 * 10) nucleus = 'ㅘ';
            else if (((syllable - '가') % 588) < 28 * 11) nucleus = 'ㅙ';
            else if (((syllable - '가') % 588) < 28 * 12) nucleus = 'ㅚ';
            else if (((syllable - '가') % 588) < 28 * 13) nucleus = 'ㅛ';
            else if (((syllable - '가') % 588) < 28 * 14) nucleus = 'ㅜ';
            else if (((syllable - '가') % 588) < 28 * 15) nucleus = 'ㅝ';
            else if (((syllable - '가') % 588) < 28 * 16) nucleus = 'ㅞ';
            else if (((syllable - '가') % 588) < 28 * 17) nucleus = 'ㅟ';
            else if (((syllable - '가') % 588) < 28 * 18) nucleus = 'ㅠ';
            else if (((syllable - '가') % 588) < 28 * 19) nucleus = 'ㅡ';
            else if (((syllable - '가') % 588) < 28 * 20) nucleus = 'ㅢ';
            else if (((syllable - '가') % 588) < 28 * 21) nucleus = 'ㅣ';

            if (((syllable - '가') % 28) == 1) coda = 'ㄱ';
            else if (((syllable - '가') % 28) == 2) coda = 'ㄲ';
            else if (((syllable - '가') % 28) == 3) coda = 'ㄳ';
            else if (((syllable - '가') % 28) == 4) coda = 'ㄴ';
            else if (((syllable - '가') % 28) == 5) coda = 'ㄵ';
            else if (((syllable - '가') % 28) == 6) coda = 'ㄶ';
            else if (((syllable - '가') % 28) == 7) coda = 'ㄷ';
            else if (((syllable - '가') % 28) == 8) coda = 'ㄹ';
            else if (((syllable - '가') % 28) == 9) coda = 'ㄺ';
            else if (((syllable - '가') % 28) == 10) coda = 'ㄻ';
            else if (((syllable - '가') % 28) == 11) coda = 'ㄼ';
            else if (((syllable - '가') % 28) == 12) coda = 'ㄽ';
            else if (((syllable - '가') % 28) == 13) coda = 'ㄾ';
            else if (((syllable - '가') % 28) == 14) coda = 'ㄿ';
            else if (((syllable - '가') % 28) == 15) coda = 'ㅀ';
            else if (((syllable - '가') % 28) == 16) coda = 'ㅁ';
            else if (((syllable - '가') % 28) == 17) coda = 'ㅂ';
            else if (((syllable - '가') % 28) == 18) coda = 'ㅄ';
            else if (((syllable - '가') % 28) == 19) coda = 'ㅅ';
            else if (((syllable - '가') % 28) == 20) coda = 'ㅆ';
            else if (((syllable - '가') % 28) == 21) coda = 'ㅇ';
            else if (((syllable - '가') % 28) == 22) coda = 'ㅈ';
            else if (((syllable - '가') % 28) == 23) coda = 'ㅊ';
            else if (((syllable - '가') % 28) == 24) coda = 'ㅋ';
            else if (((syllable - '가') % 28) == 25) coda = 'ㅌ';
            else if (((syllable - '가') % 28) == 26) coda = 'ㅍ';
            else if (((syllable - '가') % 28) == 27) coda = 'ㅎ';
        }
        private List<int> SyllableToIndexMaking(char syllable)
        {
            List<int> indexList = new List<int>();

            if (syllable < '까') indexList.Add(0);
            else if (syllable < '나') indexList.Add(1);
            else if (syllable < '다') indexList.Add(2);
            else if (syllable < '따') indexList.Add(3);
            else if (syllable < '라') indexList.Add(4);
            else if (syllable < '마') indexList.Add(5);
            else if (syllable < '바') indexList.Add(6);
            else if (syllable < '빠') indexList.Add(7);
            else if (syllable < '사') indexList.Add(8);
            else if (syllable < '싸') indexList.Add(9);
            else if (syllable < '아') indexList.Add(10);
            else if (syllable < '자') indexList.Add(11);
            else if (syllable < '짜') indexList.Add(12);
            else if (syllable < '차') indexList.Add(13);
            else if (syllable < '카') indexList.Add(14);
            else if (syllable < '타') indexList.Add(15);
            else if (syllable < '파') indexList.Add(16);
            else if (syllable < '하') indexList.Add(17);
            else if (syllable < '\uD7A4') indexList.Add(18);

            if (((syllable - '가') % 588) < 28 * 1) indexList.Add(19);
            else if (((syllable - '가') % 588) < 28 * 2) indexList.Add(20);
            else if (((syllable - '가') % 588) < 28 * 3) indexList.Add(21);
            else if (((syllable - '가') % 588) < 28 * 4) indexList.Add(22);
            else if (((syllable - '가') % 588) < 28 * 5) indexList.Add(23);
            else if (((syllable - '가') % 588) < 28 * 6) indexList.Add(24);
            else if (((syllable - '가') % 588) < 28 * 7) indexList.Add(25);
            else if (((syllable - '가') % 588) < 28 * 8) indexList.Add(26);
            else if (((syllable - '가') % 588) < 28 * 9) indexList.Add(27);
            else if (((syllable - '가') % 588) < 28 * 10) indexList.Add(28);
            else if (((syllable - '가') % 588) < 28 * 11) indexList.Add(29);
            else if (((syllable - '가') % 588) < 28 * 12) indexList.Add(30);
            else if (((syllable - '가') % 588) < 28 * 13) indexList.Add(31);
            else if (((syllable - '가') % 588) < 28 * 14) indexList.Add(32);
            else if (((syllable - '가') % 588) < 28 * 15) indexList.Add(33);
            else if (((syllable - '가') % 588) < 28 * 16) indexList.Add(34);
            else if (((syllable - '가') % 588) < 28 * 17) indexList.Add(35);
            else if (((syllable - '가') % 588) < 28 * 18) indexList.Add(36);
            else if (((syllable - '가') % 588) < 28 * 19) indexList.Add(37);
            else if (((syllable - '가') % 588) < 28 * 20) indexList.Add(38);
            else if (((syllable - '가') % 588) < 28 * 21) indexList.Add(39);

            if (((syllable - '가') % 28) == 1) indexList.Add(40);
            else if (((syllable - '가') % 28) == 2) indexList.Add(41);
            else if (((syllable - '가') % 28) == 3) indexList.Add(42);
            else if (((syllable - '가') % 28) == 4) indexList.Add(43);
            else if (((syllable - '가') % 28) == 5) indexList.Add(44);
            else if (((syllable - '가') % 28) == 6) indexList.Add(45);
            else if (((syllable - '가') % 28) == 7) indexList.Add(46);
            else if (((syllable - '가') % 28) == 8) indexList.Add(47);
            else if (((syllable - '가') % 28) == 9) indexList.Add(48);
            else if (((syllable - '가') % 28) == 10) indexList.Add(49);
            else if (((syllable - '가') % 28) == 11) indexList.Add(50);
            else if (((syllable - '가') % 28) == 12) indexList.Add(51);
            else if (((syllable - '가') % 28) == 13) indexList.Add(52);
            else if (((syllable - '가') % 28) == 14) indexList.Add(53);
            else if (((syllable - '가') % 28) == 15) indexList.Add(54);
            else if (((syllable - '가') % 28) == 16) indexList.Add(55);
            else if (((syllable - '가') % 28) == 17) indexList.Add(56);
            else if (((syllable - '가') % 28) == 18) indexList.Add(57);
            else if (((syllable - '가') % 28) == 19) indexList.Add(58);
            else if (((syllable - '가') % 28) == 20) indexList.Add(59);
            else if (((syllable - '가') % 28) == 21) indexList.Add(60);
            else if (((syllable - '가') % 28) == 22) indexList.Add(61);
            else if (((syllable - '가') % 28) == 23) indexList.Add(62);
            else if (((syllable - '가') % 28) == 24) indexList.Add(63);
            else if (((syllable - '가') % 28) == 25) indexList.Add(64);
            else if (((syllable - '가') % 28) == 26) indexList.Add(65);
            else if (((syllable - '가') % 28) == 27) indexList.Add(66);
                        
            return indexList;
        }
        private List<int> WordIndexMaking(string word, bool useNLF)
        {
            List<int> indexList = new List<int>();

            for(int syllableIndex=0;syllableIndex<word.Length;syllableIndex++)
            {
                List<int> nlfIndex = SyllableToIndexMaking(word[syllableIndex]);
                List<int> lfIndex = new List<int>();
                foreach(int index in nlfIndex)
                {
                    if (useNLF) lfIndex.Add(index + 67 * (syllableIndex + 1));
                    else lfIndex.Add(index + 67 * syllableIndex);
                }
                if (useNLF) indexList.AddRange(nlfIndex);
                indexList.AddRange(lfIndex);
            }

            return indexList;
        }
        private string PatternMaking(List<int> indexList, int maxSyllable, bool useNLF)
        {
            int[] patternArray = new int[maxSyllable*67];
            if (useNLF) patternArray = new int[(maxSyllable + 1) * 67];
            for(int index=0;index<patternArray.Length;index++) patternArray[index] = 0;
            foreach (int index in indexList) patternArray[index] = 1;
            StringBuilder newPattern = new StringBuilder();
            foreach (int pattern in patternArray) newPattern.Append(pattern + " ");

            return newPattern.Remove(newPattern.Length-1,1).ToString();
        }
        private int LetterCount(char syllable)
        {
            if (((syllable - '가') % 28) == 0) return 2;
            else return 3;
        }
    }

    class WordData
    {
        public WordData()
        {
            IndexList = new List<int>();
        }

        public string Word
        {
            get;
            set;
        }
        public List<int> IndexList
        {
            get;
            set;
        }
        public string Pattern
        {
            get;
            set;
        }
    }
}
