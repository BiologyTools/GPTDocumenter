using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;
using OpenAI;

namespace GPTDocumenter
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        List<Block> blocks = new List<Block>();
        public class Block
        {
            public string Path { get; set; }
            public string[] Content
            {
                get
                {
                    bool end = false;
                    List<string> list = new List<string>();
                    string[] ll = File.ReadAllLines(Path);
                    int br = 0;
                    for (int i = Location; i < ll.Length; i++)
                    {
                        string con = "";
                        for (int j = 0; j < ll[i].Length; j++)
                        {
                            con += ll[i][j];
                            if (ll[i][j] == '{')
                            {
                                br++;
                            }
                            if (ll[i][j] == '}')
                            {
                                br--;
                                if (br == 0)
                                {
                                    //End of block
                                    end = true;
                                    break;
                                }
                            }
                        }
                        list.Add(con);
                        if (end)
                            break;
                    }
                    return list.ToArray();
                }
            }
            public string[] Documentation
            {
                get
                {
                    List<string> list = new List<string>();
                    string[] ll = File.ReadAllLines(Path);
                    for (int i = Location; i > 0; i--)
                    {
                        if (ll[i - 1].Contains("///"))
                            list.Add(ll[i - 1]);
                        else
                            break;
                    }
                    list.Reverse();
                    return list.ToArray();
                }
                set
                {
                    List<string> sts = new List<string>();
                    string[] ll = File.ReadAllLines(Path);
                    sts.AddRange(ll);
                    int i = 0;
                    foreach (string s in value)
                    {
                        if (s.Contains("///"))
                        {
                            sts.Insert(Location + i, s);
                            i++;
                        }
                    }
                    File.WriteAllLines(Path, sts.ToArray());
                }
            }
            public Block Parent { get; set; }
            public string Header { get; set; }
            public List<Block> Children = new List<Block>();
            public int Location { get; set; }
            public bool isPublic
            {
                get
                {
                    return Header.Contains("public");
                }
            }
            public Block(string file)
            {
                Block main = new Block();
                Block b = new Block();
                string[] ls = File.ReadAllLines(file);
                int br = 0;
                for (int i = 0; i < ls.Length; i++)
                {
                    b.Path = file;
                    for (int j = 0; j < ls[i].Length; j++)
                    {
                        if (ls[i][j] == '{')
                        {
                            if (br == 0)
                            {
                                //This is a namespace
                                b.Header = ls[i - 1];
                                b.Location = i - 1;
                                main = b;
                            }
                            else
                            {
                                Block bl = new Block();
                                //This a new block of code we will add it to it's parent
                                bl.Header = ls[i - 1];
                                bl.Location = i - 1;
                                bl.Parent = b;
                                b.Children.Add(bl);
                                b = bl;
                            }
                            br++;
                        }
                        else
                        if (ls[i][j] == '}')
                        {
                            br--;
                            if (br == 0)
                            {
                                //End of namespace
                                return;
                            }
                            else
                            {
                                b = b.Parent;
                            }
                        }
                    }
                }
            }
            public Block()
            {

            }
            public override string ToString()
            {
                return Header.ToString();
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Block b = (Block)e.Node.Tag;
            blockBox.Lines = b.Content;
            docBox.Lines = b.Documentation;
            if (e.Node.Nodes.Count == 0)
            foreach (var item in b.Children)
            {
                TreeNode n = new TreeNode();
                n.Tag = item;
                n.Text = item.Header;
                e.Node.Nodes.Add(n);
            }
        }

        private async Task<string> Generate(string code)
        {
            var openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = apiBox.Text
            });
            var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("You are a C# code documenter reply only in XML documentation format without source code."),
                    ChatMessage.FromUser(code),
                },
                Model = Models.Gpt_3_5_Turbo,
            });
            if (completionResult.Successful)
            {
                return completionResult.Choices.First().Message.Content;
            }
            else
            {
                MessageBox.Show(completionResult.Error.Message, "OpenAI Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(completionResult.Error.Message);
            }
            return null;
        }

        async void GenerateSelected(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                Block b = (Block)node.Tag;
                if (node.Checked)
                {
                    if (anyRBut.Checked || (publicRBut.Checked && b.isPublic))
                    {
                        string con = "";
                        foreach (string s in b.Content) { con += s + Environment.NewLine; }
                        string doc = await Generate(con);
                        string[] ds = doc.Split('\n');
                        b.Documentation = ds;
                        node.Tag = b;
                    }
                }
                // If the node has child nodes, recursively call this method
                if (node.Nodes.Count > 0)
                {
                    GenerateSelected(node.Nodes);
                }
            }
        }

        async void GenerateAll(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                Block b = (Block)node.Tag;
                if (anyRBut.Checked || (publicRBut.Checked && b.isPublic))
                {
                    string con = "";
                    foreach (string s in b.Content) { con += s + Environment.NewLine; }
                    string doc = await Generate(con);
                    string[] ds = doc.Split('\n');
                    b.Documentation = ds;
                    node.Tag = b;
                }
                // If the node has child nodes, recursively call this method
                if (node.Nodes.Count > 0)
                {
                    GenerateAll(node.Nodes);
                }
            }
        }

        private void UpdateItems(TreeNodeCollection ns)
        {
            foreach (TreeNode node in ns)
            {
                Block b = (Block)node.Tag;
                node.Tag = new Block(b.Path);
                // If the node has child nodes, recursively call this method
                if (node.Nodes.Count > 0)
                {
                    UpdateItems(node.Nodes);
                }
            }
        }

        private async void genSelBut_Click(object sender, EventArgs e)
        {
            GenerateSelected(treeView.Nodes);
        }

        private void genAllBut_Click(object sender, EventArgs e)
        {
            GenerateAll(treeView.Nodes);
        }

        private void openFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
            foreach (string item in Directory.GetFiles(folderBrowserDialog.SelectedPath))
            {
                blocks.Add(new Block(item));
            }
            foreach (Block b in blocks)
            {
                TreeNode tn = new TreeNode();
                tn.Tag = b;
                tn.Text = Path.GetFileName(b.Path);
                treeView.Nodes.Add(tn);
            }
        }

        private void treeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            foreach (TreeNode n in e.Node.Nodes)
            {
                n.Checked = e.Node.Checked;
            }
        }

        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            foreach (string item in openFileDialog.FileNames)
            {
                blocks.Add(new Block(item));
            }
            foreach (Block b in blocks)
            {
                TreeNode tn = new TreeNode();
                tn.Tag = b;
                tn.Text = Path.GetFileName(b.Path);
                treeView.Nodes.Add(tn);
            }
        }

        private void clearFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView.Nodes.Clear();
            blocks.Clear();
        }
    }
}
