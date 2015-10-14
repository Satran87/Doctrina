using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using DataTable = System.Data.DataTable;

namespace Doctrina
{
    public partial class Form1 : Form
    {

        private const string colunmNameFileName = "Имя файла";
        private const string colunmNameRepeat = "Повторы";
        private const string colunmNamelPrintTime = "Время последней печати";
        private const string fileNameListText = "FileList.csv";
        private const string ConfigIniFileName = "config.ini";
        private const string DateThenAllowPrintPickerString = "Data=";
        private const string NumberOfListsString = "NumberOfLists=";
        private const string MaxQuestionOnListString = "MaxQuestionOnList=";
        private const string MaxQuestionRepeatString = "MaxQuestionRepeat=";

        private List<List<DoneBlock>> allQuestionsGlobal = new List<List<DoneBlock>>();

        private FileHlp wordHlp = new FileHlp();

        public uint MaxQuestionOnListUint
        {
            get { return _maxQuestionOnListUint; }
            set
            {
                if (value <= 0)
                {
                    _maxQuestionOnListUint = 1;
                    MaxQuestionOnListText.Text = "1";
                }
                _maxQuestionOnListUint = value;
            }
        }

        public uint MaxQuestonRepeatUint
        {
            get { return _maxQuestonRepeatUint; }
            set
            {
                if (value <= 0)
                {
                    _maxQuestonRepeatUint = 1;
                    MaxQuestionRepeatText.Text = "1";
                }
                _maxQuestonRepeatUint = value;
            }
        }

        public uint MaxLists
        {
            get { return _maxLists; }
            set
            {
                if (value <= 0)
                {
                    _maxLists = 1;
                    NumberOfLists.Text = "1";
                }
                _maxLists = value;
            }
        }
        private uint _maxQuestionOnListUint = 5;
        private uint _maxQuestonRepeatUint = 5;
        private uint _maxLists = 5;
        private DateTime dateThenAllowPrint;
        private DataTable myDT;
        private string _chooseFolderPath = null;
        private List<DoneBlock> doneBlocks = new List<DoneBlock>();

        public Form1()
        {
            CheckPassword();
            InitializeComponent();
            wordHlp.MyEvent += MyEventHendler;
            backgroundWorker1.DoWork+=BackgroundWorker1OnDoWork;
            backgroundWorker2.DoWork += BackgroundWorker2_DoWork;
            cancelButton.Enabled = false;
            InitMyComponents();
        }



        private void InitMyComponents()
        {
            if (!File.Exists("config.ini"))
            {
                OnErrorHappen("Файл config.ini не найден. Запуск невозможен");
                Environment.Exit(0);
            }
                
                var lines = File.ReadAllLines("config.ini", Encoding.Default);
            foreach (var line in lines)
            {
                if (line.Contains(DateThenAllowPrintPickerString))
                {
                    var newLine = line.Replace(DateThenAllowPrintPickerString, "");
                    if (string.IsNullOrEmpty(newLine)) newLine = "2015-01-01";
                    dateThenAllowPrintPicker.Value = Convert.ToDateTime(newLine);
                    continue;
                }
                if (line.Contains(NumberOfListsString))
                {
                    var newLine = line.Replace(NumberOfListsString, "");
                    NumberOfLists.Text = newLine;
                    if (string.IsNullOrEmpty(newLine)) newLine = "1";
                    MaxLists =Convert.ToUInt32(newLine);
                    continue;
                }
                if (line.Contains(MaxQuestionOnListString))
                {
                    var newLine = line.Replace(MaxQuestionOnListString, "");
                    MaxQuestionOnListText.Text = newLine;
                    if (string.IsNullOrEmpty(newLine)) newLine = "1";
                    MaxQuestionOnListUint = Convert.ToUInt32(newLine);
                    continue;
                }
                if (line.Contains(MaxQuestionRepeatString))
                {
                    var newLine = line.Replace(MaxQuestionRepeatString, "");
                    MaxQuestionRepeatText.Text = newLine;
                    if (string.IsNullOrEmpty(newLine)) newLine = "1";
                    MaxQuestonRepeatUint = Convert.ToUInt32(newLine);
                    continue;
                }
            }
        }


