/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainRichTextBoxSave.cs
 *@class FormRichTextBoxSave.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[97] RichTextBox -- SaveFile()
 *@subject RichTextBox
 *         void  rich.SaveFile(string path)
 *         void  rich.SaveFile(string path, RichTextBoxStreamType)
 *         void  rich.SaveFile(Stream, RichTextBoxStreamType)
 *           |      └ FileStream(string path, FileMode) : Stream
 *           |      × StreamWriter : TextReader 継承関係がないので利用不可
 *           |
 *           | OLE: Object Linking and Embedding オブジェクト
 *           |         文字装飾に関する情報を格納するオブジェクトか？
 *           | RTF: Rich Text Format 
 *           |         Windows[Word]で開くテキスト、Font, Size, Colorなどの情報を含む
 *           |
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
 *@subject SeekDirectory()
 *         Main()を実行したソースコードのある Directoryを抽出。
 *         =>〔CSharpBegin / Utility / SeekPath.cs〕
 *
 *@NOTE 文字コード問題
 *      [.txt] で保存すると、"Shift-JIS"？を読込むための ASCIIコード？らしきもの
 *     「\'82\'ed\'82\'a9\'82\'e6\'82\'bd\'82\ ... \par」となり、
 *      Visual Studio (UTF-8)では正しく表示されない。
 *      
 *      [.rtf] で保存すると、ファイルを開くときに Windows[Word]が起動し、
 *      Font, Size, Colorなどが再現され、元のテキストも正しく表示される。
 *      
 *@see FormRichTextBoxSave.jpg
 *@see ./iroha.txt
 *@see ./iroha.rtf
 *@see CsharpBegin / Utility / SeekPath.cs
 *@author shika
 *@date 2022-07-21
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainRichTextBoxSave
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormRichTextBoxSave());
        }//Main()
    }//class

    class FormRichTextBoxSave : Form
    {
        private TableLayoutPanel table;
        private Label labelDir;
        private Label labelFile;
        private TextBox txDir;
        private TextBox txFile;
        private Button button;
        private RichTextBox rich;

        public FormRichTextBoxSave()
        {
            this.Text = "FormRichTextBoxSave";
            this.Font = new Font("ＭＳ 明朝", 12, FontStyle.Regular);
            this.AutoSize = true;

            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 4,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            labelDir = new Label()
            {
                Text = "Directory/Folder:",
                Margin = new Padding(5, 10, 0, 0),
                AutoSize = true,
            };
            table.Controls.Add(labelDir, 0, 0);

            labelFile = new Label()
            {
                Text = "File Name:",
                Margin = new Padding(5, 10, 0, 0),
                AutoSize = true,
            };
            table.Controls.Add(labelFile, 1, 0);

            txDir = new TextBox()
            {
                Text = SeekDirectory(),
                Multiline = false,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(txDir, 0, 1);

            txFile = new TextBox()
            {
                TabIndex = 0,
                Multiline = false,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(txFile, 1, 1);

            button = new Button()
            {
                Text = "Save",
                Dock = DockStyle.Fill,
            };
            button.Click += new EventHandler(button_Click);
            table.Controls.Add(button, 0, 2);
            table.SetColumnSpan(button, 2);

            rich = new RichTextBox()
            {
                Text = "いろはにほへと　ちりぬるを\n" +
                       "わかよたれそ つねならむ\n" +
                       "うゐのおくやま けふこえて\n" +
                       "あさきゆめみし ゑひもせす\n",
                Size = new Size(400, 300),
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(rich, 0, 3);
            table.SetColumnSpan(rich, 2);

            this.Controls.Add(table);
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            if (txDir.Text == "")
            {   //Main()を実行したソースコードのある Directory
                txDir.Text = SeekDirectory();
            }

            if (txFile.Text == "")
            {
                MessageBox.Show(
                      $"File Name is required.",
                      "Error",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
                return;
            }

            if (txFile.Text.Contains("."))
            {   //どの形式でも「.rtf」に変更
                txFile.Text =
                    txFile.Text.Substring(
                        0, txFile.Text.LastIndexOf("."))
                    + ".rtf";                   
            }
            else
            {  //ファイル名のみなら、拡張子「.rtf」を付加
                txFile.Text += ".rtf";
            }

            string path = $@"{txDir.Text}\{txFile.Text}";
            
            using(var fileStream = 
                new FileStream(path, FileMode.Create))
            {
                rich.SaveFile(fileStream, RichTextBoxStreamType.RichText);

                fileStream.Close();
            }//using

            MessageBox.Show(
                    $"[ {txFile.Text} ] saved.",
                    "Notation",
                    MessageBoxButtons.OK);
        }//button_Click()

        //Main()を実行したソースコードのファイル
        private string SeekFileFull()
        {
            StackTrace trace = new StackTrace(true);
            StackFrame frame = trace.GetFrame(trace.FrameCount - 1); //static Main()          
            return frame.GetFileName();
        }//SeekFile() 

        //Main()を実行したソースコードのある Directory
        private string SeekDirectory()
        {
            string thisFileName = SeekFileFull();
            string dir = thisFileName.Substring(
                0, thisFileName.LastIndexOf("\\"));

            return dir;
        }//SeekDirectory()
    }//class
}
