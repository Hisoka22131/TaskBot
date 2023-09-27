using TaskBot.Repository.Repo.Abstract;

namespace TaskBot.Repository.UoF;

public interface IUnitOfWork: IDisposable
{
    IUserRepository UserRepository { get; set; }
    ITaskRepository TaskRepository { get; set; }

    Task<int> Save();
}