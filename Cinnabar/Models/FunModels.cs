using System.Text.Json.Serialization;

namespace Cinnabar.Models;

public class Cat
{
    
    public string Id { get; set; }
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class Dog
{
    public string Message  { get; set; }
    public string Status { get; set; }
}

public class Fox
{
    public string Image { get; set; }
    public string Link { get; set; }
}

public class Weather
{
    public string Temperature { get; set; }
    public string Wind  { get; set; }
    public string Description { get; set; }
    public Forecast[] Forecast { get; set; }
}

public class Forecast
{
    public string Day { get; set; }
    public string Temperature { get; set; }
    public string Wind { get; set; }
}

public class DictionaryDef
{
    public string Word { get; set; }
    public string Phonetic { get; set; }
    public Phonetic[] Phonetics { get; set; }
    public string Origin { get; set; }
    public required Meaning[] Meanings { get; set; }
}

public class Phonetic
{
    public string? Text  { get; set; }
    public string? Audio { get; set; }
}

public class Meaning
{
    public required string PartOfSpeech  { get; set; }
    public required Definitions[] Definitions { get; set; }
}

public class Definitions
{
    public string Definition { get; set; }
    public string Example { get; set; }
    public string[] Synonyms { get; set; }
    public string[] Antonyms { get; set; }
}

public class AlbumRootObject
{
    public Album Album { get; set; }
}

public class Album
{
    public string Artist { get; set; }
    public string Mbid { get; set; }
    public Tags Tags { get; set; }
    public string Playcount { get; set; }
    public Image[] Image { get; set; }
    public Tracks Tracks { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public string Listeners { get; set; }
    public Wiki Wiki { get; set; }
}

public class Tags
{
    public Tag[] Tag { get; set; }
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

public class Tracks
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

public class Artist
{
    public string Url { get; set; }
    public string Name { get; set; }
    public string Mbid { get; set; }
}

public class Wiki
{
    public string Published { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
}

