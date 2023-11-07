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
        public string name { get; set; }
        public string ver { get; set; }
    }
}
