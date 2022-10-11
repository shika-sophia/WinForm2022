/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR09_FileDirectory
 *@class MainByteFileWriteSample.cs
 *@class   └ new FormByteFileWriteSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[272] p480 / ByteFileWriteSample
 *@subject ◆FileStream class -- System.IO.
 *         FileStream  new FileStream(string path, FileMode)
 *            引数 enum FileMode
 *            {
 *                CreateNew = 1,    ファイルを作成。既に存在すると IOException
 *                Create = 2,       ファイルを作成。既に存在すると 上書き
 *                Open = 3,         ファイルを開く。存在しないと IOException
 *                OpenOrCreate = 4, ファイルを開く。存在しないと 作成
 *                Truncate = 5,     ファイルを開く。ファイルサイズを 0にする。(内容を削除)
 *                Append = 6,       内容を追加
 *            }
 *            
 *          FileStream  File.Create(string path)
 *          FileStream  File.OpenRead(string path)
 *          FileStream  File.OpenText(string path)   UTF-8 読取専用
 *          FileStream  File.OpenWrite(string path)
 *         (StreamWriter  File.CreateText(string path, bool append)     )
 *         (StreamWriter  File.AppendText(string path)  UTF-8 書込専用   )
 *          
 *          void  fileStream.Write(byte[], int offset, int length)
 *          int   fileStream.Read(byte[], int offset, int length)
 *          long  foleStream.Length   バイト長のファイルサイズ
 *          void  fileStream.Close()
 *
 *@NOTE【】Binary Editor
 *      Binary file ('.bin' file) can be read  as text, 
 *      because Visual Studio has binary editor.
 *      Windows 'Copy' function or usual Text Editor cannot be read.
 *      So see the Image. 〔below〕
 *      
 *@see ImageByteFileWriteSample.jpg
 *@see ImageBinaryDataSample_bin.jpg
 *@author shika
 *@date 2022-10-12
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR09_FileDirectory
{
    class MainByteFileWriteSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormByteFileWriteSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormByteFileWriteSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormByteFileWriteSample : Form
    {
        private readonly TableLayoutPanel table;
        private readonly Label label;
        private readonly TextBox textBox;
        private readonly Button button;

        public FormByteFileWriteSample()
        {
            this.Text = "FormByteFileWriteSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 180);
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Fill,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            label = new Label()
            {
                Text = "File Name:",
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(label, 0, 0);

            textBox = new TextBox()
            {
                Text = "/binaryDataSample.bin",
                TextAlign = HorizontalAlignment.Left,
                Dock = DockStyle.Fill,
                Multiline = false,
            };
            table.Controls.Add(textBox, 1, 0);

            button = new Button()
            {
                Text = "Create Binary File",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button, 0, 1);
            table.SetColumnSpan(button, 2);

            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void Button_Click(object sender, EventArgs e)
        {
            string dir = Path.GetDirectoryName(
                "../../WinFormSample/ReverseReference/RR09_FileDirectory/MainByteFileWriteSample.cs");
            string fileName = textBox.Text;

            if (String.IsNullOrEmpty(fileName)) { return; }

            byte[] byteDataAry = new byte[1024];
            for(int i = 0; i < byteDataAry.Length; i++)
            {
                byteDataAry[i] = (byte)(i % (0xFF + 1)); // 0-255 as byte range
            }//for

            long byteLength = 0L;
            using(FileStream fs = File.Create(dir + fileName))
            {
                for(int i = 0; i < byteDataAry.Length; i++)
                {
                    fs.Write(byteDataAry, 0, byteDataAry.Length);
                }//for
                byteLength = fs.Length;

                fs.Close();
            }//using

            MessageBox.Show(
                $"Created binary file of {byteLength:N} Bytes",
                "Result");
        }//Button_Click()
    }//class
}
