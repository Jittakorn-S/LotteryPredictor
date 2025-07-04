using HtmlAgilityPack;
using System.Text;

public class ThaiScraper
{
    private readonly HttpClient _httpClient = new();
    private const string BaseUrl = "https://news.sanook.com/lotto/archive/";

    public async Task<List<LotteryResult>> ScrapeHistoricalData()
    {
        var results = new List<LotteryResult>();
        string nextPageUrl = BaseUrl;

        while (!string.IsNullOrEmpty(nextPageUrl))
        {
            var html = await _httpClient.GetStringAsync(nextPageUrl);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var articles = doc.DocumentNode.SelectNodes("//article[contains(@class, 'archive--lotto')]");
            if (articles == null) break;

            foreach (var article in articles)
            {
                var dateNode = article.SelectSingleNode(".//time");
                var date = dateNode?.GetAttributeValue("datetime", "").Trim();

                var prizeNodes = article.SelectNodes(".//ul[contains(@class, 'archive--lotto__result-list')]/li");
                if (prizeNodes == null) continue;

                foreach (var li in prizeNodes)
                {
                    var label = li.SelectSingleNode(".//em")?.InnerText.Trim();
                    if (label?.Contains("รางวัลที่ 1") == true)
                    {
                        var number = li.SelectSingleNode(".//strong")?.InnerText.Trim();
                        if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(number))
                        {
                            results.Add(new LotteryResult {
                                Date = date,
                                Number = number,
                                Type = "Thai"
                            });
                        }
                    }
                }
            }

            var nextButton = doc.DocumentNode.SelectSingleNode("//a[contains(@class, 'pagination__item--next')]");
            nextPageUrl = nextButton?.GetAttributeValue("href", null);
        }

        return results;
    }
}