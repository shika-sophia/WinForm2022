/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR10_EntityDataModel
 *@class MainDbContextSample.cs
 *@class   └ new FormDbContextSample() : Form
 *@class       └ new SubDbContextEntitySample : DbContext  (as self defined)
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference MB marunaka-blog《【C#】DataGridViewの使い方》, 2021
 *              https://marunaka-blog.com/csharp-datagridview-use/3088/
 *              =>〔~\Reference\Article_DataGridView.txt〕
 *              
 *@content RR[281]-[] p505- / DbContext
 *
 */
#region -> ■ DbContext Reference
/*
 *@subject ◆class DbContext : IDisposable, IObjectContextAdapter
 *                   -- System.Data.Entity.
 *         DbContext インスタンスは、データベースに照会してすべての変更をグループ化し、
 *         1 つの単位としてストアに書き戻すことができるような、
 *         作業単位パターンとリポジトリパターンの組み合わせを表します。
 *         DbContext は ObjectContext と概念的に似ています。
 *         
 *         [ + public / # protected ]
 *         # DbContext   new DbContext();
 *         # DbContext   new DbContext(DbCompiledModel model);
 *         + DbContext   new DbContext(string nameOrConnectionString)
 *         + DbContext   new DbContext(string nameOrConnectionString, [DbCompiledModel model])
 *                           指定文字列を接続先のデータベース名前または接続文字列として使用する新しいDbContextインスタンスを生成
 *                           指定されたモデルで そのインスタンスを初期化
 *                           引数 nameOrConnectionString:  データベース名または接続文字列。
 *                                model:  コンテキストをサポートするモデル。
 *                                
 *         + DbContext   new DbContext(DbConnection existingConnection, [DbCompiledModel model], bool contextOwnsConnection)
 *                           既存の接続を使用してデータベースに接続する新しいDbContextインスタンスを生成
 *                           引数 DbConnection  existingConnection:    新しいコンテキストに使用する既存の接続。
 *                                bool  contextOwnsConnection:  true:  DbContext が破棄されたときに接続も破棄。
 *                                                              false: 呼び出し元が接続を破棄する必要がある。
 *         + DbContext   new DbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
 *
 *         Database       dbContext.Database { get; }
 *         
 *         DbEntityEntry  dbContext.Entry(object entity)
 *         DbEntityEntry<TEntity>  dbContext.Entry<TEntity>(TEntity) where TEntity : class
 *                         エンティティに関する情報にアクセスし、エンティティに対してアクションを実行できる、
 *         
 *         int        dbContext.SaveChanges()       dbContextでの変更を DBに同期的に反映する
 *         Task<int>  dbContext.SaveChangesAsync()  dbContextでの変更を DBに非同期的に反映する
 *         Task<int>  dbContext.SaveChangesAsync(CancellationToken)
 *                       引数 cancellationToken: Task完了を監視する Token
 *                       
 *         DbSet      dbContext.Set(Type entityType)
 *         DbSet<TEntity>   dbContext.Set<TEntity>() where TEntity : class;
 *                       コンテキストの特定の型のエンティティと基になるストアにアクセスするための DbSetインスタンスを返します。
 *                       
 *         void      dbContext.Dispose()
 *       # void      dbContext.Dispose(bool disposing)
 *                       引数 disposing:  true: マネージ リソースとアンマネージ リソースの両方を解放
 *                                        false: アンマネージ リソースだけを解放
 *                                        
 *@subject ◆class DbCompiledModel -- System.Data.Entity.Infrastructure
 *                System.Data.Entity.Core.Objects.ObjectContext の作成に使用できる、
 *                または System.Data.Entity.DbContextのコンストラクターに渡すことのできる
 *                Entity Data Model (EDM) モデルの不変表記。
 *                パフォーマンスを向上させるには、
 *                この型のインスタンスをキャッシュしてコンテキストの作成に再使用する必要があります。
 *
 *      + TContext  CreateObjectContext<TContext>(DbConnection existingConnection)
 *                      where TContext : ObjectContext
 *                      (where: 型パラメータ <T> の制約:  TContext を ObjectContext に限定)
 *                  ObjectContext のインスタンスまたは ObjectContext から派生したクラスを作成します。
 *                  代わりに適切な DbContext コンストラクターを使用することによって
 *                  DbContext のインスタンスを作成できることに注意してください。
 *                  派生 ObjectContext を使用する場合は、単独の EntityConnectionパラメーターを持つ public コンストラクターが必要です。
 *                  渡された接続は作成された ObjectContext によって使用されますが、
 *                  そのコンテキストに所有されることはありません。
 *                  コンテキストが破棄された後、その接続は呼び出し側で破棄する必要があります。
 *                  
 *@subject ◆abstract class DbConnection : Component, IDbConnection, IDisposable
 *             -- System.Data.Common.
 *          # DbConnection  new DbConnection()
 *
 *         abstract string  dbConnection.Database      { get; }
 *         abstract string  dbConnection.DataSource    { get; }
 *         abstract string  dbConnection.ServerVersion { get; }
 *         abstract string  dbConnection.ConnectionString  { get; set; }
 *         abstract ConnectionState  dbConnection.State { get; }
 *         virtual  int     dbConnection.ConnectionTimeout { get; }
 *         
 *         abstract void    dbConnection.Open()              ConnectionString で指定した設定を使用して、データベース接続を開きます。
 *                  Task    dbConnection.Open()OpenAsync();　上記の非同期バージョン。 CancellationToken.None
 *                  Task    dbConnection.OpenAsync(CancellationToken) 
 *                             上記の非同期バージョン。
 *                             引数 cancellationToken: Task完了を監視する Token
 *         abstract void    dbConnection.Close();
 *         
 *         abstract void       dbConnection.ChangeDatabase(string databaseName);
 *         abstract DbCommand  dbConnection.CreateDbCommand();
 *                  DbCommand  dbConnection.CreateCommand()  
 *                  DataTable  dbConnection.GetSchema()     データ ソースのスキーマ情報を返します
 *                  DataTable  dbConnection.GetSchema(string collectionName)  スキーマ名のデータ ソース情報を返します
 *                  DataTable  dbConnection.GetSchema(string collectionName, string[] restrictionValues);
 *                                 スキーマ名に指定した文字列と制限値に指定した文字列配列でデータ ソースのスキーマ情報を返します
 *                                 
 *@subject ◆EntityConnection : DbConnection -- System.Data.Entity.Core.EntityClient.
 *         EntityConnection  new EntityConnection()
 *         EntityConnection  new EntityConnection(string connectionString)
 *         EntityConnection  new EntityConnection(MetadataWorkspace, DbConnection)
 *         EntityConnection  new EntityConnection(
 *             MetadataWorkspace, DbConnection, bool entityConnectionOwnsStoreConnection)
 *         
 *         (メンバーは DbConnectionと共通)
 *         
 *         
 *@subject ◆EntityConnectionStringBuilder  : DbConnectionStringBuilder
 *             -- System.Data.Entity.Core.EntityClient.
 *         EntityConnectionStringBuilder  new EntityConnectionStringBuilder()
 *         EntityConnectionStringBuilder  new EntityConnectionStringBuilder(string connectionString)
 *         
 *         object  this[string keyword]  { get; set; }  インデクサー
 *         string  entityBld.Name     { get; set; }
 *         string  entityBld.Provider { get; set; }
 *         string  entityBld.ProviderConnectionString  { get; set; }
 *         string  entityBld.Metadata { get; set; }
 *         string  entityBld.ToString()
 *         ICollection  entityBld.Keys { get; }
 *         
 *         bool    entityBld.ContainsKey(string keyword)
 *         bool    entityBld.TryGetValue(string keyword, out object value);
 *         bool    entityBld.Remove(string keyword)
 *         void    entityBld.Clear()
 *
 *@subject DbSet<T>
 *
 *@subject ◆DataGridView : Control, ISupportInitialize -- System.Windows.Forms
 *         DataGridView   new DataGridView()
 *         
 *         (Mainly Members)  dataGrid.Xxxx
 *         
 *         ＊Indexer
 *         DataGridViewCell this[string columnName, int rowIndex] { get; set; }
 *         DataGridViewCell this[int columnIndex, int rowIndex]   { get; set; }
 *         
 *         ＊Property
 *         string Text { get; set; }
 *         object DataSource { get; set; }
 *         string DataMember { get; set; }
 *         bool AutoGenerateColumns { get; set; }
 *         DataGridViewColumnCollection Columns { get; }
 *         DataGridViewRowCollection Rows { get; }
 *         
 *         int ColumnCount { get; set; }
 *         int RowCount { get; set; }
 *         int ColumnHeadersHeight { get; set; }
 *         int RowHeadersWidth { get; set; }
 *         Size DefaultSize { get; }
 *         DataGridViewAutoSizeRowsMode AutoSizeRowsMode { get; set; }
 *         DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode { get; set; }
 *         DataGridViewRowHeadersWidthSizeMode RowHeadersWidthSizeMode { get; set; }
 *         
 *         bool MultiSelect { get; set; }
 *         DataGridViewColumn SortedColumn { get; }
 *         DataGridViewRow CurrentRow { get; }
 *         DataGridViewCell CurrentCell { get; set; }
 *               
 *         bool EnableHeadersVisualStyles { get; set; }
 *         DataGridViewCellStyle RowsDefaultCellStyle { get; set; }
 *         DataGridViewCellStyle RowHeadersDefaultCellStyle { get; set; }
 *         DataGridViewCellStyle AlternatingRowsDefaultCellStyle { get; set; }
 *         DataGridViewHeaderBorderStyle RowHeadersBorderStyle { get; set; }
 *         
 *         SortOrder SortOrder { get; }
 *           └ enum SortOrder
 *         DataGridViewSelectionMode SelectionMode { get; set; }
 *           └ enum DataGridViewSelectionMode
 *         
 *         DataGridViewSelectedColumnCollection SelectedColumns { get; }
 *         DataGridViewSelectedRowCollection    SelectedRows { get; }
 *         DataGridViewSelectedCellCollection   SelectedCells { get; }
 *         
 *         Panel EditingPanel { get; }
 *         Control EditingControl { get; }
 *         DataGridViewEditMode EditMode { get; set; }
 *           └ enum DataGridViewEditMode
 *           
 *         ＊Method
 *         AutoResizeColumn(int columnIndex, [DataGridViewAutoSizeColumnMode]);
 *         void AutoResizeColumns([DataGridViewAutoSizeColumnsMode]);
 *         void AutoResizeColumnHeadersHeight([int columnIndex]);
 *         void AutoResizeRow(int rowIndex, [DataGridViewAutoSizeRowMode]);
 *         void AutoResizeRowHeadersWidth(int rowIndex, [DataGridViewRowHeadersWidthSizeMode]);
 *         void AutoResizeRows(DataGridViewAutoSizeRowsMode autoSizeRowsMode);
 *         
 *         bool BeginEdit(bool selectAll);
 *         bool EndEdit(DataGridViewDataErrorContexts context);
 *         bool CommitEdit(DataGridViewDataErrorContexts);
 *         bool CancelEdit();
 *         bool RefreshEdit();
 *         
 *         void UpdateCellValue(int columnIndex, int rowIndex);
 *         void SelectAll();
 *         void ClearSelection();
 *         
 *         void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction);
 *         void Sort(IComparer comparer);
 */
