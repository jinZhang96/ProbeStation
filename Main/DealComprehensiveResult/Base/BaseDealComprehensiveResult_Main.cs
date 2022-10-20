using BasicClass;
using CalibDataManager;
using DealCalibrate;
using Main_EX;
using System;
using DealConfigFile;
using System.Threading;
using DealPLC;
using DealResult;

namespace Main
{
    /// <summary>
    /// 继承于Main_EX中基类
    /// </summary>
    public partial class BaseDealComprehensiveResult_Main : BaseDealComprehensiveResult
    {
        #region 变量定义            
        //粗定位空跑用
        static Random rd = new Random();
        int glassnum = rd.Next(1, 10);
        int cnt = 0;

        const string strRotateCalibCell1 = @"C10";
        const string strRotateCalibCell2 = @"C16";
        public Point2D[] pt2Calib = new Point2D[2] { new Point2D(), new Point2D() };

        public static Point2D[] pt2Mark1 = new Point2D[2] { new Point2D(), new Point2D() };
        public static Point2D[] pt2Mark2 = new Point2D[2] { new Point2D(), new Point2D() };
        public static Point2D[] pt2MarkArray = new Point2D[10] { new Point2D(), new Point2D(),
            new Point2D(), new Point2D(), new Point2D(), new Point2D(), new Point2D(), new Point2D(),new Point2D(),new Point2D() };
        public static double[] calcResultInsp = new double[3];
        public static double[] calcResultInsert = new double[3];

        public static Action StopBelt;
        public static Action RefreshStatistics = null;
        /// <summary>
        /// 放大系数 --YF
        /// </summary>
        public double AMP { get => ParCalibWorld.V_I[g_NoCamera]; }

        public const string ReservDigits = "f3";
        /// <summary>
        /// 理论上的旋转中心 --YF
        /// </summary>
        public Point2D PRCCam5
        {
            get
            {
                Point4D pStd = CalibDataMngr.instance.CalibPos_L[0];
                Point2D pRCGeneral = new Point2D(pStd.DblValue1, pStd.DblValue2) + new Point2D(ModelParams.WastageX / 2 / AMP, -ModelParams.WastageY / 2 / AMP);
                return pRCGeneral;
            }
        }
        #endregion

        #region 算子序号定义
        public string strCamera1Template1 = @"C2T";
        public string strCamera2Template1 = @"C2";
        public string strCamera3Template1 = @"C2";
        public string strCamera4Template1 = @"C2";
        public string strCamera1Template2 = @"C12";
        public string strCamera2Template2 = @"C12";
        public string strCamera3Template2 = @"C8";
        public string strCamera4Template2 = @"C12";
        public string strCamera3Template3 = @"C14";

        public string strCamera1Match1 = @"C2";//
        public string strCamera2Match1 = @"C2";//
        public string strCamera3Match1 = @"C4";//
        public string strCamera4Match1 = @"C4";//
        public string strCamera1Match2 = @"C14";
        public string strCamera2Match2 = @"C14";
        public string strCamera3Match2 = @"C12";
        public string strCamera4Match2 = @"C12";
        public string strCamera3Match3 = @"C10";

        public string strCamera5Match1 = @"C4";//
        public string strCamera5Match2 = @"C12";
        public string strCamera5Match3 = @"C2";

        public const string strCamera1RC = @"C8";//
        public const string strCamera5RC = @"C16";//

        public string strCamera6Match1 = @"C2";//
        public string strCamera6Match2 = @"C12";
        public string strCamera6Match3 = @"C10";

        public const string strCamera6RC = @"C16";//

        public string strCamera7Match1 = @"C2";//
        public string strCamera7Match2 = @"C2";
        public string strCamera7Match3 = @"C2";

        public string strCamera8Match1 = @"C2";//
        public string strCamera8Template1 = @"C2T";
        #endregion

