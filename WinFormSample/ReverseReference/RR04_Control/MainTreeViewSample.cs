/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainTreeViewSample.CS
 *@class FormTreeViewSample.CS
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *
 *@content RR[90] TreeView / p171
 *         階層構造のデータをツリー表示
 *         
 *@subject ◆TreeView : Control
 *         TreeView            new TreeView()
 *         TreeNodeCollection  tree.Nodes     〔下記〕
 *         bool      tree.ShowPlusMinus             デフォルト: true
 *         bool      tree.ShowLines / ShowRootLines デフォルト: true
 *         Color     tree.LineColor        
 *         TreeNode  tree.SelectedNode     選択されたノード
 *         string    tree.PathSeparator    node.FullPathの区切り文字 デフォルト「¥」=「\」
 *         int       tree.Indent           インデント px単位         デフォルト 19px
 *         ImageList tree.StateImageList   ノードの状態を表すときに利用する画像リスト
 *         ImageList tree.ImageList        ノードのコンテンツとしての画像リスト
 *         
 *         void      tree.CollapseAll()    全て収納
 *         void      tree.ExpandAll()      全て展開
 *         TreeNode  tree.GetNodeAt(Point pt) / GetNodeAt(int x, int y)
 *         int       tree.GetNodeCount(bool)  ノードの数。true: サブツリーを含む
 *         void      tree.BeginUpdate()
 *         void      tree.EndUpdate()
 *         
 *         ・ソート
 *         bool      tree.Sorted             ソートを行うか
 *         IComparer tree.TreeViewNodeSoter  ソート順のカスタマイズ
 *         void      tree.Sort()             ソートの実行
 *         
 *@subject TreeNodeCollection : IList, ICollection, IEnumerable
 *         TreeNodeCollection tree.Nodes 
 *         int       tree.Nodes.Count
 *         bool      tree.Nodes.IsReadOnly
 *         
 *         ・追加 / 削除
 *         int       tree.Nodes.Add(TreeNode)
 *         TreeNode  tree.Nodes.Add([string key], string text)
 *         void      tree.AddRange(TreeNode[])
 *         int       tree.Nodes.Insert(int index, TreeNode)
 *         void      tree.Nodes.Remove(TreeNode)
 *         void      tree.Nodes.RemoveAt(int) / RemoveByKey(string key)
 *         void      tree.Nodes.Clear()   全てのノードを削除
 *         
 *         ・検索
 *         bool      tree.Nodes.Contains(TreeNode) / ContainsKey(string key)
 *         int       tree.Nodes.IndexOf(TreeNode) / IndexOfKey(string key)
 *         TreeNode[] tree.Find(string key, bool)  keyで検索。 true: すべての子ノードも含む
 *
 *@subject TreeNode
 *         TreeNode  new TreeNode(string text, TreeNode[] children)
 *         TreeNodeCollection node.Nodes      子ノードのコレクション〔上記〕
 *         int       node.GetNodeCount(bool)  ノードの数。true: 子ノードのサブツリーも含む
 *         int       node.Index
 *         int       node.Level    階層深度 最初: 0
 *         TreeNode  node.Parent
 *         TreeNode  node.FirstNode / LastNode / PrevNode / NextNode
 *         TreeNode  node.PrevVisibleNode / NextVisibleNode
 *         Font      node.NodeFont
 *         
 *         bool      node.IsExpanded   展開しているか
 *         bool      node.IsEditing    編集可能か
 *         bool      node.IsSelected   選択されているか
 *         bool      node.IsVisible    表示可能か
 *         bool      node.Checked      チェックされているか
 *         string    node.FullPath     ルートノードからのpath
 *         
 *         ・展開 / 収納
 *         void      node.Collapse([bool])  収納。true: 子ノードを含む
 *         void      node.Expand()          展開
 *         void      node.ExpandAll()       すべて展開
 *         void      node.Toggle()          展開・収納の状態を切替
 *         
 *         ・編集
 *         void      node.BeginEdit()
 *         void      node.EndEdit(bool cancel)  編集終了。true: 保存せずにキャンセル
 *         void      node.Remove()
 *         object    node.Clone()
 *         
 *@subject BuildInitialNode() 自己定義メソッド、初期階層の構築
 *         新規 WindowsForm -> TreeView -> [Nodeの編集]ダイアログにて編集
 *         実際の階層と順番が異なるのは、未宣言のノードを子ノードとして配列に代入できないため
 *         子ノードを持たない末端ノードから宣言されるから。
 *
 *         => Form1.Designer.cs / InitializeComponent()内にコード自動生成
 *
 *@see FormTreeViewSample_withAdd.jpg
 *@author shika
 *@date 2022-07-18
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainTreeViewSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormTreeViewSample());
        }//Main()
    }//class

    class FormTreeViewSample : Form
    {
        private TableLayoutPanel table;
        private TreeView tree;
        private Label label;
        private TextBox textBox;
        private Button buttonPath;
        private Button buttonAdd;
        private Button buttonRemove;
        private Button buttonCollapse;
        private Button buttonExpand;

        public FormTreeViewSample()
        {
            this.Text = "FormTreeViewSample";
            this.AutoSize = true;

            table = new TableLayoutPanel()
            {
                ColumnCount = 3,
                RowCount = 6,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));

            tree = new TreeView()
            {
                ShowPlusMinus = true,
                ShowLines = true,
                Sorted = false,
                PathSeparator = " / \n",
                Dock = DockStyle.Fill,
            };
            BuildInitialNode(); //自己定義メソッド -- 初期ノード階層
            table.Controls.Add(tree, 0, 0);
            table.SetRowSpan(tree, 6);

            buttonPath = new Button()
            {
                Text = "Show Path",
                Dock = DockStyle.Fill,
            };
            buttonPath.Click += new EventHandler(buttonPath_Click);
            table.Controls.Add(buttonPath, 1, 0);
            table.SetColumnSpan(buttonPath, 2);

            label = new Label()
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
                AutoSize = true,
            };
            table.Controls.Add(label, 1, 1);
            table.SetColumnSpan(label, 2);

            buttonAdd = new Button()
            {
                Text = "Add",
                Dock = DockStyle.Fill,
            };
            buttonAdd.Click += new EventHandler(buttonAdd_Click);
            table.Controls.Add(buttonAdd, 1, 2);
            table.SetColumnSpan(buttonAdd, 2);

            textBox = new TextBox()
            {
                TabIndex = 0,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(textBox, 1, 3);
            table.SetColumnSpan(textBox, 2);

            buttonRemove = new Button()
            {
                Text = "Remove",
                Dock = DockStyle.Fill,
            };
            buttonRemove.Click += new EventHandler(buttonRemove_Click);
            table.Controls.Add(buttonRemove, 1, 4);
            table.SetColumnSpan(buttonRemove, 2);

            buttonCollapse = new Button()
            {
                Text = "Collapse All",
                AutoSize = true,
            };
            buttonCollapse.Click += new EventHandler(buttonCollapse_Click);
            table.Controls.Add(buttonCollapse, 1, 5);

            buttonExpand = new Button()
            {
                Text = "Expand All",
                AutoSize = true,
            };
            buttonExpand.Click += new EventHandler(buttonExpand_Click);
            table.Controls.Add(buttonExpand, 2, 5);

            this.Controls.Add(table);
        }//constructor

        private void buttonPath_Click(object sender, EventArgs e)
        {
            label.Text = tree.SelectedNode.FullPath;
            this.Refresh();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if(textBox.Text == "")
            {
                return;
            }
            
            if(tree.SelectedNode == null)
            {
                ShowMessage("Where does Node Add to?", "Add Notation");
                return;
            }
            else
            {
                DialogResult result = ShowMessage(
                    $"Add OK?\nNode[ {textBox.Text} ]", "Add Confirm");

                if(result == DialogResult.OK)
                {
                    tree.SelectedNode.Nodes.Add(new TreeNode(textBox.Text));
                }
                else
                {
                    return;
                }
            }
        }//buttonAdd_Click()

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode == null)
            {
                ShowMessage("Where does Node Remove from?", "Remove Notation");
                return;
            }
            else
            {
                DialogResult result = ShowMessage(
                    $"Remove OK?\nNode[ {tree.SelectedNode.Text} ]", "Remove Confirm");

                if (result == DialogResult.OK)
                {
                    tree.SelectedNode.Nodes.Remove(tree.SelectedNode);
                }
                else
                {
                    return;
                }
            }
        }//buttonRemove_Click()

        private void buttonCollapse_Click(object sender, EventArgs e)
        {
            tree.CollapseAll();
        }

        private void buttonExpand_Click(object sender, EventArgs e)
        {
            tree.ExpandAll();
        }

        private DialogResult ShowMessage(string text, string caption)
        {
            DialogResult result = MessageBox.Show(
                text, 
                caption, 
                MessageBoxButtons.OKCancel, 
                MessageBoxIcon.Warning);

            return result;
        }//ShowMessage()

        private void BuildInitialNode()
        {
            TreeNode treeNode1 = new TreeNode("第5011機械工兵中隊");
            TreeNode treeNode2 = new TreeNode("第501工兵中隊",
                new TreeNode[] {treeNode1});
            TreeNode treeNode3 = new TreeNode("第１装甲師団 (グデーリアン)",
                new TreeNode[] {treeNode2});
            TreeNode treeNode4 = new TreeNode("第502工兵中隊");
            TreeNode treeNode5 = new TreeNode("第２装甲師団 (マンシュタイン)",
                new TreeNode[] {treeNode4});
            TreeNode treeNode6 = new TreeNode("自動車衛生中隊");
            TreeNode treeNode7 = new TreeNode("自動車補給中隊");
            TreeNode treeNode8 = new TreeNode("第51自動車歩兵師団 (リスト)",
                new TreeNode[] {treeNode6,　treeNode7});
            TreeNode treeNode9 = new TreeNode("A軍集団 (ルントテンシュタット)",
                new TreeNode[] {treeNode3, treeNode5, treeNode8});
            TreeNode treeNode10 = new TreeNode("第10装甲師団 (モーゼル)");
            TreeNode treeNode11 = new TreeNode("第101自動車歩兵師団 (ハインリヒ)");
            TreeNode treeNode12 = new TreeNode("B軍集団 (ポック)",
                new TreeNode[] {treeNode10, treeNode11});
            TreeNode treeNode13 = new TreeNode("第32軽戦車旅団 (エメリッヒ)");
            TreeNode treeNode14 = new TreeNode("衛生中隊");
            TreeNode treeNode15 = new TreeNode("補給中隊");
            TreeNode treeNode16 = new TreeNode("第171歩兵師団 (メッサーシュミット)",
                new TreeNode[] {treeNode14, treeNode15});
            TreeNode treeNode17 = new TreeNode("DAK (ロンメル)", 
                new TreeNode[] {treeNode13, treeNode16});

            tree.Nodes.Add(treeNode9);  //A軍集団
            tree.Nodes.Add(treeNode12); //B軍集団
            tree.Nodes.Add(treeNode17); //DAK
        }//BuildInitialNode()
    }//class
}
