using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Doctrina.Enums;
using Config;

namespace Config
{
    public partial class Настройка : Form
    {
        
        private const string EasyQuestionString = "Easy=";
        private const string MiddleQuestionString = "Middle=";
        private const string HardQuestionString = "Hard=";
        private const string ModeString = "Mode=";
        public WorkLikeEnum CurrentWorkEnum = WorkLikeEnum.OnlyGenerator;
        public int LSTEasyNumber = 0;
        public int LSTMiddleNumber = 0;
        public int LSTHardNumber = 0;
        /// <summary>
        /// Позиция элемента в документе
        /// 0 - Легкий
        /// 1- Средний 
        /// 2 - Тяжелый
        /// 3 - Способ работы
        /// </summary>
        public int [] PositionArray =new int[4];
        public Настройка()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitMyComponents();
        }
        private void InitMyComponents()
        {
            if (!File.Exists("config.ini"))
            {
                Error.OnErrorHappen("Файл config.ini не найден. Запуск невозможен");
                Environment.Exit(0);
            }

            var lines = File.ReadAllLines("config.ini", Encoding.Default);
            int lineCounter = -1;
            foreach (var line in lines)
            {
                ++lineCounter;
                if (line.Contains(EasyQuestionString))
                {
                    var tempstring = line.Replace(EasyQuestionString, "");
                    LSTEasyNumber = Convert.ToInt32(tempstring);
                    PositionArray[0] = lineCounter;
                }
                if (line.Contains(MiddleQuestionString))
                {
                    var tempstring = line.Replace(MiddleQuestionString, "");
                    LSTMiddleNumber = Convert.ToInt32(tempstring);
                    PositionArray[1] = lineCounter;
                }
                if (line.Contains(HardQuestionString))
                {
                    var tempstring = line.Replace(HardQuestionString, "");
                    LSTHardNumber = Convert.ToInt32(tempstring);
                    PositionArray[2] = lineCounter;
                }
                if (line.Contains(ModeString))
                {
                    var tempstring = line.Replace(ModeString, "");
                    switch (Convert.ToInt32(tempstring))
                    {
                        case 1:
                            {
                                CurrentWorkEnum = WorkLikeEnum.OnlyGenerator;
                                break;
                            }
                        case 2:
                            {
                                CurrentWorkEnum = WorkLikeEnum.GeneratorAndConst;
                                break;
                            }
                        case 3:
                            {
                                CurrentWorkEnum = WorkLikeEnum.GeneratorAndLST;
                                break;
                            }
                        default:
                            Error.OnErrorHappen("Не указан режим работы");
                            break;
                    }
                    PositionArray[3] = lineCounter;
                }
            }
            SetDataToUI();
        }

        private void SetDataToUI()
        {
            switch (CurrentWorkEnum)
            {
                case WorkLikeEnum.OnlyGenerator:
                {
                    OnlyGeneratorRadioButon.Checked = true;
                    GeneratorAndConstRadioButton.Checked = false;
                    GeneratorAndLSTradioButton.Checked = false;

                    break;
                }
                case WorkLikeEnum.GeneratorAndConst:
                {
                    OnlyGeneratorRadioButon.Checked = false;
                    GeneratorAndConstRadioButton.Checked = true;
                    GeneratorAndLSTradioButton.Checked = false;
                    break;
                }
                case WorkLikeEnum.GeneratorAndLST:
                {
                    OnlyGeneratorRadioButon.Checked = false;
                    GeneratorAndConstRadioButton.Checked = false;
                    GeneratorAndLSTradioButton.Checked = true;
                    break;
                }
            }
            EasytextBox.Text=LSTEasyNumber.ToString();
            MiddletextBox.Text=LSTMiddleNumber.ToString();
            HardtextBox.Text=LSTHardNumber.ToString();
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            if(OnlyGeneratorRadioButon.Checked)
                CurrentWorkEnum=WorkLikeEnum.OnlyGenerator;
            if(GeneratorAndConstRadioButton.Checked)
                CurrentWorkEnum = WorkLikeEnum.GeneratorAndConst;
            if(GeneratorAndLSTradioButton.Checked)
                CurrentWorkEnum = WorkLikeEnum.GeneratorAndLST;
            SaveAll();
        }

        private void SaveAll()
        {
            var lines = File.ReadAllLines("config.ini", Encoding.Default);
            lines[PositionArray[0]] = EasyQuestionString + EasytextBox.Text;
            lines[PositionArray[1]] = MiddleQuestionString + MiddletextBox.Text;
            lines[PositionArray[2]] = HardQuestionString + HardtextBox.Text;
            lines[PositionArray[3]] = ModeString + Convert.ToInt32(CurrentWorkEnum);
            File.WriteAllLines("config.ini", lines);
        }

        private void EasytextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIsDigit(e);
        }

        private void MiddletextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIsDigit(e);
        }

        private void HardtextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckIsDigit(e);
        }
        private static void CheckIsDigit(KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }
    }
}
