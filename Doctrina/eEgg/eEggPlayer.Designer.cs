namespace Doctrina.eEgg
{
    partial class EEggPlayer
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
            this.boxForVideo = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.boxForVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // boxForVideo
            // 
            this.boxForVideo.Location = new System.Drawing.Point(12, 12);
            this.boxForVideo.Name = "boxForVideo";
            this.boxForVideo.Size = new System.Drawing.Size(399, 238);
            this.boxForVideo.TabIndex = 0;
            this.boxForVideo.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 33000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // EEggPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 262);
            this.Controls.Add(this.boxForVideo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EEggPlayer";
            this.Text = "eEggPlayer";
            this.Load += new System.EventHandler(this.eEggPlayer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.boxForVideo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox boxForVideo;
        private System.Windows.Forms.Timer timer1;
    }
}