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
        public void CreateDoc()
        {
            
            Document doc = myWord.Documents.Add();
        }

        public IDataObject ReadTextFromDoc(string pathToDoc)
        {
            Document doc = myWord.Documents.Open(FileName: pathToDoc, Visible: false);
           doc.ActiveWindow.Selection.WholeStory();
            doc.ActiveWindow.Selection.Copy();
            doc.Close();
            var data = Clipboard.GetDataObject();
            return data;
        }

        public void SaveDataFromClipboard(bool isAnswer,string fileName)
        {
            Document doc;
            if (!Directory.Exists("TempFolder"))
            {
                Directory.CreateDirectory("TempFolder");
                
            }
            fileName = GetTempDirectory() + fileName;
            if (isAnswer)
            {
                fileName = fileName + "_Ans";
            }
            else
            {
                fileName = fileName + "_Ques";
            }
            string fullName = fileName + ".docx";
            if (File.Exists(fullName))
            {
                doc = myWord.Documents.Open(fullName); //TODO: Добавить пустую страницу

            }
            else
            {
               doc = myWord.Documents.Add();
 
            }
            object unit;
            object extend;
            unit = WdUnits.wdStory;
            extend = WdMovementType.wdMove;
            myWord.Selection.EndKey(ref unit, ref extend);
            myWord.Selection.Paste();
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
        public void CreateFilesFromArray(List<DoneBlock> doneBlocks)
        {
            var name=Path.GetRandomFileName();
            foreach (var block in doneBlocks)
            {
                ReadTextFromDoc(block.AnswerPath);
                SaveDataFromClipboard(true, name);
                ReadTextFromDoc(block.QuestionPath);
                SaveDataFromClipboard(false, name);
            }
            OnMyEvent(new FileHlpArgs());
        }

        public void Print(object fileName)
        {
            Document doc = myWord.Documents.Open(FileName: fileName, Visible: false);
            doc.ActiveWindow.PrintOut();
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
            DirectoryInfo directoryInfo = new DirectoryInfo(GetTempDirectory()); //загружаем в экземпляр класса DirectroyInfo информацию о всех файлах

            foreach (FileInfo file in directoryInfo.GetFiles()) //получаем список всех файлов в выбранном каталоге
            {
                try
                {
                        file.Delete(); //пытаемся удалить файл
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
