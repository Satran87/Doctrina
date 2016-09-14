using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IniHlp
{
    public class IniHlpClass
    {
        /// <summary>
        /// Список позиций, в которых нашли указанные данные
        /// </summary>
        private Dictionary<string,int> _positionDictionary; 
        private string _iniFileName;
        private string[] _allLinesFromIniFile;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iniFile">Полное имя ini файла</param>
        public IniHlpClass(string iniFile)
        {
            _positionDictionary = new Dictionary<string, int>();
            _iniFileName = iniFile;
            if (!File.Exists(_iniFileName))
            {
                throw new FileNotFoundException("Не могу найти ini файл");
            }

            _allLinesFromIniFile = File.ReadAllLines(_iniFileName, Encoding.Default);
        }
        /// <summary>
        /// Вернуть число из указанной строки
        /// </summary>
        /// <param name="searchString">Строка для поиска в ini фйале</param>
        /// <returns>Возвращает номер из указанной строки, если не найдено, возвращает null</returns>
        public int GetIntFromLine(string searchString)
        {
           return Convert.ToInt32(GetDatafromLine(searchString));
        }

        /// <summary>
        /// Вернуть дату из указанной строки
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public DateTime GetDateTimeFromLine(string searchString)
        {
            return Convert.ToDateTime(GetDatafromLine(searchString));
        }

        public string GetStringFromLine(string searchString)
        {
            return Convert.ToString(GetDatafromLine(searchString));
        }

        public object GetDatafromLine(string searchString) 
        {
            int lineCounter = -1;
            foreach (var line in _allLinesFromIniFile)
            {
                ++lineCounter;
                if (line.Contains(searchString))
                {
                    _positionDictionary.Add(searchString, lineCounter);
                    var tempstring = line.Replace(searchString, "");
                    return tempstring;
                }
            }
            return null;
        }
        //Имя переменной и значение которое присвоить, в качестве входных параметров
        public void SaveDataToIniFile(string paramName,string paramValue)
        {
            _allLinesFromIniFile[_positionDictionary[paramName]] = paramName + paramValue;
            File.WriteAllLines(_iniFileName, _allLinesFromIniFile,Encoding.GetEncoding("windows-1251"));
        }
}
}
