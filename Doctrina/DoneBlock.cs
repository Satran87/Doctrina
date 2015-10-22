using System;
using System.IO;

namespace Doctrina
{
    public class DoneBlock
    {
        public DoneBlock(string qPath, string aPath, DateTime lPrintTime, uint timeRepeated = 0)
        {
            _questionPath = qPath;
            _answerPath = aPath;
            _timeRepeat = timeRepeated;
            _lastPrintTime = lPrintTime;

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
        public string ShortFileName
        {
            get
            {
                if(ShortAnswerPath.Contains(BannedSymbols.BannedSymbol1))
                {
                    return ShortAnswerPath.Remove(ShortAnswerPath.Length - 2,1);
                }
                if (ShortAnswerPath.Contains(BannedSymbols.BannedSymbol2))
                {
                    return ShortAnswerPath.Remove(ShortAnswerPath.Length - 2,1);
                }
                if (ShortAnswerPath.Contains(BannedSymbols.BannedSymbol3))
                {
                    return ShortAnswerPath.Remove(ShortAnswerPath.Length - 2,1);
                }
                if (ShortAnswerPath.Contains(BannedSymbols.BannedSymbol4))
                {
                    return ShortAnswerPath.Remove(ShortAnswerPath.Length - 2,1);
                }
                if (ShortAnswerPath.Contains(BannedSymbols.BannedSymbol5))
                {
                    return ShortAnswerPath.Remove(ShortAnswerPath.Length - 2, 1);
                }
                if (ShortAnswerPath.Contains(BannedSymbols.BannedSymbol6))
                {
                    return ShortAnswerPath.Remove(ShortAnswerPath.Length - 2, 1);
                }
                if (ShortAnswerPath.Contains(BannedSymbols.BannedSymbol7))
                {
                    return ShortAnswerPath.Remove(ShortAnswerPath.Length - 2, 1);
                }
                if (ShortAnswerPath.Contains(BannedSymbols.BannedSymbol8))
                {
                    return ShortAnswerPath.Remove(ShortAnswerPath.Length - 2, 1);
                }
                return ShortAnswerPath.Remove(ShortAnswerPath.Length - 1);//хз почему но replace сбоит
            }
        }

        public bool AllowPrint(uint maxRepeat, DateTime timeAllowPrint)
        {
            var currentTimeStamp = DateTime.Now - timeAllowPrint;
            var checkdT = _lastPrintTime.Add(currentTimeStamp);
            if (_timeRepeat < maxRepeat || checkdT >= timeAllowPrint)
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

