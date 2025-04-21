using Discord.Interactions;

namespace Cinnabar.Modules;

public class FunModule : InteractionModuleBase
{
    [SlashCommand("cat", "Get a picture of a cat.")]
    public async Task Cat()
    {
        
        await RespondAsync("hi");
    }
}