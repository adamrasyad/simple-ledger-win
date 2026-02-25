using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Accounting.Domain
{
    public class Log
    {
        public Guid Id { get; } = Guid.NewGuid(); // Auto-generates a unique ID
        public DateTime Date { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        //log.Tags = new HashSet<string>(
        //    input.Split(',')
        //     .Select(t => t.Trim().ToLower()),
        //StringComparer.OrdinalIgnoreCase);

    }

}
