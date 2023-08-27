using MVDTestApp.Model;
using System;
using System.Linq;
using System.Windows.Controls;

namespace MVDTestApp.View;

/// <summary>
/// Interaction logic for WorkTaskCreateOrEdite.xaml
/// </summary>
public partial class WorkTaskCreateOrEdite : Page
{
    public WorkTaskCreateOrEdite()=>
        InitializeComponent();

    private void DigitsOnlyFilter_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (e.Text.Any(x=>!char.IsDigit(x)))
            e.Handled = true;
    }
}