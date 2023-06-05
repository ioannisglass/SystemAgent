
#ifndef MINSTALLEDAPP_H
#define MINSTALLEDAPP_H
#include <QString>

class MInstalledApp
{
public:
    MInstalledApp();
    MInstalledApp(QString _strDisplayName, QString _strInstalledLocation, QString _strDisplayVersion);
    QString DisplayName;
    QString InstalledLocation;
    QString DisplayVersion;
};

#endif // MINSTALLEDAPP_H
