using RubbishRecycle.PC.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //String publicKey = this._proxy.RequestCommunication();
            Task<HttpResponseMessage> tr = this._proxy.RequestCommunicationAsync();
        }

        private void StartLogin_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
