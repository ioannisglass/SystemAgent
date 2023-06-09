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

            // List<MInstalledApp> w_lstmInstalledApp = OSInfoHelper.GetFullListInstalledApplication();
            List<MInstalledApp> w_lstmInstalledApp = OSInfoHelper.getFullThirdPartyApps();
            foreach (MInstalledApp app in w_lstmInstalledApp)
            {
                ListViewItem w_lvmInstalledApp = new ListViewItem((lstvApps.Items.Count + 1).ToString());
                w_lvmInstalledApp.SubItems.Add(app.displayName);
                // w_lvmInstalledApp.SubItems.Add(app.InstallationLocation);
                w_lvmInstalledApp.SubItems.Add(app.displayVersion);
                lstvApps.Items.Add(w_lvmInstalledApp);
            }
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            MAgentData w_mAgentData = new MAgentData();
            w_mAgentData.osInfo = OSInfoHelper.getOSFullName() + $" {OSInfoHelper.getOSbit()} ({OSInfoHelper.getOSVersion()}) {OSInfoHelper.getOSDescription()}";
            w_mAgentData.machineName = OSInfoHelper.getMachineName();
            w_mAgentData.auth.cusid = Program.g_setting.customer_id;
            w_mAgentData.auth.actkey = Program.g_setting.activation_key;

            w_mAgentData.installedApps.AddRange(OSInfoHelper.getFullThirdPartyApps());

            string w_strRet = AgentHelper.postAgentData(w_mAgentData);
            MessageBox.Show(w_strRet);
        }
    }
}
