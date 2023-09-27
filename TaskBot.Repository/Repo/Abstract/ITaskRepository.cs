using TaskBot.Repository.GenericRepo;
using Task = TaskBot.Library.Models.Task;

namespace TaskBot.Repository.Repo.Abstract;

public interface ITaskRepository : IGenericRepository<Task>
{
    Task<Task> GetTask(string number);
}