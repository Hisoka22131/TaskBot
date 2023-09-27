using TaskBot.Repository.Repo.Abstract;
using TaskBot.Repository.UoF;
using TaskBot.Services.Interfaces;
using Telegram.Bot.Types;
using Task = System.Threading.Tasks.Task;
using User = TaskBot.Library.Models.User;

namespace TaskBot.Services.Services;

public class UserService : IUserService
{
    
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private IUserRepository UserRepository => _unitOfWork.UserRepository;
    
    public async Task CheckUser(Update update)
    {
        if (update.Message == null) return;
        
        if (await UserRepository.Any(q => q.UserName == update.Message.Chat.Username)) return;

        var user = new User
        {
            UserName = update.Message.Chat.Username!,
            ChatId = update.Message.Chat.Id
        };

        await UserRepository.Insert(user);
        await _unitOfWork.Save();
    }

    public async Task<string?> GetUserTasks(long userChatId)
    {
        var user = UserRepository.GetUser(userChatId);

        return !user.Tasks.Any() ? "У вас нет задач" : user.Tasks.SelectMany(q => q.Number).ToString();
    }
}