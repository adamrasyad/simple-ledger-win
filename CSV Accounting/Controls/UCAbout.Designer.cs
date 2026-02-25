namespace CSV_Accounting.Controls
{
    partial class UCAbout
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
            this.labelAboutTitle = new System.Windows.Forms.Label();
            this.labelDesc = new System.Windows.Forms.Label();
            this.buttonBackAbout = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelAboutTitle
            // 
            this.labelAboutTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAboutTitle.AutoSize = true;
            this.labelAboutTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAboutTitle.Location = new System.Drawing.Point(464, 3);
            this.labelAboutTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAboutTitle.Name = "labelAboutTitle";
            this.labelAboutTitle.Size = new System.Drawing.Size(87, 24);
            this.labelAboutTitle.TabIndex = 5;
            this.labelAboutTitle.Text = "Tentang";
            this.labelAboutTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAboutTitle.Click += new System.EventHandler(this.buttonBackAbout_Click);
            // 
            // labelDesc
            // 
            this.labelDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDesc.Location = new System.Drawing.Point(40, 70);
            this.labelDesc.Margin = new System.Windows.Forms.Padding(40, 40, 40, 0);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(936, 343);
            this.labelDesc.TabIndex = 4;
            this.labelDesc.Text = "     Perangkat lunak ini dikembangkan oleh Mochamad Adamrasyad Iqbal (C) 1444 H. " +
    "Perangkat lunak ini berguna untuk membuat tabel akuntan setiap saat dibutuhkan d" +
    "an membuka file CSV peruntukannya.";
            this.labelDesc.Click += new System.EventHandler(this.buttonBackAbout_Click);
            // 
            // buttonBackAbout
            // 
            this.buttonBackAbout.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonBackAbout.BackColor = System.Drawing.SystemColors.ControlLight;
            this.buttonBackAbout.Location = new System.Drawing.Point(458, 421);
            this.buttonBackAbout.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBackAbout.Name = "buttonBackAbout";
            this.buttonBackAbout.Size = new System.Drawing.Size(100, 28);
            this.buttonBackAbout.TabIndex = 3;
            this.buttonBackAbout.Text = "Kembali";
            this.buttonBackAbout.UseVisualStyleBackColor = false;
            this.buttonBackAbout.Click += new System.EventHandler(this.buttonBackAbout_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelDesc, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelAboutTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonBackAbout, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1016, 457);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // UCAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UCAbout";
            this.Size = new System.Drawing.Size(1016, 457);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelAboutTitle;
        private System.Windows.Forms.Label labelDesc;
        private System.Windows.Forms.Button buttonBackAbout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
