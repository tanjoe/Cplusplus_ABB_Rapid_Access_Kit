#include "ConnectorTester.h"

ConnectorTester::ConnectorTester()
{
    connector = factory.createInstance();
}

ConnectorTester::~ConnectorTester()
{
}

void ConnectorTester::getAndSetBool()
{
    int ret;
    double set_value = 0.0;
    double get_value = 0.0;

    for (int i = 0; i < 10; ++i)
    {
        set_value = i % 2;
        ret = connector->setValue("T_ROB1", "MotionModule", "cameraTriggerFlag", set_value);
        areEqual(ret, 0);
        ret = connector->getValue("T_ROB1", "MotionModule", "cameraTriggerFlag", get_value);
        areEqual(ret, 0);
        areEqual(set_value, get_value);
    }
    qDebug() << "Test passed";
}

void ConnectorTester::getAndSetLoc()
{
    int ret;
    double set_loc[6];
    double get_loc[6];

    for (int i = 0; i < 100; ++i)
    {
        for (int j = 0; j < 6; ++j)
        {
            set_loc[j] = j + i;
        }
        ret = connector->setValue("T_ROB1", "MotionModule", "targetLoc", set_loc);
        areEqual(ret, 0);
        ret = connector->getValue("T_ROB1", "MotionModule", "targetLoc", get_loc);
        areEqual(ret, 0);
        for (int j = 0; j < 3; ++j)
        {
            areEqual(set_loc[j], get_loc[j]);
        }
    }
    qDebug() << "Test passed";
}

void ConnectorTester::getCurrentLoc()
{
    int ret;
    double loc[6];

    for (int i = 0; i < 100; ++i)
    {
        ret = connector->getCurrentLoc(loc);
        areEqual(ret, 0);
    }
    //qDebug() << loc[0] << loc[1] << loc[2] << loc[3] << loc[4] << loc[5];
    qDebug() << "Test passed";
}

void ConnectorTester::areEqual(int a, int b)
{
    if (a != b)
    {
        qDebug() << a << b;
        throw std::exception();
    }
}
