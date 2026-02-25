using CSV_Accounting.Domain;
using CSV_Accounting.Helper;
using CSV_Accounting.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CSV_Accounting.Services
{
    public partial class UCAnalytics : UserControl, IPage
    {
        private readonly AnalyticsViewModel _analyticsVM = new AnalyticsViewModel();

        private readonly CsvService _csvService = new CsvService();
        private readonly LedgerQueryService _queryService = new LedgerQueryService();
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

        public void UpdateLanguage()
        {
            buttonBackStat.Text = Resources.Strings.Btn_Back;
            UpdateDescStat();
            //labelQuartiles.Text = Resources.Strings.Lbl_Quar;
            //labelDesc.Text = Resources.Strings.Lbl_Desc;

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

        private LedgerState _state;
        public UCAnalytics(LedgerState state)
        {
            InitializeComponent();
            int padding = 20;
            labelDesc.AutoSize = false;
            labelDesc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            // Ensure it starts with a good width based on the UC size
            labelDesc.Width = this.Width - (labelDesc.Left * 3);
            labelDesc.Height = 80;  // Enough for 3 lines

            _state = state;

            var logs = _queryService.GetAllLogs(_state.CurrentLedger);

            UpdateDescStat();

            var areaChart = chartMain.ChartAreas[0];

            areaChart.AxisX.MajorGrid.LineColor = Color.LightGray;
            areaChart.AxisY.MajorGrid.LineColor = Color.LightGray;

            areaChart.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            areaChart.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            comboTagFilter.Items.Clear();
            comboTagFilter.Items.Add("All tags");

            ViewModel = _analyticsVM.Build(_queryService.GetAllLogs(_state.CurrentLedger));
            if (ViewModel == null)
            {
                chartMain.Series.Clear();
                comboTagFilter.Enabled = false;
                labelFilterByTag.Text = Resources.Strings.Lbl_FilterByTag; 
                labelDesc.Text = Resources.Strings.NoDataFound;
                return;
            }
            comboTagFilter.Enabled = true;
            foreach (var tag in ViewModel.AvailableTags.OrderBy(t => t))
            {
                comboTagFilter.Items.Add(tag);
            }

            comboTagFilter.SelectedIndex = 0;
            // This is where you run your LINQ and Chart logic
            PopulateCharts(ViewModel);
        }
        AnalyticsViewModel ViewModel = new AnalyticsViewModel();
        // Define the "Shout"
        public event EventHandler BackRequested;
        private readonly LedgerAnalyticsService _analyticsService = new LedgerAnalyticsService();

        private double _variance;
        private double _mean;
        private void PopulateCharts(AnalyticsViewModel ViewModel)
        {

            chartMain.Series.Clear();
            var seriesMn = chartMain.Series.Add("Monthly Net");
            var seriesMi = chartMain.Series.Add("Monthly Inflow");
            var seriesMo = chartMain.Series.Add("Monthly Outflow");
            var seriesAn = chartMain.Series.Add("Annual Net");
            var seriesAi = chartMain.Series.Add("Annual Inflow");
            var seriesAo = chartMain.Series.Add("Annual Outflow");

            seriesMn.XValueType = ChartValueType.DateTime;
            seriesMi.XValueType = ChartValueType.DateTime;
            seriesMo.XValueType = ChartValueType.DateTime;

            seriesAn.XValueType = ChartValueType.DateTime;
            seriesAi.XValueType = ChartValueType.DateTime;
            seriesAo.XValueType = ChartValueType.DateTime;

            //seriesMn.ChartType = SeriesChartType.Line;
            //seriesMi.ChartType = SeriesChartType.Line;
            //seriesMo.ChartType = SeriesChartType.Line;

            //seriesAn.ChartType = SeriesChartType.Line;
            //seriesAi.ChartType = SeriesChartType.Line;
            //seriesAo.ChartType = SeriesChartType.Line;
            foreach (var ser in chartMain.Series)
            {
                ser.XValueType = ChartValueType.DateTime;
                ser.ChartType = SeriesChartType.Line;
                ser.BorderWidth = 2;
                ser.MarkerStyle = MarkerStyle.Circle;
                ser.MarkerSize = 5;
            }


            foreach (var item in ViewModel.MonthlyNetSeries)
            {
                seriesMn.Points.AddXY(item.Key, item.Value);
            }
            foreach (var item in ViewModel.MonthlyInflowSeries)
            {
                seriesMi.Points.AddXY(item.Key, item.Value);
            }
            foreach (var item in ViewModel.MonthlyOutflowSeries)
            {
                seriesMo.Points.AddXY(item.Key, item.Value);
            }
            foreach (var item in ViewModel.AnnualNetSeries)
            {
                seriesAn.Points.AddXY(item.Key, item.Value);
            }
            foreach (var item in ViewModel.AnnualInflowSeries)
            {
                seriesAi.Points.AddXY(item.Key, item.Value);
            }
            foreach (var item in ViewModel.AnnualOutflowSeries)
            {
                seriesAo.Points.AddXY(item.Key, item.Value);
            }
            chartMain.ChartAreas[0].AxisX.LabelStyle.Format = "MMM yyyy";
            chartMain.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
            chartMain.ChartAreas[0].AxisX.Interval = 1;

            //var ledger = _state.CurrentLedger;

            //var logs = ledger.Months
            //    .SelectMany(m => m.Logs)
            //    .OrderBy(l => l.Date)
            //    .ToList();

            //if (!logs.Any()) return;

            ////------------------------------------
            //// 1. Chart (your existing code)
            ////------------------------------------
            //chartMain.Series.Clear();
            //var series = chartMain.Series.Add("Balance Trend");
            //series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            //decimal runningBalance = 0;
            //foreach (var log in logs)
            //{
            //    runningBalance += (log.Debit - log.Credit);
            //    series.Points.AddXY(log.Date.ToShortDateString(), (double)runningBalance);
            //}

            ////------------------------------------
            //// 2. Statistics
            ////------------------------------------
            //var stats = _analyticsService.Calculate(ledger);
            //if (stats == null) return;

            ////_mean = stats.Mean;
            ////_variance = stats.Variance;

            //UpdateDescStat();

            ////------------------------------------
            //// 3. Descriptive text (data mining style)
            ////------------------------------------
            //labelDesc.Text = GenerateInsight(logs);
        }

        private void UpdateDescStat()
        {
            if (comboTagFilter.SelectedItem != null){
                var tagLast3MonthSpend = ViewModel.TagRatios;
            var selectedTags = comboTagFilter.SelectedItem.ToString();
                if (selectedTags != "All tags")
                    labelDesc.Text = string.Format(Resources.Strings.Lbl_Desc, selectedTags, tagLast3MonthSpend[selectedTags].ToString("P0"));
                else
                {
                    var mostDateTagSpend = ViewModel.MaxSpendInsight;
                    var joinedTags = JoinListLocalized(mostDateTagSpend.Value.Tags);
                    string localizedDate = new DateTime(mostDateTagSpend.Value.Year, mostDateTagSpend.Value.Month, 1).ToString("MMMM yyyy", CultureInfo.CurrentUICulture);
                    labelDesc.Text = string.Format(
                        Resources.Strings.MaxSpendTags,
                        localizedDate,
                        joinedTags
                     );
                }
            }
            labelFilterByTag.Text = Resources.Strings.Lbl_FilterByTag;
            //labelVar.Text = $"{Resources.Strings.Lbl_Var}{_variance:F2}";
        }

        private string JoinListLocalized(List<string> items)
        {
            if (items.Count == 0) return "";
            if (items.Count == 1) return items[0];

            // Resources.Strings.ListSeparator: "," (en/id) or "،" (ar)
            // Resources.Strings.AndSeparator: " and " (en), " dan " (id), " و" (ar)
            string initial = string.Join(Resources.Strings.ListSeparator + " ", items.Take(items.Count - 1));
            return $"{initial}{Resources.Strings.AndSeparator}{items.Last()}";
        }

        private string GenerateInsight(List<Log> logs)
        {
            var highestMonth = logs
                .GroupBy(l => new { l.Date.Year, l.Date.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Spend = g.Sum(x => x.Credit),
                    Logs = g.ToList()
                })
                .OrderByDescending(x => x.Spend)
                .FirstOrDefault();

            if (highestMonth == null)
                return "No spending data.";

            var topTags = highestMonth.Logs
                .SelectMany(l => l.Tags)
                .GroupBy(t => t)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key);

            return $"You spend the most in {highestMonth.Month}/{highestMonth.Year} " +
                   $"mostly on {string.Join(", ", topTags)}.";
        }


        private string GenerateDescription(List<Log> logs)
        {
            // Example rule:
            // Detect month with highest spending

            var monthlySpend = logs
                .GroupBy(l => new { l.Date.Year, l.Date.Month })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Spend = g.Sum(x => x.Credit)
                })
                .OrderByDescending(x => x.Spend)
                .First();

            return $"Highest spending occurs in month {monthlySpend.Month} " +
                   $"with total {monthlySpend.Spend:F2}.";
        }

        private string GenerateTagInsight(List<Log> logs, string tag)
        {
            var tagged = logs.Where(l => l.Tags.Contains(tag)).ToList();
            if (!tagged.Any()) return "";

            var grouped = tagged
                .GroupBy(l => l.Date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Spend = g.Sum(x => x.Credit)
                })
                .OrderByDescending(x => x.Spend)
                .Take(2)
                .ToList();

            if (grouped.Count < 2) return "";

            return $"You tend to spend more on '{tag}' during months " +
                   $"{grouped[0].Month} and {grouped[1].Month}.";
        }


        private void buttonBackStat_Click(object sender, EventArgs e)
        {
            BackRequested?.Invoke(this, EventArgs.Empty);
        }

        private void comboTagFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboTagFilter.SelectedItem == null) return;
            string selectedTag = comboTagFilter.SelectedItem?.ToString();

            // Normalize "All tags" → empty string
            if (selectedTag == "All tags")
                selectedTag = "";

            var filteredLogs = _queryService.FilterByTag(_state.CurrentLedger, selectedTag);
            ViewModel = _analyticsVM.Build(filteredLogs);
            PopulateCharts(ViewModel);
            UpdateDescStat();
        }
    }
}
