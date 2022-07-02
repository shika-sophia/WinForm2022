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
 *@subject ◆ComboBox System.Windows.Forms.
 *         ComboBox new ComboBox()
 *         ComboBoxStyle     combo.DropDownStyle
 *             enum ComboBoxStyle
 *             {
 *                 Simple = 0,       // シンプル
 *                 DropDown = 1,     // ドロップダウン
 *                 DropDownList = 2  // ドロップダウンリスト
 *             }
 *             
 *         ComboBox.ObjectCollection  combo.Items
 *         int               combo.Items.Add(object item)  項目を追加
 *         void              combo.Items.AddRange(object[] items)
 *         object combo.SelectedItem   //選択された項目
 *         int    combo.SelectedIndex  // 最初に選択される項目
 *         string combo.SelctedText    //ComboBoxStyle.DropDownList時は ""
 *         
 *         ＊イベント
 *         EventHandler combo.SelectedChanged
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
        [STAThread]
        static void Main()
        //public void Main()
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
