/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainBinaryWirterMarshal.cs
 *@class   └ new FormBinaryWirterMarshal() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content RR[273] p482  BinaryWirterMarshal
 *         structデータを ファイルに保存
 *         
 *@subject ◆BinaryWriter class : IDisposable -- System.IO.
 *         プリミティブ型をバイナリでストリームに書き込みます。
 *         特定のエンコーディングの文字列の書き込みをサポートします。
 *         BinaryWriter  new BinaryWriter([Stream output])
 *         BinaryWriter  new BinaryWriter([Stream, Encoding, bool leaveOpen])
 *         
 *         void  binaryWriter.Write(T value)
 *         void  binaryWriter.Flush()
 *         void  binaryWriter.Dispose()
 *         void  binaryWriter.Close()
 *         
 *@subject ◆Marshal static class 〔一部〕-- System.Runtime.InteropServices.
 *         アンマネージド コードを扱うときに使用できる さまざまなメソッドを提供するクラス。
 *         アンマネージド メモリの割り当て、アンマネージドメモリ ブロックのコピー、
 *         マネージド型からアンマネージド型への変換など。
 *
 *         int SizeOf<T>([T structure])  [.NET Framework 4.5.1 -]
 *         int SizeOf(Type)
 *         int SizeOf(object)
 *             指定された型のオブジェクトのアンマネージのサイズをバイト数で返します。
 *             引数 structure:  サイズが返されるオブジェクト。
 *
 *         void  Marshal.Copy(                     配列データを アンマネージド メモリ ポインタにコピー
 *             byte[] source, int startIndex, 
 *             IntPtr destination, int length);
 *            
 *        　IntPtr  Marshal.CreateAggregatedObject<T>(IntPtr pOuter, T)  
 *          IntPtr  Marshal.CreateAggregatedObject   (IntPtr pOuter, object)
 *        　    指定した COM オブジェクトを使用してマネージド オブジェクトを集約。
 *              引数: pOuter:  外部 IUnknown ポインター。
 *              戻値: マネージド オブジェクトの内部 IUnknown ポインター。
 *             
 *          void    Marshal.StructureToPtr(object structure, IntPtr ptr, bool fDeleteOld);
 *          void    Marshak.StructureToPtr<T>(T structure, IntPtr ptr, bool fDeleteOld);
 *              [.NET Framework Version 4.5.1 -]
 *              指定した型のマネージド オブジェクトから、アンマネージド メモリ ブロックにデータをマーシャリング。
 *              引数 structure:  マーシャリングするデータを保持すマネージド オブジェクト。 
 *                               オブジェクトは、書式指定クラスの構造体またはインスタンスである必要がある。
 *                   ptr:        このメソッドを呼び出す前に割り当てる必要があるアンマネージ メモリ ブロックへのポインター。
 *                   fDeleteOld: このメソッドがデータをコピーする前に、ptr パラメーターに対して 
 *                               System.Runtime.InteropServices.Marshal.DestroyStructure``1(System.IntPtr)メソッドを呼び出す場合は true。
 *                               ブロックには有効なデータを含める必要があります。
 *                               メモリ ブロックに既にデータが格納されているときに false を渡すと、
 *                               メモリリークが発生する可能性があることに注意してください。
 *                               
 *@subject ◆GCHandle struct -- System.Runtime.InteropServices.
 *         GCHandle  GCHandle.Alloc(object value, [GCHandleType type])
 *                      指定したオブジェクトに GCHandleType.Normal ハンドルを割り当てます。
 *                      引数: GCHandle を使用するオブジェクト。
 *                      戻値: オブジェクトをガベージ コレクションから保護する新しい GCHandle。
 *                            GCHandleは、不要になったときに GCHandle.Free で解放する必要があります。
 *                      
 *                      enum GCHandleType
 *                      {
 *                          Weak = 0,  この種類のハンドルはオブジェクトを追跡するために使用
 *                                      そのオブジェクトを収集できるようにもします。 
 *                                      オブジェクトが収集された場合、GCHandleインスタンスの内容は 0 に設定されます。
 *                                      Weak 参照はファイナライザーの実行前に 0 に設定されるため、
 *                                      ファイナライザーがそのオブジェクトを復活させた場合でも、Weak 参照は 0 のままになります。
 *                                      
 *                          WeakTrackResurrection = 1,     //ファイナライズ中にオブジェクトが復活された場合、このハンドルは0 には設定されません。
 *                          
 *                          Normal = 2, この種類のハンドルは、非透過ハンドルを表します。
 *                                      つまり、このハンドルを使用して、その中に格納されている固定化されたオブジェクトのアドレスを解決することはできません。
 *                                      この種類のハンドルは、オブジェクトを追跡し、ガベージ コレクターによって収集されないようにするために使用できます。 
 *                                      マネージド オブジェクトへの、ガベージコレクターが検出できない唯一の参照をアンマネージド クライアントが保持している場合、
 *                                      この列挙体メンバーを使用すると便利です。
 *                               
 *                          Pinned = 3  このハンドルを使用した場合は、固定オブジェクトのアドレスを取得できます。
 *                                      これにより、ガベージ コレクターがそのオブジェクトを移動できなくなるため、
 *                                      ガベージ コレクターの効率は低下することになります。
 *                                      割り当てられたハンドルをできる限り早く解放するには、GCHandle.Free()を使用します。
 *                      }
 *                      
 *         void      gcHandle.Free()
 *         
 *         IntPtr    gcHandle.AddrOfPinnedObject()  
 *                      GCHandleType.Pinned ハンドル内のオブジェクトのアドレスを取得
 *                      戻り値:  System.IntPtr としての固定オブジェクトのアドレス。
 *                      
 *@see ImageBinaryWirterMarshal.jpg
 *@see 
 *@author shika
 *@date 2022-10-14
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR09_FileDirectory
{
    class MainBinaryWirterMarshal
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormBinaryWirterMarshal()");

            Application.EnableVisualStyles();
            Application.Run(new FormBinaryWirterMarshal());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormBinaryWirterMarshal : Form
    {
        private readonly TableLayoutPanel table;
        private readonly Label label;
        private readonly TextBox textBox;
        private readonly Button button;

        public FormBinaryWirterMarshal()
        {
            this.Text = "FormBinaryWirterMarshal";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 160);
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Fill,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            label = new Label()
            {
                Text = "File Name:",
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(label, 0, 0);

            textBox = new TextBox()
            {
                Text = "/binaryDataStruct.bin",
                TextAlign = HorizontalAlignment.Left,
                Dock = DockStyle.Fill,
                Multiline = false,
            };
            table.Controls.Add(textBox, 1, 0);

            button = new Button()
            {
                Text = $"Write 'struct {nameof(Block)}' data to File.",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button, 0, 1);
            table.SetColumnSpan(button, 2);

            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private struct Block  //struct as self defined
        {
            public int x;
            public int y;
            public int z;
            public Color color;
        }//struct

        private void Button_Click(object sender, EventArgs e)
        {
            string dir = Path.GetDirectoryName(
                "../../WinFormSample/ReverseReference/RR09_FileDirectory/MainByteFileWriteSample.cs");
            string fileName = textBox.Text;

            if (String.IsNullOrEmpty(fileName)) { return; }

            Block block = new Block()  //struct〔above〕
            {
                x = 100,
                y = 200,
                z = 0,
                color = Color.HotPink,
            };

            using (BinaryWriter writer = new BinaryWriter(File.Create(dir + fileName)))
            {
                byte[] dataAry = new byte[Marshal.SizeOf(typeof(Block))];
                GCHandle handle = GCHandle.Alloc(dataAry, GCHandleType.Pinned);

                try
                {
                    Marshal.StructureToPtr(
                        block, handle.AddrOfPinnedObject(), fDeleteOld: false);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                finally
                {
                    handle.Free();
                }

                for(int i = 0; i < dataAry.Length; i++)
                {
                    writer.Write(dataAry[i]);
                }//for

                writer.Close();
            }//using

            MessageBox.Show(
                $"Created 'struct {nameof(Block)}' data file.",
                "Result");
        }//Button_Click()
    }//class
}
