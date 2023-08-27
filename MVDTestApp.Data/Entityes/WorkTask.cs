
using MVDTestApp.Data.Base;

namespace MVDTestApp.Data.Entityes;

public class WorkTask : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Executors { get; set; }
    public DateTime Registration { get; set; }
    public WorkTaskStatus Status { get; set; }
    public int PlannedHours { get; set; }
    public int? FactualHours { get; set; }
    public DateTime? Completion { get; set; }

    public ICollection<WorkTask>? SubTasks { get; set;}
    public int? ParentWorkTaskId { get; set; }
    public WorkTask? ParentWorkTask { get; set; }

}