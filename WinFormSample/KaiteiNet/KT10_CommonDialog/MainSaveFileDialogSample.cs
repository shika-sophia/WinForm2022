/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT10_CommonDialog
 *@class MainSaveFileDialogSample.cs
 *@class   └ new FormSaveFileDialogSample() : Form
 *#class       └ new SaveFileDialog() : FileDialog
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/common-dialogs/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm10_CommonDialog.txt〕
 *           
 *@content KT10 CommonDialog / SaveFileDialog | RR[275] p489
 *         ・[名前を付けて保存] ダイアログ
 *         ・SampleCode 
 *             Form Load時:  既存のファイルを参照するか MessageBox
 *                           [Yes] -> new OpenFileDialog()
 *             Form Close時  ファイルを保存するか MessageBox
 *                           [Yes] -> new SaveFileDialog()
 *                           
 *@subject abstract FileDialog : abstract CommonDialog
 *                     =>〔MainOpenFileDialogSample.cs〕
 *             ↑
 *@subject ◆SaveFileDialog : FileDialog
 *         SaveFileDialog     new SaveFileDialog()
 *         bool    saveFileDialog.CreatePrompt    
 *                   true:  存在しないファイルを指定したとき警告
 *                   false: 存在しない場合、新しいファイルを自動作成 (デフォルト)
 *         bool    saveFileDialog.OverwritePrompt 
 *                   true:  すでに存在するファイルを指定時、事前に上書きするかを警告 (デフォルト)
 *                   false: 警告せずに上書き
 *         Stream  saveFileDialog.OpenFile();
 *
 *@subject form.OnLoad(EventArgs e)                   Formロード時 イベント
 *@subject form.OnFormClosing(FormClosingEventArgs e) Foemクローズ時 イベント
 *         FormClosingEventArgs : CancelEventArgs
 *         ComponentModel.CancelEventArgs e
 *         bool     e.Cancel
 *
 *@see ImageSaveFileDialogSample.jpg
 *@see MainOpenFileDialogSample.cs
 *@author shika
 *@date 2022-08-10
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT10_CommonDialog
{
    class MainSaveFileDialogSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormSaveFileDialogSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormSaveFileDialogSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormSaveFileDialogSample : Form
    {
        private TextBox textBox;

        public FormSaveFileDialogSample()
        {
            this.Text = "FormSaveFileDialogSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            textBox = new TextBox()
            {
                Multiline = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Fill,
            };

            this.Controls.AddRange(new Control[]
            {
                textBox,
            });
        }//constructor

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DialogResult result = MessageBox.Show(
                "Open presented file?",
                "FileOpen",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if(result != DialogResult.Yes) { return; }

            var openDialog = new OpenFileDialog()
            {
                InitialDirectory = Path.GetFullPath("../../WinFormSample"),
                Filter = "Text File|*.txt;|すべてのファイル|*.*;",
                Multiselect = false,
            };

            DialogResult openResult = openDialog.ShowDialog();
            if(openResult == DialogResult.OK)
            {
                using (var reader = new StreamReader(openDialog.OpenFile()))
                {
                    textBox.Text = reader.ReadToEnd();
                    reader.Close();
                }//using

                //form.Text: Formタイトル経由で値を受け渡し
                this.Text = openDialog.SafeFileName;
            }
        }//OnLoad()

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            //---- MessageBox ----
            DialogResult mesSaveResult = MessageBox.Show(
                "Save file?",
                "Save Confirm",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if(mesSaveResult == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            else if(mesSaveResult != DialogResult.Yes)
            {
                return;
            }

            //---- SaveFileDialog ----
            var saveDialog = new SaveFileDialog()
            { 
                FileName = this.Text,   //form.Text: Formタイトル経由で値を受け渡し
                OverwritePrompt = true,
                Filter = "Text File|*.txt;|すべてのファイル|*.*;",
            };
            DialogResult saveResult = saveDialog.ShowDialog();
            if(saveResult != DialogResult.OK) { return; }

            using (var writer = new StreamWriter(saveDialog.OpenFile()))
            {
                writer.Write(textBox.Text);
                writer.Close();
            }//using
        }//OnFormClosing()
    }//class
}
