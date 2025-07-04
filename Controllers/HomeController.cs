using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly ThaiScraper _thaiScraper;
    private readonly LaoScraper _laoScraper;
    private readonly LotteryAnalyzer _analyzer;

    public HomeController()
    {
        _thaiScraper = new ThaiScraper();
        _laoScraper = new LaoScraper();
        _analyzer = new LotteryAnalyzer();
    }

    public async Task<IActionResult> Index()
    {
        var thaiData = await _thaiScraper.ScrapeHistoricalData();
        var laoData = await _laoScraper.ScrapeHistoricalData();

        var allData = thaiData.Concat(laoData).ToList();
        var prediction = _analyzer.PredictNextNumber(allData);

        return View(prediction);
    }
}