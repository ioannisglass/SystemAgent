
#ifndef OSINFOHELPER_H
#define OSINFOHELPER_H
#include <QString>
#include <QSysInfo>
#include <QSettings>
#include <QList>
#include "minstalledapp.h"

class OSInfoHelper
{
public:
    // OSInfoHelper();
    static QString getOSDescription();
    static QString getOSVersion();
    static QString getOSFullName();
    static QString getMachineName();
    static QList<MInstalledApp*> getInstalledThirdPartySoftware();
};

#endif // OSINFOHELPER_H
