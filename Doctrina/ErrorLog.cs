using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public static class ErrorLog
{
    [DllImport("dbghelp.dll")]
    public static extern bool MiniDumpWriteDump(IntPtr hProcess, Int32 ProcessId, IntPtr hFile, int DumpType, IntPtr ExceptionParam, IntPtr UserStreamParam, IntPtr CallackParam);

    public static void AddNewEntry(string text)
    {
        if (!File.Exists("errorLog.log"))
        {
            File.Create("errorLog.log").Close();
        }
        using (var sw = new StreamWriter("errorLog.log",true))
        {
            sw.WriteLine("{0}:{1}", DateTime.Now, text);
            sw.Close();
        }
    }

    public static void CreateDump()
    {
        using (FileStream fs = new FileStream(DateTime.Now.Month+"_"+DateTime.Now.Day+"_"+DateTime.Now.Hour+"_"+DateTime.Now.Minute+"_"+DateTime.Now.Second+".dmp", FileMode.Create))
        {
            var pr = Process.GetCurrentProcess();
            MiniDumpWriteDump(pr.Handle, pr.Id, fs.SafeFileHandle.DangerousGetHandle(), 0x00000002, IntPtr.Zero, IntPtr.Zero,
                IntPtr.Zero);
        }
            
    }
}

