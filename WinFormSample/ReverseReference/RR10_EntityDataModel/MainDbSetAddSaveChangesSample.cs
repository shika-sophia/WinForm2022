/** 
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
 *@content RR[282][283] p506 DbSet Add(), SaveChanges()
 *         
 *@subject DbContext
 *         =>〔MainDbContextSample.cs〕
 *         
 *@subject SubDbContextPersonRR
 *         ・DB接続情報を保持し、new によって DB接続を実行
 *         ・DBのテーブル情報を DbSet<PersonRR> オブジェクトで保持
 *         
 *         DbSet<TEntity>  DbContext.PersonRR
 *         =>〔MainDbContextSample.cs〕
 *         
 *@subject ObservableCollection<TEntity>  dbSet.Load()    //DBのテーブル情報をキャッシュとして保持
 *         object  dataGridView.DataSource = dbSet.Local  //DataGridViewの DataSource に DbSetのキャッシュを代入
 *         bool    dataGridView.AutoGenerateColumns       //DataSourceを元に 自動で列,行を生成
 *
 *@subject BindingList<TEntity>  ObservalCollection<TEntity>.ToBindingList<TEntity>()
 *         ・項目が追加または削除されたとき、
 *           あるいはリスト全体が更新されたときに通知を行う動的なデータ コレクション
 *           (grid.DataSource = entity.PersonRR.Local; のままだと、更新情報を表示できない)
 *         ・DBへの変更内容を BindingListに保持し、grid.SaveChanges()で DBに反映
 *         
 *         例 grid.DataSource = entity.PersonRR.Local.ToBindingList();
 *
 *@subject ◆class ObservableCollection<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
 *                   -- System.Collections.ObjectModel
 *         ObservableCollection<Object>   new ObservableCollection()
 *         ObservableCollection<T>        new ObservableCollection(List<T>)
 *         ObservableCollection<T>        new ObservableCollection(IEnumerable<T>)
 *         
 *         [+: public, #: protected]
 *         # void SetItem(int index, T item)
 *         # void InsertItem(int index, T item)
 *         + void Move(int oldIndex, int newIndex)
 *         # void MoveItem(int oldIndex, int newIndex)
 *         # void RemoveItem(int index)
 *         # void ClearItems()
 *         + event NotifyCollectionChangedEventHandler CollectionChanged
 *         # event PropertyChangedEventHandler PropertyChanged
 *         
 *@subject static class ObservableCollectionExtensions -- System.Data.Entity
 *         + static BindingList<T>  ObservableCollection.ToBindingList<T>(this ObservableCollection<T> source) where T : class;
 *         
 *@subject ◆class BindingList<T> : Collection<T>, IBindingList, IList, ICollection, IEnumerable, ICancelAddNew, IRaiseItemChangedEvents
 *                    -- System.ComponentModel
 *         データ バインディングをサポートするジェネリック コレクション
 *
 *@subject ◆class DbSqlQuery<T> : DbRawSqlQuery -- System.Data.Entity.Infrastructure
 *         # DbSqlQuery  DbSqlQuery()
 *         + DbSqlQuery<TEntity>  dbSet.SqlQuery(string sql, params object[] parameters);

 *         virtual DbSqlQuery AsNoTracking()  
 *             DbContext によってクエリ結果が追跡されない場合に新しいクエリを返します。
 *         virtual DbSqlQuery AsStreaming();  
 *              バッファリングの代わりに結果をストリームする新しいクエリを返します。
 *             [非推奨] 現在はデフォルトで Streaming を利用しているので、
 *                      どの ExecutionStrategy を使うかに関わらず、この Methodを呼び出しても何の効果も得られない
 *             [Obsolete("Queries are now streaming by default unless a retrying ExecutionStrategy is used. 
 *                        Calling this method will have no effect.")]
 *                        
 *@subject Grid_SelectionChanged()
 *         カーソルのある Cell から その Row を特定し、各列の値を取得
 *         DataGridViewRowCollection   dataGridView.Rows
 *         int                         dataGridView.CurrentCell.RowIndex
 *         DataGridViewRow             dataGridViewRowCollection[int]
 *         DataGridViewCellCollection  dataGridViewRow.Cells
 *         DataGridViewCell            dataGridViewCellCollection[string]
 *         object                      dataGridViewCell.Value
 *         
 *         例 var rowCollection = grid.Rows;
 *            int rowIndex = grid.CurrentCell.RowIndex;
 *            DataGridViewCellCollection cellCollection = rowCollection[rowIndex].Cells;
 *            textBoxName.Text = cellCollection["Name"].Value?.ToString() ?? ""; 
 *             
 *@subject  ＊[C#6-] null条件演算子「オブジェクト?.メンバー」〔CS 8〕
 *          ・「オブジェクト?.メンバー」 
 *                <=>  if (オブジェクト != null) { メンバー処理 } else { null }と同義
 *          ・オブジェクトが 非nullのときのみ、そのメンバーにアクセス
 *          ・null時は nullを返す
 *          ・NullReferenceExceptionの発生を防ぐ。nullチェックの簡易記法
 *          
 *          ＊null合体演算子「式１ ?? 式2」 〔CS 12〕
 *         ・「式１ ?? 式2」        
 *               <=>  if (式1 != null) { 式1 } else { 式2 } と同義
 *         ・式1が 非nullなら、式1を返す。null時は 式2を返す
 *         ・変数に null が代入されることを防ぐ
 *         ・null許容型の数値型も利用可
 *         
 *@subject dataGridView.Refresh()   
 *         ・いったん DataGridViewオブジェクトを破棄し再描画
 *         ・InsertRow(), DeleteRow()は grid.SaveChanges()で表示にも反映するが
 *           UpdateRow()時は 更新表示されないので、grid.Refresh(); が必要。
 *           
 *@subject bool  IEnumerable<T>.SequenceEqual(IEnumerable<T>);
 *         ・Listなどコレクションの要素どうしが同じであるか
 *         ・<T>は 対象オブジェクトと、引数のオブジェクトで同じ型にしないとコンパイルエラー
 *
 *@NOTE【Problem】grid.Rows.Add(person);
 *      -> System.InvalidOperationException:
 *         コントロールがデータバインドされているとき、
 *         DataGridView の行コレクションにプログラムで行を追加することはできません。
 *         
 *      => 行の追加は表示される。その後に上記の例外となる。
 *      
 *      => grid.DataSource = entity.PersonRR.Local から
 *         grid.DataSource = entity.PersonRR.Local.ToBindingList() に変更すると
 *         「grid.Rows.Add(person);」をしなくても表示に反映する。削除も ちゃんとできる。
 *      
 *@NOTE【Problem】
 *      ・Cell.Valueで Idの値は取れているが、
 *        DBを DELETE するのは、指定行の index を削除してしまう。
 *      ・LINQ内で (int), Int32.Parse(string)は 利用できない。
 *      ・おそらく、Remove(PersonRR)内で、
 *        引数の行と一致する rowIndexを調べ、その行を DELETEしているのでは？
 *        
 *      => grid.DataSource = entity.PersonRR.Local から
 *         grid.DataSource = entity.PersonRR.Local.ToBindingList() に変更すると
 *         削除も ちゃんとできる。
 *
 *@NOTE【Problem】grid.CurrentRow.Cells[0].Value?.ToString();
 *      空行の Id は 直前の行の Id の値になっているときと、0になっているときがある。
 *      
 *      bool  String.IsNullOrEmpty(string) では空行を識別できない。
 *      bool  cellCollection[0].Value.ToString() == "0"; でも識別できない。
 *      
 *      => 表示のための BingingList と、DbSet.Localのデータが異なる様子
 *         BindingList()時の 空行は、実際には 空行ではなく、
 *         表示するために挿入されたもの。
 *         空行の RowIndex は、最終レコードと同じになる。
 *         空行の row.Cells[rowIndex].Value は　最終行のデータが入っている
 *         
 *      => 空行の判定は Grid_SelectionChanged()で null -> ""に置換した
 *         TextBox.Text が "" であるかどうかで判定すると解決
 *         
 *@see ImageDbSetAddSaveChangesSample.jpg
 *@see 
 *@author shika
 *@date 2022-10-21
 */
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    class MainDbSetAddSaveChangesSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
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
        private readonly TextBox[] textBoxAry;
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
            grid.DataSource = entity.PersonRR.Local.ToBindingList();
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
            textBoxAry = new TextBox[4];

            textBoxName = new TextBox()
            {
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            textBoxAry[0] = textBoxName;
            table.Controls.Add(textBoxName, 1, 1);
            table.SetColumnSpan(textBoxName, 2);

            textBoxAddress = new TextBox()
            {
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            textBoxAry[1] = textBoxAddress;
            table.Controls.Add(textBoxAddress, 1, 2);
            table.SetColumnSpan(textBoxAddress, 2);

            textBoxTel = new TextBox()
            {
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            textBoxAry[2] = textBoxTel;
            table.Controls.Add(textBoxTel, 1, 3);
            table.SetColumnSpan(textBoxTel, 2);

            textBoxEmail = new TextBox()
            {
                Padding = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            textBoxAry[3] = textBoxEmail;
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
                Text = "Update Row",
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
            
            var row = grid.CurrentRow;
            DataGridViewCellCollection cellCollection = row.Cells;
         
            textBoxName.Text = cellCollection["Name"].Value?.ToString() ?? "";
            textBoxAddress.Text = cellCollection["Address"].Value?.ToString() ?? "";
            textBoxTel.Text = cellCollection["Tel"].Value?.ToString() ?? "";
            textBoxEmail.Text = cellCollection["Email"].Value?.ToString() ?? "";
        }//Grid_SelectionChanged()

        private void ButtonInsert_Click(object sender, EventArgs e)
        {
            bool isDuplicateRow = JudgeDuplicateRow();
            if (isDuplicateRow) { return; }
            bool canInsert = ValidateInput();
            if (!canInsert) { return; }

            //---- Insert Row ----
            string messageTitle = "Confirm to Insert DB";

            PersonRR insertPerson = new PersonRR()
            {
                Name = textBoxName.Text,
                Address = textBoxAddress.Text,
                Tel = textBoxTel.Text,
                Email = textBoxTel.Text,
            };

            DialogResult result = ShowConfirmMessageBox(
                "This record will be insert new row into Database, OK ?\n\n" + 
                insertPerson.ToString(), 
                messageTitle);

            if (result == DialogResult.Cancel) { return; }
            else if (result == DialogResult.OK)
            {
                entity.PersonRR.Add(insertPerson);
                entity.SaveChanges();
                grid.Refresh();
            }
        }//ButtonInsert_Click()

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            bool isEmptyRow = JudgeEmptyRow();
            if (isEmptyRow) { return; }

            var row = grid.CurrentRow;
            var cellCollection = row.Cells;

            StringBuilder deleteBld = new StringBuilder();
            deleteBld.Append("This record will be removed from Database, really OK ?\n\n");

            foreach(DataGridViewCell cell in cellCollection)
            {
                deleteBld.Append($"{cell.Value},\n");
            }

            DialogResult result = MessageBox.Show(
                deleteBld.ToString(),
                "Confirm to Delete Row",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) { return; }
            else if (result == DialogResult.OK)
            {
                int id = (int)cellCollection[0].Value;
                PersonRR deletePersonRR = entity.PersonRR.Single(
                    person => person.Id == id);
                Console.WriteLine(deletePersonRR.Id);
                entity.PersonRR.Remove(deletePersonRR);
                entity.SaveChanges();
                grid.Refresh();
            }
        }//ButtonDelete_Click()
        
        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            bool isEmptyRow = JudgeEmptyRow();
            if (isEmptyRow) { return; }

            var row = grid.CurrentRow;
            var cellCollection = row.Cells;

            bool canUpdate = ValidateInput();
            if (!canUpdate) { return; }

            //---- Update Row ----
            List<string> olderValueList = new List<string>();
            List<string> changedValueList = new List<string>();
            List<int> updateCellIndex = new List<int>();
            StringBuilder updateBld = new StringBuilder();
            updateBld.Append("The below value changes will be updated to Database, OK ?\n\n");
            updateBld.Append("[Older Value]  =>  [Changed Value]\n");

            for (int i = 0; i < textBoxAry.Length; i++)
            {
                string olderValue = cellCollection[i + 1].Value?.ToString() ?? "";
                string changedValue = textBoxAry[i].Text;

                if(olderValue != changedValue)
                {
                    olderValueList.Add(olderValue);
                    changedValueList.Add(changedValue);
                    updateCellIndex.Add(i);
                    updateBld.Append($"{olderValue} => {changedValue}\n");
                }
            }//for
            string message = null;

            if(changedValueList.Count == 0)
            {
                message = "(No Changed)";
            }
            else
            {
                message = updateBld.ToString();
            }
            string messageTitle = "Confirm to Update Database";
            DialogResult result = ShowConfirmMessageBox(message, messageTitle);

            if (result == DialogResult.Cancel) { return; }
            else if (result == DialogResult.OK)
            {
                Int32.TryParse(cellCollection[0].Value.ToString(), out int id);
                PersonRR updatePersonRR = entity.PersonRR
                    .Single(person => person.Id == id);

                updatePersonRR.Name = textBoxName.Text;
                updatePersonRR.Address = textBoxAddress.Text;
                updatePersonRR.Tel = textBoxTel.Text;
                updatePersonRR.Email = textBoxEmail.Text;
                
                entity.SaveChanges();
                grid.Refresh();
            }
        }//ButtonUpdate_Click()

        private bool JudgeEmptyRow()  //空行であるかを判定
        {
            //---- inputList ----
            List<string> inputList = new List<string>();

            foreach (TextBox textBox in textBoxAry)
            {
                inputList.Add(textBox.Text);
            }//foreach

            //---- Judge Empty ----
            bool isEmptyRow = inputList.All(value => value == "");

            //---- Test Print ----
            //Console.WriteLine("inputList: ");
            //inputList.ForEach(value => Console.Write($"{value}, "));
            //Console.WriteLine();
            //Console.WriteLine($"IsEmptyRow: {isEmptyRow}");

            //---- MessageBox ----
            if (isEmptyRow)
            {
                MessageBox.Show("(Empty Row)", "Notation");
            }

            return isEmptyRow;
        }//JudgeEmptyRow()

        private bool JudgeDuplicateRow()  //重複行であるかを判定
        {
            //---- inputList ----
            List<string> inputList = new List<string>();

            foreach (TextBox textBox in textBoxAry)
            {
                inputList.Add(textBox.Text);
            }//foreach

            //---- cellValueList ----
            var row = grid.CurrentRow;
            var cellCollection = row.Cells;

            List<string> cellValueList = new List<string>();
            for (int i = 1; i < cellCollection.Count - 1; i++)
            {
                cellValueList.Add(cellCollection[i].Value?.ToString() ?? "");
            }//for

            //---- Judge Duplicate ----
            bool isDuplicateRow = inputList.SequenceEqual(cellValueList);

            //---- Test Print ----
            //Console.WriteLine("inputList: ");
            //inputList.ForEach(value => Console.Write($"{value}, "));
            //Console.WriteLine();
            //Console.WriteLine("cellValueList: ");
            //cellValueList.ForEach(value => Console.Write($"{value}, "));
            //Console.WriteLine();
            //Console.WriteLine($"IsDuplicateRow: {isDuplicateRow}");

            //---- MessageBox ----
            if (isDuplicateRow)
            {
                MessageBox.Show(
                    "(Duplicate Row)\nThis record already has been in Database.",
                    "Notation");
            }

            return isDuplicateRow;
        }//JudgeDuplicateRow()

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
                bld.Append("<！> Name should be discribed in 50 characters.\n");
            }

            if (textBoxAddress.Text.Trim().Length == 0)
            {
                bld.Append("<！> Address is required.\n");
            }

            if (textBoxTel.Text.Trim().Length > 50)
            {
                bld.Append("<！> Tel should be discribed in 50 characters.\n");
            }

            if (!textBoxTel.Text.Trim()
                .ToCharArray()
                .All(c => Char.IsDigit(c)))
            {
                bld.Append("<！> Tel should be discribed by Number ONLY.\n");
            }

            if (textBoxEmail.Text != "" && !Regex.IsMatch(textBoxEmail.Text.Trim(),
                @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", //Email〔NT27〕
                RegexOptions.IgnoreCase))
            {
                bld.Append("<！> Email should be discribed within the Format.\n");
            }

            //---- against SQL Injection ----
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
