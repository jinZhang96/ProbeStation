using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using System.Threading;
using System.Threading.Tasks;
using DealFile;
using DealComprehensive;
using Common;
using SetPar;
using ParComprehensive;
using BasicClass;
using Camera;
using System.Collections;
using DealResult;
using DealConfigFile;
using DealCalibrate;
using DealRobot;
using DealMath;
using DealImageProcess;
using BasicComprehensive;
using System.Diagnostics;
using BasicDisplay;
using Main_EX;


namespace Main
{
    public partial class DealComprehensiveResult2 : BaseDealComprehensiveResult_Main
    {

        #region 定义
        public double Mark1X = 0;
        public double Mark1Y = 0;

        public double StdMark1X = 0;
        public double StdMark1Y = 0;

        public double Mark2X = 0;
        public double Mark2Y = 0;

        public double StdMark2X = 0;
        public double StdMark2Y = 0;

        public ResultCalibRotate resultCalibRotate = null;
        #endregion 定义

        #region 位置1拍照
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun1(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 1;
            bool blResult = true;
            BlFinishPos_Cam2 = false;
            BlResultPos1_Cam2 = false;
            BlFinishPos1_Cam2 = false;
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                double Amp = ParCalibWorld.V_I[2];                
                StateComprehensive_enum stateComprehensive_e =
                    g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);

                if (!ParSetWork.P_I[8].BlSelect)
                {
                    BaseResult resultMatch = null;

                        resultMatch = (BaseResultMatch)htResult["C6"];
                        Mark1X = resultMatch.X * Amp;
                        Mark1Y = resultMatch.Y * Amp;

                        StdMark1X = resultMatch.StdX * Amp;
                        StdMark1Y = resultMatch.StdY * Amp;
                

                    if (resultMatch.X == 0 && resultMatch.Y == 0)
                    {
                        LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                        g_UCDisplayCamera.DispString("NG", 26, 80, 20, "red");
                        ShowAlarm("Stage1，相机2MARK1未识别！");
                        return stateComprehensive_e;
                    }
                    resultCalibRotate = (ResultCalibRotate)htResult["C21"];//旋转中心结果类
                  
                   
                        #region 使用双目相机计算
                        BlFinishPos_Cam2 = true;
                        CalDelta(1);
                        #endregion
                   
                }
                else
                {
                    #region 图像处理
                    BaseResultMatch baseResultMatch = (BaseResultMatch)htResult["C3"]; //匹配定位
                    ResultCrossLines resultCrossLines = htResult["C4"] as ResultCrossLines;   //抓取直线      
                    ResultCalibRotate resultCalibRotate = (ResultCalibRotate)htResult["C5"];//旋转中心结果类               
                    #endregion

                    if (baseResultMatch.X * baseResultMatch.Y == 0 || resultCrossLines.X * resultCrossLines.Y == 0 ||
                        Math.Abs(resultCrossLines.IncludedR_J - 90) > ParAdjust.Value1("adj6"))
                    {
                        ShowState("相机2位置1未匹配到产品,产品夹角超出设定偏差！");
                        LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                        BlResultPos1_Cam2 = false;
                        BlFinishPos1_Cam2 = true;
                        return StateComprehensive_enum.False;
                    }

                    PCam2StdPos1 = new Point3D(resultCrossLines.StdX * Amp, resultCrossLines.StdY * Amp, resultCrossLines.StdR_J);  //基准坐标
                    PCam2WorkPos1 = new Point3D(resultCrossLines.X * Amp, resultCrossLines.Y * Amp, resultCrossLines.R_J);  //拍照坐标    

                    PRotateCenterCam2_stage1 = new Point2D(resultCalibRotate.PointRC.DblValue1 * Amp, resultCalibRotate.PointRC.DblValue2 * Amp);
                    BlResultPos1_Cam2 = true;
                    BlFinishPos1_Cam2 = true;
                    //拍照完成信号
                    LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 1);
                }

                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
        #endregion 位置1拍照

