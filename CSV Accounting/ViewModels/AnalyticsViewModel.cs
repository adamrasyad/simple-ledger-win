using CSV_Accounting.Domain;
using CSV_Accounting.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Accounting.ViewModels
{
    public class AnalyticsViewModel
    {
        public List<string> AvailableTags { get; set; }
        public SortedDictionary<DateTime, decimal> MonthlyNetSeries { get; set; }
        public SortedDictionary<DateTime, decimal> MonthlyInflowSeries { get; set; }
        public SortedDictionary<DateTime, decimal> MonthlyOutflowSeries { get; set; }
        public SortedDictionary<DateTime, decimal> AnnualNetSeries { get; set; }
        public SortedDictionary<DateTime, decimal> AnnualInflowSeries { get; set; }
        public SortedDictionary<DateTime, decimal> AnnualOutflowSeries { get; set; }
        // Add this to store the "Highest Spend" insight data
        public (int Month, int Year, List<string> Tags)? MaxSpendInsight { get; set; }

        // Add this to store the ratios for the "Last 3 Months" insight
        public Dictionary<string, decimal> TagRatios { get; set; }
        public AnalyticsViewModel Build(List<Log> logs)
        {
            LedgerAnalyticsService service = new LedgerAnalyticsService();
            var result = service.Calculate(logs);
            if(result==null)
                    return null;
            AvailableTags = result.TagSpend.Keys.ToList();
            return new AnalyticsViewModel
            {
                AvailableTags = result.TagSpend.Keys.ToList(),
                MonthlyNetSeries = result.MonthlyNet,
                MonthlyInflowSeries = result.MonthlyInflow,
                MonthlyOutflowSeries = result.MonthlyOutflow,
                AnnualNetSeries = result.YearlyNet,
                AnnualInflowSeries = result.YearlyInflow,
                AnnualOutflowSeries = result.YearlyOutflow,
                MaxSpendInsight = service.GetDateTagFromMaxSpendLog(logs),
                TagRatios = service.CalculateTag3MonthTrend(logs)
            };
        }
    }
}
