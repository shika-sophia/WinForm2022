/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT08_Resource
 *@class MainTextResorceSample.cs
 *@class   └ new FormTextResorceSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/resources/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm08_Resource.txt〕
 *           
 *@content KT08 Resource /
 *@subject stringを [Resource.resx]に登録
 *         [java] Locale / [C#] Culture 設定に利用する
 *         
 *         => ResourceManagerクラス 〔後述〕
 *
 *@see ImageTextResorceSample.jpg
 *@see 
 *@author shika
 *@date 2022-08-04
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT08_Resource
{
    class MainTextResourceSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("MainTextResourceSample BEGIN");

            string str1 = WinFormGUI.Properties.Resources.TextSample1;
            string str2 = WinFormGUI.Properties.Resources.TextSample2;
            MessageBox.Show(str1, str2);

            Console.WriteLine("END");
        }//Main()
    }//class
}
