using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Doctrina
{
    internal static class CheckClass
    {
        public static bool Cheks(Form1 form1,string selectedFolder)
        {
            if (folderCheck(form1,selectedFolder))
                return true;
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
                bool bannedSymbol5Meets = false;
                bool bannedSymbol6Meets = false;
                bool bannedSymbol7Meets = false;
                bool bannedSymbol8Meets = false;
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
                        if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol5))
                        {
                            if (bannedSymbol5Meets) continue;
                        }
                        if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol6))
                        {
                            if (bannedSymbol6Meets) continue;
                        }
                        if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol7))
                        {
                            if (bannedSymbol7Meets) continue;
                        }
                        if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol8))
                        {
                            if (bannedSymbol8Meets) continue;
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
                            else if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol5))
                            {
                                bannedSymbol5Meets = true;
                                uniqueQuestion.Add(randBlockCopy[randBlock]);
                                ++questions;
                            }
                            else if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol6))
                            {
                                bannedSymbol6Meets = true;
                                uniqueQuestion.Add(randBlockCopy[randBlock]);
                                ++questions;
                            }
                            else if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol7))
                            {
                                bannedSymbol7Meets = true;
                                uniqueQuestion.Add(randBlockCopy[randBlock]);
                                ++questions;
                            }
                            else if (randBlockCopy[randBlock].ShortFileName.Contains(BannedSymbols.BannedSymbol8))
                            {
                                bannedSymbol8Meets = true;
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

        public static bool folderCheck(Form1 form1,string folderPath)
        {
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