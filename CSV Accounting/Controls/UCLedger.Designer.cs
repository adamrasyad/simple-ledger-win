namespace CSV_Accounting.Controls
{
    partial class UCLedger
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.buttonRedo = new System.Windows.Forms.Button();
            this.buttonUndo = new System.Windows.Forms.Button();
            this.labelCSVSaved = new System.Windows.Forms.Label();
            this.labelLoadCSV = new System.Windows.Forms.Label();
            this.buttonCSVImport = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelLastEdit = new System.Windows.Forms.Label();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonSaveAs = new System.Windows.Forms.Button();
            this.buttonAddLog = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.dataGridViewLedger = new System.Windows.Forms.DataGridView();
            this.buttonShowStats = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLedger)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(3, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(146, 22);
            this.txtSearch.TabIndex = 25;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // buttonRedo
            // 
            this.buttonRedo.Location = new System.Drawing.Point(75, 4);
            this.buttonRedo.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRedo.Name = "buttonRedo";
            this.buttonRedo.Size = new System.Drawing.Size(63, 28);
            this.buttonRedo.TabIndex = 24;
            this.buttonRedo.Text = "Redo";
            this.buttonRedo.UseVisualStyleBackColor = true;
            this.buttonRedo.Click += new System.EventHandler(this.buttonRedo_Click);
            // 
            // buttonUndo
            // 
            this.buttonUndo.Location = new System.Drawing.Point(4, 4);
            this.buttonUndo.Margin = new System.Windows.Forms.Padding(4);
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Size = new System.Drawing.Size(63, 28);
            this.buttonUndo.TabIndex = 23;
            this.buttonUndo.Text = "Undo";
            this.buttonUndo.UseVisualStyleBackColor = true;
            this.buttonUndo.Click += new System.EventHandler(this.buttonUndo_Click);
            // 
            // labelCSVSaved
            // 
            this.labelCSVSaved.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelCSVSaved.Location = new System.Drawing.Point(4, 249);
            this.labelCSVSaved.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCSVSaved.Name = "labelCSVSaved";
            this.labelCSVSaved.Size = new System.Drawing.Size(133, 41);
            this.labelCSVSaved.TabIndex = 22;
            this.labelCSVSaved.Text = "Berkas CSV berhasil disimpan";
            // 
            // labelLoadCSV
            // 
            this.labelLoadCSV.ForeColor = System.Drawing.Color.Black;
            this.labelLoadCSV.Location = new System.Drawing.Point(4, 208);
            this.labelLoadCSV.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLoadCSV.Name = "labelLoadCSV";
            this.labelLoadCSV.Size = new System.Drawing.Size(133, 41);
            this.labelLoadCSV.TabIndex = 21;
            this.labelLoadCSV.Text = "Dibuka CSV ";
            // 
            // buttonCSVImport
            // 
            this.buttonCSVImport.Location = new System.Drawing.Point(4, 176);
            this.buttonCSVImport.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCSVImport.Name = "buttonCSVImport";
            this.buttonCSVImport.Size = new System.Drawing.Size(147, 28);
            this.buttonCSVImport.TabIndex = 13;
            this.buttonCSVImport.Text = "Impor CSV...";
            this.buttonCSVImport.UseVisualStyleBackColor = true;
            this.buttonCSVImport.Click += new System.EventHandler(this.ButtonImportCSV_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(4, 68);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(147, 28);
            this.buttonSave.TabIndex = 20;
            this.buttonSave.Text = "Simpan";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelLastEdit
            // 
            this.labelLastEdit.Location = new System.Drawing.Point(4, 437);
            this.labelLastEdit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLastEdit.Name = "labelLastEdit";
            this.labelLastEdit.Size = new System.Drawing.Size(456, 23);
            this.labelLastEdit.TabIndex = 16;
            this.labelLastEdit.Text = "Last Edited on :";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(4, 140);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(147, 28);
            this.buttonClear.TabIndex = 18;
            this.buttonClear.Text = "Kosongkan Tabel";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonSaveAs
            // 
            this.buttonSaveAs.Location = new System.Drawing.Point(4, 104);
            this.buttonSaveAs.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSaveAs.Name = "buttonSaveAs";
            this.buttonSaveAs.Size = new System.Drawing.Size(147, 28);
            this.buttonSaveAs.TabIndex = 17;
            this.buttonSaveAs.Text = "Simpan Sebagai ...";
            this.buttonSaveAs.UseVisualStyleBackColor = true;
            this.buttonSaveAs.Click += new System.EventHandler(this.buttonSaveAsCsv_Click);
            // 
            // buttonAddLog
            // 
            this.buttonAddLog.Location = new System.Drawing.Point(4, 32);
            this.buttonAddLog.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAddLog.Name = "buttonAddLog";
            this.buttonAddLog.Size = new System.Drawing.Size(147, 28);
            this.buttonAddLog.TabIndex = 15;
            this.buttonAddLog.Text = "Tambah Barisan";
            this.buttonAddLog.UseVisualStyleBackColor = true;
            this.buttonAddLog.Click += new System.EventHandler(this.buttonAddEmptyRow_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(4, 399);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(101, 28);
            this.buttonBack.TabIndex = 14;
            this.buttonBack.Text = "Kembali";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.ButtonBack_Click);
            // 
            // dataGridViewLedger
            // 
            this.dataGridViewLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLedger.Location = new System.Drawing.Point(4, 4);
            this.dataGridViewLedger.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewLedger.Name = "dataGridViewLedger";
            this.dataGridViewLedger.RowHeadersWidth = 51;
            this.dataGridViewLedger.Size = new System.Drawing.Size(905, 429);
            this.dataGridViewLedger.TabIndex = 12;
            this.dataGridViewLedger.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewLedger_CellBeginEdit);
            this.dataGridViewLedger.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLedger_CellClick);
            this.dataGridViewLedger.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLedger_CellContentClick);
            this.dataGridViewLedger.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLedger_CellEndEdit);
            this.dataGridViewLedger.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewLedger_CellFormatting);
            this.dataGridViewLedger.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dataGridViewLedger_CellParsing);
            this.dataGridViewLedger.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.dataGridViewLedger_CellToolTipTextNeeded);
            this.dataGridViewLedger.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLedger_CellValueChanged);
            this.dataGridViewLedger.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewLedger_DataBindingComplete);
            this.dataGridViewLedger.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewLedger_DataError);
            this.dataGridViewLedger.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewLedger_EditingControlShowing);
            // 
            // buttonShowStats
            // 
            this.buttonShowStats.Location = new System.Drawing.Point(4, 363);
            this.buttonShowStats.Margin = new System.Windows.Forms.Padding(4);
            this.buttonShowStats.Name = "buttonShowStats";
            this.buttonShowStats.Size = new System.Drawing.Size(101, 28);
            this.buttonShowStats.TabIndex = 26;
            this.buttonShowStats.Text = "Analitik";
            this.buttonShowStats.UseVisualStyleBackColor = true;
            this.buttonShowStats.Click += new System.EventHandler(this.btnShowStats_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1109, 503);
            this.tableLayoutPanel1.TabIndex = 27;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttonUndo);
            this.flowLayoutPanel1.Controls.Add(this.buttonRedo);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1109, 30);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.tableLayoutPanel2.Controls.Add(this.labelLastEdit, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.dataGridViewLedger, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 33);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1103, 467);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.labelCSVSaved, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.labelLoadCSV, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.txtSearch, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonCSVImport, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.buttonSave, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.buttonClear, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.buttonAddLog, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonSaveAs, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.buttonBack, 0, 10);
            this.tableLayoutPanel3.Controls.Add(this.buttonShowStats, 0, 9);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(916, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 11;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(184, 431);
            this.tableLayoutPanel3.TabIndex = 17;
            // 
            // UCLedger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UCLedger";
            this.Size = new System.Drawing.Size(1109, 503);
            this.Load += new System.EventHandler(this.UCLedger_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLedger)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button buttonRedo;
        private System.Windows.Forms.Button buttonUndo;
        private System.Windows.Forms.Label labelCSVSaved;
        private System.Windows.Forms.Label labelLoadCSV;
        private System.Windows.Forms.Button buttonCSVImport;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelLastEdit;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonSaveAs;
        private System.Windows.Forms.Button buttonAddLog;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.DataGridView dataGridViewLedger;
        private System.Windows.Forms.Button buttonShowStats;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}
