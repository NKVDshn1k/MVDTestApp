using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MVDTestApp.Data.Base;
using MVDTestApp.Locator;
using MVDTestApp.Model;
using MVDTestApp.View;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;
using EntityWorkTask = MVDTestApp.Data.Entityes.WorkTask;
using WorkTaskStatus = MVDTestApp.Data.Entityes.WorkTaskStatus;

namespace MVDTestApp.ViewModel;

public class WorkTaskDetailsViewModel : BaseViewModel
{
    public static event Action<WorkTask> RequestForEditing;

    public WorkTask Task { get; }


    private IRepository<EntityWorkTask> _repository;
    private IMapper _mapper;
    public WorkTaskDetailsViewModel(IRepository<EntityWorkTask> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;

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
            var etityTask = await _repository.GetAsync(task.Id);
            if (etityTask == null)
                throw new Exception(Literals.TaskNotFound);
            etityTask.Status = status;
            await _repository.UpdateAsync(etityTask);
            task.Status = status;
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
            var etityTask = await _repository.Items.FirstAsync(x => x.Id == task.Id);
            if (etityTask == null)
                throw new Exception(Literals.TaskNotFound);

            if (etityTask.Status != WorkTaskStatus.Completed)
                if (!new FactualHourseSetter(etityTask).ShowDialog().Value)
                    return false;

            etityTask.Status = WorkTaskStatus.Completed;

            etityTask.Completion =
                DateTime.Now;

            for(int i = 0; i < task.SubTasks.Count; i++)
                if (!await CompliteHierarchically(task.SubTasks[i]))
                    return false;

            await _repository.UpdateAsync(etityTask);

            if (task.SubTasks.Count != etityTask?.SubTasks?.Count)
                _mapper.Map(task.SubTasks, etityTask.SubTasks);

            _mapper.Map(etityTask, task);

            return true;
        }
        catch (Exception ex)
        {
            ErrorAssistant.Display(ex, Literals.ResetStateError);
            return false;
        }
    }
}