        //如果运行模式序号10勾选，则CQ2K为-1，取反。即勾选10，探针组1和2的补偿值X取反
        public static double C12k { get { if (ParSetWork.P_I[10].BlSelect) { return -1; } else { return 1; } } }
        #region 相机1和2双目处理完成
        //？？
        Mutex g_MtPreceise = new System.Threading.Mutex();
        /// <summary>
        /// 相机1和2双目处理完成，计算
        /// </summary>
        public bool CalDelta(int intStageNo)
        {
            //定义数组变量 存放偏差值
            double[] dblDelta1 = new double[4];
            double[] dblDelta2 = new double[4];
            bool blResult = false;
            //？？
            g_MtPreceise.WaitOne();
            try
            {
                //如果相机1和2都处理完成了，开始计算
                if (BlFinishPos_Cam1 && BlFinishPos_Cam2)
                {
                    //将相机1和2处理完成初始化为false
                    BlFinishPos_Cam1 = false;
                    BlFinishPos_Cam2 = false;
                    //实际工作角度=（相机2的MARK点Y实际值-相机1的MARK点Y实际值）/调整值adj7中输入的两MARK的间距
                    //tan（角度）=角度？？
                    //“DealComprehensiveResult2.D_I.Mark1Y”是“相机2的MARK点Y实际值”？？
                    double AngleRadWork = (DealComprehensiveResult2.D_I.Mark1Y - DealComprehensiveResult1.D_I.Mark1Y) / ParAdjust.Value1("adj7");//实际角度
                    //基准角度=（相机2的MARK点Y基准值-相机1的MARK点Y基准值）/调整值adj7中输入的两MARK的间距？？
                    double AngleRadStd = (DealComprehensiveResult2.D_I.StdMark1Y - DealComprehensiveResult1.D_I.StdMark1Y) / ParAdjust.Value1("adj7");//标准值的角度
                    //角度偏差=实际工作角度-基准角度
                    double dblDeltaR = AngleRadWork - AngleRadStd;

                    //如果运行模式勾选2，则是探针组1补正
                    if (ParSetWork.P_I[2].BlSelect)//探针组1补正
                    {
                        //定义两个变量，为手动补偿值
                        double dblComX = 0, dblComY = 0;
                        //定义两个变量，探针基准值和相机基准值
                        Point2D TanzhenStd = new Point2D();
                        Point2D CamStd = new Point2D();
                        //如果当前平台为Stage1则执行
                        if (intStageNo == 1)
                        {
                            //将调整值“adj1”位置的X和Y对应赋值给两个变量，实现手动加补偿值
                            dblComX = ParAdjust.Value1("adj1");
                            dblComY = ParAdjust.Value2("adj1");

                            //将从寄存器保留读值1和寄存器保留读值2获取的值分别赋值给探针基准值的数据1和数据2
                            TanzhenStd.DblValue1 = LogicPLC.L_I.RegReserveData1;
                            TanzhenStd.DblValue2 = LogicPLC.L_I.RegReserveData2;
                            //将从寄存器保留读值3和寄存器保留读值4获取的值分别赋值给相机基准值的数据1和数据2
                            CamStd.DblValue1 = LogicPLC.L_I.RegReserveData3;
                            CamStd.DblValue2 = LogicPLC.L_I.RegReserveData4;

                        }
                        //如果当前平台为Stage2则执行
                        else if (intStageNo == 2)
                        {
                            //将调整值“adj2”位置的X和Y对应赋值给两个变量，实现手动加补偿值
                            dblComX = ParAdjust.Value1("adj2");
                            dblComY = ParAdjust.Value2("adj2");

                            //将从寄存器保留读值5和寄存器保留读值6获取的值分别赋值给探针基准值的数据1和数据2
                            TanzhenStd.DblValue1 = LogicPLC.L_I.RegReserveData5;
                            TanzhenStd.DblValue2 = LogicPLC.L_I.RegReserveData6;
                            //将从寄存器保留读值7和寄存器保留读值8获取的值分别赋值给相机基准值的数据1和数据2
                            CamStd.DblValue1 = LogicPLC.L_I.RegReserveData7;
                            CamStd.DblValue2 = LogicPLC.L_I.RegReserveData8;
                        }
                        //打印在软件上显示：
                        //“探针组1工位X探针基准：”
                        ShowState("探针组1工位" + intStageNo + "探针基准：" + TanzhenStd.ToString());
                        //“探针组1工位X拍照基准：”
                        ShowState("探针组1工位" + intStageNo + "拍照基准：" + CamStd.ToString());


                        //计算标准Mark转动dblDeltaR以后的坐标
                        Point2D WorkMarkAfterR = null;
                        ResultCalibRotate resultCalibRotate = DealComprehensiveResult1.D_I.resultCalibRotate;//旋转中心结果类
                        FunCalibRotate funCalibRotate = new FunCalibRotate();
                        Point2D currentRC = new Point2D();
                        double Amp = ParCalibWorld.V_I[1];

                        if (ParSetWork.P_I[14].BlSelect)
                        {
                            if (ParSetWork.P_I[13].BlSelect)
                            {
                                if (dblDeltaR >= 0)
                                {
                                    currentRC.DblValue1 = ParAdjust.Value1("adj23") * Amp;
                                    currentRC.DblValue2 = ParAdjust.Value2("adj23") * Amp;
                                }
                                else
                                {
                                    currentRC.DblValue1 = ParAdjust.Value1("adj21") * Amp;
                                    currentRC.DblValue2 = ParAdjust.Value2("adj21") * Amp;
                                }
                            }
                            else
                            {
                                currentRC.DblValue1 = ParAdjust.Value1("adj23") * Amp;
                                currentRC.DblValue2 = ParAdjust.Value2("adj23") * Amp;
                            }

                        }
                        else
                        {
                            //旋转中心换算，乘上Amp
                            currentRC.DblValue1 = resultCalibRotate.PointRC.DblValue1 * Amp - (TanzhenStd.DblValue1 - CamStd.DblValue1);//需要加上拍照位置和探针下压位置的轴坐标差！注意这里的方向！！
                            currentRC.DblValue2 = resultCalibRotate.PointRC.DblValue2 * Amp + (TanzhenStd.DblValue2 - CamStd.DblValue2);//需要加上拍照位置和探针下压位置的轴坐标差！注意这里的方向！！
                        }


                        //获取旋转角度的新坐标   （旋转角度，旋转中心，旋转前坐标）
                        WorkMarkAfterR = funCalibRotate.GetPoint_AfterRotation(dblDeltaR, currentRC, new Point2D(DealComprehensiveResult1.D_I.Mark1X, DealComprehensiveResult1.D_I.Mark1Y));

                        //与基准的差值，计算补偿值    旋转后的新坐标-基准坐标
                        double dblDeltaX = WorkMarkAfterR.DblValue1 - DealComprehensiveResult1.D_I.StdMark1X;
                        double dblDeltaY = WorkMarkAfterR.DblValue2 - DealComprehensiveResult1.D_I.StdMark1Y;

                        //将双精度浮点值舍入到指定数量的小数位，并将中点值舍入到最接近的偶数
                        //将计算的值放到dblDelta1这个数组中
                        dblDelta1[0] = C12k * Math.Round(-dblDeltaX + dblComX, 3);
                        dblDelta1[1] = Math.Round(dblDeltaY + dblComY, 3);
                        dblDelta1[2] = 0;
                        dblDelta1[3] = Math.Round(dblDeltaR / Math.PI * 180, 3);

                    }

                    //如同探针组1
                    // //如果运行模式勾选3，则是启用探针组2补正
                    if (ParSetWork.P_I[3].BlSelect)//探针组2补正
                    {
                        double dblComX = 0, dblComY = 0;
                        Point2D TanzhenStd = new Point2D();
                        Point2D CamStd = new Point2D();
                        if (intStageNo == 1)
                        {
                            dblComX = ParAdjust.Value1("adj3");
                            dblComY = ParAdjust.Value2("adj3");

                            TanzhenStd.DblValue1 = LogicPLC.L_I.RegReserveData9;
                            TanzhenStd.DblValue2 = LogicPLC.L_I.RegReserveData10;
                            CamStd.DblValue1 = LogicPLC.L_I.RegReserveData11;
                            CamStd.DblValue2 = LogicPLC.L_I.RegReserveData12;
                        }
                        else if (intStageNo == 2)
                        {
                            dblComX = ParAdjust.Value1("adj4");
                            dblComY = ParAdjust.Value2("adj4");

                            TanzhenStd.DblValue1 = LogicPLC.L_I.RegReserveData13;
                            TanzhenStd.DblValue2 = LogicPLC.L_I.RegReserveData14;
                            CamStd.DblValue1 = LogicPLC.L_I.RegReserveData15;
                            CamStd.DblValue2 = LogicPLC.L_I.RegReserveData16;
                        }
                        ShowState("探针组2工位" + intStageNo + "探针基准：" + TanzhenStd.ToString());
                        ShowState("探针组2工位" + intStageNo + "拍照基准：" + CamStd.ToString());


                        //计算标准Mark转动dblDeltaR以后的坐标
                        Point2D WorkMarkAfterR = null;
                        ResultCalibRotate resultCalibRotate = DealComprehensiveResult2.D_I.resultCalibRotate;//旋转中心结果类
                        FunCalibRotate funCalibRotate = new FunCalibRotate();
                        Point2D currentRC = new Point2D();
                        double Amp = ParCalibWorld.V_I[2];
                        if (ParSetWork.P_I[14].BlSelect)
                        {
                            if (ParSetWork.P_I[13].BlSelect)
                            {
                                if (dblDeltaR >= 0)
                                {
                                    currentRC.DblValue1 = ParAdjust.Value1("adj24") * Amp;
                                    currentRC.DblValue2 = ParAdjust.Value2("adj24") * Amp;
                                }
                                else
                                {
                                    currentRC.DblValue1 = ParAdjust.Value1("adj22") * Amp;
                                    currentRC.DblValue2 = ParAdjust.Value2("adj22") * Amp;
                                }
                            }
                            else
                            {
                                currentRC.DblValue1 = ParAdjust.Value1("adj24") * Amp;
                                currentRC.DblValue2 = ParAdjust.Value2("adj24") * Amp;
                            }

                        }
                        else
                        {
                            currentRC.DblValue1 = resultCalibRotate.PointRC.DblValue1 * Amp + (TanzhenStd.DblValue1 - CamStd.DblValue1);//需要加上拍照位置和探针下压位置的轴坐标差！注意这里的方向！！
                            currentRC.DblValue2 = resultCalibRotate.PointRC.DblValue2 * Amp + (TanzhenStd.DblValue2 - CamStd.DblValue2);//需要加上拍照位置和探针下压位置的轴坐标差！注意这里的方向！！
                        }

                        WorkMarkAfterR = funCalibRotate.GetPoint_AfterRotation(dblDeltaR, currentRC, new Point2D(DealComprehensiveResult2.D_I.Mark1X, DealComprehensiveResult2.D_I.Mark1Y));

                        double dblDeltaX = WorkMarkAfterR.DblValue1 - DealComprehensiveResult2.D_I.StdMark1X;
                        double dblDeltaY = WorkMarkAfterR.DblValue2 - DealComprehensiveResult2.D_I.StdMark1Y;

                        dblDelta2[0] = C12k * Math.Round(dblDeltaX + dblComX, 3);
                        dblDelta2[1] = Math.Round(dblDeltaY + dblComY, 3);
                        dblDelta2[2] = 0;
                        dblDelta2[3] = Math.Round(-dblDeltaR / Math.PI * 180, 3);
                    }
                    blResult = true;
                    //调用DealCam1方法，显示并写入结果
                    DealComprehensiveResult1.D_I.DealCam1(intStageNo, blResult, dblDelta1);
                    DealComprehensiveResult2.D_I.DealCam2(intStageNo, blResult, dblDelta2);

                }
                return true;
            }


            //??
            catch (Exception ex)
            {
                DealComprehensiveResult2.D_I.DealCam2(intStageNo, false, dblDelta2);
                DealComprehensiveResult1.D_I.DealCam1(intStageNo, false, dblDelta1);
                g_MtPreceise.ReleaseMutex();
                Log.L_I.WriteError("BaseDealComprehensiveResult", ex);
                return false;
            }
            finally
            {
                g_MtPreceise.ReleaseMutex();
            }
        }
        #endregion 相机1和2双目处理完成
    }

    public enum PtType_Mono
    {
        AutoMark1,//自动流程Mark1
        AutoMark2,//自动流程Mark2
        CalibMark1,//标定Mark1
        CalibMark2,//标定Mark2
        Verify1,//验证角度补正Mark1
        Verify2,//验证角度补正Mark2
        R180Mark1,//反转180度后Mark1
        R180Mark2,//反转180度后Mark2
        RcMark1,//旋转中心Mark1
        RcMark2//旋转中心Mark2
    }
}
