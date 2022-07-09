/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainChckedListBoxSample.cs
 *@class FormChckedListBoxSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[75] CheckedListBox / p146
 *@subject ◆ChekedListBox : ListBox -- System.Windws.Forms.
 *         CheckedListBox new CheckedListBox()
 *         CheckedListBox.ObjectCollection  checkListBox.Items
 *         CheckedListBox.CheckedCollection checkListBox.CheckedItems チェックされた項目
 *         object   checkListBox.SelectedItem  選択している項目〔ListBox〕
 *         void     checkListBox.SetItemChecked(int index, bool value)  チェック状態のデフォルト値を設定
 *         
 *@subject ListBox.ObjectCollection, 
 *         CheckListBox.ObjectCollction / CheckedCollectionの変換
 *         ・直接の互換性がない
 *         ・foreachで object Itemを取得し、Add()する
 *
 *@see FormCheckedListBoxSample.jpg
 *@author shika
 *@date 2022-07-10
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainChckedListBoxSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormCheckedListBoxSample());
        }//Main()
    }//class

    class FormCheckedListBoxSample : Form
    {
        private CheckedListBox check;
        private ListBox list;
        private Button button;
        private readonly string[] itemAry = new string[]
        {
            "テニス","バトミントン","陸上","水泳","野球","サッカー",
        };

        public FormCheckedListBoxSample()
        {
            this.Text = "FormCheckedListBoxSample";
            this.AutoSize = true;
            this.BackColor = Color.Gray;

            check = new CheckedListBox()
            {
                Location = new Point(10, 10),
            };
            check.Items.AddRange(itemAry);
            check.SetItemChecked(0, true);

            button = new Button()
            {
                Text = "Show Selected Item",
                Location = new Point(150, 50),
                BackColor = Color.White,
                AutoSize = true,
            };

            list = new ListBox()
            {
                Location = new Point(10, 100),
                BackColor = Color.White,
                AutoSize = true,
            };

            button.Click += new EventHandler(button_Click);

            this.Controls.AddRange(new Control[]
            {
                check, list, button,
            });
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            list.Items.Clear();
            foreach(object checkedItem in check.CheckedItems)
            {
                list.Items.Add(checkedItem);
            }            
        }//button_Click()
    }//class

}
