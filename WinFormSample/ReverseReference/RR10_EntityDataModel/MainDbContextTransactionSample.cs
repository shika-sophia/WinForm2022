/** <!--
 *@title WinFormGUI / WinFormSample / ReverseReference / RR10_EntityDataModel
 *@class MainDbContextTransactionSample.cs
 *@class   └ new FormDbContextTransactionSample() : Form
 *@class       └ new SubDbContextEntityPersonRR() : DbContext
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content DbContext.Database / Transaction
 *         ・複数ユーザーからの DBアクセス (= マルチスレッド)などで、
 *           DBデータの競合による不整合を防ぐために 
 *           Commit, Rollbackの仕組みを作って DBアクセスする
 *
 *@subject ◆class Database -- System.Data.Entity
 *         ・DbContextクラスのプロパティ。DBの管理、Transaction処理に利用する
 *         
 *         Database  dbContext.Database
 *         ([×] 'new' is not available.)
 *         
 *         Action<string>        database.Log { get; set; }   DbContextで生成された SQL文のログをデリゲートに記録
 *         DbConnection          database.Connection { get; }
 *         DbContextTransaction  database.CurrentTransaction { get; }
 *         int?                  database.CommandTimeout { get; set; }
 *
 *         static void   database.SetInitializer<TContext>(IDatabaseInitializer<TContext> strategy) where TContext : DbContext;
 *           └ 引数 interface IDatabaseInitializer<in TContext> where TContext : DbContext -- System.Data.Entity.
 *                    └ class CreateDatabaseIfNotExists <TContext>
 *                              : IDatabaseInitializer<TContext> where TContext : DbContext
 *                                  DBが存在しない場合に DBを新規作成
 *                    └ class DropCreateDatabaseIfModelChanges
 *                              : IDatabaseInitializer<TContext> where TContext : DbContext
 *                                  DBの元になる Modelクラスが変更された場合に 旧 DBを破棄し、新規作成
 *                    └ class DropCreateDatabaseAlway
 *                              : IDatabaseInitializer<TContext> where TContext : DbContext
 *                                  常に DBを新規作成
 *                                  
 *                  DbContext 派生クラスのインスタンスが初めて使われた場合、このインターフェイスの実装が基になるデータベースの初期化に使用されます。
 *                  この初期化では、条件に基づいて、データベースの作成やデータベースへのデータのシードを行うことができます。
 *                  使用する方法は Database クラスの静的な InitializationStrategy プロパティを使用して設定されます。
 *                  
 *         void  database.Initialize(bool force);
 *                  このコンテキストで、登録された IDatabaseInitializer を実行します。
 *                  このメソッドは、通常、操作がトランザクションの一部である場合など、
 *                  限定的に実行すると問題が発生する操作を開始する前にデータベースが作成されてシードされていることを確認する必要があるときに使用されます。
 *                  引数 force:  true:   以前に実行したことがあるかどうかに関係なく、初期化子が実行されます。
 *                                       これは、アプリケーションの実行中にデータベースが削除され、初期化が必要になった場合に役立ちます。
 *                               false:  初期化子は、このアプリケーション ドメインの
 *                                       このコンテキスト、モデル、および接続に対して まだ実行されていない場合のみ実行されます。
 *         bool  database.Exists();
 *         void  database.Create();
 *         bool  database.CreateIfNotExists();
 *         bool  database.Delete();
 *         DbContextTransaction  database.BeginTransaction()
 *         DbContextTransaction  database.BeginTransaction(IsolationLevel)
 *           └ 引数 enum IsolationLevel  分離レベル
 *                 {
 *                     Unspecified = -1,       //指定した分離レベルとは異なる分離レベルが使用されていますが、レベルを確認できません。
 *                     Chaos = 16,             //これより分離性の高いトランザクションからの保留中の変更に対しては上書きできません。
 *                     ReadUncommitted = 256,  //ダーティ読み込みができます。つまり、共有ロックが発行されておらず、排他ロックが有効ではありません。
 *                     ReadCommitted = 4096,   //データが読み込まれている間、ダーティ読み込みを防ぐために共有ロックが保持されますが、トランザクションが終了する前にデータを変更できます。このため、読み込みは繰り返されません。また実際には存在しないデータを生成できます。
 *                     RepeatableRead = 65536, //他のユーザーがデータを更新できないようにするために、クエリで使用するすべてのデータをロックします。 繰り返し不能読み込みはできませんが、実際には存在しない行を生成できます。
 *                     Serializable = 1048576, //System.Data.DataSet にレンジ ロックがかけられ、トランザクションが完了するまで、他のユーザーは行を更新したりデータセットに行を挿入できません。
 *                     Snapshot = 16777216     //あるアプリケーションで変更中のデータを他のアプリケーションから読み取ることができるように、そのデータのバージョンを保存して、ブロッキングを減らします。 この場合、クエリを再実行しても、あるトランザクションで加えられた変更を、他のトランザクションで表示できません。
 *                 }
 *                 
 *         void  database.UseTransaction(DbTransaction);
 *           └ 引数 class DbTransaction  外部トランザクション
 *                  Entity Framework で外部トランザクション内でコマンドを実行する必要がある場合、
 *                  ユーザーは、Databaseオブジェクトの外部で作成されたデータベース トランザクションを渡すことができます。
 *                  または、null を渡して、フレームワークのそのトランザクションの情報をクリアします。
 *                  
 *         int  database.ExecuteSqlCommand(string sql, params object[] parameters);
 *         int  database.ExecuteSqlCommand(TransactionalBehavior, string sql, params object[] parameters);
 *         Task<int>  database.ExecuteSqlCommandAsync(TransactionalBehavior, string sql, params object[] parameters);
 *         Task<int>  database.ExecuteSqlCommandAsync(string sql, CancellationToken, params object[] parameters);
 *         Task<int>  database.ExecuteSqlCommandAsync(TransactionalBehavior, string sql, CancellationToken, params object[] parameters);
 *            └ 引数 enum TransactionalBehavior
 *                  {
 *                      EnsureTransaction = 0,      //トランザクションが存在しない場合、この操作のために新しいトランザクションが使用されます。
 *                      DoNotEnsureTransaction = 1  //既存のトランザクションがある場合、それが使用されます。それ以外の場合は、トランザクションなしでコマンドまたはクエリが実行されます。
 *                  }
 *         DbRawSqlQuery            database.SqlQuery(Type elementType, string sql, params object[] parameters);
 *         DbRawSqlQuery<TElement>  database.SqlQuery<TElement>(string sql, params object[] parameters);
 *           └ class DbRawSqlQuery
 *
 *@subject ◆abstract class DbTransaction : MarshalByRefObject, IDbTransaction, IDisposable
 *             -- System.Data.Common
 *         # DbTransaction  DbTransaction()
 *         
 *         +          DbConnection Connection { get; }
 *         + abstract IsolationLevel IsolationLevel { get; }
 *         # abstract DbConnection DbConnection { get; }
 *         
 *         + abstract void Commit();
 *         + abstract void Rollback();
 *         + void Dispose();
 *         # virtual void Dispose(bool disposing);
 *         
 *@subject ◆class DbContextTransaction : IDisposable 
 *             -- System.Data.Entity
 *         + DbContextTransaction  database.CurrentTransaction { get; }
 *         + DbContextTransaction  database.BeginTransaction()
 *           ([×] 'new' is not available.)
 *         
 *         + DbTransaction  UnderlyingTransaction { get; }
 *         + void Commit();
 *         + void Rollback();
 *         + void Dispose();
 *         # virtual void Dispose(bool disposing);
 *
 *@subject ◆class DbRawSqlQuery : IEnumerable, IListSource, IDbAsyncEnumerable
 *              -- System.Data.Entity.Infrastructure.
 *         DbSqlQuery               dbSet.SqlQuery(string sql, params object[] parameters);
 *         DbRawSqlQuery            database.SqlQuery(Type elementType, string sql, params object[] parameters);
 *         DbRawSqlQuery<TElement>  database.SqlQuery<TElement>(string sql, params object[] parameters);
 *         DbRawSqlQuery  AsStreaming();  [非推奨] バッファリングの代わりに結果をストリームする新しいクエリを返します。
 *         ([×] 'new' is not available.)
 *         
 *         IEnumerator        dbRawSqlQuery.GetEnumerator();
 *         Task               dbRawSqlQuery.ForEachAsync(Action<object> action, [CancellationToken]);
 *         Task<List<object>> dbRawSqlQuery.ToListAsync([CancellationToken]);
 *         
 *@NOTE【Problem】Rollback()
 *      context内の SaveChanges()した処理を元に戻すだけで、
 *      BindingListで表示されている部分は 変更した値のままになっている。
 *      
 *      => 初期の状態を保存して、
 *           tempState = new  ObservableCollection<PersonRR>();
 *           tempState = grid.PersonRR.Local;
 *         Rollback()で代入しても元に戻らない
 *           grid.DataSource = initalLocal.ToBindingList();
 *           
 *      => 現在の値を保存して、Rollback()で代入しようと思ったが
 *         データバインドしている場合は grid.Rows().Add()は InvalidOperationException
 *         grid.Rows, row.Cellsは { get; } 読取専用のため、この方法では解決しない。
 *      
 *      => RollBack()内で context を 再 new して、DBから 再 Load()すると、元に戻る。
 *      
 *@NOTE【】ColumnHeaderChangedイベント
 *        明示的に設定していないのに、なぜかソート機能が備わっている・・
 *        
 *@see ImageDbContextTransactionSample.jpg
 *@see 
 *@author shika
 *@date 2022-10-30
 * -->
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    class MainDbContextTransactionSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormDbContextTransactionSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormDbContextTransactionSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormDbContextTransactionSample : Form
    {
        private readonly TableLayoutPanel table;
        private readonly DataGridView grid;
        private readonly Label[] labelAry;
        private readonly TextBox[] textBoxAry;
        private readonly Button[] buttonAry;
        private readonly Padding padding = new Padding(10);
        private readonly Color tempColor = Color.Lavender;
        private SubDbContextEntityPersonRR context;
        private Database db;
        private DbContextTransaction transaction;
        
        private readonly string[] labelTextAry = new string[]
        {
            "Name", "Address", "Tel", "Email",
        };

        private readonly string[] buttonTextAry = new string[]
        {
            "Insert", "Update", "Delete","Commit", "Rollback",
        };

        public FormDbContextTransactionSample()
        {
            this.Text = "FormDbContextTransactionSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(720, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 5,
                RowCount = 5,
                ClientSize = this.ClientSize,
                Margin = padding,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            
            for(int i = 0; i < table.ColumnCount; i++)
            {
                table.ColumnStyles.Add(new ColumnStyle(
                    SizeType.Percent, (float)Math.Round(100d / table.ColumnCount, 2))
                );
            }//for

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 60f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));

            //---- DataGridView ----
            context = new SubDbContextEntityPersonRR();
            db = context.Database;

            grid = new DataGridView()
            {
                DataSource = context.PersonRR.Local.ToBindingList(),
                ReadOnly = true,
                MultiSelect = false,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = 
                    DataGridViewAutoSizeColumnsMode.Fill,
                AutoGenerateColumns = true,
            };
            grid.SelectionChanged += new EventHandler(Grid_SelectionChanged);
            table.Controls.Add(grid, 0, 0);
            table.SetColumnSpan(grid, table.ColumnCount);

            //---- InitialComponent ----
            labelAry = new Label[labelTextAry.Length];
            textBoxAry = new TextBox[labelTextAry.Length];
            buttonAry = new Button[buttonTextAry.Length];
            InitialCompnent();

            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void InitialCompnent()
        {
            for(int i = 0; i < labelTextAry.Length; i++)
            {
                //---- Label ----
                labelAry[i] = new Label()
                {
                    Text = $"{labelTextAry[i]}: ",
                    TextAlign = ContentAlignment.TopRight,
                    Margin = padding,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                table.Controls.Add(labelAry[i], 1, i + 1);

                //---- TextBox ----
                textBoxAry[i] = new TextBox()
                {
                    Margin = padding,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                table.Controls.Add(textBoxAry[i], 2, i + 1);
                table.SetColumnSpan(textBoxAry[i], 2);
            }//for Label, TextBox

            //---- Button ----
            for (int i = 0; i < buttonTextAry.Length; i++)
            {
                buttonAry[i] = new Button()
                {
                    Text = buttonTextAry[i],
                    Margin = padding,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                buttonAry[i].Click += new EventHandler(Button_Click);
                table.Controls.Add(buttonAry[i], i, table.RowCount);
            }//for Button
        }//InitialComponent()

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.CurrentRow == null) { return; }  //when selected ColumnHeader

            var row = grid.CurrentRow;
            var cellCollection = row.Cells;

            for(int i = 0; i < textBoxAry.Length; i++)
            {
                textBoxAry[i].Text = 
                    cellCollection[i + 1].Value?.ToString() ?? "";
            }//for
        }//Grid_SelectionChanged

        private void Button_Click(object sender, EventArgs e)
        {
            string buttonText = (sender as Button).Text;
            switch (buttonText)
            {
                case "Insert":
                    InsertRow();
                    break;
                case "Update":
                    UpdateRow();
                    break;
                case "Delete":
                    DeleteRow();
                    break;
                case "Commit":
                    CommitDatabase();
                    break;
                case "Rollback":
                    RollbackChanges();
                    break;
            }//switch
        }//Button_Click()

        private void InsertRow()
        {
            bool isEmptyRow = JudgeEmptyRow();
            if(isEmptyRow) { return; }
            bool isDuplicateRow = JudgeDuplicateRow();
            if(isDuplicateRow) { return; }
            bool canInsert = ValidateInput();
            if(!canInsert) { return; }

            //---- Insert ----
            if (db.CurrentTransaction == null)
            {
                transaction = db.BeginTransaction();
            }

            var insertPerson = new PersonRR()
            {
                Name = textBoxAry[0].Text,
                Address = textBoxAry[1].Text,
                Tel = textBoxAry[2].Text,
                Email = textBoxAry[3].Text,
            };

            var insertBld = new StringBuilder();
            insertBld.Append("The below values will be inserted temporarilly.\n");
            insertBld.Append("If reflect actually into Database, Please Push 'Commit' Button.\n");
            insertBld.Append("If cancel, Please Push 'Rollback' Button.\n\n");
            insertBld.Append(insertPerson.ToString());

            DialogResult insertAnswer = ShowConfirmMessageBox(
                insertBld.ToString(), "Confirm to Insert");
            if(insertAnswer == DialogResult.Cancel) { return; }
            else if(insertAnswer == DialogResult.OK)
            {
                context.PersonRR.Add(insertPerson);
                context.SaveChanges();
                grid.Rows[grid.NewRowIndex - 1]              //空行の分を -1
                    .DefaultCellStyle.BackColor = tempColor; //未Commintの色
                grid.Refresh();
            }
        }//InsertRow()

        private void UpdateRow()
        {
            bool isEmptyRow = JudgeEmptyRow();
            if (isEmptyRow) { return; }
            bool isDuplicateRow = JudgeDuplicateRow();
            if (isDuplicateRow) { return; }
            bool canInsert = ValidateInput();
            if (!canInsert) { return; }

            //---- Update ----
            if (db.CurrentTransaction == null)
            {
                transaction = db.BeginTransaction();
            }

            var row = grid.CurrentRow;
            var cellCollection = row.Cells;

            var updateBld = new StringBuilder();
            updateBld.Append("The below values will be updated temporarilly.\n");
            updateBld.Append("If update actually into Database, Please Push 'Commit' Button.\n");
            updateBld.Append("If cancel, Please Push 'Rollback' Button.\n\n");
            updateBld.Append("[Older Value]  =>  [New Value]\n");

            for (int i = 0; i < textBoxAry.Length; i++)
            {
                string olderValue = cellCollection[i + 1].Value.ToString();
                string changedValue = textBoxAry[i].Text;
                
                if(olderValue != changedValue)
                {
                    updateBld.Append($"{olderValue} => {changedValue}\n");
                }

                cellCollection[i + 1].Value = textBoxAry[i].Text;
            }//for

            DialogResult updateAnswer = ShowConfirmMessageBox(
                updateBld.ToString(), "Confirm to Update");
            if(updateAnswer == DialogResult.Cancel) { return; }
            else if (updateAnswer == DialogResult.OK)
            {
                context.SaveChanges();
                row.DefaultCellStyle.BackColor = tempColor; //未Commintの色
                grid.Refresh();
            }
        }//UpdateRow()

        private void DeleteRow()
        {
            bool isEmptyRow = JudgeEmptyRow();
            if (isEmptyRow) { return; }

            //---- Delete ----
            if (db.CurrentTransaction == null)
            {
                transaction = db.BeginTransaction();
            }

            var row = grid.CurrentRow;
            var cellCollection = row.Cells;
            int id = (int)cellCollection[0].Value;
            PersonRR deletePerson = context.PersonRR
                .Single(person => person.Id == id);
                
           var deleteBld = new StringBuilder();
            deleteBld.Append("The below values will be deleted temporarilly.\n");
            deleteBld.Append("If delete actually from Database, Please Push 'Commit' Button.\n");
            deleteBld.Append("If cancel, Please Push 'Rollback' Button.\n\n");
            deleteBld.Append(deletePerson);

            DialogResult deleteAnswer = ShowConfirmMessageBox(
                deleteBld.ToString(), "Confirm to Delete");
            if(deleteAnswer == DialogResult.Cancel) { return; }
            else if (deleteAnswer == DialogResult.OK)
            {
                context.PersonRR.Remove(deletePerson);
                context.SaveChanges();
                grid.Refresh();
            }            
        }//DeleteRow()

        private void CommitDatabase()
        {
            if(db.CurrentTransaction == null) { return; }

            DialogResult commitAnswer = ShowConfirmMessageBox(
                "All Temporary changes will commit into Database, OK ?",
                "Confirm to Commit");
            if(commitAnswer == DialogResult.Cancel) { return; }
            else if (commitAnswer == DialogResult.OK)
            {
                transaction.Commit();
                
                foreach (DataGridViewRow row in grid.Rows)
                {
                    row.DefaultCellStyle.BackColor = this.BackColor;
                }//foreach

                grid.Refresh();
            }
        }//CommitDatabase()

        private void RollbackChanges()
        {
            if (db.CurrentTransaction == null) { return; }

            DialogResult rollbackAnswer = ShowConfirmMessageBox(
                "The current state will be disappeared,\nand turn back before all changes.\n\nRollback OK ?\n",
                "Confirm to Rollback");
            if(rollbackAnswer == DialogResult.Cancel) { return; }
            else if(rollbackAnswer == DialogResult.OK)
            {
                transaction.Rollback();
                context = new SubDbContextEntityPersonRR();
                db = context.Database;
                grid.DataSource = context.PersonRR.Local.ToBindingList();
                grid.Refresh();
            }
        }//RollbackChanges()

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
            StringBuilder errorMessageBld = new StringBuilder();

            //==== Valdate input ====
            //---- Name: textBoxAry[0] ----
            if (textBoxAry[0].Text.Trim().Length == 0)
            {
                errorMessageBld.Append("<！> Name is required.\n");
            }

            if (textBoxAry[0].Text.Trim().Length > 50)
            {
                errorMessageBld.Append("<！> Name should be discribed in 50 characters.\n");
            }

            //---- Address: textBoxAry[1] ----
            if (textBoxAry[1].Text.Trim().Length == 0)
            {
                errorMessageBld.Append("<！> Address is required.\n");
            }

            //---- Tel: textBoxAry[2] ----
            if (textBoxAry[2].Text.Trim().Length > 50)
            {
                errorMessageBld.Append("<！> Tel should be discribed in 50 characters.\n");
            }

            if (!textBoxAry[2].Text.Trim()
                .ToCharArray()
                .All(c => Char.IsDigit(c)))
            {
                errorMessageBld.Append("<！> Tel should be discribed by Number ONLY.\n");
            }

            //---- Email: textBoxAry[3] ----
            if (textBoxAry[3].Text != "" && !Regex.IsMatch(textBoxAry[3].Text.Trim(),
                @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", //Email〔NT27〕
                RegexOptions.IgnoreCase))
            {
                errorMessageBld.Append("<！> Email should be discribed within the Format.\n");
            }

            //---- against SQL Injection ----
            string[] textAry = new string[]
            {
                textBoxAry[0].Text, textBoxAry[1].Text,
                textBoxAry[2].Text, textBoxAry[3].Text,
            };

            foreach (string text in textAry)
            {
                if (Regex.IsMatch(text.Trim(), "[<>&;=]+") || Regex.IsMatch(text.Trim(), "[-]{2}"))
                {
                    errorMessageBld.Append("<！> Invalidate input !\n");
                }
            }//foreach

            //---- Show Error Massage ----
            if (errorMessageBld.Length > 0)
            {
                MessageBox.Show(
                    errorMessageBld.ToString(),
                    "Input Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }//ValidateInput()

        private DialogResult ShowConfirmMessageBox(string message, string messageTitle)
        {
            DialogResult answer = MessageBox.Show(
                message,
                messageTitle,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            return answer;
        }//ShowConfirmMessageBox()
    }//class
}
