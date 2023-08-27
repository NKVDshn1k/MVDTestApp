
using MVDTestApp.View;
using MVDTestApp.ViewModel;

namespace MVDTestApp.Locator
{
    internal static class DataLocator
    {
        internal static object Data;
        //По каким-то причинам RelativeSource к коммандам VM не работает, поэтому использую этот костыль -\(-_-)/-
        internal static MainWindowViewModel MainWindowVM { get; } = 
            App.Current.MainWindow.DataContext as MainWindowViewModel;
    }
}
