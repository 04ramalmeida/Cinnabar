using System.Net.Sockets;
using System.Reflection;
using Cinnabar.Modules;
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
    private static DiscordSocketClient _client;
    private static InteractionService _interaction;

    static IServiceProvider CreateServices()
    {
        
        var collection = new ServiceCollection()
            .AddSingleton(new DiscordSocketConfig())
            .AddSingleton(new DiscordSocketClient())
            .AddTransient<GeneralModule>()
            .AddTransient<FunModule>()
            .AddTransient<EmbedBase>();

        return collection.BuildServiceProvider();
    }
    
    public static async Task Main()
    {
        _serviceProvider = CreateServices();
        _client = _serviceProvider.GetRequiredService<DiscordSocketClient>();
        _client.Log += Log;

        var interactionServiceConfig = new InteractionServiceConfig
        {
            LogLevel = LogSeverity.Debug,
        };
        _interaction = new InteractionService(_client, interactionServiceConfig);
        _interaction.Log += Log;
        
        var token = JsonConvert.DeserializeObject<Config>(File.ReadAllText("appsettings.json")).Token;
        
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        

        await _interaction.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        _client.Ready += async () =>  await _interaction.RegisterCommandsGloballyAsync();
        _client.InteractionCreated += (socketInteraction) => InteractionCreated(socketInteraction);

        
        await Task.Delay(-1);
    }

    private static async Task InteractionCreated(SocketInteraction socketInteraction)
    {
        try
        {
            SocketInteractionContext ctx = new(_client, socketInteraction);

            await _interaction.ExecuteCommandAsync(ctx, _serviceProvider);
        }
        catch
        {
            if (socketInteraction.Type == InteractionType.ApplicationCommand)
            {
                await socketInteraction.GetOriginalResponseAsync()
                    .ContinueWith(async (msg) => msg.Result.DeleteAsync());
            }
        }
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}