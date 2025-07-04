using HtmlAgilityPack;
using System.Text.RegularExpressions;

public class LaoScraper
{
    private readonly HttpClient _httpClient = new();
    private const string BaseUrl = "https://expalert.com/backward/laosdevelops";

    public async Task<List<LotteryResult>> ScrapeHistoricalData()
    {
        var results = new List<LotteryResult>();
        string nextPageUrl = BaseUrl;

        while (!string.IsNullOrEmpty(nextPageUrl))
        {
            var html = await _httpClient.GetStringAsync(nextPageUrl);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var rows = doc.DocumentNode.SelectNodes("//div[contains(@class, 'mantine-Grid-root')]");
            if (rows == null) break;

            foreach (var row in rows)
            {
                var cols = row.SelectNodes(".//div[contains(@class, 'mantine-Grid-col')]");
                if (cols == null || cols.Count < 3) continue;

                var date = cols[0].InnerText.Trim();
                var firstPrize = cols[1].InnerText.Trim();

                if (firstPrize == "งดออกผล") continue;

                var match = Regex.Match(firstPrize, @"\d{3}");
                if (match.Success)
                {
                    results.Add(new LotteryResult {
                        Date = date,
                        Number = match.Value,
                        Type = "Lao"
                    });
                }
            }

            var nextButton = doc.DocumentNode.SelectSingleNode("//a[@title='ผลหวยลาวพัฒนาย้อนหลัง หน้าต่อไป']");
            nextPageUrl = nextButton?.GetAttributeValue("href", null);
        }

        return results;
    }
}