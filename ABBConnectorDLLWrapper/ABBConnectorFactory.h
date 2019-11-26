#pragma once
#include "ABBConnector.h"

#ifdef ABBCONNECTORDLLWRAPPER_EXPORTS
#define ABB_API __declspec(dllexport)
#else
#define ABB_API __declspec(dllimport)
#endif

class ABB_API ABBConnectorFactory
{
public:
    ABBConnectorFactory();

public:
    ABBConnector* createInstance();
};

