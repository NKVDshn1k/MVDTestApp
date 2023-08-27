using MVDTestApp.Data.Entityes;
using MVDTestApp.Locator;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MVDTestApp.Model;

public class WorkTask : ObservableObject
{
    public int Id { get; set; }

    private string _name;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _description;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private string _executors;
    public string Executors
    {
        get => _executors;
        set => SetProperty(ref _executors, value);
    }

    private DateTime _registration;
    public DateTime Registration
    {
        get => _registration;
        set => SetProperty(ref _registration, value);
    }

    private WorkTaskStatus _status;
    public WorkTaskStatus Status
    {
        get => _status;
        set
        {
            SetProperty(ref _status, value);
            this.OnPropertyChanged("IsStopedOrNotStarted");
            this.OnPropertyChanged("IsInProgress");
            this.OnPropertyChanged("IsStoped");
            this.OnPropertyChanged("IsComplited");
        }
    }

    private int _plannedHours;
    public int PlannedHours
    {
        get => _plannedHours;
        set => SetProperty(ref _plannedHours, value);
    }

    private int _factualHours;
    public int FactualHours
    {
        get => _factualHours;
        set => SetProperty(ref _factualHours, value);
    }

    #region Countable

    public bool HasSubTasks =>
        SubTasks?.Count > 0;

    public int SubPlannedHours =>
        SubTasks?.Sum(x => x.FullPlannedHours) ?? 0;

    public int FullPlannedHours =>
        PlannedHours + SubPlannedHours;

    public int SubFactualHours =>
    SubTasks?.Sum(x => x.FullFactualHours) ?? 0;

    public int FullFactualHours =>
        FactualHours + SubFactualHours;

    public bool IsStopedOrNotStarted =>
    Status == WorkTaskStatus.Paused || Status == WorkTaskStatus.Assigned;

    public bool IsInProgress =>
        Status == WorkTaskStatus.InProgress;

    public bool IsStoped =>
        Status == WorkTaskStatus.Paused;

    public bool IsComplited =>
        Status == WorkTaskStatus.Completed;

    #endregion


    private DateTime? _completion;

    public DateTime? Completion
    {
        get => _completion;
        set => SetProperty(ref _completion, value);
    }

    public ObservableCollection<WorkTask> SubTasks { get; set; }

    public int? ParentTaskId { get; set; }
    public WorkTask ParentTask { get; set; }

    public bool IsSubTasksReadyToComplite()
    {
        foreach (WorkTask task in this.SubTasks)
        {
            if (task.Status != WorkTaskStatus.InProgress && task.Status != WorkTaskStatus.Completed)
                return false;
            if (task.SubTasks != null && !task.IsSubTasksReadyToComplite())
                return false;
        }
        return true;
    }

    public void CompliteSubTasks()
    {
        foreach (WorkTask task in this.SubTasks)
        {
            task.Status = WorkTaskStatus.Completed;
            task.Completion = DateTime.Now;
            task.CompliteSubTasks();
        }
    }

    public void AddSubTask(WorkTask task)
    {
        SubTasks.Add(task);
        task.ParentTask = this;
        task.ParentTaskId = Id;
    }

    public Command ReloadCommand =>
        DataLocator.MainWindowVM.ReloadCommand;
    public Command AddTaskCommand =>
        DataLocator.MainWindowVM.AddTaskCommand;
    public Command EditeTaskCommand => 
        DataLocator.MainWindowVM.EditeTaskCommand;
    public Command DeleteTaskCommand =>
        DataLocator.MainWindowVM.DeleteTaskCommand;
}