using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WinAgentSvc.BaseModel;
using Windows.Management.Deployment;

namespace WinAgentSvc.Helpers
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

        public static List<MInstalledApp> getFullThirdPartyApps()
        {
            string registry_key_32 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            string registry_key_64 = @"SOFTWARE\WoW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            IEnumerable<MInstalledApp> finalList = new List<MInstalledApp>();
            List<MInstalledApp> win32AppsCU = getThirdPartyApps(Registry.CurrentUser, registry_key_32);
            List<MInstalledApp> win64AppsCU = getThirdPartyApps(Registry.CurrentUser, registry_key_64);
            List<MInstalledApp> win32AppsLM = getThirdPartyApps(Registry.LocalMachine, registry_key_32);
            List<MInstalledApp> win64AppsLM = getThirdPartyApps(Registry.LocalMachine, registry_key_64);
            List<MInstalledApp> uwpApps = getUWPApps();

            // finalList = win32AppsCU.Concat(win64AppsCU);
            // finalList = finalList.Concat(win32AppsLM);
            // finalList = finalList.Concat(win64AppsLM);

            finalList = win32AppsCU.Concat(win32AppsLM).Concat(win64AppsCU).Concat(win64AppsLM).Concat(uwpApps);
            finalList = finalList.GroupBy(d => d.displayName).Select(d => d.First());

            return finalList.OrderBy(o => o.displayName).ToList();
        }

        public static List<MInstalledApp> getThirdPartyApps(RegistryKey regKey, string registryKey)
        {
            List<MInstalledApp> list = new List<MInstalledApp>();
            RegistryKey uninstallKey = regKey.OpenSubKey(registryKey);
            if (uninstallKey != null)
            {
                foreach (string subKeyName in uninstallKey.GetSubKeyNames())
                {
                    try
                    {
                        RegistryKey subKey = uninstallKey.OpenSubKey(subKeyName);
                        string displayName = subKey.GetValue("DisplayName") as string;
                        string displayVersion = subKey.GetValue("DisplayVersion") as string;
                        // string installLocation = (string)subkey.GetValue("InstallLocation");
                        string publisher = subKey.GetValue("Publisher") as string;
                        bool isSystemComponent = Convert.ToBoolean(subKey.GetValue("SystemComponent", 0));

                        // if (!string.IsNullOrEmpty(displayName) && !isSystemComponent && !IsMicrosoftStoreApp(publisher))
                        // if (!string.IsNullOrEmpty(displayName))
                        if (!string.IsNullOrEmpty(displayName) && !isSystemComponent && !isMSStoreAppWithName(displayName))
                        {
                            displayName = Regex.Replace(displayName, @"[^\u0000-\u007F]+", string.Empty);
                            displayVersion = Regex.Replace(displayVersion, @"[^\u0000-\u007F]+", string.Empty);

                            char[] separators = { '\0', '\a', '\b', '\t', '\n', '\v', '\f', '\r' };
                            displayName = displayName.Split(separators, StringSplitOptions.RemoveEmptyEntries)[0];
                            displayVersion = displayVersion.Split(separators, StringSplitOptions.RemoveEmptyEntries)[0];

                            list.Add(new MInstalledApp()
                            {
                                displayName = displayName.Trim(),
                                // installationLocation = "",
                                displayVersion = displayVersion ?? "Unknown"
                                // publisher = publisher ?? "Unknown"
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        SvcLogger.log(ex.Message);
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

        static bool isMSStoreAppWithName(string _strDisplayName)
        {
            return _strDisplayName?.StartsWith("Microsoft.") ?? false;
        }

        public static List<MInstalledApp> getUWPApps()
        {
            List<MInstalledApp> list = new List<MInstalledApp>();
            PackageManager packageManager = new PackageManager();
            IEnumerable<Windows.ApplicationModel.Package> packages = packageManager.FindPackages();
            int nCpt = 0;
            foreach (var package in packages)
            {
                try
                {
                    string sInstalledLocation = package.InstalledLocation.Path;
                    string sSignatureKind = package.SignatureKind.ToString();
                    if (sInstalledLocation.Contains("WindowsApps") && sSignatureKind == "Store" && package.IsFramework == false)
                    {
                        // Console.WriteLine("Package n°{0}", nCpt);
                        //Console.WriteLine("\tId Name {0}", package.Id.Name);
                        Console.WriteLine("\tDisplay Name : {0}", package.DisplayName);
                        Console.WriteLine("\tFamily : {0}", package.Id.FamilyName);
                        // Console.WriteLine("\tLogo : {0}", package.Logo.ToString());
                        string w_strDisplayName = package.DisplayName.Trim();

                        string w_strVersion = $"{package.Id.Version.Major}.{package.Id.Version.Minor}.{package.Id.Version.Build}.{package.Id.Version.Revision}";
                        string w_strPublisher = package.PublisherDisplayName;

                        list.Add(new MInstalledApp()
                        {
                            displayName = w_strDisplayName,
                            // installationLocation = "",
                            displayVersion = w_strVersion ?? "Unknown",
                            // publisher = w_strPublisher ?? "Unknown"
                        });
                        nCpt += 1;
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return list;
        }
    }
}
