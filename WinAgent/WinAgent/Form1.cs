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

            /* List<string> programs = new List<string>();
            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
            foreach (ManagementObject mo in mos.Get())
            {
                try
                {
                    //more properties:
                    //http://msdn.microsoft.com/en-us/library/windows/desktop/aa394378(v=vs.85).aspx
                    programs.Add(mo["Name"].ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            Console.WriteLine("Done");*/
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
            lbCusId.Text = $"Customer ID: {Program.g_setting.customer_id}";
            lbActKey.Text = $"Activation Key: {Program.g_setting.activation_key}";

            // List<MInstalledApp> w_lstmInstalledApp = OSInfoHelper.GetFullListInstalledApplication();
            // List<MInstalledApp> w_lstmInstalledApp = OSInfoHelper.getFullThirdPartyApps();
            List<MInstalledApp> w_lstmInstalledApp = OSInfoHelper.getExactNameAppList();
            foreach (MInstalledApp app in w_lstmInstalledApp)
            {
                ListViewItem w_lvmInstalledApp = new ListViewItem((lstvApps.Items.Count + 1).ToString());
                w_lvmInstalledApp.SubItems.Add(app.name);
                // w_lvmInstalledApp.SubItems.Add(app.InstallationLocation);
                w_lvmInstalledApp.SubItems.Add(app.ver);
                w_lvmInstalledApp.Tag = app.uns;
                lstvApps.Items.Add(w_lvmInstalledApp);
            }
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            MAgentData w_mAgentData = new MAgentData();
            w_mAgentData.osInfo = OSInfoHelper.getOSFullName() + $" {OSInfoHelper.getOSbit()} ({OSInfoHelper.getOSDescription()})";
            w_mAgentData.version = OSInfoHelper.getOSVersion();
            w_mAgentData.machineName = OSInfoHelper.getMachineName();
            w_mAgentData.auth.customerid = Program.g_setting.customer_id;
            w_mAgentData.auth.actkey = Program.g_setting.activation_key;

            w_mAgentData.installedApps.AddRange(OSInfoHelper.getExactNameAppList());

            string w_strRet = AgentHelper.postAgentData(w_mAgentData);
            MessageBox.Show(w_strRet);
        }

        private void lstvApps_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lstvApps.FocusedItem != null && lstvApps.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    ContextMenu m = new ContextMenu();

                    MenuItem uninstallMenuItem = new MenuItem("Uninstall");
                    uninstallMenuItem.Click += delegate (object sender2, EventArgs e2)
                    {
                        UninstallAction(sender, e);
                    };// your action here
                    m.MenuItems.Add(uninstallMenuItem);
                    MenuItem removeMenuItem = new MenuItem("Remove");
                    removeMenuItem.Click += delegate (object sender2, EventArgs e2)
                    {
                        RemoveAction(sender, e);
                    };// your action here 
                    m.MenuItems.Add(removeMenuItem);

                    m.Show(lstvApps, new Point(e.X, e.Y));
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (lstvApps.FocusedItem != null && lstvApps.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    SelectAction(sender, e);
                }
            }
        }

        private void SelectAction(object sender, MouseEventArgs e)
        {
            ListView ListViewControl = sender as ListView;

            if (ListViewControl.SelectedItems.Count > 1)
                return;

            string w_strName = ListViewControl.SelectedItems[0].SubItems[1].Text;
            string w_strVer = ListViewControl.SelectedItems[0].SubItems[2].Text;
            string w_strKey = $"{w_strName}_{w_strVer}";

            if (Program.g_dictAppData == null || Program.g_dictAppData.Keys.Count == 0)
            {
                lbLoc.Text = string.Empty;
                return;
            }
            if (!Program.g_dictAppData.ContainsKey(w_strKey))
            {
                lbLoc.Text = string.Empty;
                return;
            }

            if (!string.IsNullOrEmpty(Program.g_dictAppData[w_strKey].quns))
            {
                lbLoc.Text = Program.g_dictAppData[w_strKey].quns;
                return;
            }
            if (!string.IsNullOrEmpty(Program.g_dictAppData[w_strKey].loc))
            {
                lbLoc.Text = Program.g_dictAppData[w_strKey].loc;
                return;
            }

            if (!string.IsNullOrEmpty(Program.g_dictAppData[w_strKey].uns))
            {
                string w_strUnsPath = Program.g_dictAppData[w_strKey].uns;
                if (w_strUnsPath.Contains(" "))
                    w_strUnsPath = w_strUnsPath.Split(' ')[0];
                lbLoc.Text = w_strUnsPath;
            }
        }

        private void UninstallAction(object sender, MouseEventArgs e)
        {
            //id is extra value when you need or delete it
            ListView ListViewControl = sender as ListView;

            if (ListViewControl.SelectedItems.Count > 1)
                return;
            // string w_strUninstallString = ListViewControl.SelectedItems[0].Tag as string;
            string w_strName = ListViewControl.SelectedItems[0].SubItems[1].Text;
            string w_strVer = ListViewControl.SelectedItems[0].SubItems[2].Text;
            string w_strKey = $"{w_strName}_{w_strVer}";

            if (Program.g_dictAppData == null || Program.g_dictAppData.Keys.Count == 0)
            {
                MessageBox.Show("Empty app data dictionary.");
                return;
            }
            if (!Program.g_dictAppData.ContainsKey(w_strKey))
            {
                MessageBox.Show("This app data is empty.");
                return;
            }
            SysAppHandler.UninstallApp(Program.g_dictAppData[w_strKey]);
        }

        private void RemoveAction(object sender, EventArgs e)
        {
            ListView ListViewControl = sender as ListView;

            if (ListViewControl.SelectedItems.Count > 1)
                return;

            string w_strName = ListViewControl.SelectedItems[0].SubItems[1].Text;
            string w_strVer = ListViewControl.SelectedItems[0].SubItems[2].Text;
            string w_strKey = $"{w_strName}_{w_strVer}";

            if (Program.g_dictAppData == null || Program.g_dictAppData.Keys.Count == 0)
            {
                MessageBox.Show("Empty app data dictionary.");
                return;
            }
            if (!Program.g_dictAppData.ContainsKey(w_strKey))
            {
                MessageBox.Show("This app data is empty.");
                return;
            }
            string w_strRet = SysAppHandler.RemoveAppDir(Program.g_dictAppData[w_strKey]);
            MessageBox.Show(w_strRet);
        }

        private void btnToUninstall_Click(object sender, EventArgs e)
        {
            string w_strHost = OSInfoHelper.getMachineName();
            List<string> w_lstrAppsToRemove = AgentHelper.GetAppsToRemove(w_strHost);
            if (w_lstrAppsToRemove == null || w_lstrAppsToRemove.Count == 0)
                MessageBox.Show("No apps to uninstall.");
            else
                MessageBox.Show(string.Join(",", w_lstrAppsToRemove));
        }
    }
}
