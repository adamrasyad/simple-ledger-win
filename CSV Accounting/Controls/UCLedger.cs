using CSV_Accounting.Domain;
using CSV_Accounting.Helper;
using CSV_Accounting.Services;
using CSV_Accounting.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CSV_Accounting.Controls
{
    public partial class UCLedger : UserControl, IPage
    {
        // Implementation of the Interface Event
        public event EventHandler BackRequested;
        public event EventHandler OpenStatRequested;

        private LedgerState _state;
        public UCLedger(LedgerState state)
        {
            InitializeComponent();
            SetSearchPlaceholder();
            labelLastEdit.Text = dateTime + DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            _state = state; // We share the state with Form1
            _currentLedger = _state.CurrentLedger;


            //Messages
            labelCSVSaved.Visible = false;
            labelLoadCSV.Visible = false;

            buttonUndo.Enabled = _undoRedoService.CanUndo;
            buttonRedo.Enabled = _undoRedoService.CanRedo;
            _undoRedoService.OnStateChanged += () =>
            {
                _state.MarkDirty();
                buttonUndo.Enabled = _undoRedoService.CanUndo;
                buttonRedo.Enabled = _undoRedoService.CanRedo;
            };
            dataGridViewLedger.AutoGenerateColumns = true;
        }



        private bool _isNightMode = false;

        string dateTime = Resources.Strings.Lbl_LastEdited;

        bool isOpeningFile = false;
        string theFileNameWithoutPath = "";

        private Ledger _currentLedger = new Ledger(); // Starts empty but not null


        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);

        private const int EM_SETCUEBANNER = 0x1501;

        private void SetSearchPlaceholder()
        {
            // Use your localized resource string here
            string hint = Resources.Strings.SearchPlaceholder;
            SendMessage(txtSearch.Handle, EM_SETCUEBANNER, 1, hint); // 1 = show even when focused
        }

        private bool _useAccountingTerms = false;

        public void UpdateAccountingTerms(bool useAccountingTerms)
        {
            _useAccountingTerms = useAccountingTerms;
            UpdateGridHeaders(); // Refresh headers when checkbox toggles
        }

        private void UpdateGridHeaders()
        {
            if (dataGridViewLedger.Columns.Contains("Debit"))
                dataGridViewLedger.Columns["Debit"].HeaderText = _useAccountingTerms
                    ? Resources.Strings.Col_Debit
                    : Resources.Strings.Col_Inflow;

            if (dataGridViewLedger.Columns.Contains("Credit"))
                dataGridViewLedger.Columns["Credit"].HeaderText = _useAccountingTerms
                    ? Resources.Strings.Col_Credit
                    : Resources.Strings.Col_Outflow;

            // Update other non-toggling headers 
            // Grid headers - Target by the Property Name in LedgerViewModel
            if (dataGridViewLedger.Columns.Contains("DateText")) // The keys are based on the LedgerRowViewModel property names, not the display names
                dataGridViewLedger.Columns["DateText"].HeaderText = Resources.Strings.Col_Date;

            if (dataGridViewLedger.Columns.Contains("Description"))
                dataGridViewLedger.Columns["Description"].HeaderText = Resources.Strings.Col_Description;

            if (dataGridViewLedger.Columns.Contains("Reference"))
                dataGridViewLedger.Columns["Reference"].HeaderText = Resources.Strings.Col_Reference;

            if (dataGridViewLedger.Columns.Contains("Tags"))
                dataGridViewLedger.Columns["Tags"].HeaderText = Resources.Strings.Col_Tags;

            if (dataGridViewLedger.Columns.Contains("Balance"))
                dataGridViewLedger.Columns["Balance"].HeaderText = Resources.Strings.Col_Balance;
        }

        public void UpdateLanguage()
        {

            buttonCSVImport.Text = Resources.Strings.Btn_ImportCsv;
            buttonBack.Text = Resources.Strings.Btn_Back;
            buttonAddLog.Text = Resources.Strings.Btn_AddRow;
            buttonSaveAs.Text = Resources.Strings.Btn_SaveAs;
            buttonClear.Text = Resources.Strings.Btn_Clear;
            buttonSave.Text = Resources.Strings.Btn_Save;
            buttonUndo.Text = Resources.Strings.Btn_Undo;
            buttonRedo.Text = Resources.Strings.Btn_Redo;
            buttonShowStats.Text = Resources.Strings.Lbl_Analytics;

            labelLoadCSV.Text = Resources.Strings.Lbl_OpenedCSV;
            labelCSVSaved.Text = Resources.Strings.Lbl_SaveSuccess;

            dateTime = Resources.Strings.Lbl_LastEdited;
            labelLastEdit.Text = dateTime + DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            // Refresh the grid to show new Month Separator names (e.g., "January" to "Januari")
            //RenderLedger(_currentLedger.Months);
            //// 2. Update Grid Headers
            //UpdateGridHeaders();

            // 3. Re-Render to translate month names in separators
            RenderLedger(_currentLedger.Months);

            // 4. Handle RTL
            bool isArabic = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "ar";
            this.RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No;
            dataGridViewLedger.RightToLeft = this.RightToLeft;
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

            labelCSVSaved.ForeColor = isNightMode ? Color.LightGreen : Color.Green;

            this.Invalidate();
        }


        private void RenderLedger(IEnumerable<MonthLog> months, bool focusLastRow = false)
        {
            List<LedgerRowViewModel> rows = new List<LedgerRowViewModel>();

            // If there is no data, we can just show an empty list or a "Start" row
            if (months == null || !months.Any())
            {
                dataGridViewLedger.DataSource = rows; // Pure empty
                return;
            }

            decimal grandTotal = 0;

            foreach (var month in months.OrderBy(m => m.Month))
            {
                foreach (var log in month.Logs.OrderBy(l => l.Date))
                {
                    grandTotal += (log.Debit - log.Credit);
                    rows.Add(new LedgerRowViewModel
                    {
                        LogId = log.Id,
                        Date = log.Date,
                        DateText = log.Date.ToString("d", CultureInfo.CurrentCulture),
                        Description = log.Description,
                        Reference = log.Reference,
                        Tags = string.Join(", ", log.Tags),
                        Debit = log.Debit,
                        Credit = log.Credit,
                        Balance = grandTotal
                    });
                }

                // Monthly Separator
                rows.Add(new LedgerRowViewModel
                {
                    Date = month.Month,
                    DateText = $"--- {month.Month:MMMM yyyy} ---",
                    Description = Resources.Strings.StringMonthlyBalance,
                    Balance = month.Balance,
                    IsMonthSeparator = true
                });
            }

            // Grand Total
            rows.Add(new LedgerRowViewModel
            {
                Description = Resources.Strings.StringGrandBalance,
                Balance = grandTotal,
                IsMonthSeparator = true
            });

            // Use BeginInvoke to prevent the Reentrant error.
            // This schedules the data swap to happen 1ms after the grid is "ready".
            this.BeginInvoke(new Action(() =>
            {
                int scrollIndex = dataGridViewLedger.FirstDisplayedScrollingRowIndex;

                dataGridViewLedger.DataSource = null;
                dataGridViewLedger.DataSource = rows;
                if (dataGridViewLedger.Columns.Contains("DateText")) // The keys are based on the LedgerRowViewModel property names, not the display names
                    dataGridViewLedger.Columns["DateText"].HeaderText = Resources.Strings.Col_Date;

                if (scrollIndex >= 0 && scrollIndex < dataGridViewLedger.RowCount)
                    dataGridViewLedger.FirstDisplayedScrollingRowIndex = scrollIndex;

                if (dataGridViewLedger.IsCurrentCellInEditMode)
                {
                    dataGridViewLedger.EndEdit();
                }

                // IMPORTANT: Clear the current cell to stop the grid's focus logic
                dataGridViewLedger.CurrentCell = null;

                // Optional: formatting logic here if needed
            
                if (focusLastRow && dataGridViewLedger.Rows.Count > 0)
                {
                    // Find the last row that is NOT a separator (optional, depends on your preference)
                    int targetRow = dataGridViewLedger.Rows.Count - 3;

                    // If the last row is the "Grand Total" separator, maybe move up one?
                    // For now, let's just hit the very last row:
                    dataGridViewLedger.CurrentCell = dataGridViewLedger.Rows[targetRow].Cells["DateText"];

                    dataGridViewLedger.Focus();
                    dataGridViewLedger.BeginEdit(true);
                }
                else
                {
                    // Standard behavior for opening files/Undo
                    dataGridViewLedger.CurrentCell = null;
                }
            }));
        }


        UndoRedoService _undoRedoService = new UndoRedoService();
        private void buttonUndo_Click(object sender, EventArgs e)
        {
            var undoneLedger = _undoRedoService.Undo();
            if (undoneLedger != null)
            {
                _currentLedger = undoneLedger;
                RenderLedger(_currentLedger.Months);
            }
            buttonUndo.Enabled = _undoRedoService.CanUndo;
            buttonRedo.Enabled = _undoRedoService.CanRedo;
        }

        private void buttonRedo_Click(object sender, EventArgs e)
        {
            var redoneLedger = _undoRedoService.Redo();
            if (redoneLedger != null)
            {
                _currentLedger = redoneLedger;
                RenderLedger(_currentLedger.Months);
            }
            buttonUndo.Enabled = _undoRedoService.CanUndo;
            buttonRedo.Enabled = _undoRedoService.CanRedo;
        }

        private void dataGridViewLedger_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            /// The CellValueChanged event fires every time a cell value is updated, regardless of how it happened. This causes two major issues:
            /// The Initialization Storm: When you call RenderLedger and set DataSource = rows, the grid fills every cell.This triggers CellValueChanged hundreds of times in a millisecond. If you have AddSnapshot() inside that event, your Undo history will fill up with garbage immediately.
            //isJustCleared = false; //welp, this event mean its filled not just cleared thus empty
            // Ignore during initial loading or header clicks
            if (e.RowIndex < 0) return;

            _state.MarkDirty();
            labelLastEdit.Text = Resources.Strings.Lbl_LastEdited + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            labelCSVSaved.Visible = false;
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            _state.CurrentLedger = _currentLedger; // Update the shared state with the latest ledger before going back
            // 1. If there are unsaved changes, you could show a prompt here, 
            // but usually, we just let the event "bubble up" to Form1.

            // 2. Safely trigger the event
            BackRequested?.Invoke(this, EventArgs.Empty);
        }
        int rowsAmount = 0;
        private void buttonAddEmptyRow_Click(object sender, EventArgs e)
        {
            dataGridViewLedger.EndEdit();
            // Save the current state BEFORE adding the new row
            _undoRedoService.AddSnapshot(_currentLedger);
            // 1. Get or Create the month group for "Today"
            var monthLog = _currentLedger.GetOrCreateMonth(DateTime.Now);

            // 2. Add a new empty log to that month
            monthLog.Logs.Add(new Log
            {
                Date = DateTime.Today,
                Description = Resources.Strings.Log_DefaultDesc,
                Debit = 0,
                Credit = 0
            });

            // 3. Re-render the grid and Force focus
            // Use BeginInvoke to ensure the UI has finished re-binding before focusing
            RenderLedger(_currentLedger.Months, true);
            //state 5 : FILL
        }
        //bool isJustCleared = false;
        //Queue<string> beforeClearedDGVCellValues = new Queue<string>();
        //int rowsCountBeforeCleared = 0;
        //int columnsCountBeforeCleared = 0;

        // SAVE CSV
        private void buttonSaveAsCsv_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV File|*.csv" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // One line to save everything correctly
                _csvService.Save(sfd.FileName, _currentLedger);

                labelCSVSaved.Visible = true;
                _state.MarkSaved();
            }
        }

        // CLEAR TABLE
        private void buttonClear_Click(object sender, EventArgs e)
        {
            // Save the current state BEFORE adding the new row
            _undoRedoService.AddSnapshot(_currentLedger);
            _currentLedger = new Ledger(); // Reset data
            RenderLedger(_currentLedger.Months); // Reset UI
            labelCSVSaved.Visible = false;
        }

        private void dataGridViewLedger_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridViewLedger.Rows[e.RowIndex].DataBoundItem as LedgerRowViewModel;
            if (row == null || row.IsMonthSeparator) return;

            // Find by ID
            var log = _currentLedger.Months
                        .SelectMany(m => m.Logs)
                        .FirstOrDefault(l => l.Id == row.LogId);

            if (log != null)
            {
                // Convert the current list to a string for an easy "is it different?" check
                string currentTagsString = string.Join(", ", log.Tags);

                bool changed = (log.Debit != row.Debit ||
                                log.Credit != row.Credit ||
                                log.Description != row.Description ||
                                log.Reference != row.Reference ||
                                currentTagsString != row.Tags || // Simple string comparison
                                log.Date != row.Date);

                if (changed)
                {
                    log.Debit = row.Debit;
                    log.Credit = row.Credit;
                    log.Description = row.Description;
                    log.Reference = row.Reference;

                    // --- HANDLE TAGS HERE ---
                    // Split the user's string by comma, trim whitespace, and remove empty entries
                    log.Tags = row.Tags?
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => t.Trim())
                        .ToList() ?? new List<string>();

                    // Check if date changed
                    DateTime newDate = row.Date;

                    if (newDate != log.Date)
                    {
                        _currentLedger.UpdateLogDate(log, newDate);
                    }

                    // saves the state WITH the new edit
                    _undoRedoService.AddSnapshot(_currentLedger);

                    // REFRESH VIEW: This respects the current search filter
                    this.BeginInvoke(new Action(() =>
                    {
                        UpdateDisplay();
                    }));
                }
            }

        }

        private void dataGridViewLedger_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void dtp_CloseUp(object sender, EventArgs e)
        {
            var dtp = sender as DateTimePicker;
            if (dtp != null)
            {
                var rowVM = dataGridViewLedger.CurrentRow.DataBoundItem as LedgerRowViewModel;

                if (rowVM != null)
                {
                    var log = _currentLedger.Months
                        .SelectMany(m => m.Logs)
                        .FirstOrDefault(l => l.Id == rowVM.LogId);

                    if (log != null)
                    {
                        _currentLedger.UpdateLogDate(log, dtp.Value);
                    }
                }

                //    // Update UI cell
                //    dataGridViewLedger.CurrentCell.Value = dtp.Value.ToString("dd/MM/yyyy");

                //    dtp.Visible = false;


                //    // 3. Use BeginInvoke to delay the re-render until the Grid finishes its internal "CloseUp" tasks
                // 3. NOW schedule the UI to re-draw
                this.BeginInvoke(new Action(() =>
                {
                    UpdateDisplay(); // This calls RenderLedger
                    dtp.Dispose();
                }));
            }
        }

        private void dtp_OnTextChange(object sender, EventArgs e)
        {
            // Optional: Update live as they pick, or wait for CloseUp. 
            // Usually better to wait for CloseUp to avoid flickering.
        }
        private void dataGridViewLedger_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Don't show picker for headers or month separators
            // 1. EXIT if user clicks Row Headers (ColumnIndex < 0) or Column Headers (RowIndex < 0)
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var row = dataGridViewLedger.Rows[e.RowIndex].DataBoundItem as LedgerRowViewModel;
            if (row == null || row.IsMonthSeparator) return;

            // Remove any existing DTPs first
            var oldPickers = dataGridViewLedger.Controls.OfType<DateTimePicker>().ToList();
            foreach (var p in oldPickers) p.Dispose();

            // Only show for the "Date" column
            if (dataGridViewLedger.Columns[e.ColumnIndex].Name == "DateText")
            {
                DateTimePicker dtp = new DateTimePicker();
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "dd/MM/yyyy";

                // Match the current cell's value
                if (DateTime.TryParse(dataGridViewLedger.CurrentCell.Value?.ToString(), out DateTime currentVal))
                    dtp.Value = currentVal;

                // Position it perfectly
                Rectangle rect = dataGridViewLedger.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                dtp.Size = new Size(rect.Width, rect.Height);
                dtp.Location = new Point(rect.X, rect.Y);

                // Link events
                dtp.CloseUp += dtp_CloseUp;
                dtp.TextChanged += dtp_OnTextChange;


                dataGridViewLedger.Controls.Add(dtp);
                dtp.Visible = true;
                dtp.Focus();
                // Send F4 to automatically drop down the calendar (nice UX!)
                SendKeys.Send("{F4}");
            }

            // Check if user clicked the "Tag" column
            // Check if the "Tag" column was clicked AND Ctrl is held down
            if (dataGridViewLedger.Columns[e.ColumnIndex].Name == "Tags" && ModifierKeys == Keys.Control)
            {
                string cellValue = dataGridViewLedger.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    // Take the first tag if multiple exist
                    string firstTag = cellValue.Split(',').First().Trim();
                    txtSearch.Text = $"Tag:{firstTag}";

                    // Prevent the cell from entering "Edit Mode" immediately
                    dataGridViewLedger.EndEdit();
                    return;
                }
            }
        }


        public bool SaveData()
        {
            _state.CurrentLedger = _currentLedger; // Ensure the shared state is updated with the latest ledger data
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // If we have a valid path (from Import or a previous Save As), save directly
            if (!string.IsNullOrEmpty(_state.FilePath) && File.Exists(_state.FilePath))
            {
                try
                {
                    // Use the Service to handle the logic
                    _csvService.Save(_state.FilePath, _currentLedger);

                    labelCSVSaved.Visible = true;
                    _state.MarkSaved();
                    labelLastEdit.Text = Resources.Strings.Lbl_SavedTo + Path.GetFileName(_state.FilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.Strings.Err_Save + ex.Message);
                }
            }
            else
            {
                // If we don't have a path yet, redirect to "Save As..."
                buttonSaveAsCsv_Click(sender, e);
            }
        }

        private bool ProceedWithOpen()
        {
            if (!_state.IsDirty) return true; // Nothing to save, proceed.

            // Using YesNoCancel gives the user a way to "Stop everything"
            DialogResult result = MessageBox.Show(
                this,
                Resources.Strings.Msg_SaveBeforeOpen,
                Resources.Strings.Msg_OverwriteCurrent,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Return true only if SaveData succeeded
                // If user cancels the SaveFileDialog inside SaveData(), it returns false
                return SaveData();
            }

            if (result == DialogResult.No)
            {
                return true; // User explicitly said "Don't save, just open the new file"
            }

            return false; // User clicked Cancel or closed the dialog
        }

        CsvService _csvService = new CsvService();
        private void ButtonImportCSV_Click(object sender, EventArgs e)
        {
            if (!ProceedWithOpen())
            {
                return; // Stop here, user canceled
            }

            //display the table
            // Clear history so they can't "Undo" back to the previous file
            _undoRedoService.ClearHistory();

            // Add the initial state of the new file as the first "Undo" point
            _undoRedoService.AddSnapshot(_currentLedger);

            OpenFileDialog open = new OpenFileDialog { Filter = "CSV Files (*.csv)|*.csv" };

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 1. Logic: Load the data into the Domain object
                    _currentLedger = _csvService.Load(open.FileName);
                    _state.CurrentLedger = _currentLedger; // Update shared state with the newly loaded ledger
                    _state.FilePath = open.FileName;
                    theFileNameWithoutPath = System.IO.Path.GetFileName(_state.FilePath);

                    // 2. UI: Update the Grid
                    RenderLedger(_currentLedger.Months);

                    labelLastEdit.Text = Resources.Strings.StringLoaded + Path.GetFileName(open.FileName);
                    isOpeningFile = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.Strings.Err_Load + ex.Message);
                }
            }
            // just opened file so no need to save anything
            _state.MarkSaved();
        }
        private void dataGridViewLedger_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Clean up hosted controls if the grid is emptied
            if (dataGridViewLedger.DataSource == null || dataGridViewLedger.RowCount == 0)
            {
                var pickers = dataGridViewLedger.Controls.OfType<DateTimePicker>().ToList();
                foreach (var picker in pickers)
                {
                    picker.Dispose();
                }
            }
            // Hide the logic columns the user doesn't need to see
            if (dataGridViewLedger.Columns.Contains("IsMonthSeparator"))
            {
                dataGridViewLedger.Columns["IsMonthSeparator"].Visible = false;
                dataGridViewLedger.Columns["IsMonthSeparator"].ReadOnly = true;
            }
            if (dataGridViewLedger.Columns.Contains("Date"))
            {
                dataGridViewLedger.Columns["Date"].Visible = false;
                dataGridViewLedger.Columns["Date"].ReadOnly = true;
            }

            if (dataGridViewLedger.Columns.Contains("LogId"))
            {
                dataGridViewLedger.Columns["LogId"].Visible = false;
                dataGridViewLedger.Columns["LogId"].ReadOnly = true;
            }
            if (dataGridViewLedger.Columns.Contains("Balance"))
            {
                dataGridViewLedger.Columns["Balance"].ReadOnly = true;
            }

            // Apply the N2 format to all decimal columns
            foreach (DataGridViewColumn column in dataGridViewLedger.Columns)
            {
                if (column.ValueType == typeof(decimal))
                {
                    column.DefaultCellStyle.Format = "N2";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        private void dataGridViewLedger_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // 1. Get the data object for the current row
            var rowData = dataGridViewLedger.Rows[e.RowIndex].DataBoundItem as LedgerRowViewModel;

            if (rowData != null && rowData.IsMonthSeparator)
            {
                // 2. Apply styling only to separators
                e.CellStyle.BackColor = _isNightMode ? Color.FromArgb(45, 45, 45) : Color.LightGray;
                e.CellStyle.Font = new Font(dataGridViewLedger.Font, FontStyle.Bold);

                // Optional: If it's a separator, you might want to hide the '0.00' in Debit/Credit
                if (dataGridViewLedger.Columns[e.ColumnIndex].Name == Resources.Strings.Col_Debit ||
                    dataGridViewLedger.Columns[e.ColumnIndex].Name == Resources.Strings.Col_Credit)
                {
                    e.Value = ""; // Makes the cell look empty
                    e.FormattingApplied = true;
                }
            }
            dataGridViewLedger.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }


        private void dataGridViewLedger_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Check if the error happened because of a bad number format
            if (e.Exception is FormatException)
            {
                // Show your simple message
                MessageBox.Show(Resources.Strings.Msg_InsertNumber, // "Please enter only numbers"
                                Resources.Strings.Err_InvalidInput,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                // This stops the technical popup from appearing
                e.ThrowException = false;

                // This keeps the user in the cell so they can fix it
                e.Cancel = true;
            }
        }

        private void dataGridViewLedger_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
            {
                // If it's a number column, maybe show a hint or just select all
                tb.SelectAll();
            }
        }

        private void dataGridViewLedger_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Capture the state BEFORE the user types anything

        }

        LedgerQueryService _queryService = new LedgerQueryService();
        private string _activeSearchTerm = string.Empty;

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _activeSearchTerm = txtSearch.Text;
            UpdateDisplay();
        }


        private void UpdateDisplay()
        {
            // 1. Get filtered logs based on the saved search term
            var filteredLogs = _queryService.SearchByKeyword(_currentLedger, _activeSearchTerm);

            // 2. Group them back into MonthLogs for the Render method
            var displayList = filteredLogs
                .GroupBy(l => new DateTime(l.Date.Year, l.Date.Month, 1))
                .Select(g => new MonthLog
                {
                    Month = g.Key,
                    Logs = g.OrderBy(l => l.Date).ToList()
                })
                .OrderBy(m => m.Month)
                .ToList();

            // 3. Render only what matches
            RenderLedger(displayList);
        }

        private void dataGridViewLedger_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string colName = dataGridViewLedger.Columns[e.ColumnIndex].Name;

            if (colName == "Tags") // Ensure this matches your ViewModel property
            {
                // Localize this string in your Resources (e.g., Msg_TagTip)
                e.ToolTipText = Resources.Strings.TagsTooltip;
            }
            else if (colName == "DateText")
            {
                e.ToolTipText = Resources.Strings.DateTooltip;
            }
        }

        private void btnShowStats_Click(object sender, EventArgs e)
        {
            // 1. Create the new control
            UCAnalytics uc = new UCAnalytics(_state);
            _state.CurrentLedger = _currentLedger; // Pass the current

            OpenStatRequested.Invoke(this, EventArgs.Empty); // Let Form1 know we want to switch to stats
        }

        public void HandleShortcut(Keys keyData)
        {
            // 1. Check for Ctrl + Shift + S (Save As)
            if (keyData == (Keys.Control | Keys.Shift | Keys.S))
            {
                buttonSaveAsCsv_Click(this, EventArgs.Empty);
            }
            // 2. Check for Ctrl + S (Quick Save)
            else if (keyData == (Keys.Control | Keys.S))
            {
                buttonSave_Click(this, EventArgs.Empty);
            }
            // 3. Check for Ctrl + N (New Entry)
            else if (keyData == (Keys.Control | Keys.N))
            {
                buttonAddEmptyRow_Click(this, EventArgs.Empty);
            }
            // 4. Check for Ctrl + Z (Undo)
            else if (keyData == (Keys.Control | Keys.Z))
            {
                buttonUndo_Click(this, EventArgs.Empty);
            }
            // 5. Check for Ctrl + Y or Ctrl + Shift + Z (Redo)
            else if (keyData == (Keys.Control | Keys.Y) || keyData == (Keys.Control | Keys.Shift | Keys.Z))
            {
                buttonRedo_Click(this, EventArgs.Empty);
            }
            // If the user presses Enter while inside the DataGridView
            if (keyData == Keys.Tab && dataGridViewLedger.CurrentCell != null)
            {
                int currentCol = dataGridViewLedger.CurrentCell.ColumnIndex;
                int tagsColIndex = dataGridViewLedger.Columns["Tags"].Index;

                // Logic: If we are on 'Tags', trigger the new row
                if (currentCol == tagsColIndex)
                {
                    // Call your existing button logic
                    buttonAddEmptyRow_Click(this, EventArgs.Empty);
                }
            }
            if (keyData == (Keys.Control | Keys.Delete))
            {
                DeleteSelectedLog();
            }
        }
        private void DeleteSelectedLog()
        {
            // 1. Identify which row is selected
            if (dataGridViewLedger.CurrentRow == null) return;

            var rowVM = dataGridViewLedger.CurrentRow.DataBoundItem as LedgerRowViewModel;

            // 2. Prevent deleting Month Separators or Grand Totals
            if (rowVM == null || rowVM.IsMonthSeparator) return;

            // 3. Confirm (Optional but recommended for accounting)
            var result = MessageBox.Show(Resources.Strings.Msg_ConfirmDelete,
                                         Resources.Strings.Title_Delete,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // 4. Save state for Undo BEFORE modifying the domain
                _undoRedoService.AddSnapshot(_currentLedger);

                // 5. Remove from the Domain (The real source of truth)
                foreach (var month in _currentLedger.Months)
                {
                    var logToRemove = month.Logs.FirstOrDefault(l => l.Id == rowVM.LogId);
                    if (logToRemove != null)
                    {
                        month.Logs.Remove(logToRemove);

                        // If the month is now empty, you might want to remove the month too
                        if (month.Logs.Count == 0) _currentLedger.Months.Remove(month);
                        break;
                    }
                }

                // 6. Update UI (This triggers the ViewModel conversion and Rendering)
                UpdateDisplay();

                _state.MarkDirty();
            }
        }


        private void UCLedger_Load(object sender, EventArgs e)
        {
            //Load current session currentLedger; empty on start, stay same when back forth main menu to this page
            RenderLedger(_currentLedger.Months);
            // 2. Now that dataGridViewLedger exists, make it smooth
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, dataGridViewLedger, new object[] { true });
            dataGridViewLedger.EditMode = DataGridViewEditMode.EditOnEnter;
        }
        private static readonly Dictionary<string, string> MathShortcuts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "k", "*1000" },     // Kilo
            { "jt", "*1000000" }, // Juta (ID)
            { "m", "*1000000" },  // Million (EN) / Milyar (ID) - Be careful with "M" context!
            { "万", "*10000" },    // Wan (JP/CN)
            { "ك", "*1000" },     // Kilo (AR)
            { "مل", "*1000000" }  // Million (AR)
        };

        private decimal EvaluateMath(string expression)
        {
            try
            {
                var culture = System.Globalization.CultureInfo.CurrentCulture;
                string thousandSep = culture.NumberFormat.NumberGroupSeparator;
                string decimalSep = culture.NumberFormat.NumberDecimalSeparator;

                // 1. Basic Cleaning
                string sanitized = expression.Replace(thousandSep, "");
                if (decimalSep != ".") sanitized = sanitized.Replace(decimalSep, ".");

                //APPLY SHORTCUTS(k, jt, m, etc.)
                foreach (var shortcut in MathShortcuts)
                {
                    // We use Case-Insensitive replace to handle "K" or "k"
                    if (sanitized.IndexOf(shortcut.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        sanitized = sanitized.ToLower();
                        sanitized = sanitized.Replace(shortcut.Key.ToLower(), shortcut.Value);
                    }
                }

                // 2. THE PERCENTAGE HACK: Convert 10% to 10/100
                sanitized = sanitized.Replace("%", "/100.0");

                // 3. Compute
                var table = new System.Data.DataTable();
                var result = table.Compute(sanitized, "");

                if (result.ToString().Contains("^"))
                    throw new Exception();

                return Convert.ToDecimal(result, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new Exception(Resources.Strings.Msg_InvalidMathExpression);
            }
        }

        private void dataGridViewLedger_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            // Only apply to Debit and Credit columns
            string colName = dataGridViewLedger.Columns[e.ColumnIndex].Name;
            if ((colName == "Debit" || colName == "Credit") && e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                try
                {
                    // Try to evaluate. If it's just a number, it still works!
                    e.Value = EvaluateMath(e.Value.ToString());
                    e.ParsingApplied = true;
                }
                catch
                {
                    // Fall back to default behavior if it's not math or a number
                }
            }

            // --- HANDLE DATE (New!) ---
            else if (colName == "DateText")
            {
                string input = e.Value?.ToString();
                if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    e.Value = parsedDate.ToString("dd/MM/yyyy");
                    e.ParsingApplied = true;

                    // Update the actual Domain object
                    SyncDateToDomain(parsedDate);
                }
                else
                {
                    // If it's a "troll" string, this triggers DataError!
                    MessageBox.Show(this,
                    Resources.Strings.Err_InvalidDate, // "Format tanggal salah!" / "تنسيق التاريخ غير صحيح"
                    Resources.Strings.Err_InvalidInput,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                    e.ParsingApplied = false; // Tells the grid the parse failed
                }
            }
        }
        private void SyncDateToDomain(DateTime newDate)
        {
            var rowVM = dataGridViewLedger.CurrentRow?.DataBoundItem as LedgerRowViewModel;
            if (rowVM == null) return;

            // --- THE FIX: MANUALLY SYNC THE VIEWMODEL NOW ---
            rowVM.Date = newDate;
            rowVM.DateText = newDate.ToString("d", CultureInfo.CurrentCulture);

            var log = _currentLedger.Months.SelectMany(m => m.Logs)
                                          .FirstOrDefault(l => l.Id == rowVM.LogId);

            if (log != null && log.Date != newDate)
            {
                _currentLedger.UpdateLogDate(log, newDate);
                _undoRedoService.AddSnapshot(_currentLedger);

                // Now that the ViewModel is synced, this will work perfectly!
                this.BeginInvoke(new Action(() => UpdateDisplay()));
            }
        }


    }
}
