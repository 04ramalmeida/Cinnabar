using System.Net.Sockets;
using System.Reflection;
using Cinnabar.Modules;
using Cinnabar.Services;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace Cinnabar;
public class Program
{
    private static DiscordSocketClient _client;
    private static InteractionService _interaction;
    private static IHost _host;
    
    
    public static async Task Main()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton(new DiscordSocketConfig());
                services.AddSingleton(new DiscordSocketClient());
                services.AddSingleton<ApiService>();
                services.AddSingleton<GeneralModule>();
                services.AddSingleton<FunModule>();
                services.AddSingleton<EmbedBase>();

            })
            .Build();
        
        

        var config = _host.Services.GetRequiredService<IConfiguration>();
        _client = _host.Services.GetRequiredService<DiscordSocketClient>();
        _client.Log += Log;

        var interactionServiceConfig = new InteractionServiceConfig
        {
            LogLevel = LogSeverity.Debug,
        };
        _interaction = new InteractionService(_client, interactionServiceConfig);
        _interaction.Log += Log;

        var token = config.GetValue<string>("Token");
        
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        

        await _interaction.AddModulesAsync(Assembly.GetEntryAssembly(), _host.Services);
        _client.Ready += async () =>  await _interaction.RegisterCommandsGloballyAsync();
        _client.InteractionCreated += InteractionCreated;

        await _host.RunAsync();
        await Task.Delay(-1);
    }

    private static async Task InteractionCreated(SocketInteraction socketInteraction)
    {
        try
        {
            SocketInteractionContext ctx = new(_client, socketInteraction);

            await _interaction.ExecuteCommandAsync(ctx, _host.Services);
        }
        catch
        {
            if (socketInteraction.Type == InteractionType.ApplicationCommand)
            {
                await socketInteraction.GetOriginalResponseAsync()
                    .ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}

