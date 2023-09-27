using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskBot.Library.Context;
using TaskBot.Repository.UoF;
using TaskBot.Services.Interfaces;
using TaskBot.Services.Services;
using Telegram.Bot;

namespace TaskBot.IoC;

public static class ServiceProviderExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext()
            .AddUnitOfWork()
            .AddTelegramBotClient(configuration)
            .AddCustomServices();
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services) =>
        services.AddDbContext<TaskContext>();

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services) =>
        services.AddScoped<IUnitOfWork, UnitOfWork>();

    private static IServiceCollection AddTelegramBotClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        var client = new TelegramBotClient(configuration["BotToken"]);
        var webHook = $"{configuration["Url"]}/api/message/update";
        client.SetWebhookAsync(webHook).Wait();

        return services.AddSingleton<ITelegramBotClient>(client);
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services) =>
        services
            .AddScoped<IBotService, BotService>()
            .AddScoped<IUserService, UserService>();
}