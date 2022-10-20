using BasicClass;
using DealCIM;
using System;
using System.Windows;
using DealPLC;
using DealRobot;
using ModulePackage;

namespace Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WinMain1 : MainWindow
    {
        #region 定义        

        #endregion 定义

        #region 其他

        #endregion 其他

        #region CIM模拟

        #endregion CIM模拟

        #region 操作设置
        private void epSetWork_Collapsed(object sender, RoutedEventArgs e)
        {

        }

        private void epSetWork_Expanded(object sender, RoutedEventArgs e)
        {

        }
        #endregion 操作设置        

        private void InsertTempComR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                //ModelParams.InsertTempComR = Math.Round((double)InsertTempComR.Value, 2);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

       


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OperationWnd wnd = new OperationWnd();
            wnd.Show();
        }

        public void RefreshData()
        {
            //this.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    lblProSUM.Content = string.Format("精定位产量：{0}", RegeditMain.R_I.PreciseSUM);
            //    lblPreciseNG.Content = string.Format("精定位NG：{0}", RegeditMain.R_I.PreciseNG);
            //    lblWastageNG1.Content = string.Format("残材1NG：{0}", RegeditMain.R_I.WastageNG1);
            //    lblWastageNG2.Content = string.Format("残材2NG：{0}", RegeditMain.R_I.WastageNG2);
            //}));
        }

        private void MHardwareConfig_Click(object sender, RoutedEventArgs e)
        {
            if(!EngineerReturn())
            {
                return;
            }

            WndHardwareConfig wnd = WndHardwareConfig.GetInstance();
            if (!wnd.IsVisible)
                wnd.Show();
        }

        private void BtnProductivity_Click(object sender, RoutedEventArgs e)
        {
            ProductivityReport wndProductivityReport = new ProductivityReport();
            wndProductivityReport.Show();
        }


        private void BtnSetPar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
