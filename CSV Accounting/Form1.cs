using CSV_Accounting.Controls;
using CSV_Accounting.Domain;
using CSV_Accounting.Helper;
using CSV_Accounting.Services;
using CSV_Accounting.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


/**
 *State : 
    1. Menu,
    2. Tambah file,
    3. Edit file,
    4. Buka folder,
    5. Isi tabel akuntan.
 */

namespace CSV_Accounting
{
    public partial class Form1 : Form
    {
        private Ledger _currentLedger = new Ledger();
        private bool _isNightMode = false;

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true; // Added for your Ctrl+S shortcuts!
            InitializeLanguageBox();
            ShowMainMenu(); // Start here
        }

        // --- NAVIGATION METHODS ---
        private LedgerState _state = new LedgerState();

        public void ShowMainMenu()
        {
            var uc = new UCMainMenu(_state);
            uc.OpenLedgerRequested += (s, e) => ShowLedger();
            uc.OpenAboutRequested += (s, e) => ShowAbout();
            SwapPage(uc);
        }
        private void ShowLedger()
        {
            // Pass the STATE, not just individual variables
            var uc = new UCLedger(_state);
            uc.BackRequested += (s, e) => ShowMainMenu();
            uc.OpenStatRequested += (s, e) => ShowStats();

            SwapPage(uc);
        }
        public void ShowStats()
        {
            var uc = new UCAnalytics(_state);
            uc.BackRequested += (s, e) => ShowLedger();
            SwapPage(uc);
        }
        public void ShowAbout()
        {
            var uc = new UCAbout(_state);
            uc.BackRequested += (s, e) => ShowMainMenu();
            SwapPage(uc);
        }

        private void SwapPage(UserControl page)
        {
            panelContent.Controls.Clear();
            page.Dock = DockStyle.Fill;
            panelContent.Controls.Add(page);

            // Sync the new page to current Global State
            if (page is IPage p)
            {
                p.UpdateTheme(_isNightMode);
                p.UpdateLanguage();
            }
        }

        string[] supportedCultures = { "en", "id", "ar" };
        private void InitializeLanguageBox()
        {
            comboBoxLang.Items.Clear();
            // Add the cultures you have created .resx files for


            foreach (string culture in supportedCultures)
            {
                var info = new CultureInfo(culture);
                // Gets the "LangName" string from the specific resource file
                string localizedName = Resources.Strings.ResourceManager.GetString("LanguageName", info);
                comboBoxLang.Items.Add(localizedName);
            }

            comboBoxLang.SelectedItem = Resources.Strings.LanguageName;
        }// Default
        
        
        bool flag = false;
        private void pictureBoxToggleNightMode_Click(object sender, EventArgs e)
        {
            flag = !flag;
            // Ternary operator: if flag is true, use Sun, else use Moon
            pictureBoxToggleNightMode.Image = flag ? Properties.Resources.CSV_App_Sun_Icon : Properties.Resources.CSV_App_Moon_Icon;
            SwitchDesign();
            //Debug with Console
            //Console.WriteLine(pictureBox1.Image.ToString());

        }

        //thanks to https://stackoverflow.com/questions/67311404/how-to-create-a-switch-to-select-a-dark-theme-for-windows-form-that-can-darken
        //answered May 1, 2021 at 19:12 by Maik8
        // --- THEME ---
        private void SwitchDesign()
        {
            // 1. Style Form1 (The Frame)
            Color bgColor = flag ? Color.FromArgb(32, 32, 32) : SystemColors.Control;
            Color fontColor = flag ? Color.White : Color.Black;
            this.BackColor = bgColor;
            this.ForeColor = fontColor;

            comboBoxLang.BackColor = bgColor;
            comboBoxLang.ForeColor = fontColor;
            checkBoxAccountingTerm.BackColor = bgColor;
            checkBoxAccountingTerm.ForeColor = fontColor;

            // 2. Broadcast to the Page
            if (panelContent.Controls.Count > 0 && panelContent.Controls[0] is IPage page)
            {
                page.UpdateTheme(flag);
            }
        }

        // --- LANGUAGE ---
        private void ApplyLanguage()
        {
            this.Text = Resources.Strings.App_Title;
            checkBoxAccountingTerm.Text = Resources.Strings.Checkbox_AccountingTerms;

            // Broadcast to the Page
            if (panelContent.Controls.Count > 0 && panelContent.Controls[0] is IPage page)
            {
                page.UpdateLanguage();
            }

            // RTL Check
            bool isArabic = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "ar";
            this.RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No;
            this.RightToLeftLayout = isArabic;
        }

