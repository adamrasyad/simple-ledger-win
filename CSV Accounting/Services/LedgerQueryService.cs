using CSV_Accounting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Accounting.Services
{
    public class LedgerQueryService
    {
        // Specific logic for Tag filtering
        public List<Log> FilterByTag(Ledger ledger, string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return GetAllLogs(ledger);

            return ledger.Months
                .SelectMany(m => m.Logs)
                .Where(l => l.Tags.Any(t => t.Equals(tag.Trim(), StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        // General Search Router
        public List<Log> SearchByKeyword(Ledger ledger, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetAllLogs(ledger);

            // COUPLING: Route to FilterByTag if the user uses the "Tag:" prefix
            if (keyword.StartsWith("Tag:", StringComparison.OrdinalIgnoreCase))
            {
                string tag = keyword.Substring(4);
                return FilterByTag(ledger, tag); // Reuse the specific method
            }

            // Default: Broad search across multiple fields
            string k = keyword.ToLower();
            return ledger.Months
                .SelectMany(m => m.Logs)
                .Where(l =>
                    (l.Description?.ToLower().Contains(k) ?? false) ||
                    (l.Reference?.ToLower().Contains(k) ?? false) ||
                    (l.Tags.Any(t => t.ToLower().Contains(k)))
                ).ToList();
        }

        public List<Log> GetAllLogs(Ledger ledger) => ledger.Months.SelectMany(m => m.Logs).ToList();
    }
}
