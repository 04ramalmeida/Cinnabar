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
}