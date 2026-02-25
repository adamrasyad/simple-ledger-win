using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSV_Accounting
{
    public interface IPage
    {
        event EventHandler BackRequested;
        void UpdateLanguage();
        void UpdateTheme(bool isNightMode);
        void UpdateAccountingTerms(bool useAccountingTerms); // New
        bool SaveData();
        void HandleShortcut(Keys keyData);
    }
}
