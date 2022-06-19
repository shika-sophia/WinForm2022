/** 
 *@title WinFormGUI / WinFormSample / 
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/ =>〔~/Reference/Article_KaiteiNet〕
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@content 
 *@subject
 * 
 *@author shika
 *@date 
 */
using System;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT03_Form
{
    class MainFormConstract
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.Run(new FormConstract());
        }//Main()
    }//class
}
