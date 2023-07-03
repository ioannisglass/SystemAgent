using Helpers;
using MailParser;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WinAgent
{
    internal static class Program
    {
        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        static extern uint MsiEnumProducts(uint iProductIndex, StringBuilder lpProductBuf);

        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        static extern uint MsiGetProductInfo(string szProduct, string szProperty, StringBuilder lpValueBuf, ref uint pcchValueBuf);

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
            // frmActivate w_frmActivate = new frmActivate();
            // if (w_frmActivate.ShowDialog() != DialogResult.OK)
            //     Environment.Exit(0);

            /*string query = "SELECT * FROM Win32_Product";

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    string displayName = obj["Name"] as string;
                    string installationPath = obj["InstallLocation"] as string;

                    // Display the application information
                    if (!string.IsNullOrEmpty(displayName))
                    {
                        Console.WriteLine("Display Name: " + displayName);
                        Console.WriteLine("Installation Path: " + installationPath);
                        Console.WriteLine("---------------------------");
                        if (displayName.ToLower().Contains("whatsapp"))
                            Console.WriteLine("here");
                    }
                }
            }*/

            /* const int ERROR_NO_MORE_ITEMS = 259;
            const int MAX_GUID_LENGTH = 38;

            uint index = 0;
            uint result;
            StringBuilder productCode = new StringBuilder(MAX_GUID_LENGTH);

            while ((result = MsiEnumProducts(index, productCode)) == 0)
            {
                StringBuilder productName = new StringBuilder(256);
                uint productNameLength = 256;
                StringBuilder installationPath = new StringBuilder(256);
                uint installationPathLength = 256;

                MsiGetProductInfo(productCode.ToString(), "ProductName", productName, ref productNameLength);
                MsiGetProductInfo(productCode.ToString(), "InstallLocation", installationPath, ref installationPathLength);

                Console.WriteLine("Product Code: " + productCode);
                Console.WriteLine("Product Name: " + productName);
                Console.WriteLine("Installation Path: " + installationPath);
                Console.WriteLine("---------------------------");

                index++;
                productCode.Clear();
            }

            if (result == ERROR_NO_MORE_ITEMS)
            {
                Console.WriteLine("No more installed products found.");
            }
            else
            {
                Console.WriteLine("An error occurred while retrieving installed products.");
            }*/

            // Process p = new Process();
            // p.StartInfo.UseShellExecute = false;
            // p.StartInfo.CreateNoWindow = true;
            // p.StartInfo.RedirectStandardOutput = true;
            // p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            // p.StartInfo.FileName = "powershell.exe";
            // // p.StartInfo.Arguments = @"Get-AppxPackage | select name";
            // p.StartInfo.Arguments = @"Get-AppxPackage | select *";
            // p.Start();
            // string output = p.StandardOutput.ReadToEnd();
            // p.WaitForExit();
            // Console.WriteLine(output);

            Application.Run(new Form1());
            // using (var appx = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Appx"))
            // {
            //     var packageRoot = appx.GetValue("PackageRoot");
            // }
        }
        static List<string> GetCustomApplications()
        {
            List<string> customApplications = new List<string>();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("AppSettings");

            foreach (string key in appSettings.Settings.AllKeys)
            {
                string applicationName = appSettings.Settings[key].Value;
                customApplications.Add(applicationName);
            }

            return customApplications;
        }

        static List<string> GetInstalledApplications()
        {
            List<string> installedApplications = new List<string>();

            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string programFilesx86Path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            string[] programFolders = { programFilesPath, programFilesx86Path };

            foreach (string programFolder in programFolders)
            {
                if (Directory.Exists(programFolder))
                {
                    string[] subDirectories = Directory.GetDirectories(programFolder);

                    foreach (string subDirectory in subDirectories)
                    {
                        string applicationName = Path.GetFileName(subDirectory);
                        installedApplications.Add(applicationName);
                    }
                }
            }

            return installedApplications;
        }
    }
}
