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
 *         DataGridView コントロールの基本
 *         DB接続を行わず、プログラムからテーブルデータを表示。
 *         (追加 / 修正 / 削除 / 整順 などの加工内容は １回きりで保存されない)
 */
#region -> DataGridView Reference
/*
 *@subject ◆DataGridView : Control, ISupportInitialize -- System.Windows.Forms
 *         DataGridView   new DataGridView()
 *        
 *         ＊Indexer
 *         DataGridViewCell this[string columnName, int rowIndex] { get; set; }
 *         DataGridViewCell this[int columnIndex, int rowIndex]   { get; set; }
 *         
 *         ＊Property
 *         ・Basic
 *         string   dataGridView.Text { get; set; }
 *         object   dataGridView.DataSource { get; set; }
 *         string   dataGridView.DataMember { get; set; }
 *         int      dataGridView.ColumnCount { get; set; }
 *         int      dataGridView.RowCount { get; set; }
 *         int      dataGridView.NewRowIndex { get; }
 *         bool     dataGridView.ReadOnly { get; set; }
 *         bool     dataGridView.MultiSelect { get; set; }
 *         bool     dataGridView.AutoGenerateColumns { get; set; }    //DataSourceを元に自動で列,行を生成
 *         DataGridViewColumn  dataGridView.SortedColumn { get; }
 *         DataGridViewRow     dataGridView.CurrentRow   { get; }
 *         DataGridViewCell    dataGridView.CurrentCell { get; set; }
 *         DataGridViewCell    dataGridView.FirstDisplayedCell { get; set; }
 *         
 *         ・Collection
 *         DataGridViewColumnCollection  dataGridView.Columns { get; }
 *         DataGridViewRowCollection     dataGridView.Rows { get; }
 *         DataGridViewSelectedColumnCollection dataGridView.SelectedColumns { get; }
 *         DataGridViewSelectedRowCollection    dataGridView.SelectedRows { get; }
 *         DataGridViewSelectedCellCollection   dataGridView.SelectedCells { get; }
 *         
 *         ・Size / AutoSize
 *         bool     dataGridView.ColumnHeadersVisible { get; set; }
 *         bool     dataGridView.RowHeadersVisible { get; set; }
 *         int      dataGridView.ColumnHeadersHeight { get; set; }
 *         int      dataGridView.RowHeadersWidth { get; set; } 
 *         Size     dataGridView.DefaultSize { get; }
 *         bool     dataGridView.AllowUserToResizeRows { get; set; }
 *         bool     dataGridView.AllowUserToResizeColumns { get; set; }
 *         bool     dataGridView.AllowUserToOrderColumns { get; set; }
 *         bool     dataGridView.AllowUserToDeleteRows { get; set; }
 *         bool     dataGridView.AllowUserToAddRows { get; set; }
 *         
 *         DataGridViewColumnHeadersHeightSizeMode  dataGridView.ColumnHeadersHeightSizeMode { get; set; }
 *           └ enum DataGridViewColumnHeadersHeightSizeMode
 *             {
 *                EnableResizing = 0,
 *                DisableResizing = 1,
 *                AutoSize = 2,
 *             }
 *             
 *         DataGridViewAutoSizeColumnsMode      dataGridView.AutoSizeColumnsMode { get; set; }    自動で画面幅に調整。各列も均等になり、Column幅の自己定義は無視される
 *            └ enum DataGridViewAutoSizeColumnsMode
 *              {
 *                 None = 1,
 *                 ColumnHeader = 2,
 *                 AllCellsExceptHeader = 4,
 *                 AllCells = 6,
 *                 DisplayedCellsExceptHeader = 8,
 *                 DisplayedCells = 10,
 *                 Fill = 16,                       親コントロールの Sizeに合わせて 列幅, 行幅を均等配置
 *                                                  (自己設定した 列幅, 行幅は無視され、自動設定される)
 *              }
 *              
 *         DataGridViewRowHeadersWidthSizeMode  dataGridView.RowHeadersWidthSizeMode { get; set; }
 *           └ enum DataGridViewRowHeadersWidthSizeMode
 *             {
 *                EnableResizing = 0,
 *                DisableResizing = 1,
 *                AutoSizeToAllHeaders = 2,
 *                AutoSizeToDisplayedHeaders = 3,
 *                AutoSizeToFirstHeader = 4,
 *             }
 *             
 *         DataGridViewAutoSizeRowsMode         dataGridView.AutoSizeRowsMode { get; set; }
 *           └ enum DataGridViewAutoSizeRowsMode
 *             {
 *                None = 0,
 *                AllHeaders = 5,
 *                AllCellsExceptHeaders = 6,
 *                AllCells = 7,
 *                DisplayedHeaders = 9,
 *                DisplayedCellsExceptHeaders = 10,
 *                DisplayedCells = 11,
 *              }
 *          
 *         ・CellStyle / BorderStyle
 *         bool                    dataGridView.EnableHeadersVisualStyles { get; set; }
 *         DataGridViewCellStyle   dataGridView.ColumnHeadersDefaultCellStyle { get; set; }
 *         DataGridViewCellStyle   dataGridView.RowHeadersDefaultCellStyle { get; set; }
 *         DataGridViewCellStyle   dataGridView.RowsDefaultCellStyle { get; set; }
 *         DataGridViewCellStyle   dataGridView.DefaultCellStyle { get; set; }
 *         DataGridViewCellStyle   dataGridView.AlternatingRowsDefaultCellStyle { get; set; }   交互に色などを変える場合
 *           └ class DataGridViewCellStyle 〔下記〕
 *           
 *         Color                          dataGridView.GridColor { get; set; }
 *         DataGridViewHeaderBorderStyle  dataGridView.ColumnHeadersBorderStyle { get; set; }
 *         DataGridViewHeaderBorderStyle  dataGridView.RowHeadersBorderStyle { get; set; }
 *           └ enum DataGridViewHeaderBorderStyle
 *             {
 *                Custom = 0,
 *                Single = 1,
 *                Raised = 2,
 *                Sunken = 3,
 *                None = 4,
 *             }
 *         
 *         DataGridViewCellBorderStyle       dataGridView.CellBorderStyle { get; set; }
 *           └ enum DataGridViewCellBorderStyle
 *             {
 *                Custom = 0,
 *                Single = 1,
 *                Raised = 2,
 *                Sunken = 3,
 *                None = 4,
 *                SingleVertical = 5,
 *                RaisedVertical = 6,
 *                SunkenVertical = 7,
 *                SingleHorizontal = 8,
 *                RaisedHorizontal = 9,
 *                SunkenHorizontal = 10,
 *             }
 *             
 *         DataGridViewAdvancedBorderStyle   dataGridView.AdvancedRowHeadersBorderStyle { get; }
 *         DataGridViewAdvancedBorderStyle   dataGridView.AdvancedColumnHeadersBorderStyle { get; }
 *         DataGridViewAdvancedBorderStyle   dataGridView.AdvancedCellBorderStyle { get; }
 *         DataGridViewAdvancedBorderStyle   dataGridView.AdjustedTopLeftHeaderBorderStyle { get; }
 *           └ class DataGridViewAdvancedBorderStyle
 *               └ enum DataGridViewAdvancedCellBorderStyle
 *                 {
 *                    NotSet = 0,
 *                    None = 1,
 *                    Single = 2,
 *                    Inset = 3,
 *                    InsetDouble = 4,
 *                    Outset = 5,
 *                    OutsetDouble = 6,
 *                    OutsetPartial = 7,
 *                 }
 *                 
 *         SortOrder dataGridView.SortOrder { get; }
 *           └ enum SortOrder
 *             {
 *                None = 0,
 *                Ascending = 1,
 *                Descending = 2,
 *             }
 *             
 *         DataGridViewSelectionMode  dataGridView.SelectionMode { get; set; }
 *           └ enum DataGridViewSelectionMode
 *             {
 *                CellSelect = 0,
 *                FullRowSelect = 1,
 *                FullColumnSelect = 2,
 *                RowHeaderSelect = 3,
 *                ColumnHeaderSelect = 4,
 *             }
 *  
 *         Panel    dataGridView.EditingPanel { get; }
 *         Control  dataGridView.EditingControl { get; }
 *         DataGridViewEditMode  dataGridView.EditMode { get; set; }
 *           └ enum DataGridViewEditMode
 *             {
 *                EditOnEnter = 0,
 *                EditOnKeystroke = 1,
 *                EditOnKeystrokeOrF2 = 2,
 *                EditOnF2 = 3,
 *                EditProgrammatically = 4,
 *             }
 *           
 *         ＊Method
 *         void     dataGridView.AutoResizeColumn(int columnIndex, [DataGridViewAutoSizeColumnMode]);
 *         void     dataGridView.AutoResizeColumns([DataGridViewAutoSizeColumnsMode]);
 *         void     dataGridView.AutoResizeColumnHeadersHeight([int columnIndex]);
 *         void     dataGridView.AutoResizeRow(int rowIndex, [DataGridViewAutoSizeRowMode]);
 *         void     dataGridView.AutoResizeRowHeadersWidth(int rowIndex, [DataGridViewRowHeadersWidthSizeMode]);
 *         void     dataGridView.AutoResizeRows(DataGridViewAutoSizeRowsMode autoSizeRowsMode);
 *         
 *         bool     dataGridView.BeginEdit(bool selectAll);
 *         bool     dataGridView.EndEdit(DataGridViewDataErrorContexts context);
 *         bool     dataGridView.CommitEdit(DataGridViewDataErrorContexts);
 *         bool     dataGridView.CancelEdit();
 *         bool     dataGridView.RefreshEdit();
 *         
 *         void     dataGridView.UpdateCellValue(int columnIndex, int rowIndex);
 *         void     dataGridView.SelectAll();
 *         void     dataGridView.ClearSelection();
 *         
 *         void     dataGridView.Sort(IComparer comparer)
 *         void     dataGridView.Sort(DataGridViewColumn, ListSortDirection direction);
 *           └ enum ListSortDirection
 *             {
 *                Ascending = 0,
 *                Descending = 1,
 *             }
 *             
 *         ＊Event
 *         event EventHandler  dataGridView.Sorted
 *         evrnt EventHandler  dataGridView.SelectionChanged
 *         event EventHandler  dataGridView.CurrentCellChanged
 *         event DataGridViewCellEventHandler  dataGridView.CellClick   ボタンクリック時？
 *                 └ 引数: DataGridViewCellEventArgs : EventArgs        引数にセル情報を含む
 *                      int   e.ColumnIndex
 *                      int   e.RowIndex
 *         event DataGridViewCellMouseEventHandler  dataGridView.ColumnHeaderMouseClick   マウスクリック時
 *                 └ 引数: DataGridViewCellMouseClickEventArgs : MouseEventArgs           引数にセル情報を含む
 *                      int   e.ColumnIndex
 *                      int   e.RowIndex
 *                      
 *@subject ◆DataGridViewColumn : DataGridViewBand, IComponent, IDisposable
 *              -- System.Windows.Forms.
 *         + DataGridViewColumn  new ColumnDataGridViewColumn(DataGridViewCell cellTemplate)
 *         + DataGridViewColumn  dataGridView.SortedColumn
 *
 *         string  column.Name  { get; set; }  列を識別する為の名前です。大文字・小文字は区別されません。
 *         string  column.HeaderText  { get; set; }  列のヘッダーセルの見出しの文字列です。
 *         string DataPropertyName { get; set; }     バインドされている、データ ソース プロパティの名前またはデータベースの列の名前を取得または設定します。
 *         int     column.Width  { get; set; }       列の幅を設定します。既定値は100です。
 *         int MinimumWidth { get; set; }
 *         int     column.DividerWidth  区分線の幅を設定します。既定値は 0 です。
 *         
 *         DataGridViewCell  column.CellTemplate          セルのタイプを設定します。セルにテキストボックスを表示したり、
 *                                                  チェックボックス、コンボボックス等を表示することが可能です。
 *         int     dataGridViewColumn.Index         DataGridView内での相対位置を取得します。
 *         bool IsDataBound { get; }
 *         bool Visible { get; set; }
 *         bool ReadOnly { get; set; }
 *         bool Frozen { get; set; }
 *         DataGridViewAutoSizeColumnMode AutoSizeMode 
 *         DataGridViewColumnSortMode SortMode { get; set; }
 *         DataGridViewCellStyle DefaultCellStyle { get; set; }
 *         
 *@subject ◆DataGridViewRow : DataGridViewBand -- System.Windows.Forms.
 *         + DataGridViewRow   new DataGridViewRow()
 *         + DataGridViewRow   dataGridView.Rows[i]
 *         + DataGrodViewRow   dataGridView.CurrentRow
 *         + DataGridViewRow   dataGridView.RowTemplate { get; set; }
 *         
 *         DataGridViewCellCollection  row.Cells { get; }
 *         object  row.DataBoundItem { get; }   データバインド時のオブジェクトを取得
 *         int     row.Height { get; set; }
 *         int     row.MinimumHeight { get; set; }
 *         int     row.DividerHeight { get; set; }  行の区分線の高さ
 *         bool    row.Visible { get; set; }
 *         bool    row.Displayed { get; }
 *         bool    row.ReadOnly { get; set; }
 *         bool    row.Selected { get; set; }
 *         bool    row.Frozen { get; set; }        行が固定された状態か
 *         bool    row.IsNewRow { get; }
 *         DataGridViewElementStates  row.State { get; }
 *            └ enum DataGridViewElementStates  行の状態
 *              {
 *                 None = 0,
 *                 Displayed = 1,
 *                 Frozen = 2,
 *                 ReadOnly = 4,
 *                 Resizable = 8,
 *                 ResizableSet = 16,
 *                 Selected = 32,
 *                 Visible = 64,
 *              }
 *              
 *         DataGridViewRowHeaderCell  row.HeaderCell { get; set; }
 *         DataGridViewCellStyle      row.DefaultCellStyle { get; set; }
 *         
 *         void   row.CreateCells(DataGridView dataGridView, params object[] values);
 *         bool   row.SetValues(params object[] values)
 *         DataGridViewCellCollection  row.CreateCellsInstance()
 *         
 *@subject ◆DataGridViewCell : Control, ISupportInitialize
 *              -- System.Windows.Forms.
 *         # DataGridViewCell  new DataGridViewCell()
 *         + DataGridViewCell  dataGridView.this[string columnName, int rowIndex]
 *         + DataGridViewCell  dataGridView.this[int columnName, int rowIndex]
 *         + DataGridViewCell  dataGridView.Rows[i].Cells[i]
 *         + DataGrodViewCell  dataGridView.CurrentCell
 *         + DataGridViewCell  dataGridView.FirstDisplayedCell
 *         
 *         object  cell.Value { get; set; }
 *         bool    cell.Selected { get; set; }
 *         int     cell.ColumnIndex { get; }
 *         int     cell.RowIndex { get; }
 *         object  cell.GetValue(int rowIndex)
 *         bool    cell.SetValue(int rowIndex, object value);
 *         DataGridViewColumn   cell.OwningColumn { get; }    このセルを格納している列
 *         DataGridViewRow      cell.OwningRow { get; }       このセルを格納している行
 *
 *@subject ◆DataGridViewCellStyle : ICloneable  -- System.Windows.Forms.
 *         + DataGridViewCellStyle   new DataGridViewCellStyle([DataGridViewCellStyle])
 *         
 *         ＊Property  DataGridViewCellStyle.Xxxx
 *         DataGridViewContentAlignment  Alignment { get; set; }
 *           └ enum DataGridViewContentAlignment
 *             {
 *                NotSet = 0,
 *                TopLeft = 1,
 *                TopCenter = 2,
 *                TopRight = 4,
 *                MiddleLeft = 16,
 *                MiddleCenter = 32,
 *                MiddleRight = 64,
 *                BottomLeft = 256,
 *                BottomCenter = 512,
 *                BottomRight = 1024,
 *             }
 *             
 *         Color ForeColor { get; set; }
 *         Color BackColor { get; set; }
 *         Color SelectionForeColor { get; set; }
 *         Color SelectionBackColor { get; set; }
 *         Font Font { get; set; }
 *         string Format { get; set; }
 *         
 *@subject 行の追加 / 挿入
 *         void    dataGridView.Rows.Add(params object[])  行データを追加
 *         void    dataGridView.Rows.Add()                 空行を追加
 *         int     dataGridView.Rows.Count                 行数を指定 (空行を追加)。
 *         void    dataGridView.Rows.Insert(int index, params object[])  指定行に行データを挿入
 *         
 *@subject セルに値を追加
 *         dataGridView.Rows[i].Cells[i].Value = value
 *         
 *@subject ソート機能
 *         ・ColumnHeaderクリック時のイベントハンドラー内や、追加の後などにソート
 *         
 *         grid.ColumnHeaderMouseClick += 
 *             new DataGridViewCellMouseEventHandler(
 *                 object sender, DataGridViewCellMouseClickEventArgs e);
 *                    └ 引数: DataGridViewCellMouseClickEventArgs : MouseEventArgs
 *                      int   e.ColumnIndex
 *                      int   e.RowIndex
 *                      
 *         SortOrder dataGridView.SortOrder { get; }
 *           └ enum SortOrder
 *             {
 *                None = 0,
 *                Ascending = 1,
 *                Descending = 2,
 *             }
 *             
 *         void  dataGridView.Sort(IComparer)
 *         void  dataGridView.Sort(DataGridViewColumn, ListSortDirection)
 *                 └ 引数: enum ListSortDirection
 *                         {
 *                             Ascending   昇順
 *                             Descending  降順
 *                         }
 *                       
 *         例 dataGridView.Sort(
 *              dataGridView1.Columns["Id"], 
 *              ListSortDirection.Ascending
 *            );  
 *            
 *         例 Grid_ColumnHeaderMouseClick(
 *              object sender, DataGridViewCellMouseClickEventArgs e)内
 *         
 *         int columnIndex = e.ColumnIndex;
 *         
 *         if (grid.SortOrder == SortOrder.None 
 *             || grid.SortOrder == SortOrder.Descending)
 *         {
 *             dataGridView.Sort(
 *                 dataGridView1.Columns["columnIndex"], 
 *                 ListSortDirection.Ascending
 *             );   
 *         }
 */
