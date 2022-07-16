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
 *         ・Windowsエクスプローラのようなフォルダ／ファイル表示
 *         ・List: リストで一覧表示
 *         ・Details: 複数列でテーブル表示など
 *         
 *@subject ◆ListView : Control -- System.Woindows.Forms
 *         ListView  new ListView()
 *         bool   listView.GridLines
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
 *         int          listView.Items.Add(ListViewItem)
 *         void         listView.Items.AddRange(ListViewItem[])
 *           └ ListViewItem new ListViewItem()
 *             ListViewItem new ListViewItem(string[])  配列で１行分のレコードを追加
 *             int          listViewItem.Index
 *             ListViewSubItemCollection
 *                             listViewItem.SubItems   = listView.Items[i].SubItems
 *             ListViewItem    listViewItem.SubItems.Add(ListViewSubItem / string)  各行の各列の値を表すオブジェクトを追加
 *             void            listViewItem.SubItems.AddRange(ListViewSubItem[] / string[])
 *             ListViewSubItem listViewItem.GetSubItemAt(int x, int y)  クライアント座標(x, y)の SubItem
 *             void            listViewItem.BeginEdit()    編集モードを開始
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
 *@subject 効果
 *         bool  listView.HoverSelection   マウスポイント時に自動選択  / デフォルト false
 *         bool  listView.HotTracking      マウスポイント時にリンクの外観に変化 / false
 *         bool  listView.HideSelection    フォーカス外したとき協調も外す       / true
 *         bool  listView.MultiSelect      複数選択可 / デフォルト true
 *         bool  listView.FullRowSelect    マウスクリック時に1行全て選択 / false
 *         bool  listView.LabelEdit        コントロールの項目を編集可能にするか / false
 *         bool  listView.LabelWrap        アイコン表示のとき、ラベルを折り返すか / true
 *         
 *@subject ソート
 *         SortOrder listView.Sorting
 *           └ enum SortOrder 
 *             {
 *                 None = 0,
 *                 Ascending = 1,  昇順
 *                 Descending = 2, 降順
 *             }
 *             
 *         IComparer listView.ListViewItemSorter  ソートの比較子を取得/設定
 *         void      listView.Sort()              ソートの実行
 *         
 *@subject 検索
 *         ListViewItem listView.FindItemWithText(
 *            string text, 列名
 *            [bool,       SubItemも含むか
 *            [int,        start index
 *            [bool ]]])   部分一致も含むか
 *         ListViewItem listView.GetItemAt(int x, int y) クライアント座標(x, y)の項目
 *         
 *@see FormListViewSample.jpg
 *@see ~/WinFormSample/ReverseReference/RR04_Control/MainListViewSubItemSample.cs
 *@author shika
 *@date 2022-07-06, 07-07, 07-16
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
