using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using System.IO;
using DealFile;

namespace DealInsert
{
    public partial class BaseDealInsert
    {
        #region 宗旨
        //本部分类的宗旨是能对部分机台直接套用本代码或者是通过配置来间接使用
        //本代码暂时只支持两个卡塞的插篮方案，并且两个卡塞为互锁关系
        //本代码是1.0版本，属于开荒探索版本
        #endregion

        #region 静态类实例
        public static BaseDealInsert B_I = new BaseDealInsert();

        #endregion 静态类实例

        #region Path

        public static string c_PathCSTBase
        {
            get
            {
                string str = IniFile.I_I.ReadIniStr("PathRoot", "root", @"D:\Store\ConfigFile\InsertData.ini");
                return str;
            }
            set
            {
                IniFile.I_I.WriteIni("PathRoot", "root", value, @"D:\Store\ConfigFile\InsertData.ini");
            }
        }

        private static string c_PathCSTDeltaIniCate = c_PathCSTBase + "CSTDelta";
        private static string c_PathCSTIniCate = c_PathCSTBase + "CST";
        private static string c_PathCSTZIntervalIniCate = c_PathCSTBase + "CSTZInterval";
        private static string c_PathCSTZIniCate = c_PathCSTBase + "CSTZ";
        private string c_PathCSTDeltaIni
        {
            get
            {
                string str = c_PathCSTDeltaIniCate + ParInsertRegData.P_I.CurStationNo + ".ini";
                return str;
            }
        }
        private string c_PathCSTZIntervalIni
        {
            get
            {
                string str = c_PathCSTZIntervalIniCate + ParInsertRegData.P_I.CurStationNo + ".ini";
                return str;
            }
        }
        private string c_PathCSTIni
        {
            get
            {
                string str = c_PathCSTIniCate + ParInsertRegData.P_I.CurStationNo + ".ini";
                return str;
            }
        }
        private string c_PathCSTZIni
        {
            get
            {
                string str = c_PathCSTZIniCate + ParInsertRegData.P_I.CurStationNo + ".ini";
                return str;
            }
        }
        #endregion Path

        #region 拍照计数
        public static int iTopKeelCnt = 0;
        public static int iBottomKeelCnt = 0;
        #endregion

        #region 链表定义

        private static List<List<Point2D>> CST1PosAll_L = new List<List<Point2D>>();
        private static List<List<Point2D>> CST2PosAll_L = new List<List<Point2D>>();
        private static List<List<Point2D>> CST3PosAll_L = new List<List<Point2D>>();
        private static List<List<Point2D>> CST4PosAll_L = new List<List<Point2D>>();

        private static List<List<double>> DeltaX_L = new List<List<double>>();
        private static List<List<double>> CST1DeltaX_L = new List<List<double>>();
        private static List<List<double>> CST2DeltaX_L = new List<List<double>>();
        private static List<List<double>> CST3DeltaX_L = new List<List<double>>();
        private static List<List<double>> CST4DeltaX_L = new List<List<double>>();

        private List<Point2D> TopKeelPos_L = new List<Point2D>();
        private List<Point2D> BottomKeelPos_L = new List<Point2D>();
        private List<List<double>> DeltaXFirstLine_L = new List<List<double>>();
        private List<List<double>> DeltaXBaseOnFirstLine_L = new List<List<double>>();
        private List<List<double>> KeelInterval_L = new List<List<double>>();
        private List<double> DeltaZ = new List<double>();
        private List<double> StepZ_L = new List<double>();

        #endregion

        #region 其他

        private string strCellMatch = "C2";
        private const string strLeftKeelOffset = "LeftKeelOffset";
        private const string strRightKeelOffset = "RightKeelOffset";
        private const string strKeelIntervalGate = "KeelIntervalGate";
        private const string strStepZIntervalGate = "StepZIntervalGate";
        private const string strXComprehensiveNo = "XComprehensiveNo";
        private const string strOffsetGate = "OffsetGate";
        private const string strSection_Gate = "Gate";
        private const string strSection_No = "No";
        private const string strPathParInsert = @"D:\Store\ConfigFile\ParParInsert.ini";

        public double LeftKeelOffset
        {
            set
            {
                RegeditFile.R_I.WriteRegedit(strLeftKeelOffset, value.ToString());
            }
            get
            {
                return RegeditFile.R_I.ReadRegeditDbl(strLeftKeelOffset);
            }
        }

        public double RightKeelOffset
        {
            set
            {
                RegeditFile.R_I.WriteRegedit(strRightKeelOffset, value.ToString());
            }
            get
            {
                return RegeditFile.R_I.ReadRegeditDbl(strRightKeelOffset);
            }
        }

        public double KeelIntervalGate
        {
            get
            {
                if (IniFile.I_I.ReadIniDbl(strSection_Gate, strKeelIntervalGate, strPathParInsert) < 0.00001)
                {
                    IniFile.I_I.WriteIni(strSection_Gate, strKeelIntervalGate, 5, strPathParInsert);
                    return 5;
                }
                else
                {
                    return IniFile.I_I.ReadIniDbl(strSection_Gate, strKeelIntervalGate, strPathParInsert);
                }
            }
        }

        public double StepZIntervalGate
        {
            get
            {
                if (IniFile.I_I.ReadIniDbl(strSection_Gate, strStepZIntervalGate, strPathParInsert) < 0.00001)
                {
                    IniFile.I_I.WriteIni(strSection_Gate, strStepZIntervalGate, 1, strPathParInsert);
                    return 1;
                }
                else
                {
                    return IniFile.I_I.ReadIniDbl(strSection_Gate, strStepZIntervalGate, strPathParInsert);
                }
            }
        }

        public double OffsetGate
        {
            get
            {
                if (IniFile.I_I.ReadIniDbl(strSection_Gate, strOffsetGate, strPathParInsert) < 0.00001)
                {
                    IniFile.I_I.WriteIni(strSection_Gate, strOffsetGate, 1, strPathParInsert);
                    return 1;
                }
                else
                {
                    return IniFile.I_I.ReadIniDbl(strSection_Gate, strOffsetGate, strPathParInsert);
                }
            }
        }

        public int XComprehensive
        {
            get
            {
                if (IniFile.I_I.ReadIniInt(strSection_No, strXComprehensiveNo, strPathParInsert) < 1)
                {
                    IniFile.I_I.WriteIni(strSection_No, strXComprehensiveNo, 11, strPathParInsert);
                    return 11;
                }
                else
                {
                    return IniFile.I_I.ReadIniInt(strSection_No, strXComprehensiveNo, strPathParInsert);
                }
            }
        }


        #endregion
    }
}
