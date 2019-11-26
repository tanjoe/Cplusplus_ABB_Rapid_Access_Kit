#pragma once
#include <QDebug>
#include <exception>

#include "../ABBConnectorDLLWrapper/ABBConnectorFactory.h"
#include "../ABBConnectorDLLWrapper/ABBConnector.h"

class ConnectorTester
{
public:
    ConnectorTester();
    ~ConnectorTester();

public:
    void getAndSetBool();
    void getAndSetLoc();
    void getCurrentLoc();

private:
    void areEqual(int a, int b);

private:
    ABBConnectorFactory factory;
    ABBConnector* connector;
};

