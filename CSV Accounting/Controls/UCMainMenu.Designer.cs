namespace CSV_Accounting.Controls
{
    partial class UCMainMenu
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
            this.buttonAbout = new System.Windows.Forms.Button();
            this.buttonOpenLedger = new System.Windows.Forms.Button();
            this.labelWelcome = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAbout
            // 
            this.buttonAbout.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonAbout.Location = new System.Drawing.Point(541, 385);
            this.buttonAbout.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(167, 28);
            this.buttonAbout.TabIndex = 5;
            this.buttonAbout.Text = "Tentang";
            this.buttonAbout.UseVisualStyleBackColor = true;
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // buttonOpenLedger
            // 
            this.buttonOpenLedger.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOpenLedger.Location = new System.Drawing.Point(541, 349);
            this.buttonOpenLedger.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenLedger.Name = "buttonOpenLedger";
            this.buttonOpenLedger.Size = new System.Drawing.Size(167, 28);
            this.buttonOpenLedger.TabIndex = 4;
            this.buttonOpenLedger.Text = "Buka Tabel";
            this.buttonOpenLedger.UseVisualStyleBackColor = true;
            this.buttonOpenLedger.Click += new System.EventHandler(this.buttonOpenLedger_Click);
            // 
            // labelWelcome
            // 
            this.labelWelcome.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelWelcome.AutoSize = true;
            this.labelWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWelcome.Location = new System.Drawing.Point(400, 160);
            this.labelWelcome.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelWelcome.Name = "labelWelcome";
            this.labelWelcome.Size = new System.Drawing.Size(449, 24);
            this.labelWelcome.TabIndex = 6;
            this.labelWelcome.Text = "Selamat Datang di Pembuat Arsip CSV Akuntan";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.buttonOpenLedger, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelWelcome, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonAbout, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1249, 763);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // UCMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UCMainMenu";
            this.Size = new System.Drawing.Size(1249, 763);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.Button buttonOpenLedger;
        private System.Windows.Forms.Label labelWelcome;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
