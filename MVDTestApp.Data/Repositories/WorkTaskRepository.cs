using Microsoft.EntityFrameworkCore;
using MVDTestApp.Data.Base;
using MVDTestApp.Data.Entityes;

namespace MVDTestApp.Data.Repositories;

public class WorkTaskRepository : DbRepository<WorkTask>
{
    public override IQueryable<WorkTask> Items =>
        base.Items
        .Include(x => x.ParentWorkTask)
        .ThenInclude(x => x.SubTasks);

    public WorkTaskRepository(MVDTestContext db)
        : base(db) { }

}
