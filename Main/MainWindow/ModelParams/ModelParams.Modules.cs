
using BasicClass;
using ModulePackage;

namespace Main
{
    public partial class ModelParams
    {
        #region module
        #region 二维码qrcode
        /// <summary>
        /// 二维码处机器人角度
        /// </summary>
        public static double QrCodeAngle
        {
            get
            {
                return PreciseRobotAngle;
            }
        }
        /// <summary>
        /// 机器人二维码位置
        /// </summary>
        public static Point4D QrCodePos
        {
            get
            {
                Point2D delta = TransDelta(new Point2D(QrCodeX, QrCodeY), QrCodeAngle, PickAngle);
                return stdPosQrCode + new Point4D();
            }
        }
        /// <summary>
        /// 二维码X（以玻璃二维码角度情况下，左下角为原点）
        /// </summary>
        public static double QrCodeX
        {
            get
            {
                return confQrCodeX - confGlassX;
            }
        }
        /// <summary>
        /// 二维码Y（以玻璃二维码角度情况下，左下角为原点）
        /// </summary>
        public static double QrCodeY
        {
            get
            {
                return confQrCodeY - confGlassY;
            }
        }

        public static Point2D CodeComXY
        {
            get
            {
                Point2D com = new Point2D
                {
                    DblValue1 = (IfMirrior ? -1 : 1) * adjCodeCom.DblValue2,
                    DblValue2 = (IfMirrior ? -1 : 1) * adjCodeCom.DblValue1
                };
                return com;
            }
        }
        #endregion

        #region 残材wastage
        /// <summary>
        /// 机器人残材平台1放片位置
        /// </summary>
        public static Point4D PosWastagePlat1
        {
            get
            {
                return stdPosWastagePlat1 + adjPosWastagePlat1 +
                    new Point4D(WastageOffset.DblValue1, WastageOffset.DblValue2, -confGlassThicknes, BotWastageAngle);
            }
        }
        /// <summary>
        /// 机器人残材平台2放片位置
        /// </summary>
        public static Point4D PosWastagePlat2
        {
            get
            {
                return stdPosWastagePlat2 + adjPosWastagePlat2 +
                    new Point4D(WastageOffset.DblValue1, WastageOffset.DblValue2, -confGlassThicknes, BotWastageAngle);
            }
        }
        /// <summary>
        /// 残材平台处玻璃X
        /// </summary>
        public static double WastageX
        {
            get
            {
                return WastageAngle % 180 == 0 ? confGlassX : confGlassY;
            }
        }
        /// <summary>
        /// 计算残材平台处的缩进，根据机器人放置角度，以及在平台上的放片位置
        /// 按照操作面的坐标系x>y^来计算，先得到操作面下需要走的偏差，然后再转换
        /// </summary>
        public static Point2D WastageOffset
        {
            get
            {
                Point2D deviation = new Point2D((int)PlacePos < 2 ? WastageX / 2 : -WastageX / 2, //如果是left,则向右移动
                    ((int)PlacePos == 1 || (int)PlacePos == 2) ? WastageY / 2 : -WastageY / 2);//如果是bottom，则向上移动

                return TransDelta(deviation, 0, BotPlaceAngle);
            }
        }
        /// <summary>
        /// 残材平台处玻璃Y
        /// </summary>
        public static double WastageY
        {
            get
            {
                return WastageAngle % 180 == 0 ? confGlassY : confGlassX;
            }
        }
        /// <summary>
        /// 残材平台处机器人u轴角度
        /// </summary>
        public static double BotWastageAngle
        {
            get
            {
                double angle = (WastageAngle + PickAngle + 360) % 360;
                return angle > 180 ? 180 - angle : angle;
            }
        }
        /// <summary>
        /// 平台放片电极朝向，上左下右-0123，单电极时
        /// </summary>
        public static int[] PlatPlaceDir
        {
            get
            {
                //这边的做法是，把单电极朝向的方向，放在数组[0]，另一个放在数组[1]，这是为了单电极计算的时候直接计算第一位
                //假设是左上或者右下，左上朝向是（1，0），右下是（3，2）
                //假设是左下和右上，左下朝向是（1，2），右上是（3，0）
                if (horizontal)
                    return new int[2] {
                        (int)PlacePos % 2 == 0 ? ((int)PlacePos + 1) % 4 : (int)PlacePos,
                        (int)PlacePos % 2 == 0 ? (int)PlacePos : ((int)PlacePos + 1) % 4 };
                //如果是竖直方向，那与上面相反
                //左上（0，1），右下（2，3）
                //左下（2，1），右上（3，2）
                else
                    return new int[2] {
                        (int)PlacePos % 2 == 0 ? (int)PlacePos : ((int)PlacePos + 1) % 4,
                        (int)PlacePos % 2 == 0 ? ((int)PlacePos + 1) % 4 : (int)PlacePos };
            }
        }
        /// <summary>
        /// 玻璃在残材平台的角度(0123-上左下右)
        /// </summary>
        public static double WastageAngle
        {
            get
            {
                int i = 0;
                foreach (var item in ElectrodeArray)
                {
                    if (item > 0)
                        ++i;
                }
                if (i == 0 || i > 2)
                    return 0;

                WastageDetection.GetWastageAngle(ElectrodeArray, PlatPlaceDir, out double angle, IfSingleElectrode);

                if (confWastagePlatStation == 1)
                    return angle;
                else
                {
                    if (!IfSingleElectrode)
                        return (angle + 180) % 360;
                    else
                        return (angle - 90) % 360;//针对对角线翻转，如果不是对角线也不会有2工位
                }

            }
        }
        /// <summary>
        /// 残材检测阈值2
        /// </summary>
        public static double WastageThread1
        {
            get
            {
                return adjSharpnessThread1;
            }
        }

