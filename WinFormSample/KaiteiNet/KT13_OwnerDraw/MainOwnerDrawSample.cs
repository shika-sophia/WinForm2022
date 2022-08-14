/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT13_OwnerDraw
 *@class MainOwnerDrawSample.cs
 *@class   └ new FormOwnerDrawSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/owner-draw/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm13_OwnerDraw.txt〕
 *           
 *@content KT13 OwnerDraw
 *         ・OwnerDraw: OSによる自動描画ではなく、各コントロールによる自己定義のカスタマイズ描画
 *         ・位置, サイズ, 色, 文字列などを自己定義で指定する必要がある
 *         
 *@subject ◆DrawMode  control.DrawModeプロパティを変更する
 *             └ enum DrawMode
 *               {
 *                   Normal = 0,            // OSによる自動描画 (デフォルト値)
 *                   OwnerDrawFixed = 1,    // 手動で描画
 *                   OwnerDrawVariable = 2, // 手動で異なるサイズに描画
 *               }
 *               
 *@subject OwnerDrawイベント
 *         DrawItemEventHandler   control.DrawItem   
 *           └ delegate void DrawItemEventHandler(object sender, DrawItemEventArgs e)
 *           
 *         ※ListBoxなどを登録するすると、各行の描画のたびにイベント発生
 *         (for文がなくてもこの挙動をする〔下記コード参照〕)
 *
 *         DrawItemEventArgs e
 *         int        e.Index      描画中の何番目の要素か
 *         Color      e.ForeColor  文字色
 *         Color      e.BackColor  背景色
 *         Rectangle  e.Bounds     位置,サイズを示す四角形
 *         Graphics   e.Graphics   描画用 Graphicsオブジェクト
 *         DrawItemState e.State   描画中の項目の状態
 *           └ enum DrawItemState  「|」ビット論理和で複数指定可
 *             {
 *                 None = 0,      //状態なし、解除
 *                 Selected = 1,  //選択されている
 *                 Grayed = 2,    //淡色表示 メニューのみ
 *                 Disabled = 4,  //無効
 *                 Checked = 8,   //チェックされている
 *                 Focus = 16,    //フォーカス
 *                 Default = 32,  //デフォルト値
 *                 HotLight = 64, //マウスポイント時の強調表示
 *                 Inactive = 128,//非アクティブ
 *                 NoAccelerator = 256,  //Keyboard Accelerator (=ショートカット, アクセスキー)なしで表示
 *                 NoFocusRect = 512,    //フォーカスのビジュアルキューなしで表示
 *                 ComboBoxEdit = 4096,  //ComboBoxの編集部分
 *              }
 *              
 *         void       e.DrawBackground()     既定の背景を描画。カスタム描画前に記述
 *         void       e.DrawFocusRectabgle() フォーカスを示す四角形を描画
 *
 *@subject 選択項目の取得 〔KT13〕
 *         listBox_DrawItem イベントハンドラの中の 
 *         (e.State & DrawItemState.Selected) != 0 ? ... は，説明が必要かもしれません。
 *         このコードの心は e.State == DrawItemState.Selected ? ... なのですが，
 *         こうすると うまくいかない場合があります。
 *         なぜならば，上に掲げた定義を見てもらえればわかるように，
 *         DrawItemState 列挙体はビットフィールドとして使われうるためです。
 *         実際，e.State == DrawItemState.Selected | DrawItemState.Focus 
 *         などとなっている場合がよくあるので，
 *         DrawItemState.Selected が入ったすべての組合せを得るためには，
 *         ちょっと面倒ですが (e.State & DrawItemState.Selected) != 0 ? ... とする必要があります。
 *         
 *@subject Graphics
 *         void     graphics.FillRectangle(Brush, Rectangle)
 *         void     graphics.DrawString(
 *                      string s,     // 文字列
 *                      Font font,    // フォント
 *                      Brush brush,  // ブラシ
 *                      RectangleF rectangle  // 領域を示す四角形, float
 *                  )
 *@subject Brush ブラシ -- Syatem.Drawing
 *         Brush    new SolidBrush(Color)   単色ブラシ
 *         Brush    Brushes.Xxxx             システム定義の標準色
 *         Brush    Brush.SystemBrushes      システムで利用されている色
 *           └ class SystemBrushes / staticメンバー  { get; }
 *             Brush  SystemBrushes.GradientActiveCaption    アクティブなタイトルバーのグラデーションで最も明るい色
 *             Brush  SystemBrushes.GradientInactiveCaption  非アクティブなタイトルバーのグラデーションで最も明るい色
 *             Brush  SystemBrushes.Window      クライアント領域の背景色
 *             Brush  SystemBrushes.ScrollBar   スクロール バーの背景の色
 *             Brush  SystemBrushes.MenuText    メニューのテキストの色
 *             Brush  SystemBrushes.MenuHighlight フラットメニュー項目の強調表示色
 *             Brush  SystemBrushes.MenuBar     メニューバーの背景色
 *             Brush  SystemBrushes.Menu        メニュー背景色
 *             Brush  SystemBrushes.InfoText    ツールヒントのテキスト色
 *             Brush  SystemBrushes.Info        ツールヒントの背景色
 *             Brush  SystemBrushes.InactiveCaptionText 非アクティブのタイトルバーのテキスト色
 *             Brush  SystemBrushes.InactiveBorder      非アクティブなウィンドウの境界線の色
 *             Brush  SystemBrushes.InactiveCaption     非アクティブのタイトルバーの背景色
 *             Brush  SystemBrushes.HotTrack    フォーカス項目の色
 *             Brush  SystemBrushes.HighlightText 選択項目のテキスト色
 *             Brush  SystemBrushes.Highlight   選択項目の背景色
 *             Brush  SystemBrushes.GrayText    淡色表示のテキスト色
 *             Brush  SystemBrushes.WindowText  クライアント領域のテキスト色
 *             Brush  SystemBrushes.ActiveBorder  アクティブなウィンドウの境界線の色
 *             Brush  SystemBrushes.ActiveCaption アクティブなウィンドウのタイトルバーの背景色
 *             Brush  SystemBrushes.ActiveCaptionText  アクティブなウィンドウのタイトルバーのテキスト色
 *             Brush  SystemBrushes.AppWorkspace       アプリケーション作業領域の色
 *             Brush  SystemBrushes.ButtonFace         3D要素の表面の色
 *             Brush  SystemBrushes.ButtonHighlight    3D要素の強調表示色
 *             Brush  SystemBrushes.WindowFrame        ウィンドウ フレームの色
 *             Brush  SystemBrushes.ButtonShadow       3D要素の影色
 *             Brush  SystemBrushes.ControlLightLight  3D要素の強調表示色
 *             Brush  SystemBrushes.ControlLight       3D要素の明るい色
 *             Brush  SystemBrushes.ControlDark        3D要素の影色
 *             Brush  SystemBrushes.ControlDarkDark    3D要素の暗い影色
 *             Brush  SystemBrushes.ControlText        3D要素のテキストの色
 *             Brush  SystemBrushes.Desktop            デスクトップの色
 *             Brush  SystemBrushes.Control            3D要素の表面の色
 *         
 *@NOTE【Problem】
 *      e.Boundsのままだと、１行の高さが小さすぎて文字が入りきらない問題
 *      Rectangle newBounds = new Rectangle(
 *          e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height + 30);
 *      として、e.Bounds -> newBoundsに変更しても、e.Boundsを利用しているのが、
 *      色付けした行だけなので、うまくいかない。
 *      
 *      => ListBox.ItemHeightプロパティを指定すると解決。
 *         listBox.ItemHeight = (int) this.Font.Size,だと
 *         フォントサイズ 12 -> 12ピクセルに変更して、それを１行の高さにしている。
 *         ちょうどいい１行の高さを整数で記述
 *         
 *      => DrawMode.Normalなら、Fontに合わせて、ItemHeightが調整される
 *      
 *@see ImageOwnerDrawSample.jpg
 *@see 
 *@author shika
 *@date 2022-08-12
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT13_OwnerDraw
{
    class MainOwnerDrawSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormOwnerDrawSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormOwnerDrawSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormOwnerDrawSample : Form
    {
        private ListBox list;
        private readonly string[] itemAry = new string[]
        {
            "Mercury", "Venus", "Earth", "Mars", "Jupiter",
            "Saturn", "Uranus","Neptune", "Pluto",
        };

        public FormOwnerDrawSample()
        {
            this.Text = "FormOwnerDrawSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            list = new ListBox()
            {
                Location  = new Point(10, 10),
                Size = new Size(300, 300),
                ItemHeight = 22,
                DrawMode = DrawMode.OwnerDrawFixed,
            };
            list.Items.AddRange(itemAry);
            list.DrawItem += new DrawItemEventHandler(list_DrawItem);

            this.Controls.AddRange(new Control[]
            {
                list,
            });
        }//constructor

        private void list_DrawItem(object sender, DrawItemEventArgs e)
        {
            //---- BackColor ----
            e.DrawBackground();    //既定色で背景色を塗る

            if((e.Index % 2) == 1) //奇数番目(最初: 0)に色付け
            {
                // 項目が選択中であるか否かに応じてブラシを取得
                Brush brush = (e.State & DrawItemState.Selected) != 0 ?
                    SystemBrushes.Highlight : Brushes.AliceBlue;

                // e境界内を塗りつぶし
                e.Graphics.FillRectangle(brush, e.Bounds);  
            }

            //---- Text ----
            string itemText = (sender as ListBox).Items[e.Index].ToString();
            Brush foreBrush = new SolidBrush(e.ForeColor);
            e.Graphics.DrawString(itemText, e.Font, foreBrush, e.Bounds);
            foreBrush.Dispose();

            //---- Focus ----
            e.DrawFocusRectangle(); //フォーカスを示す四角形を描画
        }//list_DrawItem()
    }//class
}
