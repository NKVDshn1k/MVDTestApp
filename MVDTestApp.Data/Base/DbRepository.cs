using Microsoft.EntityFrameworkCore;
using MVDTestApp.Data.Entityes;
using System.Threading.Channels;

namespace MVDTestApp.Data.Base;

public abstract class DbRepository<T> : IRepository<T> where T : class, IEntity, new()
{
    protected readonly MVDTestContext _db;
    protected readonly DbSet<T> _set;

    public bool AutoSaveChanges { get; set; } = true;

    public DbRepository(MVDTestContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public virtual IQueryable<T> Items => _set;

    public virtual IQueryable<T> ItemsWithoutInclude => _set;


    public virtual T Add(T item)
    {
        try
        {
            _db.Entry(item).State = EntityState.Added;
            if (AutoSaveChanges)
                _db.SaveChanges();
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }
        return item;
    }

    public virtual async Task<T> AddAsync(T item, CancellationToken Cancel = default)
    {
        try
        {
            _db.Entry(item).State = EntityState.Added;
            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel);
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }

        return item;
    }

    public virtual T Get(int id) =>
        Items.FirstOrDefault(x => x.Id.Equals(id));

    public virtual async Task<T> GetAsync(int id, CancellationToken Cancel = default) =>
        await Items.FirstOrDefaultAsync(x => x.Id.Equals(id), Cancel);

    public virtual void Remove(int id)
    {
        var item = Get(id);
        try
        {
            _db.Remove(item);
            if (AutoSaveChanges)
                _db.SaveChanges();
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }
    }

    public virtual async Task RemoveAsync(int id, CancellationToken Cancel = default)
    {
        var item = await GetAsync(id);
        try
        {
            _db.Remove(item);
            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel);
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }
    }

    public virtual void Update(T item)
    {
        try
        {
            _db.Entry(item).State = EntityState.Modified;
            if (AutoSaveChanges)
                _db.SaveChanges();
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }
    }

    public virtual async Task UpdateAsync(T item, CancellationToken Cancel = default)
    {
        try
        {
            _db.Entry(item).State = EntityState.Modified;
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
