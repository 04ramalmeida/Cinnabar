using Discord;
using Discord.Interactions;

namespace Cinnabar;

public class EmbedBase : InteractionModuleBase
{
    public Embed CinnabarEmbed(string authorTitle,
        string? description,
        string? imageUrl,
        EmbedFieldBuilder[]? fields,
        IUser user)
    {
        Embed embed;
        if (fields == null)
        {
            embed = new EmbedBuilder() 
                .WithAuthor(authorTitle, iconUrl: imageUrl)
                .WithDescription(description)
                .WithThumbnailUrl(imageUrl)
                .WithColor(new Color(255, 5, 59))
                .WithFooter($"Command ran by {user.Username}")
                .WithCurrentTimestamp()
                .Build();
        }
        else if (description == null)
        {
            embed = new EmbedBuilder() 
                .WithAuthor(authorTitle, iconUrl: imageUrl)
                .WithFields(fields)
                .WithThumbnailUrl(imageUrl)
                .WithColor(new Color(255, 5, 59))
                .WithFooter($"Command ran by {user.Username}")
                .WithCurrentTimestamp()
                .Build();
        }
        else
        {
            embed = new EmbedBuilder() 
                .WithAuthor(authorTitle, iconUrl: imageUrl)
                .WithDescription(description)
                .WithFields(fields)
                .WithThumbnailUrl(imageUrl)
                .WithColor(new Color(255, 5, 59))
                .WithFooter($"Command ran by {user.Username}")
                .WithCurrentTimestamp()
                .Build();
        }
        
        return embed;
    }
}