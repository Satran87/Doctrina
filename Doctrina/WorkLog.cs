using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Doctrina
{
    public static class WorkLog
    {
        private static string _directoryName = "PrintStatistics";
        private  const string DocString = "Документ_";
        public static void AddNewEntry(string [] writedDocks,string folderName)
        {
            int docNumber = 0;
            if (!Directory.Exists(_directoryName))
            {
                Directory.CreateDirectory(_directoryName);
            }
            var fileName= _directoryName + @"\"+ DateTime.Now.Year + "_"+ DateTime.Now.Month+"_"
            +DateTime.Now.Day + ".csv";

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
                    sw.WriteLine("{0};{1}{2}{3}", tempDocString, tempString,"Из папки: ",folderName);
                    sw.Close();
                }
        }
    }
}
