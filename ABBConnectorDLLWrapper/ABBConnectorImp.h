#pragma once
#include "stdafx.h"
#include "ABBConnector.h"
#include <string>
#include <iostream>
#include <msclr/marshal_cppstd.h>

#using "..\ABBConnectSDK\bin\Release\ABBConnectSDK.dll"
#pragma managed

class ABBConnectorImp : public ABBConnector
{
public:
    ABBConnectorImp();
    virtual ~ABBConnectorImp();

public:
    virtual int getValue(std::string task_name, std::string module_name, std::string var_name, double& value) override;
    virtual int getValue(std::string task_name, std::string module_name, std::string var_name, double value[6]) override;
    virtual int setValue(std::string task_name, std::string module_name, std::string var_name, double value) override;
    virtual int setValue(std::string task_name, std::string module_name, std::string var_name, double value[6]) override;
    virtual int getCurrentLoc(double value[]) override;

private:
    int connect();
    int disconnect();
    System::String^ getSystemString(std::string str);
    std::string getStdString(System::String^ str);

private:
    bool is_connected_ = false;
    gcroot<ABBConnectSDK::RobotManager^> robot_manager_;
};

