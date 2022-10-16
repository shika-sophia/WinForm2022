/** <!--
 *@title WinFormGUI / WinFormSample / ReverseReference / RR10_EntityDataModel
 *@class MainShowTableSample.cs
 *@class   └ new FormShowTableSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content RR[279][280] p500 / ShowTableSample
 *@subject EDM: Entity Data Model の作成 
 *         VS[ソリューションエクスプローラ] -> フォルダ選択 ->
 *         [追加] -> [新しい項目] -> [データ] -> [ADO.NET Entity Data Model]
 *         -> (Model名を入力) -> [OK]
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
 *         ・接続名前空間 
 *         
 *         〔~/App.config〕内
 *         <configuration>
 *         <configSections>
 *            //<!ーー For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 ーー>
 *            <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection,
 *              EntityFramework, Version=6.0.0.0, 
 *              Culture=neutral, PublicKeyToken=b77a5c561934e089"
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
 *                provider=System.Data.SqlClient; provider connection string=&quot;
 *                data source=(LocalDB)\MSSQLLocalDB;
 *                initial catalog=ASPState;
 *                integrated security=True;
 *                MultipleActiveResultSets=True;
 *                App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
 *         </ connectionStrings >
 *                :
 *                
 *@see ImageShowTableSample.jpg
 * @see 
 *@author shika
 *@date 2022-10-17
 * -->
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel
{
    class MainShowTableSample
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Console.WriteLine("new FormShowTableSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormShowTableSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormShowTableSample : Form
    {
        private readonly FlowLayoutPanel flow;
        private readonly Label label;
        private readonly Button button;
        private readonly DataGrid dataGrid;

        public FormShowTableSample()
        {
            this.Text = "FormShowTableSample";
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
                Text = "Connect Database as Entity Data Model.",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            flow.Controls.Add(label);

            button = new Button()
            {
                Text = "Show Tabel",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            flow.Controls.Add(button);

            dataGrid = new DataGrid()
            {
                Dock = DockStyle.Fill,
            };
            flow.Controls.Add(dataGrid);

            this.Controls.AddRange(new Control[]
            {
                flow,
            });
        }//constructor

        private void Button_Click(object sender, EventArgs e)
        {
            var entity = new EntityDataModelRRSetting();
            dataGrid.DataSource = entity.PersonRR_tb;

            //(Studing... about DbContext, DataGrid)
        }
    }//class
}
