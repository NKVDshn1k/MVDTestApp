using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MVDTestApp.Data.Entityes;

namespace MVDTestApp.Data;

public class MVDTestContext : DbContext
{
    public DbSet<WorkTask> Tasks { get; set; }
    public MVDTestContext(DbContextOptions<MVDTestContext> options) 
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkTask>()
            .HasOne(x => x.ParentWorkTask)
            .WithMany(x => x.SubTasks)
            .HasForeignKey(x => x.ParentWorkTaskId);
    }
}
