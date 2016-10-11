using System;
using System.Windows.Forms;
using Doctrina.Enums;
using IniHlp;

namespace Config
{
    public partial class SettingsForm : Form
    {
        private IniHlpClass IniHlp;
        private const string EasyQuestionString = "Easy=";
        private const string MiddleQuestionString = "Middle=";
        private const string HardQuestionString = "Hard=";
        private const string ModeString = "Mode=";
        public WorkLikeEnum CurrentWorkEnum = WorkLikeEnum.OnlyGenerator;
        public int LSTEasyNumber = 0;
        public int LSTMiddleNumber = 0;
        public int LSTHardNumber = 0;
        public SettingsForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.F5 && e.Alt && e.Control)
            {
                MessageBox.Show(IniHlp.IniFileName);
                e.Handled = true;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            InitMyComponents();
            SetDataToUI();
            Save_button.Enabled = false;
        }

        private void InitMyComponents()
        {
            try
            {
                string[] args = Environment.GetCommandLineArgs();
                if (args.Length<=1)
                    IniHlp = new IniHlpClass("config.ini");
                else
                {
                    IniHlp = new IniHlpClass(args[1]);
                }
                LSTEasyNumber = IniHlp.GetIntFromLine(EasyQuestionString);
                LSTMiddleNumber = IniHlp.GetIntFromLine(MiddleQuestionString);
                LSTHardNumber = IniHlp.GetIntFromLine(HardQuestionString);
                switch (IniHlp.GetIntFromLine(ModeString))
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
            }
            catch (Exception exception)
            {
                Error.OnErrorHappen(exception.Message);
                Environment.Exit(0);
            }
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
            Save_button.Enabled = false;
        }
        /// <summary>
        /// Если были изменения в форме, активировать кнопку сохранения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enabled_SaveButton(object sender, EventArgs e)
        {
            Save_button.Enabled = true;
        }
        private void SaveAll()
        {
            IniHlp.SaveDataToIniFile(EasyQuestionString, EasytextBox.Text);
            IniHlp.SaveDataToIniFile(MiddleQuestionString, MiddletextBox.Text);
            IniHlp.SaveDataToIniFile(HardQuestionString, HardtextBox.Text);
            IniHlp.SaveDataToIniFile(ModeString, Convert.ToInt32(CurrentWorkEnum).ToString());
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
