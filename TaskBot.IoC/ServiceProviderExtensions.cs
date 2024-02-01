using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
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
            .AddCustomServices()
            .AddTelegramBotClient(configuration)
            .AddDiscordClient(configuration);
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services) =>
        services.AddDbContext<TaskContext>();

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services) =>
        services.AddScoped<IUnitOfWork, UnitOfWork>();

    private static IServiceCollection AddTelegramBotClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ITelegramBotService, TelegramBotService>();
        
        var client = new TelegramBotClient(configuration["TelegramBotToken"]!);
        var webHook = $"{configuration["Url"]}/api/telegram/message";
        client.SetWebhookAsync(webHook).Wait();

        return services.AddSingleton<ITelegramBotClient>(client);
    }

    private static IServiceCollection AddDiscordClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDiscordBotService, DiscordBotService>();
        
        var config = new DiscordSocketConfig { GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent };
        var client = new DiscordSocketClient(config);
        var commands = new CommandService();
        
        client.LoginAsync(TokenType.Bot, configuration["DiscordBotToken"]!).Wait();
        client.StartAsync().Wait();
        
        services.AddSingleton(client).AddSingleton(commands);
        
        var discordBotService = services.BuildServiceProvider().GetRequiredService<IDiscordBotService>();
        client.MessageReceived += discordBotService.HandleMessage;

        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services) =>
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<ITaskService, TaskService>();
}