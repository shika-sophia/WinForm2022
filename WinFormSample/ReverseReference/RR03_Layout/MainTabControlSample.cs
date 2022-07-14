/** 
 *@title WinFormGUI / WinFormSample /
 *@class 
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/dialogs/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm09_Dialog1.txt〕
 *           
 *@content RR[77] TabControl / p149
 *         複数タブにコントロールを乗せるコンテナ
 *         
 *@subject ◆TabControl : Control
 *         TabControl        new TabControl()
 *         TabPage : Panel   new TabPage(string text)
 *         
 *         int            tab.SelectedIndex
 *         TabPage        tab.SelectedTab
 *         TabControl.TabPageCollection  tab.TabPages
 *         void           tab.TabPages.Add(TabPage) / AddRange(TabPage[])
 *                                        //TabCotrolに TabPageを追加
 *         TabPage        tab.TabPages[i]
 *         void           tab.TabPages[i].Controls.Add(Control) / AddRange(Control[])
 *                                        //各TabPageに Controlを追加
 *                                        
 *         bool           tab.HotTrack   タブにマウスポイント時、外観を変化させるか
 *         bool           tab.Multiline  複数行表示するか / デフォルト: false,
 *         TabSizeMode    tab.SizeMode
 *           └ enum TabSizeMode
 *             {
 *                 Normal = 0,      //タブ内のテキストに合わせて調整
 *                 FillToRight = 1, //全タブ幅はTabControlの幅に自動調整 (複数行ある場合のみ)
 *                 Fixed = 2        //タブ幅すべて同じに固定
 *             }
 *             
 *         TabAlignment   tab.Alignment
 *           └ enum TabAlignment
 *             {
 *                 Top = 0,
 *                 Bottom = 1,
 *                 Left = 2,
 *                 Right = 3
 *             }
 *             
 *         TabAppearance  tab.Appearance
 *           └ enum TabAppearance
 *             {
 *                 Normal = 0,
 *                 Buttons = 1,
 *                 FlatButtons = 2
 *             }
 *             
 *@subject コンテナ階層
 *         RadioButton -> RadioButton[] -> TabPage -> TabControl -> Form
 *         のように乗せている。
 *         
 *@subject 情報の取得
 *         tab.TabPages -> TabPage -> TabPage.Controls (= RadioButton)
 *         二重の foreach構文で、各TabPage, 各RadioButtonを取得し、
 *         radio.Checkedを判定していく。
 *
 *@NOTE【註】Size, Location
 *      RadioButtonの座標を記入しないと
 *      最初の項目だけ表示されて、他は後層に隠れてしまう。
 *      TabControl, Button, ListBoxの Size, Locationを指定しないと、
 *      TabControlの後層に隠れて表示されないので注意。
 *      
 *@see ~/WinFormSample/KaiteiNet/KT06_Control/MainRadioButtonSample.cs
 *@see FormTabControlSample.jpg
 *@author shika
 *@date 2022-07-14
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR03_Layout
{
    class MainTabControlSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormTabControlSample());
        }//Main()
    }//class

    class FormTabControlSample : Form
    {
        private TabControl tab;
        private readonly string[] pageAry = new string[] 
        {
            "Main Menu","Set Menu","Sweets",
        };
        private RadioButton[] menuAry;
        private RadioButton[] setAry;
        private RadioButton[] sweetAry;
        private Button button;
        private ListBox list;
        
        public FormTabControlSample()
        {
            this.Text = "FormTabControlSample";

            tab = new TabControl()
            {
                HotTrack = true,
                Multiline = true,
                SizeMode = TabSizeMode.FillToRight,
                Alignment = TabAlignment.Top,
                Appearance = TabAppearance.Normal,
            };

            foreach (string page in pageAry)
            {
                tab.TabPages.Add(new TabPage(page));
            }//foreach

            menuAry = new RadioButton[]
            {
                new RadioButton(){ Text = "日替わり", Left = 20, Top = 10 },
                new RadioButton(){ Text = "ハンバーグ", Left = 20, Top = 30 },
                new RadioButton(){ Text = "鶏の唐揚げ", Left = 20, Top = 50 },
            };

            setAry = new RadioButton[]
            {
                new RadioButton(){ Text = "ライス + 味噌汁", Left = 20, Top = 10 },
                new RadioButton(){ Text = "パン + スープ", Left = 20, Top = 30 },
                new RadioButton(){ Text = "ビール", Left = 20, Top = 50 },
            };

            sweetAry = new RadioButton[]
            {
                new RadioButton(){ Text = "紅茶", Left = 20, Top = 10 },
                new RadioButton(){ Text = "コーヒー", Left = 20, Top = 30 },
                new RadioButton(){ Text = "ケーキ", Left = 20, Top = 50 },
            };

            tab.TabPages[0].Controls.AddRange(menuAry);
            tab.TabPages[1].Controls.AddRange(setAry);
            tab.TabPages[2].Controls.AddRange(sweetAry);

            button = new Button()
            {
                Text = "決定", 
                Location = new Point(10, 110),
                AutoSize = true,
            };
            button.Click += new EventHandler(button_Click);

            list = new ListBox()
            {
                Location = new Point(10, 160),
            };
            

            this.Controls.AddRange(new Control[]
            {
                tab, button, list,
            });
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            list.Items.Clear();
            list.Items.Add("＊ご注文の確認＊");
            list.Items.Add("\n");
            foreach (TabPage tagPage in tab.TabPages)
            {
                foreach(RadioButton radio in tagPage.Controls)
                {
                    if (radio.Checked)
                    {
                        list.Items.Add(radio.Text);
                    }
                }//foreach radio
            }//foreach tabPage
        }
    }//class
}
