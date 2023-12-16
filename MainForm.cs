using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;
using OpenAI;
using System.Xml.Linq;
using System.Data;
using System.Dynamic;
using static System.Net.Mime.MediaTypeNames;

namespace GPTDocumenter
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        List<Block> blocks = new List<Block>();
        public class Block : TreeNode
        {
            public enum ProcedureKeywords
            {
                @as,
                @break,
                @case,
                @catch,
                @checked,
                @continue,
                @default,
                @event,
                @finally,
                @fixed,
                @for,
                @foreach,
                @goto,
                @if,
                @else,
                @in,
                @is,
                @lock,
                @out,
                @return,
                @sizeof,
                @stackalloc,
                @switch,
                @this,
                @throw,
                @try,
                @typeof,
                @unchecked,
                @using,
                @while,

                @add,
                @and,
                @ascending,
                @await,
                @by,
                @descending,
                @equals,
                @file,
                @from,
                @get,
                @group,
                @init,
                @into,
                @join,
                @let,
                @managed,
                @nameof,
                @not,
                @notnull,
                @on,
                @or,
                @orderby,
                @remove,
                @select,
                @set,
                @value,
                @when,
                @where,
                @with,
                @yield,
            }
            public enum FuncKeywords
            {
                @abstract,
                @base,
                @char,
                @class,
                @const,
                @decimal,
                @double,
                @enum,
                @event,
                @explicit,
                @extern,
                @float,
                @implicit,
                @int,
                @interface,
                @internal,
                @lock,
                @long,
                @namespace,
                @new,
                @object,
                @operator,
                @override,
                @private,
                @protected,
                @public,
                @readonly,
                @sbyte,
                @sealed,
                @short,
                @static,
                @string,
                @struct,
                @switch,
                @uint,
                @ulong,
                @unsafe,
                @ushort,
                @virtual,
                @void,
                @volatile,
                @while,
                //Contextual
                @alias,
                @args,
                @async,
                @dynamic,
                @file,
                @global,
                @managed,
                @nint,
                @notnull,
                @nuint,
                @partial,
                @record,
                @required,
                @scoped,
                @unmanaged,
                @var,
            }
            public string Path { get; set; }
            private string[] cache;
            private bool update = false;
            public string[] Content
            {
                get
                {
                    if (isProcedure) return null;
                    bool end = false;
                    List<string> list = new List<string>();
                    if (cache == null || update)
                        cache = File.ReadAllLines(Path);
                    int br = 0;
                    for (int i = Location; i < cache.Length; i++)
                    {
                        string con = "";
                        for (int j = 0; j < cache[i].Length; j++)
                        {
                            con += cache[i][j];
                            if (cache[i][j] == '{')
                            {
                                br++;
                            }
                            if (cache[i][j] == '}')
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
                    if (isProcedure) return null;
                    List<string> list = new List<string>();
                    if (cache == null || update)
                        cache = File.ReadAllLines(Path);
                    for (int i = Location; i > 0; i--)
                    {
                        if (cache[i - 1].Contains("///"))
                            list.Add(cache[i - 1]);
                        else
                            break;
                    }
                    list.Reverse();
                    return list.ToArray();
                }
                set
                {
                    if (isProcedure) return;
                    List<string> sts = new List<string>();
                    int dl = Documentation.Length;
                    if (dl > 0)
                    {
                        //We need to remove the old documentation and replace it. 
                        List<string> ss = new List<string>();
                        ss.AddRange(cache);
                        int c = 0;
                        for (int l = Location; l < Location + dl; l++)
                        {
                            ss.RemoveAt(Location - dl);
                        }
                        sts = ss;
                        Location -= dl;
                    }
                    else
                    {
                        string[] ll = File.ReadAllLines(Path);
                        sts.AddRange(ll);
                    }
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
                    cache = sts.ToArray();
                    Location += i;
                }
            }
            public int Location { get; set; }
            public bool IsFile = false;
            public bool IsPublic
            {
                get
                {
                    return Text.Contains("public");
                }
            }
            public bool isProcedure
            {
                get
                {
                    foreach (ProcedureKeywords aw in (ProcedureKeywords[])Enum.GetValues(typeof(ProcedureKeywords)))
                    {
                        string[] words = Text.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries);
                        if (words.Contains(aw.ToString()))
                            return true;
                    }
                    return false;
                }
            }
            public Block(string file)
            {
                Path = file;
                Block main = new Block();
                main.Path = file;
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
                                b.Text = ls[i - 1];
                                b.Location = i - 1;
                                main = b;
                            }
                            else
                            {
                                Block bl = new Block();
                                //This a new block of code we will add it to it's parent
                                bl.Text = ls[i - 1];
                                bl.Location = i - 1;
                                bl.Path = file;
                                b.Nodes.Add(bl);
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
                                Path = main.Path;
                                Nodes.Clear();
                                TreeNode[] tns = new TreeNode[main.Nodes.Count];
                                for (int t = 0; t < main.Nodes.Count; t++)
                                {
                                    tns[t] = main.Nodes[t];
                                }
                                Nodes.AddRange(tns);
                                Text = main.Text;
                                Location = main.Location;
                                return;
                            }
                            else
                            {
                                b = (Block)b.Parent;
                            }
                        }
                    }
                }
                Path = main.Path;
                TreeNode[] bls = new TreeNode[main.Nodes.Count];
                for (int t = 0; t < main.Nodes.Count; t++)
                {
                    bls[t] = main.Nodes[t];
                }
                Nodes.AddRange(bls);
                Text = main.Text;
                Location = main.Location;
            }
            public Block()
            {

            }
            public void Update()
            {
                cache = File.ReadAllLines(Path);
                foreach (Block b in Nodes)
                {
                    b.Update();
                }
            }
            public override string ToString()
            {
                return Text.ToString();
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
            foreach (Block b in nodes)
            {
                if (!b.IsFile)
                    if (b.Checked)
                    {
                        if (anyRBut.Checked || (publicRBut.Checked && b.IsPublic))
                        {
                            string con = "";
                            foreach (string s in b.Content) { con += s + Environment.NewLine; }
                            string doc = await Generate(con);
                            string sp = "\r\n";
                            string[] ds;
                            if(doc.Contains(sp))
                                ds = doc.Split(sp);
                            else
                                ds = doc.Split('\n');
                            b.Documentation = ds;
                            b.Update();
                        }
                    }
                // If the node has child nodes, recursively call this method
                if (b.Nodes.Count > 0)
                {
                    GenerateSelected(b.Nodes);
                }
            }
        }

        async void GenerateAll(TreeNodeCollection nodes)
        {
            foreach (Block b in nodes)
            {
                if (!b.IsFile)
                    if (b.isProcedure)
                        if (anyRBut.Checked || (publicRBut.Checked && b.IsPublic))
                        {
                            string con = "";
                            foreach (string s in b.Content) { con += s + Environment.NewLine; }
                            string doc = await Generate(con);
                            string[] ds = doc.Split('\n');
                            b.Documentation = ds;
                            b.Update();
                        }
                // If the node has child nodes, recursively call this method
                if (b.Nodes.Count > 0)
                {
                    GenerateAll(b.Nodes);
                }
            }
        }
        Block Find(TreeNodeCollection nodes, Block bl)
        {
            foreach (Block b in nodes)
            {
                if (b == bl)
                    return b;
                // If the node has child nodes, recursively call this method
                if (b.Nodes.Count > 0)
                {
                    Find(b.Nodes, bl);
                }
            }
            return null;
        }
        List<Block> bls = new List<Block>();
        void RemoveInvalid(TreeNodeCollection nodes)
        {
            foreach (Block b in nodes)
            {
                if (!b.IsFile && b.isProcedure)
                    bls.Add(b);
                // If the node has child nodes, recursively call this method
                if (b.Nodes.Count > 0)
                {
                    RemoveInvalid(b.Nodes);
                }
            }
        }

        async void GenerateMissing(TreeNodeCollection nodes)
        {
            foreach (Block b in nodes)
            {
                if (!b.IsFile)
                    if (b.Documentation.Length == 0 && b.isProcedure)
                        if (anyRBut.Checked || (publicRBut.Checked && b.IsPublic))
                        {
                            string con = "";
                            foreach (string s in b.Content) { con += s + Environment.NewLine; }
                            string doc = await Generate(con);
                            string[] ds = doc.Split('\n');
                            b.Documentation = ds;
                            b.Update();
                        }
                // If the node has child nodes, recursively call this method
                if (b.Nodes.Count > 0)
                {
                    GenerateMissing(b.Nodes);
                }
            }
        }
        private void UpdateTreeview()
        {
            foreach (Block n in blocks)
            {
                treeView.Nodes.Add(n);
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
                if (item.EndsWith(".cs") && !item.ToLower().Contains(".designer"))
                {
                    Block b = new Block();
                    b.IsFile = true;
                    b.Text = Path.GetFileName(item);
                    b.Nodes.Add(new Block(item));
                    blocks.Add(b);
                }
            }
            UpdateTreeview();
        }

        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            foreach (string item in openFileDialog.FileNames)
            {
                Block b = new Block();
                b.IsFile = true;
                b.Text = Path.GetFileName(item);
                b.Nodes.Add(new Block(item));
                blocks.Add(b);
            }
            UpdateTreeview();
        }

        private void clearFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView.Nodes.Clear();
            blocks.Clear();
        }

        private void missingBut_Click(object sender, EventArgs e)
        {
            GenerateMissing(treeView.Nodes);
        }

        private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (((Block)e.Node).isProcedure)
                e.Cancel = true;
            docBox.Lines = ((Block)e.Node).Documentation;
            blockBox.Lines = ((Block)e.Node).Content;
        }

        private void treeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if(((Block)e.Node).isProcedure)
                e.Cancel= true;
        }
    }
}
