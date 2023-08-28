using MVDTestApp.Locator;
using MVDTestApp.Model;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;
using WorkTaskStatus = MVDTestApp.Data.Entityes.WorkTaskStatus;

namespace MVDTestApp.ViewModel;

public class WorkTaskDetailsViewModel : BaseViewModel
{
    public static event Action<WorkTask> RequestForEditing;

    public WorkTask Task { get; }


    public WorkTaskDetailsViewModel()
    {

        Task = DataLocator.Data as WorkTask;

        if (Task == null)
        {
            ErrorAssistant.ThrowCritical(Literals.WrongParams, this);
            return;
        }

        EditeCommand = new Command(Edite);
        StartCommand = new AsyncCommand(Start);
        StopCommand = new AsyncCommand(Stop);
        CompliteCommand = new AsyncCommand(Complite);
    }

    public Command EditeCommand { get; }
    public AsyncCommand StartCommand { get; }
    public AsyncCommand StopCommand { get; }
    public AsyncCommand CompliteCommand { get; }


    public string DateSign =>
        (! (Task!=null && Task.IsComplited) ?
            Literals.Registraited : Literals.RegistraitedAndComplited) + ": ";

    private void Edite(object param) =>
        RequestForEditing(Task);

    private async Task Start() =>
        await ResetState(Task, WorkTaskStatus.InProgress);

    private async Task Stop() =>
        await ResetState(Task, WorkTaskStatus.Paused);

    private async Task Complite()
    {
        if (!Task.IsSubTasksReadyToComplite())
        {
            MessageBox.Show(Literals.NotAllTasksMayBeComplited);
            return;
        }

        if (await CompliteHierarchically(Task))
            OnPropertyChanged("DateSign");

    }

    private async Task ResetState(WorkTask task, WorkTaskStatus status)
    {
        try
        {
            await DbDomainSharedQuery.ResetState(task, status);
        }
        catch (Exception ex)
        {
            ErrorAssistant.Display(ex, Literals.ResetStateError);
        }
    }

    private async Task<bool> CompliteHierarchically(WorkTask task)
    {
        try
        {
            return await DbDomainSharedQuery.CompliteHierarchically(task);
        }
        catch (Exception ex)
        {
            ErrorAssistant.Display(ex, Literals.ResetStateError);
            return false;
        }
    }
}
