using AngleSharp;
using AngleSharp.Dom;
using System.Text.RegularExpressions;
using System.Web;

IConfiguration config = Configuration.Default.WithDefaultLoader();
var context = BrowsingContext.New(config);

List<string> imotUrls = new List<string>();

// Number of pages that we will scraper. 11/16/2024 the number of pages with apartments in Sofia are 100.
int pages = 3;

for (int i = 2; i <= pages; i++)
{
    string address = $"https://imoti.info/en/for-rent/grad-sofiya/apartments/page-{i}";
    IDocument document = await context.OpenAsync(address);

    // on every page there are 20 apartments with number of nth-child - 2-21
    for (int j = 2; j <= 21; j++)
    {
        string cellSelector = $"body > app-root > div > app-results > div.pageBgr > div > div" +
            $" > div.results.ng-star-inserted > div:nth-child({j}) > app-gallery > div > a.num.ng-star-inserted";
        var elements = document.QuerySelectorAll(cellSelector);
        foreach (var element in elements)
        {
            string rawUrl = element.OuterHtml.ToString();
            string pattern = @"href=""([^""]+)"""; ;
            Regex regex = new Regex(pattern);
            Match match = regex.Match(rawUrl);
            var url = $"https://imoti.info{match.Groups[1].Value}";
            imotUrls.Add(url);
        }
    }
}






Console.WriteLine(string.Join('\n', imotUrls));
Console.WriteLine(imotUrls.Count);

