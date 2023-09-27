using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBot.Services.Interfaces;

namespace TaskBot.Web.Controllers;

[ApiController]
[Route("api/message")]
[AllowAnonymous]
public class BotController : ControllerBase
{
    private readonly IBotService _botService;

    public BotController(IBotService botService) => _botService = botService;

    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> Post([FromBody] object update) => Ok(await _botService.Handle(update));
}