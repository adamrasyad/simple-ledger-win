using CSV_Accounting.Domain;
using CSV_Accounting.Helper;
using CSV_Accounting.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace CSV_Accounting.Controls
{
    public partial class UCMainMenu : UserControl, IPage
    {
        // 1. The Signals (Events)
        public event EventHandler OpenLedgerRequested;
        public event EventHandler OpenAboutRequested;
        public event EventHandler ImportCsvRequested;
        public event EventHandler BackRequested; // Satisfies IPage, though not used here

        private LedgerState _state;
        public UCMainMenu(LedgerState state)
        {
            InitializeComponent();
            _state = state;
        }

        // 2. The Logic (Satisfying IPage)
        public void UpdateLanguage()
        {
            labelWelcome.Text = Resources.Strings.Lbl_Welcome;
            buttonOpenLedger.Text = Resources.Strings.Btn_OpenTable;
            buttonAbout.Text = Resources.Strings.Btn_About;
        }
        private bool _isNightMode = false;
        public void UpdateTheme(bool isNightMode)
        {
            _isNightMode = isNightMode;

            // 1. Define Professional Palette
            Color bgColor = isNightMode ? Color.FromArgb(32, 32, 32) : SystemColors.Control;
            Color fontColor = isNightMode ? Color.White : Color.Black;
            Color accentColor = isNightMode ? Color.FromArgb(23, 23, 23) : Color.White;

            // 2. Style the UserControl itself
            this.BackColor = bgColor;
            this.ForeColor = fontColor;

            // 3. Simple loop for top-level controls
            foreach (Control c in this.Controls)
            {
                if (c is Button btn)
                {
                    btn.BackColor = accentColor;
                    btn.ForeColor = fontColor;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = isNightMode ? Color.DimGray : Color.DarkGray;
                }
                else if (c is Label lbl)
                {
                    lbl.ForeColor = fontColor;
                }
                else if (c is DataGridView dgv)
                {
                    dgv.BackgroundColor = bgColor;
                    dgv.DefaultCellStyle.BackColor = accentColor;
                    dgv.DefaultCellStyle.ForeColor = fontColor;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = bgColor;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = fontColor;
                    dgv.EnableHeadersVisualStyles = false;
                }
            }

            this.Invalidate();
        }

        public void HandleShortcut(Keys keyData) { /* Menu shortcuts like Ctrl+O if you want */ }
        private CsvService _csvService = new CsvService();
        public bool SaveData()
        {
            // If we have a valid path (from Import or a previous Save As), save directly
            if (!string.IsNullOrEmpty(_state.FilePath) && File.Exists(_state.FilePath))
            {
                try
                {
                    // Use the Service to handle the logic
                    _csvService.Save(_state.FilePath, _state.CurrentLedger);
                    _state.MarkSaved();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.Strings.Err_Save + ex.Message);
                    return false;
                }
            }
            else
            {
                // If we don't have a path yet, redirect to "Save As..."
                SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV File|*.csv" };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // One line to save everything correctly
                    _csvService.Save(sfd.FileName, _state.CurrentLedger);

                    _state.MarkSaved();
                    return true;
                }
            }
            return false; // No save performed
            // No data to save in About page
        }
        public void UpdateAccountingTerms(bool useAccountingTerms) { /* Not needed here */ }
        public bool HasUnsavedChanges => false;

        // 3. The "Broadcasters"
        // Connect these to your buttons in the Designer!
        private void buttonOpenLedger_Click(object sender, EventArgs e)
        {
            OpenLedgerRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            OpenAboutRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
