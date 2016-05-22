using RubbishRecycle.Models;
using RubbishRecycle.PC.Communication;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace RubbishRecycle.PC.Main
{
    /// <summary>
    /// RegisterWin.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterWin : Window
    {
        private readonly AccountProxy _proxy = new AccountProxy();

        public RegisterWin()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (String.IsNullOrWhiteSpace(txtVerifyCode.Text))
            {
                return;
            }
            RegisterInfo ri = new RegisterInfo();
            ri.AppKey = txtAppKey.Text;
            ri.VerifyCode = txtVerifyCode.Text;
            ri.BindingPhone = txtPhone.Text;
            ri.Name = txtPassword.Text;
            ri.Password = txtPassword.Text;
            this._proxy.RegisterBuyerAsync(ri, RegisterCallback);
        }

        private void RegisterCallback(OperationResult result)
        {
            if (result.IsSuccess)
            {
                App.Token = result.Data.ToString();
                MessageBox.Show("注册成功");
            }
            else
            {
                MessageBox.Show(result.ErrorMessage);
                return;
            }
        }

        private void RetriveVerifyCode_Click(object sender, RoutedEventArgs e)
        {
            RequestParamBeforeSignIn<String> arg = new RequestParamBeforeSignIn<String>();
            arg.Data = txtPhone.Text;
            arg.AppKey = txtAppKey.Text;
            OperationResult result = this._proxy.GetRegisterVerifyCode(arg);
            if (!result.IsSuccess)
            {
                MessageBox.Show(result.ErrorMessage);
            }
        }
    }
}
