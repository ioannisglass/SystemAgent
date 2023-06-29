using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.IO;

namespace WinAgentUninstaller
{
    public static class ServiceExts
    {
        public static void StartService(string[] args, string serviceName = "WinAgentSvc", int timeoutMilliseconds = 1000)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Start(args);
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                // ...
            }
        }

        public static void StopService(string serviceName = "WinAgentSvc", int timeoutMilliseconds = 10000)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch
            {
                // ...
            }
        }

        public static void RestartService(string serviceName = "WinAgentSvc", int timeoutMilliseconds = 3000)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                // ...
            }
        }

        public static void InstallService(string _strService)
        {
            /*string cmd = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InstallUtil.exe");
            cmd += " " + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ScanService.exe");*/

            string cmd = $"InstallUtil.exe {_strService}";
            //cmd = "\"C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe\" \"F:\\Projects\\C_sharp_process\\ScanApp\\ScanApp\\bin\\Debug\\ScanService.exe\"";
            string err_msg = string.Empty;
            ProcessExts.executeCommand(cmd, out err_msg);
        }

        public static void UninstallService(string _strService)
        {
            /*string cmd = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InstallUtil.exe");
            cmd += " -u " + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ScanService.exe");*/

            // string cmd = "InstallUtil.exe -u ScanService.exe";
            string cmd = $"InstallUtil.exe -u {_strService}";
            string err_msg = string.Empty;
            ProcessExts.executeCommand(cmd, out err_msg);
        }

        public static bool IsServiceInstalled(string service_name = "WinAgentSvc")
        {
            return ServiceController.GetServices().Any(s => s.ServiceName == service_name);
        }

        public static string GetWindowsServiceStatus(string SERVICENAME = "WinAgentSvc")
        {
            ServiceController sc = new ServiceController(SERVICENAME);

            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    return "Running";
                case ServiceControllerStatus.Stopped:
                    return "Stopped";
                case ServiceControllerStatus.Paused:
                    return "Paused";
                case ServiceControllerStatus.StopPending:
                    return "Stopping";
                case ServiceControllerStatus.StartPending:
                    return "Starting";
                default:
                    return "Status Changing";
            }
        }
    }
}
