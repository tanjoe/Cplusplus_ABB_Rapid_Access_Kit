using Microsoft.VisualStudio.TestTools.UnitTesting;
using ABBConnectSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ABBConnectSDK.Tests
{
    [TestClass()]
    public class RobotManagerTests
    {
        RobotManager manager = new RobotManager();

        [TestMethod()]
        public void connectTest()
        {
            var ret = manager.connect();
            Assert.AreEqual(ret, 0);
        }

        [TestMethod()]
        public void setAndGetValueTest()
        {
            var ret = manager.setValue("T_ROB1", "MotionModule", "cameraTriggerFlag", 1);
            Assert.AreEqual(ret, 0);
            double get_result;
            ret = manager.getValue("T_ROB1", "MotionModule", "cameraTriggerFlag", out get_result);
            Assert.AreEqual(ret, 0);
            Assert.AreEqual(get_result, 1.0);

            double[] target_loc = new double[6];
            for (int i = 0; i < 50; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    target_loc[j] = i + j + 1;
                }
                Stopwatch stop_watch = Stopwatch.StartNew();
                ret = manager.setValue("T_ROB1", "MotionModule", "targetLoc", target_loc);
                Assert.AreEqual(ret, 0);
                double[] arr_result;
                ret = manager.getValue("T_ROB1", "MotionModule", "targetLoc", out arr_result);
                stop_watch.Stop();
                Console.WriteLine("execution time: {0}", stop_watch.ElapsedMilliseconds);
                Assert.AreEqual(ret, 0);
                for (int k = 0; k < 3; ++k)
                {
                    if (Math.Abs(target_loc[k] - arr_result[k]) > 0.1)
                    {
                        Console.Write("{0}\t{1}", target_loc[k], arr_result[k]);
                        Assert.Fail();
                    }
                }
            }
        }

        [TestMethod()]
        public void getCurrentLocTest()
        {
            double[] result;
            var ret = manager.getCurrentLoc(out result);
            Assert.AreEqual(ret, 0);
            for (int i = 0; i < 6; ++i)
            {
                Console.Write("{0}\t", result[i]);
            }
        }

        [TestMethod()]
        public void exceptionTest()
        {
            int ret;
            double d_result;
            double[] a_result;

            //non-existant task
            ret = manager.getValue("Null", "MotionModule", "cameraTriggerFlag", out d_result);
            Assert.IsTrue(ret != 0);

            //non-existant module
            ret = manager.getValue("T_ROB1", "Null", "cameraTriggerFlag", out d_result);
            Assert.IsTrue(ret != 0);

            //non-existant variable
            ret = manager.getValue("T_ROB1", "MotionModule", "null", out d_result);
            Assert.IsTrue(ret != 0);

            //non-existant task
            ret = manager.getValue("Null", "MotionModule", "cameraTriggerFlag", out a_result);
            Assert.IsTrue(ret != 0);

            //non-existant module
            ret = manager.getValue("T_ROB1", "Null", "cameraTriggerFlag", out a_result);
            Assert.IsTrue(ret != 0);

            //non-existant variable
            ret = manager.getValue("T_ROB1", "MotionModule", "null", out a_result);
            Assert.IsTrue(ret != 0);

            double d_to_set = 1.0;
            double[] a_to_set = { 100, 100, 100, 100, 100, 100 };

            //non-existant task
            ret = manager.setValue("Null", "MotionModule", "cameraTriggerFlag", d_to_set);
            Assert.IsTrue(ret != 0);

            //non-existant module
            ret = manager.setValue("T_ROB1", "Null", "cameraTriggerFlag", d_to_set);
            Assert.IsTrue(ret != 0);

            //non-existant variable
            ret = manager.setValue("T_ROB1", "MotionModule", "null", d_to_set);
            Assert.IsTrue(ret != 0);

            //non-existant task
            ret = manager.setValue("Null", "MotionModule", "cameraTriggerFlag", a_to_set);
            Assert.IsTrue(ret != 0);

            //non-existant module
            ret = manager.setValue("T_ROB1", "Null", "cameraTriggerFlag", a_to_set);
            Assert.IsTrue(ret != 0);

            //non-existant variable
            ret = manager.setValue("T_ROB1", "MotionModule", "null", a_to_set);
            Assert.IsTrue(ret != 0);
        }

        [TestMethod()]
        public void disconnectTest()
        {
            var ret = manager.disconnect();
            Assert.AreEqual(ret, 0);
        }
    }
}