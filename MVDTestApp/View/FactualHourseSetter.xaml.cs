using MVDTestApp.Data.Entityes;
using System.Linq;
using System.Windows;

namespace MVDTestApp.View;


public partial class FactualHourseSetter : Window
{
    private WorkTask _task;

    public FactualHourseSetter(WorkTask task)
    {
        InitializeComponent();

        _task = task;
        TaskName_TextBlock.Text = task.Name;
    }

    private void DigitsOnlyFilter_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (e.Text.Any(x => !char.IsDigit(x)))
            e.Handled = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void AcceptButton_Click(object sender, RoutedEventArgs e)
    {
        int hours = int.Parse(FactualHourse_TextBox.Text);
        if (hours <= 0) 
        {
            MessageBox.Show(Literals.FactualHoursMustBeMoreThanZero);
            return;
        }
        _task.FactualHours = hours;
       DialogResult = true;
    }
}
