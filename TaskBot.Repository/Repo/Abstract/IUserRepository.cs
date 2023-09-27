using TaskBot.Library.Models;
using TaskBot.Repository.GenericRepo;

namespace TaskBot.Repository.Repo.Abstract;

public interface IUserRepository : IGenericRepository<User>
{
    User GetUser(long id);
}