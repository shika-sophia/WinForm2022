/** 
 *@title WinFormGUI / WinFormSample / MainSplitContainer.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@content RR108 SplitContainer
 *@subject SplitContainer コントロール
 *         =>〔FormGuiSplitContainer.cs〕
 *         [デザインView] -> ツールボックスから[SplitCotainer]をドラッグ
 *         FormGuiSplitContainer.Designer.cs内
 *         InitializeComponent()に Formコードを自動生成
 *         
 *@subject FormCodeSplitContainer.cs
 *         FormGuiSplitContainer.Designer.cs内
 *         InitializeComponent()の内容を自己定義
 *         
 *@subject ◆SplitCotainerクラス System.Windows.Forms
 *         new SplitCotainer()
 *         new SplitterPanel(SplitContainer owner)
 *         SplitterPanel sc.Panel1 左 | 上の分割パネル
 *         SplitterPanel sc.Panel2 右 | 下の分割パネル
 *         Orientation sc.Orientation 分割方向 
 *            値 enum Orientation.Horizontal | Virtical
 *         int sc.SplitterDistance 分割位置: 左|上からの距離 px単位
 *         
 *         ※コントロールの追加
 *         Form <- SplitContainer <- SplitterPanel <- Controlのように
 *         contrtol.Controls.Add(Control child)を行い、コントロールを追加する。
 *         
 *@author shika
 *@date 2022-06-23
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR03_Layout
{
    class MainSplitContainer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormCodeSplitContainer());
        }//Main()

    }//class

    class FormCodeSplitContainer : Form
    {
        SplitContainer sc = new SplitContainer();

        public FormCodeSplitContainer()
        {
            //---- Form ----
            this.Text = "FormCodeSplitContainer";
            this.Size = new Size(400, 300);

            //---- SplitContainer ----
            sc.Dock = DockStyle.Fill;
            sc.Orientation = Orientation.Vertical;
            sc.SplitterDistance = 50; //px
            sc.BorderStyle = BorderStyle.FixedSingle;
            
            SplitterPanel panel1 = new SplitterPanel(sc);
            Label label1 = new Label()
            {
                Text = "label1 on panel1",
                AutoSize = true
            };
            panel1.Controls.Add(label1);

            SplitterPanel panel2 = new SplitterPanel(sc);
            Label label2 = new Label()
            {
                Text = "label2 on panel2",
                AutoSize = true
            };
            panel2.Controls.Add(label2);

            sc.Panel1.Controls.Add(panel1);
            sc.Panel2.Controls.Add(panel2);
            this.Controls.Add(sc);
        }//constructor
    }//class
}
