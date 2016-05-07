using RubbishRecycle.Models;
using RubbishRecycle.PC.Communication;
using RubbishRecycle.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RubbishRecycle.PC.Main
{
    /// <summary>
    /// InitWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        #region Fields

        private readonly AccountProxy _proxy = new AccountProxy();

        #endregion

        public LoginWindow()
        {
            InitializeComponent();
            Util.SetCertificatePolicy();
        }

        #region Methods

        #region Login

        private void StartLogin_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            DialogResult = true;
            RegisterInfo ri = new RegisterInfo();
            ri.BindingPhone = "18284559968";
            ri.Name = "hujing";
            ri.Password = "123456";
            OperationResult<String> result = this._proxy.RegisterBuyer(ri);
            if (result.IsSuccess)
            {
                App.Token = result.Data;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show(result.ErrorMessage);
            }
        }

        #endregion

        #endregion

        private void ForgetPassword_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            RequestParamBeforeSignIn<String> arg = new RequestParamBeforeSignIn<String>();
            arg.Data = "18284559968";
            arg.AppKey = "EDF6D00C74DB486880835FD2AEE8CB71";
            OperationResult result = this._proxy.GetRegisterVerifyCode(arg);
            if (!result.IsSuccess)
            {
                MessageBox.Show(result.ErrorMessage);
                return;
            }
            Prompt.BusyContent = "注册中...";
            Prompt.IsBusy = true;
            RegisterInfo ri = new RegisterInfo();
            ri.AppKey = "EDF6D00C74DB486880835FD2AEE8CB71";
            ri.VerifyCode = String.Empty;
            ri.BindingPhone = "18284559968";
            ri.Name = "hujing";
            ri.Password = "123456";
            this._proxy.RegisterBuyerAsync(ri, RegisterCallback);
        }

        private void RegisterCallback(OperationResult<String> result)
        {
            Prompt.Dispatcher.Invoke(() =>
            {
                Prompt.IsBusy = false;
                if (result.IsSuccess)
                {
                    App.Token = result.Data;
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage);
                    return;
                }
            });
        }
    }
}
