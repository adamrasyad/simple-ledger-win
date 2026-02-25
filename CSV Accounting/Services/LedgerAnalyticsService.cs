using CSV_Accounting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Accounting.Services
{
    public class LedgerAnalyticsService
    {
        public StatisticsResult Calculate(List<Log> logs)
        {
            if (logs == null || !logs.Any())
                return null;

            logs = logs.OrderBy(l => l.Date).ToList();

            //--------------------------------
            // MONTHLY
            //--------------------------------
            var monthlyNet = new SortedDictionary<DateTime, decimal>(
                logs.GroupBy(l => new DateTime(l.Date.Year, l.Date.Month, 1))
                    .ToDictionary(g => g.Key, g => g.Sum(l => l.Debit - l.Credit)));

            var monthlyInflow = new SortedDictionary<DateTime, decimal>(
                logs.Where(l => l.Debit > 0)
                    .GroupBy(l => new DateTime(l.Date.Year, l.Date.Month, 1))
                    .ToDictionary(g => g.Key, g => g.Sum(l => l.Debit)));

            var monthlyOutflow = new SortedDictionary<DateTime, decimal>(
                logs.Where(l => l.Credit > 0)
                    .GroupBy(l => new DateTime(l.Date.Year, l.Date.Month, 1))
                    .ToDictionary(g => g.Key, g => g.Sum(l => l.Credit)));

            //--------------------------------
            // YEARLY
            //--------------------------------
            var yearlyNet = new SortedDictionary<DateTime, decimal>(
                logs.GroupBy(l => new DateTime(l.Date.Year, 12, 31))
                    .ToDictionary(g => g.Key, g => g.Sum(l => l.Debit - l.Credit)));

            var yearlyInflow = new SortedDictionary<DateTime, decimal>(
                logs.Where(l => l.Debit > 0)
                    .GroupBy(l => new DateTime(l.Date.Year, 12, 31))
                    .ToDictionary(g => g.Key, g => g.Sum(l => l.Debit)));

            var yearlyOutflow = new SortedDictionary<DateTime, decimal>(
                logs.Where(l => l.Credit > 0)
                    .GroupBy(l => new DateTime(l.Date.Year, 12, 31))
                    .ToDictionary(g => g.Key, g => g.Sum(l => l.Credit)));

            //--------------------------------
            // TAG ANALYTICS
            //--------------------------------
            var tagSpend = logs
                .Where(l => l.Credit > 0)
                .SelectMany(l => l.Tags.Select(t => new { Tag = t, Amount = l.Credit }))
                .GroupBy(x => x.Tag)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

            //--------------------------------
            // EXTREME VALUES
            //--------------------------------
            var maxExpense = logs.Any(l => l.Credit > 0)
                ? logs.Where(l => l.Credit > 0).Max(l => l.Credit)
                : 0;

            var maxIncome = logs.Any(l => l.Debit > 0)
                ? logs.Where(l => l.Debit > 0).Max(l => l.Debit)
                : 0;

            var maxExpenseTags = logs
                .Where(l => l.Credit == maxExpense)
                .SelectMany(l => l.Tags)
                .Distinct()
                .ToList();

            var maxIncomeTags = logs
                .Where(l => l.Debit == maxIncome)
                .SelectMany(l => l.Tags)
                .Distinct()
                .ToList();

            //--------------------------------
            // RESULT
            //--------------------------------
            return new StatisticsResult
            {
                MonthlyNet = monthlyNet,
                MonthlyInflow = monthlyInflow,
                MonthlyOutflow = monthlyOutflow,

                YearlyNet = yearlyNet,
                YearlyInflow = yearlyInflow,
                YearlyOutflow = yearlyOutflow,

                TagSpend = tagSpend,

                MaxExpense = maxExpense,
                MaxIncome = maxIncome,
                MaxExpenseTags = maxExpenseTags,
                MaxIncomeTags = maxIncomeTags
            };
        }

        private (double slope, double intercept) CalculateLinearRegression(IEnumerable<decimal> values)
        {
            var yValues = values.Select(v => (double)v).ToArray();
            var xValues = Enumerable.Range(1, yValues.Length).Select(x => (double)x).ToArray();
            int n = yValues.Length;

            double sumX = xValues.Sum();
            double sumY = yValues.Sum();
            double sumXY = xValues.Zip(yValues, (x, y) => x * y).Sum();
            double sumXSq = xValues.Sum(x => x * x);

            double slope = (n * sumXY - sumX * sumY) / (n * sumXSq - sumX * sumX);
            double intercept = (sumY - slope * sumX) / n;

            return (slope, intercept);
        }

        public Dictionary<string, decimal> CalculateTag3MonthTrend(List<Log> logs)
        {
            var result = new Dictionary<string, decimal>();

            if (logs == null || !logs.Any())
                return result;

            //--------------------------------
            // Last recorded month (NOT DateTime.Now)
            //--------------------------------
            var lastDate = logs.Max(l => l.Date);
            var startDate = new DateTime(lastDate.Year, lastDate.Month, 1).AddMonths(-2);

            //--------------------------------
            // Total spend per tag (all time)
            //--------------------------------
            var totalTagSpend = logs
                .Where(l => l.Credit > 0)
                .SelectMany(l => l.Tags.Select(t => new { Tag = t, Amount = l.Credit }))
                .GroupBy(x => x.Tag)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

            //--------------------------------
            // Last 3 months spend per tag
            //--------------------------------
            var last3TagSpend = logs
                .Where(l => l.Credit > 0 && l.Date >= startDate)
                .SelectMany(l => l.Tags.Select(t => new { Tag = t, Amount = l.Credit }))
                .GroupBy(x => x.Tag)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

            //--------------------------------
            // Ratio calculation
            //--------------------------------
            foreach (var tag in totalTagSpend.Keys)
            {
                decimal total = totalTagSpend[tag];

                decimal last3 = last3TagSpend.ContainsKey(tag)
                    ? last3TagSpend[tag]
                    : 0;

                decimal ratio = total == 0 ? 0 : last3 / total;

                result[tag] = ratio;
            }

            return result;
        }


        public (int Month, int Year, List<string> Tags)? GetDateTagFromMaxSpendLog(List<Log> logs)
        {
            var highestMonth = logs
                .GroupBy(l => new { l.Date.Year, l.Date.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Spend = g.Sum(x => x.Credit),
                    Logs = g.ToList()
                })
                .OrderByDescending(x => x.Spend)
                .FirstOrDefault();

            if (highestMonth == null)
                return null;

            var topTags = highestMonth.Logs
                .SelectMany(l => l.Tags)
                .GroupBy(t => t)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key)
                .ToList();

            return (highestMonth.Month, highestMonth.Year, topTags);
        }

        public string getTagFromMaxIncomeLog(List<Log> logs)
        {
            var highestMonth = logs
                .GroupBy(l => new { l.Date.Year, l.Date.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Income = g.Sum(x => x.Debit),
                    Logs = g.ToList()
                })
                .OrderByDescending(x => x.Income)
                .FirstOrDefault();
            if (highestMonth == null)
                return "No income data.";
            var topTags = highestMonth.Logs
                .SelectMany(l => l.Tags)
                .GroupBy(t => t)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key);
            return $"You earn the most in {highestMonth.Month}/{highestMonth.Year} " +
                   $"mostly from {string.Join(", ", topTags)}.";
        }

        public string getOverallInsight(List<Log> logs)
        {
            var highestMonth = logs
                .GroupBy(l => new { l.Date.Year, l.Date.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Net = g.Sum(x => x.Debit - x.Credit),
                    Logs = g.ToList()
                })
                .OrderByDescending(x => x.Net)
                .FirstOrDefault();
            if (highestMonth == null)
                return "No financial data.";
            var topTags = highestMonth.Logs
                .SelectMany(l => l.Tags)
                .GroupBy(t => t)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key);
            return $"Your best month is {highestMonth.Month}/{highestMonth.Year} " +
                   $"with a net of {highestMonth.Net:C}, mostly influenced by {string.Join(", ", topTags)}.";
        }

        public string getOverallWorstInsight(List<Log> logs)
        {
            var lowestMonth = logs
                .GroupBy(l => new { l.Date.Year, l.Date.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Net = g.Sum(x => x.Debit - x.Credit),
                    Logs = g.ToList()
                })
                .OrderBy(x => x.Net)
                .FirstOrDefault();
            if (lowestMonth == null)
                return "No financial data.";
            var topTags = lowestMonth.Logs
                .SelectMany(l => l.Tags)
                .GroupBy(t => t)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key);
            return $"Your worst month is {lowestMonth.Month}/{lowestMonth.Year} " +
                   $"with a net of {lowestMonth.Net:C}, mostly influenced by {string.Join(", ", topTags)}.";
        }


        //private double CalculateVariance(List<double> values)
        //{
        //    double avg = values.Average();
        //    return values.Sum(v => Math.Pow(v - avg, 2)) / values.Count;
        //}
    }

}
