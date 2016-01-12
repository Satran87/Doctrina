using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Doctrina.Enums;

namespace Doctrina
{
    internal static class CheckClass
    {
        public static bool Cheks(Form1 form1,string selectedFolder)
        {
            if (FolderCheck(form1,selectedFolder))
                return true;
            //NOTE:Может потом сделать более точную проверку
            return CheckAlllOther(form1);
        }
        private static bool CheckAlllOther(Form1 form1)
        {
            List<DoneBlock> randBlockCopy = CopyDoneBlocks(form1, form1.DoneBlocks);
            List < List < DoneBlock >> temp=new List<List<DoneBlock>>();//Для совместимости.
            if (GetBlocks_1(form1, ref randBlockCopy,ref temp)) return true;
            return false;
        }

        internal static bool GetBlocks_1(Form1 form1, ref List<DoneBlock> randBlockCopy, ref List<List<DoneBlock>> allQuestions,List<DoneBlock> constBlocks=null,int maxQuestionOnList=0 )
        {
            WorkLikeEnum currentWorType = form1.CurrentWorkEnum;
            uint someTimer = 0;
            for (int listNumber = 0; listNumber < form1.MaxLists;)
            {
                var uniqueQuestion = constBlocks == null ? new List<DoneBlock>() : new List<DoneBlock>(constBlocks);

                long maxRepeat;
                switch (currentWorType)
                {
                    case WorkLikeEnum.OnlyGenerator:
                    {
                        maxRepeat = form1.MaxQuestionOnListUint;
                        break;
                    }
                    case WorkLikeEnum.GeneratorAndConst:
                    {
                        maxRepeat = form1.MaxQuestionOnListUint - form1.NumberOfConstFiles;
                        break;
                    }
                    case WorkLikeEnum.GeneratorAndLST:
                    {
                        maxRepeat = maxQuestionOnList > 0
                            ? maxQuestionOnList
                            : (form1.MaxQuestionOnListUint - form1.NumberOfConstFiles - form1.LSTEasyNumber -
                               form1.LSTMiddleNumber - form1.LSTHardNumber);
                        break;
                    }
                    default:
                    {
                        maxRepeat = form1.MaxQuestionOnListUint;
                        break;
                    }
                }
                for (int questions = 0; questions < maxRepeat;)
                {
                    var randBlock = RandomNumber.Between(0, randBlockCopy.Count - 1);
                    if (!uniqueQuestion.Contains(randBlockCopy[randBlock]))
                    {
                        int bannedSymbolsCount = 0;
                        bool bannedSymbolMeets = false;
                        foreach (var bannedSymbol in form1.NewBannedSymbols1.BannedSymbolsList)
                        {
                            if (randBlockCopy[randBlock].ShortFileName.Contains(bannedSymbol))
                            {
                                if (form1.NewBannedSymbols1.IsBannedSymbolMeet(bannedSymbolsCount)) //TODO:Проверить тщательно!
                                {
                                    bannedSymbolMeets = true;
                                    break;
                                }
                            }
                            ++bannedSymbolsCount;
                        }
                        if (bannedSymbolMeets) continue;
                        if (randBlockCopy[randBlock].AllowPrint(form1.MaxQuestonRepeatUint, form1.DateThenAllowPrint))
                        {
                            int bannedSymbolsCount2 = 0;
                            bool contain = false;
                            foreach (var bannedSymbol in form1.NewBannedSymbols1.BannedSymbolsList)
                            {
                                if (randBlockCopy[randBlock].ShortFileName.Contains(bannedSymbol))
                                {
                                    form1.NewBannedSymbols1.BannedSymbolMeet(bannedSymbolsCount2);
                                    uniqueQuestion.Add(randBlockCopy[randBlock]);
                                    ++questions;
                                    contain = true;
                                    break;
                                }
                                ++bannedSymbolsCount2;
                            }
                            if (!contain)
                            {
                                uniqueQuestion.Add(randBlockCopy[randBlock]);
                                ++questions;
                            }
                        }
                    }
                    if (someTimer >= 60000)
                    {
                        form1.OnErrorHappen(
                            "Ошибка комбинации возможных вариантов \n\rПроверьте вводимые данные и количество доступных вопросов");
                        ErrorLog.AddNewEntry("Вопросов_на_лист=" + form1.MaxQuestionOnListUint + " | Макс_повторов_вопросов= " +
                                             form1.MaxQuestonRepeatUint + " | Всего_листов= " + form1.MaxLists);
                        if (File.Exists(form1.ChooseFolderPath + @"\" + Form1.FileNameListText))
                            File.Copy(form1.ChooseFolderPath + @"\" + Form1.FileNameListText,
                                "ErrorList" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" +
                                DateTime.Now.Second + ".csv");
                        return true;
                    }
                    ++someTimer;
                }
                allQuestions.Add(uniqueQuestion);
                ++listNumber;
                form1.NewBannedSymbols1.ClearBannedSymbolMeet();
            }
            form1.NewBannedSymbols1.ClearBannedSymbolMeet();
            return false;
        }

        public static List<DoneBlock> CopyDoneBlocks(Form1 form1, List<DoneBlock> doneBlocks)
        {
            List<DoneBlock> copyedList = doneBlocks.Select(doneBlock => new DoneBlock(form1, doneBlock.QuestionPath, doneBlock.AnswerPath, doneBlock.LastTimePrint, doneBlock.TimeRepeated)).ToList();
            return copyedList;
        }

        public static bool FolderCheck(Form1 form1,string folderPath)
        {
            folderPath = folderPath + "\\";
            var tempAnsDir = Path.GetDirectoryName(folderPath);
            var splitedLines = tempAnsDir.Split('\\');
            string dirName = String.Empty;
            try
            {
                dirName = splitedLines[splitedLines.Count() - 2];//Директория с папкой
                if (dirName.Contains(":"))
                {
                    form1.OnErrorHappen("Строка для папки указывает на корень");
                    return true;
                }
            }
            catch (Exception e)
            {
                form1.OnErrorHappen("Не получилось выделить папку с папками");
                ErrorLog.AddNewEntry(e.Message);
                return true;
            }
            return false;
        }
    }

}