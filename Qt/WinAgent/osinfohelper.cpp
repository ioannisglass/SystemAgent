
#include "osinfohelper.h"

// OSInfoHelper::OSInfoHelper()
// {
// }

QString OSInfoHelper::getOSDescription()
{
    QString w_strOSinfo = "";
    return w_strOSinfo;
}

QString OSInfoHelper::getOSVersion()
{
    QString w_strOSVersion = "";
    return w_strOSVersion;
}

QString OSInfoHelper::getOSFullName()
{
    return QSysInfo::prettyProductName();
}

QString OSInfoHelper::getMachineName()
{
    return "";
}

QList<MInstalledApp*> getInstalledThirdPartySoftware()
{
    QSettings settings("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall", QSettings::NativeFormat);
    QStringList softwareKeys = settings.childGroups();
    QList<MInstalledApp*> w_lstmInstalledApp;

    foreach (const QString &softwareKey, softwareKeys) {
        settings.beginGroup(softwareKey);
        QString displayName = settings.value("DisplayName").toString();
        QString displayVersion = settings.value("DisplayVersion").toString();
        QString publisher = settings.value("Publisher").toString();
        settings.endGroup();

        // Filter out Microsoft and Windows components
        if (!displayName.isEmpty() && !displayName.contains("Microsoft") && !publisher.contains("Microsoft") && !displayName.contains("Windows")) {
            // qDebug() << "Name:" << displayName;
            // qDebug() << "Version:" << displayVersion;
            // qDebug() << "Publisher:" << publisher;
            // qDebug() << "---------------------------------";
            w_lstmInstalledApp << new MInstalledApp(displayName, "", displayVersion);
        }
    }
    return w_lstmInstalledApp;
}
