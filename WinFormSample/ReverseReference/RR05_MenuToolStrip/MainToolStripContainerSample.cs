/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR05_MenuToolStrip
 *@class MainToolStripContainerSample.cs
 *@class FormToolStripContainerSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menu-strips/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm12_MenuToolStrip.txt〕
 *           
 *@content RR[88] ToolStripContainer / p167
 *         ToolStrip, MenuStrip, StatusStripを載せて、他の位置に移動するコンテナ
 *         toolStripItem.GripStyle = true;  Grip移動(=つかんで移動)のための十字矢印が出る。 
 *
 *@subject ◆ContainerControl :  ScrollableControl, IContainerControl
 *@subject ◆ToolStripContainer : ContainerControl
 *         ToolStripContainer    new ToolStripContainer()
 *         
 *         bool  toolStripContainer.TopToolStripPanelVisible    上部 Panelを表示するか / デフォルト true,
 *         bool  toolStripContainer.BottomToolStripPanelVisible 下部 Panelを表示するか / デフォルト true,
 *         bool  toolStripContainer.LeftToolStripPanelVisible   左端 Panelを表示するか / デフォルト true,
 *         bool  toolStripContainer.RightToolStripPanelVisible  右端 Panelを表示するか / デフォルト true,
 *
 *         ToolStripContentPanel  toolStripContainer.ContentPanel    中央 Panel / getのみ
 *                                                                   これに Add()すると Grip移動は不可
 *         ToolStripPanel    toolStripContainer.TopToolStripPanel    getのみ / Controls.Add()は可
 *         ToolStripPanel    toolStripContainer.BottmToolStripPanel  getのみ / Controls.Add()は可
 *         ToolStripPanel    toolStripContainer.LeftToolStripPanel   getのみ / Controls.Add()は可
 *         ToolStripPanel    toolStripContainer.RightToolStripPanel  getのみ / Controls.Add()は可
 *         
 *         ControlCollection toolStripContainer.CreateControlsInstance();  Controlsが利用不可なので ControlCollenctionの取得に利用
 *         × ControlCollection  control.Controls          利用不可
 *         × ContextMenuStrip   control.ContextMenuStrip  利用不可
 *
 *@NOTE【】ToolStripSeparator
 *         var separator = new ToolStripSeparator();
 *         をして、AddRange()内に separatorを載せると、
 *         最後の区切り線しか表示されない。
 *         
 *         => 毎回 new ToolStripSeparator()を AddRange()内ですると解決
 *            
 *@subject Deployment 配置
 *  Form / Panel
 *   └ control.Controls.Add(Control)
 *       └ ToolStripContainer
 *           └ toolStripContainer.TopToolStripPanel.Controls.Add(ToolStripItem)  初期配置位置のPanel
 *               └ ToolStrip                                  GripStyle = true; が必要
 *                   └ toolStrip.Items.Add(ToolStripItem)    〔× ToolBarのように Buttonsではない〕
 *                       └ ToolBarButton                      各Button
 *                               
 *@see FormToolStripContainerSample.jpg
 *@copyTo WinFormGUI / WinFormSample / ToolStripReference.txt
 *@author shika
 *@date 2022-07-27
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR05_MenuToolStrip
{
    class MainToolStripContainerSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormToolStripContainerSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormToolStripContainerSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormToolStripContainerSample : Form
    {
        private ToolStripContainer toolContainer;
        private ToolStrip tool;

        public FormToolStripContainerSample()
        {
            this.Text = "FormToolStripContainerSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(600, 600);
            this.AutoSize = true;

            //---- ImageList ----
            var imageList = new ImageList()
            {
                ImageSize = new Size(40, 40),
                TransparentColor = Color.Magenta,
            };
            imageList.Images.Add("document", new Bitmap("../../Image/Icon/DocumentIcon36px.png"));
            imageList.Images.Add("folder", new Bitmap("../../Image/Icon/folderIcon40px.jpg"));
            imageList.Images.Add("print", new Bitmap("../../Image/Icon/printIcon40px.jpg"));
            imageList.Images.Add("save", new Bitmap("../../Image/Icon/saveIcon40px.jpg"));
            imageList.Images.Add("saveNamed", new Bitmap("../../Image/Icon/saveNamedIcon40px.jpg"));
            imageList.Images.Add("saveAll", new Bitmap("../../Image/Icon/saveAllIcon40px.jpg"));
            imageList.Images.Add("cut", new Bitmap("../../Image/Icon/cutIcon40px.jpg"));
            imageList.Images.Add("copy", new Bitmap("../../Image/Icon/copyIcon40px.jpg"));
            imageList.Images.Add("paste", new Bitmap("../../Image/Icon/pasteIcon40px.jpg"));
            imageList.Images.Add("fileDelete", new Bitmap("../../Image/Icon/FileDeleteIcon36px.jpg"));
            imageList.Images.Add("help", SystemIcons.Question);

            //---- ToolStripButton ----
            var toolDocument = new ToolStripButton()
            {
                ToolTipText = "New File",
                ImageKey = "document",
            };

            var toolFolder = new ToolStripButton()
            {
                ToolTipText = "Folder Open",
                ImageKey = "folder",
            };

            var toolPrint = new ToolStripButton()
            {
                ToolTipText = "Print out",
                ImageKey = "print",
            };

            var toolSave = new ToolStripButton()
            {
                ToolTipText = "Save File",
                ImageKey = "save",
            };

            var toolSaveNamed = new ToolStripButton()
            {
                ToolTipText = "Save New Named File",
                ImageKey = "saveNamed",
            };

            var toolSaveAll = new ToolStripButton()
            {
                ToolTipText = "Save All Files",
                ImageKey = "saveAll",
            };

            var toolCut = new ToolStripButton()
            {
                ToolTipText = "Cut",
                ImageKey = "cut",
            };

            var toolCopy = new ToolStripButton()
            {
                ToolTipText = "Copy",
                ImageKey = "copy",
            };

            var toolPaste = new ToolStripButton()
            {
                ToolTipText = "Paste",
                ImageKey = "paste",
            };

            var toolDelete = new ToolStripButton()
            {
                ToolTipText = "Delete this File",
                ImageKey = "fileDelete",
            };

            var toolHelp = new ToolStripButton()
            {
                ToolTipText = "Help",
                ImageKey = "help",
            };

            //---- Event ----
            //toolXxxx.Click += new EventHandler(toolXxxx_Click);
            //   :  〔本来は各Buttonクリック時の Event, EventHandler を記述。ここでは省略〕

            //---- ToolStrip / ToolStripContainer ----
            tool = new ToolStrip()
            {
                ShowItemToolTips = true,
                ImageList = imageList,
                ImageScalingSize = new Size(40, 40),
                Dock = DockStyle.None,
                GripStyle = ToolStripGripStyle.Visible,                
            }; 

            tool.Items.AddRange(new ToolStripItem[]
            {
                toolDocument, toolFolder, toolPrint,
                new ToolStripSeparator(),
                toolSave, toolSaveNamed, toolSaveAll,
                new ToolStripSeparator(),
                toolCut, toolCopy, toolPaste,
                new ToolStripSeparator(),
                toolDelete, toolHelp,
            });

            toolContainer = new ToolStripContainer()
            {
                TopToolStripPanelVisible = true,
                BottomToolStripPanelVisible = true,
                LeftToolStripPanelVisible = true,
                RightToolStripPanelVisible = true,
                Dock = DockStyle.Fill,
            };
            toolContainer.TopToolStripPanel.Controls.Add(tool);

            //---- Form ----
            this.Controls.AddRange(new Control[]
            {
                toolContainer,
            });
        }//constructor
    }//class
}
