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

        private void InitCallback(Task<HttpResponseMessage> r)
        {
            Prompt.Dispatcher.Invoke(() =>
            {
                Prompt.IsBusy = false;
            });
            if (r.IsFaulted)
            {
                MessageBox.Show(r.Exception.Message);
                return;
            }
            this._publicKey = r.Result.Content.ReadAsStringAsync().Result;
        }

        #endregion

        #region Login

        private void StartLogin_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            RijndaelManaged aesProvider = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            Byte[] secretKey = aesProvider.Key;
            Models.RegisterInfo ri = new Models.RegisterInfo();
            ri.BindingPhone = "18284559968";
            ri.Name = "hujing";
            ri.Password = "123456";
            ri.SecretKey = secretKey;
            Models.LoginResult result = this._proxy.RegisterBuyer(ri, this._publicKey);
        }

        #endregion

        #endregion
    }
}
