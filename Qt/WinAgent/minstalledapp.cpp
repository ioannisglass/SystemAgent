
#include "minstalledapp.h"

MInstalledApp::MInstalledApp()
{

}

MInstalledApp::MInstalledApp(QString _strDisplayName, QString _strInstalledLocation, QString _strDisplayVersion)
{
    this->DisplayName = _strDisplayName;
    this->DisplayVersion = _strDisplayVersion;
    this->InstalledLocation = _strInstalledLocation;
}
