/** 
 *@title WinFormGUI / Reference / MainGui.cs 
 *@reference (Web) かいてい.net / C# フォーム入門 / コントロールの基本
 *           http://kaitei.net/csforms/controls-basics/
 *           => Reference/WinForm04_ControlBasic.txt
 *           
 *@content Windows Form GUI
 *@prepare 参照追加 System.Windows.Forms
 *         [VS] -> [プロジェクト]配下-> [参照] 右クリック
 *         -> [参照の追加] -> 「System.Windows.Forms」をチェック -> [追加]
 *@subject Main()
 *         [STAThread] Single Thread Application?
 *         SingleThreadであることを宣言する属性
 *         Windows.Controlsの各クラスを呼び出すにはこの宣言が必須?
 *         
 *         //System.InvalidOperationException:
 *         //呼び出しスレッドは、多数の UI コンポーネントが必要としているため、
 *         //STA である必要があります。
 *@subject void System.Windows.Forms.Application.Run(Form);
 *         WindowsFormを実行
 *         
 *@see WinFormButton.jpg
 *@copyTo WinFormSample / KaiteiNet / KT01_Introduction / MainFormSample.cs
 *@author shika 
 *@date 2022-02-21 
*/
using System;
using System.Drawing;
using System.Windows.Forms;
 
namespace WinFormGUI.Reference
{ 
    class MainGui 
    { 
        //[STAThread]
        //static void Main(string[] args) 
        public void Main(string[] args) 
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            //System.Windows.Forms.Application.Run(new FormLabel());
            System.Windows.Forms.Application.Run(new FormButton());
        }//Main() 
 
    }//class
     
    class FormLabel : Form
    {
        private readonly Label label = new Label();

        public FormLabel()
        {
            label.Text = "Hello World";
            label.Location = new Point(10, 20);
            label.AutoSize = true;

            this.Controls.Add(label);
            //or label.Parent = this;
        }//constructor
    }//class

    class FormButton : Form
    {
        private readonly Button button = new Button();
        private int count;

        public FormButton()
        {
            button.Text = "OK";
            button.Location = new Point(10, 10);
            button.Size = new Size(120, 40);
            button.Click += new EventHandler(Btn_Click);

            this.Controls.Add(button);
        }//constructor

        private void Btn_Click(object sender, EventArgs e)
        {
            count++;
            button.Text = $"{count} times Clicked";
        }
    }//class

} 
