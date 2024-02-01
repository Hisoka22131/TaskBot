using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBot.Services.Interfaces;

namespace TaskBot.Web.Controllers;

[ApiController]
[Route("api/message")]
[AllowAnonymous]
public class BotController : ControllerBase
{
    private readonly ITelegramBotService _telegramBotService;
    private readonly IDiscordBotService _discordBotService;

    public BotController(ITelegramBotService telegramBotService, IDiscordBotService discordBotService)
    {
        _telegramBotService = telegramBotService;
        _discordBotService = discordBotService;
    }
    
    [HttpPost]
    [Route("telegram/update")]
    public async Task<IActionResult> TelegramRequest([FromBody] object update) => new OkObjectResult(await _telegramBotService.Handle(update));
    
    [HttpPost]
    [Route("discord/update")]
    public async Task<IActionResult> DiscordRequest([FromBody] object update) => new OkObjectResult(await _discordBotService.Handle(update));
}