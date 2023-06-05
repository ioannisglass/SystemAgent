
#include "mainwindow.h"
#include "ui_mainwindow.h"


MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    ui->lbOSinfo->setText(OSInfoHelper::getOSFullName());

    QList<MInstalledApp*> w_lstmInstalledApp = OSInfoHelper::getInstalledThirdPartySoftware();

    QStandardItemModel w_mdItems;
    w_mdItems.setColumnCount(3);
    w_mdItems.setHeaderData(0, Qt::Horizontal, "No");
    w_mdItems.setHeaderData(1, Qt::Horizontal, "Name");
    w_mdItems.setHeaderData(2, Qt::Horizontal, "Version");


    foreach (MInstalledApp* w_mInstalledApp, w_lstmInstalledApp) {
        // qDebug() << "Object name:" << obj->getName();
        QList<QStandardItem*> w_rowData;
        w_rowData.append(new QStandardItem(w_mdItems.rowCount() + 1));
        w_rowData.append(new QStandardItem(w_mInstalledApp->DisplayName));
        w_rowData.append(new QStandardItem(w_mInstalledApp->DisplayVersion));
        w_mdItems.appendRow(w_rowData);

    }
    ui->tableView->setModel(&w_mdItems);
    ui->tableView->show();
}

MainWindow::~MainWindow()
{
    delete ui;
}


