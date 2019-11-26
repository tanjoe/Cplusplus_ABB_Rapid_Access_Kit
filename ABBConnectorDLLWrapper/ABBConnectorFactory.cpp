#include "stdafx.h"
#include "ABBConnectorFactory.h"
#include "ABBConnectorImp.h"

ABBConnectorFactory::ABBConnectorFactory()
{
}

ABBConnector* ABBConnectorFactory::createInstance()
{
    static ABBConnectorImp instance;
    return &instance;
}
