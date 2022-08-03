/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT08_Resource
 *@class MainResourceIcon.cs
 *@class FormResourceIcon : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/resources/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm08_Resource.txt〕
 *           
 *@content KT08 Resource / Icon
 *         Icon   WinFormGUI.Properties.Resources.xxxx.ico
 *         
 *@subject フォームのアイコン設定
 *         Icon    form.Icon       フォームのアイコン
 *         bool    form.ShowIcon   フォームのアイコンを表示するか / デフォルト: true
 *         => 〔RR05_MenuToolStrip\MainNotifyIconSample.cs〕
 *         
 *@subject 「.exe」プロジェクト実行ファイルのアイコン設定
 *         VS[プロジェクト] -> [プロパティ] -> [アプリケーション]タブ
 *         -> アイコン欄 -> [...]クリック -> アイコン指定 -> [OK]
 *
 *@see ImageResourceIcon.jpg
 *@see MainResourceSample.cs
 *@see ~\WinFormSample\ReverseReference\RR05_MenuToolStrip\MainNotifyIconSample.cs
 *@author shika
 *@date 2022-08-03
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT08_Resource
{
    class MainResourceIcon
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormResourceIcon()");

            Application.EnableVisualStyles();
            Application.Run(new FormResourceIcon());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormResourceIcon : Form
    {
        public FormResourceIcon()
        {
            this.Text = "FormResourceIcon";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            Icon icon = WinFormGUI.Properties.Resources.triColorIcon48px;
            this.Icon = icon;

            //this.Controls.AddRange(new Control[]
            //{

            //});
        }//constructor
    }//class
}
