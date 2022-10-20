using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using System.Threading;
using BasicClass;
using CalibDataManager;
using DealCalibrate;
using Main_EX;
using System;
using DealConfigFile;
using DealResult;


namespace Main
{
    /// <summary>
    /// 继承于Main_EX中基类
    /// </summary>
    public partial class BaseDealComprehensiveResult_Main : BaseDealComprehensiveResult
    {
        /// <summary>
        /// 是否只计算X
        /// </summary>
        public static bool BlOnlyX
        {
            get
            {
                return ParSetWork.P_I[1].BlSelect;
            }
        }

        #region 相机之间间距
        /// <summary>
        /// 相机1与相机2之间距离
        /// </summary>
        public static double DisMark
        {
            get
            {
                return ParAdjust.Value1("adj7");
            }
            set
            {
                ParAdjust.SetValue1("adj7", value);
            }
        }

        public static double DisCamera34
        {
            get
            {
                return ParAdjust.Value2("adj1");
            }
            set
            {
                ParAdjust.SetValue2("adj1", value);
            }
        }

        /// <summary>
        /// 计算间距时的拍照位置X坐标
        /// </summary>
        public static double Cam1PhotoPosX = 0;
        public static double Cam2PhotoPosX = 0;
        public static double Cam3PhotoPosX = 0;
        public static double Cam4PhotoPosX = 0;

        /// <summary>
        /// 计算间距时相机的Mark坐标的X坐标
        /// </summary>
        public static double Cam1MarkPosX = 0;
        public static double Cam2MarkPosX = 0;
        public static double Cam3MarkPosX = 0;
        public static double Cam4MarkPosX = 0;
        #endregion 相机之间间距

        /// <summary>
        /// 旋转中心，以相机1为参考
        /// </summary>
        public static Point2D PRotateCenterCam1 { get; set; }

        public static Point2D PRotateCenterCam2_stage1 { get; set; }

        public static Point2D PRotateCenterCam2_stage2 { get; set; }

        /// <summary>
        /// 放料部分旋转中心，以相机3为参考
        /// </summary>
        public static Point2D PRotateCenterCam3 { get; set; }

        public static double C1k { get { if (ParSetWork.P_I[11].BlSelect) { return -1; } else { return 1; } } }
        public static double C2k { get { if (ParSetWork.P_I[12].BlSelect) { return -1; } else { return 1; } } }


        #region 角度计算
        #region 相机1
        /// <summary>
        /// 位置1，2基准的角度
        /// </summary>
        public static double Prod1StdAngle
        {
            get
            {
                return Math.Asin((PCam1StdPos2.DblValue2 - PCam1StdPos1.DblValue2) / DisMark);  //mark之间的间距
                //return 0;
            }
        }

        /// <summary>
        /// 位置3，4基准的角度
        /// </summary>
        public static double Prod2StdAngle
        {
            get
            {
                return Math.Asin((PCam1StdPos4.DblValue2 - PCam1StdPos3.DblValue2) / DisMark); //mark之间的间距
            }
        }

        /// <summary>
        /// 位置1，2工作角度
        /// </summary>
        public static double Prod1WorkAngle
        {
            get
            {
                return Math.Asin((PCam1WorkPos2.DblValue2 - PCam1WorkPos1.DblValue2) / DisMark);
                //return -Math.PI * ((PCam1WorkPos1.DblValue3 - PCam1StdPos1.DblValue3) + (PCam1WorkPos2.DblValue3 - PCam1StdPos2.DblValue3)) / 360;
            }
        }

        /// <summary>
        /// 位置3，4工作角度
        /// </summary>
        public static double Prod2WorkAngle
        {
            get
            {
                return Math.Asin((PCam1WorkPos4.DblValue2 - PCam1WorkPos3.DblValue2) / DisMark);
            }
        }

        /// <summary>
        /// 位置1,2相机角度偏差-走到产品位置来抓取，顺时针为+，逆时针为-
        /// </summary>
        public static double Prod1DeltaAngle
        {
            get
            {
                return Prod1StdAngle - Prod1WorkAngle;
            }
        }


        /// <summary>
        /// 3，4位置角度偏差
        /// </summary>
        public static double Prod2DeltaAngle
        {
            get
            {
                return Prod2StdAngle - Prod2WorkAngle;
            }
        }
        #endregion

