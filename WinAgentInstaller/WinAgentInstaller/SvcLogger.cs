using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentInstaller
{
    public static class SvcLogger
    {
        public static void log(string msg)
        {
            DateTime w_dteNow = DateTime.Now;
            string w_strNow = w_dteNow.ToString("dd.MM.yyyy_hh:mm:ss");
            Console.WriteLine(msg);
            lock (Program.g_objLock)
            {
                File.AppendAllText(Program.g_log_path, $"{w_strNow}: {msg}\n");    /////
            }
        }
    }
}
