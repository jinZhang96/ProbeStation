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
using System.Threading;

namespace DealInsert
{
    public partial class BaseDealInsert
    {
        #region 宗旨
        //此类存储所有计算卡塞的函数
        #endregion

        #region 从Ini读取卡塞插篮数据
        private void ReadStdInsertData()
        {
            try
            {
                #region 清空卡塞基准插篮数据链表
                CST1PosAll_L.Clear();
                CST2PosAll_L.Clear();
                CST3PosAll_L.Clear();
                CST4PosAll_L.Clear();
                #endregion

                #region 清空卡塞偏差插篮数据链表
                CST1DeltaX_L.Clear();
                CST2DeltaX_L.Clear();
                CST3DeltaX_L.Clear();
                CST4DeltaX_L.Clear();
                #endregion

                #region 将卡塞基准插篮数据读入链表
                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol; i++)
                {
                    List<Point2D> p_L = new List<Point2D>();
                    ReadP2ListIni("Col" + i.ToString(), "", c_PathCSTIniCate + "1.ini", out p_L);
                    CST1PosAll_L.Add(p_L);
                }

                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol; i++)
                {
                    List<Point2D> p_L = new List<Point2D>();
                    ReadP2ListIni("Col" + i.ToString(), "", c_PathCSTIniCate + "2.ini", out p_L);
                    CST2PosAll_L.Add(p_L);
                }

                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol; i++)
                {
                    List<Point2D> p_L = new List<Point2D>();
                    ReadP2ListIni("Col" + i.ToString(), "", c_PathCSTIniCate + "3.ini", out p_L);
                    CST3PosAll_L.Add(p_L);
                }

                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol; i++)
                {
                    List<Point2D> p_L = new List<Point2D>();
                    ReadP2ListIni("Col" + i.ToString(), "", c_PathCSTIniCate + "4.ini", out p_L);
                    CST4PosAll_L.Add(p_L);
                }
                #endregion

                #region 将卡塞偏差插篮数据读入链表
                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol; i++)
                {
                    List<double> p_L = new List<double>();
                    ReadDblListIni("Col" + i.ToString(), "", c_PathCSTDeltaIniCate + "1.ini", out p_L);
                    CST1DeltaX_L.Add(p_L);
                }
                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol; i++)
                {
                    List<double> p_L = new List<double>();
                    ReadDblListIni("Col" + i.ToString(), "", c_PathCSTDeltaIniCate + "2.ini", out p_L);
                    CST2DeltaX_L.Add(p_L);
                }
                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol; i++)
                {
                    List<double> p_L = new List<double>();
                    ReadDblListIni("Col" + i.ToString(), "", c_PathCSTDeltaIniCate + "3.ini", out p_L);
                    CST3DeltaX_L.Add(p_L);
                }
                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol; i++)
                {
                    List<double> p_L = new List<double>();
                    ReadDblListIni("Col" + i.ToString(), "", c_PathCSTDeltaIniCate + "4.ini", out p_L);
                    CST4DeltaX_L.Add(p_L);
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #region 龙骨拍照

        #region 上龙骨拍照
        private StateComprehensive_enum RecordTopKeel(
            TriggerSource_enum trigerSource_e, int pos, int g_NoCamera, string g_regClearCamera, string g_regFinishPhoto, Hashtable htResult)
        {
            try
            {
                if (iTopKeelCnt == 0)
                {
                    ShowState_event?.Invoke("触发新卡塞拍照，清除当前卡塞数据");
                    ClearCSTData();
                }
                BaseResult result = (BaseResult)htResult[strCellMatch];
                if (result.Num != 0)
                {
                    iTopKeelCnt++;
                    result.Y_L.Sort();

                    double resultTopPxl = result.Y_L[0];

                    if (Math.Abs(Math.Abs(result.Y_L[0] - result.StdY) * ParCalibWorld.V_I[g_NoCamera] - ParInsertRecipe.P_I.stdCSTLayerInterval) < 1.8)
                    {
                        resultTopPxl = result.Y_L[0] - ParInsertRecipe.P_I.stdCSTLayerInterval / ParCalibWorld.V_I[g_NoCamera];
                        ShowAlarm_event("根据软件计算得到最上方龙骨像素坐标:" + resultTopPxl);
                    }
                    else if (Math.Abs(Math.Abs(result.Y_L[0] - result.StdY) * ParCalibWorld.V_I[g_NoCamera] - 2 * ParInsertRecipe.P_I.stdCSTLayerInterval) < 1.8)
                    {
                        resultTopPxl = result.Y_L[0] - ParInsertRecipe.P_I.stdCSTLayerInterval * 2 / ParCalibWorld.V_I[g_NoCamera];
                        ShowAlarm_event("根据软件计算得到最上方龙骨像素坐标:" + resultTopPxl);
                    }

                    TopKeelPos_L.Add(new Point2D(result.DeltaX * ParCalibWorld.V_I[g_NoCamera], (result.StdY - resultTopPxl) * ParCalibWorld.V_I[g_NoCamera]));
                    //这里Y方向存储要补正的值是PLC的正方向
                    return DealResult(1, @"龙骨识别成功", g_regClearCamera, g_regFinishPhoto);
                }
                else
                {
                    return DealResult(2, @"龙骨识别失败", g_regClearCamera, g_regFinishPhoto);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return DealResult(2, @"龙骨计算异常", g_regClearCamera, g_regFinishPhoto);
            }
        }
        #endregion

        #region 下龙骨拍照
        private StateComprehensive_enum RecordBottomKeel(
            TriggerSource_enum trigerSource_e, int pos, int g_NoCamera, string g_regClearCamera, string g_regFinishPhoto, Hashtable htResult)
        {
            try
            {

                BaseResult result = htResult[strCellMatch] as BaseResult;
                if (result.Num != 0)
                {
                    iBottomKeelCnt++;
                    result.Y_L.Sort((x, y) => -x.CompareTo(y));

                    double resultBottomPxl = result.Y_L[0];

                    if (Math.Abs(Math.Abs(result.Y_L[0] - result.StdY) * ParCalibWorld.V_I[g_NoCamera] - ParInsertRecipe.P_I.stdCSTLayerInterval) < 1.8)
                    {
                        resultBottomPxl = result.Y_L[0] + ParInsertRecipe.P_I.stdCSTLayerInterval / ParCalibWorld.V_I[g_NoCamera];
                        ShowAlarm_event("根据软件计算得到最下方龙骨像素坐标:" + resultBottomPxl);
                    }
                    else if (Math.Abs(Math.Abs(result.Y_L[0] - result.StdY) * ParCalibWorld.V_I[g_NoCamera] - 2 * ParInsertRecipe.P_I.stdCSTLayerInterval) < 1.8)
                    {
                        resultBottomPxl = result.Y_L[0] + ParInsertRecipe.P_I.stdCSTLayerInterval * 2 / ParCalibWorld.V_I[g_NoCamera];
                        ShowAlarm_event("根据软件计算得到最下方龙骨像素坐标:" + resultBottomPxl);
                    }

                    BottomKeelPos_L.Add(new Point2D(result.DeltaX * ParCalibWorld.V_I[g_NoCamera], (result.StdY - resultBottomPxl) * ParCalibWorld.V_I[g_NoCamera]));

                    if (iBottomKeelCnt >= ParInsertRecipe.P_I.KeelCol)
                    {
                        if (!CalcCSTData())
                        {
                            return DealResult(2, @"龙骨计算数据异常", g_regClearCamera, g_regFinishPhoto);
                        }
                        ShowState_event("开始检测卡塞规格!");
                        for (int k = 0; k < ParInsertRecipe.P_I.confCSTCol; ++k)
                        {
                            if (Math.Abs(TopKeelPos_L[k].DblValue1 - TopKeelPos_L[k + 1].DblValue1) > KeelIntervalGate || Math.Abs(BottomKeelPos_L[k].DblValue1 - BottomKeelPos_L[k + 1].DblValue1) > KeelIntervalGate)
                            {
                                return DealResult(2, @"龙骨间距超出设定范围" + KeelIntervalGate + "mm", g_regClearCamera, g_regFinishPhoto);
                            }
                            if (Math.Abs(TopKeelPos_L[k].DblValue2 - TopKeelPos_L[k + 1].DblValue2) > StepZIntervalGate || Math.Abs(BottomKeelPos_L[k].DblValue2 - BottomKeelPos_L[k + 1].DblValue2) > StepZIntervalGate)
                            {
                                return DealResult(2, @"龙骨左右高低差超出设定范围" + StepZIntervalGate + "mm", g_regClearCamera, g_regFinishPhoto);
                            }
                        }
                    }
                    return DealResult(1, @"龙骨定位完成，无任何异常", g_regClearCamera, g_regFinishPhoto);
                }
                else
                {
                    return DealResult(2, @"龙骨识别失败，请检查匹配参数", g_regClearCamera, g_regFinishPhoto);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return DealResult(2, @"龙骨计算过程软件捕捉到异常", g_regClearCamera, g_regFinishPhoto);
            }
        }
        #endregion

        #region 水平展开左龙骨拍照
        private StateComprehensive_enum CalcLeftKeel(
            TriggerSource_enum trigerSource_e, int pos, int g_NoCamera, string g_regClearCamera, string g_regFinishPhoto, Hashtable htResult)
        {
            try
            {
                BaseResult result = htResult[strCellMatch] as BaseResult;
                if (result.Num != 0)
                {
                    LeftKeelOffset = Math.Round(result.DeltaX * ParCalibWorld.V_I[g_NoCamera], 3);
                    ShowState_event("龙骨左侧定位偏差:" + LeftKeelOffset);
                }
                else
                {
                    ShowAlarm_event("左侧龙骨抓取NG!");
                }
                FinishPhotoPLC(1, g_regClearCamera, g_regFinishPhoto);
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return DealResult(2, @"龙骨计算异常", g_regClearCamera, g_regFinishPhoto);
            }
        }
        #endregion

        #region 水平展开右龙骨拍照
        private StateComprehensive_enum CalcRightKeel(
            TriggerSource_enum trigerSource_e, int pos, int g_NoCamera, string g_regClearCamera, string g_regFinishPhoto, Hashtable htResult)
        {
            try
            {
                BaseResult result = htResult[strCellMatch] as BaseResult;
                if (result.Num != 0)
                {
                    RightKeelOffset = Math.Round(result.DeltaX * ParCalibWorld.V_I[g_NoCamera], 3);
                    ShowState_event("龙骨右侧定位偏差:" + RightKeelOffset);
                }
                else
                {
                    ShowAlarm_event("右侧龙骨抓取NG!");
                }
                FinishPhotoPLC(1, g_regClearCamera, g_regFinishPhoto);
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return DealResult(2, @"龙骨计算异常", g_regClearCamera, g_regFinishPhoto);
            }
        }
        #endregion

        #endregion

        #region 清空当前卡塞数据
        public void ClearCSTData()
        {
            try
            {
                TopKeelPos_L.Clear();
                BottomKeelPos_L.Clear();
                DeltaZ.Clear();
                StepZ_L.Clear();
                DeltaXFirstLine_L.Clear();
                DeltaXBaseOnFirstLine_L.Clear();
                switch (ParInsertRegData.P_I.CurStationNo)
                {
                    case 1:
                        CST1DeltaX_L.Clear();
                        CST1PosAll_L.Clear();
                        break;
                    case 2:
                        CST2DeltaX_L.Clear();
                        CST2PosAll_L.Clear();
                        break;
                    case 3:
                        CST3DeltaX_L.Clear();
                        CST3PosAll_L.Clear();
                        break;
                    case 4:
                        CST4DeltaX_L.Clear();
                        CST4PosAll_L.Clear();
                        break;
                    default:
                        break;
                }

                File.Delete(c_PathCSTDeltaIni);
                File.Delete(c_PathCSTIni);
                File.Delete(c_PathCSTZIntervalIni);
                File.Delete(c_PathCSTZIni);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region 处理拍照结果
        private StateComprehensive_enum DealResult(int result, string msg, string g_regClearCamera, string g_regFinishPhoto)
        {
            FinishPhotoPLC(result, g_regClearCamera, g_regFinishPhoto);
            if (result == 1)
            {
                ShowState_event(msg);
                return StateComprehensive_enum.True;
            }
            else
            {
                ShowAlarm_event(msg);
                return StateComprehensive_enum.False;
            }
        }
        #endregion

        #region 给PLC清空拍照触发寄存器，并且发送结果寄存器
        private void FinishPhotoPLC(int result, string g_regClearCamera, string g_regFinishPhoto)
        {
            if (ParSetPLC.P_I.TypePLC_e == TypePLC_enum.Null)
            {
                return;
            }

            LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, result);
        }
        #endregion

        #region 计算卡塞数据
        private bool CalcCSTData()
        {
            try
            {
                //拍照计数清0
                iTopKeelCnt = 0;
                iBottomKeelCnt = 0;

                //生成基准插篮链表
                switch (ParInsertRegData.P_I.CurStationNo)
                {
                    case 1:
                        CST1PosAll_L = CreateKeelPos();
                        WriteInsertPos(CST1PosAll_L);
                        break;
                    case 2:
                        CST2PosAll_L = CreateKeelPos();
                        WriteInsertPos(CST2PosAll_L);
                        break;
                    case 3:
                        CST3PosAll_L = CreateKeelPos();
                        WriteInsertPos(CST3PosAll_L);
                        break;
                    case 4:
                        CST4PosAll_L = CreateKeelPos();
                        WriteInsertPos(CST4PosAll_L);
                        break;
                    default:
                        break;
                }

                if (CalcComZ() && CalcComX())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 生成基准插篮坐标
        private List<List<Point2D>> CreateKeelPos()
        {
            List<List<Point2D>> pLst = new List<List<Point2D>>();

            try
            {
                double std = 0;
                switch (ParInsertRegData.P_I.CurStationNo)
                {
                    case 1:
                        std = Math.Round(ParInsertRegData.P_I.Insert1Std, 3);
                        break;
                    case 2:
                        std = Math.Round(ParInsertRegData.P_I.Insert2Std, 3);
                        break;
                    case 3:
                        std = Math.Round(ParInsertRegData.P_I.Insert3Std, 3);
                        break;
                    case 4:
                        std = Math.Round(ParInsertRegData.P_I.Insert4Std, 3);
                        break;
                }
                ShowState_event("卡塞" + ParInsertRegData.P_I.CurStationNo + "基准值:" + std);
                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol; i++)
                {
                    List<Point2D> cLst = new List<Point2D>();
                    for (int j = 0; j < ParInsertRecipe.P_I.confCSTRow; j++)
                    {
                        cLst.Add(new Point2D(std + InsertWindow.StdDirection * ParInsertRecipe.P_I.confCol1Interval + InsertWindow.StdDirection * (i + 0.5) * ParInsertRecipe.P_I.confKeelInterval, 0));
                    }
                    pLst.Add(cLst);
                }
                return pLst;
            }
            catch (Exception ex)
            {
                return new List<List<Point2D>>();
            }
        }
        #endregion

        #region 将基准插篮数据写入Ini
        private void WriteInsertPos(List<List<Point2D>> p_L)
        {
            try
            {
                if (File.Exists(c_PathCSTIni))
                {
                    File.Delete(c_PathCSTIni);
                }
                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol * ParInsertRecipe.P_I.confCSTRow; i++)
                {
                    WriteP2ListIni(@"Col" + i.ToString(), "", c_PathCSTIni, p_L[i]);

                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region 读取Ini生成double类型链表
        private static bool ReadDblListIni(string strSection, string strKey, string strPath, out List<double> list)
        {
            list = new List<double>();
            int intNum = IniFile.I_I.ReadIniInt(strSection, "Num", strPath);
            try
            {
                for (int i = 0; i < intNum; i++)
                {
                    double dblX = IniFile.I_I.ReadIniDbl(strSection, strKey + "X" + i.ToString(), strPath);

                    list.Add(dblX);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("DealComprehensiveResult2", ex);
                return false;
            }
        }
        #endregion

        #region 读取Ini生成Point2D类型链表工具
        private static bool ReadP2ListIni(string strSection, string strKey, string strPath, out List<Point2D> list)
        {
            list = new List<Point2D>();
            int intNum = IniFile.I_I.ReadIniInt(strSection, "Num", strPath);
            try
            {
                for (int i = 0; i < intNum; i++)
                {
                    double dblX = IniFile.I_I.ReadIniDbl(strSection, strKey + "X" + i.ToString(), strPath);
                    double dblY = IniFile.I_I.ReadIniDbl(strSection, strKey + "Y" + i.ToString(), strPath);

                    list.Add(new Point2D(dblX, dblY));
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 将Point2D链表写入Ini工具
        private bool WriteP2ListIni(string strSection, string strKey, string strPath, List<Point2D> list)
        {
            try
            {
                IniFile.I_I.WriteIni(strSection, "Num", list.Count.ToString(), strPath);
                for (int i = 0; i < list.Count; i++)
                {
                    IniFile.I_I.WriteIni(strSection, strKey + "X" + i.ToString(), list[i].DblValue1.ToString(), strPath);
                    IniFile.I_I.WriteIni(strSection, strKey + "Y" + i.ToString(), list[i].DblValue2.ToString(), strPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 计算行高和Z轴补偿
        private bool CalcComZ()
        {
            try
            {
                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol; ++i)
                {
                    int addr_insertcomz_index1 = 0;
                    int addr_insertcomz_index2 = 0;
                    int addr_insertstepz_index1 = 0;
                    int addr_insertstepz_index2 = 0;
                    //计算每一列的高度补偿
                    DeltaZ.Add(InsertWindow.ZComprehensiveDirection * (TopKeelPos_L[i].DblValue2 + TopKeelPos_L[i + 1].DblValue2) / 2);

                    StepZ_L.Add(ParInsertRecipe.P_I.stdCSTLayerInterval + (TopKeelPos_L[i].DblValue2 - BottomKeelPos_L[i].DblValue2) / (ParInsertRecipe.P_I.confCSTRow - 2));

                    switch (ParInsertRegData.P_I.CurStationNo)
                    {
                        case 1:
                            addr_insertcomz_index1 = ParInsertRegData.P_I.addr_insert1comz_index1;
                            addr_insertcomz_index2 = ParInsertRegData.P_I.addr_insert1comz_index2;
                            addr_insertstepz_index1 = ParInsertRegData.P_I.addr_insert1stepz_index1;
                            addr_insertstepz_index2 = ParInsertRegData.P_I.addr_insert1stepz_index2;
                            break;
                        case 2:
                            addr_insertcomz_index1 = ParInsertRegData.P_I.addr_insert2comz_index1;
                            addr_insertcomz_index2 = ParInsertRegData.P_I.addr_insert2comz_index2;
                            addr_insertstepz_index1 = ParInsertRegData.P_I.addr_insert2stepz_index1;
                            addr_insertstepz_index2 = ParInsertRegData.P_I.addr_insert2stepz_index2;
                            break;
                        case 3:
                            addr_insertcomz_index1 = ParInsertRegData.P_I.addr_insert3comz_index1;
                            addr_insertcomz_index2 = ParInsertRegData.P_I.addr_insert3comz_index2;
                            addr_insertstepz_index1 = ParInsertRegData.P_I.addr_insert3stepz_index1;
                            addr_insertstepz_index2 = ParInsertRegData.P_I.addr_insert3stepz_index2;
                            break;
                        case 4:
                            addr_insertcomz_index1 = ParInsertRegData.P_I.addr_insert4comz_index1;
                            addr_insertcomz_index2 = ParInsertRegData.P_I.addr_insert4comz_index2;
                            addr_insertstepz_index1 = ParInsertRegData.P_I.addr_insert4stepz_index1;
                            addr_insertstepz_index2 = ParInsertRegData.P_I.addr_insert4stepz_index2;
                            break;
                    }
                    WriteRegData(addr_insertcomz_index1, addr_insertcomz_index2 + i, DeltaZ[i]);
                    WriteRegData(addr_insertstepz_index1, addr_insertstepz_index2 + i, StepZ_L[i]);
                    if (Math.Abs(DeltaZ[i]) > 3)
                    {
                        ShowAlarm_event(string.Format("第{0}列首行高度补偿:{1}", i + 1, DeltaZ[i] + ",超出阈值"));
                    }
                    else
                    {
                        ShowState_event(string.Format("第{0}列首行高度补偿:{1}", i + 1, DeltaZ[i]));
                    }
                    if (Math.Abs(StepZ_L[i] - ParInsertRecipe.P_I.stdCSTLayerInterval) > 0.05)
                    {
                        ShowAlarm_event(string.Format("第{0}列视觉层间距:{1}", i + 1, StepZ_L[i] + ",超出阈值"));
                    }
                    else
                    {
                        ShowState_event(string.Format("第{0}列视觉层间距:{1}", i + 1, StepZ_L[i]));
                    }
                }
                WriteDeltaZ();
                WriteStepZ();
                return true;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return false;
            }
        }

        private void WriteRegData(int index1, int index2, double data)
        {
            try
            {
                switch (index1)
                {
                    case 1:
                        LogicPLC.L_I.WriteRegData1(index2, data);
                        return;
                    case 2:
                        LogicPLC.L_I.WriteRegData2(index2, data);
                        return;
                    case 3:
                        LogicPLC.L_I.WriteRegData3(index2, data);
                        return;
                    case 4:
                        LogicPLC.L_I.WriteRegData4(index2, data);
                        return;
                    case 5:
                        LogicPLC.L_I.WriteRegData5(index2, data);
                        return;
                    case 6:
                        LogicPLC.L_I.WriteRegData6(index2, data);
                        return;
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #region 计算每一列中每一行依据上下龙骨的线性偏差
        private bool CalcComX()
        {
            try
            {
                //对此函数进行重写
                //计算纵向偏差
                for (int i = 0; i < ParInsertRecipe.P_I.KeelCol; i++)
                {
                    List<double> lst = new List<double>();
                    //纵向deltax
                    double deltax = BottomKeelPos_L[i].DblValue1 - TopKeelPos_L[i].DblValue1;
                    double avgDeltaX = deltax / (ParInsertRecipe.P_I.confCSTRow);
                    for (int j = 0; j < ParInsertRecipe.P_I.confCSTRow; j++)
                    {
                        double deltaX = avgDeltaX * j;
                        lst.Add(deltaX);
                    }
                    DeltaXBaseOnFirstLine_L.Add(lst);
                }
                //计算横向偏差
                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol; ++i)
                {
                    List<double> lst = new List<double>();

                    for (int j = 0; j < ParInsertRecipe.P_I.confCSTRow; j++)
                    {
                        double deltax = (TopKeelPos_L[i].DblValue1 + TopKeelPos_L[i + 1].DblValue1) / 2.0;
                        lst.Add(deltax);
                    }
                    DeltaXFirstLine_L.Add(lst);
                }

                //偏差相加
                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol; ++i)
                {
                    List<double> lst = new List<double>();
                    for (int j = 0; j < ParInsertRecipe.P_I.confCSTRow; ++j)
                    {
                        lst.Add(DeltaXFirstLine_L[i][j] + DeltaXBaseOnFirstLine_L[i][j]);
                    }
                    DeltaX_L.Add(lst);
                }
                switch (ParInsertRegData.P_I.CurStationNo)
                {
                    case 1:
                        CST1DeltaX_L = DeltaX_L;
                        WriteDeltaXIni(CST1DeltaX_L);
                        break;
                    case 2:
                        CST2DeltaX_L = DeltaX_L;
                        WriteDeltaXIni(CST2DeltaX_L);
                        break;
                    case 3:
                        CST3DeltaX_L = DeltaX_L;
                        WriteDeltaXIni(CST3DeltaX_L);
                        break;
                    case 4:
                        CST4DeltaX_L = DeltaX_L;
                        WriteDeltaXIni(CST4DeltaX_L);
                        break;
                    default:
                        break;
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 将Z轴补偿写入Ini
        private bool WriteDeltaZ()
        {
            try
            {
                if (File.Exists(c_PathCSTZIni))
                {
                    File.Delete(c_PathCSTZIni);
                }

                WriteDblListIni("deltaz", c_PathCSTZIni, DeltaZ);
                return true;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return false;
            }
        }
        #endregion

        #region 将层高写入Ini
        private bool WriteStepZ()
        {
            try
            {
                if (File.Exists(c_PathCSTZIntervalIni))
                {
                    File.Delete(c_PathCSTZIntervalIni);
                }

                WriteDblListIni("step", c_PathCSTZIntervalIni, StepZ_L);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 将Double链表写入Ini工具
        private bool WriteDblListIni(string strSection, string strPath, List<double> list)
        {
            try
            {
                IniFile.I_I.WriteIni(strSection, "Num", list.Count.ToString(), strPath);
                for (int i = 0; i < list.Count; i++)
                {
                    IniFile.I_I.WriteIni(strSection, "X" + i.ToString(), list[i].ToString(), strPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 将X方向偏差写入Ini
        private bool WriteDeltaXIni(List<List<double>> p_L)
        {
            try
            {
                if (File.Exists(c_PathCSTDeltaIni))
                {
                    File.Delete(c_PathCSTDeltaIni);
                }
                for (int i = 0; i < ParInsertRecipe.P_I.confCSTCol * ParInsertRecipe.P_I.confCSTRow; ++i)
                    WriteDblListIni(@"Col" + i.ToString(), c_PathCSTDeltaIni, p_L[i]);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 插篮计算catch日志记录

        private void WriteLog(Exception ex)
        {
            Log.L_I.WriteError("BaseDealInsert", ex);
        }

        #endregion
    }
}
