﻿/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR10_EntityDataModel
 *@class MainDbSetAddSaveChangesSample.cs
 *@class   └ new FormDbSetAddSaveChangesSample() : Form
 *@class       └ new SubDbContextEntityPersonRR() : DbContext
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference MB marunaka-blog《【C#】DataGridViewの使い方 》, 2021
 *              https://marunaka-blog.com/csharp-datagridview-use/3088/
 *              =>〔~\Reference\\Article_EntityFrameworkCodeFirst\Article_DataGridView.txt〕
 *              
 *@reference EF densanlabs『Entity Framework Code First』, 2012
 *              https://densan-labs.net/tech/codefirst/index.html
 *              =>〔~\Reference\Article_EntityFrameworkCodeFirst〕
 *              
 *@content RR[282] p506 DbSet Add(), SaveChanges()
 *@subject DbSet<TEntity>
 *
 *@see ImageDbSetAddSaveChangesSample.jpg
 *@see 
 *@author shika
 *@date 2022-10-21
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    class MainDbSetAddSaveChangesSample
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Console.WriteLine("new FormDbSetAddSaveChangesSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormDbSetAddSaveChangesSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormDbSetAddSaveChangesSample : Form
    {
        private readonly TableLayoutPanel table;
        private readonly DataGridView grid;
        private readonly SubDbContextEntityPersonRR entity
            = new SubDbContextEntityPersonRR();
        private readonly Label labelName;
        private readonly Label labelAddress;
        private readonly Label labelTel;
        private readonly Label labelEmail;
        private readonly TextBox textBoxName;
        private readonly TextBox textBoxAddress;
        private readonly TextBox textBoxTel;
        private readonly TextBox textBoxEmail;
        private readonly Button buttonInsert;
        private readonly Button buttonDelete;
        private readonly Button buttonUpdate;
        
        public FormDbSetAddSaveChangesSample()
        {
            this.Text = "FormDbSetAddSaveChangesSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 640);
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            Padding padding = new Padding(10);

            table = new TableLayoutPanel()
            {
                ColumnCount = 3,
                RowCount = 6,
                ClientSize = this.ClientSize,
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));

            grid = new DataGridView()
            {
                AutoGenerateColumns = true,
                MultiSelect = false,
                ReadOnly = true,
                TabStop = false,
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            grid.DataSource = entity.PersonRR.Local;
            grid.SelectionChanged += new EventHandler(Grid_SelectionChanged);
            
            table.Controls.Add(grid, 0, 0);
            table.SetColumnSpan(grid, 3);

            //---- Label ----
            labelName = new Label()
            {
                Text = "Name: ",
                TextAlign = ContentAlignment.TopRight,
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelName, 0, 1);

            labelAddress = new Label()
            {
                Text = "Address: ",
                TextAlign = ContentAlignment.TopRight,
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelAddress, 0, 2);

            labelTel = new Label()
            {
                Text = "Tel: ",
                TextAlign = ContentAlignment.TopRight,
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelTel, 0, 3);

            labelEmail = new Label()
            {
                Text = "Email: ",
                TextAlign = ContentAlignment.TopRight,
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelEmail, 0, 4);

            //---- TextBox ----
            textBoxName = new TextBox()
            {
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            
            table.Controls.Add(textBoxName, 1, 1);
            table.SetColumnSpan(textBoxName, 2);

            textBoxAddress = new TextBox()
            {
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxAddress, 1, 2);
            table.SetColumnSpan(textBoxAddress, 2);

            textBoxTel = new TextBox()
            {
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxTel, 1, 3);
            table.SetColumnSpan(textBoxTel, 2);

            textBoxEmail = new TextBox()
            {
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxEmail, 1, 4);
            table.SetColumnSpan(textBoxEmail, 2);

            //---- Button ----
            buttonInsert = new Button()
            {
                Text = "Insert Row",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonInsert.Click += new EventHandler(ButtonInsert_Click);
            table.Controls.Add(buttonInsert, 0, 5);

            buttonDelete = new Button()
            {
                Text = "Delete Row",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonDelete.Click += new EventHandler(ButtonDelete_Click);
            table.Controls.Add(buttonDelete, 1, 5);

            buttonUpdate = new Button()
            {
                Text = "Update Database",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonUpdate.Click += new EventHandler(ButtonUpdate_Click);
            table.Controls.Add(buttonUpdate, 2, 5);

            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            if(grid.CurrentCell == null) { return; }

            var rowCollection = grid.Rows;
            int rowIndex = grid.CurrentCell.RowIndex;

            DataGridViewCellCollection cellCollection = rowCollection[rowIndex].Cells;
         
            textBoxName.Text = cellCollection["Name"].Value?.ToString() ?? "";
            textBoxAddress.Text = cellCollection["Address"].Value?.ToString() ?? "";
            textBoxTel.Text = cellCollection["Tel"].Value?.ToString() ?? "";
            textBoxEmail.Text = cellCollection["Email"].Value?.ToString() ?? "";
        }//Grid_SelectionChanged()

        private void ButtonInsert_Click(object sender, EventArgs e)
        {
            bool canInsert = ValidateInput();
            if(!canInsert) { return; }

            //---- Insert Row ----
            PersonRR person = new PersonRR()
            {
                Name = textBoxName.Text,
                Address = textBoxAddress.Text,
                Tel = textBoxTel.Text,
                Email = textBoxTel.Text,
            };

            DialogResult result = MessageBox.Show(
                person.ToString(),
                "Confirm to Insert DB",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) { return; }
            else if (result == DialogResult.OK)
            {
                entity.PersonRR.Add(person);
                entity.SaveChanges();
                entity.PersonRR.Load();
                grid.DataSource = entity.PersonRR.Local;
                this.Invalidate();
            }
        }//ButtonInsert_Click()

        private bool ValidateInput()
        {
            StringBuilder bld = new StringBuilder();

            //---- Valdate input ----
            if (textBoxName.Text.Trim().Length == 0)
            {
                bld.Append("<！> Name is required.\n");
            }

            if (textBoxName.Text.Trim().Length > 50)
            {
                bld.Append("<！> Name should discribe in 50 characters.\n");
            }

            if (textBoxAddress.Text.Trim().Length == 0)
            {
                bld.Append("<！> Address is required.\n");
            }

            if (textBoxTel.Text.Trim().Length > 50)
            {
                bld.Append("<！> Tel should discribe in 50 characters.\n");
            }

            if (!textBoxTel.Text.Trim()
                .ToCharArray()
                .All(c => Char.IsDigit(c)))
            {
                bld.Append("<！> Tel should discribe by Number ONLY.\n");
            }

            if (textBoxEmail.Text != "" && !Regex.IsMatch(textBoxEmail.Text.Trim(),
                @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", //Email〔NT27〕
                RegexOptions.IgnoreCase))
            {
                bld.Append("<！> Email should discribe within the Format.\n");
            }

            //---- against SQL Ingection ----
            string[] textAry = new string[]
            {
                textBoxName.Text, textBoxAddress.Text,
                textBoxTel.Text, textBoxEmail.Text,
            };

            foreach (string text in textAry)
            {
                if (Regex.IsMatch(text.Trim(), "[<>&;=]+") || Regex.IsMatch(text.Trim(), "[-]{2}"))
                {
                    bld.Append("<！> Invalidate input !\n");
                }
            }//foreach

            //---- Show Error Massage ----
            if (bld.Length > 0)
            {
                MessageBox.Show(
                    bld.ToString(),
                    "Input Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }//ValidateInput()

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            var rowCollextion = grid.Rows;
            var rowIndex = grid.CurrentCell.RowIndex;

            var cellCollection = rowCollextion[rowIndex].Cells;

            StringBuilder bld = new StringBuilder();
            foreach(DataGridViewCell cell in cellCollection)
            {
                bld.Append($"{cell.Value},\n");
            }

            DialogResult result = MessageBox.Show(
                bld.ToString(),
                "Confirm to Delete Row",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            //@NOTE【Problem】
            //・Cell.Valueで Idの値は取れているが、
            //  DBを DELETE するのは、最終行を削除してしまう。
            //・LINQ内で (int), Int32.Parse(string)は利用付加
            //
            //if(result == DialogResult.Cancel) { return; }
            //else if(result == DialogResult.OK)
            //{
            //    int id = (int)cellCollection[0].Value;
            //    PersonRR deletePersonRR = entity.PersonRR.Single(
            //        person => person.Id == id);
            //    entity.PersonRR.Remove(deletePersonRR);
            //    entity.SaveChanges();
            //}
            
        }//ButtonDelete_Click()

        private void ButtonUpdate_Click(object sender, EventArgs e)
        {

        }//Button_Click()
    }//class
}