        /// <summary>
        /// 残材检测阈值2
        /// </summary>
        public static double WastageThread2
        {
            get
            {
                return adjSharpnessThread2;
            }
        }

        public static double[] ElectrodeArray
        {
            get
            {
                return new double[] { confTopElectrode, confLeftElectrode, confBottomElectrode, confRightElectrode };
            }
        }

        /// <summary>
        /// 是否单电极
        /// </summary>
        public static bool IfSingleElectrode
        {
            get
            {
                int cnt = 0;
                if (confTopElectrode != 0)
                    cnt++;
                if (confBottomElectrode != 0)
                    cnt++;
                if (confLeftElectrode != 0)
                    cnt++;
                if (confRightElectrode != 0)
                    cnt++;

                if (cnt == 1)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 残材平台处二维码X
        /// </summary>
        public static double CodeXInWastage
        {
            get
            {
                return GetCurMarkPos(new Point2D(confQrCodeX, confQrCodeY)).DblValue1;
            }
        }
        /// <summary>
        /// 残材平台处二维码Y
        /// </summary>
        public static double CodeYInWastage
        {
            get
            {
                return GetCurMarkPos(new Point2D(confQrCodeX, confQrCodeY)).DblValue2;
            }
        }
        /// <summary>
        /// 二维码在残材平台上，相对于玻璃中心的偏差
        /// </summary>
        public static Point2D CodeOffsetInWastage
        {
            get
            {
                return new Point2D(CodeXInWastage - confGlassX / 2,
                CodeYInWastage - confGlassY / 2);
            }
        }
        /// <summary>
        /// 残材平台处MarkX
        /// </summary>
        public static double MarkXInWastage
        {
            get
            {
                return GetCurMarkPos(new Point2D(confMarkX, confMarkY)).DblValue1;
            }
        }
        /// <summary>
        /// 残材平台处MarkY
        /// </summary>
        public static double MarkYInWastage
        {
            get
            {
                return 0;
            }
        }

        public static Point2D GetCurMarkPos(Point2D markpos)
        {
            return GetCurMarkPos(markpos, confGlassX, confGlassY, WastageAngle);
        }
        public static Point2D GetCurMarkPos(Point2D markpos, double width, double height, double r)
        {
            Point2D delta = new Point2D(markpos.DblValue1 - width / 2, markpos.DblValue2 - height / 2);
            Point2D size = new Point2D(r % 180 == 0 ? width : height, r % 180 == 0 ? height : width);

            delta = TransDelta(delta, r, 0);
            return new Point2D(delta.DblValue1 + size.DblValue1 / 2, delta.DblValue2 + size.DblValue2 / 2);
        }

        /// <summary>
        /// 根据所有电极宽度，玻璃旋转角度，重新获得当前角度下各电极的宽度
        /// </summary>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="left"></param>
        /// <param name="index"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static double GetCurElectordeWidth(int index)
        {
            return WastageDetection.GetCurElectordeWidth(ElectrodeArray, index, (int)WastageAngle);
        }
        #endregion

        #region 单目定位MonoCamera
        public static Point4D LocationPos1
        {
            get
            {
                return stdPosPrecise.Add(3, QrCodeAngle);
            }
        }

        public static Point4D LocationPos2
        {
            get
            {
                return stdPosPrecise.Add(1, confGlassX).Add(3, QrCodeAngle);
            }
        }

        public static double MonoAngle
        {
            get
            {
                return WastageAngle;
            }
        }

        public static double MonoGlassX
        {
            get
            {
                return MonoAngle % 180 == 0 ? confGlassX : confGlassY;
            }
        }

        public static double MonoGlassY
        {
            get
            {
                return MonoAngle % 180 == 0 ? confGlassY : confGlassX;
            }
        }
        #endregion

        #region 巡边insp
        /// <summary>
        /// 巡边平台1理论放片位置
        /// </summary>
        public static Point4D PosInspPlat1
        {
            get
            {
                return stdPosInspPlat1 + adjInspPosPlat1 + new Point4D(-InspY / 2, 0, confGlassThicknes, 0);
            }
        }
        /// <summary>
        /// 巡边平台2理论放片位置
        /// </summary>
        public static Point4D PosInspPlat2
        {
            get
            {
                return stdPosInspPlat2 + adjInspPosPlat2 + new Point4D(-InspY / 2, 0, confGlassThicknes, 0);
            }
        }
        /// <summary>
        /// 巡边平台处玻璃X
        /// </summary>
        public static double InspX
        {
            get
            {
                return AngleInsp % 180 == 0 ? confGlassX : confGlassY;
            }
        }
        /// <summary>
        /// 巡边平台处玻璃Y
        /// </summary>
        public static double InspY
        {
            get
            {
                return AngleInsp % 180 == 0 ? confGlassY : confGlassX;
            }
        }
        /// <summary>
        /// 巡边平台处玻璃角度
        /// </summary>
        public static double AngleInsp
        {
            get
            {
                double angle = 0;
                if (confInspDirection == 1)
                    angle = 0;
                else if (confInspDirection == 2)
                    angle = 180;
                else if (confInspDirection == 4)
                    angle = -90;
                else if (confInspDirection == 8)
                    angle = 90;
                else
                    angle = 0;
                return angle;
            }
        }

        public static double InspRobotAngle
        {
            get
            {
                return AngleInsp + PickAngle;
            }
        }
        #endregion

        #region 抛料dump
        /// <summary>
        /// 抛料处玻璃X
        /// </summary>
        public static double GlassXInDump
        {
            get
            {
                return DumpAngle % 180 == 0 ? confGlassX : confGlassY;
            }
        }
        /// <summary>
        /// 抛料处玻璃Y
        /// </summary>
        public static double GlassYInDump
        {
            get
            {
                return DumpAngle % 180 == 0 ? confGlassY : confGlassX;
            }
        }
        /// <summary>
        /// 机器人抛料角度
        /// </summary>
        public static double DumpAngle
        {
            get
            {
                return confBeltDirection + PickAngle;
            }
        }
        /// <summary>
        /// 抛料位置
        /// </summary>
        public static Point4D DumpPos
        {
            get
            {
                return stdDumpPos + adjDumpPos;
            }
        }
        #endregion

        #region 皮带线belt

        public static Point4D BeltPickPos
        {
            get
            {
                return stdBeltPickPos + adjBeltPickPos;
            }
        }
        /// <summary>
        /// 机器人皮带线角度
        /// </summary>
        public static double BeltRobotAngle
        {
            get
            {
                return BeltAngle + PickAngle;
            }
        }
        /// <summary>
        /// 玻璃皮带线角度
        /// </summary>
        public static double BeltAngle
        {
            get
            {
                if (confBeltDirection == 1)
                    return 0;
                else if (confBeltDirection == 2)
                    return 180;
                else if (confBeltDirection == 4)
                    return -90;
                else if (confBeltDirection == 8)
                    return 90;
                else
                    return 0;
            }
        }
        /// <summary>
        /// 皮带线处玻璃Y
        /// </summary>
        public static double BeltY
        {
            get
            {
                return BeltAngle % 180 == 0 ? confGlassY : confGlassX;
            }
        }
        /// <summary>
        /// 皮带线系数
        /// </summary>
        public static double BeltRatioY
        {
            get
            {
                return confGlassY / 6 + adjBeltRatio;
            }
        }

        public static double BeltRatioX
        {
            get
            {
                return confGlassX / 6 + adjBeltRatio;
            }
        }
        #endregion
        #endregion
    }
}
