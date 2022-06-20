/** 
 *@title WinFormGUI / WinFormSample / 
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/ =>〔~/Reference/Article_KaiteiNet〕
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *
 *@content KT 3. Form
 */
#region ◆Formクラス System.Windows.Forms
/*
 *@subject ◆Formクラス 
 *         ＊コンストラクタ
 *         new Form()
 *              event formID_Load(object sender, EventArgs e) 初期設定
 *         new FormXxxx() -> class FormXxxx のコンストラクタで初期設定
 * 
 *         string form.text   //Formのタイトル
 *         Icon   form.Icon   //new Icon(string path)
 *         
 *         ＊表示 / 閉じる
 *         モーダル Modal: 表示したFormが開いている間は、他Formの操作不可
 *                        戻り値が必要なときに利用
 *         モーダレス Modaless: 上記の制約なし
 *         
 *         DiaglgResult form.ShowDialog()
 *         void         form.Show()
 *         void         form.Close()
 *         
 *              ※Formをダイアログ形式にする設定値
 *                  FormBorderStyle="FixedDialog"
 *                  MaximizeBox="false"
 *                  MinimizeBox="false"
 *                  
 *         ＊タイトルバー
 *         bool form.MaximizeBox タイトルバーの「□」アイコン(最大化)を表示するか
 *         bool form.MinimizeBox タイトルバーの「ー」アイコン(最大化)を表示するか
 *         bool form.ControlBox 「ー □ ×」を表示するか
 *                              falseにすると「×」も表示されなくなるので、
 *                              別途にFormを閉じるボタンなどを用意する。
 *         bool HelpButton      ヘルプボタン「？」
 *                              MaximizeBox, MinimizeBox を falseにしておく必要がある。
 *                              event HelpRequested で処理内容を記述
 *         ＊サイズ変更 / 境界線 
 *         FormBorderStyle form.FormBorderStyle 
 *                              Formのサイズ変更の可否、境界線のスタイル
 *                              FormBorderStyle.Xxxxx で指定
 *             enum FormBorderStyle
 *             { 
 *                None = 0,
 *                Fixed3D = ,
 *                FixedDialog = ,
 *                FixedSingle = ,
 *                FixedToolWindow ,
 *                Sizable = ,        //default
 *                SizableToolWindow =
 *             }
 *         
 *         ＊デバイスサイズ
 *         FormWindowState form.WindowState 
 *                              デバイスのモニターサイズを取得し、それに合わせて調整
 *                              FormWindowState.Xxxxで指定
 *                                 Nomal | Maximized | Minimized
 *         ＊Formサイズ
 *         Size form.Size        Form全体のサイズ new Size(int width, int height)
 *         int  Width            横幅
 *         int  Height           高さ
 *         Size form.ClientSize  タイトルバー以下の領域
 *                               Sizeより優先されるので、タイトルバーの表示が崩れる可能性あり。
 *                               
 *         ＊表示位置
 *         FormStartPosition form.StartPosition
 *             enum FormStartPosition
 *             {
 *                 Manual = 0,                //自己定義 form.Location
 *                 CenterScreen = 1,          //表示領域の中央
 *                 WindowsDefaultLocation = 2,//Windowsデフォルトの位置とサイズ
 *                 WindowsDefaultBounds = 3,  //Windowsデフォルトの位置と境界スタイル
 *                 CrnterParent = 4           //親Formの中央
 *             }
 *             
 *         ＊デフォルトボタン
 *         IButtonControl form.AccessButton 
 *                         //[Enter]key で クリック状態にするButtonコントロールを指定
 *         IButtonControl form.CancelButton
 *                         //[Esc]key で キャンセル状態にするButtonコントロールを指定
 *                         
 *         ＊透過効果
 *         double form.Opacity // 0-1 (0-100%)の不透明度を指定
 *                             // フェイドイン/フェイドアウト効果: Timerコンポーネントで、この値を操作
 *         string Opacity.ToString() 現在のOpacity値の文字列を取得
 *         
 *         ＊アクセス修飾子
 *         ??     (control).Modifier //private : (デフォルト値) 他Formから参照できない
 *                                   //internal: 同一アセンブリ内からの参照可
 *                                   
 *         ＊他Formの値参照
 *         (formID).(コントロール).(プロパティ)
 *         
 *         ＊ダイアログの結果参照
 *         (formID).DialogResult
 *         
 *         ＊色指定
 *         Color form.ForeColor 文字色
 *         Color from.BackColor 背景色
 *                 Color.Xxxx   色指定
 *                 Color.Transparent 透明(親要素と同じ)
 *                 Color.FormArgb(int red, int green, int blue)
 *                 SystmColors.Color  Control | Window | Desktop
 *                 
 *         ＊背景画像
 *         Image form.BackgroundImage //new Bitmap(string path) -> エラーが出る
 *                                    //bitmap.Dispose()
 *                                    //Image.FromFile(string path)
 *         ImageLayout form.BackgroundImageLayout
 *                        enum ImageLayout { None, Tile, Center, Stretch, Zoom }
 */

#endregion
/*
 *@author shika
 *@date 
 */
using System;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT03_Form
{
    class MainFormConstruct
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.Run(new FormConstruct());
        }//Main()
    }//class
}
