using CSV_Accounting.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Accounting.Services
{
    public class CsvService
    {
        public Ledger Load(string path)
        {
            Ledger ledger = new Ledger();

            using (StreamReader reader = new StreamReader(path))
            {
                string header = reader.ReadLine(); // skip header

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    DateTime date = DateTime.ParseExact(
                        values[0],
                        "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture
                    );
                    decimal debit = decimal.Parse(
                        values[1],
                        System.Globalization.CultureInfo.InvariantCulture
                    );
                    decimal credit = decimal.Parse(
                        values[2],
                        System.Globalization.CultureInfo.InvariantCulture
                    );
                    //string description = values[3].Replace("|", ", ");
                    //string reference = values[4].Replace("|", ", ");
                    //List<string> tags = values[5].Split('|').ToList();
                    
                    // Ensure minimum column count
                    Array.Resize(ref values, 6);

                    string description = values[3].Replace("|", ", ") ?? "";
                    string reference = values[4].Replace("|", ", ") ?? "";

                    List<string> tags = new List<string>();
                    if (!string.IsNullOrWhiteSpace(values[5]))
                    {
                        tags = values[5].Split('|').ToList();
                    }

                    Log log = new Log
                    {
                        Date = date,
                        Debit = debit,
                        Credit = credit,
                        Description = description,
                        Reference = reference,
                        Tags = tags
                    };

                    MonthLog month = ledger.GetOrCreateMonth(date);
                    month.Logs.Add(log);
                }
            }

            return ledger;
        }

        public void Save(string path, Ledger ledger)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("Date,Debit,Credit,Description,Reference,Tags");

                // Sort by date to ensure the CSV is organized
                var allLogs = ledger.Months
                    .SelectMany(m => m.Logs)
                    .OrderBy(l => l.Date);

                foreach (var log in allLogs)
                {
                    // Join tags with '|' to match your Load logic

                    string description = SanitizeText(log.Description);
                    string reference = SanitizeText(log.Reference);
                    string tags = string.Join("|", log.Tags);

                    writer.WriteLine(
                        $"{log.Date:yyyy-MM-dd}," +
                        $"{log.Debit.ToString(System.Globalization.CultureInfo.InvariantCulture)}," +
                        $"{log.Credit.ToString(System.Globalization.CultureInfo.InvariantCulture)}," +
                        $"{description}," +
                        $"{reference}," +
                        $"{tags}"
                    );
                }
            }
        }

        private string SanitizeText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            return text.Replace(",", "|");
        }
    }

}