#endregion
/*
 *@see ImageDataGridViewBasicSample.jpg
 *@see 
 *@author shika
 *@date 2022-10-23
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    class MainDataGridViewBasicSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
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
            grid.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(Grid_ColumnHeaderMouseClick);
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
                Value = 1,
                TextAlign = HorizontalAlignment.Center,
                Width = 60,
            };
            flowBirth.Controls.Add(numMonth);

            numDay = new NumericUpDown()
            {
                Maximum = 31,
                Minimum = 1,
                Value = 1,
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

        private void Grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            
            if (grid.SortOrder == SortOrder.None 
                || grid.SortOrder == SortOrder.Descending)
            {
                grid.Sort(grid.Columns[columnIndex], ListSortDirection.Ascending);
            }
            else
            {
                grid.Sort(grid.Columns[columnIndex], ListSortDirection.Descending);
            }
        }//Grid_ColumnHeaderMouseClick()

        private void Button_Click(object sender, EventArgs e)
        {
            switch((sender as Button).Text)
            {
                case "Insert Row":
                    InsertRow();
                    break;
                case "Update Row":
                    UpdateRow();
                    break;
                case "Delete Row":
                    DeleteRow();
                    break;
                case "Delete All":
                    DeleteAll();
                    break;
            }//switch
        }//Button_Click()

        private void InsertRow()
        {
            //==== Validate Input ====
            bool canInsert = ValidateInput();

            if (!canInsert) { return; }

            string[] newData = BuildNewData();

            //==== Confirm to Insert ====
            StringBuilder bld = new StringBuilder();
            bld.Append("The below data will be inserted into Database, OK?\n\n");

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

            grid.Sort(grid.Columns["id"], ListSortDirection.Ascending);
        }//InsertRow()

        private string[] BuildNewData()
        {
            //==== newData[] ====
            string[] newData = new string[columnTextAry.Length];

            //---- Id: newData[0] ---
            newData[0] = AlgoAutoIncrement().ToString();

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
            return newData;
        }

        private int AlgoAutoIncrement()
        {
            List<int> idList = new List<int>();
            foreach(DataGridViewRow row in grid.Rows)
            {
                string idStr = row.Cells[0].Value?.ToString() ?? "";

                if (idStr == "") { continue; }

                idList.Add(Int32.Parse(idStr));
            }//foreach

            if(idList.Count == 0)
            {
                return 1;
            }

            int maxId = idList.Max();
            
            if (maxId == grid.Rows.Count - 1) //空行が１行ある
            {
                return grid.Rows.Count;
            }

            int findBlank = -1;
            for (int i = 0; i < idList.Count; i++)
            {
                if(i + 1 != idList[i])
                {
                    findBlank = i + 1;
                    break;
                }
            }//for

            return findBlank;
        }//AlgoAutoIncrement()

        private void UpdateRow()
        {
            //---- Validate Input ----
            bool canUpdate = ValidateInput(isUpdate: true);

            if(!canUpdate) { return; }

            //---- Find Changed Value ----
            int rowIndex = grid.CurrentCell.RowIndex;
            var row = grid.Rows[rowIndex];

            if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "")  //if selected empty row
            {
                string messageTitle = "Notation";
                string message = "<！> New Row cannot be updated.\n =>  Please use 'Insert Row'.\n\n";
                ShowConfirmMessageBox(message, messageTitle);
                return;
            }

            string[] newData = BuildNewData();
            List<string> olderValueList = new List<string>();
            List<string> changedValueList = new List<string>();
            List<DataGridViewCell> changedCellList = new List<DataGridViewCell>();

            for (int i = 1; i < newData.Length - 1; i++)   // Cells[0]: Id
            {                                              // Cells[newData.Length - 1]: Age
                DataGridViewCell cell = row.Cells[i];      
                string olderValue = cell.Value.ToString();

                if(newData[i] != olderValue) 
                {
                    olderValueList.Add(olderValue);
                    changedValueList.Add(newData[i]);
                    changedCellList.Add(cell);
                }
            }//foreach

            if(changedCellList.Count == 0)
            {
                string message = "<！> No changed value.";
                string messageTitle = "Notation";
                ShowConfirmMessageBox(message, messageTitle);
                return;
            }

            //---- Confirm to Update ----
            var updateBld = new StringBuilder();
            updateBld.Append("Update to Database. OK ?\n\n");
            updateBld.Append("[ Older Value ] => [ New Value ]\n");

            for(int i = 0; i < changedCellList.Count; i++)
            {
                updateBld.Append($"{olderValueList[i]} => {changedValueList[i]}\n");
            }//for

            DialogResult updateResult = ShowConfirmMessageBox(
                updateBld.ToString(), "Confirm to Update Database");
            if(updateResult == DialogResult.Cancel) { return; }
            else if(updateResult == DialogResult.OK)
            {
                for(int i = 0; i < changedCellList.Count; i++)
                {
                    row.Cells[changedCellList[i].ColumnIndex].Value
                        = changedValueList[i];
                }//for
            }
        }//UpdateRow()

        private void DeleteRow()
        {
            int rowIndex = grid.CurrentCell.RowIndex;
            var row = grid.Rows[rowIndex];

            var bld = new StringBuilder();
            bld.Append("The below row will be deleted from Database, OK ?\n\n");
            
            for(int i = 0; i < row.Cells.Count; i++)
            {
                bld.Append(
                    $"{grid.Columns[i].HeaderText}: " +
                    $"{row.Cells[i].Value?.ToString() ?? ""}\n"
                );
            }//foreach

            string messageTitle = "Confirm to Delete Row";
            DialogResult deleteResult = ShowConfirmMessageBox(bld.ToString(), messageTitle);
            if(deleteResult == DialogResult.Cancel) { return; }
            else if(deleteResult == DialogResult.OK)
            {
                grid.Rows.Remove(row);
            }
        }//DeleteRow()

        private void DeleteAll()
        {
            DialogResult deleteAllResult = ShowConfirmMessageBox(
                "DELETE All, OK ?", "Confirm to Delete All");
            if(deleteAllResult == DialogResult.Cancel) { return; }
            else if (deleteAllResult == DialogResult.OK)
            {
                grid.Rows.Clear();
            }
        }//DeleteAll()

        private bool ValidateInput(bool isUpdate = false)
        {
            StringBuilder errorMessageBuilder = new StringBuilder();

            //==== Valdate input ====
            //---- Number: inputControlAry[0] ----
            if (!isUpdate)
            {
                foreach (DataGridViewRow row in grid.Rows)
                {
                    Decimal.TryParse(row.Cells[1].Value?.ToString(), out decimal num);

                    if ((inputControlAry[0] as NumericUpDown).Value == num)
                    {
                        errorMessageBuilder.Append("<！> Number need be unique numeric value.\n");
                    }
                }//foreach
            }

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
