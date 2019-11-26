using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;
using System;
using System.Linq;

namespace ABBConnectSDK
{
    public class RobotManager
    {
        enum ErrorCode:int
        {
            err_no_controller = -1,
            err_no_such_var = -2,
            err_incompatible_var = -3,
            err_get_mastership_failed = -4,
            err_illegal_array = -5,
            err_no_such_task = -6
        };

        public static ControllerInfoCollection controllers = null;
        public static Controller controller = null;

        /// <summary>
        /// 扫描ABB控制器并连接到第一台设备
        /// </summary>
        /// <returns>
        /// 0: 连接正常
        /// -1: 未发现设备
        /// </returns>
        public int connect()
        {
            int ret = 0;

            NetworkScanner netscan = new NetworkScanner();
            netscan.Scan();
            controllers = netscan.Controllers;
            if (controllers.Count() < 1)
            {
                ret = (int)ErrorCode.err_no_controller;
            }
            controller = new Controller(controllers[0]);
            controller.Logon(UserInfo.DefaultUser);
            return ret;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        public int disconnect()
        {
            int ret = 0;

            if (controller != null)
            {
                controller.Logoff();
                controller.Dispose();
            }
            return ret;
        }

        /// <summary>
        /// 获取机器人RAPID数据
        /// </summary>
        /// <param name="task_name"></param>
        /// <param name="module_name"></param>
        /// <param name="var_name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int getValue(string task_name, string module_name, string var_name, out double value)
        {
            int ret = 0;
            RapidData rapid_data;

            value = -1.0;
            controller.Logon(UserInfo.DefaultUser);
            try
            {
                rapid_data = controller.Rapid.GetRapidData(task_name, module_name, var_name);
            }
            catch (ABB.Robotics.GenericControllerException)
            {
                ret = (int)ErrorCode.err_no_such_var;
                return ret;
            }
            if (rapid_data.Value is Bool)
            {
                Bool rapid_bool = (Bool)rapid_data.Value;
                value = Convert.ToDouble(rapid_bool.Value);
            }
            if (rapid_data.Value is ABB.Robotics.Controllers.RapidDomain.Num)
            {
                Num rapid_num = (Num)rapid_data.Value;
                value = rapid_num.Value;
            }
            return ret;
        }

        /// <summary>
        /// 获取指定点位变量
        /// </summary>
        /// <param name="task_name"></param>
        /// <param name="module_name"></param>
        /// <param name="var_name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int getValue(string task_name, string module_name, string var_name, out double[] value)
        {
            int ret = 0;
            RapidData rapid_data;

            value = new double[6];
            controller.Logon(UserInfo.DefaultUser);
            try
            {
                rapid_data = controller.Rapid.GetRapidData(task_name, module_name, var_name);
            }
            catch (ABB.Robotics.GenericControllerException)
            {
                ret = (int)ErrorCode.err_no_such_var;
                return ret;
            }
            if (rapid_data.Value is RobTarget)
            {
                RobTarget rapid_target = (RobTarget)rapid_data.Value;
                value[0] = rapid_target.Trans.X;
                value[1] = rapid_target.Trans.Y;
                value[2] = rapid_target.Trans.Z;
                rapid_target.Rot.ToEulerAngles(out value[3], out value[4], out value[5]);
            }
            else
            {
                ret = (int)ErrorCode.err_incompatible_var;
                return ret;
            }
            return ret;
        }

        /// <summary>
        /// 设置机器人RAPID数据
        /// </summary>
        /// <param name="task_name"></param>
        /// <param name="module_name"></param>
        /// <param name="var_name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int setValue(string task_name, string module_name, string var_name, double value)
        {
            int ret = 0;
            RapidData rapid_data;

            controller.Logon(UserInfo.DefaultUser);
            try
            {
                rapid_data = controller.Rapid.GetRapidData(task_name, module_name, var_name);
            }
            catch (ABB.Robotics.GenericControllerException)
            {
                ret = (int)ErrorCode.err_no_such_var;
                return ret;
            }
            try
            {
                using (Mastership.Request(controller.Rapid))
                {
                    if (rapid_data.Value is Bool)
                    {
                        Bool rapid_bool = (Bool)rapid_data.Value;
                        rapid_bool.Value = Convert.ToBoolean(value);
                        rapid_data.Value = rapid_bool;
                    }
                    if (rapid_data.Value is Num)
                    {
                        Num rapid_num = (Num)rapid_data.Value;
                        rapid_num.Value = value;
                        rapid_data.Value = rapid_num;
                    }
                }
            }
            catch (ABB.Robotics.GenericControllerException)
            {
                ret = (int)ErrorCode.err_get_mastership_failed;
                return ret;
            }
            return ret;
        }

        /// <summary>
        /// 设置点位
        /// </summary>
        /// <param name="task_name"></param>
        /// <param name="module_name"></param>
        /// <param name="var_name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int setValue(string task_name, string module_name, string var_name, double[] value)
        {
            int ret = 0;
            RapidData rapid_data;

            if (value.Length != 6)
            {
                ret = (int)ErrorCode.err_illegal_array;
                return ret;
            }
            controller.Logon(UserInfo.DefaultUser);
            try
            {
                rapid_data = controller.Rapid.GetRapidData(task_name, module_name, var_name);
            }
            catch (ABB.Robotics.GenericControllerException)
            {
                ret = (int)ErrorCode.err_no_such_var;
                return ret;
            }
            try
            {
                if (!(rapid_data.Value is RobTarget))
                {
                    ret = (int)ErrorCode.err_incompatible_var;
                    return ret;
                }
                RobTarget target_loc = (RobTarget)rapid_data.Value;
                target_loc.Trans.X = (float)value[0];
                target_loc.Trans.Y = (float)value[1];
                target_loc.Trans.Z = (float)value[2];
                target_loc.Rot.FillFromEulerAngles(value[3], value[4], value[5]);
                using (Mastership.Request(controller.Rapid))
                {
                    rapid_data.Value = target_loc;
                }
            }
            catch (ABB.Robotics.GenericControllerException)
            {
                ret = (int)ErrorCode.err_get_mastership_failed;
                return ret;
            }
            return ret;
        }

        /// <summary>
        /// 获取机器人当前位置。姿态使用XYZ欧拉角表示
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public int getCurrentLoc(out double[] arr)
        {
            int ret = 0;

            arr = new double[6];
            controller.Logon(UserInfo.DefaultUser);
            ABB.Robotics.Controllers.RapidDomain.Task motion_task = controller.Rapid.GetTask("T_ROB1");
            if (motion_task == null)
            {
                ret = (int)ErrorCode.err_no_such_task;
                return ret;
            }
            RobTarget current_loc = motion_task.GetRobTarget();
            arr[0] = current_loc.Trans.X;
            arr[1] = current_loc.Trans.Y;
            arr[2] = current_loc.Trans.Z;
            current_loc.Rot.ToEulerAngles(out arr[3], out arr[4], out arr[5]);
            return ret;
        }
    }
}
