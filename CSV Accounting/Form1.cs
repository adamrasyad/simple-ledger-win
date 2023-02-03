using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

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
        public Form1()
        {
            //state 1
            InitializeComponent();
            this.Text = "Pembuat Arsip Akuntansi CSV";
            panel1.Visible = false;
            dataGridView1.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            label1.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            panel2.Visible = false;

            comboBox1.SelectedItem = "Bahasa Indonesia";

            // table header generate
            dataGridView1.Columns.Add("Masukan", "Masukan");
            dataGridView1.Columns.Add("Keluaran", "Keluaran");
            dataGridView1.Columns.Add("Total", "Total");
            dataGridView1.Columns.Add("Timbangan", "Timbangan");
            //dataGridView1.Columns.Add("Waktu", "Waktu");
            dataGridView1.Columns["Masukan"].ReadOnly = false; 
            dataGridView1.Columns["Keluaran"].ReadOnly = false;
            dataGridView1.Columns["Total"].ReadOnly = true;
            dataGridView1.Columns["Timbangan"].ReadOnly = true;
            //dataGridView1.Columns["Waktu"].ReadOnly = false;
            //dataGridView1.Rows[0].ReadOnly = true; //header are readonly
        }
        string dateTime = "Terakhir diedit pada : ";
        bool askToSave = false;
        private void button1_Click(object sender, EventArgs e)
        {            
            //state 2 : ADD
            panel1.Visible = true;
            dataGridView1.Visible = true;
            button6.Visible = true;
            button5.Visible = true;
            button4.Visible = true;
            label4.Text = dateTime + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }
        bool isOpeningFile = false;
        string thePath = "";
        string theFileNameWithoutPath = "";
        private void button2_Click(object sender, EventArgs e)
        {
            //state 3 : OPEN AND EDIT
            OpenFileDialog chooseFileDialog = new OpenFileDialog();
            chooseFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            chooseFileDialog.FilterIndex = 1;
            if (chooseFileDialog.ShowDialog() == DialogResult.OK) //ketika dialog muncul
            {
                dataGridView1.Rows.Clear();
                thePath = chooseFileDialog.FileName; //path diambil disimpan di variabel itu
                theFileNameWithoutPath = System.IO.Path.GetFileName(thePath);

                using (StreamReader streamReader = new StreamReader(thePath))
                {
                    //string headerLine = streamReader.ReadLine(); //skip first line
                    string stringLine = streamReader.ReadToEnd();
                    string[] stringArray = stringLine.Split(',');
                    string headerLine = "";
                    int j = 0;
                    for (j = 0; j < dataGridView1.Columns.Count; j++)  //skip first line
                    {
                        headerLine += stringArray[j];
                    }
                    /*foreach (string value in stringArray)
                    {
                        dataGridView1.Columns.Add(value.Trim());
                    }
                    */
                    int i = -1;
                    /*while(stringArray[i] != null) //means read each line because each column accessed via loop nested below
                    {
                        for(int j = i; j < dataGridView1.Columns.Count; j++) //means insert each columns in current row
                        {
                            dataGridView1.Rows.Add("0", "0", "0");
                            dataGridView1.Columns["Masukan"].ReadOnly = false;
                            dataGridView1.Columns["Keluaran"].ReadOnly = false;
                            dataGridView1.Rows[i].Cells[j].Value = stringArray[j].Trim();
                        }
                        i++;
                    }*/
                    j = 0;
                    int k = 0;
                    foreach (string values in stringArray) //iterasi setiap cell
                    {/*
                        if (values != "Masukan" && values != "Keluaran" && values != "Total")
                        {
                            for (int j = 0; j < dataGridView1.Columns.Count; j++) //means insert each columns in current row
                            {
                                dataGridView1.Rows.Add("0", "0", "0");
                                dataGridView1.Columns["Masukan"].ReadOnly = false;
                                dataGridView1.Columns["Keluaran"].ReadOnly = false;
                                dataGridView1.Rows[i].Cells[j].Value = values.Trim();
                            }
                            i++;
                        }
                        */
                        if ((values != "Masukan" && values != "Keluaran" && values != "Total" && values != "Timbangan") &&
                            (values != "Debit" && values != "Kredit" && values != "Total" && values != "Saldo") &&
                            (values != "Inflow" && values != "Outflow" && values != "Total" && values != "Balance") &&
                            (values != "Debit" && values != "Credit" && values != "Total" && values != "Balance") &&
                            (values != "Debit" && values != "Kredit" && values != "Total" && values != "Saldo") &&
                            (values != "الداخل" && values != "الخارج" && values != "المجموع" && values != "الرصيد") &&
                            (values != "الخصم" && values != "الائتمان" && values != "المجموع" && values != "الرصيد")
                            &&
                            j != stringArray.Length - dataGridView1.Columns.Count - 1)
                        {
                            if ((j % dataGridView1.Columns.Count) == 0 || j == 0) //new row
                            {
                                dataGridView1.Rows.Add("0", "0", "0", "0");
                                dataGridView1.Columns["Masukan"].ReadOnly = false;
                                dataGridView1.Columns["Keluaran"].ReadOnly = false;
                                i++;
                                k = 0;
                                dataGridView1.Rows[i].Cells[k].Value = values.Trim();
                                k++;
                            }
                            else
                            {
                                dataGridView1.Rows[i].Cells[k].Value = values.Trim();
                                k++;
                            }
                            j++;
                            // Debugging Through Console
                            /*
                            Console.WriteLine("Columns : " + dataGridView1.Columns.Count);
                            Console.WriteLine("iteration (after CSV header) " + (j-1) + " -> " + values + 
                                " is placed in cell[" + (k-1) + "]" + " row[" + (i-1) + "]");
                            Console.WriteLine("Until : " + stringArray.Length);
                            */
                        }
                    }
                    //table done written
                }
                isOpeningFile = true;
                label6.Visible = true;
                label6.Text = label6.Text + theFileNameWithoutPath;
                panel1.Visible = true;
                dataGridView1.Visible = true;
                button6.Visible = true;
                button5.Visible = true;
                button4.Visible = true;
                string theCurrentTime = System.IO.File.GetLastWriteTime(thePath).ToString("dd/MM/yyyy HH:mm");
                label4.Text = dateTime + theCurrentTime;
                Array.Resize(ref total, dataGridView1.RowCount);
                Array.Resize(ref balance, dataGridView1.RowCount);
                foreach(DataGridViewRow dGRow in dataGridView1.Rows)
                {
                    total[dGRow.Index] = decimal.Parse(dGRow.Cells["Keluaran"].Value.ToString());
                    total[dGRow.Index] += decimal.Parse(dGRow.Cells["Masukan"].Value.ToString());
                    for(int i=0; i < 2; i++)
                    preventError(dataGridView1, new DataGridViewCellEventArgs(i, dGRow.Index));
                    totalCellCount(dataGridView1, new DataGridViewCellEventArgs(dGRow.Cells["Total"].ColumnIndex, dGRow.Index));
                    balanceCellCount(dataGridView1, new DataGridViewCellEventArgs(dGRow.Cells["Total"].ColumnIndex, dGRow.Index));
                }
                
                
                //display the table
            }
        }
        //state 1
        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            dataGridView1.Visible = false;
            button6.Visible = false;
            button5.Visible = false;
            button4.Visible = false;
        }
        int rowsAmount = 0;
        private void button5_Click(object sender, EventArgs e)
        {
            //state 5 : FILL
            dataGridView1.Rows.Add("0", "0", "0", "0");
            dataGridView1.Columns["Masukan"].ReadOnly = false;
            dataGridView1.Columns["Keluaran"].ReadOnly = false;
            rowsAmount += 1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //state 2 : ADD : SAVEnhnhgjg
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV File|*.csv";
            saveFileDialog.FileName = "Accounting " + DateTime.Now.ToString("ddMMyyyyHHmmss");
            saveFileDialog.Title = "Save CSV File";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string theSavePath = saveFileDialog.FileName;
                using(StreamWriter streamWriter = new StreamWriter(File.Create(theSavePath)))
                {
                    // Streamwrite header first
                    for (int i = 0; i < dataGridView1.Columns.Count; i++) {
                        streamWriter.Write(dataGridView1.Columns[i].HeaderText);
                        streamWriter.Write(",");        
                    }
                    
                    foreach (DataGridViewRow dataGRow in dataGridView1.Rows)
                    {
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            streamWriter.Write(dataGRow.Cells[i].Value);
                            streamWriter.Write(",");
                        }
                    }
                    streamWriter.Close();
                }
                // csv file created and saved
                dataGridView1.Rows.Clear();
                Array.Clear(total, 0, total.Length);
                label7.Visible = true;
                askToSave = false;
            }
        }
        private void preventTotalHeaderColumnError(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells["Total"].Value == null || dataGridView1.Rows[e.RowIndex].Cells["Total"].Value.ToString() == "")
                dataGridView1.Rows[e.RowIndex].Cells["Total"].Value = "0";
        }

        bool isJustCleared = false;
        Queue<string> beforeClearedDGVCellValues = new Queue<string>();
        int rowsCountBeforeCleared = 0;
        int columnsCountBeforeCleared = 0;
        private void button7_Click(object sender, EventArgs e)
        {
            // clear table

            //back up data grid view values
            int j = 0;
            rowsCountBeforeCleared = dataGridView1.Rows.Count;
            columnsCountBeforeCleared = dataGridView1.Columns.Count;
            foreach (DataGridViewRow dataGRow in dataGridView1.Rows)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    preventError(this.dataGridView1, new DataGridViewCellEventArgs(i, j));
                    preventTotalHeaderColumnError(this.dataGridView1, new DataGridViewCellEventArgs(i, j));
                    beforeClearedDGVCellValues.Enqueue(dataGridView1.Rows[j].Cells[i].Value.ToString());
                }
                j++;
            }

            isJustCleared = true;
            dataGridView1.Rows.Clear();
            Array.Clear(total, 0, total.Length);
            isOpeningFile = false;
            label7.Visible = false;
        }
        decimal[] total = { };
        decimal[] balance = { };
        Stack<int> lastEditedRowIndex = new Stack<int>();
        Stack<int> lastEditedColumnIndex = new Stack<int>();
        Stack<string> lastEditedCellValue = new Stack<string>();
        private void preventError(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value == null || dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString() == ""))
                dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value = "0";
            if (e.RowIndex >= 0 && (dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value == null || dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString() == ""))
                dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value = "0";
            /*if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "Masukan" && (dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value == null || dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString() == ""))
            {
                sequencesOfEditedColumnIndexes.Push(e.RowIndex);
                sequencesOfEditedRowIndexes.Push(dataGridView1.Columns["Masukan"].Index);
                sequencesOfPreviousCellValue.Push(dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString());
            } else if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "Keluaran" && (dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value == null || dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString() == ""))
            {
                sequencesOfEditedColumnIndexes.Push(e.RowIndex);
                sequencesOfEditedRowIndexes.Push(dataGridView1.Columns["Keluaran"].Index);
                sequencesOfPreviousCellValue.Push(dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString());
            }*/
                // we use these below if TryParse accept /, +, -, and * thus return true, because total wont accept these operators
                if (e.RowIndex >= 0 && (dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString().Contains(",")))
                dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value = dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString().Replace(",", String.Empty);
            else if (e.RowIndex >= 0 && (dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString().Contains("/")))
                dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value = dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString().Replace("/", String.Empty);
            else if (e.RowIndex >= 0 && (dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString().Contains("+")))
                dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value = dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString().Replace("+", String.Empty);
            else if (e.RowIndex >= 0 && (dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString().Contains("-")))
                dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value = dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString().Replace("-", String.Empty);
            else if (e.RowIndex >= 0 && (dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString().Contains("*")))
                dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value = dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString().Replace("*", String.Empty);

            if (e.RowIndex >= 0 && (dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString().Contains(",")))
                dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value = dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString().Replace(",", String.Empty);
            else if (e.RowIndex >= 0 && (dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString().Contains("/")))
                dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value = dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString().Replace("/", String.Empty);
            else if (e.RowIndex >= 0 && (dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString().Contains("+")))
                dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value = dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString().Replace("+", String.Empty);
            else if (e.RowIndex >= 0 && (dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString().Contains("-")))
                dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value = dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString().Replace("-", String.Empty);
            else if (e.RowIndex >= 0 && (dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString().Contains("*")))
                dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value = dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString().Replace("*", String.Empty);
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Console.WriteLine(dataGridView1.CurrentCell.OwningColumn.Name);
            Array.Resize(ref total, dataGridView1.RowCount);
            Array.Resize(ref balance, dataGridView1.RowCount);
            preventError(sender, e);
            //string cellValue = dataGridView1.Rows[(int)e.RowIndex]
            /*
            if (decimal.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out _) 
                && 
                dataGridView1.Columns[e.ColumnIndex].HeaderText == "Masukan")
            */
            if (decimal.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out _)
            &&
            dataGridView1.CurrentCell.OwningColumn.Name == "Masukan")
            {
                total[e.RowIndex] = 0;
                label1.Visible = false;
                // Debugging Through Console
                //Console.WriteLine("Keluaran : \"" + dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString() + "\"");
                total[e.RowIndex] += decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                if (decimal.TryParse(dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString(), out _))
                    total[e.RowIndex] -= decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["Keluaran"].Value.ToString());
            }
            else if (decimal.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out _)
                &&
            //    dataGridView1.Columns[e.ColumnIndex].HeaderText == "Keluaran")
            dataGridView1.CurrentCell.OwningColumn.Name == "Keluaran")
            {
                total[e.RowIndex] = 0;
                label1.Visible = false;
                total[e.RowIndex] -= decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                if(decimal.TryParse(dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString(), out _))
                    total[e.RowIndex] += decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["Masukan"].Value.ToString());
            }
            else
            {
                label1.Visible = true;
            }
            totalCellCount(sender, e);
            balanceCellCount(sender, e);
        }
        private void totalCellCount(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.RowIndex < total.Length) //not out of bound
                dataGridView1.Rows[e.RowIndex].Cells["Total"].Value = total[e.RowIndex].ToString();
        }
        private void balanceCellCount(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < total.Length)
            {
                int k = 0;
                foreach (DataGridViewRow dataGRow in dataGridView1.Rows)
                {
                    if (k < balance.Length)
                    {
                        balance[k] = 0;
                        for (int j = 0; j <= k; j++) //totalkan value di kolom Total hingga didapatkan Balance dari row awal hingga row tersebut
                        {
                            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                            {
                                if (dataGridView1.Columns[i].Name /*HeaderText */ == "Total") //pakai telusuri kolom agar lebih jelas konteksnya; yaitu mendapatkan value di total
                                    balance[k] += total[j];
                            }
                        }
                        dataGridView1.Rows[k].Cells["Timbangan"].Value = balance[k].ToString(); //tuliskan balance[k] di timbangan
                    }
                    k++;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridView1_CellClick(sender, e);
            askToSave = true;

            isJustCleared = false; //welp, this event mean its filled not just cleared thus empty

            if (e.RowIndex >= 0 && e.ColumnIndex != dataGridView1.Columns["Total"].Index && e.ColumnIndex != dataGridView1.Columns["Timbangan"].Index)
            {
                preventError(sender, e);
                totalCellCount(sender, e);
                balanceCellCount(sender, e);

                // for undo-redo feature
                sequencesOfEditedColumnIndexes.Push(e.ColumnIndex);
                sequencesOfEditedRowIndexes.Push(e.RowIndex);
                sequencesOfPreviousCellValue.Push(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                
                //Console.WriteLine("ADDED!!! row[" + sequencesOfEditedRowIndexes.Peek() + "]" + " Cell[" + sequencesOfEditedColumnIndexes.Peek() + "]"
                //    + " with value " + sequencesOfPreviousCellValue.Peek());
            }
            /*if (sequencesOfPreviousCellValue.Count > 0)
            {
                decimal[] array = { };
                array = new decimal[sequencesOfPreviousCellValue.Count];
                decimal.Parse(sequencesOfPreviousCellValue.Peek());
                Console.WriteLine("[{0}]", string.Join(", ", array));
            }*/
            //Console.WriteLine("Row just added ? " + isRowJustAdded);
            string theCurrentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            if (e.RowIndex > -1)
                label4.Text = dateTime + theCurrentTime;
            /*if (
                isRowJustAdded
                &&
                (sequencesOfEditedColumnIndexes.Count % 3 == 0 ||
                sequencesOfEditedRowIndexes.Count % 3 == 0 ||
                sequencesOfPreviousCellValue.Count % 3 == 0)
                &&
                (sequencesOfEditedColumnIndexes.Count > 0 ||
                sequencesOfEditedRowIndexes.Count > 0 ||
                sequencesOfPreviousCellValue.Count > 0)
                )
            {
                sequencesOfEditedColumnIndexes.Pop();
                sequencesOfEditedRowIndexes.Pop();
                sequencesOfPreviousCellValue.Pop();
                sequencesOfEditedColumnIndexes.Pop();
                sequencesOfEditedRowIndexes.Pop();
                sequencesOfPreviousCellValue.Pop();
                isRowJustAdded = false;
                //if(sequencesOfPreviousCellValue.Count > 0)
                //    Console.WriteLine("Cleaned two stack and remains : " + sequencesOfPreviousCellValue.Peek());
                //else
                //    Console.WriteLine("Empty after clean!!! Error detected!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
            if (sequencesOfPreviousCellValue.Count > 0)
            {
                decimal[] array = { };
                array = new decimal[sequencesOfPreviousCellValue.Count];
                decimal.Parse(sequencesOfPreviousCellValue.Peek());
                for(int i = 0; i<sequencesOfPreviousCellValue.Count; i++)
                {
                    array[i] = decimal.Parse(sequencesOfPreviousCellValue.Skip(i).First());
                }
                Console.WriteLine("[{0}]", string.Join(", ", array));
            }
            else
                Console.WriteLine("Empty after clean!!! Error detected!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            */
        }

        bool flag = false;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            flag = !flag;
            if (flag)
            {
                pictureBox1.Image = Properties.Resources.CSV_App_Sun_Icon;
                SwitchDesign();
            }
            else
            {
                pictureBox1.Image = Properties.Resources.CSV_App_Moon_Icon;
                SwitchDesign();
            }
                //Debug with Console
                //Console.WriteLine(pictureBox1.Image.ToString());
            
        }

        //thanks to https://stackoverflow.com/questions/67311404/how-to-create-a-switch-to-select-a-dark-theme-for-windows-form-that-can-darken
        //answered May 1, 2021 at 19:12 by Maik8
        private void SwitchDesign()
        {
            if (flag)
            {
                //SWITCH TO DARK
                Color colorBackground = Color.FromArgb(32, 32, 32);
                Color colorFont = Color.White;
                Color colorButtonBack = Color.FromArgb(23, 23, 23);
                //lets change...
                this.ForeColor = colorFont;
                this.BackColor = colorBackground;
                //Now for every special-control that does need an extra color / property to be set use something like this
                foreach (Button button in this.Controls.OfType<Button>())
                {
                    button.BackColor = colorButtonBack;
                    button.ForeColor = colorFont;
                }
                foreach (Label label in this.Controls.OfType<Label>())
                {
                    label.ForeColor = colorFont;
                }
                checkBox1.BackColor = colorBackground;
                checkBox1.ForeColor = colorFont;
                comboBox1.BackColor = colorBackground;
                comboBox1.ForeColor = colorFont;
                //child of panels
                foreach (Button button in panel1.Controls.OfType<Button>())
                {
                    button.BackColor = colorButtonBack;
                }
                foreach (DataGridView dataGridView in panel1.Controls.OfType<DataGridView>())
                {
                    dataGridView.Columns["Masukan"].DefaultCellStyle.BackColor = colorButtonBack;
                    dataGridView.Columns["Keluaran"].DefaultCellStyle.BackColor = colorButtonBack;
                    dataGridView.Columns["Total"].DefaultCellStyle.BackColor = colorButtonBack;
                    dataGridView.Columns["Timbangan"].DefaultCellStyle.BackColor = colorButtonBack;
                }
                foreach (Label label in panel1.Controls.OfType<Label>())
                {
                    label.ForeColor = colorFont;
                }
                foreach (Button button in panel2.Controls.OfType<Button>())
                {
                    button.BackColor = colorButtonBack;
                }
                //error labels or alert texts
                label1.BackColor = Color.DarkRed;
                label7.BackColor = Color.DarkGreen;
                //You could now add more controls in a similar fashion.
                this.Invalidate(); //Forces a re-draw of your controls / form
            }
            else
            {
                //SWITCH TO BRIGHT
                Color colorBackground = SystemColors.Control;
                Color colorFont = Color.Black;
                Color colorButtonBack = SystemColors.ControlLight;
                Color colorDataGridViewBack = Color.White;
                //lets change...
                this.ForeColor = colorFont;
                this.BackColor = colorBackground;
                //Now for every special-control that does need an extra color / property to be set use something like this
                foreach (Button button in this.Controls.OfType<Button>())
                {
                    button.BackColor = colorButtonBack;
                    button.ForeColor = colorFont;
                }
                foreach (Label label in this.Controls.OfType<Label>())
                {
                    label.ForeColor = colorFont;
                }
                checkBox1.BackColor = colorBackground;
                checkBox1.ForeColor = colorFont;
                comboBox1.BackColor = colorBackground;
                comboBox1.ForeColor = colorFont;
                //child of panels
                foreach (Button button in panel1.Controls.OfType<Button>())
                {
                    button.BackColor = colorButtonBack;
                }
                foreach (Button button in panel2.Controls.OfType<Button>())
                {
                    button.BackColor = colorButtonBack;
                }
                foreach (DataGridView dataGridView in panel1.Controls.OfType<DataGridView>())
                {
                    dataGridView.Columns["Masukan"].DefaultCellStyle.BackColor = colorDataGridViewBack;
                    dataGridView.Columns["Keluaran"].DefaultCellStyle.BackColor = colorDataGridViewBack;
                    dataGridView.Columns["Total"].DefaultCellStyle.BackColor = colorDataGridViewBack;
                    dataGridView.Columns["Timbangan"].DefaultCellStyle.BackColor = colorDataGridViewBack;
                }
                foreach (Label label in panel1.Controls.OfType<Label>())
                {
                    label.ForeColor = colorFont;
                }
                //error labels or alert texts
                label1.BackColor = Color.Transparent;
                label1.ForeColor = Color.Red;
                label7.BackColor = Color.Transparent;
                label7.ForeColor = Color.DarkGreen;
                //You could now add more controls in a similar fashion.
                this.Invalidate(); //Forces a re-draw of your controls / form
            }
        }
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            //nvm
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            Size size = new Size(25, 25);
            pictureBox1.Size = size;
            pictureBox1.Top += 5;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            Size size = new Size(30, 30);
            pictureBox1.Size = size;
            pictureBox1.Top -= 5;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (isOpeningFile)
            {
                //save directly without dialogue
                using (StreamWriter streamWriter = new StreamWriter(this.thePath))
                {
                    // Streamwrite header first
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        streamWriter.Write(dataGridView1.Columns[i].HeaderText);
                        streamWriter.Write(",");
                    }

                    foreach (DataGridViewRow dataGRow in dataGridView1.Rows)
                    {
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            streamWriter.Write(dataGRow.Cells[i].Value);
                            streamWriter.Write(",");
                        }
                    }
                    streamWriter.Close();
                    label7.Visible = true;
                    askToSave = false;
                }
            }
            else
                button6_Click(sender, e); //use Save As ...
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            int j = 0;
            foreach (DataGridViewRow dataGRow in dataGridView1.Rows)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Rows[j].Cells["Masukan"].Value == null || dataGridView1.Rows[j].Cells["Masukan"].Value.ToString() == "")
                        


                }
                j++;
            }
            */
            string caption = "";
            string theTitle = "";
            switch (selected)
            {
                case "Bahasa Indonesia":
                    {
                        caption = "Simpan sebelum tutup?";
                        theTitle = "Simpan CSV";
                        break;
                    }
                case "English":
                    {
                        caption = "Save before closing?";
                        theTitle = "Save CSV";
                        break;
                    }
                case "العربية":
                    {
                        caption = "حفظ قبل الإغلاق؟";
                        theTitle = "حفظ ملف سي إس في";
                        break;
                    }
            }
            bool stopLoop = false;
            while (!stopLoop)
            {
                if (askToSave)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                    MessageBoxIcon ico = MessageBoxIcon.Question;
                    DialogResult result;
                    result = MessageBox.Show(this, caption, theTitle, buttons, ico);
                    if (result == DialogResult.Yes)
                        button9_Click(sender, e);
                    else if (result == DialogResult.Cancel)
                    {
                        stopLoop = true;
                        e.Cancel = true;
                    }
                    else if (result == DialogResult.No)
                        stopLoop = true;
                }
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            //UNDO
            /*
            if (sequencesOfPreviousCellValue.Count() != 0
                && sequencesOfEditedRowIndexes.Count() != 0
                && sequencesOfEditedColumnIndexes.Count() != 0)
            {
                string[] array = { };
                Array.Resize(ref array, sequencesOfPreviousCellValue.Count());
                array[sequencesOfPreviousCellValue.Count()-1] = sequencesOfPreviousCellValue.Peek();
                Console.WriteLine("UNDO values STACK --> [{0}]", string.Join(", ", array));
            }*/
            // Debug with Console
            //Console.WriteLine("Row[" + sequencesOfEditedRowIndexes.Peek() + "]" + " Cell[" + sequencesOfEditedColumnIndexes.Peek()  + "]"
            //    + " with value " + sequencesOfPreviousCellValue.Peek());
            if (isJustCleared)
            {
                for (int j = 0; j < rowsCountBeforeCleared; j++)
                {
                    if (j != rowsCountBeforeCleared - 1)
                    {
                        dataGridView1.Rows.Add("0", "0", "0", "0");
                        dataGridView1.Columns["Masukan"].ReadOnly = false;
                        dataGridView1.Columns["Keluaran"].ReadOnly = false;
                    }
                    for (int i = 0; i < columnsCountBeforeCleared; i++)
                    {
                        dataGridView1.Rows[j].Cells[i].Value = beforeClearedDGVCellValues.Dequeue();
                    }
                }
            }
            else
            {
                lastEditedCellValue.Push(
                dataGridView1.Rows[sequencesOfEditedRowIndexes.Peek()].Cells[sequencesOfEditedColumnIndexes.Peek()].Value.ToString());
                lastEditedColumnIndex.Push(sequencesOfEditedColumnIndexes.Pop());
                lastEditedRowIndex.Push(sequencesOfEditedRowIndexes.Pop());
                Console.WriteLine(dataGridView1.CancelEdit());
            }
            
            /*
            else if (sequencesOfPreviousCellValue.Count() != 0
                && sequencesOfEditedRowIndexes.Count() != 0
                && sequencesOfEditedColumnIndexes.Count() != 0)
            {
                //removing same thing or recently on top, because the top is assigend with value changed recently
                //if (sequencesOfPreviousCellValue.Count() == 1 && sequencesOfEditedRowIndexes.Count() == 1 && sequencesOfEditedColumnIndexes.Count() == 1)

                // Debug with Console
                /*Console.WriteLine("Row[" + sequencesOfEditedRowIndexes.Peek() + "]" + " Cell[" + sequencesOfEditedColumnIndexes.Peek()  + "]"
                    + " with value " + sequencesOfPreviousCellValue.Peek());
                *//*
                if   (sequencesOfPreviousCellValue.Count() > 1
                && sequencesOfEditedRowIndexes.Count() > 1
                && sequencesOfEditedColumnIndexes.Count() > 1)
                {
                        sequencesOfPreviousCellValue.Pop();
                    sequencesOfEditedRowIndexes.Pop();
                    sequencesOfEditedColumnIndexes.Pop();
                }*/
                /*
                if (sequencesOfPreviousCellValue.Count() != 0
                    && sequencesOfEditedRowIndexes.Count() != 0
                    && sequencesOfEditedColumnIndexes.Count() != 0)
                {
                    Console.WriteLine("Row[" + sequencesOfEditedRowIndexes.Peek() + "]" + " Cell[" + sequencesOfEditedColumnIndexes.Peek() + "]"
                    + " with value " + sequencesOfPreviousCellValue.Peek());
                    sequencesOfPreviousCellValue.Pop();
                    sequencesOfEditedRowIndexes.Pop();
                    sequencesOfEditedColumnIndexes.Pop();
                }*/
                /*if (sequencesOfPreviousCellValue.Count() != 0
                    && sequencesOfEditedRowIndexes.Count() != 0
                    && sequencesOfEditedColumnIndexes.Count() != 0)
                {
                    //Console.WriteLine("Row[" + sequencesOfEditedRowIndexes.Peek() + "]" + " Cell[" + sequencesOfEditedColumnIndexes.Peek() + "]"
                    //+ " with value " + sequencesOfPreviousCellValue.Peek());
                    //for redo feature
                    lastEditedRowIndex.Push(sequencesOfEditedRowIndexes.Peek());
                    lastEditedColumnIndex.Push(sequencesOfEditedColumnIndexes.Peek());
                    lastEditedCellValue
                        .Push(dataGridView1.Rows[sequencesOfEditedRowIndexes.Peek()].Cells[sequencesOfEditedColumnIndexes.Peek()].Value.ToString());

                    //for recounting total
                    int undoneColumnIndex = sequencesOfEditedColumnIndexes.Peek();
                    int undoneRowIndex = sequencesOfEditedRowIndexes.Peek();

                    //undoing
                    dataGridView1
                        .Rows[sequencesOfEditedRowIndexes.Pop()]
                        .Cells[sequencesOfEditedColumnIndexes.Pop()]
                        .Value = sequencesOfPreviousCellValue.Pop();

                    // Debug with Console
                    //Console.WriteLine("Undo result : " + dataGridView1.Rows[undoneRowIndex].Cells[undoneColumnIndex].Value);

                    //recounting total
                    dataGridView1_CellEndEdit(
                        this.dataGridView1,
                        new DataGridViewCellEventArgs(undoneColumnIndex, undoneRowIndex)
                        );
                }
            }*/
            //else
            //    Console.WriteLine("stack is empty!!!");
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (lastEditedCellValue.Count() != 0 && lastEditedRowIndex.Count() != 0 && lastEditedColumnIndex.Count() != 0)
            {
                string[] array = { };
                Array.Resize(ref array, lastEditedCellValue.Count);
                array[lastEditedCellValue.Count() - 1] = lastEditedCellValue.Peek();
                Console.WriteLine("[{0}]", string.Join(", ", array));
            }
            //REDO
            if (lastEditedCellValue.Count() != 0 && lastEditedRowIndex.Count() != 0 && lastEditedColumnIndex.Count() != 0)
            {
                int redoneColumnIndex = lastEditedColumnIndex.Peek();
                int redoneRowIndex = lastEditedRowIndex.Peek();
                dataGridView1.Rows[lastEditedRowIndex.Pop()].Cells[lastEditedColumnIndex.Pop()].Value = lastEditedCellValue.Pop();
                /*dataGridView1_CellEndEdit(
                        this.dataGridView1,
                        new DataGridViewCellEventArgs(redoneColumnIndex, redoneRowIndex)
                        );*/
                preventError(
                        this.dataGridView1,
                        new DataGridViewCellEventArgs(redoneColumnIndex, redoneRowIndex)
                        );
                if (dataGridView1.Columns[redoneColumnIndex].Name == "Keluaran" && decimal.TryParse(dataGridView1.Rows[redoneRowIndex].Cells[redoneColumnIndex].Value.ToString(), out _))
                {
                    total[redoneRowIndex] = -1 * decimal.Parse(dataGridView1.Rows[redoneRowIndex].Cells[redoneColumnIndex].Value.ToString());
                    if (decimal.TryParse(dataGridView1.Rows[redoneRowIndex].Cells["Masukan"].Value.ToString(), out _))
                        total[redoneRowIndex] += decimal.Parse(dataGridView1.Rows[redoneRowIndex].Cells["Masukan"].Value.ToString());
                }

                else if (dataGridView1.Columns[redoneColumnIndex].Name == "Masukan" && decimal.TryParse(dataGridView1.Rows[redoneRowIndex].Cells[redoneColumnIndex].Value.ToString(), out _))
                {
                    total[redoneRowIndex] = decimal.Parse(dataGridView1.Rows[redoneRowIndex].Cells[redoneColumnIndex].Value.ToString());
                    if (decimal.TryParse(dataGridView1.Rows[redoneRowIndex].Cells["Masukan"].Value.ToString(), out _))
                        total[redoneRowIndex] -= decimal.Parse(dataGridView1.Rows[redoneRowIndex].Cells["Keluaran"].Value.ToString());
                }
                //Console.WriteLine("before recount total : " + total[redoneRowIndex]);
                totalCellCount(
                        this.dataGridView1,
                        new DataGridViewCellEventArgs(redoneColumnIndex, redoneRowIndex)
                        );
                balanceCellCount(
                        this.dataGridView1,
                        new DataGridViewCellEventArgs(redoneColumnIndex, redoneRowIndex)
                        );
                //Console.WriteLine("redo total counting in column " + redoneColumnIndex + " and row " + redoneRowIndex + 
                    //" with result " + total[redoneRowIndex]);
            }
        }
        Stack<int> sequencesOfEditedRowIndexes = new Stack<int>();
        Stack<int> sequencesOfEditedColumnIndexes = new Stack<int>();
        Stack<string> sequencesOfPreviousCellValue = new Stack<string>();
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            //if (e.RowIndex != null && e.ColumnIndex != null)
            //{
                sequencesOfEditedColumnIndexes.Push(e.ColumnIndex);
                sequencesOfEditedRowIndexes.Push(e.RowIndex);
                sequencesOfPreviousCellValue.Push(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            //}
            */
        }
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {/*
            foreach(DataGridViewRow dataGRow in dataGridView1.Rows)
            {
                foreach (DataGridViewColumn dataGColumn in dataGridView1.Columns)
                {
                    if (dataGRow.Cells[dataGColumn.Index].Value == null || dataGRow.Cells[dataGColumn.Index].Value.ToString() == "")
                        dataGRow.Cells[dataGColumn.Index].Value = 0;
                }
            }*/
            /*
             * //if (e.RowIndex != null && e.ColumnIndex != null)
            //{
            sequencesOfEditedColumnIndexes.Push(e.ColumnIndex);
            sequencesOfEditedRowIndexes.Push(e.RowIndex);
            sequencesOfPreviousCellValue.Push(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            //}
            
            if(dataGridView1.Columns[e.ColumnIndex].HeaderText == "Waktu")
            {
                DateTimePicker dateTimePicker = new DateTimePicker();
                dateTimePicker.Format = DateTimePickerFormat.Short;
                dateTimePicker.Visible = true;
                dateTimePicker.Value = DateTime.Parse(dataGridView1.CurrentCell.Value.ToString());

                var rectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                dateTimePicker.Size = new Size(rectangle.Width, rectangle.Height);
                dateTimePicker.Location = new Point(rectangle.X, rectangle.Y);

                dateTimePicker.CloseUp += new EventHandler(dateTimePicker_CloseUp);
                dateTimePicker.TextChanged += new EventHandler(dateTimePicker_OnTextChange);

                dataGridView1.Controls.Add(dateTimePicker);
            }
            */
        }
        bool toggle = false;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            toggle = !toggle;
            if (toggle)
                SwitchLang();
            else
                SwitchLang();
            //Debug with Console
            //Console.WriteLine(pictureBox1.Image.ToString());

        }

        //thanks to https://stackoverflow.com/questions/67311404/how-to-create-a-switch-to-select-a-dark-theme-for-windows-form-that-can-darken
        //answered May 1, 2021 at 19:12 by Maik8
        string selected = "Bahasa Indonesia";
        private void SwitchLang()
        {
            selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            switch (selected)
            {
                case "Bahasa Indonesia":
                    {
                        this.Text = "Pembuat Arsip Akuntansi CSV";
                        button1.Text = "Buka Tabel";
                        button2.Text = "Impor CSV";
                        button3.Text = "Tentang";
                        button4.Text = "Kembali";
                        button5.Text = "Tambah Barisan";
                        button6.Text = "Simpan Sebagai ...";
                        button7.Text = "Kosongkan Tabel";
                        button8.Text = "Kembali";
                        button9.Text = "Simpan";
                        button10.Text = "Undo";
                        button11.Text = "Redo";

                        label1.Text = "Mohon masukan angka";
                        label2.Text = "     Perangkat lunak ini dikembangkan oleh Mochamad Adamrasyad Iqbal (C) 1444 H. " +
                            "Perangkat lunak ini berguna untuk membuat tabel akuntan setiap saat dibutuhkan dan membuka file CSV peruntukannya.";
                        label3.Text = "Tentang";
                        label4.Text = "Terakhir diedit pada : ";
                        dateTime = "Terakhir diedit pada : ";
                        label5.Text = "Selamat Datang di Pembuat Arsip CSV Akuntan";
                        label6.Text = "Dibuka CSV ";
                        label7.Text = "Berkas CSV berhasil disimpan";
                        if (toggle)
                        {
                            dataGridView1.Columns["Masukan"].HeaderText = "Debit";
                            dataGridView1.Columns["Keluaran"].HeaderText = "Kredit";
                            dataGridView1.Columns["Total"].HeaderText = "Total";
                            dataGridView1.Columns["Timbangan"].HeaderText = "Saldo";
                        }
                        else
                        {
                            dataGridView1.Columns["Masukan"].HeaderText = "Masukan";
                            dataGridView1.Columns["Keluaran"].HeaderText = "Keluaran";
                            dataGridView1.Columns["Total"].HeaderText = "Total";
                            dataGridView1.Columns["Timbangan"].HeaderText = "Timbangan";
                        }

                        checkBox1.Text = "Pakai Istilah Akuntansi";
                        break;
                    }
                case "English":
                    {
                        this.Text = "CSV Accounting File Maker";
                        button1.Text = "Open table";
                        button2.Text = "Import CSV";
                        button3.Text = "About";
                        button4.Text = "Back";
                        button5.Text = "Add Row";
                        button6.Text = "Save As ...";
                        button7.Text = "Clear";
                        button8.Text = "Back";
                        button9.Text = "Save";
                        button10.Text = "Undo";
                        button11.Text = "Redo";

                        label1.Text = "Please insert number";
                        label2.Text = "     This software is developed by Mochamad Adamrasyad Iqbal (C) 1444 H. " +
                            "The software is useful to make an accounting table everytime there is a need and it can open the CSV compatible with this software.";
                        label3.Text = "About";
                        string theCurrentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        dateTime = "Last Edited on : ";
                        label4.Text = dateTime + theCurrentTime;
                        label5.Text = "Welcome to CSV Accounting File Maker";
                        label6.Text = "Opened CSV ";
                        label7.Text = "CSV File is successfully saved";

                        if (toggle)
                        {
                            dataGridView1.Columns["Masukan"].HeaderText = "Debit";
                            dataGridView1.Columns["Keluaran"].HeaderText = "Credit";
                            dataGridView1.Columns["Total"].HeaderText = "Total";
                            dataGridView1.Columns["Timbangan"].HeaderText = "Balance";
                        }
                        else
                        {
                            dataGridView1.Columns["Masukan"].HeaderText = "Inflow";
                            dataGridView1.Columns["Keluaran"].HeaderText = "Outflow";
                            dataGridView1.Columns["Total"].HeaderText = "Total";
                            dataGridView1.Columns["Timbangan"].HeaderText = "Balance";
                        }

                        checkBox1.Text = "Use Accounting Terms";
                        break;
                    }
                case "العربية":
                    {
                        this.Text = "صانع ملف المحاسبة سي إس في (CSV)";
                        button1.Text = "افتح الجدول";
                        button2.Text = "ورد ملف سي إس في";
                        button3.Text = "عن البرنامح";
                        button4.Text = "رجع";
                        button5.Text = "أضف الصف";
                        button6.Text = "حفظ باسم ...";
                        button7.Text = "فرغ";
                        button8.Text = "رجع";
                        button9.Text = "حفظ";
                        button10.Text = "أرجع";
                        button11.Text = "أعد";

                        label1.Text = "رجاء إدخال الرقم فقط";
                        label2.Text = "     هذا البرنامج طوره محمد أدم رشاد إقبال (C) 1444 هجرية. " +
                            "والبرنانج يخدم عمل الجدول للمحاسبة كلما احتيج ويفتح ملف سي إس في الصالح له.";
                        label3.Text = "عن البرنامج";
                        dateTime = "ءاخر التغيير في : ";
                        string theCurrentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        label4.Text = dateTime + theCurrentTime;
                        label5.Text = "أهلا بكم في البرنامج صانع ملف سي إس في للمحاسبة";
                        label6.Text = "افتح سي إس في ";
                        label7.Text = "تم حفظ الملف سي إس في";

                        if (toggle)
                        {
                            dataGridView1.Columns["Masukan"].HeaderText = "الخصم";
                            dataGridView1.Columns["Keluaran"].HeaderText = "الائتمان";
                            dataGridView1.Columns["Total"].HeaderText = "المجموع";
                            dataGridView1.Columns["Timbangan"].HeaderText = "الرصيد";
                        }
                        else
                        {
                            dataGridView1.Columns["Masukan"].HeaderText = "الداخل";
                            dataGridView1.Columns["Keluaran"].HeaderText = "الخارج";
                            dataGridView1.Columns["Total"].HeaderText = "المجموع";
                            dataGridView1.Columns["Timbangan"].HeaderText = "الرصيد";
                        }

                        checkBox1.Text = "استعمل مصطلح المحاسبة";
                        break;
                    }                
            }
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            SwitchLang();
        }
        //bool isRowJustAdded = false;
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //isRowJustAdded = true;
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