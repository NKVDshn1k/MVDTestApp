using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MVDTestApp.Data.Base;
using MVDTestApp.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using WorkTaskStatus = MVDTestApp.Data.Entityes.WorkTaskStatus;
using EntityWorkTask = MVDTestApp.Data.Entityes.WorkTask;
using MVDTestApp.View;

namespace MVDTestApp;

public static class DbDomainSharedQuery
{
    private static IRepository<EntityWorkTask> _repository =
        App.Services.GetRequiredService<IRepository<EntityWorkTask>>();

    private static IMapper _mapper =
        App.Services.GetRequiredService<IMapper>();


    public static async Task DeleteTask(WorkTask task, ObservableCollection<WorkTask> tasks)
    {
        if (task.HasSubTasks)
        {
            MessageBox.Show(Literals.CanNotDeliteNotTerminalTask);
            return;
        }
        if (task.ParentTaskId == null)
            tasks.Remove(task);
        else
            task.ParentTask.SubTasks.Remove(task);

        await _repository.RemoveAsync(task.Id);
    }

    public static async Task<ObservableCollection<WorkTask>> LoadTasks()
    {
        var entityTasks =
            (await _repository.Items.ToArrayAsync())
            .Where(x => x.ParentWorkTaskId == null);

        return
            new ObservableCollection<WorkTask>(_mapper.Map<WorkTask[]>(entityTasks));
    }

    public static async Task ReloadTask(WorkTask task, ObservableCollection<WorkTask> tasks)
    {
        var reloadedTask = _mapper
                    .Map<WorkTask>(await _repository.GetAsync(task.Id));

        int taskIndex = tasks.IndexOf(task);
        tasks[taskIndex] = reloadedTask;
    }


    public static async Task EditeTask(WorkTask task)
    {
        var entityTask = await _repository.GetAsync(task.Id);

        if (task.SubTasks == null || task.SubTasks.Count == 0)
            entityTask = _mapper.Map(task, entityTask);
        else
        {
            //При изменении задач с подзадачами EF бросает ошибку
            //Пока нет идей как это исправить, кроме вот таких костылей(
            entityTask.Name = task.Name;
            entityTask.PlannedHours = task.PlannedHours;
            entityTask.Description = task.Description;
            entityTask.Executors = task.Executors;
        }

        await _repository.UpdateAsync(entityTask);
    }

    public static async Task AddTask(WorkTask task, WorkTask parentTask = null)
    {

        if (parentTask != null)
            parentTask.AddSubTask(task);
        var entityTask = _mapper.Map<WorkTask, EntityWorkTask>(task);
        await _repository.AddAsync(entityTask);
        _mapper.Map(entityTask, task);
    }

    public static async Task ResetState(WorkTask task, WorkTaskStatus status)
    {
        var etityTask = await _repository.GetAsync(task.Id);

        if (etityTask == null)
            throw new Exception(Literals.TaskNotFound);

        etityTask.Status = status;
        await _repository.UpdateAsync(etityTask);

        task.Status = status;
    }

    public static async Task<bool> CompliteHierarchically(WorkTask task)
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

        for (int i = 0; i < task.SubTasks.Count; i++)
            if (!await CompliteHierarchically(task.SubTasks[i]))
                return false;

        await _repository.UpdateAsync(etityTask);

        if (task.SubTasks.Count != etityTask?.SubTasks?.Count)
            _mapper.Map(task.SubTasks, etityTask.SubTasks);

        _mapper.Map(etityTask, task);

        return true;
    }
}
