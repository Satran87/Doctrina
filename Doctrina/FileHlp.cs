using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;

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

        public IDataObject ReadTextFromDoc(string pathToDoc,bool ans, string fileName)
        {
            Document doc = null;
            try
            {
                doc = myWord.Documents.Open(FileName: pathToDoc, Visible: false);
                doc.ActiveWindow.Selection.WholeStory();
                doc.ActiveWindow.Selection.Copy();
            }
            catch (Exception e)
            {
                doc.Close();
                ErrorLog.AddNewEntry(e.Message);
                DeleteAllFilesOnTempDirectory();
                throw new Exception("Ошибка при чтении документа");
            }
            
            doc.Close();
            var data = Clipboard.GetDataObject();
            NewSaveFile(ans, fileName, pathToDoc);
            return data;
        }

        private List<string> colontitul;
        private int t = 0;

        private void NewSaveFile(bool isAnswer,string newFileName,string oldFileName)
        {
            object unit;
            object extend;
            unit = WdUnits.wdStory;
            extend = WdMovementType.wdMove;
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
                    ++t;
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
                    myWord.Selection.Paste();
                }
            }
            catch (Exception e)
            {
                doc.Close();
                ErrorLog.AddNewEntry(e.Message);
                DeleteAllFilesOnTempDirectory();
                throw new Exception("Ошибка при сохранении документа из буфера");
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

        private string randName;
        public void CreateFilesFromArray(List<DoneBlock> doneBlocks)
        {
            colontitul = new List<string>(); 
            t = 0;
            var name=Path.GetRandomFileName();
            randName = name;
            foreach (var block in doneBlocks)
            {
                ReadTextFromDoc(block.AnswerPath,true, name);
                ReadTextFromDoc(block.QuestionPath,false, name);
            }
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
    }

    public class FileHlpArgs : EventArgs
    {
    }
}
