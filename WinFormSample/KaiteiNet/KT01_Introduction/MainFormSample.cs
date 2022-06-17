/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet /
 *       KT01_Introduction / MainFormSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/getting-started/#item-01
 *           =>〔~/Reference/Article_KaiteiNet/WinForm01_Introduction.txt〕
 *           
 *@content KT 第１章 Introduction (原題: GettingStarted)
 *          C# Main()から Form(= ウィンドウ)を生成
 * 
 *@subject Main()
 *         [STAThread] Single Thread Application?
 *         SingleThreadであることを宣言する属性
 *         Windows.Controlsの各クラスを呼び出すにはこの宣言が必須?
 *         
 *         //System.InvalidOperationException:
 *         //呼び出しスレッドは、多数の UI コンポーネントが必要としているため、
 *         //STA である必要があります。
 *         
 *@subject using System.Windows.Forms;
 *@prepare【参照追加】 System.Windows.Forms
 *         [VS] -> [プロジェクト]配下-> [参照] 右クリック
 *         -> [参照の追加] -> 「System.Windows.Forms」をチェック -> [追加]
 *         
 *         ※ Windows Formプロジェクトはデフォルトで参照済。
 *            コンソールアプリケーションから Formを利用する場合に参照追加
 *            
 *@subject Formクラス System.Windows.Forms
 *         new Form()
 *         string form.Text       //ウィンドウのタイトル
 *         
 *@subject Applicationクラス System.Windows.Forms
 *         Application.Run(Form); //WindowsFormを実行
 * 
 *@see      ~/Reference/Article_KaiteiNet/WinForm01_Introduction.txt
 *@copyFrom ~/Reference/MainGui.cs
 *@author shika
 *@date 2022-06-18
 */
using System;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT01_Introduction
{
    class MainFormSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Form form = new Form();
            form.Text = "Hello World by WinForm";
            Application.Run(form);
        }//Main()

    }//class
}
