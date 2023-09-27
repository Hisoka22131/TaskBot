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

    public async Task<User> GetUser(long chatId) => await GetEntity(q => q.ChatId == chatId, q => q.Tasks);
}