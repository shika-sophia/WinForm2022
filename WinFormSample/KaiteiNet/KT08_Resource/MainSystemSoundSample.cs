/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT08_Resource
 *@class MainSystemSoundSample.cs
 *@class   └ new FormSystemSoundSample : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/resources/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm08_Resource.txt〕
 *           
 *@content KT08 Resource 
 *@subject SystemSound / SystemSounds クラス -- System.Media
 *         (static) SystemSound  SystemSounds.Asterisk     情報
 *         (static) SystemSound  SystemSounds.Beep         一般の警告音
 *         (static) SystemSound  SystemSounds.Exclamation  警告
 *         (static) SystemSound  SystemSounds.Hand         エラー
 *         (static) SystemSound  SystemSounds.Question     問合せ
 *         
 *         (static) void SystemSound.Play()  サウンドを再生
 *         
 *@NOTE【】SystemSound 音が鳴らない...
 *      ・コマンドプロンプトでコンパイル・実行しても同様
 *          >csc Xxxx.cs  // csc: C Sharp Compile => Xxxx.exeを生成
 *          >Xxxx         //クラス名のみで実行
 *
 *@subject PCのシステムサウンド設定
 *         [PC] -> [コントロールパネル] -> [ハードウェアとサウンド]大項目 ->
 *         [サウンド]ダイアログ -> [サウンド]タブ -> 各「.wav」名と音を確認できる
 *         ->「.wav」があるなら [参照]から設定可能
 *         
 *@subject System.Console
 *         void Console.Beep()
 *         void Console.Beep(int toon, int length)
 *            Console.Beep(262, 400);  // ド
 *            Console.Beep(294, 400);  // レ
 *            Console.Beep(330, 400);  // ミ
 *            Console.Beep(349, 400);  // ファ
 *            Console.Beep(392, 400);  // ソ
 *            Console.Beep(440, 400);  // ラ
 *            Console.Beep(494, 400);  // シ
 *            Console.Beep(523, 400);  // ド
 *         
 *@NOTE【】Console.Beep() 
 *        こっちは音が鳴るが、「ブチブチ」と詰まったようなノイズが入る
 *
 *@see 
 *@author shika
 *@date 2022-08-04
 */
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT08_Resource
{
    class MainSystemSoundSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormSystemSoundSample()");
            
            Application.EnableVisualStyles();
            Application.Run(new FormSystemSoundSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormSystemSoundSample : Form
    {
        private Label[] labelAry;
        private Button[] buttonAry;
        private readonly string[] itemAry = new string[]
        {
            "Asterisk", "Beep", "Exclamation", "Error", "Question",
        };

        public FormSystemSoundSample()
        {
            this.Text = "FormSystemSoundSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            //---- Label[], Button[] ----
            labelAry = new Label[itemAry.Length];
            buttonAry = new Button[itemAry.Length];

            for(int i = 0; i < itemAry.Length; i++)
            {
                labelAry[i] = new Label()
                {
                    Text = itemAry[i],
                    Location = new Point(20, (i * 30 + 20)),
                    Width = 120,
                };

                buttonAry[i] = new Button()
                {
                    Text = "Play",
                    Location = new Point(140, (i * 30 + 20)),
                    TabIndex = i,
                    Width = 60,
                };
                buttonAry[i].Click += new EventHandler(button_Click);
            }

            this.Controls.AddRange(labelAry);
            this.Controls.AddRange(buttonAry);
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            int index = (sender as Button).TabIndex;
            
            switch (itemAry[index])
            {
                case "Asterisk":
                    SystemSounds.Asterisk.Play();
                    break;
                case "Beep":
                    SystemSounds.Beep.Play();
                    break;
                case "Exclamation":
                    SystemSounds.Exclamation.Play();
                    break;
                case "Error": 
                    SystemSounds.Hand.Play();
                    break;
                case "Question": 
                    SystemSounds.Question.Play();
                    break;
                default:
                    throw new InvalidOperationException();
            }//switch
        }//button_Click()
    }//class
}
