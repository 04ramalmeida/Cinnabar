using System.Text.Json.Serialization;

namespace Cinnabar.Models;


public class AlbumRootObject
{
    public Album Album { get; set; }
}

public class Album
{
    public string Artist { get; set; }
    public string Mbid { get; set; }
    [JsonPropertyName("tags")]
    public TagsRootObject TagsRoot { get; set; }
    public string Playcount { get; set; }
    [JsonPropertyName("image")]
    public Image[] Images { get; set; }
    [JsonPropertyName("tracks")]
    public TracksRootObject TracksRoot { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public string Listeners { get; set; }
    public Wiki Wiki { get; set; }
}


public class TagsRootObject
{
    [JsonPropertyName("tag")]
    public Tag[] Tags { get; set; }
}

public class Tag
{
    public string Url { get; set; }
    public string Name { get; set; }
}

public class Image
{
    public string Size { get; set; }
    [JsonPropertyName("#text")]
    public string ImageUrl { get; set; }
}

public class TracksRootObject
{
    public Track[] Track { get; set; }
}

public class Track
{
    public Streamable Streamable { get; set; }
    public int Duration { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    [JsonPropertyName("#attr")]
    public Attr Attr { get; set; }
    public Artist Artist { get; set; }
}

public class Streamable
{
    public string Fulltrack { get; set; }
    [JsonPropertyName("#text")]
    public string Text { get; set; }
}

public class Attr
{
    public int Rank { get; set; }
}


public class Wiki
{
    public string Published { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
}

public class ArtistRootObject
{
    public Artist artist { get; set; }
}

public class Artist
{
    public string Name { get; set; }
    public string Mbid { get; set; }
    public string Url { get; set; }
    public Image[] Image { get; set; }
    public string Streamable { get; set; }
    public string Ontour { get; set; }
    public Stats Stats { get; set; }
    public Similar Similar { get; set; }
    [JsonPropertyName("tags")]
    public TagsRootObject TagsRoot { get; set; }
    public Bio Bio { get; set; }
}



public class Stats
{
    public string Listeners { get; set; }
    public string Playcount { get; set; }
}

public class Similar
{
    public Artist[] Artist { get; set; }
}

public class Bio
{
    public Links Links { get; set; }
    public string Published { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
}

public class Links
{
    public Link Link { get; set; }
}

public class Link
{
    [JsonPropertyName("#text")]
    public string Text { get; set; }
    public string Rel { get; set; }
    public string Href { get; set; }
}