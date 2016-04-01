using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RubbishRecycle.PC.Main
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            LoginWindow win = new LoginWindow();
            Boolean? result = win.ShowDialog();
            if (result.HasValue && result.Value)
            {
                ShutdownMode = ShutdownMode.OnMainWindowClose;
                base.OnStartup(e);
            }
            else
            {
                Shutdown();
            }
        }
    }
}
