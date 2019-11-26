#include <QtCore/QCoreApplication>
#include "ConnectorTester.h"

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);
    ConnectorTester tester;
    tester.getAndSetBool();
    tester.getAndSetLoc();
    tester.getCurrentLoc();
    return a.exec();
}