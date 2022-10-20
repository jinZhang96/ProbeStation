using BasicClass;
using DealComprehensive;
using System;
using System.Collections;
using System.Diagnostics;
using DealCalibrate;
using DealPLC;
using DealResult;
using DealConfigFile;


namespace Main
{
    public partial class DealComprehensiveResult1 : BaseDealComprehensiveResult_Main
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

        #region 相机1位置1拍照
        /// <summary>
        /// </summary>
        
        //
        public override StateComprehensive_enum DealComprehensiveResultFun1(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            PosNow_e = Pos_enum.Pos1;//当前拍照位置
            bool blResult = true;
            bool SaveImage = false;
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                //相机像素精度
                double Amp = ParCalibWorld.V_I[1];
                //？？
                StateComprehensive_enum stateComprehensive_e =
                    g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                //如果运行模式中序号8不勾选为传统探针方式点亮，勾选为插拔模式点亮
                if (!ParSetWork.P_I[8].BlSelect)
                {
                    #region 传统探针方式点亮
                    //定义一个resultMatch局部变量
                    BaseResult resultMatch = null;
                    //将C6算子结果赋值给resultMatch变量
                    resultMatch = (BaseResultMatch)htResult["C6"];
                    //将像素坐标转换成实际坐标  像素坐标*AMP
                    Mark1X = resultMatch.X * Amp;
                    Mark1Y = resultMatch.Y * Amp;

                    //基准坐标
                    StdMark1X = resultMatch.StdX * Amp;
                    StdMark1Y = resultMatch.StdY * Amp;

                    //如果X和Y坐标同时为0，则报错NG
                    if (resultMatch.X == 0 && resultMatch.Y == 0)
                    {
                        //拍照结果，给PLC拍照完成结果 发送“2”，NG
                        LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                        //窗口界面在（80，20）位置显示红色26号的“NG”
                        g_UCDisplayCamera.DispString("NG", 26, 80, 20, "red");
                        //在软件报警栏中，打印“Stage1，相机1MARK1未识别！”
                        ShowAlarm("Stage1，相机1MARK1未识别！");
                        //？？
                        return stateComprehensive_e;
                    }
                    //读取C21的旋转中心数据赋值给resultCalibRotate
                    resultCalibRotate = (ResultCalibRotate)htResult["C21"];//旋转中心结果类


                    #region 使用双目相机计算
                    //相机1处理完成
                    BlFinishPos_Cam1 = true;
                    //计算
                    CalDelta(1);
                    #endregion

                    #endregion
                }
                else
                {
                    #region 插拔模式点亮
                    #region 图像处理
                    BaseResultMatch baseResultMatch = (BaseResultMatch)htResult["C3"]; //匹配定位
                    ResultCrossLines resultCrossLines = htResult["C4"] as ResultCrossLines;   //抓取直线      
                    ResultCalibRotate resultCalibRotate = (ResultCalibRotate)htResult["C5"];//旋转中心结果类               
                    #endregion
                    //如果匹配定位异常、抓取直线异常、角度设定大于adj6设定偏差值，则执行
                    if (baseResultMatch.X * baseResultMatch.Y == 0 || resultCrossLines.X * resultCrossLines.Y == 0 ||
                        Math.Abs(resultCrossLines.IncludedR_J - 90) > ParAdjust.Value1("adj6"))
                    {
                        //显示“相机1位置1未匹配到产品,产品夹角超出设定偏差！”
                        ShowState("相机1位置1未匹配到产品,产品夹角超出设定偏差！");
                        //拍照结果，给PLC拍照完成结果，NG  为何用“+”？？是“清除相机触发信号，同时给PLC发送NG信号，寄存器写2”这个意思吗？    
                        LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);

                        //拍照NG后，结果为false，完成为true
                        BlResultPos1 = false;
                        BlFinishPos1 = true;
                        //不保存NG图
                        SaveImage = false;
                        //??
                        return StateComprehensive_enum.False;
                    }

                    //基准坐标
                    PCam1StdPos1 = new Point3D(resultCrossLines.StdX * Amp, resultCrossLines.StdY * Amp, resultCrossLines.StdR_J);
                    //拍照坐标 
                    PCam1WorkPos1 = new Point3D(resultCrossLines.X * Amp, resultCrossLines.Y * Amp, resultCrossLines.R_J);   
                    //旋转中心
                    PRotateCenterCam1 = new Point2D(resultCalibRotate.PointRC.DblValue1 * Amp, resultCalibRotate.PointRC.DblValue2 * Amp);
                    //拍照ok后，结果为true，完成为true
                    BlResultPos1 = true;
                    BlFinishPos1 = true;

                    //拍照完成信号  
                    LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 1);
                    #endregion
                }
                //匹配实际轴坐标之后的偏差值，发给PLC
                ////保存OK图
                SaveImage = true;
                return StateComprehensive_enum.True;
            }
            //？？
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                //显示拍照序号,结果，时间
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                //如果SaveImage为true则保存OK图，如果为false则保存NG图
                if (SaveImage)
                {
                    //if (ParSetWork.P_I[5].BlSelect)
                    g_DealComprehensiveBase.SaveOKImage("C1");
                }
                else
                {
                    g_DealComprehensiveBase.SaveNGImage("C1");
                }
                #endregion 显示和日志记录
            }
        }
        #endregion 位置1拍照

        #region 位置2拍照
        public override StateComprehensive_enum DealComprehensiveResultFun2(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            PosNow_e = Pos_enum.Pos2;//当前位置
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            bool SaveImage = false;
            #endregion 定义

            try
            {
                //相机像素精度
                double Amp = ParCalibWorld.V_I[1];
                StateComprehensive_enum stateComprehensive_e =
                    g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos2, out htResult);
                //如果运行模式中序号8不勾选为传统探针方式点亮，勾选为插拔模式点亮
                if (!ParSetWork.P_I[8].BlSelect)
                {
                    //定义一个resultMatch局部变量
                    BaseResult resultMatch = null;
                    //将C15算子结果赋值给resultMatch变量
                    resultMatch = (BaseResultMatch)htResult["C15"];
                    //将像素坐标转换成实际坐标  像素坐标*AMP
                    Mark1X = resultMatch.X * Amp;
                    Mark1Y = resultMatch.Y * Amp;
                    //基准坐标
                    StdMark1X = resultMatch.StdX * Amp;
                    StdMark1Y = resultMatch.StdY * Amp;
                    //如果X和Y坐标同时为0，则报错NG
                    if (resultMatch.X == 0 && resultMatch.Y == 0)
                    {
                        //拍照结果，给PLC拍照完成结果 发送“2”，NG
                        LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                        //窗口界面在（80，20）位置显示红色26号的“NG”
                        g_UCDisplayCamera.DispString("NG", 26, 80, 20, "red");
                        //在软件报警栏中，打印“Stage2，相机1MARK1未识别！”
                        ShowAlarm("Stage2，相机1MARK1未识别！");
                        //？？
                        return stateComprehensive_e;
                    }
                    //获取C21的旋转中心
                    resultCalibRotate = (ResultCalibRotate)htResult["C21"];//旋转中心结果类                  
                    #region 双目相机计算
                    //相机1处理完成
                    BlFinishPos_Cam1 = true;
                    //计算
                    CalDelta(2);
                    #endregion

                }
                else            //插拔模式
                {
                    #region 图像处理
                    BaseResultMatch baseResultMatch = (BaseResultMatch)htResult["C9"];            //匹配定位
                    ResultCrossLines resultCrossLines = htResult["C10"] as ResultCrossLines;      //直线交点
                    #endregion
                    //如果匹配定位异常、抓取直线交点异常、角度设定大于adj6设定偏差值，则执行
                    if (baseResultMatch.X * baseResultMatch.Y == 0 || resultCrossLines.X * resultCrossLines.Y == 0 ||
                        Math.Abs(resultCrossLines.IncludedR_J - 90) > ParAdjust.Value1("adj6"))
                    {
                        //显示“相机1位置1未匹配到产品,产品夹角超出设定偏差！”
                        ShowState("相机1位置2未匹配到产品,产品夹角超出设定偏差！！");
                        //拍照结果，给PLC拍照完成结果，NG  为何用“+”？？
                        LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);

                        BlResultPos2 = false;
                        BlFinishPos2 = true;
                        return StateComprehensive_enum.False;
                    }
                    //基准坐标
                    PCam1StdPos2 = new Point3D(resultCrossLines.StdX * Amp, resultCrossLines.StdY * Amp, resultCrossLines.StdR_J);
                    //拍照坐标
                    PCam1WorkPos2 = new Point3D(resultCrossLines.X * Amp, resultCrossLines.Y * Amp, resultCrossLines.R_J);

                    BlResultPos2 = true;
                    BlFinishPos2 = true;
                    //g_UCDisplayCamera.ShowResult("位置2基准坐标：" + PCam1StdPos2.ToString() + "\n工作坐标：" + PCam1WorkPos2.ToString());
                    //计算偏差并发送给PLC
                    if (CalDeltaProd1())
                    {

                    }
                    else
                    {
                        LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                        return StateComprehensive_enum.False;
                    }
                }
                //保存OK图片
                SaveImage = true;
                //？？
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
                if (SaveImage)
                {
                    //if (ParSetWork.P_I[5].BlSelect)
                    g_DealComprehensiveBase.SaveOKImage("C1");
                }
                else
                {
                    g_DealComprehensiveBase.SaveNGImage("C1");
                }
                #endregion 显示和日志记录
            }
        }
        #endregion 位置2拍照
        public bool DealCam1(int intStationNo, bool BlResult, double[] delta)
        {
            try
            {
                //如果计算完成，BlResult为true，则执行
                if (BlResult)
                {
                    //将计算结果打印到软件上显示
                    ShowState("Stage" + intStationNo.ToString() + "相机1- X: " + delta[0].ToString() + "," + "Y: " + delta[1].ToString() + "," + "R: " + Math.Round(delta[3], 2).ToString());
                    //拍照结果，给PLC拍照完成结果，OK  为何用“+”？？
                    LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 1);
                    //在窗口上显示绿色“OK”
                    g_UCDisplayCamera.DispString("OK", 26, 80, 20, "green");
                    //写入偏差值   写入相机寄存器11？？
                    LogicPLC.L_I.WriteCalData(g_regData_L, delta, g_regFinishData, 11);
                }
                //否则拍照结果NG
                else
                {
                    //拍照结果，给PLC拍照完成结果，NG
                    LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                    //软件报警窗口打印异常
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
            htResult = g_HtResult;
            BlResultPos3 = false;
            BlFinishPos3 = false;
            bool blResult = true;
            bool SaveImage = false;
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                double Amp = ParCalibWorld.V_I[1];
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
                    LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                    g_UCDisplayCamera.DispString("NG", 26, 80, 20, "red");
                    ShowAlarm("Stage2，相机1MARK1未识别！");
                    blResult = false;
                    return stateComprehensive_e;
                }
                PCam1StdPos3 = new Point2D(resultCrossLines.StdX * Amp, resultCrossLines.StdY * Amp);
                PCam1WorkPos3 = new Point2D(resultCrossLines.X * Amp, resultCrossLines.Y * Amp);
                //位置3旋转中心
                PRotateCenterCam3 = new Point2D(resultCalibRotate.PointRC.DblValue1 * Amp, resultCalibRotate.PointRC.DblValue2 * Amp);
                BlResultPos3 = true;
                BlFinishPos3 = true;
                LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 1);
                SaveImage = true;
                return StateComprehensive_enum.True;

            }
            catch (Exception ex)
            {
                ShowAlarm("相机1位置3处理异常，请查看异常信息日志！");
                LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                BlResultPos3 = false;
                BlFinishPos3 = true;
                blResult = false;
                Log.L_I.WriteError("BaseDealComprehensiveResult", ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos3, htResult, blResult, sw);
                #endregion 显示和日志记录
                if (SaveImage)
                {
                    //if (ParSetWork.P_I[5].BlSelect)
                    g_DealComprehensiveBase.SaveOKImage("C1");
                }
                else
                {
                    g_DealComprehensiveBase.SaveNGImage("C1");
                }
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
            htResult = g_HtResult;
            BlResultPos4 = false;
            BlFinishPos4 = false;
            bool blResult = true;
            bool SaveImage = false;
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                double Amp = ParCalibWorld.V_I[1];
                StateComprehensive_enum stateComprehensive_e =
                   g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos4, out htResult);
                #region 图像处理
                BaseResultMatch baseResultMatch = (BaseResultMatch)htResult["C21"];
                ResultCrossLines resultCrossLines = htResult["C22"] as ResultCrossLines;
                #endregion

                if (baseResultMatch.X * baseResultMatch.Y == 0 || resultCrossLines.X * resultCrossLines.Y == 0 ||
                    Math.Abs(resultCrossLines.IncludedR_J - 90) > ParAdjust.Value1("adj6"))
                {
                    ShowState("相机1位置4未匹配到产品,产品夹角超出设定偏差！");
                    LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                    BlResultPos4 = false;
                    BlFinishPos4 = true;
                    blResult = false;
                    SaveImage = false;
                    return StateComprehensive_enum.False;
                }

                PCam1StdPos4 = new Point2D(resultCrossLines.StdX * Amp, resultCrossLines.StdY * Amp);
                PCam1WorkPos4 = new Point2D(resultCrossLines.X * Amp, resultCrossLines.Y * Amp);
                BlResultPos4 = true;
                BlFinishPos4 = true;
                //计算偏差并发送给PLC
                if (CalDeltaProd2())
                {

                }
                else
                {
                    blResult = false;
                    LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                    return StateComprehensive_enum.False;
                }
                SaveImage = true;
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                ShowAlarm("相机1位置4处理异常，请查看异常信息日志！");
                LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, 2);
                BlResultPos4 = false;
                BlFinishPos4 = true;
                blResult = false;
                Log.L_I.WriteError("BaseDealComprehensiveResult", ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos4, htResult, blResult, sw);
                if (SaveImage)
                {
                    //if (ParSetWork.P_I[5].BlSelect)
                    g_DealComprehensiveBase.SaveOKImage("C1");
                }
                else
                {
                    g_DealComprehensiveBase.SaveNGImage("C1");
                }
                #endregion 显示和日志记录
            }
        }
        #endregion 位置4拍照
    }
}
