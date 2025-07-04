using System.Globalization;

public class LotteryAnalyzer
{
    public PredictionResult PredictNextNumber(List<LotteryResult> historicalData)
    {
        // Simplified prediction logic (replace with actual AI framework)
        var numbers = historicalData
            .Select(x => int.Parse(x.Number))
            .ToList();

        // Frequency analysis
        var digitFrequency = new int[10];
        foreach (var num in numbers)
        {
            digitFrequency[num / 100]++;     // Hundreds
            digitFrequency[(num / 10) % 10]++; // Tens
            digitFrequency[num % 10]++;        // Units
        }

        // Predict most probable digits
        int hundreds = Array.IndexOf(digitFrequency, digitFrequency.Max());
        int tens = Array.IndexOf(digitFrequency, digitFrequency.Max());
        int units = Array.IndexOf(digitFrequency, digitFrequency.Max());

        return new PredictionResult
        {
            PredictedNumber = $"{hundreds}{tens}{units}",
            Confidence = 65.7, // Placeholder value
            Methodology = "Digit Frequency Analysis",
            HistoricalData = historicalData
        };
    }
}