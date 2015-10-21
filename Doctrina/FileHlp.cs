using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;

namespace Doctrina
{
    public delegate void MyEventHandler(object sender, FileHlpArgs e);
    public class FileHlp
    {
        public event MyEventHandler MyEvent;
        protected virtual void OnMyEvent(FileHlpArgs e)
        {
            MyEvent(this, e);
        }
        Application myWord = new ApplicationClass();
        public FileHlp()
        {
            myWord.Visible = false;
        }

        private void GenerateDocFiles(string newFileName,string oldFileName, bool isAnswer)
        {
            object unit = WdUnits.wdStory;
            object extend = WdMovementType.wdMove;
            Document doc = null;
            bool isDocExist = false;
            if (!Directory.Exists("TempFolder"))
            {
                Directory.CreateDirectory("TempFolder");

            }
            newFileName = GetTempDirectory() + newFileName;
            if (isAnswer)
            {
                newFileName = newFileName + "_Ans";
            }
            else
            {
                newFileName = newFileName + "_Ques";
            }
            string fullName = newFileName + ".docx";
            try
            {
                if (File.Exists(fullName))
                {
                    doc = myWord.Documents.Open(fullName);
                    isDocExist = true;
                }
                else
                {
                    doc = myWord.Documents.Add();
                }

                if (isDocExist)
                {
                    myWord.Selection.EndKey(ref unit, ref extend);
                    myWord.Selection.InsertBreak(WdBreakType.wdSectionBreakNextPage);
                    myWord.Selection.InsertFile(oldFileName);
                }
                else
                {
                    myWord.Selection.EndKey(ref unit, ref extend);
                    myWord.Selection.InsertFile(oldFileName);
                }
        }
            catch (Exception e)
            {
                doc.Close();
                ErrorLog.AddNewEntry(e.Message);
                DeleteAllFilesOnTempDirectory();
                throw new Exception("Ошибка при создании файла");
            }
            doc.SaveAs(FileName: fullName);
            doc.Close();
        }

        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public string GetTempDirectory()
        {
            return GetCurrentDirectory()+ @"\"+@"TempFolder\";
        }

        public void CreateFilesFromArray(List<DoneBlock> doneBlocks,int counter)
        {
            List<string>printingNames=new List<string>();
            var randomFileName= counter +"_"+ Path.GetRandomFileName();
            string folderBlock=string.Empty;
            foreach (var block in doneBlocks)
            {
                GenerateDocFiles(randomFileName, block.AnswerPath, true);//TODO:Переделать код на однокартый вызов doc для документа
                GenerateDocFiles(randomFileName, block.QuestionPath, false);
                printingNames.Add(block.ShortFileName);
                folderBlock = block.AnswerFolder;
            }
            WorkLog.AddNewEntry(printingNames.ToArray(), folderBlock);
            OnMyEvent(new FileHlpArgs());
        }

        public void Print(object fileName)
        {
            Document doc = null;
            try
            {
                doc = myWord.Documents.Open(FileName: fileName, Visible: false);
                doc.ActiveWindow.PrintOut();
            }
            catch (Exception e)
            {
                if (doc != null) doc.Close();
                ErrorLog.AddNewEntry(e.Message);
                DeleteAllFilesOnTempDirectory();
                throw new Exception("Ошибка при открытии документа");
            }
            doc.Close();
            LocalPrintServer ps = new LocalPrintServer();
            while (ps.DefaultPrintQueue.NumberOfJobs > 0)//TODO:Получить очередь печати принтера и найти норм вирт принтер
            {
                ps.Refresh();
            }
            DeleteAllFilesOnTempDirectory(fileName.ToString());
        }
        public void CloseWord()
        {
            myWord.Quit();
        }
        private void DeleteAllFilesOnTempDirectory(string fileName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetTempDirectory()); //загружаем в экземпляр класса DirectroyInfo информацию о всех файлах

            foreach (FileInfo file in directoryInfo.GetFiles()) //получаем список всех файлов в выбранном каталоге
            {
                try
                {
                    if(file.FullName== fileName)
                    file.Delete(); //пытаемся удалить файл
                }
                catch (Exception e)
                {
                    ErrorLog.AddNewEntry(e.Message);
                }
            }
        }

        public void DeleteAllFilesOnTempDirectory()
        {
            if (!Directory.Exists(GetTempDirectory()))
                return;
            DirectoryInfo directoryInfo = new DirectoryInfo(GetTempDirectory());

            foreach (FileInfo file in directoryInfo.GetFiles()) 
            {
                try
                {
                        file.Delete();
                }
                catch (Exception e)
                {
                    ErrorLog.AddNewEntry(e.Message);
                }
            }
        }
        public static void NumericalSort(string[] ar)
        {
            Regex rgx = new Regex("([^0-9]*)([0-9]+)");
            Array.Sort(ar, (a, b) =>
            {
                var ma = rgx.Matches(a);
                var mb = rgx.Matches(b);
                for (int i = 0; i < ma.Count; ++i)
                {
                    int ret = ma[i].Groups[1].Value.CompareTo(mb[i].Groups[1].Value);
                    if (ret != 0)
                        return ret;

                    ret = int.Parse(ma[i].Groups[2].Value) - int.Parse(mb[i].Groups[2].Value);
                    if (ret != 0)
                        return ret;
                }

                return 0;
            });
        }
    }

    public class FileHlpArgs : EventArgs
    {
    }
}
