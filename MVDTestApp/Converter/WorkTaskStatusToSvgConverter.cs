using MVDTestApp.Properties;
using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace MVDTestApp.Converter;

public class WorkTaskStatusToSvgConverter : IValueConverter
{
    private static string[] _icons =
    {
        Encoding.UTF8.GetString(Resources.TaskSeted),
        Encoding.UTF8.GetString(Resources.TaskInProgress),
        Encoding.UTF8.GetString(Resources.TaskStopped),
        Encoding.UTF8.GetString(Resources.TaskComplited)
    };
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        _icons[(int)value];

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}
