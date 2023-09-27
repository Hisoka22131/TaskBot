using Microsoft.EntityFrameworkCore;
using Task = TaskBot.Library.Models.Task;
using User = TaskBot.Library.Models.User;

namespace TaskBot.Library.Context;

public class TaskContext : DbContext
{
    private const string _workDB =
        "Server=.\\; Database=TaskManager; Trusted_Connection=True; MultipleActiveResultSets=True; TrustServerCertificate=True";
    
    public TaskContext()
    {
    }

    public TaskContext(DbContextOptions<TaskContext> dbContextOptions): base(dbContextOptions)
    {
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_workDB);
        }
    }
}