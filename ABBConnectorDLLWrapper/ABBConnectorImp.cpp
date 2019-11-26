#include "stdafx.h"
#include "ABBConnectorImp.h"
using namespace msclr::interop;
using namespace System;
using namespace ABBConnectSDK;

/******************************************************************
 * @brief
 * @details
 * @return
 * @author    Qiao Tan
 * @date      2019/11/26
 ******************************************************************/
ABBConnectorImp::ABBConnectorImp()
{
    robot_manager_ = gcnew RobotManager();
    this->connect();
}

/******************************************************************
 * @brief
 * @details
 * @return
 * @author    Qiao Tan
 * @date      2019/11/26
 ******************************************************************/
ABBConnectorImp::~ABBConnectorImp()
{
    this->disconnect();
}

/******************************************************************
 * @brief
 * @details
 * @return    int
 * @author    Qiao Tan
 * @date      2019/11/26
 ******************************************************************/
int ABBConnectorImp::connect()
{
    int ret;

    if (is_connected_)
    {
        return 0;
    }
    ret = robot_manager_->connect();
    if (ret == 0)
    {
        is_connected_ = true;
    }
    return ret;
}

/******************************************************************
 * @brief
 * @details
 * @return    int
 * @author    Qiao Tan
 * @date      2019/11/26
 ******************************************************************/
int ABBConnectorImp::disconnect()
{
    int ret;

    if (!is_connected_)
    {
        return 0;
    }
    ret = robot_manager_->disconnect();
    if (ret == 0)
    {
        is_connected_ = false;
    }
    return ret;
}

/******************************************************************
 * @brief
 * @details
 * @param     task_name
 * @param     module_name
 * @param     var_name
 * @param     value
 * @return    int
 * @author    Qiao Tan
 * @date      2019/11/26
 ******************************************************************/
int ABBConnectorImp::getValue(std::string task_name, std::string module_name, std::string var_name, double& value)
{
    if (this->connect() != 0)
    {
        return err_no_controller;
    }
    System::String^ sys_task = getSystemString(task_name);
    System::String^ sys_module = getSystemString(module_name);
    System::String^ sys_var = getSystemString(var_name);
    return robot_manager_->getValue(sys_task, sys_module, sys_var, value);
}

/******************************************************************
 * @brief
 * @details
 * @param     task_name
 * @param     module_name
 * @param     var_name
 * @param     value
 * @return    int
 * @author    Qiao Tan
 * @date      2019/11/26
 ******************************************************************/
int ABBConnectorImp::getValue(std::string task_name, std::string module_name, std::string var_name, double value[6])
{
    int ret;

    if (this->connect() != 0)
    {
        return err_no_controller;
    }
    System::String^ sys_task = getSystemString(task_name);
    System::String^ sys_module = getSystemString(module_name);
    System::String^ sys_var = getSystemString(var_name);
    cli::array<double> ^arr;
    ret = robot_manager_->getValue(sys_task, sys_module, sys_var, arr);
    if (ret == 0)
    {
        for (int i = 0; i < 6; ++i)
        {
            value[i] = arr[i];
        }
    }
    return ret;
}

/******************************************************************
 * @brief
 * @details
 * @param     task_name
 * @param     module_name
 * @param     var_name
 * @param     value
 * @return    int
 * @author    Qiao Tan
 * @date      2019/11/26
 ******************************************************************/
int ABBConnectorImp::setValue(std::string task_name, std::string module_name, std::string var_name, double value)
{
    if (this->connect() != 0)
    {
        return err_no_controller;
    }
    System::String^ sys_task = getSystemString(task_name);
    System::String^ sys_module = getSystemString(module_name);
    System::String^ sys_var = getSystemString(var_name);
    return robot_manager_->setValue(sys_task, sys_module, sys_var, value);
}

/******************************************************************
 * @brief
 * @details
 * @param     task_name
 * @param     module_name
 * @param     var_name
 * @param     value
 * @return    int
 * @author    Qiao Tan
 * @date      2019/11/26
 ******************************************************************/
int ABBConnectorImp::setValue(std::string task_name, std::string module_name, std::string var_name, double value[6])
{
    if (this->connect() != 0)
    {
        return err_no_controller;
    }
    System::String^ sys_task = getSystemString(task_name);
    System::String^ sys_module = getSystemString(module_name);
    System::String^ sys_var = getSystemString(var_name);
    cli::array<double> ^arr = gcnew array<double>(6);
    for (int i = 0; i < 6; ++i)
    {
        arr[i] = value[i];
    }
    return robot_manager_->setValue(sys_task, sys_module, sys_var, arr);
}

/******************************************************************
 * @brief
 * @details
 * @param     value
 * @return    int
 * @author    Qiao Tan
 * @date      2019/11/26
 ******************************************************************/
int ABBConnectorImp::getCurrentLoc(double value[])
{
    int ret;

    if (this->connect() != 0)
    {
        return err_no_controller;
    }
    cli::array<double> ^arr;
    ret = robot_manager_->getCurrentLoc(arr);
    if (ret == 0)
    {
        for (int i = 0; i < 6; ++i)
        {
            value[i] = arr[i];
        }
    }
    return ret;
}

/******************************************************************
 * @brief
 * @details
 * @param     str
 * @return    System::String^
 * @author    Qiao Tan
 * @date      2019/11/26
 ******************************************************************/
System::String^ ABBConnectorImp::getSystemString(std::string str)
{
    return marshal_as<System::String^>(str.c_str());
}

/******************************************************************
 * @brief
 * @details
 * @param     str
 * @return    std::string
 * @author    Qiao Tan
 * @date      2019/11/26
 ******************************************************************/
std::string ABBConnectorImp::getStdString(System::String^ str)
{
    return marshal_as<std::string>(str);
}
