using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Accounting.Domain
{
    public class MonthLog
    {
        public DateTime Month { get; set; }
        public List<Log> Logs { get; set; } = new List<Log>();
        public decimal Balance =>
        Logs.Sum(l => l.Debit - l.Credit);
    }

}
