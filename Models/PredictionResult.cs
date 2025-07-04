public class PredictionResult
{
    public string PredictedNumber { get; set; }
    public double Confidence { get; set; }
    public string Methodology { get; set; }
    public List<LotteryResult> HistoricalData { get; set; }
}