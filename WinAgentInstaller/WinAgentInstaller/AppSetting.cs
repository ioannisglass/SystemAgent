using Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentInstaller
{
    public class AppSettings<T> where T : new()
    {
        private const string DEFAULT_FILENAME = "api_config.ini";

        public void Save(string fileName = DEFAULT_FILENAME)
        {
            try
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception Error ({System.Reflection.MethodBase.GetCurrentMethod().Name}): {exception.Message + "\n" + exception.StackTrace}");
            }
        }

        public static void Save(T pSettings, string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(pSettings, Newtonsoft.Json.Formatting.Indented));
        }

        public static T Load(string fileName = DEFAULT_FILENAME)
        {
            try
            {
                T t = new T();
                if (File.Exists(fileName))
                    t = JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));
                else
                    return default(T);
                return t;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception Error ({System.Reflection.MethodBase.GetCurrentMethod().Name}): {exception.Message + "\n" + exception.StackTrace}");
                return default(T);
            }
        }
    }

    public class UserSetting : AppSettings<UserSetting>
    {
        public string api_base = "http://192.168.8.171:5000";
        public string api_activate = "/api/activate";
    }
}
