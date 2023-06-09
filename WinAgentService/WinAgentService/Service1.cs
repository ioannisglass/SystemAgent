using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using WinAgentService;
using WinAgentService.BaseModule;
using Newtonsoft.Json;
using System.IO;

namespace WinAgentService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        ProcessExts clsProcessExts = new ProcessExts();
        List<string> lstOldProcPath = new List<string>();

        string strScanHome = string.Empty;
        private string varScanApp = "SCAN_HOME";

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            strScanHome = System.Environment
                .GetEnvironmentVariable(varScanApp, EnvironmentVariableTarget.Machine);

            File.AppendAllText(Program.g_log_path, strScanHome + "\n");    /////

            GetConfigFile();

            foreach (string one in Program.g_lstBaseProcList)
            {
                if (clsProcessExts.CheckRunning(one))
                    lstOldProcPath.Add(one);
            }

            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in milisecinds  
            timer.Enabled = true;
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            //WriteToFile("Service is recall at " + DateTime.Now);
            List<string> ListTmp = new List<string>();
            clsProcessExts.CheckProcessStatus(lstOldProcPath, out ListTmp);
            File.AppendAllText(Program.g_log_path, $"count of ListTmp = {ListTmp.Count.ToString()}\n");    /////
            lstOldProcPath.Clear();
            lstOldProcPath.AddRange(ListTmp);
            File.AppendAllText(Program.g_log_path, $"count of oldproc = {lstOldProcPath.Count.ToString()}\n");    /////
        }

        protected override void OnStop()
        {
        }

        public void GetConfigFile()
        {
            Program.g_lstBaseProcList = new List<string>();

            string strConfigPath = Path.Combine(strScanHome, "config.ini");

            List<ProcessInfo> lstConfig = JsonConvert.DeserializeObject<List<ProcessInfo>>(File.ReadAllText(strConfigPath));

            File.AppendAllText(Program.g_log_path, File.ReadAllText(strConfigPath) + "\n");    /////

            foreach (ProcessInfo one in lstConfig)
                Program.g_lstBaseProcList.Add(one.ProcessPath);
        }
    }
}
