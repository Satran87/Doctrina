using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Doctrina;

namespace Config
{
    public static class Error
    {
        public static void OnErrorHappen(string text)
        {
            MessageBox.Show(text, "Произошла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ErrorLog.AddNewEntry(text);
        }
        public static bool OnErrorHappenYesNo(string text)
        {
            var code = MessageBox.Show(text, "Произошла ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            ErrorLog.AddNewEntry(text);
            if (code == DialogResult.Yes)
                return true;
            return false;
        }
    }
}
