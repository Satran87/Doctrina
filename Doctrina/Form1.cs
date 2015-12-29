﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Doctrina.eEgg;
using Doctrina.Enums;

namespace Doctrina
{
    public partial class Form1 : Form
    {
        private const string ColunmNameFileName = "Имя файла";
        private const string ColunmNameRepeat = "Повторы";
        private const string ColunmNamelPrintTime = "Время последней печати";
        private const string ColunmNamelCheckFile = "Выбрать файл для печати";
        internal const string FileNameListText = "FileList.csv";
        private const string ConfigIniFileName = "config.ini";
        private const string DateThenAllowPrintPickerString = "Data=";
        private const string NumberOfListsString = "NumberOfLists=";
        private const string MaxQuestionOnListString = "MaxQuestionOnList=";
        private const string MaxQuestionRepeatString = "MaxQuestionRepeat=";
        private const string BannedSymbolsString = "BannedSymbols=";
        public WorkLikeEnum CurrentWorkEnum=WorkLikeEnum.OnlyGenerator;

        private List<int>CheckedIndex=new List<int>(); 

        public int NumberOfConstFiles = 0;

        internal BannedSymbolsClass NewBannedSymbols1;


        private List<List<DoneBlock>> _allQuestionsGlobal = new List<List<DoneBlock>>();

        private FileHlp _wordHlp = new FileHlp();

        /// <summary>
        /// Количество вопросов на лист
        /// </summary>
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
        /// <summary>
        /// Количество повторов вопроса
        /// </summary>
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
        /// <summary>
        /// Всего листов
        /// </summary>
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
        internal DateTime DateThenAllowPrint;
        internal DataTable MyDt;
        internal string ChooseFolderPath = null;
        /// <summary>
        /// Готовый комплект вопросов загруженный из файла
        /// </summary>
        internal List<DoneBlock> DoneBlocks = new List<DoneBlock>();
        /// <summary>
        /// Копия блоков до запуска. В случае ошибки через них идет отмена.
        /// </summary>
        internal List<DoneBlock> DoneBlocksOldCopy = new List<DoneBlock>();

        public Form1()
        {
            CheckPassword();
            InitializeComponent();
            _wordHlp.MyEvent += MyEventHendler;
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
                if (line.Contains(BannedSymbolsString))
                {
                    var bannedSymbols= line.Replace(BannedSymbolsString, "");
                    NewBannedSymbols1 = new BannedSymbolsClass(bannedSymbols.Split('|'));
                }
            }
        }


