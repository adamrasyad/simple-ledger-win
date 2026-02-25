using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Accounting.Domain
{
    public class Ledger
    {
        public List<MonthLog> Months { get; set; } = new List<MonthLog>();
        public decimal TotalBalance =>
        Months.Sum(m => m.Balance);

        public MonthLog GetLogAtMonth(DateTime month)
        {
            return Months.FirstOrDefault(m =>
                m.Month.Year == month.Year &&
                m.Month.Month == month.Month);
        }
        //public List<Log> GetLogsByTagsAll(IEnumerable<string> queryTags)
        //{
        //    return Months
        //        .SelectMany(m => m.Logs)
        //        .Where(log => queryTags.All(qTag =>
        //            log.Tags.Any(lTag => lTag.Equals(qTag, StringComparison.OrdinalIgnoreCase))))
        //        .ToList();
        //}
        public MonthLog GetOrCreateMonth(DateTime date)
        {
            var month = Months.FirstOrDefault(m =>
                m.Month.Year == date.Year &&
                m.Month.Month == date.Month);

            if (month == null)
            {
                month = new MonthLog
                {
                    Month = new DateTime(date.Year, date.Month, 1)
                };
                Months.Add(month);
            }

            return month;
        }
        public void UpdateLogDate(Log log, DateTime newDate)
        {
            // 1. Remove it from wherever it is now
            foreach (var m in Months)
            {
                if (m.Logs.Contains(log))
                {
                    m.Logs.Remove(log);
                    break;
                }
            }

            // 2. Update the date
            log.Date = newDate;

            // 3. Put it in the correct (potentially new) month
            var targetMonth = GetOrCreateMonth(newDate);
            targetMonth.Logs.Add(log);
        }

    }

}
