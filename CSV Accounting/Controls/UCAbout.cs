using CSV_Accounting.Domain;
using CSV_Accounting.Helper;
using CSV_Accounting.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSV_Accounting.Controls
{
    public partial class UCAbout : UserControl, IPage
    {
        public event EventHandler BackRequested;
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

        // satisfies IPage.HandleShortcut - ignores all keys
        public void HandleShortcut(Keys keyData)
        {
            // Optional: Handle Escape to go back
            if (keyData == Keys.Escape)
            {
                BackRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        // satisfies IPage.UpdateAccountingTerms - ignore
        public void UpdateAccountingTerms(bool useAccountingTerms)
        {
            // About page doesn't care about accounting terms
        }

        // satisfies IPage.HasUnsavedChanges
        
        private LedgerState _state;
        public UCAbout(LedgerState state)
        {
            _state = state;
            InitializeComponent();
        }

        public void UpdateLanguage()
        {
            buttonBackAbout.Text = Resources.Strings.Btn_Back;
            labelDesc.Text = Resources.Strings.Lbl_AboutDesc;
            labelAboutTitle.Text = Resources.Strings.Lbl_AboutTitle;

            // Arabic RTL support
            bool isArabic = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "ar";
            this.RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No;
            // Manually fix alignment for controls that don't auto-mirror
            foreach (Control c in this.Controls)
            {
                if (c is Button || c is Label)
                {
                    // In Arabic, text should usually be MiddleRight
                    c.RightToLeft = this.RightToLeft;
                }
            }
        }
        private bool _isNightMode;

        public void UpdateTheme(bool isNightMode)
        {
            _isNightMode = isNightMode;
            Color bgColor = isNightMode ? Color.FromArgb(32, 32, 32) : SystemColors.Control;
            Color fontColor = isNightMode ? Color.White : Color.Black;
            Color cellColor = isNightMode ? Color.FromArgb(23, 23, 23) : Color.White;
            Color colorButtonBack = isNightMode ? Color.FromArgb(23, 23, 23) : Color.White; ;

            //lets change...
            this.ForeColor = fontColor;
            this.BackColor = bgColor;
            //Now for every special-control that does need an extra color / property to be set use something like this
            foreach (Button button in this.Controls.OfType<Button>())
            {
                button.BackColor = colorButtonBack;
                button.ForeColor = fontColor;
            }
            foreach (Label label in this.Controls.OfType<Label>())
            {
                label.ForeColor = fontColor;
            }
            this.Invalidate(); //Forces a re-draw of your controls / form
        }
        
        private void buttonBackAbout_Click(object sender, EventArgs e)
        {
            // 1. If there are unsaved changes, you could show a prompt here, 
            // but usually, we just let the event "bubble up" to Form1.

            // 2. Safely trigger the event
            BackRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
