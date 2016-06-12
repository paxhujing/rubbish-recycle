using RubbishRecycle.Models;
using RubbishRecycle.PC.Communication;
using System;
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

        private readonly AccountProxy _proxy = new AccountProxy();

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
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
            //Account account = this._proxy.GetAccount(App.Token,App.AESProvider);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            OperationResult result = this._proxy.Logout(App.Token);
            AppToken = null;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            OperationResult result = this._proxy.GetChangePasswordVerifyCode(App.Token);
            if (result.IsSuccess)
            {
                ChangePasswordInfo info = new ChangePasswordInfo();
                info.Password = "654321";
                info.VerifyCode = String.Empty;
                result = this._proxy.ChangePassword(info, App.Token);
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
