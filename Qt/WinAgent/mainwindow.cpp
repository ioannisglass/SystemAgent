
#include "mainwindow.h"
#include "ui_mainwindow.h"


MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    ui->lbOSinfo->setText(OSInfoHelper::getOSFullName());

    QList<MInstalledApp*> w_lstmInstalledApp = OSInfoHelper::getInstalledThirdPartySoftware();

    QStandardItemModel *w_mdItems = new QStandardItemModel(this);
    ui->tableView->setModel(w_mdItems);

    w_mdItems->setColumnCount(2);
    w_mdItems->setHeaderData(0, Qt::Horizontal, "Name");
    w_mdItems->setHeaderData(1, Qt::Horizontal, "Version");

    foreach (MInstalledApp* w_mInstalledApp, w_lstmInstalledApp) {
        // qDebug() << "Object name:" << obj->getName();
        QList<QStandardItem*> w_rowData;
        // w_rowData.append(new QStandardItem(w_mdItems->rowCount() + 1));
        w_rowData.append(new QStandardItem(w_mInstalledApp->DisplayName));
        w_rowData.append(new QStandardItem(w_mInstalledApp->DisplayVersion));
        w_mdItems->appendRow(w_rowData);

    }
    ui->tableView->show();
}

MainWindow::~MainWindow()
{
    delete ui;
}


