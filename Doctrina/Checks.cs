using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Doctrina
{
    internal static class CheckClass
    {
        public static bool Cheks(Form1 form1)
        {
            //NOTE:Может потом сделать более точную проверку
            return CheckAlllOther(form1);
        }
        private static bool CheckAlllOther(Form1 form1)
        {
            List<DoneBlock> randBlockCopy = CopyDoneBlocks(form1.DoneBlocks);
            uint someTimer = 0;
            for (int listNumber = 0; listNumber < form1.MaxLists;)
            {
                bool bannedSymbol1Meets = false;
                bool bannedSymbol2Meets = false;
                bool bannedSymbol3Meets = false;
                bool bannedSymbol4Meets = false;
                List<DoneBlock> uniqueQuestion = new List<DoneBlock>();
                for (int questions = 0; questions < form1.MaxQuestionOnListUint;)
                {
                    var randBlock = RandomNumber.Between(0, randBlockCopy.Count - 1);
                    if (!uniqueQuestion.Contains(randBlockCopy[randBlock]))
                    {
                        if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol1))
                        {
                            if (bannedSymbol1Meets) continue;
                        }
                        if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol2))
                        {
                            if (bannedSymbol2Meets) continue;
                        }
                        if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol3))
                        {
                            if (bannedSymbol3Meets) continue;
                        }
                        if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol4))
                        {
                            if (bannedSymbol4Meets) continue;
                        }
                        if (randBlockCopy[randBlock].AllowPrint(form1.MaxQuestonRepeatUint, form1.DateThenAllowPrint))
                        {
                            if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol1))
                            {
                                bannedSymbol1Meets = true;
                                uniqueQuestion.Add(randBlockCopy[randBlock]);
                                ++questions;
                            }
                            else if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol2))
                            {
                                bannedSymbol2Meets = true;
                                uniqueQuestion.Add(randBlockCopy[randBlock]);
                                ++questions;
                            }
                            else if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol3))
                            {
                                bannedSymbol3Meets = true;
                                uniqueQuestion.Add(randBlockCopy[randBlock]);
                                ++questions;
                            }
                            else if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol4))
                            {
                                bannedSymbol4Meets = true;
                                uniqueQuestion.Add(randBlockCopy[randBlock]);
                                ++questions;
                            }
                            else
                            {
                                uniqueQuestion.Add(randBlockCopy[randBlock]);
                                ++questions;
                            }
                        }
                    }
                    if (someTimer >= 60000)
                    {
                        form1.OnErrorHappen("Ошибка комбинации возможных вариантов \n\rПроверьте вводимые данные и количество доступных вопросов");
                        ErrorLog.AddNewEntry("Вопросов_на_лист=" + form1.MaxQuestionOnListUint + " | Макс_повторов_вопросов= " +
                                             form1.MaxQuestonRepeatUint + " | Всего_листов= " + form1.MaxLists);
                        if(File.Exists(form1.ChooseFolderPath + @"\" + Form1.FileNameListText))
                        File.Copy(form1.ChooseFolderPath + @"\" + Form1.FileNameListText,
                            "ErrorList" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".csv");
                        return true;
                    }
                    ++someTimer;
                }
                ++listNumber;
            }
            return false;
        }

        public static List<DoneBlock> CopyDoneBlocks(List<DoneBlock> doneBlocks)
        {
            List<DoneBlock> copyedList = doneBlocks.Select(doneBlock => new DoneBlock(doneBlock.QuestionPath, doneBlock.AnswerPath, doneBlock.LastTimePrint, doneBlock.TimeRepeated)).ToList();
            return copyedList;
        }
    }

}