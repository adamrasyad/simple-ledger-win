using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Accounting.ViewModels
{
    public class LedgerRowViewModel
    {
        public Guid LogId { get; set; } // The "Key" to finding the right data
        public DateTime Date { get; set; }
        public string DateText { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public string Tags { get; set; }
        public decimal Balance { get; set; }
        public bool IsMonthSeparator { get; set; }
    }
}
