/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR10_EntityDataModel
 *@class MainDbContextSample.cs
 *@class   └ new FormDbContextSample() : Form
 *@class       └ new SubDbContextEntitySample : DbContext  (as self defined)
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
 *@content RR[281]-[286] p505-512 / DbContext
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
 *                                DbCompiledModel  model:  コンテキストをサポートするモデル。〔下記〕
 *                                
 *         + DbContext   new DbContext(DbConnection existingConnection, [DbCompiledModel model], bool contextOwnsConnection)
 *                           既存の接続を使用してデータベースに接続する新しいDbContextインスタンスを生成
 *                           引数 DbConnection  existingConnection:    新しいコンテキストに使用する既存の接続。
 *                                bool  contextOwnsConnection:  true:  DbContext が破棄されたときに接続も破棄。
 *                                                              false: 呼び出し元が接続を破棄する必要がある。
 *         + DbContext   new DbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
 *
 *         Database         dbContext.Database { get; }
 *           └ class Database  => 〔MainDbContextTransactionSample.cs〕
 *         DbChangeTracker  dbContext.ChangeTracker { get; }
 *           └ class DbChangeTracker => 〔下記〕
 *         DbContextConfiguration  dbContext.Configuration { get; }
 *           └ class DbContextConfiguration => 〔下記〕
 *         
 *         DbEntityEntry  dbContext.Entry(object entity)
 *         DbEntityEntry<TEntity>  dbContext.Entry<TEntity>(TEntity) where TEntity : class
 *                         エンティティに関する情報にアクセスし、エンティティに対してアクションを実行できる、
 *           └ class DbEntityEntry => 〔下記〕
 *           
 *         int        dbContext.SaveChanges()       dbContextでの変更を DBに同期的に反映する
 *         Task<int>  dbContext.SaveChangesAsync()  dbContextでの変更を DBに非同期的に反映する
 *         Task<int>  dbContext.SaveChangesAsync(CancellationToken)
 *                       引数 cancellationToken: Task完了を監視する Token
 *                       
 *         DbSet      dbContext.Set(Type entityType)
 *         DbSet<TEntity>   dbContext.Set<TEntity>() where TEntity : class;
 *                       コンテキストの特定の型のエンティティと基になるストアにアクセスするための DbSetインスタンスを返します。
 *           └ class DbSet => 〔下記〕
 *           
 *       # bool      dbContext.ShouldValidateEntity(DbEntityEntry entityEntry);
 *       + IEnumerable<DbEntityValidationResult>  dbContext.GetValidationErrors();
 *       # DbEntityValidationResult               dbContext.ValidateEntity(DbEntityEntry, IDictionary<object, object> items);
 *       # void      dbContext.OnModelCreating(DbModelBuilder);
 *       + void      dbContext.Dispose()
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
 *@subject ◆class DbChangeTracker -- System.Data.Entity.Infrastructure.
 *         DbChangeTracker  dbContext.ChangeTracker { get; }
 *         ([×] 'new' is not available.)
 *         
 *         IEnumerable<DbEntityEntry>           dbChangeTracker.Entries()
 *         IEnumerable<DbEntityEntry<TEntity>>  dbChangeTracker.Entries<TEntity>() where TEntity : class;
 *         bool  dbChangeTracker.HasChanges();
 *                   dbContext.SaveChanges() が呼び出されたときにDBに送信される変更を
 *                   DbContext が追跡しているかどうか
 *       
 *         void  dbChangeTracker.DetectChanges();
 *                   POCO Entityのプロパティとリレーションシップに加えられた変更を検出します。
 *                   TEntityの型によっては (System.Data.Entity.Core.Objects.DataClasses.EntityObjectから派生する変更追跡プロキシやエンティティなど)、
 *                   変更が自動的に報告され、これらの型のエンティティに対する DetectChanges の呼び出しは通常必要ないことに注意してください。
 *                   また、通常 DetectChanges は、DbContext のメソッドとそれに関連するクラスによって自動的に呼び出されるので、
 *                   このメソッドを明示的に呼び出す必要があるのは ほとんどないです。
 *                   ただし、一般的にはパフォーマンス上の理由から、
 *                   DbContext.Configuration から AutoDetectChangesEnabled フラグを使って
 *                   DetectChanges の自動呼び出しをオフにした方がよい場合があります。
 *
 *@subject ◆class DbContextConfiguration -- System.Data.Entity.Infrastructure
 *         DbContextConfiguration Configuration { get; }
 *         ([×] 'new' is not available.)
 *         
 *         bool  dbConfig.EnsureTransactionsForFunctionsAndCommands { get; set; }
 *                 SQL の関数とコマンドが常にトランザクション内で実行されるかどうか
 *         bool  dbConfig.LazyLoadingEnabled { get; set; }
 *                 リレーションシップの遅延読み込みが有効になっているかどうか。default: true
 *         bool  dbConfig.ProxyCreationEnabled { get; set; }                       default: true
 *                 Entity型のインスタンスが作成されるたびに、動的に生成されたProxyクラスのインスタンスがフレームワークによって作成されるかどうか
 *                 このフラグによって Proxy の作成が有効になっていても、エンティティ型がプロキシ扱いの要件を満足しなければ
 *                 Proxyインスタンスは作成されないことに注意してください。
 *         bool  dbConfig.UseDatabaseNullSemantics { get; set; }                    default: false
 *                 null になる可能性のある 2 つのオペランドを比較する際、
 *                 データベースの null セマンティクスを使用するかどうか
 *                 例 (operand1 == operand2) は次のように変換されます。
 *                 UseDatabaseNullSemantics = true:   (operand1 = operand2)。
 *                 UseDatabaseNullSemantics = false:  それぞれ 
 *                    (((operand1 = operand2) AND (NOT (operand1 IS NULL 
 *                    OR operand2 IS NULL))) OR ((operand1 IS NULL) 
 *                    AND (operand2 IS NULL)))
 *         bool  dbConfig.AutoDetectChangesEnabled { get; set; }                default: true
 *                    dbChangeTracker.DetectChanges()を 
 *                    DbContext と関連クラスのメソッドによって自動的に呼び出すかどうか
 *         bool  dbConfig.ValidateOnSaveEnabled { get; set; }                   default: true
 *                    dbContext.SaveChanges() 時、追跡されているエンティティが自動的に検証されるかどうか
 *                    
 *@subject ◆class DbEntityEntry  -- System.Data.Entity.Infrastructure
 *         DbEntityEntry           dbContext.Entry(object entity)
 *         DbEntityEntry<TEntity>  dbContext.Entry<TEntity>(TEntity) where TEntity : class
 *         ([×] 'new' is not available.)
 *
 *         object            dbEntityEntry.Entity { get; }
 *         DbPropertyValues  dbEntityEntry.OriginalValues { get; }
 *         DbPropertyValues  dbEntityEntry.CurrentValues { get; }
 *            └ class DbPropertyValues
 *         EntityState       dbEntityEntry.State { get; set; }
 *            └ enum EntityState
 *              {
 *                 Detached = 1,   エンティティは、コンテキストによって追跡されていません。
 *                                 エンティティが new 演算子 または dbSet.Create() によって作成されると、直ちに この状態になります。
 *                 Unchanged = 2,  エンティティはコンテキストによって追跡されていて、データベースに存在します。
 *                                 また、プロパティ値はデータベースの値から変更されていません。
 *                 Added = 4,      エンティティはコンテキストによって追跡されていますが、
 *                                 データベースにまだ存在していません。
 *                 Deleted = 8,    エンティティはコンテキストによって追跡されていて、
 *                                 データベース内に存在していますが、
 *                                 SaveChanges() が次回呼び出されたときにデータベースから削除するようにマークが付けられています。
 *                 Modified = 16   エンティティはコンテキストによって追跡されていて、
 *                                 データベースに存在します。
 *                                 また、一部またはすべてのプロパティ値が変更されています。
 *             }
 *             
 *         DbPropertyEntry          dbEntityEntry.Property(string propertyName);
 *         DbComplexPropertyEntry   dbEntityEntry.ComplexProperty(string propertyName);
 *         DbMemberEntry            dbEntityEntry.Member(string propertyName);
 *         DbReferenceEntry         dbEntityEntry.Reference(string navigationProperty);
 *         DbCollectionEntry        dbEntityEntry.Collection(string navigationProperty);
 *         DbEntityEntry<TEntity>   dbEntityEntry.Cast<TEntity>() where TEntity : class;
 *         
 *         DbPropertyValues         dbEntityEntry.GetDatabaseValues();
 *         Task<DbPropertyValues>   dbEntityEntry.GetDatabaseValuesAsync([CancellationToken]);
 *         DbEntityValidationResult dbEntityEntry.GetValidationResult();
 *         void                     dbEntityEntry.Reload();
 *         Task                     dbEntityEntry.ReloadAsync([CancellationToken]);
 *
 *@subject ◆class DbSet<TEntity> : DbQuery<TEntity>, IDbSet<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>, IQueryable, IEnumerable, IInternalSetAdapter where TEntity : class
 *                   -- System.Data.Entity
 *         # DbSet<TEntity>      new DbSet<TEntity>()  where TEntity : class;
 *         DbSet<TEntity>        DbContext派生クラス.[テーブルクラス]  <- プロパティに「DbSet<T> テーブル名」を記述
 *         
 *         ObservableCollection<TEntity>  dbSet.Local { get; }   
 *             ・Entityのキャッシュを保持。自動更新される
 *             ・ObservableCollection<TEntity>
 *         TEntity               dbSet.Add(TEntity entity)
 *         IEnumerable<TEntity>  dbSet.AddRange(IEnumerable<TEntity> entities)
 *         TEntity               dbSet.Attach(TEntity entity)
 *         TEntity               dbSet.Create()
 *         TDerivedEntity        dbSet.Create<TDerivedEntity>() where TDerivedEntity : class, TEntity;
 *         TEntity               dbSet.Find(params object[] keyValues);
 *         Task<TEntity>         dbSet.FindAsync(CancellationToken cancellationToken, params object[] keyValues);
 *         Task<TEntity>         dbSet.FindAsync(params object[] keyValues);
 *         TEntity               dbSet.Remove(TEntity entity)
 *         IEnumerable<TEntity>  dbSet.RemoveRange(IEnumerable<TEntity> entities)
 *         DbSqlQuery<TEntity>   dbSet.SqlQuery(string sql, params object[] parameters)
 *         
 *@subject ◆interface IQueryable : IEnumerable
 *@subject ◆static class QueryableExtensions -- System.Data.Entity
 *         void  DbSet<T>.Load(this IQueryable source)  
 *                 拡張メソッド: 第１引数 this のクラスにこのメソッドを追加
 *                 Load(): DbSet<T>, ObjectSet<T>, ObjectQuery<T>などのサーバー クエリを対象に、
 *                         クエリの結果がクライアント上の関連付けられた DbContext, ObjectContextなどのキャッシュに読み込まれるように、
 *                         クエリを列挙します。これは ToList() を呼び出してから、実際にリストを作成するオーバーヘッドなしでリストを破棄する場合と同じです。
 *    
 *@subject ◆DataGridView  =>〔MainDataGridViewBasicSample.cs〕
 */