        private void BackgroundWorker1OnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            try
            {
                allQuestionsGlobal.Clear();
                SetTextinThread("Получаем список");
                GetBlocks(ref allQuestionsGlobal);
                SetTextinThread("Создаем файлы для печати");
                CreateDocxFiles(allQuestionsGlobal);
                SetTextinThread("Печатаем");
                if (allCheckBox.Checked)
                {
                    PrintAll(wordHlp.GetTempDirectory());
                }
                else if (answerCheckBox.Checked)
                {
                    PrintAnswer(wordHlp.GetTempDirectory());
                    SetBoolInThread(true);
                }
                else if (questionCheckBox.Checked)
                {
                    PrintQuestion(wordHlp.GetTempDirectory());
                    SetBoolInThread(true);
                }
                SetTextinThread("");
            }
            catch (Exception e)
            {
                ErrorLog.AddNewEntry(e.Message);
                if (OnErrorHappenYesNo("Произошла ошибка " + e.Message + " Закрыть программу?"))
                {
                    Environment.Exit(0);
                }
            }
            
        }

        private void BackgroundWorker2_DoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            try
            {
                SetTextinThread("Печатаем");
                PrintAll(wordHlp.GetTempDirectory());
                SetTextinThread("");
            }
            catch (Exception e)
            {
                ErrorLog.AddNewEntry(e.Message);
                if (OnErrorHappenYesNo("Произошла ошибка " + e.Message + " Закрыть программу?"))
                {
                    Environment.Exit(0);
                }
            }
            
        }

        private void chooseFolderButton_Click(object sender, EventArgs e)
        {

            var dlgResult = folderDialog.ShowDialog();
            if (!string.IsNullOrEmpty(_chooseFolderPath))
            {
                SaveDtToFile(_chooseFolderPath);
                SaveInit();
            }
            if (dlgResult == DialogResult.OK)
            {
                _chooseFolderPath = folderDialog.SelectedPath;
                currentFolderTextBox.Text = _chooseFolderPath;
                RunButton.Enabled = true;
                wordHlp.DeleteAllFilesOnTempDirectory();
                PrintLastButton.Visible = false;
                doneBlocks.Clear();
                allQuestionsGlobal.Clear();
                GetAllFileFromFolder(_chooseFolderPath);
                FillList();
            }
            
        }

        private void GetAllFileFromFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return;
            bool hasNewFiles = true;
            bool hasFileList = HasFileList(folderPath);
            if (hasFileList)//Грузить из файла иначе из папки
            {
                hasNewFiles = LoadFromFile(folderPath);
            }

            if(hasNewFiles)
            {
                var allAnswerFiles = Directory.GetFiles(folderPath, "*Р.docx", SearchOption.TopDirectoryOnly);
                var allQuestionFiles = Directory.GetFiles(folderPath, "*У.docx*", SearchOption.TopDirectoryOnly);
                if (allQuestionFiles.Count() != allAnswerFiles.Count())
                {
                    OnErrorHappen("Количество вопросов и ответов не совпадает");
                    return; //Потом кидать exception
                }
                    
                foreach (var answerFile in allAnswerFiles)
                {
                    var fileName = answerFile;
                    fileName = fileName.Replace("Р.docx", "У.docx");
                    if (allQuestionFiles.Contains(fileName))
                    {
                        var newBlock = new DoneBlock(fileName, answerFile, new DateTime(1900, 1, 1));
                        IEnumerable<string> existBlock = from block in doneBlocks
                            where block.AnswerPath == answerFile
                            select block.AnswerPath;
                        if (!existBlock.Contains(answerFile))
                        {
                            doneBlocks.Add(newBlock);
                        }
                    }
                }
            } 
        }

        private bool LoadFromFile(string folderPath)
        {
            bool hasNewFiles = true;
            var fileList = folderPath + @"\" + fileNameListText;
            var readFile = File.ReadAllLines(fileList, Encoding.GetEncoding("windows-1251"));
            var realFilesCount = Directory.GetFiles(folderPath, "*Р.docx", SearchOption.TopDirectoryOnly);
            if ((readFile.Count() - 1) == realFilesCount.Count())
                hasNewFiles = false;
            foreach (var line in readFile.Skip(1))
            {
                var lines = line.Split(';');
                var answerFullName = folderPath + @"\" + lines[0] + "Р.docx";
                var questionFullName = answerFullName.Replace("Р.docx", "У.docx");
                var repeatTime = Convert.ToUInt32(lines[1]);
                var lPrintTime = Convert.ToDateTime(lines[2]);
                doneBlocks.Add(new DoneBlock(questionFullName, answerFullName, lPrintTime, repeatTime));
            }
            return hasNewFiles;
        }

        private void FillList()
        {
            myDT.Clear();
            foreach (var block in doneBlocks)
            {
                addRowToDT(block.ShortFileName, block.TimeRepeated, block.LastTimePrint);
            }
            colorTable();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            //TODO:Проверку на то, что не зациклюсь (повторов не перебор)
            cancelButton.Enabled = true;
            dateThenAllowPrint = dateThenAllowPrintPicker.Value;

            try
            {
                MaxQuestionOnListUint = Convert.ToUInt32(MaxQuestionOnListText.Text);
                MaxQuestonRepeatUint = Convert.ToUInt32(MaxQuestionRepeatText.Text);
                MaxLists = Convert.ToUInt32(NumberOfLists.Text);
            }
            catch (Exception)
            {
                OnErrorHappen("Проверьте правильность вводимых цифр");
                return;
            }


            if (MaxLists > MaxQuestionOnListUint*MaxQuestonRepeatUint*doneBlocks.Count)
            {
                OnErrorHappen("Листов больше чем возможных комбинаций");
                return;
            }
            if (MaxQuestonRepeatUint > doneBlocks.Count)
            {
                OnErrorHappen("Количество запросов на страницу слишком большое");
                return;
            }
            if (MaxQuestionOnListUint > doneBlocks.Count)
            {
                OnErrorHappen("Количество вопросов на документ больше, чем самих вопросов");
                return;
            }
            if (RepeatCheck())
            {
                OnErrorHappen("Количество доступных повторов документа слишком мало");
                return;
            }
            //if (IsQuestionEnAllList())
           // {
           //     OnErrorHappen("Количество доступных вопросов на все документы меньше, чем запрошенно");
           //     return;
          //  }
            timer1.Start();
            progressBar1.Style = ProgressBarStyle.Marquee;
            backgroundWorker1.RunWorkerAsync();     
        }

        private bool IsQuestionEnAllList()//Есть ли вопросы для печати доступные на все страницы
        {
            List<int> rowIndex = new List<int>();
            int currentIndex = 0;
            foreach (DataGridViewRow row in datagridForDataTable.Rows)
            {
                if ((uint)row.Cells[1].Value >= MaxQuestonRepeatUint)
                    if ((DateTime)row.Cells[2].Value <= dateThenAllowPrintPicker.Value)
                        rowIndex.Add(currentIndex);
                ++currentIndex;
            }
            if (datagridForDataTable.Rows.Count - rowIndex.Count >= MaxLists* MaxQuestonRepeatUint)//BUG:Кол-во доступых(сейчас на одну страницу, а надо на все)-всего надо
                return false;
            return true;
        }

        private bool RepeatCheck()
        {
            uint totalRepeat = doneBlocks.Aggregate<DoneBlock, uint>(0, (current, block) => current + block.TimeRepeated);
            var needRepeat = MaxLists*MaxQuestonRepeatUint*MaxQuestionOnListUint;
            if (totalRepeat <= needRepeat)
                return false;
            return true;
        }
        private void CreateDocxFiles(List<List<DoneBlock>> allQuestions)
        {
            try
            {
                foreach (var questionBlockList in allQuestions)
                {
                    wordHlp.CreateFilesFromArray(questionBlockList);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.AddNewEntry(exception.Message);
            }
            
        }

        private void GetBlocks(ref List<List<DoneBlock>> allQuestions)
        {
            uint someTimer = 0;
            uint bannedSymbolMeets = 0;
            for (int listNumber = 0; listNumber < MaxLists;)
            {
                List<DoneBlock> uniqueQuestion = new List<DoneBlock>();
                for (int questions = 0; questions < MaxQuestionOnListUint;)
                {
                    var randBlock = RandomNumber.Between(0, doneBlocks.Count-1);
                    if (!uniqueQuestion.Contains(doneBlocks[randBlock]))
                    {
                        if (doneBlocks[randBlock].AllowPrint(MaxQuestonRepeatUint, dateThenAllowPrint))
                        {
                            if (doneBlocks[randBlock].AnswerPath.Contains('!'))
                            {
                                ++bannedSymbolMeets;
                                if (bannedSymbolMeets >= 10)//Если уже встречался 10 раз, так и быть, пустим.
                                {
                                    bannedSymbolMeets = 0;
                                    uniqueQuestion.Add(doneBlocks[randBlock]);
                                    ++questions;
                                }
                            }
                            else
                            {
                                uniqueQuestion.Add(doneBlocks[randBlock]);
                                ++questions;
                            }
                        }
                    }
                    if (someTimer >= 60000)
                    {
                        if(OnErrorHappenYesNo("Ошибка комбинации возможных вариантов.Создаем дамп?"))
                            ErrorLog.CreateDump();
                        ErrorLog.AddNewEntry("Вопросов_на_лист="+MaxQuestionOnListUint+" | Макс_повторов_вопросов= "+ MaxQuestonRepeatUint+ " | Всего_листов= " + MaxLists);
                        File.Copy(_chooseFolderPath + @"\" + fileNameListText,"ErrorList"+DateTime.Now.Day+DateTime.Now.Hour+DateTime.Now.Minute+".csv");
                        goto exit;

                    }
                    ++someTimer;
                }
                allQuestions.Add(uniqueQuestion);
                ++listNumber;
            }
            exit:;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            wordHlp.CloseWord();
            SaveInit();
            if (!string.IsNullOrEmpty(_chooseFolderPath))
            {
                SaveDtToFile(_chooseFolderPath);
            }
            wordHlp.DeleteAllFilesOnTempDirectory();
        }

        private void MyEventHendler(object sender, FileHlpArgs e)
        {
           // progressBar1.Value = howManyDocxWasMade;
        }

        private void CreateDT()
        {
            myDT = new DataTable();
            System.Type typeString = System.Type.GetType("System.String");
            System.Type typeUint = System.Type.GetType("System.UInt32");
            System.Type typeDateTime = System.Type.GetType("System.DateTime");
            DataColumn columnFileName = new DataColumn(colunmNameFileName, typeString);
            DataColumn columnRepeat = new DataColumn(colunmNameRepeat, typeUint);
            DataColumn columnLastPrint = new DataColumn(colunmNamelPrintTime, typeDateTime);
            columnFileName.ReadOnly = true;
            columnLastPrint.ReadOnly = true;
            columnRepeat.ReadOnly = false;
            myDT.Columns.Add(columnFileName);
            myDT.Columns.Add(columnRepeat);
            myDT.Columns.Add(columnLastPrint);
            datagridForDataTable.DataSource = myDT;
        }

        private void addRowToDT(string fileName, uint repeat, DateTime lPrintTime)
        {
            var row = myDT.NewRow();
            row[colunmNameFileName] = fileName;
            row[colunmNameRepeat] = repeat;
            row[colunmNamelPrintTime] = lPrintTime;
            myDT.Rows.Add(row);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateDT();
        }

        private void datagridForDataTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.BindingContext[(DataTable) datagridForDataTable.DataSource].EndCurrentEdit();
            var tempDT = ((DataTable) datagridForDataTable.DataSource).GetChanges();
            if (tempDT != null)
            {
                myDT.Clear();
                myDT = tempDT;
                ((DataTable) datagridForDataTable.DataSource).AcceptChanges();
            }
            datagridForDataTable.DataSource = myDT;
            colorTable();
            if (!string.IsNullOrEmpty(_chooseFolderPath))
            {
                SaveDtToFile(_chooseFolderPath);
                SaveInit();
            }
        }

        private void SaveDtToFile(string folderPath)
        {
            if (myDT.Rows.Count>1)
            {
                folderPath = folderPath + @"\" + fileNameListText;
                if (!File.Exists(folderPath))
                {
                    File.Create(folderPath).Close();
                }

                StringBuilder sb = new StringBuilder();
                IEnumerable<string> columnNames = myDT.Columns.Cast<DataColumn>().
                    Select(column => column.ColumnName);
                sb.AppendLine(string.Join(";", columnNames));
                foreach (DataRow row in myDT.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());

                    sb.AppendLine(string.Join(";", fields));
                }
                try
                {
                    File.WriteAllText(folderPath, sb.ToString(), Encoding.GetEncoding("windows-1251"));
                }
                catch (Exception e)
                {

                }
            }
        }

        private bool HasFileList(string folderPath)
        {
            folderPath = folderPath +@"\"+ fileNameListText;
            if (File.Exists(folderPath))
                return true;
            return false;
        }

        private void PrintQuestion(string folderPath)
        {
            var allQuestionFiles = Directory.GetFiles(folderPath, "*Ques.docx", SearchOption.TopDirectoryOnly);

            foreach (var file in allQuestionFiles)
            {
                wordHlp.Print(file);
            }
        }

        private void PrintAnswer(string folderPath)
        {
            var allQuestionFiles = Directory.GetFiles(folderPath, "*Ans.docx", SearchOption.TopDirectoryOnly);

            foreach (var file in allQuestionFiles)
            {
                if(file.Contains('~')) continue;
                wordHlp.Print(file);
            }
        }

        private void PrintAll(string folderPath)
        {
            PrintQuestion(folderPath);
            PrintAnswer(folderPath);
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Marquee;
            if (backgroundWorker1.IsBusy || backgroundWorker2.IsBusy)
            {
                RunButton.Enabled = false;
                chooseFolderButton.Enabled = false;

                MaxQuestionOnListText.Enabled = false;
                MaxQuestionRepeatText.Enabled = false;
                NumberOfLists.Enabled = false;
                dateThenAllowPrintPicker.Enabled = false;
                allCheckBox.Enabled = false;
                datagridForDataTable.Enabled = false;
            }
            else
            {
                RunButton.Enabled = true;
                chooseFolderButton.Enabled = true;

                MaxQuestionOnListText.Enabled = true;
                MaxQuestionRepeatText.Enabled = true;
                NumberOfLists.Enabled = true;
                dateThenAllowPrintPicker.Enabled = true;
                allCheckBox.Enabled = true;
                datagridForDataTable.Enabled = true;

                cancelButton.Enabled = false;
                progressBar1.Style = ProgressBarStyle.Blocks;
                FillList();
                timer1.Stop();
            }
        }

        private void allCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (allCheckBox.Checked)
            {
                questionCheckBox.Checked = true;
                answerCheckBox.Checked = true;
                questionCheckBox.Enabled = false;
                answerCheckBox.Enabled = false;
            }
            else
            {
                questionCheckBox.Enabled = true;
                answerCheckBox.Enabled = true;
            }
        }

        private void OnErrorHappen(string text)
        {
            MessageBox.Show(text, "Произошла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ErrorLog.AddNewEntry(text);
        }
        private bool OnErrorHappenYesNo(string text)
        {
           var code= MessageBox.Show(text, "Произошла ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            ErrorLog.AddNewEntry(text);
            if (code == DialogResult.Yes)
                return true;
            return false;
        }

        private void SaveInit()
        {
            if (File.Exists(ConfigIniFileName))
            {
               var lines= File.ReadAllLines(ConfigIniFileName, Encoding.GetEncoding("windows-1251"));
                for(var t=0;t<lines.Count();++t)
                {
                    if (lines[t].Contains(DateThenAllowPrintPickerString))
                    {
                        lines[t] = DateThenAllowPrintPickerString + dateThenAllowPrintPicker.Value;
                        continue;
                    }
                    if (lines[t].Contains(NumberOfListsString))
                    {
                        lines[t] = NumberOfListsString + NumberOfLists.Text;
                        continue;
                    }
                    if (lines[t].Contains(MaxQuestionOnListString))
                    {
                        lines[t] = MaxQuestionOnListString + MaxQuestionOnListText.Text;
                        continue;
                    }
                    if (lines[t].Contains(MaxQuestionRepeatString))
                    {
                        lines[t] = MaxQuestionRepeatString + MaxQuestionRepeatText.Text;
                        continue;
                    }
                }
                using (var sw = new StreamWriter(ConfigIniFileName, false))
                {
                    foreach (var line in lines)
                    {
                        sw.WriteLine(line);
                    }
                    sw.Close();
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            allQuestionsGlobal.Clear();

        }

        delegate void SetTextCallback(string text);
        delegate void SetBoolCallback(bool val);
        private void SetTextinThread(string text)
        {
            if (currentStatusTextBox.InvokeRequired)
            {
                SetTextCallback cb = SetTextinThread;
                Invoke(cb, text);
            }
            else
            {
                currentStatusTextBox.Text = text;
            }
        }

        private void SetBoolInThread(bool val)
        {
            if (answerCheckBox.InvokeRequired)
            {
                SetBoolCallback cb = SetBoolInThread;
                Invoke(cb, val);
            }
            else
            {
                PrintLastButton.Visible = val;
            }
            if (questionCheckBox.InvokeRequired)
            {
                SetBoolCallback cb = SetBoolInThread;
                Invoke(cb, val);
            }
            else
            {
                PrintLastButton.Visible = val;
            }
        }

        private void colorTable()
        {
            List<int> rowIndex=new List<int>();
            int currentIndex = 0;
            foreach (DataGridViewRow row in datagridForDataTable.Rows)
            {
               if((uint)row.Cells[1].Value>= MaxQuestonRepeatUint)
                    if((DateTime)row.Cells[2].Value <= dateThenAllowPrintPicker.Value)
                    rowIndex.Add(currentIndex);
                ++currentIndex;
            }
            foreach (var index in rowIndex)
            {
                datagridForDataTable.Rows[index].DefaultCellStyle.BackColor=Color.LightSlateGray;
            }
            for(int t=0;t<datagridForDataTable.RowCount;++t)
            {
                if(!rowIndex.Contains(t))
                datagridForDataTable.Rows[t].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void NumberOfLists_TextChanged(object sender, EventArgs e)
        {
            MaxLists=string.IsNullOrEmpty(NumberOfLists.Text) ? 0: Convert.ToUInt32(NumberOfLists.Text);
        }

        private void MaxQuestionRepeatText_TextChanged(object sender, EventArgs e)
        {
            MaxQuestonRepeatUint = string.IsNullOrEmpty(MaxQuestionRepeatText.Text) ? 0 : Convert.ToUInt32(MaxQuestionRepeatText.Text);
            colorTable();
        }

        private void MaxQuestionOnListText_TextChanged(object sender, EventArgs e)
        {
            MaxQuestionOnListUint = string.IsNullOrEmpty(MaxQuestionOnListText.Text) ? 0 : Convert.ToUInt32(MaxQuestionOnListText.Text);
        }

        private void dateThenAllowPrintPicker_ValueChanged(object sender, EventArgs e)
        {

            colorTable();
        }

        private void NumberOfLists_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }

        private void MaxQuestionOnListText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }

        private void MaxQuestionRepeatText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }

        private void PrintLast_Click(object sender, EventArgs e)
        {
            backgroundWorker2.RunWorkerAsync();
            timer1.Start();
            PrintLastButton.Visible = false;
        }

        private void datagridForDataTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (datagridForDataTable.Columns[e.ColumnIndex].HeaderText != colunmNameRepeat)
            {
                ErrorLog.AddNewEntry(e.Exception.Message);
                return;
            }
            OnErrorHappen("Неверное значение в ячейке. Допускается ввод только 0 и положительных цифр");
            ErrorLog.AddNewEntry(e.Exception.Message);
        }

        private void CheckPassword()
        {
            PassCheck myPassCheck = new PassCheck();
            myPassCheck.ShowDialog(this.Owner);
            if (myPassCheck.DialogResult == DialogResult.OK)
            {
                if (!myPassCheck.HaskOk)
                {
                    ErrorLog.AddNewEntry("Введен неверный пароль");
                    Environment.Exit(0);
                }
            }
            else
            {
                ErrorLog.AddNewEntry("Окно ввода пароля закрыто");
                Environment.Exit(0);
            }
           
        }

        private void datagridForDataTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            colorTable();
        }

        private void datagridForDataTable_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            colorTable();
        }
    }
}
