using BasicClass;
using DealConfigFile;


namespace Main
{
    public partial class ModelParams
    {
        #region packing
        #region config

        /// <summary>
        /// 配方-玻璃X
        /// </summary>
        public static double confGlassX
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.GlassX].DblValue;
            }
        }
        /// <summary>
        /// 配方-玻璃Y
        /// </summary>
        public static double confGlassY
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.GlassY].DblValue;
            }
        }
        /// <summary>
        /// 配方-二维码X
        /// </summary>
        public static double confQrCodeX
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.QrCodeX].DblValue;
            }
        }
        /// <summary>
        /// 配方-二维码Y
        /// </summary>
        public static double confQrCodeY
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.QrCodeY].DblValue;
            }
        }
        /// <summary>
        /// 配方-上电极宽度
        /// </summary>
        public static double confTopElectrode
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.TopE].DblValue;
            }
        }
        /// <summary>
        /// 配方-下电极宽度
        /// </summary>
        public static double confBottomElectrode
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.BottomE].DblValue;
            }
        }
        /// <summary>
        /// 配方-左电极宽度
        /// </summary>
        public static double confLeftElectrode
        {
            get
            {             
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.LeftE].DblValue;
            }
        }
        /// <summary>
        /// 配方-右电极宽度
        /// </summary>
        public static double confRightElectrode
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.RightE].DblValue;
            }
        }
        /// <summary>
        /// 配方-玻璃厚度
        /// </summary>
        public static double confGlassThicknes
        {
            get
            {                
                return 0;//ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.Thickness].DblValue;
            }
        }
        /// <summary>
        /// 配方-插栏方向
        /// </summary>
        public static double confInsertDirection
        {
            get
            {                
                return ParConfigPar.P_I[(int)RecipeRegister.DIR_Insert].DblValue;
            }
        }
        /// <summary>
        /// 配方-残材平台放片方向
        /// </summary>
        public static double confWastageDirection
        {
            get
            {
                return 0;// ParConfigPar.P_I[key_conf_WastageDirection].DblValue;
            }
        }
        /// <summary>
        /// 配方-残材平台放片工位号
        /// </summary>
        public static double confWastagePlatStation
        {
            get
            {
                return ParConfigPar.P_I[(int)RecipeRegister.PlatStaionNo].DblValue;
            }
        }
        /// <summary>
        /// 配方-龙骨列数
        /// </summary>
        public static int KeelCol
        {
            get
            {
                return confCSTCol + 1;
            }
        }
        /// <summary>
        /// 配方-插栏列数
        /// </summary>
        public static int confCSTCol
        {
            get
            {                
                return (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.CSTCols].DblValue;
            }
        }
        /// <summary>
        /// 配方-插栏行数
        /// </summary>
        public static int confCSTRow
        {
            get
            {                
                return (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.CSTRows].DblValue;
            }
        }
        /// <summary>
        /// 配方-龙骨间距
        /// </summary>
        public static double confKeelInterval
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.KeelInterval].DblValue;
            }
        }
        /// <summary>
        /// 配方-第一列龙骨位置
        /// </summary>
        public static double confCol1Interval
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DisCol1].DblValue;
            }
        }
        /// <summary>
        /// 配方-巡边放片方向
        /// </summary>
        public static double confInspDirection
        {
            get
            {
                return 0;//ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.InspDir].DblValue;
            }
        }
        /// <summary>
        /// 配方-皮带线放片方向
        /// </summary>
        public static double confBeltDirection
        {
            get
            {                
                //此处以抛料方向作为皮带线方向
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DIR_Dump].DblValue;
            }
        }
        /// <summary>
        /// 配方-龙骨层间距
        /// </summary>
        public static double confLayerSpacing
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.KeelSpacing].DblValue;
            }
        }
        /// <summary>
        /// 配方-中片取片总数
        /// </summary>
        public static double confPickSum
        {
            get
            {                
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.SUMPick].DblValue;
            }
        }
        /// <summary>
        /// 配方-Mark间距
        /// </summary>
        public static double confDisMark
        {
            get
            {
                return 0;
                //return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DisMark].DblValue;
            }
        }

        public static double confMarkX
        {
            get
            {
                return 0;// ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.MarkX].DblValue;                
            }
        }

        public static double confMarkY
        {
            get
            {
                return 0;// ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.MarkY].DblValue;
            }
        }

        /// <summary>
        /// 配方-卡塞首末行不插数量,1代表首行和末行均不插
        /// </summary>
        public static int confFobFirstLastNum
        {
            get
            {
                return 0;
                //int num = (int)ParConfigPar.P_I[(int)RecipeRegister.FobCSTFirstLast].DblValue;
                ////对首末行不插的数量进行判断
                //if (num > -1 && num < 10)
                //{
                //    return num;
                //}
                //else
                //{
                //    return 1;
                //}
            }
        }

        public static int confPickStationNum
        {
            get
            {
                return 2;
                //return (int)ParConfigPar.P_I[(int)RecipeRegister.PickStationNum].DblValue;
            }
        }
        #endregion

        #region setwork
        /// <summary>
        /// 运行设定模式-是否记录中间数据
        /// </summary>
        public static bool IfRecordData
        {
            get
            {
                return ParSetWork.P_I.WorkSelect_L[(int)RunningMode.RecordData].BlSelect;
            }
        }

        /// <summary>
        /// 标定时是否矫正角度
        /// </summary>
        public static bool IfAdjustAngle
        {
            get
            {
                return true;// ParSetWork.P_I.WorkSelect_L[4].BlSelect;
            }
        }

        /// <summary>
        /// 屏蔽相机3
        /// </summary>
        public static bool IfPassCamera3
        {
            get
            {
                return ParSetWork.P_I.WorkSelect_L[(int)RunningMode.PassCamera3].BlSelect;
            }
        }

        /// <summary>
        /// 屏蔽相机4
        /// </summary>
        public static bool IfPassCamera4
        {
            get
            {
                return ParSetWork.P_I.WorkSelect_L[(int)RunningMode.PassCamera4].BlSelect;
            }
        }

        /// <summary>
        /// 屏蔽相机4
        /// </summary>
        public static bool IfPassCamera5
        {
            get
            {
                return ParSetWork.P_I.WorkSelect_L[(int)RunningMode.PassCamera5].BlSelect;
            }
        }

        /// <summary>
        /// 是否镜像机，L为非镜像
        /// </summary>
        public static bool IfMirrior
        {
            get
            {
                return ParSetWork.P_I.WorkSelect_L[(int)RunningMode.Mirror].BlSelect;
            }
        }

        public static bool IfUseRealRC
        {
            get
            {
                return ParSetWork.P_I.WorkSelect_L[(int)RunningMode.UseRealRC].BlSelect;
            }
        }
        #endregion

        #region adj
        /// <summary>
        /// 调整值-巡边平台1调整值xyzr
        /// </summary>
        static Point4D adjInspPosPlat1
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.Platform1];
            }
        }
        /// <summary>
        /// 调整值-巡边平台2调整值xyzr
        /// </summary>
        static Point4D adjInspPosPlat2
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.Platform2];
            }
        }
        
        /// <summary>
        /// 调整值-清晰度阈值
        /// </summary>
        static double adjSharpnessThread1
        {
            get
            {               
                string key = key_adj_SharpnessRatio;
                return ParAdjust.Value1(key);
            }
        }

        /// <summary>
        /// 调整值-清晰度阈值
        /// </summary>
        static double adjSharpnessThread2
        {
            get
            {
                string key = key_adj_SharpnessRatio;
                return ParAdjust.Value2(key);
            }
        }
        /// <summary>
        /// 调整值-残边平台1补偿Xyzr
        /// </summary>
        static Point4D adjPosWastagePlat1
        {
            get
            {
                return UnifyAdj(ParBotAdj.P_I[(int)BotAdj.Platform1]);
            }
        }
        /// <summary>
        /// 调整值-残边平台1补偿Xyzr
        /// </summary>
        static Point4D adjPosWastagePlat2
        {
            get
            {
                return UnifyAdj(ParBotAdj.P_I[(int)BotAdj.Platform2]);
            }
        }
        
        /// <summary>
        /// 调整值-抛料位补偿
        /// </summary>
        static Point4D adjDumpPos
        {
            get
            {
                return UnifyAdj(ParBotAdj.P_I[(int)BotAdj.DumpPos]);
            }
        }

        /// <summary>
        /// 调整至-皮带线取片补偿
        /// </summary>
        static Point4D adjBeltPickPos
        {
            get
            {
                return UnifyAdj(ParBotAdj.P_I[(int)BotAdj.BeltPickPos]);
            }
        }

        public static Point3D adjInspCom
        {
            get
            {
                string key = key_adj_insp;
                return new Point3D(ParAdjust.Value1(key_adj_insp), ParAdjust.Value2(key), ParAdjust.Value3(key));
            }
        }

        static double adjBeltRatio
        {
            get
            {
                string key = key_adj_BeltRatio;
                return ParAdjust.Value1(key);
            }
        }

        static Point2D adjCodeCom
        {
            get
            {
                string key = key_adj_CodeCom;
                return new Point2D(ParAdjust.Value1(key), ParAdjust.Value2(key));
            }
        }
        #endregion

        #region std
        /// <summary>
        /// 基准值-巡边平台1基准位
        /// </summary>
        static Point4D stdPosInspPlat1
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.Platform1] + new Point4D(0, 0, 0, InspRobotAngle);
            }
        }
        /// <summary>
        /// 基准值-巡边平台2基准位
        /// </summary>
        static Point4D stdPosInspPlat2
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.Platform2] + new Point4D(0, 0, 0, InspRobotAngle);
            }
        }
        
        /// <summary>
        /// 基准值-二维码基准位
        /// </summary>
        static Point4D stdPosQrCode
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.QrCodePos];
            }
        }
        
        /// <summary>
        /// 基准值-残材平台1基准位
        /// </summary>
        static Point4D stdPosWastagePlat1
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.Platform1];
            }
        }
        /// <summary>
        /// 基准值-残材平台2基准位
        /// </summary>
        static Point4D stdPosWastagePlat2
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.Platform2];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static Point2D stdPlatHeightCom
        {
            get
            {
                string key = key_std_PlatHeightCom;
                return new Point2D(ParStd.Value1(key), ParStd.Value2(key));
            }
        }
        /// <summary>
        /// 基准值-旋转中心标定角度
        /// </summary>
        static double stdRCCalibAngle
        {
            get
            {
                string key = key_std_RCCalibAngle;
                return ParStd.Value1(key);
            }
        }
        /// <summary>
        /// 基准值-基准抛料位置
        /// </summary>
        static Point4D stdDumpPos
        {
            get
            {
                 return ParBotStd.P_I[(int)BotStd.DumpPos];
            }
        }

        /// <summary>
        /// 基准值-皮带线取片基准
        /// </summary>
        static Point4D stdBeltPickPos
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.BeltPickPos];
            }
        }

        public static Point3D stdInspTransCom
        {
            get
            {
                string key = key_std_InspTrans;
                return new Point3D(ParStd.Value1(key), ParStd.Value2(key), ParStd.Value3(key));
            }
        }

        public static int stdRobotAutoSpeed
        {
            get
            {
                string key = key_std_RobotSpeed;
                return (int)ParStd.Value1(key);
            }
        }

        public static int stdRobotLowSpeed
        {
            get
            {
                string key = key_std_RobotSpeed;
                return (int)ParStd.Value2(key);
            }
        }

        public static int stdRobotResetSpeed
        {
            get
            {
                string key = key_std_RobotSpeed;
                return (int)ParStd.Value3(key);
            }
        }
        #endregion
        #endregion  
    }
}
