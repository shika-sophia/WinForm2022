/*
 *@title WinFormGUI / CsharpCode / ShowEnumValue.cs
 *@content enumの 全列挙定数を表示する
 *
 *@subject foreach (object value in Enum.GetValues(enumType))
 *         object[]     Enum.GetValues(Type)
 *         string value       //value名の文字列表現
 *         int    (int) value //valueの数値表現
 *
 *@subject Type typeof(Xxxx) //Xxxxは変数不可
 *         Type xxxx.GetType()
 *         string type.Name    クラス名
 *         
 *@subject 
 *
 *@NOTE【実行】WinForm2022のプロパティ -> [アプリケーション]タブ
 *            -> 出力の種類: [コンソールアプリケーション]に変更
 *            
 *@author shika
 *@date 2022-06-26
 */
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormGUI.CsharpCode
{
    class ShowEnumValue
    {
        private readonly string enumName;
        private readonly Type enumType;

        //==== Test Main() as Console Application ====
        //static void Main()
        public void Main()
        {
            var here = new ShowEnumValue(typeof(Keys));
            string content = here.BuildEnumContent(here.enumType);
            Console.WriteLine(content);
        }//Main()

        public ShowEnumValue()
        {
            throw new ArgumentNullException(
                    "'new ShowEnumValue(Type)' require its argument as like 'typeof(Xxxx)'");
        }

        public ShowEnumValue(Type enumType)
        {
            this.enumName = enumType.Name;
            this.enumType = enumType;
            CheckEnum(enumType);
        }

        private void CheckEnum(Type enumType)
        {
            try
            {
                Enum.GetValues(enumType);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.GetType());
                Console.WriteLine(
                    $"The Constructor Argument: typeof(Xxxx)\n" +
                    $"Xxxx: {enumName}\n" +
                    $"Xxxx should be Enum type.");
            }
        }

        private string BuildEnumContent(Type enumType)
        {
            int length = Enum.GetNames(enumType).Length * 20;

            var bld = new StringBuilder(length);
            bld.Append($"enum {enumName}\n");
            bld.Append("{\n");

            foreach (var value in Enum.GetValues(enumType))
            {
                bld.Append($"    {value} = {(int)value},\n");
            }//foreach
            bld.Append("}\n");

            //Console.WriteLine($"length:{length}");
            //Console.WriteLine($"bld.Length:{bld.Length}");
            return bld.ToString();
        }        
    }//class
}

/*
enum MessageBoxButtons
{
    OK = 0,
    OKCancel = 1,
    AbortRetryIgnore = 2,
    YesNoCancel = 3,
    YesNo = 4,
    RetryCancel = 5,
}

System.ArgumentNullException: 
'new ShowEnumValue(Type)' require its argument as like 'typeof(Xxxx)'

System.ArgumentException
The Constructor Argument: typeof(Xxxx)
Xxxx: MouseEventArgs
Xxxx should be Enum type.
*/
