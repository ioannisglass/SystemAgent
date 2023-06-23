using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentSvc.BaseModule
{
    public class ProcessInfo
    {
        public string ProcessName;
        public string ProcessPath;

        public ProcessInfo()
        {
            ProcessName = string.Empty;
            ProcessPath = string.Empty;
        }
    }
}
