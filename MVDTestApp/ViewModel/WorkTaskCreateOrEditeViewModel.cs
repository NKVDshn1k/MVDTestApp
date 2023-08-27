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

    private IRepository<EntityWorkTask> _repository;
    private IMapper _mapper;
    public WorkTaskCreateOrEditeViewModel(IRepository<EntityWorkTask> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;

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
                var entityTask = await _repository.GetAsync(Task.Id);

                if (Task.SubTasks == null || Task.SubTasks.Count == 0)
                    entityTask = _mapper.Map(Task, entityTask);
                else
                {
                    //При изменении задач с подзадачами EF бросает ошибку
                    //Пока нет идей как это исправить, кроме вот таких костылей(
                    entityTask.Name = Task.Name;
                    entityTask.PlannedHours = Task.PlannedHours;
                    entityTask.Description = Task.Description;
                    entityTask.Executors = Task.Executors;  
                }

                await _repository.UpdateAsync(entityTask);
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
                if (_parentTask != null)
                    _parentTask.AddSubTask(Task);
                var entityTask = _mapper.Map<WorkTask, EntityWorkTask>(Task);
                await _repository.AddAsync(entityTask);
                _mapper.Map(entityTask, Task);
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
