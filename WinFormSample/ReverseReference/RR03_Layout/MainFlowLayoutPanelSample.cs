/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR03_Layout
 *@class MainFlowLayoutPanelSample.cs
 *@class FormFlowLayoutPanelSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[110] FlowLayoutPanel / p206
 *         ・複数コントロールを直線状に配置する Panel
 *         ・Dock = DockStyle.Fill, AutoSize = trueにすると、
 *           Form表示幅によって、自動的に折り返し、複数行に配置する
 *         
 *@subject ◆FlowLayoutPanel : Panel, IExtenderProvider
 *         FlowLayoutPanel new FlowLayoutPanel()
 *         FlowDirection flow.FlowDirection
 *           └ enum FlowDirection
 *             {  LeftToRight = 0,
 *                TopDown = 1,
 *                RightToLeft = 2,
 *                BottomUp = 3
 *             }
 *         bool flow.WrapContents  折り返しするか / デフォルト: true
 *         void flow.SetFlowBreak(Control, bool)  指定したコントロールを trueにするとFlow表示の中断
 *         bool flow.GetFlowBreak(Control)        指定したコントロールが Flow中断しているか
 *
 *@see FormFlowLayoutPanelSample.jpg
 *@author shika
 *@date 2022-07-13
 */
using System;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR03_Layout
{
    class MainFlowLayoutPanelSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormFlowLayoutPanelSample());
        }//Main()
    }//class

    class FormFlowLayoutPanelSample : Form
    {
        private FlowLayoutPanel flow;
        private Button[] buttonAry;

        public FormFlowLayoutPanelSample()
        {
            this.Text = "FormFlowLayoutPanelSample";

            flow = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            buttonAry = new Button[5];
            for(int i = 0; i < buttonAry.Length; i++)
            {
                buttonAry[i] = new Button()
                {
                    Text = $"Button{i}",
                    AutoSize = true,
                };
            }//for

            flow.Controls.AddRange(buttonAry);
            this.Controls.Add(flow);
        }//constructor
    }//class
}
