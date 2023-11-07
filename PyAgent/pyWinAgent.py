import winreg
from itertools import groupby

def isMicrosoftStoreApp(_strPublisher):
    # return True if _strPublisher.find("Microsoft Corporation") > -1 else False
    return True if _strPublisher.find("Microsoft") > -1 else False

def getThirdPartyApps(regKey, strRegistryKey):
    key = winreg.OpenKey(regKey, strRegistryKey, 0, winreg.KEY_READ | winreg.KEY_WOW64_32KEY)
    installed_applications = []

    # Enumerate through the subkeys of the uninstall key
    for i in range(winreg.QueryInfoKey(key)[0]):
        subkey_name = winreg.EnumKey(key, i)
        subkey = winreg.OpenKey(key, subkey_name)

        try:
            # Get the display name and publisher values
            display_name = winreg.QueryValueEx(subkey, "DisplayName")[0]
            publisher = winreg.QueryValueEx(subkey, "Publisher")[0]

            if display_name != None and display_name != "" and isMicrosoftStoreApp(publisher) == False:
                # Append the application details to the list
                installed_applications.append((display_name, publisher))
            
        except OSError:
            # Some subkeys may not have the required values, ignore them
            pass

        subkey.Close()

    key.Close()

    return installed_applications

def getWindowsFullThirdPartyApps():
    registry_key_32 = r"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
    registry_key_64 = r"SOFTWARE\WoW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
    all_apps = []
    win32AppsCU = getThirdPartyApps(winreg.HKEY_CURRENT_USER, registry_key_32)
    # win64AppsCU = getThirdPartyApps(winreg.HKEY_CURRENT_USER, registry_key_64)
    win32AppsLM = getThirdPartyApps(winreg.HKEY_LOCAL_MACHINE, registry_key_32)
    win64AppsLM = getThirdPartyApps(winreg.HKEY_LOCAL_MACHINE, registry_key_64)
    all_apps = all_apps + win32AppsCU + win32AppsLM + win64AppsLM
    
    # Sort the list by the 'a' attribute
    all_apps.sort(key=lambda x: x[0])

    # Group the objects by the 'a' attribute
    # grouped_objects = groupby(all_apps, key=lambda x: x[0])
    seen = set()
    filtered_objects = [obj for obj in all_apps if obj[0] not in seen and not seen.add(obj[0])]
    # unique_data = list(dict(all_apps).items())
    return filtered_objects

if __name__ == '__main__':
    applications = getWindowsFullThirdPartyApps()
    for app in applications:
        print(f"Display Name: {app[0]}")
        print(f"Publisher: {app[1]}")
        print("------")