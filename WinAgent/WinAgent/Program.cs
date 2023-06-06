using MailParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAgent
{
    internal static class Program
    {
        public static UserSetting g_setting = new UserSetting();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            g_setting = UserSetting.Load();
            if (g_setting == null)
            {
                g_setting = new UserSetting();
                g_setting.Save();
            }
            frmActivate w_frmActivate = new frmActivate();
            if (w_frmActivate.ShowDialog() != DialogResult.OK)
                Environment.Exit(0);

            Application.Run(new Form1());
        }
    }
}
