/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT12_MenuToolStrip
 *@class MainMenuIconSample.cs
 *@class FormMenuIconSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menu-strips/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm12_MenuToolStrip.txt〕
 *           
 *@content MenuStrip
 *         =>〔MainMenuStripSample.cs〕
 *         
 *@subject ImageList 
 *         =>〔~/ WinFormSample / ReverseReference / RR04_Control / MainImageListSample.cs〕
 *         =>〔../ KT11_MenuOld / MainToolBarSample.cs〕
 *         
 *         int     imageList.Images.AddStrip(Image) 短冊状のイメージから複数のアイコンを自動生成
 *                                                  ※ ImageSize は アイコンSizeの整数倍でないと例外となる〔下記〕
 *         Image   imageList.Images[i]              短冊状のイメージから個々のアイコンを indexで指定
 *         Color   imageList.TransparentColor       透過色を指定 (この色だと透明になる)
 *
 *@subject 相対パス "../../Image/Icon/DocumentIcon36px.png"
 *         「~/bin/Debug/WinFormGUI.exe」からの相対パス
 *         
 *@NOTE【Exception】imageList.Images.AddStrip(Image)
 *      System.ArgumentException: 
 *      イメージ ストリップの幅は ImageSize.Width の正の倍数でなければなりません。
 *      パラメーター名:value
 *      場所 System.Windows.Forms.ImageList.ImageCollection.AddStrip(Image value)
 *      
 *      元画像 36×36のとき
 *      => imageList.ImageSize = new Size(36, 36);を追加すると解決
 *      => imageList.ImageSize = new Size(24, 24),だと整数倍ではないので例外のまま。
 *      => AddStrip() -> Add()にすると解決
 *      
 *      AddStrip()は整数倍のサイズに限定されてしまう。
 *      サイズ変更したい場合は imageList.Images.Add(Image)を利用すべき
 *      
 *      Size  imageList.ImageSize
 *      Size  menuStrip.ImageScalingSize が競合すると MenuStripの値を優先
 *      
 *@see FormMenuIconSample.jpg
 *@author shika
 *@date 2022-07-24
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT12_MenuToolStrip
{
    class MainMenuIconSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormMenuIconSample());
        }//Main()
    }//class

    class FormMenuIconSample : Form
    {
        private MenuStrip menu;

        public FormMenuIconSample()
        {
            this.Text = "FormMenuIconSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            //---- Icon ----
            var imageList = new ImageList()
            {
                ImageSize = new Size(24, 24),
                TransparentColor = Color.Magenta,
            };
            imageList.Images.Add(
                new Bitmap("../../Image/Icon/DocumentIcon36px.png"));
            imageList.Images.Add(
                new Bitmap("../../Image/Icon/FileDeleteIcon36px.jpg"));

            //---- Menu Children ----
            var menuNew = new ToolStripMenuItem(
                "New File (&N)", imageList.Images[0]);
            menuNew.Click += new EventHandler(menuNew_Click);

            var menuExit = new ToolStripMenuItem(
                "Close (&X)", imageList.Images[1]);
            menuExit.Click += new EventHandler(menuExit_Click);

            //---- Menu Parent ----
            var menuFile = new ToolStripMenuItem("File (&F)");
            menuFile.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuNew, new ToolStripSeparator(), menuExit,
            });

            //---- Menu ----
            menu = new MenuStrip()
            {
                //ImageScalingSize = new Size(36, 36),
            };
            menu.Items.Add(menuFile);

            //---- Form ----
            this.Controls.Add(menu);
            this.MainMenuStrip = menu;
        }//constructor

        private void menuNew_Click(object sender, EventArgs e)
        {
            new FormMenuIconSample().Show();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }//class
}
