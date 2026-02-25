using CSV_Accounting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Accounting.Helper
{
    public class LedgerState
    {
        public string FilePath { get; set; } = string.Empty;
        public bool IsDirty { get; set; } = false; // "askToSave"
        public Ledger CurrentLedger { get; set; } = new Ledger();

        public void MarkSaved() => IsDirty = false;
        public void MarkDirty() => IsDirty = true;
    }
}
