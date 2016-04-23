using RubbishRecycle.Models;
using RubbishRecycle.PC.Communication;
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

        private String _publicKey;

        #endregion

        public LoginWindow()
        {
            InitializeComponent();
        }

        #region Methods

        #region Init

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Prompt.BusyContent = "初始化...";
            Prompt.IsBusy = true;
            this._proxy.RequestCommunicationAsync(InitCallback);
        }

        private void InitCallback(OperationResult<String> result)
        {
            Prompt.Dispatcher.Invoke(() =>
            {
                Prompt.IsBusy = false;
            });
            if (result.IsSussess)
            {
                this._publicKey = result.Data;
            }
            else
            {
                MessageBox.Show(result.ErrorMessage);
            }
        }

        #endregion

        #region Login

        private void StartLogin_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            DialogResult = true;
            Byte[] secretKey = App.AESProvider.Key;
            RegisterInfo ri = new RegisterInfo();
            ri.BindingPhone = "18284559968";
            ri.Name = "hujing";
            ri.Password = "123456";
            ri.SecretKey = secretKey;
            OperationResult<String> result = this._proxy.RegisterBuyer(ri, this._publicKey);
            if (result.IsSussess)
            {
                App.Token = result.Data;
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
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

            Prompt.BusyContent = "注册中...";
            Prompt.IsBusy = true;
            RegisterInfo ri = new RegisterInfo();
            ri.BindingPhone = "18284559968";
            ri.Name = "hujing";
            ri.Password = "123456";
            ri.SecretKey = App.AESProvider.Key;
            ri.IV = App.AESProvider.IV;
            this._proxy.RegisterBuyerAsync(ri, this._publicKey, RegisterCallback);
        }

        private void RegisterCallback(OperationResult<String> result)
        {
            Prompt.Dispatcher.Invoke(() =>
            {
                Prompt.IsBusy = false;
                if (result.IsSussess)
                {
                    App.Token = result.Data;
                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                    MessageBox.Show(result.ErrorMessage);
                    return;
                }
            });
        }
    }
}
