/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT06_Control
 *@class MainComboBoxSample.cs
 *@class FormComboBoxSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/controls/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm06_Control.txt〕
 *           
 *@content KT 6. Control / ComboBox ドロップダウンリスト
 *@subject ◆ComboBox : ListControlを継承  //System.Windows.Forms.
 *         ・TextBoxと ListBoxを融合したコントロール
 *         ComboBox new ComboBox()
 *         ComboBoxStyle     combo.DropDownStyle
 *             enum ComboBoxStyle
 *             {
 *                 Simple = 0,       // リストを常に表示。入力可
 *                 DropDown = 1,     // ドロップダウン。入力可
 *                 DropDownList = 2  // ドロップダウン。入力不可
 *             }
 *             
 *         ComboBox.ObjectCollection  combo.Items
 *         int    list.Items.Add(object)
 *         void   list.Items.AddRange(object[]) 
 *         void   list.Items.Remove(object)
 *         void   list.Items.RemoveAt(int)
 *         void   list.Items.Clear()
 *         void   list.Items.Insert(int, object)
 *         int    list.Items.Count
 *         bool   list.GetSelected(int)
 *         void   list.SetSelected(int, bool)
 *         
 *         int    list.SelectedIndex   非選択: -1
 *         object list.SelectedItem    非選択: null
 *         string combo.SelctedText    //入力された文字列
 *                                     //ComboBoxStyle.DropDownList時は ""
 *                                     
 *         SelectionMode list.SelectionMode
 *           └ enum SelectionMode
 *             {  None = 0,          //選択不可
 *                One = 1,           //１つだけ選択可。デフォルト値
 *                MultiSimple = 2,   //複数可。マウスクリック、スペース
 *                MultiExtended = 3, //複数可。[Ctrl] or [Shift] + 矢印
 *             }
 *             
 *         ＊イベント
 *         EventHandler combo.SelectedIndexChanged
 *         
 *@see FormComboBoxSample.jpg
 *@author shika
 *@date 2022-07-02
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT06_Control
{
    class MainComboBoxSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormComboBoxSample());
        }//Main()
    }//class

    class FormComboBoxSample : Form
    {
        private Label label;
        private ComboBox combo;
        
        public FormComboBoxSample()
        {
            this.Text = "FormComboBoxSample";
            
            label = new Label()
            {
                Location = new Point(10, 100),
                AutoSize = true,
            };
            this.Controls.Add(label);

            combo = new ComboBox()
            {
                Location = new Point(10, 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            combo.Items.Add("青巻紙");
            combo.Items.Add("赤巻紙");
            combo.Items.Add("黄巻紙");
            combo.SelectedIndex = 0;

            combo.SelectedIndexChanged +=
                new EventHandler(combo_SelectIndexChanged);

            this.Controls.Add(combo);
        }//constructor

        private void combo_SelectIndexChanged(object sender, EventArgs e)
        {
            label.Text = $"selcted {combo.SelectedIndex}: {combo.SelectedItem.ToString()} ";
        }
    }//class
}
