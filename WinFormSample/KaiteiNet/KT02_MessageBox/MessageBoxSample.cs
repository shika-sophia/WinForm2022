/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet /
 *       KT02_MessageBox / MessageBoxSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/message-boxes/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm02_MessageBox.txt〕
 *@content KT ２．MmessageBox 
 *@subject ◆MessageBoxクラス System.Windows.Forms
 *         ・複数の MessageBoxを定義すると同時には出ない。順に表示する。
 *         
 *         DialogResult MessageBox.Show(
 *             string text,                 //メッセージ
 *             [string caption],            //ウィンドウのタイトル
 *             [MessageBoxButtons buttons], //ボタンの内容
 *             [MessageBoxIcon icon]        //アイコン
 *         )
 *         
 *@subject 引数 MessageBoxButtons 〔System.Windows.Forms〕
 *         public enum MessageBoxButtons
 *         {
 *            OK = 0,         //[OK]ボタン (デフォルト値)
 *            OKCancel = 1,   //[OK][キャンセル]
 *            AbortRetryIgnore = 2,
 *                            //[中止][再試行][無視]
 *            YesNoCancel = 3,//[はい][いいえ][キャンセル]
 *            YesNo = 4,      //[はい][いいえ]
 *            RetryCancel = 5 //[再試行] [キャンセル] 
 *         }
 *
 *@subject 引数 MessageBoxIcon〔System.Windows.Forms〕
 *         public enum MessageBoxIcon
 *         {
 *             None = 0,    //なし
 *             Hand = 16,   //赤丸に X
 *             Stop = 16,
 *             Error = 16,
 *             Question = 32, //青丸に「？」
 *                            //[非推奨] どのメッセージにも該当 / ヘルプと混同
 *             Exclamation = 48, //黄△「！」
 *             Warning = 48, 
 *             Asterisk = 64,    //円に「i」
 *             Information = 64
 *          }
 *          
 *@subject 戻り値 DialogResult〔System.Windows.Forms〕
 *         public enum DialogResult
 *         {
 *            None = 0,    //戻り値 Nothing / Modalの実行が継続。
 *            OK = 1,      // [OK]
 *            Cancel = 2,  // [キャンセル]
 *            Abort = 3,   // [中止]
 *            Retry = 4,   // [再試行]
 *            Ignore = 5,  // [無視]
 *            Yes = 6,     // [はい]
 *            No = 7       // [いいえ]
 *         }
 *
 *@copyTo WinFormGUI / WinFormSample / FormReference.txt
 *@author shika
 *@date 2022-06-19
 */
using System;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT02_MessageBox
{
    class MessageBoxSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            MessageBox.Show("MessageBox: Start");
            MessageBox.Show("Yes or No ?", "Confirm",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
        }//Main()
    }//class
}