        #region 相机2
        /// <summary>
        /// 位置5，6基准的角度
        /// </summary>
        public static double Prod3StdAngle
        {
            get
            {
                return Math.Asin((PCam2StdPos2.DblValue2 - PCam2StdPos1.DblValue2) / DisMark);  //mark之间的间距
                //return 0;
            }
        }

        /// <summary>
        /// 位置7，8基准的角度
        /// </summary>
        public static double Prod4StdAngle
        {
            get
            {
                return Math.Asin((PCam2StdPos4.DblValue2 - PCam2StdPos3.DblValue2) / DisMark); //mark之间的间距
            }
        }

        /// <summary>
        /// 位置1，2工作角度
        /// </summary>
        public static double Prod3WorkAngle
        {
            get
            {
                return Math.Asin((PCam2WorkPos2.DblValue2 - PCam2WorkPos1.DblValue2) / DisMark);
            }
        }

        /// <summary>
        /// 位置3，4工作角度
        /// </summary>
        public static double Prod4WorkAngle
        {
            get
            {
                return Math.Asin((PCam2WorkPos4.DblValue2 - PCam2WorkPos3.DblValue2) / DisMark);
            }
        }

        /// <summary>
        /// 位置1,2相机角度偏差-走到产品位置来抓取，顺时针为+，逆时针为-
        /// </summary>
        public static double Prod3DeltaAngle
        {
            get
            {
                return Prod3StdAngle - Prod3WorkAngle;
            }
        }


        /// <summary>
        /// 3，4位置角度偏差
        /// </summary>
        public static double Prod4DeltaAngle
        {
            get
            {
                return Prod4StdAngle - Prod4WorkAngle;
            }
        }
        #endregion
        #endregion

        #region  Mark坐标
        /// <summary>
        /// 相机基准位坐标以及工作坐标
        /// </summary>
        #region 相机1
        public static Point3D PCam1StdPos1 = new Point3D();
        public static Point3D PCam1WorkPos1 = new Point3D();

        public static Point3D PCam1StdPos2 = new Point3D();
        public static Point3D PCam1WorkPos2 = new Point3D();

        public static Point2D PCam1StdPos3 = new Point2D();
        public static Point2D PCam1WorkPos3 = new Point2D();

        public static Point2D PCam1StdPos4 = new Point2D();
        public static Point2D PCam1WorkPos4 = new Point2D();

        public static Point3D PCam1StdPos5 = new Point3D();
        public static Point3D PCam1WorkPos5 = new Point3D();

        public static Point3D PCam1StdPos6 = new Point3D();
        public static Point3D PCam1WorkPos6 = new Point3D();

        public static Point2D PCam1StdPos7 = new Point2D();
        public static Point2D PCam1WorkPos7 = new Point2D();

        public static Point2D PCam1StdPos8 = new Point2D();
        public static Point2D PCam1WorkPos8 = new Point2D();
        #endregion

        #region 相机2
        public static Point3D PCam2StdPos1 = new Point3D();
        public static Point3D PCam2WorkPos1 = new Point3D();

        public static Point3D PCam2StdPos2 = new Point3D();
        public static Point3D PCam2WorkPos2 = new Point3D();

        public static Point2D PCam2StdPos3 = new Point2D();
        public static Point2D PCam2WorkPos3 = new Point2D();

        public static Point2D PCam2StdPos4 = new Point2D();
        public static Point2D PCam2WorkPos4 = new Point2D();

        #endregion
        #endregion

        #region 算法单元格
        /// <summary>
        /// 相机拍照算法单元格
        /// </summary>
        public const string CellPhoto1 = "C1";
        public const string CellPhoto2 = "C10";

        /// <summary>
        /// 形状匹配算子单元格
        /// </summary>
        public const string CellMatch1 = "C3";
        public const string CellMatch2 = "C12";

        /// <summary>
        /// M直线算法单元格
        /// </summary>
        public const string CellMCrossLine1 = "C5";
        public const string CellMCrossLine2 = "C14";
        public const string CellMLine1 = "C5";
        public const string CellMLine2 = "C14";

        /// <summary>
        /// 旋转中心单元格(配置的时候再确定是哪一个单元格）
        /// </summary>
        public const string CellRC = "C13";
        #endregion

