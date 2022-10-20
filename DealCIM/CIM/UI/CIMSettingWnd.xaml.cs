using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DealFile;
using BasicClass;

namespace DealCIM
{
    /// <summary>
    /// CIMSettingWnd.xaml 的交互逻辑
    /// </summary>
    public partial class CIMSettingWnd : Window
    {
        #region 定义
        string ClassName = "CIMWnd";
        #endregion

        public CIMSettingWnd()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存输入的配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CIM.StrSendQueue = tbSendQueue.Text;
                CIM.StrReadQueue = tbReadQueue.Text;
                CIM.StrIP = tbIP.Text;
                CIM.StrPort = tbPort.Text;
                CIM.StrUserID = tbUserID.Text;
                CIM.StrFab = tbFab.Text;
                CIM.StrArea = tbArea.Text;
                CIM.StrLine = tbLine.Text;
                CIM.StrOperation = tbOperation.Text;

                CIM.WriteCimConfig();
                this.Close();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }

        /// <summary>
        /// 要我写十几个函数都输入section和path_config是不可能的，希望你也是
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void WriteCimConfig(string key, string value)
        {
            try
            {
                IniFile.I_I.WriteIni(CIM.section, key, value, CIM.Path_Config);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 重连cim
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReconnect_Click(object sender, RoutedEventArgs e)
        {
            CIM.C_I.ReConnect();
        }

        /// <summary>
        /// 初始化窗口之后从本地读取参数数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CIM.InitParams();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
            //binding
            DataContext = CIM.C_I;
        }
    }
}
