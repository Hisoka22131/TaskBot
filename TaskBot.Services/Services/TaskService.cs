using TaskBot.Repository.Repo.Abstract;
using TaskBot.Repository.UoF;
using TaskBot.Services.Interfaces;
using Task = TaskBot.Library.Models.Task;

namespace TaskBot.Services.Services;

public class TaskService : ITaskService
{
    public TaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private readonly IUnitOfWork _unitOfWork;

    private IUserRepository UserRepository => _unitOfWork.UserRepository;

    private ITaskRepository TaskRepository => _unitOfWork.TaskRepository;

    public async Task<string> CreateTask(string taskString, long chatId)
    {
        var taskArray = taskString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (taskArray.Length == 1) return "Введите в правильном формате!";

        var task = new Task
        {
            Number = taskArray[0],
            Description = taskArray[1],
            CreateDt = DateTime.Now,
            User = await UserRepository.GetUser(chatId)
        };

        await TaskRepository.Insert(task);
        await _unitOfWork.Save();

        return "Задача создана";
    }
}