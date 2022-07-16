/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainListViewSubItemSample.cs
 *@class FormListViewSubItemSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/controls/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm06_Control.txt〕
 *           
 *@content RR[91] ListView SubItem / p175
 *         ListViewの SubItemを利用したサンプルコード
 *         
 *@subject ◆ListView
 *         =>〔~/WinFormSample/KaiteiNet/KT06_Control/MainListViewSample.cs〕
 *
 *@see ~/WinFormSample/KaiteiNet/KT06_Control/MainListViewSample.cs
 *@author shika
 *@date 2022-07-16
 */

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainListViewSubItemSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormListViewSubItemSample());
        }//Main()
    }//class

    class FormListViewSubItemSample : Form
    {
        private Label label;
        private TextBox textBox;
        private Button button;
        private ListView list;

        public FormListViewSubItemSample()
        {
            this.Text = "FormListViewSubItemSample";
            this.AutoSize = true;

            label = new Label()
            {
                Text = "Please describe any Folder in TextBox, and push Button.",
                Location = new Point(10, 10),
                AutoSize = true,
            };

            textBox = new TextBox()
            {
                Location = new Point(10, 30),
                Width = 200,
            };

            button = new Button()
            {
                Text = "Show Files",
                Location = new Point(220, 30),
            };
            button.Click += new EventHandler(button_Click);

            list = new ListView()
            {
                Location = new Point(10, 60),
                Width = 300, 
                Height = 200,
                GridLines = true,
                View = View.Details,
            };
            list.Columns.Add("File Name", 120, HorizontalAlignment.Center);
            list.Columns.Add("File Size", 60, HorizontalAlignment.Center);
            list.Columns.Add("Last Update", 120, HorizontalAlignment.Center);

            this.Controls.AddRange(new Control[]
            {
                label, textBox, button, list,
            });
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo;
            FileInfo[] fileInfoAry;

            list.Items.Clear();

            if (!Directory.Exists(textBox.Text))
            {
                MessageBox.Show(
                    $"The folder of [{textBox.Text}] does not exist.",
                    "Notation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            dirInfo = new DirectoryInfo(textBox.Text);
            fileInfoAry = dirInfo.GetFiles();

            foreach(FileInfo fileInfo in fileInfoAry)
            {
                ListViewItem fileItem = new ListViewItem(fileInfo.Name);
                fileItem.SubItems.Add(fileInfo.Length.ToString());
                fileItem.SubItems.Add(fileInfo.LastWriteTimeUtc.ToShortDateString());

                list.Items.Add(fileItem);
            }//foreach

        }//button_Click()
    }//class
}