        private void BackgroundWorker1OnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            try
            {
                _allQuestionsGlobal.Clear();
                SetTextinThread("Получаем список");
                GetBlocks(ref _allQuestionsGlobal);
                SetTextinThread("Создаем файлы для печати");
                CreateDocxFiles(_allQuestionsGlobal);
                SetTextinThread("Печатаем");
                if (allCheckBox.Checked)
                {
                    PrintAll(_wordHlp.GetTempDirectory());
                }
                else if (answerCheckBox.Checked)
                {
                    PrintAnswer(_wordHlp.GetTempDirectory());
                    SetBoolInThread(true);
                }
                else if (questionCheckBox.Checked)
                {
                    PrintQuestion(_wordHlp.GetTempDirectory());
                    SetBoolInThread(true);
                }
                SetTextinThread("");
                DoneBlocksOldCopy.Clear();
                DoneBlocksOldCopy=CheckClass.CopyDoneBlocks(this,DoneBlocks);
            }
            catch (Exception e)
            {
                ErrorLog.AddNewEntry(e.Message);
                DoneBlocks.Clear();
                DoneBlocks = CheckClass.CopyDoneBlocks(this,DoneBlocksOldCopy);
                _wordHlp.CloseWord();
                _wordHlp.DeleteAllFilesOnTempDirectory();
                if (OnErrorHappenYesNo("Произошла ошибка \r\n" + e.Message + "\r\nЗакрыть программу?"))
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
                PrintAll(_wordHlp.GetTempDirectory());
                SetTextinThread("");
            }
            catch (Exception e)
            {
                ErrorLog.AddNewEntry(e.Message);
                _wordHlp.CloseWord();
                if (OnErrorHappenYesNo("Произошла ошибка \r\n" + e.Message + "\r\nЗакрыть программу?"))
                {
                    Environment.Exit(0);
                }
            }
        }

        private void chooseFolderButton_Click(object sender, EventArgs e)
        {
            var dlgResult = folderDialog.ShowDialog();
            if (!string.IsNullOrEmpty(ChooseFolderPath))
            {
                SaveDtToFile(ChooseFolderPath);
                SaveInit();
            }
            if (dlgResult == DialogResult.OK)
            {
                CheckedIndex.Clear();
                ChooseFolderPath = folderDialog.SelectedPath;
                currentFolderTextBox.Text = ChooseFolderPath;
                RunButton.Enabled = true;
                _wordHlp.DeleteAllFilesOnTempDirectory();
                PrintLastButton.Visible = false;
                ReloadData();
            }
        }

        private void ReloadData()
        {
            DoneBlocks.Clear();
            _allQuestionsGlobal.Clear();
            GetAllFileFromFolder(ChooseFolderPath);
            FillList();
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
                var allAnswerFiles = Directory.GetFiles(folderPath, "*Р?.docx", SearchOption.TopDirectoryOnly);
                var allQuestionFiles = Directory.GetFiles(folderPath, "*У?.docx*", SearchOption.TopDirectoryOnly);
                if (allQuestionFiles.Count() != allAnswerFiles.Count())
                {
                    OnErrorHappen("Количество вопросов и ответов не совпадает");
                    RunButton.Enabled = false;
                    return; //Потом кидать exception
                }
                    
                foreach (var answerFile in allAnswerFiles)
                {
                    var newAnsFile = answerFile;
                    var fileName = newAnsFile;
                    fileName = ReplaceEndOfFile(fileName);
                    
                    if (allQuestionFiles.Contains(fileName))
                    {
                        var newBlock = new DoneBlock(this,fileName, newAnsFile, new DateTime(1900, 1, 1));
                        IEnumerable<string> existBlock = from block in DoneBlocks
                            where block.AnswerPath == newAnsFile
                                                         select block.AnswerPath;
                        if (!existBlock.Contains(newAnsFile))
                        {
                            DoneBlocks.Add(newBlock);
                        }
                    }
                }
            } 
        }

        private  string ReplaceEndOfFile(string fileName)
        {
            var shortfileName=Path.GetFileName(fileName);
            var pathToFolder= Path.GetDirectoryName(fileName);
            bool contain = false;
            foreach (var bannedSymbol in NewBannedSymbols1.BannedSymbolsList)
            {
                if (shortfileName.Contains(bannedSymbol))
                {
                    shortfileName = shortfileName.Replace("Р" + bannedSymbol + ".docx",
                   "У" + bannedSymbol + ".docx");
                    contain = true;
                }
            }
            if (!contain)
            {
                shortfileName = shortfileName.Replace("Р.docx", "У.docx");
            }
            return pathToFolder + @"\" + shortfileName;
        }

        private bool LoadFromFile(string folderPath)
        {
            bool hasNewFiles = true;
            var fileList = folderPath + @"\" + FileNameListText;
            string[] readFile;
            try
            {
                readFile = File.ReadAllLines(fileList, Encoding.GetEncoding("windows-1251"));
            }
            catch (Exception e)
            {
                OnErrorHappen(e.Message);
                throw;
            }
            var realFilesCount = Directory.GetFiles(folderPath, "*Р?.docx", SearchOption.TopDirectoryOnly);
            if ((readFile.Count() - 1) == realFilesCount.Count())
                hasNewFiles = false;
            foreach (var line in readFile.Skip(1))
            {
                var lines = line.Split(';');
                string answerFullName=string.Empty;
                bool contain = false;
                foreach (var bannedSymbol in NewBannedSymbols1.BannedSymbolsList)
                {
                    if (lines[0].Contains(bannedSymbol))
                    {
                        lines[0] = lines[0].Remove(lines[0].Length - 1);
                        answerFullName = folderPath + @"\" + lines[0] + "Р" + bannedSymbol + ".docx";
                        contain = true;
                    }
                }
                if(!contain)
                {
                    answerFullName = folderPath + @"\" + lines[0] + "Р.docx";
                }
                var questionFullName = ReplaceEndOfFile(answerFullName);
                if (!File.Exists(questionFullName)|| !File.Exists(answerFullName))
                {
                    continue;
                }
                var repeatTime = Convert.ToUInt32(lines[1]);
                var lPrintTime = Convert.ToDateTime(lines[2]);
                DoneBlocks.Add(new DoneBlock(this,questionFullName, answerFullName, lPrintTime, repeatTime));
            }
            return hasNewFiles;
        }

        private void FillList()
        {
            MyDt.Clear();
            foreach (var block in DoneBlocks)
            {
                addRowToDT(block.ShortFileName, block.TimeRepeated, block.LastTimePrint);
            }
            if (CurrentWorkEnum==WorkLikeEnum.GeneratorAndConst)
            {
                var checkedColumn = MyDt.Columns[ColunmNamelCheckFile];
                foreach (var index in CheckedIndex)
                {
                    MyDt.Rows[index][checkedColumn] = true;
                }
            }
            colorTable();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            cancelButton.Enabled = true;
            DateThenAllowPrint = dateThenAllowPrintPicker.Value;
            DoneBlocksOldCopy.Clear();
            DoneBlocksOldCopy = CheckClass.CopyDoneBlocks(this,DoneBlocks);
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


            if (CheckClass.Cheks(this,currentFolderTextBox.Text)) return;
            timer1.Start();
            progressBar1.Style = ProgressBarStyle.Marquee;
            backgroundWorker1.RunWorkerAsync();     
        }



        private void CreateDocxFiles(List<List<DoneBlock>> allQuestions)
        {
            int counterForFile = 1;
            try
            {
                foreach (var questionBlockList in allQuestions)
                {
                    _wordHlp.CreateFilesFromArray(questionBlockList, counterForFile);
                    ++counterForFile;
                }
            }
            catch (Exception exception)
            {
                OnErrorHappen(exception.Message);
            }
            
        }

        private void GetBlocks(ref List<List<DoneBlock>> allQuestions)
        {
            switch (CurrentWorkEnum)
            {
                case WorkLikeEnum.OnlyGenerator:
                {
                    CheckClass.GetBlocks_1(this, ref DoneBlocks, ref allQuestions);
                    break;
                }
                case WorkLikeEnum.GeneratorAndConst:
                {
                    List<DoneBlock> copyedDoneBlocksWihoutCosnt = null;
                    List<DoneBlock> constBlocks =
                        new List<DoneBlock>(PrepareNewDoneBlockForallQuestion(ref copyedDoneBlocksWihoutCosnt));

                    NumberOfConstFiles = constBlocks.Count();
                    CheckClass.GetBlocks_1(this, ref copyedDoneBlocksWihoutCosnt, ref allQuestions, constBlocks);

                    foreach (var constBlock in constBlocks)
                    {
                        constBlock.TimeRepeated = MaxLists;
                        constBlock.LastTimePrint = DateTime.Now;
                    }
                    break;
                }
                case WorkLikeEnum.GeneratorAndLST:
                {
                    break;
                }
            }
        }

        private List<DoneBlock> PrepareNewDoneBlockForallQuestion(ref List <DoneBlock> doneBlocksWithoutConst)
        {
            var CheckColumn = MyDt.Columns[ColunmNamelCheckFile];
            CheckedIndex = new List<int>();
            int t = 0;
            foreach (DataRow row in MyDt.Rows)
            {
                var columnIsChecked = (bool) row[CheckColumn];
                if (columnIsChecked)
                    CheckedIndex.Add(t);
                ++t;
            }
            List<DoneBlock> constBlocks = new List<DoneBlock>();
            foreach (var index in CheckedIndex)
            {
                constBlocks.Add(DoneBlocks[index]);
            }
            doneBlocksWithoutConst = new List<DoneBlock>(DoneBlocks);
            foreach (var block in constBlocks)
            {
                doneBlocksWithoutConst.Remove(block);
            }
            return constBlocks;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _wordHlp.CloseWord();
            SaveInit();
            if (!string.IsNullOrEmpty(ChooseFolderPath))
            {
                SaveDtToFile(ChooseFolderPath);
            }
            _wordHlp.DeleteAllFilesOnTempDirectory();
        }

        private void MyEventHendler(object sender, FileHlpArgs e)
        {
           // progressBar1.Value = howManyDocxWasMade;
        }

        private void CreateDt()
        {
            MyDt = new DataTable();
            Type typeString = Type.GetType("System.String");
            Type typeUint = Type.GetType("System.UInt32");
            Type typeDateTime = Type.GetType("System.DateTime");
            DataColumn columnFileName = new DataColumn(ColunmNameFileName, typeString);
            DataColumn columnRepeat = new DataColumn(ColunmNameRepeat, typeUint);
            DataColumn columnLastPrint = new DataColumn(ColunmNamelPrintTime, typeDateTime);
            columnFileName.ReadOnly = true;
            columnLastPrint.ReadOnly = true;
            columnRepeat.ReadOnly = false;
            MyDt.Columns.Add(columnFileName);
            MyDt.Columns.Add(columnRepeat);
            MyDt.Columns.Add(columnLastPrint);
            datagridForDataTable.DataSource = MyDt;
        }
        private void CreateDtWithCB()
        {
            MyDt = new DataTable();
            Type typeString = Type.GetType("System.String");
            Type typeUint = Type.GetType("System.UInt32");
            Type typeDateTime = Type.GetType("System.DateTime");
            Type typeBool = Type.GetType("System.Boolean");
            DataColumn columnFileName = new DataColumn(ColunmNameFileName, typeString);
            DataColumn columnRepeat = new DataColumn(ColunmNameRepeat, typeUint);
            DataColumn columnLastPrint = new DataColumn(ColunmNamelPrintTime, typeDateTime);
            DataColumn columnCheckBool = new DataColumn(ColunmNamelCheckFile,typeBool);
            columnFileName.ReadOnly = true;
            columnLastPrint.ReadOnly = true;
            columnRepeat.ReadOnly = false;
            columnCheckBool.ReadOnly = false;
            MyDt.Columns.Add(columnFileName);
            MyDt.Columns.Add(columnRepeat);
            MyDt.Columns.Add(columnLastPrint);
            MyDt.Columns.Add(columnCheckBool);
            datagridForDataTable.DataSource = MyDt;
        }
        /// <summary>
        /// Пересоздать таблицу при смене режима работы
        /// </summary>
        private void ReCreateDT()
        {
            if (CurrentWorkEnum == WorkLikeEnum.GeneratorAndConst)
            {
                CreateDtWithCB();
            }
            else
            {
                CreateDt();
            }
            ReloadData();
        }

        private void addRowToDT(string fileName, uint repeat, DateTime lPrintTime)
        {
            var row = MyDt.NewRow();
            row[ColunmNameFileName] = fileName;
            row[ColunmNameRepeat] = repeat;
            row[ColunmNamelPrintTime] = lPrintTime;
            if (CurrentWorkEnum == WorkLikeEnum.GeneratorAndConst)
            {
                row[ColunmNamelCheckFile] = false;
            }
            MyDt.Rows.Add(row);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateDt();
        }

        //TODO:Переделать механизм - постоянно передергивает. Предложить сохранять?
        private void datagridForDataTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (CurrentWorkEnum == WorkLikeEnum.GeneratorAndConst)
            {
                //if (MyDt.Columns[ColunmNamelCheckFile] != MyDt.Columns[e.ColumnIndex])
                //    return;

                //var CheckColumn = MyDt.Columns[ColunmNamelCheckFile];
                //int t = 0;
                //foreach (DataRow row in MyDt.Rows)
                //{
                //    var columnIsChecked = (bool) row[CheckColumn];
                //    if (columnIsChecked)
                //        ++t;
                //    if (t == 3)
                //    {
                //        MessageBox.Show("Превышено допустимое число выбранных вручную вопросов");
                //        // MyDt.Rows[e.RowIndex][e.ColumnIndex] = false;
                //        MyDt.RejectChanges();
                //        return;
                //    }
                //}
                //MyDt.AcceptChanges();
                //((DataTable) datagridForDataTable.DataSource).AcceptChanges();
            }
            else
            {
                BindingContext[(DataTable) datagridForDataTable.DataSource].EndCurrentEdit();
                var tempDT = ((DataTable) datagridForDataTable.DataSource).GetChanges();
                if (tempDT != null)
                {
                    MyDt.Clear();
                    MyDt = tempDT;
                    ((DataTable) datagridForDataTable.DataSource).AcceptChanges();
                }
                datagridForDataTable.DataSource = MyDt;
                colorTable();
                if (!string.IsNullOrEmpty(ChooseFolderPath))
                {
                    SaveDtToFile(ChooseFolderPath);
                    SaveInit();
                    ReloadData();
                }
            }
        }

        private void datagridForDataTable_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (CurrentWorkEnum == WorkLikeEnum.GeneratorAndConst)
            {
                //if (MyDt.Columns[ColunmNamelCheckFile] != MyDt.Columns[e.ColumnIndex])
                //    return;
                //    var CheckColumn = MyDt.Columns[ColunmNamelCheckFile];
                //    int t = 0;
                //    foreach (DataRow row in MyDt.Rows)
                //    {
                //        var columnIsChecked = (bool)row[CheckColumn];
                //        if (columnIsChecked)
                //            ++t;
                //        if (t >= 3)
                //        {
                //            MessageBox.Show("Превышено допустимое число выбранных вручную вопросов");
                //            e.Cancel = true;
                //            break;
                //        }
                //    }
            }
        }

        private void SaveDtToFile(string folderPath)
        {
            if (MyDt.Rows.Count>1)
            {
                folderPath = folderPath + @"\" + FileNameListText;
                if (!File.Exists(folderPath))
                {
                    File.Create(folderPath).Close();
                }

                StringBuilder sb = new StringBuilder();
                IEnumerable<string> columnNames = MyDt.Columns.Cast<DataColumn>().
                    Select(column => column.ColumnName);
                sb.AppendLine(string.Join(";", columnNames));
                foreach (DataRow row in MyDt.Rows)
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
                    OnErrorHappen(e.Message);
                }
            }
        }

        private bool HasFileList(string folderPath)
        {
            folderPath = folderPath +@"\"+ FileNameListText;
            if (File.Exists(folderPath))
                return true;
            return false;
        }

        private void PrintQuestion(string folderPath)
        {
            var allQuestionFiles = Directory.GetFiles(folderPath, "*Ques.docx", SearchOption.TopDirectoryOnly);
            FileHlp.NumericalSort(allQuestionFiles);
            foreach (var file in allQuestionFiles)
            {
                if (file.Contains('~')) continue;
                _wordHlp.Print(file);
            }
        }

        private void PrintAnswer(string folderPath)
        {
            var allQuestionFiles = Directory.GetFiles(folderPath, "*Ans.docx", SearchOption.TopDirectoryOnly);
            FileHlp.NumericalSort(allQuestionFiles);
            foreach (var file in allQuestionFiles)
            {
                if(file.Contains('~')) continue;
                _wordHlp.Print(file);
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
                PrintLastButton.Visible = false;
                MaxQuestionOnListText.Enabled = false;
                MaxQuestionRepeatText.Enabled = false;
                NumberOfLists.Enabled = false;
                dateThenAllowPrintPicker.Enabled = false;
                allCheckBox.Enabled = false;
                answerCheckBox.Enabled = false;
                questionCheckBox.Enabled = false;
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
                answerCheckBox.Enabled = true;
                questionCheckBox.Enabled = true;
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

        public void OnErrorHappen(string text)
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
                        lines[t] = DateThenAllowPrintPickerString + DateThenAllowPrint;
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
            _allQuestionsGlobal.Clear();

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
                    if((DateTime)row.Cells[2].Value <= DateThenAllowPrint)
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
            //
            if (!MaxQuestionOnListSecond && !MaxQuestionRepeatThird)
            {
                ListFirst = true;
            }
            else
            {
                ListFirst = false;
                MaxQuestionOnListSecond = false;
                MaxQuestionRepeatThird = false;
            }
        }
        #region eEgg

        private bool ListFirst = false;
        private bool MaxQuestionOnListSecond = false;
        private bool MaxQuestionRepeatThird = false;
        private bool played = false;
        #endregion
        private void MaxQuestionRepeatText_TextChanged(object sender, EventArgs e)
        {
            MaxQuestonRepeatUint = string.IsNullOrEmpty(MaxQuestionRepeatText.Text) ? 0 : Convert.ToUInt32(MaxQuestionRepeatText.Text);
            colorTable();
            //
            //
            if (ListFirst && MaxQuestionOnListSecond)
            {
                MaxQuestionRepeatThird = true;
            }
            else
            {
                ListFirst = false;
                MaxQuestionOnListSecond = false;
                MaxQuestionRepeatThird = false;
            }
            ///
            if (!played)
            {
                if (ListFirst && MaxQuestionOnListSecond && MaxQuestionRepeatThird)
                {
                    if (MaxLists == 557)
                    {
                        if (MaxQuestionOnListUint == 30)
                        {
                            if (MaxQuestonRepeatUint == 27)
                            {
                                if (DateThenAllowPrint.Year == 2050)
                                {
                                    EEggPlayer fun = new EEggPlayer();
                                    fun.ShowDialog(Owner);
                                    ErrorLog.AddNewEntry("Это всё же кто-то увидел -))");
                                    played = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void MaxQuestionOnListText_TextChanged(object sender, EventArgs e)
        {
            MaxQuestionOnListUint = string.IsNullOrEmpty(MaxQuestionOnListText.Text) ? 0 : Convert.ToUInt32(MaxQuestionOnListText.Text);
            //
            if (ListFirst && !MaxQuestionRepeatThird)
            {
                MaxQuestionOnListSecond = true;
            }
            else
            {
                ListFirst = false;
                MaxQuestionOnListSecond = false;
                MaxQuestionRepeatThird = false;
            }
        }

        private void dateThenAllowPrintPicker_ValueChanged(object sender, EventArgs e)
        {
            DateThenAllowPrint = dateThenAllowPrintPicker.Value;
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
            if (datagridForDataTable.Columns[e.ColumnIndex].HeaderText != ColunmNameRepeat)
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
            myPassCheck.ShowDialog(Owner);
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

        private void OnlyGeneratorRadioButon_CheckedChanged(object sender, EventArgs e)
        {
            if (OnlyGeneratorRadioButon.Checked)
            {
                WorkLikeChanged(WorkLikeEnum.OnlyGenerator);
            }
        }

        

        private void GeneratorAndConstRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (GeneratorAndConstRadioButton.Checked)
            {
                WorkLikeChanged(WorkLikeEnum.GeneratorAndConst);
            }
        }
        private void WorkLikeChanged(WorkLikeEnum howNow)
        {
            switch (howNow)
            {
                case WorkLikeEnum.OnlyGenerator:
                    {
                        CurrentWorkEnum = howNow;
                        GeneratorAndConstRadioButton.Checked = false;
                        ReCreateDT();
                        break;
                    }

                case WorkLikeEnum.GeneratorAndConst:
                    {
                        CurrentWorkEnum = howNow;
                        OnlyGeneratorRadioButon.Checked = false;
                        ReCreateDT();
                        break;
                    }
                case WorkLikeEnum.GeneratorAndLST:
                    {
                        CurrentWorkEnum = howNow;
                        ReCreateDT();
                        break;
                    }
            }
        }

        private void datagridForDataTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (currentWorkEnum == WorkLikeEnum.GeneratorAndConst)
            //{
            //    if (MyDt.Columns[ColunmNamelCheckFile] != MyDt.Columns[e.ColumnIndex])
            //        return;
            //    var tempDT1 = ((DataTable) datagridForDataTable.DataSource).GetChanges();
            //    if (tempDT1 != null)
            //    {
            //        var CheckColumn = MyDt.Columns[ColunmNamelCheckFile];
            //        int t = 0;
            //        foreach (DataRow row in MyDt.Rows)
            //        {
            //            var columnIsChecked = (bool) row[CheckColumn];
            //            if (columnIsChecked)
            //                ++t;
            //            if (t == 3)
            //            {
            //                MessageBox.Show("Превышено допустимое число выбранных вручную вопросов");
            //                // MyDt.Rows[e.RowIndex][e.ColumnIndex] = false;
            //                MyDt.RejectChanges();
            //                return;
            //            }
            //        }
            //        MyDt.AcceptChanges();
            //        ((DataTable) datagridForDataTable.DataSource).AcceptChanges();
            //    }
            //}
        }
    }
}
