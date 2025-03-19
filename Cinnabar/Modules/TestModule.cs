using Discord.Interactions;

namespace Cinnabar.Modules;

public class TestModule : InteractionModuleBase
{
    [SlashCommand("echo", "Echo an imput")]
    public async Task Echo(string input)
    {
        await RespondAsync(input);
    }
    
    
}