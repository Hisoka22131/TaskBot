using Microsoft.EntityFrameworkCore;
using TaskBot.Repository.GenericRepo;
using TaskBot.Repository.Repo.Abstract;
using Task = TaskBot.Library.Models.Task;

namespace TaskBot.Repository.Repo.Realization;

public class TaskRepo : GenericRepository<Task>, ITaskRepository
{
    public TaskRepo(DbContext context) : base(context)
    {
    }

    public async Task<Task> GetTask(string number) => await GetEntity(q => q.Number == number, q => q.User);
}