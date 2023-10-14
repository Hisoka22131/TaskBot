namespace TaskBot.Services.Interfaces;

public interface ITaskService
{
    Task<string> CreateTask(string taskString, long telegramUserId);

    Task<string> CloseTask(string taskNumber, long telegramUserId);
}