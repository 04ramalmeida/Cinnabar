using System.Diagnostics;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Cinnabar;

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
        IUser botUser = Context.Client.CurrentUser;
        string botAvatar = botUser.GetAvatarUrl();
        string botVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        string dnetVer = DiscordConfig.Version;
        

        Embed embed = EmbedBase.CinnabarEmbed($"About {botUser.Username}",
            $"Cinnabar v{botVer} is a general-purpose Discord bot built on Discord.NET v{dnetVer} running on .NET {Environment.Version}",
            botAvatar,
            null,
            Context.Interaction.User);
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
        Embed embed = EmbedBase.CinnabarEmbed($"About {user.Username}",
            null,
            imageUrl,
            [idField, nameField, nickName, creationDate, joinedDate, isBot],
            user
        );
        await RespondAsync(embed: embed);
    }

    [SlashCommand("serverinfo", "Displays information about the server")]
    public async Task ServerInfo()
    {
        IGuild guild = Context.Guild;
        IGuildUser owner = guild.GetOwnerAsync().Result;
        var serverName = new EmbedFieldBuilder
        {
            Name = "Server Name",
            Value = guild.Name
        };
        var serverId = new EmbedFieldBuilder
        {
            Name = "Server ID",
            Value = guild.Id.ToString()
        };
        var created = new EmbedFieldBuilder
        {
            Name = "Created",
            Value = $"At ``{guild.CreatedAt.ToString()}`` by {owner.Username}"
        };
        var memberCount = new EmbedFieldBuilder
        {
            Name = "Member Count",
            Value = (guild as SocketGuild).MemberCount
        };
        var channelCount = new EmbedFieldBuilder
        {
            Name = "Channel Count",
            Value = (guild as SocketGuild).Channels.Count
        };
        
        Embed embed = EmbedBase.CinnabarEmbed($"About {guild.Name}",
            string.Empty,
            guild.IconUrl,
            [serverName, created, memberCount, channelCount],
            Context.Interaction.User);
        
        await RespondAsync(embed: embed);
    }

    
}