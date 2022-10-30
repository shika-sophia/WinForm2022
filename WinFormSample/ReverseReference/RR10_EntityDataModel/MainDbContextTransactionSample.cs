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
 *
 *@subject ◆class Database -- System.Data.Entity
 *         ・DbContextクラスのプロパティ。DBの管理、Transaction処理に利用する
 *         
 *         Database  dbContext.Database
 *         ([×] 'new' is not available.)
 *         
 *         Action<string> Log { get; set; }   DbContextで生成された SQL文のログをデリゲートに記録
 *         DbConnection Connection { get; }
 *         DbContextTransaction CurrentTransaction { get; }
 *         int? CommandTimeout { get; set; }
 *
 *         static void SetInitializer<TContext>(IDatabaseInitializer<TContext> strategy) where TContext : DbContext;
 *           └ 引数 interface IDatabaseInitializer<in TContext> where TContext : DbContext
 *                  DbContext 派生クラスのインスタンスが初めて使われた場合、このインターフェイスの実装が基になるデータベースの初期化に使用されます。
 *                  この初期化では、条件に基づいて、データベースの作成やデータベースへのデータのシードを行うことができます。
 *                  使用する方法は Database クラスの静的な InitializationStrategy プロパティを使用して設定されます。
 *                  System.Data.Entity.DropCreateDatabaseIfModelChanges`1、
 *                  System.Data.Entity.DropCreateDatabaseAlways`1、
 *                  System.Data.Entity.CreateDatabaseIfNotExists`1  の実装が提供されます。
 *                  
 *         bool Exists();
 *         void Create();
 *         bool CreateIfNotExists();
 *         bool Delete();
 *         DbContextTransaction BeginTransaction()
 *         DbContextTransaction BeginTransaction(IsolationLevel)
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
 *@subject ◆class DbContextTransaction : IDisposable -- System.Data.Entity
 *
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
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    class MainDbContextTransactionSample
    {
        [STAThread]
        static void Main()
        //public void Main()
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

        private readonly string[] labelTextAry = new string[]
        {
            "Name", "Address", "Tel", "Email",
        };

        private readonly string[] buttonTextAry = new string[]
        {
            "Insert", "Update", "Delete", "Rollback",
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
                ColumnCount = 4,
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
            var context = new SubDbContextEntityPersonRR();
            var db = context.Database;

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
            throw new NotImplementedException();
        }
    }//class
}
