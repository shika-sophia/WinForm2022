/*
 *@title WinFormGUI / CsharpCode / ShowEnumValue.cs
 *@content enumの 全列挙定数を表示する
 *
 *@subject 
 *         string[]  Enum.GetNames(Type)
 *         Array     Enum.GetValues(Type)
 *           enum   value       //value名 = enum型
 *           int    (int) value //valueの数値表現 
 *           T      array.GetValue(int index)
 *           object Array.GetValue(T[], int index)
 *         
 *           
 *@subject Type typeof(Xxxx) //Xxxxは変数不可
 *         Type xxxx.GetType()
 *         string type.Name    クラス名
 *         
 *@NOTE【実行】WinForm2022のプロパティ -> [アプリケーション]タブ
 *            -> 出力の種類: [コンソールアプリケーション]に変更
 *
 *@NOTE【Problem】名前の重複
 *      enumの数値が同一で複数の名前がある場合
 *      Array  Enum.GetValues(Type) -> value, (int) value で 出力すると、
 *      同一値の名前が重複してしまう問題
 *      
 *      => string[]  Enum.GetNames(Type)で名前を取得し、
 *          Array  Enum.GetValues(Type) -> array.GetValue(int index)で 数値を取得すると解決
 *          
 *@author shika
 *@date 2022-06-26
 */
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
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
            var here = new ShowEnumValue(typeof(BorderStyle));
            string content = here.BuildEnumContent(here.enumType, isSubject: true);
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
        }//constructor

        private void CheckEnum(Type enumType)
        {
            if(!enumType.IsEnum)
            {
                Console.WriteLine(
                    $"The Constructor Argument: typeof(Xxxx)\n" +
                    $"Xxxx: {enumName}\n" +
                    $"Xxxx should be Enum type.");

                throw new ArgumentException();
            }
        }//CheckEnum()

        private string BuildEnumContent(Type enumType, bool isSubject = true)
        {
            Array valueAry = Enum.GetValues(enumType);
            string[] nameAry = Enum.GetNames(enumType);
            int length = nameAry.Length * 100;

            var bld = new StringBuilder(length);
            if(isSubject)
            {
                bld.Append("/*\n");
                bld.Append(" *@subject "); 
            }

            bld.Append($"◆enum {enumName} : System.Enum\n");

            if (isSubject) { bld.Append(" *         "); }
            for(int i = 0; i < enumName.Length; i++)
            {
                bld.Append(" ");
            }
            bld.Append($"  -- {enumType.Namespace}\n");

            if (isSubject) { bld.Append(" *         "); }
            bld.Append("{\n");

            for(int i = 0; i < nameAry.Length; i++)
            {
                string name = nameAry[i];
                int value = (int) valueAry.GetValue(i);
                    
                if (isSubject) { bld.Append(" *         "); }
                bld.Append($"    {name} = {value},\n");
            }//for

            if (isSubject) { bld.Append(" *         "); }
            bld.Append("}\n");

            if (isSubject) { bld.Append(" */\n"); }

            //Console.WriteLine($"length:{length}");
            //Console.WriteLine($"bld.Length:{bld.Length}");
            return bld.ToString();
        }        
    }//class
}


//==== bool subject true ====
/*
/*
 *@subject ◆enum BorderStyle : System.Enum
 *                      -- System.Windows.Forms
 *         {
 *             None = 0,
 *             FixedSingle = 1,
 *             Fixed3D = 2,
 *         }

System.ArgumentNullException: 
'new ShowEnumValue(Type)' require its argument as like 'typeof(Xxxx)'

System.ArgumentException
The Constructor Argument: typeof(Xxxx)
Xxxx: MouseEventArgs
Xxxx should be Enum type.
*/
