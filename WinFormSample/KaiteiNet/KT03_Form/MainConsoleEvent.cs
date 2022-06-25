/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet
 *@class MainConsoleEvent.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/events/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm05_Event.txt〕
 *           
 *@content KT 5. Event
 *         Console Application: echo (=入力された文字列をそのまま表示)
 *           
 *@subject ◆delegate, event, EventHandlerMethodの関係
 *         ＊デリゲート宣言 -- 型
 *         delegate void KeyHandler(string str);
 *         
 *         ＊イベント宣言 -- クラスのメンバー
 *         event InputEvent;
 *         
 *         ＊イベントにデリゲート・インスタンスを追加
 *         ＊デリゲート引数にイベントハンドラ・メソッドを代入
 *         InputEvent += new KeyHandler(InputEvent_1);
 *                       └ void InputEvent_1(string str)
 *                       
 *         ＊これにより、イベントを呼び出すとイベントハンドラメソッドとして動作する
 *
 *@NOTE【実行】このプロジェクトでコンソール出力するには
 *      WinFormGUIプロジェクトは「出力の種類」が[Windows Form]となっている。
 *      
 *      VS[プロジェクト] -> [プロパティ] -> [アプリケーション]タブ
 *      -> [出力の種類] -> [コンソールアプリケーション]に変更
 *      (終了後は元に戻しておく)
 *
 *@NOTE【結果】下記
 *
 *@see MainFormClick.cs
 *@author shika
 *@date 2022-06-25
 */
using System;

delegate void KeyHandler(string str);

namespace WinFormGUI.WinFormSample.KaiteiNet.KT05_Event
{
    class MainConsoleEvent
    {
        private event KeyHandler InputEvent;

        //static void Main()
        public void Main()
        {
            var here = new MainConsoleEvent();
            here.InputEvent += new KeyHandler(here.InputEvent_1);

            while (true)
            {
                Console.Write("Please any input.[-99: END]\n>");
                string input = Console.ReadLine();

                if(input.Contains("-99") || input.Contains("ー９９"))
                {
                    Console.WriteLine("input finished.");
                    break;
                }

                here.InputEvent(input);
            }//while
        }//Main()

        private void InputEvent_1(string str)
        {
            Console.WriteLine($"input: {str}");
        }
    }//class
}

/*
//==== Result ====
Please any input.[-99: END]
>jj
input: jj
Please any input.[-99: END]
>aa
input: aa
Please any input.[-99: END]
>FRINGE
input: FRINGE
Please any input.[-99: END]
>-99
input finished.
 */