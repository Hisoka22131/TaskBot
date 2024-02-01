using Discord.Commands;

namespace TaskBot.Services.Commands;

public class DiscordCommands : ModuleBase<SocketCommandContext>
{
    [Command("hello")]
    public async Task HelloWorld()
    {
        var commandText = Context.Message.Content;
        await ReplyAsync("Hello world!");
    }
}