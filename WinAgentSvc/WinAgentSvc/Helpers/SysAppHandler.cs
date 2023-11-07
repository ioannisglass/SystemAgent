using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinAgentSvc.BaseModel;
using WinAgentSvc.BaseModule;

namespace WinAgentSvc.Helpers
{
    public static class SysAppHandler
    {
        public static string RemoveAppDir(MAppData _mAppDataToRemove)
        {
            string w_strAppLocation = string.Empty;
            try
            {
                if (_mAppDataToRemove == null)
                    return "App data is null.";

                w_strAppLocation = _mAppDataToRemove.loc;
                w_strAppLocation = w_strAppLocation.Trim('"');
                if (string.IsNullOrEmpty(w_strAppLocation))
                    w_strAppLocation = Path.GetDirectoryName(_mAppDataToRemove.uns.Trim('"'));

                if (string.IsNullOrEmpty(w_strAppLocation))
                    return "Empty location.";
                if (!Directory.Exists(w_strAppLocation))
                    return $"{w_strAppLocation} is not existed.";
                Process[] processList = ProcessExts.GetRunningProcessList();
                foreach (Process theprocess in processList)
                {
                    string w_strProcPath = ProcessExts.GetProcessPath(theprocess);
                    if (string.IsNullOrEmpty(w_strProcPath))
                        continue;
                    string w_strProcDir = Path.GetDirectoryName(w_strProcPath);
                    if (w_strProcDir.ToLower().IndexOf(w_strAppLocation.ToLower(), StringComparison.InvariantCultureIgnoreCase) != -1 ||
                        w_strAppLocation.ToLower().IndexOf(w_strProcDir.ToLower(), StringComparison.InvariantCultureIgnoreCase) != -1)
                        theprocess.Kill();
                }
                int w_nRetryNum = 0;
                while(w_nRetryNum < 5)
                {
                    try
                    {
                        if (!Directory.Exists(w_strAppLocation))
                            break;
                        Directory.Delete(w_strAppLocation, true);
                        break;
                    }
                    catch(Exception e)
                    {
                        SvcLogger.log($"App location {w_strAppLocation} removed failed - {e.Message}");
                        Thread.Sleep(2000);
                        w_nRetryNum++;
                    }
                }
                return $"{w_strAppLocation} removed.";
            }
            catch (Exception e)
            {
                return $"Failed to remove application directory({w_strAppLocation}) - {e.Message}";
            }
        }

        public static bool UninstallApp(MAppData _mAppDataToUninstall)
        {
            try
            {
                if (_mAppDataToUninstall == null)
                    return false;
                string w_strError = string.Empty;
                if (!string.IsNullOrEmpty(_mAppDataToUninstall.quns))
                    ProcessExts.ExecuteCommand(_mAppDataToUninstall.quns, out w_strError);
                string w_strRet = RemoveAppDir(_mAppDataToUninstall);
                if (!string.IsNullOrEmpty(_mAppDataToUninstall.reg))
                    OSInfoHelper.DeleteRegKey(_mAppDataToUninstall.reg);
                SvcLogger.log(w_strRet);
                return true;
            }
            catch (Exception ex)
            {
                SvcLogger.log($"UninstallApp failed - {ex.Message}");
                return false;
            }
        }
    }
}
