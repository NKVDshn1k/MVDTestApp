using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MVDTestApp.Data.Base;
using MVDTestApp.Locator;
using MVDTestApp.Model;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EntityWorkTask = MVDTestApp.Data.Entityes.WorkTask;

namespace MVDTestApp.ViewModel;

public class WorkTaskCreateOrEditeViewModel : BaseViewModel
{
    public static event Action<WorkTask> EditingOrAddingIsDone;

    public WorkTaskCreateOrEditeViewModel()
    {
        TaskDataPackage pack = DataLocator.Data as TaskDataPackage;

        if (pack == null)
        {
            ErrorAssistant.ThrowCritical(Literals.WrongParams, this);
            return;
        }

        Task = pack.Task;
        IsEditing = pack.IsEdite;
        _parentTask = pack.Parent;

        CancelCommand = new Command(Cancel);
        AcceptCommand = new AsyncCommand(Accept);
    }

    private WorkTask _parentTask;

    private WorkTask _task;
    public WorkTask Task
    {
        get => _task;
        set => SetProperty(ref _task, value);
    }

    private bool _isEditing;
    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    public Command CancelCommand { get; }
    public AsyncCommand AcceptCommand { get; }

    private void Cancel(object param)
    {
        if (IsEditing)
            EditingOrAddingIsDone(Task);
        else if (_parentTask != null)
            EditingOrAddingIsDone(_parentTask);
        else
            EditingOrAddingIsDone(null);
    }

    private bool IsValide(WorkTask task)
    {
        if (string.IsNullOrEmpty(Task.Name))
            return false;
        if (string.IsNullOrEmpty(Task.Executors))
            return false;
        if (Task.PlannedHours <= 0)
            return false;
        return true;
    }

    private async Task Accept()
    {
        if (!IsValide(Task))
        {
            MessageBox.Show(Literals.RequireFields);
            return;
        }

        if (IsEditing)
        {
            try
            {
                await DbDomainSharedQuery.EditeTask(Task);
            }
            catch (Exception ex)
            {
                ErrorAssistant.Display(ex, Literals.EditeError);
            }
        }
        else
        {
            try
            {
                await DbDomainSharedQuery.AddTask(Task, _parentTask);
            }
            catch (Exception ex)
            {
                ErrorAssistant.Display(ex, Literals.AddError);
                EditingOrAddingIsDone(null);
                return;
            }
        }

        EditingOrAddingIsDone(Task);
    }
}
