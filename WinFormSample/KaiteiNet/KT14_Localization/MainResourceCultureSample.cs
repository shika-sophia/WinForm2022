/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT14_Localization
 *@class MainResourceCultureSample.cs
 *@class   └ new FormResourceCultureSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/localization/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm14_Localization.txt〕
 *           
 *@content KT14 Localization | [C#] Culture / [Java] Locale
 *         ・WinFormGUI/Properties/Resources.resxによる カルチャー切替
 *         
 *@subject Resourceファイル
 *         ・「Resources.resx」      デフォルトのリソースファイル
 *         ・「Resources.ja-JP.resx」日本語リソースファイル 
 *         ・「Resources.en-US.resx」英語リソースファイル
 *         
 *         ・「title」「message」を keyとして、各言語で valueを定義
 *         ・デフォルト「Resources.resx」にも「title」「message」が必要。
 *         ・Thread.CurrentThread.CurrentUICulture の CultureInfoオブジェクトの
 *          「Resources.xx-XX.resx」を自動で読込
 *         ・「Resources.xx-XX.resx」内に keyが存在しない場合は「Resources.resx」を読込
 *         ・Resource.ja-JP.resx，Resource.ja.resx，Resource.resx から構成されている場合，
 *           優先順位は Resource.ja-JP.resx > Resource.ja.resx > Resource.resx 
 *         ・サテライト アセンブリ「*.resources.dll」
 *               ビルドすると「.exe」のある「~/bin/Debug」内に
 *              「/ja-JP」「/en-US」フォルダと
 *              「~/bin/Debug/ja-JP/WinFormGUI.resources.dll」
 *              「~/bin/Debug/en-US/WinFormGUI.resources.dll」を VSが自動生成
 *               カルチャー切替に必要なので、階層構成や内容編集せずに、
 *               配布の際には、実行ファイル「.exe」に これらも付加して配布する。
 *             
 *@see ImageResourceCultureSample.jpg
 *@see MainCultureSample.cs
 *@see WinFormGUI/Properties/Resources.ja-JP.resx
 *@see WinFormGUI/Properties/Resources.en-US.resx
 *@author shika
 *@date 2022-08-13
 */
using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WinFormGUI.Properties;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT14_Localization
{
    class MainResourceCultureSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormResourceCultureSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormResourceCultureSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormResourceCultureSample : Form
    {
        private Button buttonJp;
        private Button buttonEn;

        public FormResourceCultureSample()
        {
            this.Text = "FormResourceCultureSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            buttonJp = new Button()
            {
                Text = "日本語",
                Location = new Point(10, 10),
                AutoSize = true,
            };
            buttonJp.Click += new EventHandler(button_Click);

            buttonEn = new Button()
            {
                Text = "English",
                Location = new Point(100, 10),
                AutoSize = true,
            };
            buttonEn.Click += new EventHandler(button_Click);

            this.Controls.AddRange(new Control[]
            {
                buttonJp, buttonEn,
            });
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            string language = (sender as Button).Text;
            CultureInfo culture = null;

            switch (language)
            {
                case "日本語":
                    culture = new CultureInfo("ja-JP");
                    break;
                case "English":
                    culture = new CultureInfo("en-US");
                    break;
                default:
                    throw new ArgumentException("Invalid CultureInfo.Name");                    
            }//switch

            Thread.CurrentThread.CurrentUICulture = culture;
            MessageBox.Show(Resources.message, Resources.title);
        }
    }//class
}
