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

    public override void Update(WorkTask item)
    {
        try
        {
            _db.Update(item);
            if (AutoSaveChanges)
                _db.SaveChanges();
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }
    }

    public override async Task UpdateAsync(WorkTask item, CancellationToken Cancel = default)
    {
        try
        {

            _db.Update(item);
            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel);
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }
    }

}
