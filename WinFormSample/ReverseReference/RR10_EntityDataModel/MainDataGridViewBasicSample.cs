/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR10_EntityDataModel
 *@class MainDataGridViewBasicSample.cs
 *@class   └ new FormDataGridViewBasicSample() : Form
 *@class
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
 *@content MB DataGridView / Columns, Rows
 *@subject ◆DataGridViewColumn
 *
 *         string  dataGridViewColumn.Name     列を識別する為の名前です。大文字・小文字は区別されません。
 *         string  dataGridViewColumn.HeaderText            列のヘッダーセルの見出しの文字列です。
 *         int     dataGridViewColumn.Width                         列の幅を設定します。既定値は100です。
 *         int     dataGridViewColumn.DividerWidth  区分線の幅を設定します。既定値は 0 です。
 *         dataGridViewColumn.CellTemplate  セルのタイプを設定します。セルにテキストボックスを表示したり、
 *                       チェックボックス、コンボボックス等を表示することが可能です。
 *         int     dataGridViewColumn.Index         DataGridView内での相対位置を取得します。
 *
 *@see ImageDataGridViewBasicSample.jpg
 *@see 
 *@author shika
 *@date 2022-10-23
 */
using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    class MainDataGridViewBasicSample
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Console.WriteLine("new FormDataGridViewBasicSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormDataGridViewBasicSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormDataGridViewBasicSample : Form
    {
        private readonly TableLayoutPanel table;
        private readonly DataGridView grid;
        private readonly Label[] labelAry;
        private readonly TextBox[] textBoxAry;
        private readonly Button[] buttonAry;
        private readonly Padding padding = new Padding(10);
        private readonly string[] columnTextAry = new string[]
        {
            "id","number", "name", "nationality", "role", "position", "birthday", "age"
        };
        private readonly string[] buttonTextAry = new string[]
        {
            "Insert Row", "Update Row", "Delete Row",
        };

        public FormDataGridViewBasicSample()
        {
            this.Text = "FormDataGridViewBasicSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(1024, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = buttonTextAry.Length,
                RowCount = columnTextAry.Length,
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            
            for (int i = 0; i < table.ColumnCount; i++)
            {
                table.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 
                    (float)Math.Round(100d / table.ColumnCount, 2)));
            }//for ColumnStyle

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 40f));
            for (int i = 0; i < table.RowCount - 1; i++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
            }//for RowStyle

            //---- DataGridView ----
            grid = new DataGridView()
            {
                ReadOnly = true,
                MultiSelect = false,
                RowHeadersVisible = false,
                ColumnHeadersHeightSizeMode = 
                    DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                AllowUserToResizeColumns = true,
                AllowUserToResizeRows = false,
                Margin = padding,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            InitialDataGridView();
            grid.SelectionChanged += new EventHandler(Grid_SelectionChanged);
            table.Controls.Add(grid, 0, 0);
            table.SetColumnSpan(grid, table.ColumnCount);

            //---- Component ----
            labelAry = new Label[columnTextAry.Length - 2];
            textBoxAry = new TextBox[columnTextAry.Length - 2];
            buttonAry = new Button[buttonTextAry.Length];
            InitialComponent();

            this.Controls.AddRange(new Control[]
            {
               table,
            });
        }//constructor

        private void InitialDataGridView()
        {
            //---- Columns ----
            grid.ColumnHeadersHeight = 30;
            grid.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

            int[] columnWidthAry = new int[]
            {
                80, 80, 260, 120, 120, 120, 120, 80
            };

            for (int i = 0; i < columnTextAry.Length; i++)
            {
                var viewColumn = new DataGridViewColumn();
                viewColumn.Width = columnWidthAry[i];
                viewColumn.Name = columnTextAry[i];
                viewColumn.HeaderText = 
                    columnTextAry[i].Substring(0, 1).ToUpper() +
                    columnTextAry[i].Substring(1);
                viewColumn.CellTemplate = new DataGridViewTextBoxCell();

                //---- Cell Alignment ----
                if (i == 2 || i == 3) 
                {
                    viewColumn.DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleLeft;
                }
                else
                {
                    viewColumn.DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                }

                grid.Columns.Add(viewColumn);
            }//for

            //---- Row Data ----
            string[][] rowAry = new string[][]
            {
                new string[]
                {
                    "1", "51", "Ichiro Suzuki", "Japan", "Fielder", "Right", "1972/01/13", "age"
                },

                new string[]
                {
                    "2", "17", "Shouhei Otani", "Japan", "Fielder", "Pitcher", "1992/12/24", "age"
                },

                new string[]
                {
                    "3", "5", "Babe Luis", "USA", "Fielder", "Pitcher", "1952/10/24", "age"
                },
            };
            
            foreach(string[] dataAry in rowAry)
            {
                //---- CalcAge() ----
                int age = CalcAge(dataAry);
                dataAry[dataAry.Length - 1] = age.ToString();

                //---- Add() row ---- 
                grid.Rows.Add(dataAry);
            }//foreach

            //---- Alternating Rows ----
            grid.BackgroundColor = this.BackColor;
            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle()
            {
                BackColor = Color.AliceBlue,
            };
        }//InitialDataGridView()

        private int CalcAge(string[] dataAry)
        {
            //---- calc age ----
            DateTime now = DateTime.Now;
            DateTime.TryParse(dataAry[dataAry.Length - 2], out DateTime birth);
            int age = now.Year - birth.Year;
            age += (now.Month < birth.Month) ? -1 : 0;
            age += (now.Month == birth.Month && now.Day < birth.Day) ? -1 : 0;
            return age;
        }

        private void InitialComponent()
        {
            //---- Label, TextBox ----
            for(int i = 0; i < labelAry.Length; i++)
            {
                labelAry[i] = new Label()
                {
                    Text = grid.Columns[i + 1].HeaderText + ": ",
                    TextAlign = ContentAlignment.TopRight,
                    Padding = padding,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                table.Controls.Add(labelAry[i], 1, i + 1);
            }//for Label

            for (int i = 0; i < textBoxAry.Length; i++)
            {
                textBoxAry[i] = new TextBox()
                {
                    Margin = padding,
                    Multiline = false,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                table.Controls.Add(textBoxAry[i], 2, i + 1);
            }//for TextBox

            //---- Button ----
            for(int i = 0; i < buttonAry.Length; i++)
            {
                buttonAry[i] = new Button()
                {
                    Text = buttonTextAry[i],
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                buttonAry[i].Click += new EventHandler(Button_Click);
                table.Controls.Add(buttonAry[i], i, table.RowCount - 1);
            }//for Button
        }//InitialComponent()

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            var row = grid.Rows[grid.CurrentCell.RowIndex];
            
            for (int i = 0; i < textBoxAry.Length; i++)
            {
                textBoxAry[i].Text = row.Cells[i + 1].Value?.ToString() ?? "";
            }
        }//Grid_SelectionChanged()

        private void Button_Click(object sender, EventArgs e)
        {
            switch((sender as Button).Text)
            {
                case "Insert Row":
                    InsertRow();
                    break;
                case "Update Row":
                    //
                    break;
                case "Delete Row":
                    //
                    break;
            }//switch
        }//Button_Click()

        private void InsertRow()
        {
            bool canInsert = ValidateInput();

            if(!canInsert) { return; }

            string[] newData = new string[columnTextAry.Length];
            newData[0] = $"{grid.Rows.Count}";

            for(int i = 0; i < textBoxAry.Length; i++)
            {
                newData[i + 1] = textBoxAry[i].Text;
            }//for

            newData[newData.Length - 1] = "age";
            int age = CalcAge(newData);
            newData[newData.Length - 1] = age.ToString();

            StringBuilder bld = new StringBuilder();
            for (int i = 0; i < newData.Length; i++)
            {
                bld.Append($"{grid.Columns[i].HeaderText}: {newData[i]}\n");
            }//for
            
            string messageTitle = "Confirm to Insert Row";
            DialogResult insertResult = ShowConfirmMessageBox(bld.ToString(), messageTitle);

            if (insertResult == DialogResult.Cancel) { return; }
            else if (insertResult == DialogResult.OK)
            {
                grid.Rows.Add(newData);
                foreach(var textBox in textBoxAry)
                {
                    textBox.Text = "";
                }
            }
        }//InsertRow()

        private bool ValidateInput()
        {
            StringBuilder errorMessageBuilder = new StringBuilder();

            //---- Valdate input ----
            //Number
            foreach(DataGridViewRow row in grid.Rows)
            {
                if (textBoxAry[0].Text == row.Cells[1].Value?.ToString())
                {
                    errorMessageBuilder.Append("<！> Number need be unique value.\n");
                }                
            }//foreach

            //Name
            if (textBoxAry[1].Text.Trim().Length == 0) 
            {
                errorMessageBuilder.Append("<！> Name is required.\n");
            }

            if (textBoxAry[1].Text.Trim().Length > 50)
            {
                errorMessageBuilder.Append("<！> Name should discribe in 50 characters.\n");
            }

            //Birthday
            if (!DateTime.TryParse(textBoxAry[5].Text.Trim(), out DateTime birth))
            {
                errorMessageBuilder.Append("<！> Birthday should discribe with form [YYYY/MM/DD].\n");
            }

            //---- against SQL Injection ----
            foreach (TextBox textbox in textBoxAry)
            {
                string text = textbox.Text;

                if (Regex.IsMatch(text.Trim(), "[<>&;=]+") || Regex.IsMatch(text.Trim(), "[-]{2}"))
                {
                    errorMessageBuilder.Append("<！> Invalidate input !\n");
                }
            }//foreach

            //---- Show Error Massage ----
            if (errorMessageBuilder.Length > 0)
            {
                MessageBox.Show(
                    errorMessageBuilder.ToString(),
                    "Input Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }//ValidateInput()

        private DialogResult ShowConfirmMessageBox(string message, string messageTitle)
        {
            DialogResult result = MessageBox.Show(
                message,
                messageTitle,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            return result;
        }//ShowConfirmMessageBox()
    }//class
}
