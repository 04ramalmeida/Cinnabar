using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace Cinnabar;
public class Program
{
    private static IServiceProvider _serviceProvider;

    static IServiceProvider CreateServices()
    {
        
        var collection = new ServiceCollection()
            .AddSingleton(new DiscordSocketConfig())
            .AddSingleton(new DiscordSocketClient());

        return collection.BuildServiceProvider();
    }
    
    public static async Task Main()
    {
        var _serviceProvider = CreateServices();
        var client = _serviceProvider.GetRequiredService<DiscordSocketClient>();
        client.Log += Log;

        var _interaction = new InteractionService(client.Rest);
        
        var token = JsonConvert.DeserializeObject<Config>(File.ReadAllText("appsettings.json")).Token;
        
        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();
        
        
        
        await Task.Delay(-1);
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}