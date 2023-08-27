using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MVDTestApp.Converter;

internal class WorkTaskStatusToTextConverter : IValueConverter
{
    private static string[] _signs =
    {
        "Задача назначена",
        "Задача выполняется",
        "Задача приостоновлена",
        "Задача выполнена"
    };
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        _signs[(int)value];

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}
