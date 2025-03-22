using System.Diagnostics;
using Discord;
using Discord.Interactions;

namespace Cinnabar.Modules;

public class GeneralModule : InteractionModuleBase
{
    
    [SlashCommand("say", "Receives an input from the user and repeats it")]
    public async Task Say(string input)
    {
        await RespondAsync($"{Context.User.Mention} said: {input}");
    }

    [SlashCommand("ping", "Pong!")]
    public async Task Pong()
    {
        Stopwatch stopwatch = new Stopwatch();
        
        stopwatch.Start();
        await RespondAsync("Ping!");
        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        string elapsedTime = ts.Milliseconds.ToString();
        await DeleteOriginalResponseAsync();
        await ReplyAsync($"Pong! {ts.Milliseconds}ms");
        
    }

    [SlashCommand("info", "Displays information about the bot")]
    public async Task Info()
    {
        var botVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        var dnetVer = DiscordConfig.Version;
        var embed = new EmbedBuilder()
            .WithTitle($"About {Context.Client.CurrentUser.Username}")
            .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl())
            .WithDescription($"Cinnabar v{botVer} is a general-purpose Discord bot built on Discord.NET v{dnetVer} running on .NET {Environment.Version}")
            .WithColor(new Color(255, 5, 59))
            .WithFooter($"Command ran by {Context.User.Username}")
            .WithCurrentTimestamp()
            .Build();
        await RespondAsync(embed: embed);
    }
}