        #region 位置2拍照
        public override StateComprehensive_enum DealComprehensiveResultFun2(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 2;
            bool blResult = true;
            BlFinishPos_Cam2 = false;
            BlResultPos2_Cam2 = false;
            BlFinishPos2_Cam2 = false;
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                double Amp = ParCalibWorld.V_I[2];
                int pos = 2;
                StateComprehensive_enum stateComprehensive_e =
                    g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos2, out htResult);

                if (!ParSetWork.P_I[8].BlSelect)
                {
                    BaseResult resultMatch = null;

                   
                        resultMatch = (BaseResultMatch)htResult["C15"];
                        Mark1X = resultMatch.X * Amp;
                        Mark1Y = resultMatch.Y * Amp;

                        StdMark1X = resultMatch.StdX * Amp;
                        StdMark1Y = resultMatch.StdY * Amp;
                    

                    if (resultMatch.X == 0 && resultMatch.Y == 0)
                    {
                        LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                        g_UCDisplayCamera.DispString("NG", 26, 80, 20, "red");
                        ShowAlarm("Stage2，相机2MARK1未识别！");
                        return stateComprehensive_e;
                    }
                    resultCalibRotate = (ResultCalibRotate)htResult["C21"];//旋转中心结果类
                  
                    
                        #region 使用双目相机计算
                        BlFinishPos_Cam2 = true;
                        CalDelta(2);
                        #endregion
                    
                }
                else
                {
                    #region 图像处理
                    BaseResultMatch baseResultMatch = (BaseResultMatch)htResult["C9"];
                    ResultCrossLines resultCrossLines = htResult["C10"] as ResultCrossLines;
                    #endregion

                    if (baseResultMatch.X * baseResultMatch.Y == 0 || resultCrossLines.X * resultCrossLines.Y == 0 ||
                        Math.Abs(resultCrossLines.IncludedR_J - 90) > ParAdjust.Value1("adj6"))
                    {
                        ShowState("相机2位置2未匹配到产品,产品夹角超出设定偏差！");
                        LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                        BlResultPos2_Cam2 = false;
                        BlFinishPos2_Cam2 = true;
                        return StateComprehensive_enum.False;
                    }

                    PCam2StdPos2 = new Point3D(resultCrossLines.StdX * Amp, resultCrossLines.StdY * Amp, resultCrossLines.StdR_J);
                    PCam2WorkPos2 = new Point3D(resultCrossLines.X * Amp, resultCrossLines.Y * Amp, resultCrossLines.R_J);

                    BlResultPos2 = true;
                    BlFinishPos2 = true;

                    //计算偏差并发送给PLC
                    if (CalDeltaProd3())
                    {

                    }
                    else
                    {
                        LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                        return StateComprehensive_enum.False;
                    }
                }
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录

                Display(Pos_enum.Pos2, htResult, blResult, sw);

