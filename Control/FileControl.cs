using DevExpress.XtraTreeList.Nodes;
using ProjectEnc.Bean;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ProjectEnc.Control
{
    public partial class FileControl : DevExpress.XtraEditors.XtraUserControl
    {
        private Project project;

        private List<string> list = new List<string>();

        private readonly string type;

        public FileControl(string type)
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            this.type = type;

            this.treeList.OptionsSelection.MultiSelect = true;
            this.treeList.OptionsView.ShowCheckBoxes = true;

            this.treeList.SelectImageList = this.imageCollection;

            this.treeList.AfterCheckNode += TreeList_AfterCheckNode;
        }

        internal void LoadProject(Project project)
        {
            this.project = project;

            this.list = type == "merge" ? this.project.MergeDlls : this.project.PackageIgnores;

            this.treeList.Nodes.Clear();
            this.LoadDirectoryToTreeList(project.RootPath, null);
        }

        private void LoadDirectoryToTreeList(string folderPath, TreeListNode parentNode)
        {
            string[] directories = Directory.GetDirectories(folderPath);
            foreach (string directory in directories)
            {
                string dirName = Path.GetFileName(directory);
                TreeListNode dirNode = treeList.AppendNode(new object[] { dirName }, parentNode);
                dirNode.ImageIndex = 0;
                dirNode.SelectImageIndex = 0;
                dirNode.Tag = directory;
                LoadDirectoryToTreeList(directory, dirNode);

                if (this.list.Contains(directory))
                {
                    dirNode.Checked = true;
                }
            }
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                TreeListNode fileNode = treeList.AppendNode(new object[] { fileName }, parentNode);
                fileNode.ImageIndex = 1;
                fileNode.SelectImageIndex = 1;
                fileNode.Tag = file;

                if (this.list.Contains(file))
                {
                    fileNode.Checked = true;
                }
            }
        }

        #region TreeList联动事件

        // 处理选中节点事件
        private void TreeList_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            // 阻止事件递归
            treeList.BeginUpdate();

            // 向下联动 - 选中所有子节点
            CheckAllChildNodes(e.Node, e.Node.CheckState);

            // 向上联动 - 更新父节点状态
            CheckParentNodes(e.Node.ParentNode, e.Node.CheckState);

            treeList.EndUpdate();

            this.UpdateSelectFiles();
        }

        private void CheckParentNodes(TreeListNode node, CheckState check)
        {
            if (node == null)
                return;

            CheckState parentState = GetParentCheckState(node);
            node.CheckState = parentState;

            CheckParentNodes(node.ParentNode, parentState);
        }

        private CheckState GetParentCheckState(TreeListNode node)
        {
            bool allChecked = true;
            bool allUnchecked = true;

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                if (node.Nodes[i].CheckState == CheckState.Checked ||
                    node.Nodes[i].CheckState == CheckState.Indeterminate)
                    allUnchecked = false;
                if (node.Nodes[i].CheckState == CheckState.Unchecked ||
                    node.Nodes[i].CheckState == CheckState.Indeterminate)
                    allChecked = false;
            }

            if (allChecked)
                return CheckState.Checked;
            if (allUnchecked)
                return CheckState.Unchecked;

            return CheckState.Indeterminate;
        }

        private void CheckAllChildNodes(TreeListNode node, CheckState check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                CheckAllChildNodes(node.Nodes[i], check);
            }
        }

        private void UpdateSelectFiles()
        {
            this.list.Clear();

            foreach (TreeListNode node in treeList.GetAllCheckedNodes())
            {
                this.list.Add((string)node.Tag);
            }
        }

        #endregion TreeList联动事件
    }
}