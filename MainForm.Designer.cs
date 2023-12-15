namespace GPTDocumenter
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            folderBrowserDialog = new FolderBrowserDialog();
            treeView = new TreeView();
            docBox = new RichTextBox();
            blockBox = new RichTextBox();
            splitContainer1 = new SplitContainer();
            anyRBut = new RadioButton();
            publicRBut = new RadioButton();
            label1 = new Label();
            splitContainer2 = new SplitContainer();
            apiBox = new MaskedTextBox();
            genAllBut = new Button();
            genSelBut = new Button();
            menuStrip1 = new MenuStrip();
            openFolderToolStripMenuItem = new ToolStripMenuItem();
            openFolderToolStripMenuItem1 = new ToolStripMenuItem();
            addFilesToolStripMenuItem = new ToolStripMenuItem();
            clearFilesToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // folderBrowserDialog
            // 
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.CommonDocuments;
            // 
            // treeView
            // 
            treeView.CheckBoxes = true;
            treeView.Dock = DockStyle.Fill;
            treeView.Location = new Point(0, 0);
            treeView.Name = "treeView";
            treeView.Size = new Size(278, 425);
            treeView.TabIndex = 0;
            treeView.BeforeCheck += treeView_BeforeCheck;
            treeView.AfterSelect += treeView_AfterSelect;
            // 
            // docBox
            // 
            docBox.Dock = DockStyle.Fill;
            docBox.Location = new Point(0, 0);
            docBox.Name = "docBox";
            docBox.Size = new Size(550, 184);
            docBox.TabIndex = 1;
            docBox.Text = "";
            // 
            // blockBox
            // 
            blockBox.Dock = DockStyle.Fill;
            blockBox.Location = new Point(0, 0);
            blockBox.Name = "blockBox";
            blockBox.Size = new Size(550, 184);
            blockBox.TabIndex = 2;
            blockBox.Text = "";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeView);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(anyRBut);
            splitContainer1.Panel2.Controls.Add(publicRBut);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Panel2.Controls.Add(apiBox);
            splitContainer1.Panel2.Controls.Add(genAllBut);
            splitContainer1.Panel2.Controls.Add(genSelBut);
            splitContainer1.Size = new Size(839, 425);
            splitContainer1.SplitterDistance = 278;
            splitContainer1.TabIndex = 3;
            // 
            // anyRBut
            // 
            anyRBut.AutoSize = true;
            anyRBut.Location = new Point(442, 27);
            anyRBut.Name = "anyRBut";
            anyRBut.Size = new Size(44, 19);
            anyRBut.TabIndex = 10;
            anyRBut.Text = "any";
            anyRBut.UseVisualStyleBackColor = true;
            // 
            // publicRBut
            // 
            publicRBut.AutoSize = true;
            publicRBut.Checked = true;
            publicRBut.Location = new Point(490, 27);
            publicRBut.Name = "publicRBut";
            publicRBut.Size = new Size(58, 19);
            publicRBut.TabIndex = 9;
            publicRBut.TabStop = true;
            publicRBut.Text = "public";
            publicRBut.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 7);
            label1.Name = "label1";
            label1.Size = new Size(47, 15);
            label1.TabIndex = 8;
            label1.Text = "API Key";
            // 
            // splitContainer2
            // 
            splitContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer2.Location = new Point(3, 50);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(docBox);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(blockBox);
            splitContainer2.Size = new Size(550, 372);
            splitContainer2.SplitterDistance = 184;
            splitContainer2.TabIndex = 7;
            // 
            // apiBox
            // 
            apiBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            apiBox.Location = new Point(56, 4);
            apiBox.Name = "apiBox";
            apiBox.PasswordChar = '*';
            apiBox.Size = new Size(288, 23);
            apiBox.TabIndex = 6;
            apiBox.UseSystemPasswordChar = true;
            // 
            // genAllBut
            // 
            genAllBut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            genAllBut.Location = new Point(466, 3);
            genAllBut.Name = "genAllBut";
            genAllBut.Size = new Size(86, 23);
            genAllBut.TabIndex = 5;
            genAllBut.Text = "Generate All";
            genAllBut.UseVisualStyleBackColor = true;
            genAllBut.Click += genAllBut_Click;
            // 
            // genSelBut
            // 
            genSelBut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            genSelBut.Location = new Point(350, 3);
            genSelBut.Name = "genSelBut";
            genSelBut.Size = new Size(110, 23);
            genSelBut.TabIndex = 4;
            genSelBut.Text = "Generate Selected";
            genSelBut.UseVisualStyleBackColor = true;
            genSelBut.Click += genSelBut_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { openFolderToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(839, 24);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            // 
            // openFolderToolStripMenuItem
            // 
            openFolderToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openFolderToolStripMenuItem1, addFilesToolStripMenuItem, clearFilesToolStripMenuItem });
            openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            openFolderToolStripMenuItem.Size = new Size(37, 20);
            openFolderToolStripMenuItem.Text = "File";
            // 
            // openFolderToolStripMenuItem1
            // 
            openFolderToolStripMenuItem1.Name = "openFolderToolStripMenuItem1";
            openFolderToolStripMenuItem1.Size = new Size(132, 22);
            openFolderToolStripMenuItem1.Text = "Add Folder";
            openFolderToolStripMenuItem1.Click += openFolderToolStripMenuItem1_Click;
            // 
            // addFilesToolStripMenuItem
            // 
            addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
            addFilesToolStripMenuItem.Size = new Size(132, 22);
            addFilesToolStripMenuItem.Text = "Add Files";
            addFilesToolStripMenuItem.Click += addFilesToolStripMenuItem_Click;
            // 
            // clearFilesToolStripMenuItem
            // 
            clearFilesToolStripMenuItem.Name = "clearFilesToolStripMenuItem";
            clearFilesToolStripMenuItem.Size = new Size(132, 22);
            clearFilesToolStripMenuItem.Text = "Clear Files";
            clearFilesToolStripMenuItem.Click += clearFilesToolStripMenuItem_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "C# Files|*.cs";
            openFileDialog.Multiselect = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(839, 449);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "GPT Documenter";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FolderBrowserDialog folderBrowserDialog;
        private TreeView treeView;
        private RichTextBox docBox;
        private RichTextBox blockBox;
        private SplitContainer splitContainer1;
        private MaskedTextBox apiBox;
        private Button genAllBut;
        private Button genSelBut;
        private SplitContainer splitContainer2;
        private Label label1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem openFolderToolStripMenuItem;
        private ToolStripMenuItem openFolderToolStripMenuItem1;
        private ToolStripMenuItem addFilesToolStripMenuItem;
        private OpenFileDialog openFileDialog;
        private ToolStripMenuItem clearFilesToolStripMenuItem;
        private RadioButton anyRBut;
        private RadioButton publicRBut;
    }
}
