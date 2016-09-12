namespace Config
{
    partial class Настройка
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
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.HardtextBox = new System.Windows.Forms.TextBox();
            this.MiddletextBox = new System.Windows.Forms.TextBox();
            this.EasytextBox = new System.Windows.Forms.TextBox();
            this.GeneratorAndLSTradioButton = new System.Windows.Forms.RadioButton();
            this.GeneratorAndConstRadioButton = new System.Windows.Forms.RadioButton();
            this.OnlyGeneratorRadioButon = new System.Windows.Forms.RadioButton();
            this.Save_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-1, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Тяжелый";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-1, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Средний";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-1, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "Легкий";
            // 
            // HardtextBox
            // 
            this.HardtextBox.Location = new System.Drawing.Point(55, 144);
            this.HardtextBox.Name = "HardtextBox";
            this.HardtextBox.Size = new System.Drawing.Size(100, 20);
            this.HardtextBox.TabIndex = 31;
            this.HardtextBox.Text = "0";
            this.HardtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HardtextBox_KeyPress);
            // 
            // MiddletextBox
            // 
            this.MiddletextBox.Location = new System.Drawing.Point(55, 118);
            this.MiddletextBox.Name = "MiddletextBox";
            this.MiddletextBox.Size = new System.Drawing.Size(100, 20);
            this.MiddletextBox.TabIndex = 30;
            this.MiddletextBox.Text = "0";
            this.MiddletextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MiddletextBox_KeyPress);
            // 
            // EasytextBox
            // 
            this.EasytextBox.Location = new System.Drawing.Point(55, 92);
            this.EasytextBox.Name = "EasytextBox";
            this.EasytextBox.Size = new System.Drawing.Size(100, 20);
            this.EasytextBox.TabIndex = 29;
            this.EasytextBox.Text = "0";
            this.EasytextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EasytextBox_KeyPress);
            // 
            // GeneratorAndLSTradioButton
            // 
            this.GeneratorAndLSTradioButton.AutoSize = true;
            this.GeneratorAndLSTradioButton.Location = new System.Drawing.Point(215, 140);
            this.GeneratorAndLSTradioButton.Name = "GeneratorAndLSTradioButton";
            this.GeneratorAndLSTradioButton.Size = new System.Drawing.Size(163, 17);
            this.GeneratorAndLSTradioButton.TabIndex = 37;
            this.GeneratorAndLSTradioButton.TabStop = true;
            this.GeneratorAndLSTradioButton.Text = "Стандартный режим + ЛСТ";
            this.GeneratorAndLSTradioButton.UseVisualStyleBackColor = true;
            // 
            // GeneratorAndConstRadioButton
            // 
            this.GeneratorAndConstRadioButton.AutoSize = true;
            this.GeneratorAndConstRadioButton.Location = new System.Drawing.Point(215, 117);
            this.GeneratorAndConstRadioButton.Name = "GeneratorAndConstRadioButton";
            this.GeneratorAndConstRadioButton.Size = new System.Drawing.Size(193, 17);
            this.GeneratorAndConstRadioButton.TabIndex = 36;
            this.GeneratorAndConstRadioButton.Text = "Стандартный режим + константа";
            this.GeneratorAndConstRadioButton.UseVisualStyleBackColor = true;
            // 
            // OnlyGeneratorRadioButon
            // 
            this.OnlyGeneratorRadioButon.AutoSize = true;
            this.OnlyGeneratorRadioButon.Checked = true;
            this.OnlyGeneratorRadioButon.Location = new System.Drawing.Point(215, 94);
            this.OnlyGeneratorRadioButon.Name = "OnlyGeneratorRadioButon";
            this.OnlyGeneratorRadioButon.Size = new System.Drawing.Size(129, 17);
            this.OnlyGeneratorRadioButon.TabIndex = 35;
            this.OnlyGeneratorRadioButon.TabStop = true;
            this.OnlyGeneratorRadioButon.Text = "Стандартный режим";
            this.OnlyGeneratorRadioButon.UseVisualStyleBackColor = true;
            // 
            // Save_button
            // 
            this.Save_button.Location = new System.Drawing.Point(333, 213);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(75, 23);
            this.Save_button.TabIndex = 38;
            this.Save_button.Text = "Сохранить";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            // 
            // Настройка
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 248);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.GeneratorAndLSTradioButton);
            this.Controls.Add(this.GeneratorAndConstRadioButton);
            this.Controls.Add(this.OnlyGeneratorRadioButon);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.HardtextBox);
            this.Controls.Add(this.MiddletextBox);
            this.Controls.Add(this.EasytextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(428, 282);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(428, 282);
            this.Name = "Настройка";
            this.Text = "Настройка";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox HardtextBox;
        private System.Windows.Forms.TextBox MiddletextBox;
        private System.Windows.Forms.TextBox EasytextBox;
        private System.Windows.Forms.RadioButton GeneratorAndLSTradioButton;
        private System.Windows.Forms.RadioButton GeneratorAndConstRadioButton;
        private System.Windows.Forms.RadioButton OnlyGeneratorRadioButon;
        private System.Windows.Forms.Button Save_button;
    }
}

