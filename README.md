# Cplusplus_ABB_Rapid_Access_Kit

ABB提供的PC SDK仅原生支持C＃和VB。 此仓库内工程的目标（以及结果）是生成一组非托管C ++ 的API，完成读写ABB机器人RAPID数据的任务。

The PC SDK provided by ABB only natively supports C# and VB. The goal (as well as the result) of the projects in this repo, is to generate a set of unmanaged C++ API that could perform the task of reading/writing RAPID data in an ABB robot.



## 功能说明

由于目标是提供ABB机器人与工控机间的连接，故只实现了以下功能：

* 提供了对ABB机器人内bool、num、robtarget类型数据的读写
* 提供了对ABB机器人当前位置的读取



## 工程说明

整体的思路如下：

`非托管C++(调用方) ---> 托管C++(中间层) ---> C#(PC SDK调用)`

解决方案的对应工程如下：

**ABBConnectSDK:** C#工程，在其中完成了对PC SDK的实际调用

**ABBConnectSDKTests:** C#单元测试工程，在其中对`ABBConnectSDK`中的接口进行测试

**ABBConnectorDLLWrapper:** 托管C++工程，在其中对`ABBConnectSDK`编译出的dll进行封装，对外提供标准C++的接口

**WrapperConsoleTest:** 非托管C++工程，在其中对`ABBConnectorDLLWrapper`的接口进行测试



## 编译

`ABBConnectSDK`是所有其它项目的依赖项，编译`ABBConnectSDK`时要求电脑上已经安装了ABB的PC SDK，下载地址 http://developercenter.robotstudio.com/downloads_pc 



## 使用

对于一个非托管C++项目而言，若要使用这一组接口，需要使用的文件有

* dll
  * ABB PC SDK的dll
    * ABB.Robotics.Controllers.PC.dll
    * RobotStudio.Services.RobApi.Desktop.dll
    * RobotStudio.Services.RobApi.dll
  * `ABBConnectSDK`生成的dll
    * ABBConnectSDK.dll
  * `ABBConnectorDLLWrapper`生成的dll
    * ABBConnectorDLLWrapper.dll
* lib
  * `ABBConnectorDLLWrapper`生成的lib
    * ABBConnectorDLLWrapper.lib
* header
  * `ABBConnectorDLLWrapper`对外的头文件
    * ABBConnector.h
    * ABBConnectorFactory.h

可以直接下载项目内的**ABBConnectorSDK.zip**，其中包含了所有上述文件

**注意，ABB提供的DLL还依赖于ABB Robot Communication Runtime运行环境，该运行环境的安装包包含在PC SDK的安装包内。所以，即便不进行开发，仅使用这一组API也需要安装ABB Robot Communication Runtime，否则程序执行时会崩溃**
