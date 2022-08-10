/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT10_CommonDialog
 *@class MainOpenFileDialogSample.cs
 *@class   └ new FormOpenFileDialogSample() : Form
 *#class       └ new OpenFileDialog() : FileDialog
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/common-dialogs/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm10_CommonDialog.txt〕
 *           
 *@content KT10 CommonDialog / OpenFileDialog | RR[274] p486
 *@subject ◆abstract FileDialog : abstract CommonDialog
 *         string   fileDialog.Title             FileDialogのタイトル
 *         string   fileDialog.InitialDirectory  最初に開くディレクトリ path
 *         string   fileDialog.Filter            [ファイルの種類] セレクト項目を定義
 *                   「表示文字列 | 検索文字列; 検索文字列; ... | 表示文字列 | 検索文字列; ...」
 *                   「*」ワイルドカード 任意の文字列
 *                    例: "画像ファイル|*.bmp; *.jpg; *.gif; *.png;|全てのファイル|*.*;"
 *         int      fileDialog.FilterIndex       [ファイルの種類]に最初に表示する Filter / デフォルト: 1
 *         string   fileDialog.FileName          選択したファイル path / デフォルト: ""
 *                                               ※ Component名が設定されている場合は削除 or 適切に設定
 *         string[] fileDialog.FileNames         選択した全てのファイルpath の配列
 *         string   fileDialog.DefaultExt        既定の拡張子を設定 / デフォルト: ""
 *         bool     fileDialog.AddExtension      拡張子が未入力の場合、自動で拡張子を付加 / デフォルト: true
 *         bool     fileDialog.CheckPathExists   存在しないpathの場合、警告表示するか    / デフォルト: true
 *         bool     fileDialog.CheckFileExists   存在しないファイル名の場合、警告表示するか / デフォルト: false
 *         bool     fileDialog.RestoreDirectory  フォルダ変更した場合、ダイアログを閉じる時、元のフォルダに戻すか / デフォルト: false
 *         bool     fileDialog.ShowHelp          ヘルプボタンを表示するか / デフォルト: false
 *            ↑
 *@subject ◆OpenFileDialog : FileDialog : CommonDialog
 *         string   openFileDialog.SafeFileName { get; }  選択したファイル名 (拡張子を含む、pathは含まない)
 *         string[] openFileDialog.SafeFileNames { get; } 選択した全てのファイル名の配列 (拡張子を含む、pathは含まない)
 *         bool     openFileDialog.Multiselect            複数選択を可能にするか / デフォルト: false
 *         bool     openFileDialog.ReadOnlyChecked        [読み取り専用ファイルとして開く]にチェックするか / デフォルト: false
 *         bool     openFileDialog.ShowReadOnly           [読み取り専用ファイルとして開く] チェックボックスを表示するか / false
 *         Stream   openFileDialog.OpenFile();            ファイルを読取専用で開くStream
 *         
 *@see ImageOpenFileDialogSample.jpg
 *@see 
 *@author shika
 *@date 2022-08-10
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT10_CommonDialog
{
    class MainOpenFileDialogSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormOpenFileDialogSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormOpenFileDialogSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormOpenFileDialogSample : Form
    {
        private Button button;

        public FormOpenFileDialogSample()
        {
            this.Text = "FormOpenFileDialogSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;
            this.BackgroundImageLayout = ImageLayout.Zoom;

            button = new Button()
            {
                Text = "Open",
                Location = new Point(20, 20),
                UseVisualStyleBackColor = true,
            };
            button.Click += new EventHandler(button_Click);
            //this.Load += new EventHandler(button_Click);

            this.Controls.AddRange(new Control[]
            {
                button,
            });
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Multiselect = false,
                InitialDirectory = Path.GetFullPath(
                    "../../../../SelfAspNet/SelfAspNet/Image"),
                Filter = "画像ファイル|*.bmp; *.jpg; *.gif; *.png;|全てのファイル|*.*;",
            };

            DialogResult result = dialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                Stream stream = dialog.OpenFile();
                this.BackgroundImage = new Bitmap(stream);
                //this.BackgroundImage = Image.FromFile(dialog.FileName);

                this.Text = dialog.SafeFileName;
            }
        }//button_Click()
    }//class
}
