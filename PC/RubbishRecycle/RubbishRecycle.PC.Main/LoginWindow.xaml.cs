using RubbishRecycle.Models;
using RubbishRecycle.PC.Communication;
using System.Windows;

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

        private void StartLogin_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            LoginInfo ri = new LoginInfo();
            ri.AppKey = "EDF6D00C74DB486880835FD2AEE8CB71";
            ri.Name = UserId.Text;
            ri.Password = Password.Password;
            OperationResult result = this._proxy.Login(ri);
            if (result.IsSuccess)
            {
                MainWindow mwin = (MainWindow)this.Owner;
                mwin.AppToken = result.Data.ToString();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show(result.ErrorMessage);
                DialogResult = false;
            }
        }

        private void ForgetPassword_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ForgetPasswordWin win = new ForgetPasswordWin();
            win.Owner = this;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Show();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            RegisterWin win = new RegisterWin();
            win.Owner = this;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Show();
        }
    }
}
