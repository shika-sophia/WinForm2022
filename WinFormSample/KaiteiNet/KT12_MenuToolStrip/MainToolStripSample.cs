/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT12_MenuToolStrip
 *@class MainToolStripSample.cs
 *@class FormToolStripSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menu-strips/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm12_MenuToolStrip.txt〕
 *           
 *@content KT12 MenuToolStrip / ToolStrip
 *         MenuOld / ToolBarのこと。
 *         基底クラスの ToolStripと ToolBarとしての役割が同クラスなのは
 *         混同しやすく解りずらい
 *         
 *@subject ◆ToolStrip : ScrollableControl, IArrangedElement, IComponent, IDisposable
 *         =>〔MainMenuStripSample.cs〕
 *         
 *         ToolStripItemClickedEventHandler
 *                       toolStrip.ItemClicked   ToolStripクラスのクリック時イベント
 *                                               (全項目を一括で１つのイベントハンドラーで処理できる)
 *         ToolStripItemClickedEventArgs e
 *         ToolStripItem     e.ClickedItem
 *         
 *         EventHandler  toolStripItem.Click     ToolStripItemクラスのクリック時イベント
 *                                               (項目ごとにイベントハンドラーを追加する必要がある)
 *         ToolStripItem     (ToolStripItem) sender
 *
 *@subject ToolStripButton : ToolStripItem
 *         string   toolStripButton.Text        テキスト
 *         string   toolStripButton.ToolTipText ツールチップ
 *         int      toolStripButton.ImageIndex  イメージのインデックス
 *
 *@subject Deployment 配置
 *         Form / Panel
 *           └ control.Controls.Add(Control)
 *               └ ToolStrip
 *                  └ toolStrip.Items.Add(ToolStripItem)
 *                      └ ToolStripLabel 
 *                      └ ToolStripButton 
 *                      └ ToolStripDropDownButton
 *                      └ ToolStripSplitButton
 *                      └ ToolStripTextBox
 *                      └ ToolStripComboBox
 *                      └ ToolStripProgressBar
 *                      └ ToolStripSeparator
 *         
 *@see FormToolStripSample.jpg
 *@see KT11_MenuOld / MainToolBarSample.cs
 *@copyTo ~/ WinFormSample / ToolStripReference.txt
 *@author shika
 *@date 2022-07-26
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT12_MenuToolStrip
{
    class MainToolStripSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormToolStripSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormToolStripSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormToolStripSample : Form
    {
        private ToolStrip tool;
        
        public FormToolStripSample()
        {
            this.Text = "FormToolStripSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            var imageList = new ImageList()
            {
                ImageSize = new Size(36, 36),
                TransparentColor = Color.Magenta,
            };
            imageList.Images.Add(new Bitmap("../../Image/Icon/DocumentIcon36px.png"));
            imageList.Images.Add(new Bitmap("../../Image/Icon/FileDeleteIcon36px.jpg"));

            var toolNew = new ToolStripMenuItem()
            {
                ToolTipText = "New",
                ImageIndex = 0,
            };
            //toolNew.Click += new EventHandler(tool_Click);

            var toolExit = new ToolStripMenuItem()
            {
                ToolTipText = "Close",
                ImageIndex = 1,
            };
            //toolExit.Click += new EventHandler(tool_Click);

            tool = new ToolStrip()
            {
                ImageList = imageList,
                ImageScalingSize = new Size(24, 24),
                ShowItemToolTips = true,
            };
            tool.ItemClicked += 
                new ToolStripItemClickedEventHandler(tool_ItemClicked);
            
            tool.Items.AddRange(new ToolStripItem[]
            {
                toolNew, new ToolStripSeparator(), toolExit,
            });

            this.Controls.AddRange(new Control[]
            {
                tool,
            });
        }//constructor

        private void tool_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //private void tool_Click(object sender, EventArgs e)
        {
            switch(e.ClickedItem.ToolTipText)
            //switch (((ToolStripItem) sender).ToolTipText)
            {
                case "New":
                    new FormToolStripSample().Show();
                    break;
                case "Close":
                    this.Close();
                    break;
            }//switch
        }
    }//class
}
