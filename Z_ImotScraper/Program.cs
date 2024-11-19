using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Z_ImotScraper;

IConfiguration config = Configuration.Default.WithDefaultLoader();
var context = BrowsingContext.New(config);

List<string> imotUrls = new List<string>();
List<PropertyDto> properties = new List<PropertyDto>();


// Number of pages that we will scraper. 11/16/2024 the number of pages with apartments in Sofia are 100.
int pages = 2;

for (int i = 2; i <= pages; i++)
{
    string address = $"https://imoti.info/en/for-rent/grad-sofiya/apartments/page-{i}";
    IDocument document = await context.OpenAsync(address);

    // on every page there are 20 apartments with number of nth-child - 2-21
    for (int j = 2; j <= 21; j++)
    {
        string selector = $"div:nth-child({j}) > app-gallery > div > a.num.ng-star-inserted";
        var elements = document.QuerySelectorAll(selector);
        string rawUrl = elements.FirstOrDefault()!.OuterHtml.ToString();
        string pattern = @"href=""([^""]+)""";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(rawUrl);
        var url = $"https://imoti.info{match.Groups[1].Value}";
        imotUrls.Add(url);
    }
}

int n = 1;
foreach (var url in imotUrls)
{
    var property = new PropertyDto();
    property.OriginalUrl = url;

    // ImageUrl
    IDocument document = await context.OpenAsync(url);
    string selectorImage = "div.static.pic.ng-star-inserted > img";
    var elementsImage = document.QuerySelectorAll(selectorImage);
    var rawImage = elementsImage.FirstOrDefault()!.OuterHtml;
    string pattern = @"(cdn3([^""]+).jpg|imotstatic([^""]+).jpg)";
    Regex regex = new Regex(pattern);
    Match match = regex.Match(rawImage);
    var imageUrl = $"//{match}";
    property.ImageUrl = imageUrl;

    // PropertyType, City, District, Size
    string selectorInfo = "h1";
    var elementsInfo = document.QuerySelectorAll(selectorInfo);
    var heading = elementsInfo.FirstOrDefault()!.TextContent;
    string[] infos = heading.Split(", ", StringSplitOptions.RemoveEmptyEntries);

    property.District = infos[1];
    property.Size = double.Parse(infos[2].Replace(" sq.m", string.Empty));
    string[] cityHelp = infos[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
    property.City = cityHelp[cityHelp.Length - 1];
    property.PropertyType = FindTheStringBetween(infos[0], "For Rent ", " в");

    // Price
    string selectorPrice = "div.price.ng-star-inserted";
    var elementsPrice = document.QuerySelectorAll(selectorPrice);
    var price = decimal.Parse(elementsPrice.FirstOrDefault()!.TextContent.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0]);
    property.PricePerMonth = price;

    // Floor
    string selectorFloor = "div:nth-child(7) > span:nth-child(2)";
    var elementsFloor = document.QuerySelectorAll(selectorFloor);
    string rawFloor = elementsFloor.FirstOrDefault()!.TextContent;
    byte floor = byte.Parse(new string(rawFloor.TakeWhile(char.IsDigit).ToArray()));
    property.Floor = floor;

    // Description
    string selectorDesc = "div.description > div.text";
    var elementsDescr = document.QuerySelectorAll(selectorDesc);
    property.Description = elementsDescr.FirstOrDefault()!.TextContent;

    // Tags
    string selectorTags = "div.features.ng-star-inserted > ul > li";
    var elementsTags = document.QuerySelectorAll(selectorTags);
    foreach (var tag in elementsTags)
    {
       property.Tags.Add(tag.TextContent.Trim());
    }

    properties.Add(property);
    Console.WriteLine(n++);
}


string json = JsonConvert.SerializeObject(properties, Formatting.Indented);
string path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..", @"Web\RentSmart.Web\wwwroot\data\properties.json");
File.WriteAllText(path, json);



//foreach (var prop in properties)
//{
//    Console.WriteLine(prop.OriginalUrl);
//    Console.WriteLine(prop.ImageUrl);
//    Console.WriteLine(prop.PropertyType);
//    Console.WriteLine(prop.City);
//    Console.WriteLine(prop.District);
//    Console.WriteLine(prop.Size);
//    Console.WriteLine(prop.PricePerMonth);
//    Console.WriteLine(prop.Floor);
//    Console.WriteLine(prop.Description);
//    Console.WriteLine(new string('-', 20));
//}

string FindTheStringBetween(string input, string first, string second)
{
    int startIndex = input.IndexOf(first) + first.Length;
    int endIndex = input.IndexOf(second,startIndex);
    if (startIndex>=0 && endIndex>startIndex)
    {
        return input.Substring(startIndex, endIndex - startIndex);
    }
    return null!;
}
