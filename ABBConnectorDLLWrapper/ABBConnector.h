#pragma once
#include <string>

#ifdef ABBCONNECTORDLLWRAPPER_EXPORTS
#define ABB_API __declspec(dllexport)
#else
#define ABB_API __declspec(dllimport)
#endif

class ABB_API ABBConnector
{
public:
    enum ErrorCode
    {
        err_no_controller = -1,
        err_no_such_var = -2,
        err_incompatible_var = -3,
        err_get_mastership_failed = -4,
        err_illegal_array = -5,
        err_no_such_task = -6
    };

public:
    ABBConnector();
    virtual ~ABBConnector();

public:
    virtual int getValue(std::string task_name, std::string module_name, std::string var_name, double& value) = 0;
    virtual int getValue(std::string task_name, std::string module_name, std::string var_name, double value[6]) = 0;
    virtual int setValue(std::string task_name, std::string module_name, std::string var_name, double value) = 0;
    virtual int setValue(std::string task_name, std::string module_name, std::string var_name, double value[6]) = 0;
    virtual int getCurrentLoc(double value[]) = 0;
};

