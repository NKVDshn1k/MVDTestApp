using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MVDTestApp.Data.Base;
using MVDTestApp.Locator;
using MVDTestApp.Model;
using MVDTestApp.View;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using EntityWorkTask = MVDTestApp.Data.Entityes.WorkTask;

namespace MVDTestApp.ViewModel;

public class MainWindowViewModel : BaseViewModel
{
    private const int _pageResetTime = 30;

    private IRepository<EntityWorkTask> _repository;
    private IMapper _mapper;

    public MainWindowViewModel(IRepository<EntityWorkTask> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;

        ReloadCommand = new Command(async (x) => await Reload(x));
        AddTaskCommand = new Command(AddTask);
        EditeTaskCommand = new Command(EditeTask);
        DeleteTaskCommand = new Command(async (x) => await DeleteTask(x));
        SelectItemCommand = new Command(SelectItem);

        WorkTaskCreateOrEditeViewModel.EditingOrAddingIsDone
            += EditingOrAddingDoneHandler;
        WorkTaskDetailsViewModel.RequestForEditing
            += EditeTask;

        FrameOpacity = 0;

        ReloadAndShowPage();
    }



    private ObservableCollection<WorkTask> _tasks;
    public ObservableCollection<WorkTask> Tasks
    {
        get => _tasks;
        set => SetProperty(ref _tasks, value);
    }

    private Page _currentPage;
    public Page CurrentPage
    {
        get => _currentPage;
        set
        {
            SetProperty(ref _currentPage, value);
            OnPropertyChanged("NoAnyPageSetted");
        }
    }

    public bool AnyTasks =>
        Tasks != null && Tasks.Any();


    private double _frameOpacity;
    public double FrameOpacity
    {
        get => _frameOpacity;
        set => SetProperty(ref _frameOpacity, value);
    }

    public bool NoAnyPageSetted =>
        CurrentPage == null;

    public Command ReloadCommand { get; }
    public Command AddTaskCommand { get; }
    public Command EditeTaskCommand { get; }
    public Command DeleteTaskCommand { get; }
    public Command SelectItemCommand { get; }

    private async Task ReloadAndShowPage()
    {
        await Reload();

        if (Tasks == null || Tasks.Count == 0)
            return;

        ShowDetails(Tasks[0]);
    }

    private async Task Reload(object param = null)
    {
        WorkTask task = (WorkTask)param;

        try
        {
            if (task == null)
            {
                Tasks = await DbDomainSharedQuery.LoadTasks();
            }
            else
            {
                await DbDomainSharedQuery.ReloadTask(task, Tasks);
            }
        }
        catch (Exception ex)
        {
            ErrorAssistant.Display(ex, Literals.LoadError);
        }
    }
    private void AddTask(object param)
    {
        var parentTask = (WorkTask)param;

        if (parentTask != null && parentTask.Status == Data.Entityes.WorkTaskStatus.Completed)
        {
            MessageBox.Show(Literals.CanNotAddSubTaskToComplited);
            return;
        }

        DataLocator.Data =
            new TaskDataPackage(false, new WorkTask(), parentTask);
        ResetPage(new WorkTaskCreateOrEdite());
    }

    private void EditeTask(object param)
    {
        DataLocator.Data =
            new TaskDataPackage(true, (WorkTask)param);
        ResetPage(new WorkTaskCreateOrEdite());
    }

    private async Task DeleteTask(object param)
    {
        try
        {
            WorkTask task = param as WorkTask;

            await DbDomainSharedQuery.DeleteTask(task, Tasks);

            if (!AnyTasks)
                ResetPage(null);
        }
        catch (Exception ex)
        {
            ErrorAssistant.Display(ex, Literals.DeleteError);
        }
    }

    private void ShowDetails(object param)
    {
        DataLocator.Data = param;
        ResetPage(new WorkTaskDetails());

        if (param != null && param is WorkTask)
            (param as WorkTask).IsSelectedInTreeWiew = true;
    }

    private void SelectItem(object param) =>
        ShowDetails(param);

    private Task _pageReseting;
    private CancellationTokenSource _cancellationToken;

    private void ResetPage(Page page = null)
    {

        if (_pageReseting != null && !_pageReseting.IsCompleted)
            _cancellationToken.Cancel();

        _cancellationToken = new CancellationTokenSource();

        _pageReseting = Task.Run(async () =>
        {
            if (CurrentPage != null)
            {
                for (double i = FrameOpacity; i >= 0; i -= 0.1)
                {
                    FrameOpacity = i;
                    await Task.Delay(_pageResetTime);

                    if (_cancellationToken.Token.IsCancellationRequested)
                        return;
                }
            }
            CurrentPage = page;
            if (page != null)
            {
                for (double i = 0; i <= 1; i += 0.1)
                {
                    FrameOpacity = i;
                    await Task.Delay(_pageResetTime);

                    if (_cancellationToken.Token.IsCancellationRequested)
                        return;
                }
            }
        }, _cancellationToken.Token);
    }

    private void EditingOrAddingDoneHandler(WorkTask task)
    {

        if (task != null)
        {
            bool hasParent = 
                task.ParentTask != null;
            if (!hasParent && !Tasks.Any(x => x.Id == task.Id))
            {
                Tasks.Add(task);
            }
            else
            {
                if (hasParent)
                {
                    task.ParentTask.IsExpandedInTreeWiew = true;
                }
                task.IsSelectedInTreeWiew = true;
            }

            ShowDetails(task);
        }
        else if (AnyTasks)
        {
            ShowDetails(Tasks[0]);
        }
        else
        {
            ShowDetails(null);
        }
    }
}
