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

namespace DealCIM
{
    /// <summary>
    /// PostTrackOut.xaml 的交互逻辑
    /// </summary>
    public partial class PostTrackOut : Window
    {
        #region 定义
        int testcnt = 0;
        static object locker = new object();

        static PostTrackOut instance = null;

        const string port = "COM2";
        const int baudrate = 9600;

        public static Action<object> UpLoadChipID;
        public static Action UpLoadLot;
        public static Action UpLoadTrackOut;
        #endregion
        public static PostTrackOut GetInstance()
        {
            if (instance == null)
                instance = new PostTrackOut();
            return instance;
        }

        private PostTrackOut()
        {
            InitializeComponent();
        }

        private void BtnPostChipID_Click(object sender, RoutedEventArgs e)
        {
            UpLoadChipID?.Invoke(tbChipID.Text);
        }

        private void BtnPostLot_Click(object sender, RoutedEventArgs e)
        {
            CIM.StrLot = tbLot.Text;
            CIM.WriteCimConfig();
            UpLoadLot?.Invoke();
            this.Hide();
        }

        private void BtnPostTrackOut_Click(object sender, RoutedEventArgs e)
        {
            UpLoadTrackOut?.Invoke();
        }

        private void BtnAddChipdID_Click(object sender, RoutedEventArgs e)
        {
            CIM.AppendChipIDList(tbChipID1.Text);
            testcnt++;
        }

        private void BtnLoadChipdID_Click(object sender, RoutedEventArgs e)
        {
            CIM.chipid_list.Clear();
            CIM.LoadList(testcnt);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void BtnGetChipID_Click(object sender, RoutedEventArgs e)
        {
            VYCode.instance.Write();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BtnPostLot_Click(sender, e);
            }
        }
    }
}
