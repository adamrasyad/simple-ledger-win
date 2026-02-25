
namespace CSV_Accounting
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panelContent = new System.Windows.Forms.Panel();
            this.pictureBoxToggleNightMode = new System.Windows.Forms.PictureBox();
            this.checkBoxAccountingTerm = new System.Windows.Forms.CheckBox();
            this.comboBoxLang = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxToggleNightMode)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 0);
            this.panelContent.Margin = new System.Windows.Forms.Padding(4);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1167, 488);
            this.panelContent.TabIndex = 3;
            // 
            // pictureBoxToggleNightMode
            // 
            this.pictureBoxToggleNightMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxToggleNightMode.Image = global::CSV_Accounting.Properties.Resources.CSV_App_Moon_Icon;
            this.pictureBoxToggleNightMode.Location = new System.Drawing.Point(4, 4);
            this.pictureBoxToggleNightMode.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxToggleNightMode.Name = "pictureBoxToggleNightMode";
            this.pictureBoxToggleNightMode.Size = new System.Drawing.Size(33, 31);
            this.pictureBoxToggleNightMode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxToggleNightMode.TabIndex = 7;
            this.pictureBoxToggleNightMode.TabStop = false;
            this.pictureBoxToggleNightMode.Click += new System.EventHandler(this.pictureBoxToggleNightMode_Click);
            this.pictureBoxToggleNightMode.MouseEnter += new System.EventHandler(this.pictureBoxToggleNightMode_MouseEnter);
            this.pictureBoxToggleNightMode.MouseLeave += new System.EventHandler(this.pictureBoxToggleNightMode_MouseLeave);
            this.pictureBoxToggleNightMode.MouseHover += new System.EventHandler(this.pictureBoxToggleNightMode_MouseHover);
            // 
            // checkBoxAccountingTerm
            // 
            this.checkBoxAccountingTerm.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxAccountingTerm.AutoSize = true;
            this.checkBoxAccountingTerm.Location = new System.Drawing.Point(45, 4);
            this.checkBoxAccountingTerm.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxAccountingTerm.Name = "checkBoxAccountingTerm";
            this.checkBoxAccountingTerm.Size = new System.Drawing.Size(150, 26);
            this.checkBoxAccountingTerm.TabIndex = 8;
            this.checkBoxAccountingTerm.Text = "Pakai Istilah Akuntansi";
            this.checkBoxAccountingTerm.UseVisualStyleBackColor = true;
            this.checkBoxAccountingTerm.CheckedChanged += new System.EventHandler(this.checkBoxAccountingTerm_CheckedChanged);
            // 
            // comboBoxLang
            // 
            this.comboBoxLang.FormattingEnabled = true;
            this.comboBoxLang.Items.AddRange(new object[] {
            "Bahasa Indonesia",
            "English",
            "العربية"});
            this.comboBoxLang.Location = new System.Drawing.Point(203, 4);
            this.comboBoxLang.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxLang.Name = "comboBoxLang";
            this.comboBoxLang.Size = new System.Drawing.Size(160, 24);
            this.comboBoxLang.TabIndex = 9;
            this.comboBoxLang.SelectedIndexChanged += new System.EventHandler(this.comboBoxLang_SelectedIndexChanged);
            this.comboBoxLang.DropDownClosed += new System.EventHandler(this.comboBoxLang_DropDownClosed);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.pictureBoxToggleNightMode);
            this.flowLayoutPanel1.Controls.Add(this.checkBoxAccountingTerm);
            this.flowLayoutPanel1.Controls.Add(this.comboBoxLang);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 488);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1167, 70);
            this.flowLayoutPanel1.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 558);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxToggleNightMode)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.PictureBox pictureBoxToggleNightMode;
        private System.Windows.Forms.CheckBox checkBoxAccountingTerm;
        private System.Windows.Forms.ComboBox comboBoxLang;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}

