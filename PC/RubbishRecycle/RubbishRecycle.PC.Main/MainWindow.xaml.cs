using RubbishRecycle.Models;
using RubbishRecycle.PC.Communication;
using RubbishRecycle.Toolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RubbishRecycle.PC.Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Fields

        private readonly SalerOrderProxy _salerOrderProxy = new SalerOrderProxy();

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            Util.SetCertificatePolicy();
        }

        #endregion

        #region Properties

        private String _appToken;
        public String AppToken
        {
            get { return this._appToken; ; }
            set
            {
                if (this._appToken != value)
                {
                    this._appToken = value;
                    OnPropertyChanged("AppToken");
                }
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OperationResult result = this._salerOrderProxy.QueryOrderViewsByPage(1);
            this.OrderViewList.ItemsSource = result.Data as IEnumerable;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            AccountProxy proxy = new AccountProxy();
            OperationResult result = proxy.Logout(this.AppToken);
            AppToken = null;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            AccountProxy proxy = new AccountProxy();
            OperationResult result = proxy.GetChangePasswordVerifyCode(this.AppToken);
            if (result.IsSuccess)
            {
                ChangePasswordInfo info = new ChangePasswordInfo();
                info.Password = "654321";
                info.VerifyCode = String.Empty;
                result = proxy.ChangePassword(info, this.AppToken);
                if (result.IsSuccess)
                {
                    MessageBox.Show("修改密码成功");
                }
                else
                {
                    MessageBox.Show("修改密码失败");
                }
            }
            else
            {
                MessageBox.Show("获取验证码失败");
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            LoginWindow win = new LoginWindow();
            win.Owner = this;
            win.ShowDialog();
        }
    }
}
