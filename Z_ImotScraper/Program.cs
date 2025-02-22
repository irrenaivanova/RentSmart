﻿using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Threading;
using Z_ImotScraper;

class Program
{
    static async Task Main(string[] args)
    {
        IConfiguration config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);

        var imotUrls = new List<string>();
        var properties = new List<PropertyDto>();

        // Number of pages that we will scraper. 11/16/2024 the number of pages with apartments in Sofia are 100.
        int pages = 100;

        int n = 1; // To control what is happening
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();       

        // i+=3 To ensure diversity in property types and districts while browsing properties but limit the number in the  app
        for (int i = 1; i <= pages; i+=3)
        {
            string address = $"https://imoti.info/en/for-rent/grad-sofiya/apartments/page-{i}";
            IDocument document = await context.OpenAsync(address);

            // on every page there are 20 apartments with number of nth-child - 2-21
            for (int j = 2; j <= 21; j++)
            {
                string selector = $"div:nth-child({j}) > app-gallery > div > a.num.ng-star-inserted";
                var elements = document.QuerySelectorAll(selector);
                string? rawUrl = elements.FirstOrDefault()?.OuterHtml.ToString();
                if (rawUrl == null) continue;

                string pattern = @"href=""([^""]+)""";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(rawUrl);
                if (match.Success)
                {
                    var url = $"https://imoti.info{match.Groups[1].Value}";
                    imotUrls.Add(url);
                }
            }
        }

        var tasks = imotUrls.Select(async (url) =>
        {
            var property = await ScrapePropertyDetailsAsync(url, context);
            properties.Add(property);
            Console.WriteLine(n++);

        });

        await Task.WhenAll(tasks);
        stopwatch.Stop();
        TimeSpan time = stopwatch.Elapsed;
        var timeInfo = $"{time.ToString()} --> {properties.Count} Properties";

        Console.WriteLine(properties.Count());
        string json = JsonConvert.SerializeObject(properties, Formatting.Indented);
        string path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..", @"Web\RentSmart.Web\wwwroot\data\properties.json");
        string path2 = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..", @"Web\RentSmart.Web\wwwroot\data\time.txt");
        File.WriteAllText(path, json);
        File.WriteAllText(path2, timeInfo);
    }
    static async Task<PropertyDto> ScrapePropertyDetailsAsync(string url, IBrowsingContext context)
    {
        var property = new PropertyDto();
        property.OriginalUrl = url;

        IDocument document = await context.OpenAsync(url);

        // ImageUrl
        string selectorImage = "div.static.pic.ng-star-inserted > img";
        var elementsImage = document.QuerySelectorAll(selectorImage);
        var rawImage = elementsImage.FirstOrDefault()?.OuterHtml;
        if (rawImage != null)
        {
            string pattern = @"(cdn3([^""]+).jpg|imotstatic([^""]+).jpg)";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(rawImage);
            if (match.Success)
            {
                property.ImageUrl = $"//{match}";
            }
        }

        // PropertyType, City, District, Size
        string selectorInfo = "h1";
        var elementsInfo = document.QuerySelectorAll(selectorInfo);
        var heading = elementsInfo.FirstOrDefault()?.TextContent;
        if (heading != null)
        {
            string[] infos = heading.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            if (infos.Length == 3)
            {
                property.District = infos[1];
                property.Size = double.Parse(infos[2].Replace(" sq.m", string.Empty));
                string[] cityHelp = infos[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                property.City = cityHelp[cityHelp.Length - 1];
                property.PropertyType = FindTheStringBetween(infos[0], "For Rent ", " в");
            }
        }

        // Price
        string selectorPrice = "div.price.ng-star-inserted";
        var elementsPrice = document.QuerySelectorAll(selectorPrice);
        var price = elementsPrice.FirstOrDefault()?.TextContent?.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0];
        if (price != null)
        {
            decimal priceParse;
            if (decimal.TryParse(price, out priceParse))
            {
                property.PricePerMonth = priceParse;
            }
        }

        // Floor
        string selectorFloor = "div:nth-child(7) > span:nth-child(2)";
        var elementsFloor = document.QuerySelectorAll(selectorFloor);
        string? rawFloor = elementsFloor.FirstOrDefault()?.TextContent;
        if (rawFloor != null)
        {
            byte floor;
            if (byte.TryParse(new string(rawFloor.TakeWhile(char.IsDigit).ToArray()), out floor))
            {
                property.Floor = floor;
            }
        }

        //// Description
        //string selectorDesc = "div.description > div.text";
        //var elementsDescr = document.QuerySelectorAll(selectorDesc);
        //var desc = elementsDescr.FirstOrDefault()?.TextContent;
        //if(desc !=null)
        //{
        //    property.Description = desc;
        //} 

        // Tags
        string selectorTags = "div.features.ng-star-inserted > ul > li";
        var elementsTags = document.QuerySelectorAll(selectorTags);
        foreach (var tag in elementsTags)
        {
            property.Tags.Add(tag.TextContent.Trim());
        }
        return property;
    }

    static string FindTheStringBetween(string input, string first, string second)
    {
        int startIndex = input.IndexOf(first) + first.Length;
        int endIndex = input.IndexOf(second, startIndex);
        if (startIndex >= 0 && endIndex > startIndex)
        {
            return input.Substring(startIndex, endIndex - startIndex);
        }
        return null!;
    }
}
