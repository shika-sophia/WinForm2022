/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT11_MenuOld
 *@class MainToolBarSample.cs
 *@class FormToolBarSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menus/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm11_MenuOld.txt〕
 *           
 *@content KT11 MenuOld / ToolBar
 *         アイコンボタンを並べて表示するControl
 *         表示位置はクライアント領域内 (TopBarではない)
 *         
 *@subject ◆ToolBar : Control
 *         ToolBar   new ToolBar()
 *         string    toolBar.Text          ツールバー用の文字列〔？用途不明〕
 *         bool      toolBar.Divider       区切線を表示するか
 *         ImageList control.ImageList     ToolBarButtonになるアイコンList
 *        (bool      control.ShowToolTips  マウスポイント時にヒントを表示するか)
 *         
 *         ToolBarAppearance        toolBar.Appearance
 *           └ enum ToolBarAppearance
 *             {
 *                 Normal = 0,  // 3Dボタン
 *                 Flat = 1     // 平面表示、マウスポイント時に 3D
 *             }
 *             
 *         ToolBarTextAlign        toolBar.TextAlign
 *           └ enum ToolBarTextAlign   各アイコンのText位置
 *             {
 *                 Underneath = 0,  //Textは、ToolBarButtonのイメージの下に配置
 *                 Right = 1        //Textは、ToolBarButtonのイメージの右に配置
 *             }
 *             
 *         ToolBarButtonCollection  toolBar.Buttons  
 *         int     toolBar.Buttons.Add(string text);
 *         int     toolBar.Buttons.Add(ToolBarButton button);
 *         void    toolBar.Buttons.AddRange(ToolBarButton[] buttons);
 *         void    toolBar.Buttons.Clear();  
 *                           :   他 IList, ICollection系 メソッド
 *                           
 *         ToolBarButtonClickEventHandler 
 *                 toolBar.ButtonClick     ToolBarButtonクリック時のイベント
 *                (ToolBarButtonクラスのイベントではないことに注意)
 *            ||
 *         delegate void ToolBarButtonClickEventHandler(
 *             object sender, ToolBarButtonClickEventArgs e);
 *             └ ToolBarButtonClickEventArgsクラス
 *               ToolBarButton   e.Button  // クリックされたToolBarButton
 *               
 *@subject ToolBarButton : Component
 *         ToolBarButton  new ToolBarButton()
 *         ToolBarButton  new ToolBarButton(string text)
 *         
 *         string  toolBarButton.Text        各ボタンの文字列 / デフォルト ""
 *         int     toolBarButton.ImageIndex  control.ImageListのアイコンを指定する index
 *                                           imageList.Images[i] の index
 *         string  toolBarButton.ImageKey    各ボタンのアイコンの名前
 *        (string  control.ToolTipText   マウスポイント時に表示するヒントの文字列)
 *        
 *         ToolBarButtonStyle  toolBarButton.Style
 *           └ enum ToolBarButtonStyle
 *             {
 *                 PushButton = 1,  // 標準の 3D ボタン
 *                 ToggleButton = 2,// クリックにより凸凹を交互に繰り返す
 *                 Separator = 3,    // 空白または線。 外観は、toolBar.Appearance値によって異なる
 *                 DropDownButton = 4 //クリック時に、Menu or 他Formを表示するドロップダウン
 *             }
 *        
 *@subject 相対パス "../../Image/DocumentIcon36px.png"
 *         「~/bin/Debug/WinFormGUI.exe」からの相対パス
 *         
 *@see FormToolBarSample.jpg
 *@author shika
 *@date 2022-07-23
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT11_MenuOld
{
    class MainToolBarSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormToolBarSample());
        }//Main()
    }//class

    class FormToolBarSample : Form
    {
        private ToolBar toolBar;
        private ToolBarButton toolButtonNew;
        private ToolBarButton toolButtonExit;

        public FormToolBarSample()
        {
            this.Text = "FormToolBarSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            var imageList = new ImageList()
            {
                ImageSize = new Size(36, 36),
                TransparentColor = Color.Magenta,
            };
            imageList.Images.Add(new Bitmap("../../Image/Icon/DocumentIcon36px.png"));
            imageList.Images.Add(new Bitmap("../../Image/Icon/GavageIcon36px.png"));

            toolButtonNew = new ToolBarButton()
            {
                ToolTipText = "New",
                ImageIndex = 0,
            };

            toolButtonExit = new ToolBarButton()
            {
                ToolTipText = "Exit",
                ImageIndex = 1,
            };

            toolBar = new ToolBar()
            {
                ShowToolTips = true,
                ImageList = imageList,
            };
            toolBar.ButtonClick += 
                new ToolBarButtonClickEventHandler(toolBar_ButtonClick);
            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
                toolButtonNew, toolButtonExit,
            });
            
            this.Controls.Add(toolBar);
        }//constructor

        private void toolBar_ButtonClick(
            object sender, ToolBarButtonClickEventArgs e)
        {
            switch (e.Button.ToolTipText)
            {
                case "New":
                    new FormToolBarSample().Show();
                    break;
                case "Exit":
                    this.Close();
                    break;
            }//switch
        }//toolBar_ButtonClick()
    }//class
}
