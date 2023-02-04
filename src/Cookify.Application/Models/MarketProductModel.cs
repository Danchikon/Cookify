namespace Cookify.Application.Models;

public class MarketProductModel
{
    public string Name { get; set; }
    public string Link { get; set; }
    public uint Count { get; set; }

    public MarketProductModel(string name, string link, uint count)
    {
        Name = name;
        Link = link;
        Count = count;
    }
}