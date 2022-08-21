/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainDrawIconSample.cs
 *@class   └ new FormDrawIconSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics / DrawIcon()
 *@subject Graphics
 *         void  graphics.DrawIcon(Icon, int x, int y)  Iconクラスを指定位置に描画、サイズは Icon のサイズ
 *         void  graphics.DrawIcon(Icon, Rectangle)     Iconクラスを指定位置とサイズで描画
 *         
 *         [×] float, Point, PointF, RectangleF は定義なし
 *
 *@subject フォームのアイコン設定
 *         Icon    form.Icon       フォームのアイコン
 *         bool    form.ShowIcon   フォームのアイコンを表示するか / デフォルト: true
 *         
 *@subject〔WF44〕◆Icon : MarshalByRefObject, ISerializable, ICloneable, IDisposable
 *         Icon   new Icon(string fileName)
 *         Icon   new Icon(Stream stream)
 *         Icon   new Icon(string fileName, Size size)
 *         Icon   new Icon(Icon original, Size size)
 *         Icon   new Icon(Type type, string resource)
 *         Icon   new Icon(Stream stream, Size size)
 *         Icon   new Icon(string fileName, int width, int height)
 *         Icon   new Icon(Icon original, int width, int height)
 *         Icon   new Icon(Stream stream, int width, int height)
 *         
 *         int    icon.Width    { get; }
 *         int    icon.Height   { get; }
 *         Size   icon.Size     { get; }
 *         
 *         Icon   static Icon.ExtractAssociatedIcon(string filename) 画像ファイル(.bmp/.jpgなど)から Iconを作成
 *         void   icon.Save(Stream outputStream)
 *         Bitmap icon.ToBitmap()
 *         
 *         => copyFrom〔RR05_MenuToolStrip/MainNotifyIconSample.cs〕
 *         
 *@subject class SystemIcons --System.Drawing.
 *         ・System32 既定のアイコン
 *         ・enumではく classなので Reflectionを利用して、全プロパティを取得
 *         ・「Windowsアイコン」は ちゃんと描画できない
 *         ・PictureBoxで graphics.DrawIcon()することもできる
 *           =>〔Reference/SystemIconDiv/MainSystemIconSample.cs〕
 *         
 *         public sealed class SystemIcons   
 *         {
 *             static Icon Application { get; }  WIN32: IDI_APPLICATION
 *             static Icon Asterisk { get; }     WIN32: IDI_ASTERISK
 *             static Icon Error { get; }        WIN32: IDI_ERROR
 *             static Icon Exclamation { get; }  WIN32: IDI_EXCLAMATION
 *             static Icon Hand { get; }         WIN32: IDI_HAND
 *             static Icon Information { get; }  WIN32: IDI_INFORMATION
 *             static Icon Question { get; }     WIN32: IDI_QUESTION
 *             static Icon Warning { get; }      WIN32: IDI_WARNING
 *             static Icon WinLogo { get; }      WIN32: IDI_WINLOGO
 *             static Icon Shield { get; }       シールド アイコン
 *         }
 *         
 *@subject Reflection 〔CS93〕-- System.Reflection
 *         ◆Type〔CS63〕
 *         Type    typeof(T)         typeof()演算子の引数は 既定クラスのみ
 *         Type    obj.GetType()     xxxxは インスタンス
 *         
 *         ◆PropertyInfo : MemberInfo, _PropertyInfo
 *         PropertyInfo     type.GetProperty(string?)  対象型の指定したプロパティを取得
 *         PropertyInfo[]   type.GetProperties()       対象型のすべてのプロパティを取得
 *         string   memberInfo.Name                    プロパティ名
 *         object   propertyInfo.GetValue(object)      指定したオブジェクトのプロパティ値を取得
 *           └> 必要に応じてキャスト
 *           
 *         例: 
 *         foreach(PropertyInfo value in typeof(SystemIcons).GetProperties())
 *         {
 *             Icon sysIcon = (Icon)value.GetValue(value.Name);
 *               :
 *             g.DrawIcon(sysIcon, x, y);
 *             g.DrawString(
 *                 value.Name, this.Font, brush, x, y + 40);
 *         }//foreach
 *         
 *@see ImageDrawIconSample.jpg
 *@see 
 *@author shika
 *@date 2022-08-21
 */
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainDrawIconSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormDrawIconSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormDrawIconSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormDrawIconSample : Form
    {
        public FormDrawIconSample()
        {
            this.Text = "FormDrawIconSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(600, 400);
            this.BackColor = SystemColors.Window;

            //this.Controls.AddRange(new Control[]
            //{

            //});
        }//constructor

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            Brush brush = Brushes.Blue;

            int x = 20;
            int y = 60;
            int index = 0;

            g.DrawString("SystemIcons:\n", this.Font, Brushes.DeepPink, 20, 20);

            foreach(PropertyInfo value in typeof(SystemIcons).GetProperties())
            {
                Icon sysIcon = (Icon)value.GetValue(value.Name);

                //each 5 items return new line.
                x = 20 + (index % 5) * 120;
                if(index % 5 == 0 && index != 0)
                {
                    y += 100;
                }

                g.DrawIcon(sysIcon, x, y);
                g.DrawString(
                    value.Name, this.Font, brush, x, y + 40);

                index++;
            }//foreach

            brush.Dispose();
            g.Dispose();
        }//OnPaint()
    }//class
}
