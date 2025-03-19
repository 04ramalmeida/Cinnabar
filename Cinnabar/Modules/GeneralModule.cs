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
}