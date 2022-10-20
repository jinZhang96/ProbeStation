using BasicClass;
using DealConfigFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Main
{
    partial class ModelParams
    {
        #region 背光定位Precise
        /// <summary>
        /// 产品实际面积
        /// </summary>
        public static double ProductArea
        {
            get
            {
                return confGlassX * confGlassY;
            }
        }
        /// <summary>
        /// 机器人精定位位置
        /// </summary>
        public static Point4D PrecisePos
        {
            get
            {
                return stdPosPrecise + adjPrecisePos;
            }
        }
        /// <summary>
        /// 精定位处机器人u轴角度
        /// </summary>
        public static double PreciseRobotAngle
        {
            get
            {
                return 0;
            }
        }
        /// <summary>
        /// 精定位处玻璃角度
        /// </summary>
        public static double GlassAngleInPrecise
        {
            get
            {
                return PreciseRobotAngle - PickAngle;
            }
        }
        /// <summary>
        /// 精定位面积比例阈值Max
        /// </summary>
        public static double AreaMax
        {
            get
            {
                return adjAreaRatioMax == 0 ? 1.02 : adjAreaRatioMax;
            }
        }
        /// <summary>
        /// 精定位面积比例阈值Min
        /// </summary>
        public static double AreaMin
        {
            get
            {
                return adjAreaRatioMin == 0 ? 0.98 : adjAreaRatioMin;
            }
        }
        /// <summary>
        /// 旋转中心标定角度
        /// </summary>
        public static double RCCalibAngle
        {
            get
            {
                //避免人为改动导致和plc不同，也没必要传
                return -1;//stdRCCalibAngle;
            }
        }
        /// <summary>
        /// 精定位处玻璃Y
        /// </summary>
        public static double GlassYInPrecise
        {
            get
            {
                return GlassAngleInPrecise % 180 == 0 ? confGlassY : confGlassX;
            }
        }
        /// <summary>
        /// 精定位处玻璃X
        /// </summary>
        public static double GlassXInPrecise
        {
            get
            {
                return GlassAngleInPrecise % 180 == 0 ? confGlassX : confGlassY;
            }
        }
        /// <summary>
        /// 精定位处偏差X阈值
        /// </summary>
        public static double PreciseThreadX
        {
            get
            {
                return adjPreciseThreadX == 0 ? 10 : adjPreciseThreadX;
            }
        }
        /// <summary>
        /// 精定位处偏差Y阈值
        /// </summary>
        public static double PreciseThreadY
        {
            get
            {
                return adjPreciseThreadY == 0 ? 10 : adjPreciseThreadY;
            }
        }
        /// <summary>
        /// 精定位处偏差R阈值
        /// </summary>
        public static double PreciseThreadR
        {
            get
            {
                return adjPreciseThreadR == 0 ? 5 : adjPreciseThreadR;
            }
        }

        public static Point2D PreciseStdValue
        {
            get
            {
                return new Point2D(ParStd.Value1(key_std_PreciseStdValue), ParStd.Value2(key_std_PreciseStdValue));
            }
            set
            {
                ParStd.SetValue1(key_std_PreciseStdValue, value.DblValue1);
                ParStd.SetValue2(key_std_PreciseStdValue, value.DblValue2);
            }
        }
        #endregion

        #region adj
        /// <summary>
        /// 调整值-面积比例阈值Max
        /// </summary>
        static double adjAreaRatioMax
        {
            get
            {
                string key = key_adj_AreaRatio;
                return ParAdjust.Value1(key);
            }
        }
        /// <summary>
        /// 调整值-面积比例阈值Min
        /// </summary>
        static double adjAreaRatioMin
        {
            get
            {
                string key = key_adj_AreaRatio;
                return ParAdjust.Value2(key);
            }
        }
        /// <summary>
        /// 精定位偏差X阈值
        /// </summary>
        static double adjPreciseThreadX
        {
            get
            {
                string key = key_adj_precisethread;
                return ParAdjust.Value1(key);
            }
        }
        /// <summary>
        /// 精定位偏差Y阈值
        /// </summary>
        static double adjPreciseThreadY
        {
            get
            {
                string key = key_adj_precisethread;
                return ParAdjust.Value2(key);
            }
        }
        /// <summary>
        /// 精定位偏差R阈值
        /// </summary>
        static double adjPreciseThreadR
        {
            get
            {
                string key = key_adj_precisethread;
                return ParAdjust.Value3(key);
            }
        }
        
        /// <summary>
        /// 调整值-精定位补偿
        /// </summary>
        static Point4D adjPrecisePos
        {
            get
            {
                return UnifyAdj(ParBotAdj.P_I[(int)BotAdj.PrecisePos]);
            }
        }
        #endregion

        #region std
        /// <summary>
        /// 基准值-精定位基准位
        /// </summary>
        static Point4D stdPosPrecise
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.PrecisePos];
            }
        }
        #endregion
    }
}
