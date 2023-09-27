namespace TaskBot.Services.Interfaces;

public interface ITaskService
{
    Task<string> CreateTask(string taskString, long chatId);
}