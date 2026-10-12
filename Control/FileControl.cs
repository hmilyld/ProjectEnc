using DevExpress.XtraTreeList.Nodes;
using ProjectEnc.Bean;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ProjectEnc.Control
{
    /// <summary>文件列表用途。</summary>
    public enum FileListKind
    {
        /// <summary>待合并类库列表。</summary>
        Merge,

        /// <summary>安装文件打包排除列表。</summary>
        Ignore
    }

    public partial class FileControl : DevExpress.XtraEditors.XtraUserControl
    {
        private readonly FileListKind kind;

        /// <summary>指向工程模型中的目标列表（作为输出）。</summary>
        private List<string> selectedList = new List<string>();

        /// <summary>选中路径的快速查找集合，替代 List.Contains 的 O(n) 线性查找。</summary>
        private readonly HashSet<string> selectedLookup = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>联动过程中置位，用于阻止 AfterCheckNode 事件递归。</summary>
        private bool propagating;

        /// <summary>批量勾选期间置位，期间跳过列表重建。</summary>
        private bool batchUpdating;

        private Project project;

        public FileControl(FileListKind kind)
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            this.kind = kind;

            this.treeList.OptionsSelection.MultiSelect = true;
            this.treeList.OptionsView.ShowCheckBoxes = true;

            this.treeList.SelectImageList = this.imageCollection;

            this.treeList.AfterCheckNode += TreeList_AfterCheckNode;
        }

        internal void LoadProject(Project project)
        {
            this.project = project;

            this.selectedList = this.kind == FileListKind.Merge
                ? this.project.MergeDlls
                : this.project.PackageIgnores;

            this.selectedLookup.Clear();
            foreach (string path in this.selectedList)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    this.selectedLookup.Add(path);
                }
            }

            this.treeList.Nodes.Clear();
            this.LoadDirectoryToTreeList(project.RootPath, null);
        }

        private void LoadDirectoryToTreeList(string folderPath, TreeListNode parentNode)
        {
            foreach (string directory in Directory.GetDirectories(folderPath))
            {
                TreeListNode dirNode = treeList.AppendNode(new object[] { Path.GetFileName(directory) }, parentNode);
                dirNode.ImageIndex = 0;
                dirNode.SelectImageIndex = 0;
                dirNode.Tag = directory;

                LoadDirectoryToTreeList(directory, dirNode);

                if (this.selectedLookup.Contains(directory))
                {
                    dirNode.Checked = true;
                }
            }

            foreach (string file in Directory.GetFiles(folderPath))
            {
                TreeListNode fileNode = treeList.AppendNode(new object[] { Path.GetFileName(file) }, parentNode);
                fileNode.ImageIndex = 1;
                fileNode.SelectImageIndex = 1;
                fileNode.Tag = file;

                if (this.selectedLookup.Contains(file))
                {
                    fileNode.Checked = true;
                }
            }
        }

        /// <summary>
        /// 按后缀名批量切换勾选状态（如 "dll" 或 ".dll"）。
        /// </summary>
        internal void SelectFiles(string ext)
        {
            ext = (ext ?? string.Empty).Trim().TrimStart('.');
            if (ext.Length == 0)
            {
                return;
            }

            this.batchUpdating = true;
            try
            {
                foreach (TreeListNode node in this.treeList.GetNodeList())
                {
                    string file = node.Tag as string;
                    if (!string.IsNullOrEmpty(file) && Path.GetExtension(file) == $".{ext}")
                    {
                        node.Checked = !node.Checked;
                    }
                }
            }
            finally
            {
                this.batchUpdating = false;
            }

            this.UpdateSelectFiles();
        }

        #region TreeList联动事件

        // 处理选中节点事件
        private void TreeList_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            // 阻止联动过程中的事件递归
            if (this.propagating)
            {
                return;
            }

            this.propagating = true;
            try
            {
                treeList.BeginUpdate();

                // 向下联动 - 选中所有子节点
                CheckAllChildNodes(e.Node, e.Node.CheckState);

                // 向上联动 - 更新父节点状态
                CheckParentNodes(e.Node.ParentNode, e.Node.CheckState);

                treeList.EndUpdate();
            }
            finally
            {
                this.propagating = false;
            }

            // 批量勾选期间只更新一次列表
            if (!this.batchUpdating)
            {
                this.UpdateSelectFiles();
            }
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
            this.selectedList.Clear();

            foreach (TreeListNode node in treeList.GetAllCheckedNodes())
            {
                string path = node.Tag as string;

                // 仅收集真实文件，避免目录路径混入合并/排除列表
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    this.selectedList.Add(path);
                }
            }
        }

        #endregion TreeList联动事件
    }
}