        /// <summary>
        /// 计算并发送偏差值-相机1-stage1
        /// </summary>
        public bool CalDeltaProd1()
        {
            try
            {
                if (BlFinishPos1 && BlFinishPos2)
                {
                    BlFinishPos1 = BlFinishPos2 = false;
                    if (!(BlResultPos1 && BlResultPos2))
                        return false;
                }
                ShowState("位置1基准位置：" + PCam1StdPos1);
                ShowState("位置1工作位置：" + PCam1WorkPos1);
                ShowState("位置2基准位置：" + PCam1StdPos2);
                ShowState("位置2工作位置：" + PCam1WorkPos2);
                if (BlOnlyX)
                {
                    double dX = ((PCam1WorkPos1.DblValue1 + PCam1WorkPos2.DblValue1) -
                                 (PCam1StdPos1.DblValue1 + PCam1StdPos2.DblValue1)) / 2;
                    double rX = dX + ParAdjust.Value1("adj8");
                    ShowState("相机1 stage1偏差值：" + rX.ToString("f3"));
                    if (Math.Abs(rX) > ParAdjust.Value1("adj10"))
                    {
                        ShowState("工位1偏差超过设定阈值！");
                        return false;
                    }
                    DealComprehensiveResult1.D_I.DealCam1(1, true, new double[4] { rX, 0, 0, 0 });
                    return true;
                }

                //计算旋转之后的位置
                Point2D pInitAfterR = new FunCalibRotate().GetPoint_AfterRotation(-(Prod1DeltaAngle + ParAdjust.Value3("adj8") * Math.PI / 180), PRotateCenterCam1, new Point2D(PCam1StdPos1.DblValue1, PCam1StdPos1.DblValue2));
                //偏差值 + 工位1补偿值
                double deltaX = Math.Round(PCam1WorkPos1.DblValue1 - pInitAfterR.DblValue1, 3);
                double deltaY = Math.Round(PCam1WorkPos1.DblValue2 - pInitAfterR.DblValue2, 3);
                double deltaR = -Math.Round(Prod1DeltaAngle * 180 / Math.PI, 3);    //注意角度旋转方向
                ShowState("位置1，2视觉偏差值:X=" + deltaX.ToString("f3") + ",Y=" + deltaY.ToString("f3") + ",R=" + deltaR.ToString("f3"));
                double RobotX = -deltaX + ParAdjust.Value1("adj8");
                double RobotY = -deltaY + ParAdjust.Value2("adj8");
                double RobotR = deltaR + ParAdjust.Value3("adj8");
                ShowState("位置1，2偏差值:X=" + RobotX.ToString("f3") + ",Y=" + RobotY.ToString("f3") + ",R=" + RobotR.ToString("f3"));
                //偏差值判断
                if (Math.Abs(RobotX) > ParAdjust.Value1("adj10") || Math.Abs(RobotY) > ParAdjust.Value2("adj10") || Math.Abs(RobotR) > ParAdjust.Value3("adj10"))
                {
                    ShowState("工位11偏差超过设定阈值！");
                    return false;
                }
                DealComprehensiveResult1.D_I.DealCam1(1, true, new double[4] { C1k*RobotX, RobotY, 0, deltaR });
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 计算并发送偏差值-stage2
        /// </summary>
        public bool CalDeltaProd2()
        {
            try
            {
                if (BlFinishPos3 && BlFinishPos4)
                {
                    BlFinishPos3 = BlFinishPos4 = false;
                    if (!(BlResultPos3 && BlResultPos4))
                        return false;
                }

                ShowState("相机1位置3基准位置：" + PCam1StdPos3);
                ShowState("相机1位置3工作位置：" + PCam1WorkPos3);
                ShowState("相机1位置4基准位置：" + PCam1StdPos4);
                ShowState("相机1位置4工作位置：" + PCam1WorkPos4);

                if (BlOnlyX)
                {
                    double dX = ((PCam1WorkPos3.DblValue1 + PCam1WorkPos4.DblValue1) -
                                 (PCam1StdPos3.DblValue1 + PCam1StdPos4.DblValue1)) / 2;
                    double rX = dX + ParAdjust.Value1("adj9");
                    ShowState("位置3，4偏差值：" + rX.ToString("f3"));
                    if (Math.Abs(rX) > ParAdjust.Value1("adj10"))
                    {
                        ShowState("工位12偏差超过设定阈值！");
                        return false;
                    }
                    DealComprehensiveResult1.D_I.DealCam1(2, true, (new double[4] { rX, 0, 0, 0 }));
                    return true;
                }

                ///计算旋转之后的位置
                Point2D pInitAfterR = new FunCalibRotate().GetPoint_AfterRotation(-(Prod2DeltaAngle + ParAdjust.Value3("adj9") * Math.PI / 180), PRotateCenterCam3, new Point2D(PCam1StdPos3.DblValue1, PCam1StdPos3.DblValue2));

                ///偏差值要确认方向 
                double deltaX = Math.Round(PCam1WorkPos3.DblValue1 - pInitAfterR.DblValue1, 3);
                double deltaY = Math.Round(PCam1WorkPos3.DblValue2 - pInitAfterR.DblValue2, 3);
                double deltaR = -Math.Round(Prod2DeltaAngle * 180 / Math.PI, 3);
                ShowState("位置3，4视觉偏差值:X=" + deltaX.ToString("f3") + ",Y=" + deltaY.ToString("f3") + ",R=" + deltaR.ToString("f3"));
                double RobotX = -deltaX + ParAdjust.Value1("adj9");
                double RobotY = -deltaY + ParAdjust.Value2("adj9");
                double RobotR = deltaR + ParAdjust.Value3("adj9");
                ShowState("位置3，4偏差值:X=" + RobotX.ToString("f3") + ",Y=" + RobotY.ToString("f3") + ",R=" + RobotR.ToString("f3"));
                if (Math.Abs(RobotX) > ParAdjust.Value1("adj10") || Math.Abs(RobotY) > ParAdjust.Value2("adj10") || Math.Abs(RobotR) > ParAdjust.Value3("adj10"))
                {
                    ShowState("工位2偏差超过设定阈值！");
                    return false;
                }
                DealComprehensiveResult1.D_I.DealCam1(2, true, (new double[4] { C1k*deltaX, deltaY, 0, deltaR }));
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }


        /// <summary>
        /// 计算并发送偏差值-相机2 - stage1
        /// </summary>
        public bool CalDeltaProd3()
        {
            try
            {
                if (BlFinishPos1_Cam2 && BlFinishPos2_Cam2)
                {
                    BlFinishPos1_Cam2 = BlFinishPos2_Cam2 = false;
                    if (!(BlResultPos1_Cam2 && BlResultPos2_Cam2))
                        return false;
                }
                ShowState("相机2 stage1位置1：" + PCam2StdPos1);
                ShowState("相机2位置1工作位置：" + PCam2WorkPos1);
                ShowState("相机2位置2基准位置：" + PCam2StdPos2);
                ShowState("相机2位置2工作位置：" + PCam2WorkPos2);
                if (BlOnlyX)
                {
                    double dX = ((PCam2WorkPos1.DblValue1 + PCam2WorkPos2.DblValue1) -
                                 (PCam2StdPos1.DblValue1 + PCam2StdPos2.DblValue1)) / 2;
                    double rX = dX + ParAdjust.Value1("adj11");
                    ShowState("相机2 stage1偏差值：" + rX.ToString("f3"));
                    if (Math.Abs(rX) > ParAdjust.Value1("adj18"))
                    {
                        ShowState("工位1偏差超过设定阈值！");
                        return false;
                    }
                    DealComprehensiveResult2.D_I.DealCam2(1, true, new double[4] { rX, 0, 0, 0 });
                    return true;
                }
                //计算旋转之后的位置
                Point2D pInitAfterR = new FunCalibRotate().GetPoint_AfterRotation(-(Prod3DeltaAngle + ParAdjust.Value3("adj11") * Math.PI / 180), PRotateCenterCam2_stage1, new Point2D(PCam2StdPos1.DblValue1, PCam2StdPos1.DblValue2));
                //偏差值 + 工位1补偿值
                double deltaX = Math.Round(PCam2WorkPos1.DblValue1 - pInitAfterR.DblValue1, 3);
                double deltaY = Math.Round(PCam2WorkPos1.DblValue2 - pInitAfterR.DblValue2, 3);
                double deltaR = -Math.Round(Prod3DeltaAngle * 180 / Math.PI, 3);    //注意角度旋转方向
                ShowState("位置5，6视觉偏差值:X=" + deltaX.ToString("f3") + ",Y=" + deltaY.ToString("f3") + ",R=" + deltaR.ToString("f3"));
                double RobotX = deltaX + ParAdjust.Value1("adj11");
                double RobotY = -deltaY + ParAdjust.Value2("adj11");
                double RobotR = deltaR + ParAdjust.Value3("adj11");
                ShowState("位置5，6偏差值:X=" + RobotX.ToString("f3") + ",Y=" + RobotY.ToString("f3") + ",R=" + RobotR.ToString("f3"));
                //偏差值判断
                if (Math.Abs(RobotX) > ParAdjust.Value1("adj18") || Math.Abs(RobotY) > ParAdjust.Value2("adj18") || Math.Abs(RobotR) > ParAdjust.Value3("adj18"))
                {
                    ShowState("工位21偏差超过设定阈值！");
                    return false;
                }
                DealComprehensiveResult2.D_I.DealCam2(1, true, new double[4] { C2k*RobotX, RobotY, 0, deltaR });
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 计算并发送偏差值-相机2-stage2
        /// </summary>
        public bool CalDeltaProd4()
        {
            try
            {
                if (BlFinishPos3_Cam2 && BlFinishPos4_Cam2)
                {
                    BlFinishPos3_Cam2 = BlFinishPos4_Cam2 = false;
                    if (!(BlResultPos3_Cam2 && BlResultPos4_Cam2))
                        return false;
                }

                ShowState("相机2位置3基准位置：" + PCam2StdPos3);
                ShowState("相机2位置3工作位置：" + PCam2WorkPos3);
                ShowState("相机2位置4基准位置：" + PCam2StdPos4);
                ShowState("相机2位置4工作位置：" + PCam2WorkPos4);

                if (BlOnlyX)
                {
                    double dX = ((PCam2WorkPos3.DblValue1 + PCam2WorkPos4.DblValue1) -
                                 (PCam2StdPos3.DblValue1 + PCam2StdPos4.DblValue1)) / 2;
                    double rX = dX + ParAdjust.Value1("adj12");
                    ShowState("相机2 stage2偏差值：" + rX.ToString("f3"));
                    if (Math.Abs(rX) > ParAdjust.Value1("adj18"))
                    {
                        ShowState("工位22偏差超过设定阈值！");
                        return false;
                    }
                    DealComprehensiveResult2.D_I.DealCam2(2, true, new double[4] { rX, 0, 0, 0 });
                    return true;
                }
                ///计算旋转之后的位置
                Point2D pInitAfterR = new FunCalibRotate().GetPoint_AfterRotation(-(Prod4DeltaAngle + ParAdjust.Value3("adj12") * Math.PI / 180), PRotateCenterCam2_stage2, new Point2D(PCam2StdPos3.DblValue1, PCam2StdPos3.DblValue2));

                ///偏差值要确认方向 
                double deltaX = Math.Round(PCam2WorkPos3.DblValue1 - pInitAfterR.DblValue1, 3);
                double deltaY = Math.Round(PCam2WorkPos3.DblValue2 - pInitAfterR.DblValue2, 3);
                double deltaR = -Math.Round(Prod4DeltaAngle * 180 / Math.PI, 3);
                ShowState("位置7，8视觉偏差值:X=" + deltaX.ToString("f3") + ",Y=" + deltaY.ToString("f3") + ",R=" + deltaR.ToString("f3"));
                double RobotX = deltaX + ParAdjust.Value1("adj12");
                double RobotY = deltaY + ParAdjust.Value2("adj12");
                double RobotR = deltaR + ParAdjust.Value3("adj12");
                ShowState("位置7，8偏差值:X=" + RobotX.ToString("f3") + ",Y=" + RobotY.ToString("f3") + ",R=" + RobotR.ToString("f3"));
                if (Math.Abs(RobotX) > ParAdjust.Value1("adj18") || Math.Abs(RobotY) > ParAdjust.Value2("adj18") || Math.Abs(RobotR) > ParAdjust.Value3("adj18"))
                {
                    ShowState("工位2偏差超过设定阈值！");
                    return false;
                }
                DealComprehensiveResult2.D_I.DealCam2(2, true, new double[4] { C2k*RobotX, RobotY, 0, deltaR });
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }


        /// <summary>
        /// 写入偏差数据
        /// </summary>
        /// <param name="data"></param>
        public static void WriteDataProd1(double[] data)
        {
            LogicPLC.L_I.WriteRegData1(5, 4, data);  //工位1拍照完成信号
        }
        /// <summary>
        /// 写入相机3，4偏差数据
        /// </summary>
        /// <param name="data"></param>
        public static void WriteDataProd2(double[] data)
        {
            LogicPLC.L_I.WriteRegData1(10, 4, data); //工位2拍照完成信号
        }
    }
}
