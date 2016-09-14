using System;
using System.IO;
using System.Linq;
using System.Threading;
using Doctrina.Enums;

namespace Doctrina
{
    public class DoneBlock
    {
        private readonly DoctrinaMainForm _myForm;
        public DoneBlock(DoctrinaMainForm form, string qPath, string aPath, DateTime lPrintTime, uint timeRepeated = 0)
        {
            _questionPath = qPath;
            _answerPath = aPath;
            _timeRepeat = timeRepeated;
            _lastPrintTime = lPrintTime;
            _myForm = form;

        }
        public string QuestionPath
        {
            get
            {
                return _questionPath;
            }
            set { _questionPath = value; }
        }

        public string AnswerPath
        {
            get { return _answerPath; }
            set { _answerPath = value; }
        }

        public uint TimeRepeated
        {
            get { return _timeRepeat; }
            set { _timeRepeat = value; }
        }

        public string ShortAnswerPath
        {
            get { return Path.GetFileNameWithoutExtension(_answerPath); }
        }
        public string AnswerFolder
        {
            get
            {
                var temp=new DirectoryInfo(Path.GetDirectoryName(_answerPath)).Name;
                return temp;
            }
        }
        public string FolderWithAnswerDirectory
        {
            get
            {
              var tempAnsDir = Path.GetDirectoryName(_answerPath);
              var splitedLines=  tempAnsDir.Split('\\');
                string dirName=String.Empty;
                try
                {
                dirName = splitedLines[splitedLines.Count() - 2];//Директория с папкой
                }
                catch (Exception e)
                {                 
                    throw new Exception(e.Message);
                }
               
                return dirName;
            }
        }
        public string ShortFileName
        {
            get
            {
                foreach (var bannedSymbol in _myForm.NewBannedSymbols1.BannedSymbolsList)
                {
                    if (ShortAnswerPath.Contains(bannedSymbol))
                    {
                        return ShortAnswerPath.Remove(ShortAnswerPath.Length - 2, 1);
                    }
                }
                return ShortAnswerPath.Remove(ShortAnswerPath.Length - 1);//хз почему но replace сбоит
            }
        }

        public bool AllowPrint(uint maxRepeat, DateTime timeAllowPrint)
        {
            var currentTimeStamp = DateTime.Now - timeAllowPrint;
            var checkdT = _lastPrintTime.Add(currentTimeStamp);
            
            if (_myForm.CurrentWorkEnum !=WorkLikeEnum.OnlyGenerator || (_timeRepeat < maxRepeat || checkdT >= timeAllowPrint))
            {
                if (checkdT >= timeAllowPrint && _timeRepeat >= maxRepeat)
                {
                    _timeRepeat = 0;
                }
                ++_timeRepeat;
                _lastPrintTime = DateTime.Now;
                return true;
            }
            return false;
        }

        public DateTime LastTimePrint
        {
            get { return _lastPrintTime; }
            set { _lastPrintTime = value; }
        }

        private DateTime _lastPrintTime;
        private string _questionPath;
        private string _answerPath;
        private uint _timeRepeat;
    }
}

