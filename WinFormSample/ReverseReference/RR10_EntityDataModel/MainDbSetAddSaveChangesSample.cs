/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainDbSetAddSaveChangesSample.cs
 *@class   └ new FormDbSetAddSaveChangesSample() : Form
 *@class       └ new SubDbContextEntitySample() : DbContext
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
        private readonly SubDbContextEntitySample entity
            = new SubDbContextEntitySample();
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
            
        }//Button_Click()

        private void ButtonDelete_Click(object sender, EventArgs e)
        {

        }//Button_Click()

        private void ButtonUpdate_Click(object sender, EventArgs e)
        {

        }//Button_Click()
    }//class
}
