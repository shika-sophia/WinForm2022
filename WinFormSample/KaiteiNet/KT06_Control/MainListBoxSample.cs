﻿/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT06_Control
 *@class MainListBoxSample.cs
 *@class FormListBoxSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/controls/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm06_Control.txt〕
 *           
 *@content KT 6. Control / ListBox
 *@subject ◆ListBoxクラス System.Windows.Forms
 *         ListBox new ListBox()
 *         ListBox.ObjectCollection
 *                list.Items
 *         int    list.Items.Add(object)
 *         void   list.Items.AddRange(object[]) 
 *         void   list.Items.Remove(object)
 *         void   list.Items.RemoveAt(int)
 *         void   list.Items.Clear()
 *         void   list.Items.Insert(int, object)
 *         int    list.Items.Count
 *         bool   list.GetSelected(int)
 *         void   list.SetSelected(int, bool)
 *         int    list.SelectedIndex   非選択: -1
 *         object list.SelectedItem    非選択: null
 *         SelectionMode list.SelectionMode
 *           └ enum SelectionMode
 *             {  None = 0,          //選択不可
 *                One = 1,           //１つだけ選択可。デフォルト値
 *                MultiSimple = 2,   //複数可。マウスクリック、スペース
 *                MultiExtended = 3, //複数可。[Ctrl] or [Shift] + 矢印
 *             }
 *         EventHandler
 *                list.SelectIndexChanged
 *
 *@see FormListBoxSample.jpg
 *@author shika
 *@date 2022-07-03
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT06_Control
{
    class MainListBoxSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormListBoxSample());
        }//Main()
    }//class

    class FormListBoxSample : Form
    {
        private Label label;
        private ListBox list;
        
        public FormListBoxSample()
        {
            this.Text = "FormListBoxSample";

            label = new Label()
            {
                Text = "No Selected",
                Location = new Point(10, 100),
                AutoSize = true,
            };
            this.Controls.Add(label);

            list = new ListBox()
            {
                Location = new Point(10, 10),
            };

            list.Items.AddRange(new string[]
            {
                "赤巻紙", "青巻紙", "黄巻紙"
            });
            
            list.SelectedIndexChanged += new EventHandler(list_SelectChanged);
            this.Controls.Add(list);
        }//constructor

        private void list_SelectChanged(object sender, EventArgs e)
        {
            label.Text = $"{list.SelectedIndex}: {list.SelectedItem.ToString()}";
        }
    }//class

}
