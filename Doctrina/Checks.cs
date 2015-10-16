using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

namespace Doctrina
{
    internal static class CheckClass
    {
        public static bool Cheks(Form1 form1)
        {

            if (form1.MaxLists > form1.MaxQuestionOnListUint*form1.MaxQuestonRepeatUint*form1.doneBlocks.Count)
            {
                form1.OnErrorHappen("������ ������ ��� ��������� ����������");
                return true;
            }
            if (form1.MaxQuestonRepeatUint > form1.doneBlocks.Count)
            {
                form1.OnErrorHappen("���������� �������� �� �������� ������� �������");
                return true;
            }
            if (form1.MaxQuestionOnListUint > form1.doneBlocks.Count)
            {
                form1.OnErrorHappen("���������� �������� �� �������� ������, ��� ����� ��������");
                return true;
            }
            if (RepeatCheck(form1))
            {
                form1.OnErrorHappen("���������� ��������� �������� ��������� ������� ����");
                return true;
            }
            //if (IsQuestionEnAllList())
            // {
            //     OnErrorHappen("���������� ��������� �������� �� ��� ��������� ������, ��� ����������");
            //     return;
            //  }
           return CheckAlllOther(form1);
        }

        private static bool IsQuestionEnAllList(Form1 form1) //���� �� ������� ��� ������ ��������� �� ��� ��������
        {
            List<int> rowIndex = new List<int>();
            int currentIndex = 0;
            foreach (DataRow row in form1.myDT.Rows)
            {
                if ((uint) row[1] >= form1.MaxQuestonRepeatUint)
                    if ((DateTime) row[2] <= form1.DateThenAllowPrint)
                        rowIndex.Add(currentIndex);
                ++currentIndex;
            }
            if (form1.myDT.Rows.Count - rowIndex.Count >= form1.MaxLists*form1.MaxQuestonRepeatUint)
                //BUG:���-�� ��������(������ �� ���� ��������, � ���� �� ���)-����� ����
                return false;
            return true;
        }

        private static bool RepeatCheck(Form1 form1)
        {
            uint totalRepeat = form1.doneBlocks.Aggregate<DoneBlock, uint>(0,
                (current, block) => current + block.TimeRepeated);
            var needRepeat = form1.MaxLists*form1.MaxQuestonRepeatUint*form1.MaxQuestionOnListUint;
            if (totalRepeat <= needRepeat)
                return false;
            return true;
        }

        private static bool CheckAlllOther(Form1 form1)
        {
            List<DoneBlock> randBlockCopy = CopyDoneBlocks(form1.doneBlocks);
            uint someTimer = 0;
            bool bannedSymbolMeets = false;
            for (int listNumber = 0; listNumber < form1.MaxLists;)
            {
                List<DoneBlock> uniqueQuestion = new List<DoneBlock>();
                for (int questions = 0; questions < form1.MaxQuestionOnListUint;)
                {
                    var randBlock = RandomNumber.Between(0, randBlockCopy.Count - 1);
                    if (!uniqueQuestion.Contains(randBlockCopy[randBlock]))
                    {
                        if (randBlockCopy[randBlock].AllowPrint(form1.MaxQuestonRepeatUint, form1.DateThenAllowPrint))
                        {
                            if (randBlockCopy[randBlock].AnswerPath.Contains('!'))
                            {
                                if (bannedSymbolMeets) continue;
                                bannedSymbolMeets = true;
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
                        form1.OnErrorHappen("������ ���������� ��������� ���������");
                        ErrorLog.AddNewEntry("��������_��_����=" + form1.MaxQuestionOnListUint + " | ����_��������_��������= " +
                                             form1.MaxQuestonRepeatUint + " | �����_������= " + form1.MaxLists);
                        File.Copy(form1._chooseFolderPath + @"\" + Form1.fileNameListText,
                            "ErrorList" + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".csv");
                        return true;
                    }
                    ++someTimer;
                }
                ++listNumber;
            }
            return false;
        }

        private static List<DoneBlock> CopyDoneBlocks(List<DoneBlock> doneBlocks)
        {
             List <DoneBlock> copyedList = doneBlocks.Select(doneBlock => new DoneBlock(doneBlock.QuestionPath, doneBlock.AnswerPath, doneBlock.LastTimePrint, doneBlock.TimeRepeated)).ToList();
            return copyedList;
        }
    }

}