                #endregion 显示和日志记录
            }
        }
        #endregion 位置2拍照


        public bool DealCam2(int intStationNo, bool BlResult, double[] delta)
        {
            try
            {
                if (BlResult)
                {
                    ShowState("Stage" + intStationNo.ToString() + "相机2- X: " + delta[0].ToString() + "," + "Y: " + delta[1].ToString() + "," + "R: " + Math.Round(delta[3], 2).ToString());
                    //拍照结果
                    LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 1);
                    g_UCDisplayCamera.DispString("OK", 26, 80, 20, "green");
                    //写入偏差值
                    LogicPLC.L_I.WriteCalData(g_regData_L, delta, g_regFinishData, 11);
                }
                else
                {
                    LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                    ShowAlarm("Stage" + intStationNo.ToString() + "偏差计算异常！");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region 位置3拍照-stage2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun3(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = null;
            BlResultPos3_Cam2 = true;
            BlFinishPos3_Cam2 = true;
            bool blResult = true;
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义

            try
            {
                double Amp = ParCalibWorld.V_I[2];
                StateComprehensive_enum stateComprehensive_e =
                   g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos3, out htResult);
                #region 图像处理
                BaseResultMatch baseResultMatch = (BaseResultMatch)htResult["C15"];
                ResultCrossLines resultCrossLines = htResult["C16"] as ResultCrossLines;
                ResultCalibRotate resultCalibRotate = (ResultCalibRotate)htResult["C17"];//旋转中心结果类   
                #endregion

                if (baseResultMatch.X * baseResultMatch.Y == 0 || resultCrossLines.X * resultCrossLines.Y == 0 ||
                    Math.Abs(resultCrossLines.IncludedR_J - 90) > ParAdjust.Value1("adj6"))
                {
                    ShowState("相机2位置3未匹配到产品,产品夹角超出设定偏差！");
                    LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                    BlResultPos3_Cam2 = false;
                    BlFinishPos3_Cam2 = true;
                    blResult = false;
                    return StateComprehensive_enum.False;
                }
                PCam2StdPos3 = new Point2D(resultCrossLines.StdX * Amp, resultCrossLines.StdY * Amp);
                PCam2WorkPos3 = new Point2D(resultCrossLines.X * Amp, resultCrossLines.Y * Amp);
                //位置3旋转中心
                PRotateCenterCam2_stage2 = new Point2D(resultCalibRotate.PointRC.DblValue1 * Amp, resultCalibRotate.PointRC.DblValue2 * Amp);

                BlResultPos3_Cam2 = true;
                BlFinishPos3_Cam2 = true;
                LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 1);
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                ShowAlarm("相机2位置3处理异常，请查看异常信息日志！");
                LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                BlResultPos3_Cam2  = false;
                BlFinishPos3_Cam2 = true;
                blResult = false;
                Log.L_I.WriteError("BaseDealComprehensiveResult", ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos3, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
        #endregion 位置3拍照

        #region 位置4拍照-stage2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun4(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = null;
            BlResultPos4_Cam2 = true;
            BlFinishPos4_Cam2 = true;
            bool blResult = true;
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                double Amp = ParCalibWorld.V_I[2];
                StateComprehensive_enum stateComprehensive_e =
                   g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos4, out htResult);
                #region 图像处理
                BaseResultMatch baseResultMatch = (BaseResultMatch)htResult["C21"];
                ResultCrossLines resultCrossLines = htResult["C22"] as ResultCrossLines;
                #endregion

                if (baseResultMatch.X * baseResultMatch.Y == 0 || resultCrossLines.X * resultCrossLines.Y == 0 ||
                    Math.Abs(resultCrossLines.IncludedR_J - 90) > ParAdjust.Value1("adj6"))
                {
                    ShowState("相机2位置4未匹配到产品,产品夹角超出设定偏差！");
                    LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                    BlResultPos4_Cam2 = true;
                    BlFinishPos4_Cam2 = true;
                    blResult = false;
                    return StateComprehensive_enum.False;
                }

                PCam2StdPos4 = new Point2D(resultCrossLines.StdX * Amp, resultCrossLines.StdY * Amp);
                PCam2WorkPos4 = new Point2D(resultCrossLines.X * Amp, resultCrossLines.Y * Amp);
                BlResultPos4_Cam2 = true;
                BlFinishPos4_Cam2 = true;
                //计算偏差并发送给PLC
                if (CalDeltaProd4())
                {

                }
                else
                {
                    LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                    blResult = false;
                    return StateComprehensive_enum.False;
                }
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                ShowAlarm("相机2位置4处理异常，请查看异常信息日志！");
                LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                BlResultPos4_Cam2 = false;
                BlFinishPos4_Cam2 = true;
                blResult = false;
                Log.L_I.WriteError("BaseDealComprehensiveResult", ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos4, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
        #endregion 位置4拍照
    }
}
