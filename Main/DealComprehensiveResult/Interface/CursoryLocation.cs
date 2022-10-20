using BasicClass;
using DealCalibrate;
using DealComprehensive;
using DealPLC;
using DealResult;
using DealRobot;
using Main_EX;
using ModulePackage;
using System;
using System.Collections;
using System.Windows;
using DealLog;
namespace Main
{
    public partial class BaseDealComprehensiveResult_Main
    {
        #region 粗定位
        /// <summary>
        /// 九宫格标定结果计算，只支持匹配T，否则模板角度会导致出错
        /// </summary>
        /// <param name="index">工位号</param>
        /// <param name="cellName"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public StateComprehensive_enum CalcPickPos(int index, string cellName, out Hashtable htResult)
        {
            htResult = null;
            try
            {
                LogicRobot.L_I.WriteRobotCMD(ModelParams.AdjPickOffset, ModelParams.cmd_PickOffset);
                #region 空跑
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    //if (cnt == glassnum)
                    //{
                    //    ShowState(string.Format("当前工位{0}取片完成，共{1}片！", index, cnt));
                    //    LogicPLC.L_I.WriteRegData1((int)DataRegister1.NextStation, index);
                    //    cnt = 0;
                    //    glassnum = rd.Next(1, 20);
                    //}
                    //else
                    //{
                        cnt++;
                        Point4D dst = ModelParams.PickPos.Add(2, 50);
                        LogicRobot.L_I.WriteRobotCMD(dst, ModelParams.cmd_PickPos);
                        ShowState(string.Format("空跑模式，发送给机器人坐标:X:{0},Y:{1},Z:{2}", 
                            dst.DblValue1.ToString("f3"), dst.DblValue2.ToString("f3"), dst.DblValue3.ToString("f3")));
                    //}
                    FinishPhotoPLC(1);
                    return StateComprehensive_enum.True;
                }
                #endregion

                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                ResultScaledShape result = (ResultScaledShape)htResult[cellName];
                ResultTemplate template = (ResultTemplate)htResult[cellName + @"T"];

                int pos = index > 2 ? index - 2 : index;
                
                //如果没有匹配到就进行工位相关的判断
                if (result.X * result.Y == 0)
                {
                    //如果工位号已经和预设的工位数相同，即代表整个中片已经做完
                    if (pos == ModelParams.PickStationNum)
                    {
                        //判断已取片数是否与配方中的中片数一致，如果不一致则弹框提醒，一致则直接通知PLC工位取完
                        //if (RegeditMain.R_I.PickCnt < ModelParams.confPickSum)
                        //{
                        //    if (MessageBox.Show("是否拖拽报表纸", "提示", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        //    {
                        //        //最大工位号+1，作为人工确认中片未取完
                        //        LogicPLC.L_I.WriteRegData1((int)DataRegister1.NextStation, index + 1);
                        //        return StateComprehensive_enum.True;
                        //    }
                        //    else
                        //        RegeditMain.R_I.PickCnt = 0;
                        //}
                        //else
                        RegeditMain.R_I.PickCnt = 0;
                    }
                    // 通知当前工位取完，index=工位号，plc提供
                    LogicPLC.L_I.WriteRegData1((int)DataRegister1.NextStation, index);
                    //FinishPhotoPLC(CameraResult.OK);
                    ShowState(string.Format("粗定位工位{0}未识别到玻璃！", index));
                    g_UCDisplayCamera.ShowResult("未识别到产品" ,"red");
                }
                else
                {
                    //发送机器人取片位置
                    if (!CursoryLocation.SendRobotPickPos(
                        new Point2D(result.X, result.Y),
                        ModelParams.PickPos,//基准取片位，主要提供高度和角度，空跑提供坐标
                        ModelParams.AdjPickPos.Add(3, result.R_J + template.RCenterProfile / Math.PI * 180),//角度附加匹配角度和模板本身角度
                        ModelParams.cmd_PickPos,//机器人取片协议
                        ParCalibRobot1.P_I,
                        out Point4D pResult))//用于多标定配置
                    {
                        ShowAlarm("机器人坐标计算失败！");
                        g_UCDisplayCamera.ShowResult("机器人坐标计算失败", "red");
                        WinError.GetWinInst().ShowError("机器人坐标计算失败");
                    }
                    else
                    {
                        g_UCDisplayCamera.ShowResult("当前识别产品数：" + result.X_L.Count+
                            "\n取料位：\n" + pResult.ToString());
                        //判断是否和
                        if (ModelParams.DumpList_Solid.Contains((RegeditMain.R_I.PickCnt + 1).ToString()))
                        {
                            ShowState(string.Format("通知机器人当前第{0}标记为NG片", RegeditMain.R_I.PickCnt + 1));
                            LogicRobot.L_I.WriteRobotCMD(ModelParams.cmd_PickDump);
                        }                            
                    }
                    FinishPhotoPLC(CameraResult.OK);
                }

                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                FinishPhotoPLC(CameraResult.NG);
                return StateComprehensive_enum.False;
            }
            finally
            {
                ShowState(string.Format("当前中片已取：{0}片", RegeditMain.R_I.PickCnt));
            }
        }

        #endregion
    }
}
