﻿/** 
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
using System.Linq;
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
        private readonly Control[] inputControlAry;
        private readonly Button[] buttonAry;
        private NumericUpDown numNumber;
        private TextBox textBoxName;
        private TextBox textBoxNation;
        private FlowLayoutPanel flowRole;
        private FlowLayoutPanel flowBirth;
        private NumericUpDown numYear;
        private NumericUpDown numMonth;
        private NumericUpDown numDay;
        private readonly Padding padding = new Padding(10);

        private readonly string[] columnTextAry = new string[]
        {
            "id","number", "name", "nationality", "role", "position", "birthday", "age"
        };

        private readonly string[] roleTextAry = new string[]
        {
            "Fielder", "Director", "Staff", "Others"
        };

        private readonly string[] positionTextAry = new string[]
        {
            "Pitcher", "Catcher", "First", "Second", "Third",
            "Short", "Left", "Center", "Right", "None",
        };

        private readonly string[] buttonTextAry = new string[]
        {
            "Insert Row", "Update Row", "Delete Row", "Delete All"
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
                RowCount = 5,
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
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));

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
            inputControlAry = new Control[columnTextAry.Length - 2];
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
            //==== Label ====
            for(int i = 0; i < labelAry.Length; i++)
            {
                labelAry[i] = new Label()
                {
                    Text = grid.Columns[i + 1].HeaderText + ": ",
                    TextAlign = ContentAlignment.TopRight,
                    Margin = padding,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };

                int x = -1, y = -1;                    // Control Location in Column Count: 4 
                if (i < 3)      { x = 0; y = i + 1; }  // Culumn 0: Number, Name, Natuonality
                else if (3 <= i) { x = 2; y = i - 2; } // Culumn 2: Role, Position, Birthday
                table.Controls.Add(labelAry[i], x, y);
            }//for Label

            //==== Control[] inputControlAry ====
            //---- Number: inputControlAry[0] ----
            numNumber = new NumericUpDown()
            {
                Maximum = 9999,
                Minimum = 0,
                TextAlign = HorizontalAlignment.Center,
                Margin = padding,
                Width = 100,
            };
            table.Controls.Add(numNumber, 1, 1);
            inputControlAry[0] = numNumber;

            //---- Name: inputControlAry[1] ----
            textBoxName = new TextBox()
            {
                Margin = padding,
                Multiline = false,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxName, 1, 2);
            inputControlAry[1] = textBoxName;

            //---- Nationality: inputControlAry[2] ----
            textBoxNation = new TextBox()
            {
                Margin = padding,
                Multiline = false,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxNation, 1, 3);
            inputControlAry[2] = textBoxNation;

            //---- Role: inputControlAry[3] ----
            flowRole = new FlowLayoutPanel()
            {
                Margin = padding,
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(flowRole, 3, 1);
            inputControlAry[3] = flowRole;
            
            RadioButton[] roleRadioAry = new RadioButton[roleTextAry.Length];
            
            for (int i = 0; i < roleTextAry.Length; i++)
            {
                roleRadioAry[i] = new RadioButton()
                {
                    Text = roleTextAry[i],
                    Font = new Font(this.Font.FontFamily, 10, this.Font.Style),
                };

                if(i == 0)
                {
                    roleRadioAry[i].Checked = true;
                }
            }//for
            flowRole.Controls.AddRange(roleRadioAry);

            //---- Position: inputControlAry[4] ----
            ComboBox comboPosition = new ComboBox()
            {
                SelectedText = "(Select Position)",
                Margin = padding,
                DropDownStyle = ComboBoxStyle.DropDown,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            comboPosition.Items.AddRange(positionTextAry);
            table.Controls.Add(comboPosition, 3, 2);
            inputControlAry[4] = comboPosition;

            //---- Birth: inputControlAry[5] ----
            flowBirth = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(flowBirth, 3, 3);
            inputControlAry[5] = flowBirth;

            numYear = new NumericUpDown()
            {
                Maximum = 3000,
                Minimum = 1000,
                Value = 2000,
                TextAlign = HorizontalAlignment.Center,
                Width = 80,
            };
            flowBirth.Controls.Add(numYear);

            numMonth = new NumericUpDown()
            {
                Maximum = 12,
                Minimum = 1,
                Value = 01,
                TextAlign = HorizontalAlignment.Center,
                Width = 60,
            };
            flowBirth.Controls.Add(numMonth);

            numDay = new NumericUpDown()
            {
                Maximum = 31,
                Minimum = 1,
                Value = 01,
                TextAlign = HorizontalAlignment.Center,
                Width = 60,
            };
            flowBirth.Controls.Add(numDay);

            //==== Button ====
            for (int i = 0; i < buttonAry.Length; i++)
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
            var cellCollection = row.Cells;

            //---- Number: inputControl[0] ----
            inputControlAry[0].Text = cellCollection[1].Value?.ToString() ?? "";

            //---- Name: inputControlAry[1] ----
            inputControlAry[1].Text = cellCollection[2].Value?.ToString() ?? "";

            //---- Nationality: inputControlAry[2] ----
            inputControlAry[2].Text = cellCollection[3].Value?.ToString() ?? "";

            //---- Role: inputControlAry[3] ----
            string roleStr = cellCollection[4].Value?.ToString() ?? ""; 
            
            foreach (RadioButton radio in 
                (inputControlAry[3] as FlowLayoutPanel).Controls)
            {
                if (radio.Text == roleStr)
                {
                    radio.Checked = true;
                }
                else
                {
                    radio.Checked = false;
                }
            }//foreach

            //---- Position: inputControlAry[4] ----
            inputControlAry[4].Text = cellCollection[5].Value?.ToString() ?? "None";

            //---- Birthday: inputControlAry[5] ----
            var numYear = (inputControlAry[5] as FlowLayoutPanel).Controls[0];
            var numMonth = (inputControlAry[5] as FlowLayoutPanel).Controls[1];
            var numDay = (inputControlAry[5] as FlowLayoutPanel).Controls[2];
            numYear.Text = cellCollection[6].Value?.ToString().Substring(0, 4) ?? "";
            numMonth.Text = cellCollection[6].Value?.ToString().Substring(5, 2) ?? "";
            numDay.Text = cellCollection[6].Value?.ToString().Substring(8, 2) ?? "";
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
                case "Delete All":
                    //
                    break;
            }//switch
        }//Button_Click()

        private void InsertRow()
        {
            //==== Validate Input ====
            bool canInsert = ValidateInput();

            if (!canInsert) { return; }

            //==== newData[] ====
            //---- Id: newData[0] ---
            string[] newData = new string[columnTextAry.Length];
            newData[0] = $"{grid.Rows.Count}";

            //---- Number: newData[1], inputControlAry[0] ----
            newData[1] = (inputControlAry[0] as NumericUpDown).Value.ToString();

            //---- Name: newData[2], inputControlAry[1] ----
            newData[2] = (inputControlAry[1] as TextBox).Text;

            //---- Nationality: newData[3], inputControlAry[2] ----
            newData[3] = (inputControlAry[2] as TextBox).Text;

            //---- Role: newData[4], inputControlAry[3] ----
            string selectedRole = null;

            foreach (RadioButton radio in
                (inputControlAry[3] as FlowLayoutPanel).Controls)
            {
                if (radio.Checked)
                {
                    selectedRole = radio.Text;
                    break;
                }
            }//foreach
            newData[4] = selectedRole;

            //---- Position: newData[5], inputControlAry[4] ----
            newData[5] = (inputControlAry[4] as ComboBox).SelectedItem.ToString();

            //---- Birthday: newData[6], inputControlAry[5] ----
            newData[6] = BuildInputBirth();

            //---- Age: newData[7] ----
            newData[newData.Length - 1] = "age";
            int age = CalcAge(newData);
            newData[newData.Length - 1] = age.ToString();

            //==== Confirm to Insert ====
            StringBuilder bld = new StringBuilder();
            bld.Append("The below data will insert into Database, OK?\n\n");

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
            }
        }//InsertRow()

        private bool ValidateInput()
        {
            StringBuilder errorMessageBuilder = new StringBuilder();

            //==== Valdate input ====
            //---- Number: inputControlAry[0] ----
            foreach (DataGridViewRow row in grid.Rows)
            {
                Decimal.TryParse(row.Cells[1].Value?.ToString(), out decimal num);

                if ((inputControlAry[0] as NumericUpDown).Value == num)
                {
                    errorMessageBuilder.Append("<！> Number need be unique numeric value.\n");
                }
            }//foreach

            //---- Name: inputControlAry[1] ----
            if ((inputControlAry[1] as TextBox).Text.Trim().Length == 0)
            {
                errorMessageBuilder.Append("<！> Name is required.\n");
            }

            if ((inputControlAry[1] as TextBox).Text.Trim().Length > 50)
            {
                errorMessageBuilder.Append("<！> Name should discribe in 50 characters.\n");
            }

            //---- Nationality: inputControlAry[2] ----
            if ((inputControlAry[2] as TextBox).Text.Trim().Length > 50)
            {
                errorMessageBuilder.Append("<！> Nationality should discribe in 50 characters.\n");
            }

            //---- Position: inputControlAry[4] ----
            string selected = (inputControlAry[4] as ComboBox).SelectedItem?.ToString() ?? "";
            bool isPosition = positionTextAry.Any(p => (selected == p));

            if (!isPosition)
            {
                errorMessageBuilder.Append("<！> Position need be selected anything or None.\n");
            }

            //---- Birthday: inputControlAry[5] ----
            string inputBirth = BuildInputBirth();

            if (!DateTime.TryParse(inputBirth.Trim(), out DateTime birth))
            {
                errorMessageBuilder.Append("<！> Birthday should discribe with form [YYYY/MM/DD].\n");
            }

            //==== against SQL Injection ====
            string[] inputTextAry = new string[]
            {
                (inputControlAry[1] as TextBox).Text,
                (inputControlAry[2] as TextBox).Text,
            };

            foreach (string text in inputTextAry)
            {
                if (Regex.IsMatch(text.Trim(), "[<>&;=]+") || Regex.IsMatch(text.Trim(), "[-]{2}"))
                {
                    errorMessageBuilder.Append("<！> Invalidate input !\n");
                }
            }//foreach

            //==== Show Error Massage ====
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

        private string BuildInputBirth()
        {
            string format = "00";
            return $"{(inputControlAry[5].Controls[0] as NumericUpDown).Value.ToString()}/" +
                   $"{(inputControlAry[5].Controls[1] as NumericUpDown).Value.ToString(format)}/" +
                   $"{(inputControlAry[5].Controls[2] as NumericUpDown).Value.ToString(format)}";
        }

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
