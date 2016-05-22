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
    /// ForgetPasswordWin.xaml 的交互逻辑
    /// </summary>
    public partial class ForgetPasswordWin : Window
    {
        private readonly AccountProxy _proxy = new AccountProxy();

        public ForgetPasswordWin()
        {
            InitializeComponent();
        }

        private void RetriveVerifyCode_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            RequestParamBeforeSignIn<String> arg = new RequestParamBeforeSignIn<String>();
            arg.Data = txtPhone.Text;
            arg.AppKey = txtAppKey.Text;
            OperationResult result = this._proxy.GetForgetPasswordVerifyCode(arg);
            if (!result.IsSuccess)
            {
                MessageBox.Show(result.ErrorMessage);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (String.IsNullOrWhiteSpace(txtVerifyCode.Text))
            {
                return;
            }
            ForgetPasswordInfo ri = new ForgetPasswordInfo();
            ri.AppKey = txtAppKey.Text;
            ri.VerifyCode = txtVerifyCode.Text;
            ri.Phone = txtPhone.Text;
            ri.Password = txtPassword.Text;
            this._proxy.ForegetPassword(ri);
        }
    }
}
