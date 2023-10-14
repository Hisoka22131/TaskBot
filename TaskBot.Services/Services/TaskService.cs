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

    #region Properties

    private readonly IUnitOfWork _unitOfWork;

    private IUserRepository UserRepository => _unitOfWork.UserRepository;

    private ITaskRepository TaskRepository => _unitOfWork.TaskRepository;

    #endregion

    public async Task<string> CreateTask(string taskString, long telegramUserId)
    {
        var taskArray = taskString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (!taskArray.Any() || taskArray.Length == 1) return "Введите в правильном формате!";

        var task = new Task
        {
            Number = taskArray[0],
            Description = string.Join(" ", taskArray),
            CreateDt = DateTime.Now,
            User = await UserRepository.GetUser(telegramUserId)
        };

        await TaskRepository.Insert(task);
        await _unitOfWork.Save();

        return "Задача создана";
    }

    public async Task<string> CloseTask(string taskNumber, long telegramUserId)
    {
        var task = await TaskRepository.GetTask(taskNumber);

        if (task == null) return $"Задача {taskNumber} не найдена";


        task.IsClosed = true;
        task.CloseDt = DateTime.Now;

        await TaskRepository.Update(task);
        await _unitOfWork.Save();

        return $"Задача {task.Number} закрыта";
    }
}