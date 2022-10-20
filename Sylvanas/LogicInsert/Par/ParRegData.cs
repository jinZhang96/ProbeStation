using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using BasicClass;

namespace DealInsert
{
    public class ParInsertRegData
    {
        #region 静态类实例
        public static ParInsertRegData P_I = new ParInsertRegData();
        #endregion 静态类实例


        #region 寄存器地址

        #region 通用寄存器
        public int addr_insertdata_index1 = 0;
        public int addr_insertdata_index2 = 0;
        public int addr_curinsertnum_index1 = 0;
        public int addr_curinsertnum_index2 = 0;
        public int addr_confirm_index1 = 0;
        public int addr_confirm_index2 = 0;
        public int addr_curCSTNo_index1 = 0;
        public int addr_curCSTNo_index2 = 0;
        #endregion

        #region 卡塞1寄存器
        public int addr_insert1std_index1 = 0;
        public int addr_insert1std_index2 = 0;
        public int addr_insert1comz_index1 = 0;
        public int addr_insert1comz_index2 = 0;
        public int addr_insert1stepz_index1 = 0;
        public int addr_insert1stepz_index2 = 0;
        #endregion 

        #region 卡塞2寄存器
        public int addr_insert2std_index1 = 0;
        public int addr_insert2std_index2 = 0;
        public int addr_insert2comz_index1 = 0;
        public int addr_insert2comz_index2 = 0;
        public int addr_insert2stepz_index1 = 0;
        public int addr_insert2stepz_index2 = 0;
        #endregion

        #region 卡塞3寄存器
        public int addr_insert3std_index1 = 0;
        public int addr_insert3std_index2 = 0;
        public int addr_insert3comz_index1 = 0;
        public int addr_insert3comz_index2 = 0;
        public int addr_insert3stepz_index1 = 0;
        public int addr_insert3stepz_index2 = 0;
        #endregion

        #region 卡塞4寄存器
        public int addr_insert4std_index1 = 0;
        public int addr_insert4std_index2 = 0;
        public int addr_insert4comz_index1 = 0;
        public int addr_insert4comz_index2 = 0;
        public int addr_insert4stepz_index1 = 0;
        public int addr_insert4stepz_index2 = 0;
        #endregion

        #endregion

        #region 卡塞1

        #region 基准值
        public double Insert1Std
        {
            get
            {
                return GetRegData(addr_insert1std_index1, addr_insert1std_index2);
            }
        }
        #endregion

        #region z轴调整值
        public void SendCST1ComZ(double data)
        {
            try
            {
                WriteRegData(addr_insert1comz_index1, addr_insert1comz_index2, data);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #region 发送实际层高
        public void SendCST1StepZ(double data)
        {
            try
            {
                WriteRegData(addr_insert1stepz_index1, addr_insert1stepz_index2, data);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #endregion

        #region 卡塞2

        #region 基准值
        public double Insert2Std
        {
            get
            {
                return GetRegData(addr_insert2std_index1, addr_insert2std_index2);
            }
        }
        #endregion

        #region z轴调整值
        public void SendCST2ComZ(double data)
        {
            try
            {
                WriteRegData(addr_insert2comz_index1, addr_insert2comz_index2, data);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #region 发送实际层高
        public void SendCST2StepZ(double data)
        {
            try
            {
                WriteRegData(addr_insert2stepz_index1, addr_insert2stepz_index2, data);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #endregion

        #region 卡塞3

        #region 基准值
        public double Insert3Std
        {
            get
            {
                return GetRegData(addr_insert3std_index1, addr_insert3std_index2);
            }
        }
        #endregion

        #region z轴调整值
        public void SendCST3ComZ(double data)
        {
            try
            {
                WriteRegData(addr_insert3comz_index1, addr_insert3comz_index2, data);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #region 发送实际层高
        public void SendCST3StepZ(double data)
        {
            try
            {
                WriteRegData(addr_insert3stepz_index1, addr_insert3stepz_index2, data);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #endregion

        #region 卡塞4

        #region 基准值
        public double Insert4Std
        {
            get
            {
                return GetRegData(addr_insert4std_index1, addr_insert4std_index2);
            }
        }
        #endregion

        #region z轴调整值
        public void SendCST4ComZ(double data)
        {
            try
            {
                WriteRegData(addr_insert4comz_index1, addr_insert4comz_index2, data);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #region 发送实际层高
        public void SendCST4StepZ(double data)
        {
            try
            {
                WriteRegData(addr_insert4stepz_index1, addr_insert4stepz_index2, data);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #endregion

        #region 发送插篮数据并给出确认信号

        #region 外部调用
        public void SendConfirmInsertX(double data)
        {
            try
            {
                SendInsertX(data);
                SendInsertXConfirm();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #region 发送插篮X数据
        private void SendInsertX(double data)
        {
            try
            {
                WriteRegData(addr_insertdata_index1, addr_insertdata_index2, data);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #region 发送插篮数据确认信号
        private void SendInsertXConfirm()
        {
            try
            {
                WriteRegData(addr_confirm_index1, addr_confirm_index2, 1);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #endregion

        #region 当前已插篮数
        public int CurInsertNum
        {
            get
            {
                return (int)(GetRegData(addr_curinsertnum_index1, addr_curinsertnum_index2));
            }
        }
        #endregion

        #region 当前工位号，当卡塞个数>=2时生效
        public int CurStationNo
        {
            get
            {
                if (InsertWindow.NumCST == 1)
                {
                    return 1;
                }
                else
                {
                    return (int)(GetRegData(addr_curCSTNo_index1, addr_curCSTNo_index2));
                }
            }
        }
        #endregion

        #region 读数据寄存器
        private double GetRegData(int index1, int index2)
        {
            switch (index1)
            {
                case 1:
                    return LogicPLC.L_I.ReadRegData1(index2);
                case 2:
                    return LogicPLC.L_I.ReadRegData2(index2);
                case 3:
                    return LogicPLC.L_I.ReadRegData3(index2);
                case 4:
                    return LogicPLC.L_I.ReadRegData4(index2);
                case 5:
                    return LogicPLC.L_I.ReadRegData5(index2);
                case 6:
                    return LogicPLC.L_I.ReadRegData6(index2);
                default:
                    return 0;
            }
        }
        #endregion

        #region 写数据寄存器
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

        #region 日志记录
        private void WriteLog(Exception ex)
        {
            Log.L_I.WriteError("ParInsertRegData", ex);
        }
        #endregion
    }
}
