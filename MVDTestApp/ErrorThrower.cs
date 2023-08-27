using Microsoft.Extensions.DependencyInjection;
using MVDTestApp.Logging;
using System.Windows;
using System.Runtime.CompilerServices;
using System;

namespace MVDTestApp
{
    internal static class ErrorAssistant
    {
        private static ILogger _loger = App.Services.GetRequiredService<ILogger>();

        public static void ThrowCritical(string message, object caller, [CallerMemberName]string methood = "unknown")
        {
            Infom(message, caller, methood, Literals.CriticalError);
            Application.Current.Shutdown();
        }

        public static void Display(Exception ex, string errorType = null)=>
            Infom(ex.Message, ex.Source, ex.TargetSite.Name, errorType);

        private static void Infom(string message, object caller, string methood, string caption)
        {
            _loger.Info(message, caption, caller, methood);
            MessageBox.Show(message + " : " + caller.ToString() + '.' + methood,
               caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
