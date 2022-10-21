/** <!--
 *@title WinFormGUI / WinFormSample / ReverseReference / RR10_EntityDataModel
 *@class MainPrepareEntityDataModelSample.cs
 *@class   └ new FormPrepareEntityDataModelSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content RR[279][280] p500 / PrepareEntityDataModelSample
 *         EDM: Entity Data Model 〔Entity Framework | NT 77A〕
 *              Relation DBの Table DataSet形式を オブジェクト指向で操作できるように
 *              データ変換(=マッピング)した [.NET Framework 3.5-] Entity Frameworkの機能  
 *
 *@subject EDM: Entity Data Model の作成 
 *         VS[ソリューションエクスプローラ] -> フォルダ選択 ->
 *         [追加] -> [新しい項目] -> [データ] -> [ADO.NET Entity Data Model]
 *         -> (Model名を入力) -> [OK]
 *         
 *         => SQL Server に データ接続した Entity Data Model を自動生成
 *         
 *@subject ＊DBにテーブルを追加
 *         VS [サーバーエクスプローラ] -> [テーブル] 右クリック -> [新しいテーブルの追加]
 *         
 *         ＊テーブルにデータを追加
 *         VS [サーバーエクスプローラ] -> [テーブル] -> テーブル名を右クリック 
 *         -> [テーブルデータを表示]
 *         -> Design View でデータを追加 -> [更新]ボタン
 *         (上記でデータを追加すると INSERT文を自動生成される)
 *   
 *         => 〔dbo.PersonRR_tb.sql〕
 *         
 *@subject DBテーブルから Entity Data Modelに import
 *         EDM「.edmx」を開く -> (Design Viewのような画面) 右クリック
 *         -> [データベースからモデルを更新] 
 *         ->【更新ウィザード】追加タブ -> テーブル check -> 完了
 *         -> 更新タブ [dbo]に Person-tbが追加されるので、選択して[完了]
 *         ->《！》再ビルドをする [Ctrl] + [B]
 *         
 *         => EDM「.edmx」階層内に DBの「Person_tb」の列名とデータ型を
 *         プロパティ名 と .NET型に変換した partial class「Person_tb.cs」が自動生成される。
 *         (自分で手書きしたクラスを作らないように、それは普通の C#自己定義クラスです。)
 *
 *@subject ConnectionString 接続文字列
 *         ＊EDM作成ウィザードで入力する文字列
 *         ・モデル名     EDM「.edmx」のファイル名
 *         ・接続文字列  「(アプリルートからの階層).EntitiDataModelRR.csdl」「.ssdl」「.msl」のクラス名になる
 *         ・接続設定     <connectionStrings><add name="EntityDataModelRRSetting" ...>
 *                       Form内で DB接続のため EDMを newするときのクラス名
 *         ・接続名前空間 ??
 *         
 *         〔~/App.config〕内
 *         <configuration>
 *         <configSections>
 *            //<!ーー For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 ーー>
 *            <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection,
 *              EntityFramework, Version=6.0.0.0, 
 *              Culture=neutral, PublicKeyToken=xxxxxxxxxxxxx"
 *              requirePermission="false" />
 *         </configSections>
 *         <startup>
 *            <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
 *         </startup>
 *         <connectionStrings>
 *             <add name="EntityDataModelRRSetting" 
 *                connectionString="metadata=res://*／WinFormSample.ReverseReference.RR10_EntityDataModel.EntitiDataModelRR.csdl
 *                | res://*／WinFormSample.ReverseReference.RR10_EntityDataModel.EntitiDataModelRR.ssdl
 *                | res://*／WinFormSample.ReverseReference.RR10_EntityDataModel.EntitiDataModelRR.msl;
 *                provider=System.Data.SqlClient; 
 *                provider connection string="data source=(LocalDB)\MSSQLLocalDB;
 *                    initial catalog=ASPState;
 *                    integrated security=True;
 *                    MultipleActiveResultSets=True;
 *                    App=EntityFramework" 
 *                providerName="System.Data.EntityClient" />
 *         </ connectionStrings >
 *                :
 *                
 *@subject new EntityDataModelRRSetting();
 *         ・EDM: EntityDataModel 作成時に自動作成されるクラス
 *         ・各Table の DbSet<T>をプロパティに持つ
 *         ・new時に DB内の 全Tableの DbSet<T>のインスタンスを生成するので、少し重い
 *         ・利用する Tableだけを読み込むなら、DbContext派生クラスを利用すべき
 *           =>〔MainDbContextEntitySample.cs〕
 *           
 *@subject new SubDbContextEntitySample() : DbContext
 *         ・DbContext派生クラス
 *         ・DbSet<PersonRR> PersonRR { get; }  プロパティに 利用する Table名の DbSet<T>を記述
 *         ・PersonRRクラスは、同名Tableの 全Columnをプロパティとするクラスを用意する
 *         ・DbConnection をコンストラクタの引数とするか、内部化しておく
 *         ・このままだと列名の Headerだけ表示される
 *         ・DbSet<T>.Load() で DBに対して「SELECT * FROM PersonRR;」を発行する
 *           実行結果は 各行のデータを読み込み、DbSet<T>.Localにキャッシュを保持する
 *           
 *          =>〔MainDbContextEntitySample.cs〕
 *          
 *@NOTE【Problem】DataGrid.ItemsSource 
 *      Compiler Error【CS1061】:
 *          'type' does not contain a definition for 'name' and no accessible extension method 'name'
 *          accepting a first argument of type 'type' could be found
 *          (are you missing a using directive or an assembly reference ?).
 *           
 *          DataGrid型に 'ItemsSource'の定義が含まれておらず、
 *          第１引数に受け入れ可能な 'DataGrid'型の アクセス可能な拡張メソッド 'ItemsSource'は見つけられません。
 *          (using ディレクティブ もしくは アセンブリ参照が不足していないか確認してください。)
 *
 *     【MSDN】DataGrid.ItemsSource Property
 *      https://learn.microsoft.com/en-us/previous-versions/windows/silverlight/dotnet-windows-silverlight/cc189398(v=vs.95)
 *      Microsoft Silverlight will reach end of support after October 2021.
 *      Namespace:  System.Windows.Controls
 *      Assembly:  System.Windows.Controls.Data (in System.Windows.Controls.Data.dll)
 *
 *      すでに Support終了しており、上記のアセンブリ「.dll」も VSの参照候補に存在しない
 *      
 *      【MSDN】DataGrid クラス
 *       https://learn.microsoft.com/ja-jp/dotnet/api/system.windows.forms.datagrid?view=netframework-4.8
 *       Namespace: System.Windows.Forms
 *       Assembly:  System.Windows.Forms.dll
 *       
 *       スクロールできるグリッドに ADO.NET データを表示します。
 *       このクラスは .NET Core 3.1 以降のバージョンでは利用できません。
 *       代わりにコントロールを DataGridView 使用し、コントロールを置き換えて拡張 DataGrid します。
 *       
 *       RR[279]-[314] p500- サンプルコードは 旧式のため そのままではコンパイルエラー
 *       => DataGridViewを利用すべき
 *       
 *@see ImagePrepareEntityDataModelSample.jpg
 *@see 
 *@author shika
 *@date 2022-10-17
 * -->
 */
using System;
using System.Data.Entity;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    class MainPrepareEntityDataModelSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormPrepareEntityDataModelSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormPrepareEntityDataModelSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormPrepareEntityDataModelSample : Form
    {
        private readonly FlowLayoutPanel flow;
        private readonly Label label;
        private readonly Button button;
        private readonly DataGridView grid;

        public FormPrepareEntityDataModelSample()
        {
            this.Text = "FormPrepareEntityDataModelSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 240);
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            flow = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                ClientSize = this.ClientSize,
                Padding = new Padding(10),
                Dock = DockStyle.Fill,
            };

            label = new Label()
            {
                Text = "Connect Database as Entity Data Model.",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            flow.Controls.Add(label);

            button = new Button()
            {
                Text = "Show Table",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            flow.Controls.Add(button);

            grid = new DataGridView()
            {
                ClientSize = this.ClientSize,
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
            var entity = new EntityDataModelRRSetting();
            grid.DataSource = entity.PersonRR.Local.ToBindingList();
            grid.AutoGenerateColumns = true;
        }
    }//class
}
