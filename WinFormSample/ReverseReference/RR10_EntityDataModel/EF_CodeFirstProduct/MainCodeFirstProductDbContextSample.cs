/** <!--
 *@title WinFormGUI / WinFormSample / 
 *@class MainCodeFirstProductDbContextSample.cs
 *@class   └ new FormCodeFirstProductDbContextSample() : Form
 *@class       └ new 
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
 *@content EF CodeFirstProductDbContextSample
 *@subject Database.SetInitializer(IDatabaseInitializer<TContext>)  
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
 *@subject モデル変更
 *         System.InvalidOperationException:
 *           データベースの作成後、
 *           'SubDbContextEntityProductRR' コンテキストの背後にあるモデルが変更されました。
 *           Code First Migrations を使用したデータベースの更新を検討してください 
 *
 *@NOTE【Problem】Initializerを変更しても、上記の例外となる
 *      Database.SetInitializer(
 *         new DropCreateDatabaseIfModelChanges<SubDbContextEntityPersonRR>());
 *         
 *      => Migrationを行うしかなさそう
 *
 *@NOTE【】Code First
 *      Model -> DB を作成する開発方法だが、
 *      Visual Studioで行う場合、EntityDataModelを通して行ったほうがいい。
 *      
 *      このクラス MainCodeFirstProductDbContextSample.csを直接実行しても、
 *      Refererence [EF]の通り、DB作成が可能で データも入るのだが、
 *      VSの サーバーエクスプローラには テーブルが出現しない。
 *      
 *      ＊テーブルの存在を確認するには SQL Serverから入る必要がある。
 *      VS [Menu] -> [Tool] -> [SQL Server] -> [New Query] ->
 *      [[Connect Server Dialog]]
 *      Local: MSSQLServerLocal
 *      Connection: WinFormGUI\WinFormSample\ReverseReference\RR10_EntityDataModel\EF_CodeFirstProduct\SubDbContextEntityProductRR.cs
 *      -> [OK] 
 *      
 *      ＊SELECT * FROM ProductModelRR; で確認可能
 *      => 〔文末 Result〕
 *      => 〔PoductModelRRs_tb.sql〕
 *      
 *@see ImageCodeFirstProductDbContextSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-02
 * -->
 */
using System;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel.EF_CodeFirstProduct
{
    class MainCodeFirstProductDbContextSample
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Console.WriteLine("new FormCodeFirstProductDbContextSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormCodeFirstProductDbContextSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormCodeFirstProductDbContextSample : Form
    {
        //private readonly DataGridView grid;

        public FormCodeFirstProductDbContextSample()
        {
            this.Text = "FormCodeFirstProductDbContextSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            ProductModelRR[] productAry = new ProductModelRR[]
            {
                new ProductModelRR() { Name = "Pzk-III", Price = 300, },
                new ProductModelRR() { Name = "Pzk-IV-H", Price = 800, },
                new ProductModelRR() { Name = "Pzk-V Tiger", Price = 1700, },
                new ProductModelRR() { Name = "Pzk-VI-A PanterA", Price = 1600 },
            };

            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<SubDbContextEntityPersonRR>());

            using (var context = new SubDbContextEntityProductRR())
            {
                context.Products.AddRange(productAry);
                context.SaveChanges();
                
                foreach (ProductModelRR product in context.Products)
                {
                    Console.WriteLine(product.Name);
                }//foreach

                context.Dispose();
            }//using

            //grid = new DataGridView()
            //{
            //    DataSource = context.Products.Local,
            //    MultiSelect = false,
            //    AutoGenerateColumns = true,
            //    AutoSizeColumnsMode =DataGridViewAutoSizeColumnsMode.Fill,
            //    AutoSize = true,
            //};

            this.Controls.AddRange(new Control[]
            {
                //grid,
            });
        }//constructor
    }//class
}

/*
//==== Execute ====
new FormCodeFirstProductDbContextSample()

//==== Result ====
Pzk-III
Pzk-IV-H
Pzk-V Tiger
Pzk-VI-A PanterA
*/