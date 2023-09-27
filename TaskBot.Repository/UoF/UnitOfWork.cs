using TaskBot.Library.Context;
using TaskBot.Repository.Repo.Abstract;
using TaskBot.Repository.Repo.Realization;

namespace TaskBot.Repository.UoF;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork()
    {
        _dbContext = new TaskContext();
        UserRepository = new UserRepo(_dbContext);
        TaskRepository = new TaskRepo(_dbContext);
    }
    
    private readonly TaskContext _dbContext;
    
    public IUserRepository UserRepository { get; set; }
    public ITaskRepository TaskRepository { get; set; }
    
    public async void Dispose() => await _dbContext.DisposeAsync();

    public async Task<int> Save() => await _dbContext.SaveChangesAsync();
}