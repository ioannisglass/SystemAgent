using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentInstaller
{
    public static class ConstEnv
    {
        public static readonly int API_SERVER_ERROR = -1;

        public static readonly int AGENT_NO_REGISTERED = 0;
        public static readonly int AGENT_NO_ACTIVATED = 1;
        public static readonly int AGENT_REGISTERED = 2;
    }
}
