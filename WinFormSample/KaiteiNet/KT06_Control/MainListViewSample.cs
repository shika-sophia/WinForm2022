/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT06_Control
 *@class MainListViewSample.cs
 *@class FormListViewSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/controls/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm06_Control.txt〕
 *           
 *@content KT 6.Control / ListView
 *@subject ◆ListView -- System.Woindows.Forms
 *         bool   listView.GridLine
 *         View   listView.View
 *           └ enum View
 *             {
 *                 LargeIcon = 0,  // 大アイコン表示
 *                 Details = 1,    // 詳細表示
 *                 SmallIcon = 2,  // 小アイコン表示
 *                 List = 3,       // リスト表示
 *                 Tile = 4,       // タイル表示
 *             }
 *             ※Detailsの場合 ColumnsHeaderオブジェクトを 
 *             Columnsプロパティに登録しておく必要がある
 *             
 *         ＊Columnsプロパティ
 *         int                    listView.Columns.Add(ColumnsHeader)
 *         ColumnHeader           listView.Columns.Add(
 *                                   string text, 
 *                                   int width, 
 *                                   HorizontalAlignment textAlign);
 *         void                   listView.Columns.AddRange(CoumunsHeader[])
 *         ColumnHeaderCollection listView.Columns   列ヘッダのコレクション
 *           └ ColumnsHeaderクラス
 *             string columnsHeader.Text  列の見出し
 *             
 *         ＊Itemsプロパティ
 *         ListViewItemCollection listView.Items 各行のレコード
 *           └ ListViewItem new ListViewItem()
 *             int Add(string[])                 配列で１行分のレコードを追加
 *         int                    listView.Items.Add(ListViewItem)
 *         void                   listView.Items.AddRange(ListViewItem[])
 *         
 *         (FileListViewは自己定義クラス)
 *         (AddFiles()は自己定義メソッド)
 *         
 *@subject 相対パス
 *         カレントフォルダ「.」は ~/bin/Debug/WinFormGUI.exe
 *         アプリルート「~」: ASP.NETのアプリルート記号は利用できず
 *
 *@subject 列幅の調整 
 *         listView.Columns.Add(string, int) の第2引数で widthを指定するか
 *         listView.Columns.AddRange(ColumnHeader[])を利用した場合は
 *         Columns[i]で各要素を指定し Widthプロパティを設定
 *         
 *         int listView.Columns[0].Width
 *
 *@see FormListViewSample.jpg
 *@author shika
 *@date 2022-07-06, 07-07
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT06_Control
{
    class MainListViewSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormListViewSample());
        }//Main()
    }//class

    class FormListViewSample : Form
    {
        private FileListView listView;

        public FormListViewSample()
        {
            this.Text = "FormListViewSample";
            this.Size = new Size(400, 300);

            listView = new FileListView()
            {
                GridLines = true,
                View = View.Details,
                Dock = DockStyle.Fill,
            };
            listView.AddFiles(
                Path.GetFullPath(
                    "../../WinFormSample/KaiteiNet/KT06_Control"));

            this.Controls.Add(listView);
        }//constructor
    }//class

    class FileListView : ListView
    {
        ColumnHeader headerFileName;
        ColumnHeader headerFilePath;

        public FileListView()
        {
            headerFileName = new ColumnHeader() { Text = "FileName" };
            headerFilePath = new ColumnHeader() { Text = "FilePath" };

            this.Columns.AddRange(new ColumnHeader[]
            {
                headerFileName, headerFilePath,
            });
            this.Columns[0].Width = 240;
            this.Columns[1].Width = 80;
        }

        public void AddFiles(string directoryPath)
        {
            foreach(string filePath in Directory.GetFiles(directoryPath))
            {
                string fileName = Path.GetFileName(filePath);

                this.Items.Add(new ListViewItem(
                    new string[] 
                    { 
                        fileName, filePath,
                    })
                );
            }//foreach
        }//AddFiles()
    }//class
}