        // --- ACCOUNTING TERMS ---
        private void checkBoxAccountingTerm_CheckedChanged(object sender, EventArgs e)
        {
            // This is your 'toggle' boolean for Debit vs Inflow
            if (panelContent.Controls.Count > 0 && panelContent.Controls[0] is IPage page)
            {
                page.UpdateAccountingTerms(checkBoxAccountingTerm.Checked);
            }
        }

        //Toggle UI Change
        private void pictureBoxToggleNightMode_MouseHover(object sender, EventArgs e)
        {
            //nvm
        }

        private void pictureBoxToggleNightMode_MouseLeave(object sender, EventArgs e)
        {
            Size size = new Size(25, 25);
            pictureBoxToggleNightMode.Size = size;
            pictureBoxToggleNightMode.Top += 5;
        }

        private void pictureBoxToggleNightMode_MouseEnter(object sender, EventArgs e)
        {
            Size size = new Size(30, 30);
            pictureBoxToggleNightMode.Size = size;
            pictureBoxToggleNightMode.Top -= 5;
        }

        //Save CHeck
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 1. Find the active page
            if (panelContent.Controls.Count > 0 && panelContent.Controls[0] is IPage activePage)
            {
                // 2. Ask the page: "Do you have unsaved stuff?"
                if (_state.IsDirty)
                {
                    DialogResult result = MessageBox.Show(
                        this,
                        Resources.Strings.Msg_SaveBeforeClosing,
                        Resources.Strings.Msg_SaveTitle,
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        if (!activePage.SaveData())
                        {
                            // Save failed or user canceled SaveFileDialog
                            e.Cancel = true;
                            return;
                        }
                        // Trigger the save button on the active ledger page
                        // (You might need a SaveData() method in your interface too!)
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true; // Don't close
                    }
                }
            }
            // If No, the method ends and the form closes naturally
        }

        
        bool toggle = false;

        //thanks to https://stackoverflow.com/questions/67311404/how-to-create-a-switch-to-select-a-dark-theme-for-windows-form-that-can-darken
        //answered May 1, 2021 at 19:12 by Maik8
        string selected = "English";
        private void SwitchLang()
        {
            selected = this.comboBoxLang.GetItemText(this.comboBoxLang.SelectedItem);

            if (selected == "Bahasa Indonesia")
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("id");
            else if (selected == "English")
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            else if (selected == "العربية")
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar");

            ApplyLanguage();
        }
        private void comboBoxLang_DropDownClosed(object sender, EventArgs e)
        {
            // removed to selected index changed
        }
        //bool isRowJustAdded = false;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           //removed to onkeydown
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Check if we have an active page that can handle shortcuts
            if (panelContent.Controls.Count > 0 && panelContent.Controls[0] is IPage activePage)
            {
                activePage.HandleShortcut(e.KeyData);

                // If the KeyData matched one of our shortcuts, stop Windows from 'dinging'
                // We can check if e.Handled was set or just suppress if it's a known shortcut
                if (e.Control && (e.KeyCode == Keys.S || e.KeyCode == Keys.N || e.KeyCode == Keys.Z || e.KeyCode == Keys.Y))
                {
                    e.SuppressKeyPress = true;
                    return;
                }
            }

            base.OnKeyDown(e);
        }

        private void comboBoxLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBoxLang.SelectedItem.ToString();

            foreach (string culture in supportedCultures)
            {
                var info = new CultureInfo(culture);
                if (Resources.Strings.ResourceManager.GetString("LanguageName", info) == selected)
                {
                    Thread.CurrentThread.CurrentUICulture = info;
                    Thread.CurrentThread.CurrentCulture = info; // Also sets number formatting
                    break;
                }
            }

            SwitchLang();
        }




        /*
private void dateTimePicker_OnTextChange(object sender, EventArgs e)
{
dataGridView1.CurrentCell.Value = dateTimePicker.Text.ToString();
}
*/
    }
}

/**
 * Fitur :
    1. Tombol tambah - DONE
    2. Tombol edit - DONE
    3. Tombol hapus - DONE
    4. Isi tabel. - DONE
    *UPDATE (1/25/2023) :
    *- Fitur ditambahkan : save, save as, alert texts, undo-redo button
    *
 */ 