namespace Doctrina
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.chooseFolderButton = new System.Windows.Forms.Button();
            this.MaxQuestionOnListText = new System.Windows.Forms.TextBox();
            this.MaxQuestionRepeatText = new System.Windows.Forms.TextBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.NumberOfLists = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.dateThenAllowPrintPicker = new System.Windows.Forms.DateTimePicker();
            this.datagridForDataTable = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.currentFolderTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.allCheckBox = new System.Windows.Forms.CheckBox();
            this.questionCheckBox = new System.Windows.Forms.CheckBox();
            this.answerCheckBox = new System.Windows.Forms.CheckBox();
            this.currentStatusTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.PrintLastButton = new System.Windows.Forms.Button();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.OnlyGeneratorRadioButon = new System.Windows.Forms.RadioButton();
            this.GeneratorAndConstRadioButton = new System.Windows.Forms.RadioButton();
            this.GeneratorAndLSTradioButton = new System.Windows.Forms.RadioButton();
            this.EasytextBox = new System.Windows.Forms.TextBox();
            this.MiddletextBox = new System.Windows.Forms.TextBox();
            this.HardtextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.datagridForDataTable)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chooseFolderButton
            // 
            this.chooseFolderButton.Location = new System.Drawing.Point(26, 406);
            this.chooseFolderButton.Name = "chooseFolderButton";
            this.chooseFolderButton.Size = new System.Drawing.Size(91, 23);
            this.chooseFolderButton.TabIndex = 0;
            this.chooseFolderButton.Text = "ВыбратьПапку";
            this.chooseFolderButton.UseVisualStyleBackColor = true;
            this.chooseFolderButton.Click += new System.EventHandler(this.chooseFolderButton_Click);
            // 
            // MaxQuestionOnListText
            // 
            this.MaxQuestionOnListText.Location = new System.Drawing.Point(193, 75);
            this.MaxQuestionOnListText.Name = "MaxQuestionOnListText";
            this.MaxQuestionOnListText.Size = new System.Drawing.Size(100, 20);
            this.MaxQuestionOnListText.TabIndex = 3;
            this.MaxQuestionOnListText.Text = "1";
            this.MaxQuestionOnListText.TextChanged += new System.EventHandler(this.MaxQuestionOnListText_TextChanged);
            this.MaxQuestionOnListText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MaxQuestionOnListText_KeyPress);
            // 
            // MaxQuestionRepeatText
            // 
            this.MaxQuestionRepeatText.Location = new System.Drawing.Point(193, 101);
            this.MaxQuestionRepeatText.Name = "MaxQuestionRepeatText";
            this.MaxQuestionRepeatText.Size = new System.Drawing.Size(100, 20);
            this.MaxQuestionRepeatText.TabIndex = 4;
            this.MaxQuestionRepeatText.Text = "1";
            this.MaxQuestionRepeatText.TextChanged += new System.EventHandler(this.MaxQuestionRepeatText_TextChanged);
            this.MaxQuestionRepeatText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MaxQuestionRepeatText_KeyPress);
            // 
            // RunButton
            // 
            this.RunButton.Enabled = false;
            this.RunButton.Location = new System.Drawing.Point(149, 406);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 23);
            this.RunButton.TabIndex = 5;
            this.RunButton.Text = "Запуск";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // NumberOfLists
            // 
            this.NumberOfLists.Location = new System.Drawing.Point(193, 49);
            this.NumberOfLists.Name = "NumberOfLists";
            this.NumberOfLists.Size = new System.Drawing.Size(100, 20);
            this.NumberOfLists.TabIndex = 6;
            this.NumberOfLists.Text = "3";
            this.NumberOfLists.TextChanged += new System.EventHandler(this.NumberOfLists_TextChanged);
            this.NumberOfLists.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumberOfLists_KeyPress);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(6, 52);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(129, 13);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "Количество документов";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Количество вопросов на документ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Количество повторов вопроса";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(10, 377);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(340, 23);
            this.progressBar1.TabIndex = 10;
            // 
            // dateThenAllowPrintPicker
            // 
            this.dateThenAllowPrintPicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateThenAllowPrintPicker.Location = new System.Drawing.Point(193, 127);
            this.dateThenAllowPrintPicker.Name = "dateThenAllowPrintPicker";
            this.dateThenAllowPrintPicker.Size = new System.Drawing.Size(129, 20);
            this.dateThenAllowPrintPicker.TabIndex = 11;
            this.dateThenAllowPrintPicker.ValueChanged += new System.EventHandler(this.dateThenAllowPrintPicker_ValueChanged);
            // 
            // datagridForDataTable
            // 
            this.datagridForDataTable.AllowUserToAddRows = false;
            this.datagridForDataTable.AllowUserToDeleteRows = false;
            this.datagridForDataTable.AllowUserToResizeColumns = false;
            this.datagridForDataTable.AllowUserToResizeRows = false;
            this.datagridForDataTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagridForDataTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.datagridForDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridForDataTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.datagridForDataTable.Location = new System.Drawing.Point(355, 12);
            this.datagridForDataTable.MultiSelect = false;
            this.datagridForDataTable.Name = "datagridForDataTable";
            this.datagridForDataTable.RowHeadersVisible = false;
            this.datagridForDataTable.Size = new System.Drawing.Size(493, 341);
            this.datagridForDataTable.TabIndex = 12;
            this.datagridForDataTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.datagridForDataTable_CellEndEdit);
            this.datagridForDataTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.datagridForDataTable_ColumnHeaderMouseClick);
            this.datagridForDataTable.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.datagridForDataTable_ColumnHeaderMouseDoubleClick);
            this.datagridForDataTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.datagridForDataTable_DataError);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // currentFolderTextBox
            // 
            this.currentFolderTextBox.Location = new System.Drawing.Point(9, 12);
            this.currentFolderTextBox.Name = "currentFolderTextBox";
            this.currentFolderTextBox.ReadOnly = true;
            this.currentFolderTextBox.Size = new System.Drawing.Size(340, 20);
            this.currentFolderTextBox.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Ограничение по времени";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.allCheckBox);
            this.groupBox1.Controls.Add(this.questionCheckBox);
            this.groupBox1.Controls.Add(this.answerCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(13, 235);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Печать";
            // 
            // allCheckBox
            // 
            this.allCheckBox.AutoSize = true;
            this.allCheckBox.Checked = true;
            this.allCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allCheckBox.Location = new System.Drawing.Point(68, 56);
            this.allCheckBox.Name = "allCheckBox";
            this.allCheckBox.Size = new System.Drawing.Size(45, 17);
            this.allCheckBox.TabIndex = 19;
            this.allCheckBox.Text = "Всё";
            this.allCheckBox.UseVisualStyleBackColor = true;
            this.allCheckBox.CheckedChanged += new System.EventHandler(this.allCheckBox_CheckedChanged);
            // 
            // questionCheckBox
            // 
            this.questionCheckBox.AutoSize = true;
            this.questionCheckBox.Checked = true;
            this.questionCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.questionCheckBox.Enabled = false;
            this.questionCheckBox.Location = new System.Drawing.Point(111, 33);
            this.questionCheckBox.Name = "questionCheckBox";
            this.questionCheckBox.Size = new System.Drawing.Size(71, 17);
            this.questionCheckBox.TabIndex = 18;
            this.questionCheckBox.Text = "Вопросы";
            this.questionCheckBox.UseVisualStyleBackColor = true;
            // 
            // answerCheckBox
            // 
            this.answerCheckBox.AutoSize = true;
            this.answerCheckBox.Checked = true;
            this.answerCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.answerCheckBox.Enabled = false;
            this.answerCheckBox.Location = new System.Drawing.Point(6, 33);
            this.answerCheckBox.Name = "answerCheckBox";
            this.answerCheckBox.Size = new System.Drawing.Size(64, 17);
            this.answerCheckBox.TabIndex = 17;
            this.answerCheckBox.Text = "Ответы";
            this.answerCheckBox.UseVisualStyleBackColor = true;
            // 
            // currentStatusTextBox
            // 
            this.currentStatusTextBox.Location = new System.Drawing.Point(10, 351);
            this.currentStatusTextBox.Name = "currentStatusTextBox";
            this.currentStatusTextBox.ReadOnly = true;
            this.currentStatusTextBox.Size = new System.Drawing.Size(340, 20);
            this.currentStatusTextBox.TabIndex = 17;
            this.currentStatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(248, 406);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 18;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Visible = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // PrintLastButton
            // 
            this.PrintLastButton.Location = new System.Drawing.Point(236, 270);
            this.PrintLastButton.Name = "PrintLastButton";
            this.PrintLastButton.Size = new System.Drawing.Size(75, 38);
            this.PrintLastButton.TabIndex = 19;
            this.PrintLastButton.Text = "Печатать остатки";
            this.PrintLastButton.UseVisualStyleBackColor = true;
            this.PrintLastButton.Visible = false;
            this.PrintLastButton.Click += new System.EventHandler(this.PrintLast_Click);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.WorkerReportsProgress = true;
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            // 
            // OnlyGeneratorRadioButon
            // 
            this.OnlyGeneratorRadioButon.AutoSize = true;
            this.OnlyGeneratorRadioButon.Checked = true;
            this.OnlyGeneratorRadioButon.Location = new System.Drawing.Point(17, 439);
            this.OnlyGeneratorRadioButon.Name = "OnlyGeneratorRadioButon";
            this.OnlyGeneratorRadioButon.Size = new System.Drawing.Size(129, 17);
            this.OnlyGeneratorRadioButon.TabIndex = 20;
            this.OnlyGeneratorRadioButon.TabStop = true;
            this.OnlyGeneratorRadioButon.Text = "Стандартный режим";
            this.OnlyGeneratorRadioButon.UseVisualStyleBackColor = true;
            this.OnlyGeneratorRadioButon.CheckedChanged += new System.EventHandler(this.OnlyGeneratorRadioButon_CheckedChanged);
            // 
            // GeneratorAndConstRadioButton
            // 
            this.GeneratorAndConstRadioButton.AutoSize = true;
            this.GeneratorAndConstRadioButton.Location = new System.Drawing.Point(156, 439);
            this.GeneratorAndConstRadioButton.Name = "GeneratorAndConstRadioButton";
            this.GeneratorAndConstRadioButton.Size = new System.Drawing.Size(193, 17);
            this.GeneratorAndConstRadioButton.TabIndex = 21;
            this.GeneratorAndConstRadioButton.Text = "Стандартный режим + константа";
            this.GeneratorAndConstRadioButton.UseVisualStyleBackColor = true;
            this.GeneratorAndConstRadioButton.CheckedChanged += new System.EventHandler(this.GeneratorAndConstRadioButton_CheckedChanged);
            // 
            // GeneratorAndLSTradioButton
            // 
            this.GeneratorAndLSTradioButton.AutoSize = true;
            this.GeneratorAndLSTradioButton.Location = new System.Drawing.Point(370, 439);
            this.GeneratorAndLSTradioButton.Name = "GeneratorAndLSTradioButton";
            this.GeneratorAndLSTradioButton.Size = new System.Drawing.Size(163, 17);
            this.GeneratorAndLSTradioButton.TabIndex = 22;
            this.GeneratorAndLSTradioButton.TabStop = true;
            this.GeneratorAndLSTradioButton.Text = "Стандартный режим + ЛСТ";
            this.GeneratorAndLSTradioButton.UseVisualStyleBackColor = true;
            this.GeneratorAndLSTradioButton.CheckedChanged += new System.EventHandler(this.GeneratorAndLSTradioButton_CheckedChanged);
            // 
            // EasytextBox
            // 
            this.EasytextBox.Location = new System.Drawing.Point(193, 163);
            this.EasytextBox.Name = "EasytextBox";
            this.EasytextBox.Size = new System.Drawing.Size(100, 20);
            this.EasytextBox.TabIndex = 23;
            this.EasytextBox.Text = "0";
            this.EasytextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EasytextBox_KeyPress);
            // 
            // MiddletextBox
            // 
            this.MiddletextBox.Location = new System.Drawing.Point(193, 189);
            this.MiddletextBox.Name = "MiddletextBox";
            this.MiddletextBox.Size = new System.Drawing.Size(100, 20);
            this.MiddletextBox.TabIndex = 24;
            this.MiddletextBox.Text = "0";
            this.MiddletextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MiddletextBox_KeyPress);
            // 
            // HardtextBox
            // 
            this.HardtextBox.Location = new System.Drawing.Point(193, 215);
            this.HardtextBox.Name = "HardtextBox";
            this.HardtextBox.Size = new System.Drawing.Size(100, 20);
            this.HardtextBox.TabIndex = 25;
            this.HardtextBox.Text = "0";
            this.HardtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HardtextBox_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Легкий";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 189);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Средний";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 215);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "Тяжелый";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(552, 359);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 29;
            this.SaveButton.Text = "Сохранить";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 468);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.HardtextBox);
            this.Controls.Add(this.MiddletextBox);
            this.Controls.Add(this.EasytextBox);
            this.Controls.Add(this.GeneratorAndLSTradioButton);
            this.Controls.Add(this.GeneratorAndConstRadioButton);
            this.Controls.Add(this.OnlyGeneratorRadioButon);
            this.Controls.Add(this.PrintLastButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.currentStatusTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.currentFolderTextBox);
            this.Controls.Add(this.datagridForDataTable);
            this.Controls.Add(this.dateThenAllowPrintPicker);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.NumberOfLists);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.MaxQuestionRepeatText);
            this.Controls.Add(this.MaxQuestionOnListText);
            this.Controls.Add(this.chooseFolderButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.datagridForDataTable)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderDialog;
        private System.Windows.Forms.Button chooseFolderButton;
        private System.Windows.Forms.TextBox MaxQuestionOnListText;
        private System.Windows.Forms.TextBox MaxQuestionRepeatText;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.TextBox NumberOfLists;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar1;
        internal System.Windows.Forms.DateTimePicker dateThenAllowPrintPicker;
        internal System.Windows.Forms.DataGridView datagridForDataTable;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox currentFolderTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox allCheckBox;
        private System.Windows.Forms.CheckBox questionCheckBox;
        private System.Windows.Forms.CheckBox answerCheckBox;
        private System.Windows.Forms.TextBox currentStatusTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button PrintLastButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.RadioButton OnlyGeneratorRadioButon;
        private System.Windows.Forms.RadioButton GeneratorAndConstRadioButton;
        private System.Windows.Forms.RadioButton GeneratorAndLSTradioButton;
        private System.Windows.Forms.TextBox EasytextBox;
        private System.Windows.Forms.TextBox MiddletextBox;
        private System.Windows.Forms.TextBox HardtextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button SaveButton;
    }
}

