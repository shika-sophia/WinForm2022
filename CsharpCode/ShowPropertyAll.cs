/**
 *@title WinFormGUI / CsharpCode / ShowPropertyAll.cs
 *@content Relection.PropertyInfoを用いて、すべてのプロパティを表示
 *
 *@subject Reflection -- System.Reflection |〔CS93〕〔DJ159〕
 *         ◆Type〔CS63〕
 *         Type    typeof(T)         typeof()演算子の引数は 既定クラスのみ
 *         Type    xxxx.GetType()     xxxxは インスタンス
 *         
 *         ◆PropertyInfo : MemberInfo, _PropertyInfo
 *         PropertyInfo     type.GetProperty(string name, [Type returnType]);  
 *                                                     対象型のプロパティ名, [戻り値型] を指定したプロパティを取得
 *         PropertyInfo[]   type.GetProperties()       対象型のすべてのプロパティを取得
 *         
 *         string   memberInfo.Name                    メンバー名
 *         string   memberInfo.FullName                メンバー名を完全修飾名で取得
 *         bool     propertyInfo.CanWrite { get; }     set可能か
 *         Type     propertyInfo.PropertyType { get; } プロパティの型 (.NET Framework型)
 *         
 *         object   propertyInfo.GetValue(object)      指定したオブジェクトのプロパティ値を取得
 *           └> 必要に応じてキャスト
 *         
 *@NOTE【実行】
 *      ・Test Main()内 new ShowPropertyAll(typeof(Xxxx))の「Xxxx」を
 *        表示したいクラスに変更して実行する
 *      ・Test Main()内 BuildReflectionPropertyInfo(bool) を
 *        tureにすると、「 *@subject 」を付記したインデントで表示
 *        falseにすると、上記を付記しない
 *        
 *@NOTE【註】
 *      ・戻値型が C#型ではなく、.NET Framework型で表示される
 *      ・対象オブジェクトは このクラスの型を staticのように 「Xxxx.」と表示
 *        (非staticプロパティも 「Xxxx.」と Pascal記法になることに注意)
 *      ・「◆class (名前空間).Xxxxx(=クラス名)」と表示
 *        (structでも classと表示される)
 *      ・enumの場合は => 〔ShowEnumValue.cs〕を利用
 *        (動作はするが、プロパティがないので、表示項目は 0 )
 *      ・PropertyInfo  type.GetProperties()を 
 *        GetFields(), GetMethods(), GetEvents(), GetMembers()に変更することもできる
 *        GetEnumNames(), GetEnumValues() も可だが、上記を利用
 *        (Reflectionは オーバーヘッドが掛かるため、
 *         C#通常動作(=「.」演算子によるアクセス) が可能であれば、そちらを利用)〔Javaノート | DJ159〕
 *      
 *@copyTo ~\WinFormSample\KaiteiNet\KT07_Graphics\MainDrawIconSample.cs
 *@author shika
 *@date 2022-08-23
 */

using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WinFormGUI.CsharpCode
{
    class ShowPropertyAll
    {
        private readonly Type type;

        //static void Main()
        public void Main()
        {
            var here = new ShowPropertyAll(typeof(SystemColors));
            here.BuildReflectionPropertyInfo(false);  
        }//Main()

        public ShowPropertyAll()
        {
            throw new ArgumentNullException(
                "'ShowPropertyAll(Type)' require the argument as 'Type'.");
        }//constructor

        public ShowPropertyAll(Type type)
        {
            this.type = type;
        }//constructor

        private void BuildReflectionPropertyInfo(bool subject = false)
        {
            PropertyInfo[] propertyAry = type.GetProperties();

            var build = new StringBuilder(propertyAry.Length * 60);

            if (subject) { build.Append(" *@subject "); }
            build.Append($"◆class {type.FullName}\n");

            if (subject) { build.Append(" *         "); }
            build.Append("＊All Properties\n");

            foreach(PropertyInfo prop in propertyAry)
            {
                Type propType = prop.PropertyType;
                string propName = prop.Name;
                //object propValue = prop.GetValue(propName);
                bool canWrite = prop.CanWrite;

                if (subject) { build.Append(" *         "); }          //subject時のインデント
                build.Append($"{propType.Name}   ");                   //戻値型
                build.Append($"{type.Name}.{propName} ");              //対象オブジェクト.プロパティ名
                //build.Append($" = {propValue} ");                    //プロパティ値 (要キャスト)
                build.Append(canWrite ? " { get; set;}" : " { get; }");// set可能か
                build.Append("\n");                                    //改行
            }//foreach

            Console.WriteLine($"Property Count: {propertyAry.Length}");
            Console.WriteLine($"build.Length: {build.Length}");
            Console.WriteLine();
            Console.WriteLine(build.ToString());
        }//BuildReflection()
    }//class
}

