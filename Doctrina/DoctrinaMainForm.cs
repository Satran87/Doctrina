﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Doctrina.Enums;
using IniHlp;

namespace Doctrina
{
    public partial class DoctrinaMainForm : Form
    {
        private const string ColunmNameFileName = "Имя файла";
        private const string ColunmNameRepeat = "Повторы";
        private const string ColunmNamelPrintTime = "Время последней печати";
        private const string ColunmNamelCheckFile = "Выбрать файл для печати";
        internal const string FileNameListText = "FileList.csv";
        private  string ConfigIniFileName = "config.ini";
        private const string DateThenAllowPrintPickerString = "Data=";
        private const string NumberOfListsString = "NumberOfLists=";
        private const string MaxQuestionOnListString = "MaxQuestionOnList=";
        private const string MaxQuestionRepeatString = "MaxQuestionRepeat=";
        private const string BannedSymbolsString = "BannedSymbols=";
        private const string EasyQuestionString = "Easy=";
        private const string MiddleQuestionString = "Middle=";
        private const string HardQuestionString = "Hard=";
        private const string ModeString = "Mode=";

        public WorkLikeEnum CurrentWorkEnum=WorkLikeEnum.OnlyGenerator;
        public int LSTEasyNumber = 0;
        public int LSTMiddleNumber = 0;
        public int LSTHardNumber = 0;

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

        public DoctrinaMainForm()
        {
            CheckPassword();
            InitializeComponent();
            _wordHlp.MyEvent += MyEventHendler;
            backgroundWorker1.DoWork+=BackgroundWorker1OnDoWork;
            backgroundWorker2.DoWork += BackgroundWorker2_DoWork;
            cancelButton.Enabled = false;
            InitMyComponents();
            this.KeyPreview = true;
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            var nl = Environment.NewLine;
            if (e.KeyCode == Keys.F5 && e.Alt && e.Control)
            {
                MessageBox.Show(ConfigIniFileName + nl + "Легко = "+ LSTEasyNumber+ nl+"Средне=  "+ LSTMiddleNumber+nl+"Тяжело = " + LSTHardNumber+nl+"Тип работы = "+ CurrentWorkEnum) ;
                e.Handled = true;
            }
        }
        private IniHlpClass IniHlp;

        private void InitMyComponents()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                ConfigIniFileName = args[1];
            }
            if (!File.Exists(ConfigIniFileName))
            {
                OnErrorHappen("Файл "+ ConfigIniFileName + " не найден. Запуск невозможен");
                Environment.Exit(0);
            }
            IniHlp = new IniHlpClass(ConfigIniFileName);

            var newLine = IniHlp.GetDateTimeFromLine(DateThenAllowPrintPickerString);
            if (newLine.Year == 1) newLine = Convert.ToDateTime("2015-01-01");
            dateThenAllowPrintPicker.Value = newLine;

            string tempStr = string.Empty;
            tempStr = IniHlp.GetStringFromLine(NumberOfListsString);
            NumberOfLists.Text = tempStr;
            if (string.IsNullOrEmpty(tempStr)) tempStr = "1";
            MaxLists = Convert.ToUInt32(tempStr);

            tempStr = IniHlp.GetStringFromLine(MaxQuestionOnListString);
            MaxQuestionOnListText.Text = tempStr;
            if (string.IsNullOrEmpty(tempStr)) tempStr = "1";
            MaxQuestionOnListUint = Convert.ToUInt32(tempStr);

            tempStr = IniHlp.GetStringFromLine(MaxQuestionRepeatString);
            MaxQuestionRepeatText.Text = tempStr;
            if (string.IsNullOrEmpty(tempStr)) tempStr = "1";
            MaxQuestonRepeatUint= Convert.ToUInt32(tempStr);

            var bannedSymbols = IniHlp.GetStringFromLine(BannedSymbolsString);
            NewBannedSymbols1 = new BannedSymbolsClass(bannedSymbols.Split('|'));

            LSTEasyNumber = IniHlp.GetIntFromLine(EasyQuestionString);
            LSTMiddleNumber = IniHlp.GetIntFromLine(MiddleQuestionString);
            LSTHardNumber = IniHlp.GetIntFromLine(HardQuestionString);
            {
                switch (IniHlp.GetIntFromLine(ModeString))
                {
                    case 1:
                    {
                        CurrentWorkEnum = WorkLikeEnum.OnlyGenerator;
                        break;
                    }
                    case 2:
                    {
                        CurrentWorkEnum = WorkLikeEnum.GeneratorAndConst;
                        break;
                    }
                    case 3:
                    {
                        CurrentWorkEnum = WorkLikeEnum.GeneratorAndLST;
                        break;
                    }
                    default:
                        OnErrorHappen("Не указан режим работы");
                        break;
                }
            }
            ReCreateDT();
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
                SetTextinThread("");
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
                CheckFolderForDocFiles(folderPath);
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

