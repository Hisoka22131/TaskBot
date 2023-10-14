using Telegram.Bot.Types;

namespace TaskBot.Services.Interfaces;

public interface IUserService
{ 
    Task CheckUser(Update update);
    Task<ICollection<Library.Models.Task>> GetUserTasks(long telegramUserId);
}