using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentUpdate
{
    public static class ConstEnv
    {
        public static readonly int API_SERVER_ERROR = -1;

        public static readonly int AGENT_NO_REGISTERED = 0;
        public static readonly int AGENT_NO_ACTIVATED = 1;
        public static readonly int AGENT_REGISTERED = 2;

        public static readonly string API_BASE = "https://api.vulnagent.com";
        public static readonly string API_ACTIVATE = "/api/activate";
        public static readonly string API_SVC_FILE_URL = "https://api.vulnagent.com/uploads/WinAgentSvc.exe";
    }
}
