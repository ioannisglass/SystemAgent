using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using WinAgent.BaseModel;
using WinAgent.Helpers;

namespace WinAgent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lstvApps.Columns.Add("No");
            lstvApps.Columns.Add("Name");
            // lstvApps.Columns.Add("Path");
            lstvApps.Columns.Add("Version");
            lstvApps.Columns[0].Width = 30;
            lstvApps.Columns[1].Width = 300;
            lstvApps.Columns[2].Width = 200;
            // lstvApps.Columns[3].Width = 70;

            lbOSinfo.Text = "Operating System: " + OSInfoHelper.getOSFullName() + $" {OSInfoHelper.getOSbit()} ({OSInfoHelper.getOSVersion()})";
            lbOSinfo.Text += $" {OSInfoHelper.getOSDescription()}";
            lbComputerName.Text = "Computer Name: " + OSInfoHelper.getMachineName();

            /*string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        ListViewItem w_lvmInstalledApp = new ListViewItem((lstvApps.Items.Count + 1).ToString());
                        object w_objDispName = subkey.GetValue("DisplayName");
                        if (w_objDispName != null)
                        {
                            w_lvmInstalledApp.SubItems.Add(subkey.GetValue("DisplayName").ToString());
                            lstvApps.Items.Add(w_lvmInstalledApp);
                        }
                    }
                }
            }

            string query = "SELECT Name, Version FROM Win32_Product";

            // Create a new ManagementObjectSearcher with the query
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                // Execute the query and get the result
                ManagementObjectCollection results = searcher.Get();

                // Iterate over the result and print the installed apps
                foreach (ManagementObject app in results)
                {
                    ListViewItem w_lvmInstalledApp = new ListViewItem((lstvApps.Items.Count + 1).ToString());
                    w_lvmInstalledApp.SubItems.Add(app["Name"]?.ToString());
                    w_lvmInstalledApp.SubItems.Add(app["Version"]?.ToString());
                    lstvApps.Items.Add(w_lvmInstalledApp);

                }
            }*/

            List<MInstalledApp> w_lstmInstalledApp = OSInfoHelper.GetFullListInstalledApplication();
            foreach(MInstalledApp app in w_lstmInstalledApp)
            {
                ListViewItem w_lvmInstalledApp = new ListViewItem((lstvApps.Items.Count + 1).ToString());
                w_lvmInstalledApp.SubItems.Add(app.DisplayName);
                // w_lvmInstalledApp.SubItems.Add(app.InstallationLocation);
                w_lvmInstalledApp.SubItems.Add(app.DisplayVersion);
                lstvApps.Items.Add(w_lvmInstalledApp);
            }
        }
    }
}
