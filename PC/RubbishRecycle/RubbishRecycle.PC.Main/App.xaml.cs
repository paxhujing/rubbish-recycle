using System;
using System.Security.Cryptography;
using System.Windows;

namespace RubbishRecycle.PC.Main
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //internal static readonly RijndaelManaged AESProvider = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };

        internal static String Token;

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    ShutdownMode = ShutdownMode.OnExplicitShutdown;
        //    LoginWindow win = new LoginWindow();
        //    Boolean? result = win.ShowDialog();
        //    if (result.HasValue && result.Value)
        //    {
        //        ShutdownMode = ShutdownMode.OnMainWindowClose;
        //        base.OnStartup(e);
        //    }
        //    else
        //    {
        //        Shutdown();
        //    }
        //}
    }
}
