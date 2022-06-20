/*
 *@title WinFormSample / KaiteiNet / KT03_Form / MainFormColor.cs
 *@see   MainFormConstruct.cs
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT03_Form
{
    class MainFormColor
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Application.Run(new FormColorSample());
        }//Main()
    }//class

    class FormColorSample : Form
    {
        public FormColorSample()
        {
            this.Text = "FormColorSample";
            this.ForeColor = Color.White;
            this.BackColor = Color.Pink;

            Label label = new Label();
            label.Text = "文字色";            
            this.Controls.Add(label);
            
            this.BackgroundImage = Image.FromFile("../../Image/squreTile40px.png");
            this.BackgroundImageLayout = ImageLayout.Tile;
        }
    }//class
}

/*
//これだとエラーが出る
//Bitmap bitmap = new Bitmap("../../Image/squreTile40px.png");
//this.BackgroundImage = bitmap;
//bitmap.Dispose();
*/