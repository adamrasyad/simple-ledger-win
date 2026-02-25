using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Accounting.Domain
{
    public class StatisticsResult
    {
        public SortedDictionary<DateTime, decimal> MonthlyNet { get; set; }

        public SortedDictionary<DateTime, decimal> MonthlyInflow { get; set; }
        public SortedDictionary<DateTime, decimal> MonthlyOutflow { get; set; }
        public SortedDictionary<DateTime, decimal> YearlyNet { get; set; }
        public SortedDictionary<DateTime, decimal> YearlyInflow { get; set; }
        public SortedDictionary<DateTime, decimal> YearlyOutflow { get; set; }

        public Dictionary<string, decimal> TagSpend { get; set; }

        public decimal MaxExpense { get; set; }
        public decimal MaxIncome { get; set; }

        public List<string> MaxExpenseTags { get; set; }
        public List<string> MaxIncomeTags { get; set; }
    }
}
