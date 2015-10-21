using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Doctrina.eEgg
{
   
    public partial class EEggPlayer : Form
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returned, int len, IntPtr callback);
        public EEggPlayer()
        {
            InitializeComponent();
        }

        private void eEggPlayer_Load(object sender, EventArgs e)
        {
            timer1.Start();
            try
            {
                File.WriteAllBytes("eEgg.avi", Resource1.eEgg);
                videoplayer("eEgg.avi", boxForVideo);
            }
            catch (Exception er)
            {
                ErrorLog.AddNewEntry("Шутка не проигралась -("+er.Message);
            }
            
        }
        public void videoplayer(string filename, PictureBox pb)
        {
            string command = String.Empty;

            command = "open \"" + filename + "\" alias VideoFile wait";
            mciSendString(command, null, 0, IntPtr.Zero);

            command = "window VideoFile handle " + pb.Handle;
            mciSendString(command, null, 0, IntPtr.Zero);

            command = "put VideoFile destination at 0 0 " + pb.Width + " " + pb.Height + " wait";
            mciSendString(command, null, 0, IntPtr.Zero);

            command = "play VideoFile";
            mciSendString(command, null, 0, IntPtr.Zero);
        }
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            try
            {
                mciSendString("stop VideoFile", null, 0, IntPtr.Zero);
                mciSendString("close VideoFile", null, 0, IntPtr.Zero);
                Thread.Sleep(100);
                if (File.Exists("eEgg.avi"))
                {
                    File.Delete("eEgg.avi");
                }
                this.Close();
            }
            catch (Exception er)
            {
                ErrorLog.AddNewEntry("Шутка фигово остановилась" +er.Message);
                throw;
            }
            
        }
    }
}
