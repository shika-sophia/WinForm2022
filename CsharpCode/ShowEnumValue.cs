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
using System.Net.Sockets;
using System.Text;

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
            var here = new ShowEnumValue(typeof(AddressFamily));
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

        private string BuildEnumContent(Type enumType, bool isSubject = true)
        {
            Array valueAry = Enum.GetValues(enumType);
            string[] nameAry = Enum.GetNames(enumType);
            int length = nameAry.Length * 20;

            var bld = new StringBuilder(length);
            if(isSubject)
            {
                bld.Append("/*\n");
                bld.Append(" *@subject "); 
            }
            bld.Append($"enum {enumName}\n");

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
 *@subject enum FileMode
 *         {
 *             CreateNew = 1,
 *             Create = 2,
 *             Open = 3,
 *             OpenOrCreate = 4,
 *             Truncate = 5,
 *             Append = 6,
 *         }
 */
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
