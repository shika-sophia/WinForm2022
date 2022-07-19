/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainRichTextBoxSample.cs
 *@class FormRichTextBoxSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[93] RichTextBox -- Undo(), Redo() / p177
 *@subject ◆RichTextBox : TextBoxBase
 *         RichTextBox  new RichTextBox()
 *         void         rich.Undo()          処理を戻す
 *         void         rich.Redo()          戻した処理を再実行
 *         bool         rich.CanUndo         Undo()可能な処理が存在するか
 *         bool         rich.CanRedo         Redo()可能な処理が存在するか
 *         string       rich.UndoActionName  Undo()で戻す処理内容      (日本語で出る)
 *         string       rich.RedoActionName  Redo()で再実行する処理内容 (日本語で出る)
 *
 *@see FormRichTextBoxSample_withUndoRedo.jpg
 *@author shika
 *@date 2022-07-19
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainRichTextBoxSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormRichTextBoxSample());
        }//Main()
    }//class

    class FormRichTextBoxSample : Form
    {
        private TableLayoutPanel table;
        private RichTextBox rich;
        private Label label;
        private Button btnUndo;
        private Button btnRedo;

        public FormRichTextBoxSample()
        {
            this.Text = "FormRichTextBoxSample";
            this.AutoSize = true;

            table = new TableLayoutPanel()
            {
                ColumnCount = 3,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 33.4F));
            table.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 33.3F));
            table.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 33.3F));

            rich = new RichTextBox()
            {
                Size = new Size(400, 300),
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(rich, 0, 0);
            table.SetColumnSpan(rich, 3);

            label = new Label()
            {
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(label, 0, 1);

            btnUndo = new Button()
            {
                Text = "Undo",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            btnUndo.Click += new EventHandler(btnUndo_Click);
            table.Controls.Add(btnUndo, 1, 1);

            btnRedo = new Button()
            {
                Text = "Redo",
                Dock = DockStyle.Fill, 
                AutoSize = true,
            };
            btnRedo.Click += new EventHandler(btnRedo_Click);
            table.Controls.Add(btnRedo, 2, 1);

            this.Controls.Add(table);
        }//constructor

        private void btnUndo_Click(object sender, EventArgs e)
        {
            label.Text = rich.UndoActionName;

            DialogResult result = ShowMessage(
                $"[ {rich.UndoActionName} ]\n処理を戻しますか？",
                "Undo Confirm");
            if(result == DialogResult.OK)
            {
                rich.Undo();
            }

            label.Text = "";
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            rich.Redo();
        }

        private DialogResult ShowMessage(string text, string caption)
        {
            DialogResult result = MessageBox.Show(
                text,
                caption,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            return result;
        }//ShowMessage()
    }//class

}
