using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinAgent.BaseModel;

namespace WinAgent.Helpers
{
    public static class OSInfoHelper
    {
        public static string getOSDescription()
        {
            string w_strOSinfo = string.Empty;
            OperatingSystem os = Environment.OSVersion;
            // w_strOSinfo += " " + os.Platform.ToString();
            // w_strOSinfo += " " + os.ServicePack.ToString();
            // w_strOSinfo += " " + os.VersionString.ToString();

            w_strOSinfo += $"{RuntimeInformation.OSDescription}";

            return w_strOSinfo;
        }
        public static string getOSPlatform()            // WinNT
        {
            return new ComputerInfo().OSPlatform;
        }
        public static string getOSVersion()             // 6.1.7601
        {
            return new ComputerInfo().OSVersion;
        }
        public static string getOSFullName()            // Mircrosoft Windows 7 Ultimate
        {
            return new ComputerInfo().OSFullName;
        }

        public static string getMachineName()
        {
            return Environment.MachineName;
        }
        public static string getOSbit()
        {
            if (Environment.Is64BitOperatingSystem)
                return "64bit";
            else
                return "32bit";
        }

        /*
         DisplayName ==> ProductName property
         DisplayVersion ==> Derived from ProductVersion property
         Publisher ==> Manufacturer property
         VersionMinor ==> Derived from ProductVersion property
         VersionMajor ==> Derived from ProductVersion property
         Version ==> Derived from ProductVersion property
         HelpLink ==> ARPHELPLINK property
         HelpTelephone ==> ARPHELPTELEPHONE property
         InstallDate ==> The last time this product received service. 
         The value of this property is replaced each time a patch is applied or removed from 
         the product or the /v Command-Line Option is used to repair the product. 
         If the product has received no repairs or patches this property contains 
         the time this product was installed on this computer.
         InstallLocation ==> ARPINSTALLLOCATION property
         InstallSource ==> SourceDir property
         URLInfoAbout ==> ARPURLINFOABOUT property
         URLUpdateInfo ==> ARPURLUPDATEINFO property
         AuthorizedCDFPrefix ==> ARPAUTHORIZEDCDFPREFIX property
         Comments ==> Comments provided to the Add or Remove Programs control panel.
         Contact ==> Contact provided to the Add or Remove Programs control panel.
         EstimatedSize ==> Determined and set by the Windows Installer.
         Language ==> ProductLanguage property
         ModifyPath ==> Determined and set by the Windows Installer.
         Readme ==> Readme provided to the Add or Remove Programs control panel.
         UninstallString ==> Determined and set by Windows Installer.
         SettingsIdentifier ==> MSIARPSETTINGSIDENTIFIER property
         */
        private static List<MInstalledApp> GetInstalledApplication(RegistryKey regKey, string registryKey)
        {
            List<MInstalledApp> list = new List<MInstalledApp>();
            using (Microsoft.Win32.RegistryKey key = regKey.OpenSubKey(registryKey))
            {
                if (key != null)
                {
                    foreach (string name in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(name))
                        {
                            string displayName = (string)subkey.GetValue("DisplayName");
                            string installLocation = (string)subkey.GetValue("InstallLocation");
                            string version = (string)subkey.GetValue("DisplayVersion");

                            if (!string.IsNullOrEmpty(displayName)) // && !string.IsNullOrEmpty(installLocation)
                            {
                                list.Add(new MInstalledApp()
                                {
                                    displayName = displayName.Trim(),
                                    installationLocation = installLocation,
                                    displayVersion = version
                                });
                            }
                        }
                    }
                }
            }

            return list;
        }

        public static List<MInstalledApp> GetFullListInstalledApplication()
        {
            IEnumerable<MInstalledApp> finalList = new List<MInstalledApp>();

            string registry_key_32 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            string registry_key_64 = @"SOFTWARE\WoW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            List<MInstalledApp> win32AppsCU = GetInstalledApplication(Registry.CurrentUser, registry_key_32);
            List<MInstalledApp> win32AppsLM = GetInstalledApplication(Registry.LocalMachine, registry_key_32);
            List<MInstalledApp> win64AppsCU = GetInstalledApplication(Registry.CurrentUser, registry_key_64);
            List<MInstalledApp> win64AppsLM = GetInstalledApplication(Registry.LocalMachine, registry_key_64);

            finalList = win32AppsCU.Concat(win32AppsLM).Concat(win64AppsCU).Concat(win64AppsLM);

            finalList = finalList.GroupBy(d => d.displayName).Select(d => d.First());

            return finalList.OrderBy(o => o.displayName).ToList();
        }

        public static List<MInstalledApp> getFullThirdPartyApps()
        {
            string registry_key_32 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            string registry_key_64 = @"SOFTWARE\WoW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            IEnumerable<MInstalledApp> finalList = new List<MInstalledApp>();
            List<MInstalledApp> win32AppsCU = getThirdPartyApps(Registry.CurrentUser, registry_key_32);
            List<MInstalledApp> win64AppsCU = getThirdPartyApps(Registry.CurrentUser, registry_key_64);
            List<MInstalledApp> win32AppsLM = getThirdPartyApps(Registry.LocalMachine, registry_key_32);
            List<MInstalledApp> win64AppsLM = getThirdPartyApps(Registry.LocalMachine, registry_key_64);

            finalList = win32AppsCU.Concat(win64AppsCU);
            finalList = finalList.Concat(win32AppsLM);
            finalList = finalList.Concat(win64AppsLM);

            finalList = finalList.GroupBy(d => d.displayName).Select(d => d.First());

            return finalList.OrderBy(o => o.displayName).ToList();
        }

        public static List<MInstalledApp> getThirdPartyApps(RegistryKey regKey, string registryKey)
        {
            RegistryKey uninstallKey = regKey.OpenSubKey(registryKey);
            List<MInstalledApp> list = new List<MInstalledApp>();
            if (uninstallKey != null)
            {
                foreach (string subKeyName in uninstallKey.GetSubKeyNames())
                {
                    try
                    {
                        RegistryKey subKey = uninstallKey.OpenSubKey(subKeyName);
                        string displayName = subKey.GetValue("DisplayName") as string;
                        string displayVersion = subKey.GetValue("DisplayVersion") as string;
                        string publisher = subKey.GetValue("Publisher") as string;
                        bool isSystemComponent = Convert.ToBoolean(subKey.GetValue("SystemComponent", 0));

                        if (!string.IsNullOrEmpty(displayName) && !isSystemComponent && !IsMicrosoftStoreApp(publisher))
                        {
                            list.Add(new MInstalledApp()
                            {
                                displayName = displayName.Trim(),
                                installationLocation = "",
                                displayVersion = displayVersion ?? "Unknown",
                                publisher = publisher ?? "Unknown"
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return list;
        }

        static bool IsMicrosoftStoreApp(string publisher)
        {
            // Add any specific criteria to identify Microsoft Store apps
            return publisher?.Contains("Microsoft Corporation") ?? false;
        }
    }
}
