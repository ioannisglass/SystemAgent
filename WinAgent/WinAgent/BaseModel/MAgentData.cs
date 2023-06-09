using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgent.BaseModel
{
    [Serializable]
    public class MAgentData
    {
        public string osInfo { get; set; }
        public string machineName { get; set; }
        public MAuth auth { get; set; }
        public List<MInstalledApp> installedApps { get; set; }

        public MAgentData()
        {
            osInfo = string.Empty;
            machineName = string.Empty;
            auth = new MAuth();
            installedApps = new List<MInstalledApp>();
        }
    }
}
