using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBot.Services.Interfaces;

namespace TaskBot.Web.Controllers;

[ApiController]
[Route("api/telegram/message")]
[AllowAnonymous]
public class TelegramController : ControllerBase
{
    private readonly ITelegramBotService _telegramBotService;

    public TelegramController(ITelegramBotService telegramBotService) => _telegramBotService = telegramBotService;
    
    [HttpPost]
    public async Task<IActionResult> TelegramRequest([FromBody] object update) => new OkObjectResult(await _telegramBotService.HandleMessage(update));
    
}