using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAgent
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmActivate w_frmActivate = new frmActivate();
            if (w_frmActivate.ShowDialog() != DialogResult.OK)
                Environment.Exit(0);

            Application.Run(new Form1());
        }
    }
}
