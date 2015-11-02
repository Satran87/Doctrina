using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Doctrina
{
    public static class WorkLog
    {
        private static string _directoryName = "PrintStatistics";
        private const string DocName = "ФайлСоСпискомПечати.csv";
        private  const string DocString = "Билет_";
        public static void AddNewEntry(string [] writedDocks,string folderName,string folderWithFolders)
        {
            int docNumber = 0;
            if (!Directory.Exists(_directoryName))
            {
                Directory.CreateDirectory(_directoryName);
            }
            string folderForFile = _directoryName + @"\" + folderWithFolders +@"\"+ folderName+@"\";
            if (!Directory.Exists(folderForFile))
            {
                Directory.CreateDirectory(folderForFile);
            }
            //var fileName= folderForFile + DateTime.Now.Year + "_"+ DateTime.Now.Month+"_"
            //+DateTime.Now.Day + ".csv";
            var fileName = folderForFile + DocName;

                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Close();
                }
                else
                {
                    var readFile = File.ReadAllLines(fileName, Encoding.GetEncoding("windows-1251"));
                    docNumber = Convert.ToInt32(readFile[readFile.Count() - 1].Split(';')[0].Split('_')[1]);//Получить последнюю цифру в файле. (Заодно призвать демона -))
                }
                string tempString = writedDocks.Aggregate(string.Empty, (current, doc) => current + doc + ";");
                ++docNumber;
                var tempDocString = DocString + docNumber;
                using (var sw = new StreamWriter(fileName, true, Encoding.GetEncoding("windows-1251")))
                {
                    sw.WriteLine("{0};{1}", tempDocString, tempString);
                    sw.Close();
                }
        }
    }
}
