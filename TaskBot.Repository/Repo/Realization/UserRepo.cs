using Microsoft.EntityFrameworkCore;
using TaskBot.Library.Models;
using TaskBot.Repository.GenericRepo;
using TaskBot.Repository.Repo.Abstract;

namespace TaskBot.Repository.Repo.Realization;

public class UserRepo : GenericRepository<User>, IUserRepository
{
    public UserRepo(DbContext context) : base(context)
    {
    }

    public User GetUser(long id) => GetEntity(q => q.ChatId == id, q => q.Tasks);
}