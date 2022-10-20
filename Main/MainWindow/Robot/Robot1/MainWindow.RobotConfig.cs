using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealRobot;
using System.Threading;
using System.Threading.Tasks;
using BasicClass;
using DealLog;
using DealPLC;
using DealConfigFile;

namespace Main
{
    partial class MainWindow
    {
        /// <summary>
        /// 给机器人发送配置参数
        /// </summary>
        public override void ConfigRobot_Task()
        {
            try
            {
                if (ParSetRobot.P_I.TypeRobot_e == TypeRobot_enum.Null)
                {
                    return;
                }
                if (RegeditRobot.R_I.BlOffLineRobot)
                {
                    return;
                }
                #region 清空旧的参数
                LogicRobot.L_I.ParRobotCom_L.Clear();
                LogicRobot.L_I.ParRobot1_L.Clear();
                LogicRobot.L_I.ParRobot2_L.Clear();
                LogicRobot.L_I.ParRobot3_L.Clear();
                LogicRobot.L_I.ParRobot4_L.Clear();

                LogicRobot.L_I.ParRobotCom_P4L.Clear();
                LogicRobot.L_I.ParRobot1_P4L.Clear();
                LogicRobot.L_I.ParRobot2_P4L.Clear();
                LogicRobot.L_I.ParRobot3_P4L.Clear();
                LogicRobot.L_I.ParRobot4_P4L.Clear();
                #endregion 清空旧的参数
                LogicRobot.L_I.ParRobotCom_P4L.Add(ModelParams.PrecisePos);
                LogicRobot.L_I.ParRobotCom_P4L.Add(ModelParams.DumpPos);
                LogicRobot.L_I.ParRobotCom_P4L.Add(new Point4D(ModelParams.stdRobotAutoSpeed, 
                    ModelParams.stdRobotLowSpeed, ModelParams.stdRobotResetSpeed, 0));

                //发送参数
                Task task = new Task(LogicRobot.L_I.WriteConfigRobot);
                task.Start();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
       
    }
}
