//==== Document Template ====

/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class 
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *
 *@content RR[95][96] RichTextBox -- Find(), LoadFile()                                
 *@subject RichTextBox ファイルをロード
 *         void   rich.LoadFile(string path, RichTextBoxStreamType)
 *         void   rich.LoadFile(Stream, RichTextBoxStreamType)
 *           |      └ FileStream(string path, FileMode) : Stream
 *           |      × StreamReader : TextReader 継承関係がないので利用不可
 *           |
 *           | OLE: Object Linking and Embedding オブジェクト
 *           | RTF: Rich Text Format
 *           └ enum RichTextBoxStreamType
 *             {
 *                 RichText = 0,        // リッチテキスト形式(= RTF: Rich Text Format) ストリーム。
 *                 PlainText = 1,　     // OLEの代わりに空白が含まれているプレーンテキスト ストリーム。
 *                 RichNoOleObjs = 2,   // OLEの代わりの空白を持つリッチテキスト形式 (RTF) のストリーム
 *                                      // rich.SaveFile()を利用する場合のみ有効
 *                 TextTextOleObjs = 3, // OLEのテキスト表現を持つプレーンテキスト ストリーム。
 *                                      // rich.SaveFile()を利用する場合のみ有効
 *                 UnicodePlainText = 4 // OLEの代わりに空白が含まれているテキストストリーム。 
 *                                      // テキストは Unicode でエンコード
 *             }
 *
 *@subject ファイルロード機能 EventHandler Method
 *         元ファイルの拡張子を
 *         string.EndsWith(string) で検出して、場合分け。
 *         ".txt", ".rtf"によって分岐し、 
 *         rich.LoadFile()の RichTextBoxStreamTypeをそれぞれ指定する。
 *         上記以外のファイルはエラーとなるので、MessageBoxで通知。
 *
 *@subject RichTextBox 検索機能
 *         int   rich.SelectionStart    //選択範囲の開始位置
 *         int   rich.SelectionLength   //選択範囲の文字数
 *         int   rich.Find(string text, //検索語の位置を返す。未検出は -1
 *                         int start,
 *                         RichTextBoxFinds)
 *           └ enum RichTextBoxFinds   ※ビット論理和「|」で複合可
 *             {
 *                 None = 0,         //完全一致でなくても可
 *                 WholeWord = 2,    //完全一致のみ
 *                 MatchCase = 4,    //大文字と小文字を区別
 *                 NoHighlight = 8,  //一致しても強調表示(=反転表示)されない
 *                 Reverse = 16      //末尾から検索
 *             }
 *             
 *          void  rich.Focus()    現在コントロールにフォーカスを当てる
 *                                Find()した語を協調表示 
 *                                (Focus()なしだと協調されない)
 *                                
 *@subject 検索 EventHandler Method
 *         rich.SelectionStart += rich.SelectionLength;  前回検索した位置から再検索
 *         
 *         検索語を変更した場合は 
 *         rich.SelectionStart = 0;
 *         rich.SelectionLength = 0; で最初から再検索
 *         
 *         int found = rich.Find(); で検索結果の位置を取得できるが、
 *         intに代入しなくても、検索結果が出る。
 *         rich.Forcus() をしないと、検索結果を強調表示しない。
 *         検索結果をどこで取得しているのかは不明。
 *         
 *@NOTE 文字化け問題 Encoding
 *      元テキストは UTF-8、
 *      LoadFile()での読取は Environment(= PC環境)の Encodingである "Shift-JIS"
 *      RichTextBoxStreamType.UnicodePlainText にすると、「応答なし」と出る。
 *      RichTextBox, LoadFile()には Encodingを定義できない。
 *      FileStreamには Encordingを定義するプロパティがない。
 *      StreamReaderには Encodingを定義できるが、rich.LoadFile()の引数にできない。
 *      
 *      => 元テキストを "Shift-JIS"で保存すると解決
 *      => それか rich.SaveFile()でセーブした RTF形式のファイルでないと、
 *         解決できないのかも。
 *         
 *@see FormRichTextBoxLoadFind.jpg
 *@author shika
 *@date 2022-07-20
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainRichTextBoxLoadFind
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormRichTextBoxLoadFind());
        }//Main()
    }//class

    class FormRichTextBoxLoadFind : Form
    {
        private FlowLayoutPanel flow;
        private Label labelFile;
        private Label labelSearch;
        private TextBox txFile;
        private TextBox txSearch;
        private Button btnFile;
        private Button btnSearch;
        private RichTextBox rich;
        private string pastSearch;

        public FormRichTextBoxLoadFind()
        {
            this.Text = "FormRichTextBoxLoadFind";
            this.AutoSize = true;

            Font font = new Font("ＭＳ ゴシック", 12, FontStyle.Bold);
            this.Font = font;

            //---- FlowLayoutPanel ----
            flow = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Dock = DockStyle.Top,
                AutoSize = true,
            };

            //---- File Load ----
            labelFile = new Label()
            {
                Text = "File Path: ",
                Margin = new Padding(0, top: 8, 0, 0),
                AutoSize = true,
            };

            txFile = new TextBox()
            {
                Width = 150,
                Multiline = false,
            };

            btnFile = new Button()
            {
                Text = "Load",
                AutoSize = true,
            };
            btnFile.Click += new EventHandler(btnFile_Click);

            //---- Search ----
            labelSearch = new Label()
            {
                Text = "Search Keyword: ",
                Margin = new Padding(left: 20, top: 10, 0, 0),
                AutoSize = true,
            };

            txSearch = new TextBox()
            {
                Width = 150,
                Multiline = false,
            };

            btnSearch = new Button()
            {
                Text = "Search",
                AutoSize = true,
            };
            btnSearch.Click += new EventHandler(btnSearch_Click);

            //---- Rich ----
            rich = new RichTextBox()
            {
                Location = new Point(10, 50),
                Size = new Size(730, 400),
                Multiline = true,               
            };

            //---- Deployment ----
            flow.Controls.AddRange(new Control[]
            {
                labelFile, txFile, btnFile,
                labelSearch, txSearch, btnSearch,
            });
            this.Controls.Add(flow);
            this.Controls.Add(rich);
        }//constructor

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.GetFullPath(txFile.Text)))
            {
                MessageBox.Show(
                    $"[ {txFile.Text} ]\nThis File does not exist.",
                    "Error");
                return;
            }
            
            if (txFile.Text.EndsWith(".txt"))
            { 
                rich.LoadFile(
                    txFile.Text, RichTextBoxStreamType.PlainText);
            }
            else if (txFile.Text.EndsWith(".rtf"))
            {
                rich.LoadFile(
                    txFile.Text, RichTextBoxStreamType.RichText);
            }
            else
            {
                MessageBox.Show(
                    "File Type is '.txt', '.rtf' ONLY",
                    "Error");
            }
        }//btnFile_Click()

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txSearch.Text == "") { return; }

            if (pastSearch == null || txSearch.Text == pastSearch)
            {   //前回検索した位置から再検索
                rich.SelectionStart =
                    rich.SelectionStart + rich.SelectionLength;
                rich.SelectionLength = 0;
            }   
            else
            {   //検索語が変化した場合は最初から検索
                rich.SelectionStart = 0;
                rich.SelectionLength = 0;
            }
            pastSearch = txSearch.Text;  //検索語を登録

            rich.Find(
                txSearch.Text,
                rich.SelectionStart,
                RichTextBoxFinds.None);
            
            rich.Focus();  //Find()した位置にフォーカスする
        }//btnSearch_Click()
    }//class

}
