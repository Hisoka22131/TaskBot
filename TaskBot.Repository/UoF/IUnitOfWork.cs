using TaskBot.Repository.Repo.Abstract;

namespace TaskBot.Repository.UoF;

public interface IUnitOfWork: IDisposable
{
    IUserRepository UserRepository { get; set; }

    Task<int> Save();
}