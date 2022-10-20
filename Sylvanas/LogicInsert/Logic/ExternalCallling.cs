using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using BasicClass;
using DealComprehensive;
using DealResult;
using DealCalibrate;
using DealFile;
using System.IO;
using DealPLC;

namespace DealInsert
{
    public partial class BaseDealInsert
    {

        #region event
        public event StrAction ShowState_event;//需要在main中进行注册
        public event StrAction ShowAlarm_event;
        #endregion

        #region 读取本地记录
        public void Init_Insert()
        {
            try
            {
                ReadStdInsertData();
                InsertWindow.ReadInsertData();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #region 相机拍照调用
        public StateComprehensive_enum DealCSTDetect(
            TriggerSource_enum trigerSource_e, int pos, int g_NoCamera, string g_regClearCamera, string g_regFinishPhoto, Hashtable htResult)
        {
            switch (pos)
            {
                case 1:
                    return RecordTopKeel(trigerSource_e, pos, g_NoCamera, g_regClearCamera, g_regFinishPhoto, htResult);
                case 2:
                    return RecordBottomKeel(trigerSource_e, pos, g_NoCamera, g_regClearCamera, g_regFinishPhoto, htResult);
                case 3:
                    return CalcLeftKeel(trigerSource_e, pos, g_NoCamera, g_regClearCamera, g_regFinishPhoto, htResult);
                case 4:
                    return CalcRightKeel(trigerSource_e, pos, g_NoCamera, g_regClearCamera, g_regFinishPhoto, htResult);
                default:
                    return StateComprehensive_enum.False;
            }
        }
        #endregion

        #region 反馈插篮请求
        public void SendInsertData(int CstID, int FobNum, double tempCom)
        {
            try
            {
                ShowState_event("即将发送插栏坐标");
                int RealInsertRow = ParInsertRecipe.P_I.confCSTRow - 2 * FobNum;
                int numInsert = ParInsertRegData.P_I.CurInsertNum;
                int intCol = numInsert / RealInsertRow;
                int intRow = numInsert % RealInsertRow + FobNum;

                double visionOffset = 0;
                visionOffset = (LeftKeelOffset + RightKeelOffset) / 2;
                bool Valid = false;

                if (intRow == 0)
                {
                    ShowState_event("插篮进行换列,清空左右偏差数据。");
                    LeftKeelOffset = 0;
                    RightKeelOffset = 0;
                }

                if (Math.Abs(LeftKeelOffset) < 0.00001 || Math.Abs(RightKeelOffset) < 0.00001)
                {
                    Valid = false;
                }
                else
                {
                    Valid = true;
                }

                ShowState_event(string.Format("已插篮数目:{0},即将插入第{1}片", numInsert, numInsert + 1));
                ShowState_event(string.Format("插入位置为第{0}列，第{1}行", intCol + 1, intRow + 1));

                double curdx = 0;
                double fixdx = 0;
                double stdx = 0;

                switch (CstID)
                {
                    case 1:
                        stdx = CST1PosAll_L[intCol][intRow].DblValue1;
                        curdx = CST1DeltaX_L[intCol][intRow];
                        fixdx = ParInsertRecipe.P_I.CST1ComX;
                        break;
                    case 2:
                        stdx = CST2PosAll_L[intCol][intRow].DblValue1;
                        curdx = CST2DeltaX_L[intCol][intRow];
                        fixdx = ParInsertRecipe.P_I.CST2ComX;
                        break;
                    case 3:
                        stdx = CST3PosAll_L[intCol][intRow].DblValue1;
                        curdx = CST3DeltaX_L[intCol][intRow];
                        fixdx = ParInsertRecipe.P_I.CST3ComX;
                        break;
                    case 4:
                        stdx = CST4PosAll_L[intCol][intRow].DblValue1;
                        curdx = CST4DeltaX_L[intCol][intRow];
                        fixdx = ParInsertRecipe.P_I.CST4ComX;
                        break;
                    default:
                        break;
                }

                if (Valid == true)
                {
                    ShowState_event("偏移数值" + Math.Abs(visionOffset - curdx));
                    ShowState_event("门槛阈值" + OffsetGate);
                    if (Math.Abs(visionOffset - curdx) < OffsetGate)
                    {
                        ShowState_event(string.Format("视觉偏差_{0},理论偏差{1},使用视觉偏差!", visionOffset, curdx));
                        curdx = visionOffset;
                    }
                    else
                    {
                        ShowState_event(string.Format("视觉偏差_{0},理论偏差{1},使用理论偏差!", visionOffset, curdx));
                    }
                }
                else
                {
                    ShowState_event("尚未获取有效视觉偏差，等待插篮拍照后更新!");
                }

                double DataX = stdx + curdx + fixdx + tempCom;

                ShowState_event(string.Format("理论基准值{0},相机补正值{1},固定补偿值{2}", stdx, curdx, fixdx));

                ParInsertRegData.P_I.SendConfirmInsertX(DataX);

                ShowState_event("已发送插栏坐标X：" + DataX.ToString("f3"));
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

    }
}
