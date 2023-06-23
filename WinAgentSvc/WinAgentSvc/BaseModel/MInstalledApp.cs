using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentSvc.BaseModel
{
    [Serializable]
    public class MInstalledApp
    {
        public string displayName { get; set; }
        // public string installationLocation { get; set; }
        public string displayVersion { get; set; }
        // public string publisher { get; set; }
    }
}
