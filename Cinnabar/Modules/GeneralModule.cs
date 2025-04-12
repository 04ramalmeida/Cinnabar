using System.Diagnostics;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

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
        var botUser = Context.Client.CurrentUser;
        var botVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        var dnetVer = DiscordConfig.Version;
        var embed = new EmbedBuilder()
            .WithTitle($"About {botUser.Username}")
            .WithThumbnailUrl(botUser.GetAvatarUrl())
            .WithDescription($"Cinnabar v{botVer} is a general-purpose Discord bot built on Discord.NET v{dnetVer} running on .NET {Environment.Version}")
            .WithColor(new Color(255, 5, 59))
            .WithFooter($"Command ran by {botUser.Username}")
            .WithCurrentTimestamp()
            .Build();
        await RespondAsync(embed: embed);
    }

    [SlashCommand("userinfo", "Displays information about the user")]
    public async Task UserInfo(IUser? user = null)
    {
        user ??= Context.Interaction.User;
        IGuildUser guildUser = Context.Guild.GetUserAsync(user.Id).Result;
        var idField = new EmbedFieldBuilder()
        {
            Name = "User ID",
            Value = user.Id.ToString()
        };
        var nameField = new EmbedFieldBuilder()
        {
            Name = "Display Name",
            Value = guildUser.GlobalName ?? guildUser.DisplayName
        };
        var nickName = new EmbedFieldBuilder()
        {
            Name = "Nickname",
            Value = guildUser.DisplayName
        };
        var creationDate = new EmbedFieldBuilder()
        {
            Name = "Account creation date",
            Value = user.CreatedAt.ToString()
        };
        var joinedDate = new EmbedFieldBuilder()
        {
            Name = "Account creation date",
            Value = guildUser.JoinedAt.ToString()
        };
        var isBot = new EmbedFieldBuilder
        {
            Name = "Bot",
            Value = guildUser.IsBot
        };
        var imageUrl = user.GetAvatarUrl();
        var embed = new EmbedBuilder() 
            .WithAuthor($"About {user.Username}", iconUrl: imageUrl)
            .WithFields(idField, nameField, nickName, creationDate, joinedDate, isBot)
            .WithThumbnailUrl(imageUrl)
            .WithColor(new Color(255, 5, 59))
            .WithFooter($"Command ran by {user.Username}")
            .WithCurrentTimestamp()
            .Build();
        await RespondAsync(embed: embed);
    }

    
}