        /// <summary>
        /// Проверяем папку на наличие Doc файлов. При необходимости конвертируем
        /// </summary>
        /// <param name="folderPath"></param>
        private void CheckFolderForDocFiles(string folderPath)
        {
            DirectoryInfo testDir = new DirectoryInfo(folderPath);
            var fileInfoArray =
                testDir.GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => f.Extension == ".doc")
                    .ToArray<FileInfo>();
            if (fileInfoArray.Any())
                if (
                    OnErrorHappenYesNo(
                        "Папка содержит файлы с расширением Doc \n\rДля работы программы нужно, чтобы файлы имели расширение Docx.\n\rПровести конвертацию?"))
                    ConvertDocToDocX(fileInfoArray);
        }

        /// <summary>
        /// Если будут встречаться Doc файлы, то их надо конвертировать в DocX для работы
        /// </summary>
        /// <param name="fileInfoArray"></param>
        private void ConvertDocToDocX(FileInfo[] fileInfoArray)
        {
            try
            {
                foreach (var tmp in fileInfoArray)
                {
                    _wordHlp.ConvertDocToDocx(tmp.FullName);
                }
            }
            catch (Exception e)
            {
                OnErrorHappen("Не смог конвертировать файл с расширением Doc в DocX из-за " + e.Message);
                throw;
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
            int counter = -1;//Счетчик для индекса отмеченных вопросов (констант)
            foreach (var line in readFile.Skip(1))
            {
                ++counter;
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
                if (CurrentWorkEnum == WorkLikeEnum.GeneratorAndConst)
                {
                    if (lines.Count() > 3)
                        if(Convert.ToBoolean(lines[3]))
                        CheckedIndex.Add(counter);
                }
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

            if (CurrentWorkEnum == WorkLikeEnum.GeneratorAndLST)
            {
                
            }
            else
            {
                if (CheckClass.Cheks(this, currentFolderTextBox.Text)) return;
            }
            
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
                        List<List<bool>> bannedSymbolsForEachList =new List<List<bool>>();//Для совместимости
                        CheckClass.GetBlocks_1(this, ref DoneBlocks, ref allQuestions,ref bannedSymbolsForEachList);
                    break;
                }
                case WorkLikeEnum.GeneratorAndConst:
                {
                    List<DoneBlock> copyedDoneBlocksWihoutCosnt = new List<DoneBlock>();
                        List<List<bool>> bannedSymbolsForEachList = new List<List<bool>>();//Для совместимости
                        List<DoneBlock> constBlocks =
                        new List<DoneBlock>(PrepareNewDoneBlockForallQuestion(ref copyedDoneBlocksWihoutCosnt));

                    NumberOfConstFiles = constBlocks.Count();
                    CheckClass.GetBlocks_1(this, ref copyedDoneBlocksWihoutCosnt, ref allQuestions,ref bannedSymbolsForEachList,constBlocks);

                    foreach (var constBlock in constBlocks)
                    {
                        constBlock.TimeRepeated = constBlock.TimeRepeated+MaxLists;
                        constBlock.LastTimePrint = DateTime.Now;
                    }
                    break;
                }
                case WorkLikeEnum.GeneratorAndLST:
                    {
                        List<DoneBlock> copyDoneBlocks = new List<DoneBlock>(DoneBlocks);
                        List<DoneBlock> EasyList = new List<DoneBlock>();
                        List<DoneBlock> MiddleList = new List<DoneBlock>();
                        List<DoneBlock> HardList = new List<DoneBlock>();

                        List<List<bool>> bannedSymbolsForEachList = new List<List<bool>>();

                        List < List < DoneBlock >> EasyDone = new List<List<DoneBlock>>();
                        List<List<DoneBlock>> MiddleDone = new List<List<DoneBlock>>();
                        List<List<DoneBlock>> HardDone = new List<List<DoneBlock>>();
                        bool result = true;
                        if (LSTEasyNumber + LSTMiddleNumber + LSTHardNumber > MaxQuestionOnListUint)
                        {
                            OnErrorHappen("Количество спец вопросов больше, чем всего вопросов на лист");
                            return;
                        }
                        if (LSTEasyNumber > 0)
                        {
                            EasyList = PrepareNewDoneBlockForLST(ref copyDoneBlocks, "Л_");
                            result = CheckClass.GetBlocks_1(this, ref EasyList, ref allQuestions,ref bannedSymbolsForEachList,null, LSTEasyNumber);
                            EasyDone.AddRange(allQuestions);
                            allQuestions.Clear();
                        }
                        if (LSTMiddleNumber > 0)
                        {
                            MiddleList = PrepareNewDoneBlockForLST(ref copyDoneBlocks, "С_");
                            result = CheckClass.GetBlocks_1(this, ref MiddleList, ref allQuestions, ref bannedSymbolsForEachList,null, LSTMiddleNumber);
                            MiddleDone.AddRange(allQuestions);
                            allQuestions.Clear();
                        }
                        if (LSTHardNumber > 0)
                        {
                            HardList = PrepareNewDoneBlockForLST(ref copyDoneBlocks, "Т_");
                            result = CheckClass.GetBlocks_1(this, ref HardList, ref allQuestions, ref bannedSymbolsForEachList, null, LSTHardNumber);
                            HardDone.AddRange(allQuestions);
                            allQuestions.Clear();
                        }
                        result = CheckClass.GetBlocks_1(this, ref copyDoneBlocks, ref allQuestions, ref bannedSymbolsForEachList);
                        if(result)
                            throw new Exception("Произошла ошибка генерации вопросов");
                        int index = 0;
                        foreach (var question in allQuestions)
                        {
                            if (EasyDone.Count > 0)
                            {
                                question.AddRange(EasyDone[index]);
                            }
                            if (MiddleDone.Count > 0)
                            {
                                question.AddRange(MiddleDone[index]);
                            }
                            if (HardDone.Count > 0)
                            {
                                question.AddRange(HardDone[index]);
                            }
                            ++index;
                        }
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

        private List<DoneBlock> PrepareNewDoneBlockForLST(ref List<DoneBlock> doneBlocks,string symbolToSeach)
        {
            List<DoneBlock> separetedBlocks = new List<DoneBlock>();
            foreach (var doneblock in doneBlocks)
            {
                if(doneblock.ShortFileName.StartsWith(symbolToSeach))
                    separetedBlocks.Add(doneblock);
            }
            foreach (var block in separetedBlocks)
            {
                doneBlocks.Remove(block);
            }
            return separetedBlocks;
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

        //TODO:Переделать механизм - постоянно передергивает. Предложить сохранять?
        private void datagridForDataTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (CurrentWorkEnum == WorkLikeEnum.GeneratorAndConst)
            {
                if (MyDt.Columns[ColunmNamelCheckFile] != MyDt.Columns[e.ColumnIndex])
                    return;
                bool test=(bool)datagridForDataTable[e.ColumnIndex, e.RowIndex].Value;
                var CheckColumn = MyDt.Columns[ColunmNamelCheckFile];
                int t = 0;
                foreach (DataRow row in MyDt.Rows)
                {
                    var columnIsChecked = (bool)row[CheckColumn];
                    if (columnIsChecked)
                        ++t;
                    if (t == 3)
                    {
                        MessageBox.Show("Превышено допустимое число выбранных вручную вопросов");
                         MyDt.Rows[e.RowIndex][e.ColumnIndex] = false;
                        return;
                    }
                }
               // MyDt.AcceptChanges();
                //((DataTable)datagridForDataTable.DataSource).AcceptChanges();
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

        private void SaveDtToFile(string folderPath)
        {
            if (MyDt.Rows.Count>1)
            {
                //Перенес из сохранения констант  в datagridForDataTable_CellEndEdit
                //
                MyDt.AcceptChanges();
                ((DataTable)datagridForDataTable.DataSource).AcceptChanges();
                //

                folderPath = folderPath + @"\" + FileNameListText;
                if (!File.Exists(folderPath))
                {
                    File.Create(folderPath).Close();
                }

                StringBuilder sb = new StringBuilder();
                IEnumerable<string> columnNames = MyDt.Columns.Cast<DataColumn>().
                    Select(column => column.ColumnName).Where(column =>column!=ColunmNamelCheckFile);
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
        public bool OnErrorHappenYesNo(string text)
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
                IniHlp.SaveDataToIniFile(DateThenAllowPrintPickerString, DateThenAllowPrint.ToString());
                IniHlp.SaveDataToIniFile(NumberOfListsString, NumberOfLists.Text);
                IniHlp.SaveDataToIniFile(MaxQuestionOnListString, MaxQuestionOnListText.Text);
                IniHlp.SaveDataToIniFile(MaxQuestionRepeatString, MaxQuestionRepeatText.Text);
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
            DateThenAllowPrint = dateThenAllowPrintPicker.Value;
            colorTable();
        }

        private void NumberOfLists_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIsDigit(e);
        }

        private void MaxQuestionOnListText_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIsDigit(e);
        }

        private void MaxQuestionRepeatText_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIsDigit(e);
        }

        private static void CheckIsDigit(KeyPressEventArgs e)
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

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveInit();
            if (!string.IsNullOrEmpty(ChooseFolderPath))
            {
                SaveDtToFile(ChooseFolderPath);
            }
        }
    }
}