/*

//---- subject == false ----
Property Count: 33
build.Length: 1488
◆class System.Drawing.SystemColors
＊All Properties
Color   SystemColors.ActiveBorder  { get; }
Color   SystemColors.ActiveCaption  { get; }
Color   SystemColors.ActiveCaptionText  { get; }
Color   SystemColors.AppWorkspace  { get; }
Color   SystemColors.ButtonFace  { get; }
Color   SystemColors.ButtonHighlight  { get; }
Color   SystemColors.ButtonShadow  { get; }
Color   SystemColors.Control  { get; }
Color   SystemColors.ControlDark  { get; }
Color   SystemColors.ControlDarkDark  { get; }
Color   SystemColors.ControlLight  { get; }
Color   SystemColors.ControlLightLight  { get; }
Color   SystemColors.ControlText  { get; }
Color   SystemColors.Desktop  { get; }
Color   SystemColors.GradientActiveCaption  { get; }
Color   SystemColors.GradientInactiveCaption  { get; }
Color   SystemColors.GrayText  { get; }
Color   SystemColors.Highlight  { get; }
Color   SystemColors.HighlightText  { get; }
Color   SystemColors.HotTrack  { get; }
Color   SystemColors.InactiveBorder  { get; }
Color   SystemColors.InactiveCaption  { get; }
Color   SystemColors.InactiveCaptionText  { get; }
Color   SystemColors.Info  { get; }
Color   SystemColors.InfoText  { get; }
Color   SystemColors.Menu  { get; }
Color   SystemColors.MenuBar  { get; }
Color   SystemColors.MenuHighlight  { get; }
Color   SystemColors.MenuText  { get; }
Color   SystemColors.ScrollBar  { get; }
Color   SystemColors.Window  { get; }
Color   SystemColors.WindowFrame  { get; }
Color   SystemColors.WindowText  { get; }

//---- subject == true ----
Property Count: 33
build.Length: 1873
 *@subject ◆class System.Drawing.SystemColors
 *         ＊All Properties
 *         Color   SystemColors.ActiveBorder  { get; }
 *         Color   SystemColors.ActiveCaption  { get; }
 *         Color   SystemColors.ActiveCaptionText  { get; }
 *         Color   SystemColors.AppWorkspace  { get; }
 *         Color   SystemColors.ButtonFace  { get; }
 *         Color   SystemColors.ButtonHighlight  { get; }
 *         Color   SystemColors.ButtonShadow  { get; }
 *         Color   SystemColors.Control  { get; }
 *         Color   SystemColors.ControlDark  { get; }
 *         Color   SystemColors.ControlDarkDark  { get; }
 *         Color   SystemColors.ControlLight  { get; }
 *         Color   SystemColors.ControlLightLight  { get; }
 *         Color   SystemColors.ControlText  { get; }
 *         Color   SystemColors.Desktop  { get; }
 *         Color   SystemColors.GradientActiveCaption  { get; }
 *         Color   SystemColors.GradientInactiveCaption  { get; }
 *         Color   SystemColors.GrayText  { get; }
 *         Color   SystemColors.Highlight  { get; }
 *         Color   SystemColors.HighlightText  { get; }
 *         Color   SystemColors.HotTrack  { get; }
 *         Color   SystemColors.InactiveBorder  { get; }
 *         Color   SystemColors.InactiveCaption  { get; }
 *         Color   SystemColors.InactiveCaptionText  { get; }
 *         Color   SystemColors.Info  { get; }
 *         Color   SystemColors.InfoText  { get; }
 *         Color   SystemColors.Menu  { get; }
 *         Color   SystemColors.MenuBar  { get; }
 *         Color   SystemColors.MenuHighlight  { get; }
 *         Color   SystemColors.MenuText  { get; }
 *         Color   SystemColors.ScrollBar  { get; }
 *         Color   SystemColors.Window  { get; }
 *         Color   SystemColors.WindowFrame  { get; }
 *         Color   SystemColors.WindowText  { get; }
 */