#endregion 
/*
 *@see ImageDbContextSample.jpg
 *@see 
 *@author shika
 *@date 2022-10-18
 */
using System;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    class MainDbContextSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormDbContextSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormDbContextSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormDbContextSample : Form
    {
        private readonly FlowLayoutPanel flow;
        private readonly Label label;
        private readonly Button button;
        private readonly DataGridView grid;

        public FormDbContextSample()
        {
            this.Text = "FormDbContextSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            //this.ClientSize = new Size(640, 640);
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            flow = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            label = new Label()
            {
                Text = "",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            flow.Controls.Add(label);

            button = new Button()
            {
                Text = "",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            flow.Controls.Add(button);

            grid = new DataGridView()
            {

            };
            flow.Controls.Add(grid);

            this.Controls.AddRange(new Control[]
            {
                flow,
            });
        }//constructor

        private void Button_Click(object sender, EventArgs e)
        {
            string connectionString = 
                "data source = .;initial catalog = ASPState;integrated security = True;";

            var entityBld = new EntityConnectionStringBuilder();
            entityBld.Provider = "System.Data.SqlClient";
            entityBld.ProviderConnectionString = connectionString;
            entityBld.Metadata =
                "res://*/WinFormSample.ReverseReference.RR10_EntityDataModel.EntitiDataModelRR.csdl | " +
                "res://*/WinFormSample.ReverseReference.RR10_EntityDataModel.EntitiDataModelRR.ssdl | " +
                "res://*/WinFormSample.ReverseReference.RR10_EntityDataModel.EntitiDataModelRR.msl";

            var conn = new EntityConnection(entityBld.ToString());
            var entity = new SubDbContextEntitySample(conn);
            DbSqlQuery<PersonRR> sql = entity.PersonRR.SqlQuery("SELECT * FROM PersonRR;");
            
            
            grid.DataSource = entity.PersonRR.Local.ToBindingList();
            grid.AutoGenerateColumns = true;
            grid.Rows.AddRange();

            //(Editing...)

        }//Button_Click()
    }//class
}