#endregion 
/*
 *@see ImageDbContextSample.jpg
 *@see MainPrepareEntityFrameworkModelSample.cs
 *@author shika
 *@date 2022-10-18
 */
using System;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Drawing;
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
                Text = "Connect DB by DbContext",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            flow.Controls.Add(label);

            button = new Button()
            {
                Text = "Show Person Table",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            flow.Controls.Add(button);

            grid = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
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
                @"data source =(LocalDB)\MSSQLLocalDB;initial catalog = ASPState;integrated security = True;";

            var entityBld = new EntityConnectionStringBuilder();
            entityBld.Provider = "System.Data.SqlClient";
            entityBld.ProviderConnectionString = connectionString;
            entityBld.Metadata =
                "res://*/WinFormSample.ReverseReference.RR10_EntityDataModel.EntityDataModelRR.csdl | " +
                "res://*/WinFormSample.ReverseReference.RR10_EntityDataModel.EntityDataModelRR.ssdl | " +
                "res://*/WinFormSample.ReverseReference.RR10_EntityDataModel.EntityDataModelRR.msl";

            var conn = new EntityConnection(entityBld.ToString());
            using (var entity = new SubDbContextEntitySample(conn)) 
            {
                // この時点で，DBに対して「SELECT * FROM Person;」を発行する
                entity.PersonRR.Load(); 

                // DbSet.Localを使う事で，ローカルにキャッシュされたデータを使う
                grid.DataSource = entity.PersonRR.Local.ToBindingList();
                grid.AutoGenerateColumns = true;

                entity.Dispose();
                conn.Close();
            }//using

        }//Button_Click()
    }//class
}
