using DevExpress.XtraTreeList;
using ProjectEnc.Bean;
using ProjectEnc.Control;
using ProjectEnc.Form;
using ProjectEnc.Helper;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProjectEnc
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        private readonly string ProjectPath = Path.Combine(FileHelper.GetCurrentPath(), "Projects");

        private readonly ProjectControl projectControl = new ProjectControl();

        private readonly IProgress<string> progress;

        private Project project;

        public MainForm()
        {
            InitializeComponent();

            this.tablePanel1.Columns[0].Width = (int)(Screen.PrimaryScreen.WorkingArea.Width * 0.125);

            this.projectControl.Enabled = false;

            tablePanel1.SetColumn(this.projectControl, 1);
            tablePanel1.SetRow(this.projectControl, 0);
            tablePanel1.Controls.Add(this.projectControl);

            this.treeList.OptionsSelection.MultiSelect = true;
            this.treeList.OptionsView.ShowCheckBoxes = true;

            this.progress = new Progress<string>(ReportProgress);

            this.SetBarItemStatus(false);
        }

        private void SetBarItemStatus(bool status)
        {
            this.SaveBtnItem.Enabled = status;
            this.OpenBtnItem.Enabled = status;
            this.EditProjectTemplateBtnItem.Enabled = status;
            this.MergeBtnItem.Enabled = status;
            this.ProtectBtnItem.Enabled = status;
            this.PackageBtnItem.Enabled = status;
            this.OpenProjectItem.Enabled = status;
            this.OpenOutPutItem.Enabled = status;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(this.ProjectPath))
            {
                Directory.CreateDirectory(this.ProjectPath);
            }

            this.LoadProjects();
        }

        private void LoadProjects()
        {
            this.treeList.Nodes.Clear();
            string[] dirs = Directory.GetDirectories(this.ProjectPath);

            foreach (var dir in dirs)
            {
                string dirName = Path.GetFileNameWithoutExtension(dir);
                this.treeList.AppendNode(new object[] { dirName }, -1);
            }
            this.treeList.ExpandAll();
        }

        private void NewBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogNewForm dialog = new DialogNewForm();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.project = dialog.Project;

                this.LoadProjects();

                this.OpenProject();

                MsgHelper.Show("工程新建成功");
            }
        }

        private void SaveBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.project != null)
            {
                FileHelper.Serialize(Path.Combine(this.ProjectPath, this.project.Name, "工程.project"), this.project);
                MsgHelper.Show("工程保存成功");
            }
        }

        private void DelBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MsgHelper.ConfirmDelete() == DialogResult.Yes)
            {
                var selectedNodes = this.treeList.GetNodeList().ToList().Where(node => node.CheckState == CheckState.Checked);

                foreach (var node in selectedNodes)
                {
                    string dirName = node.GetValue(0).ToString();
                    FileHelper.Delete(Path.Combine(this.ProjectPath, dirName));
                }
                MsgHelper.Success();
                this.LoadProjects();
            }
        }

        private void MergeBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadingHelper.Show();
            try
            {
                this.project.Merge();
            }
            catch (Exception ex)
            {
                MsgHelper.Error(ex);
            }
            LoadingHelper.Hide();
        }

        private void ProtectBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadingHelper.Show();
            try
            {
                this.project.Protect();
            }
            catch (Exception ex)
            {
                MsgHelper.Error(ex);
            }
            LoadingHelper.Hide();
        }

        private void PackageBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadingHelper.Show();
            try
            {
                this.project.Package();
            }
            catch (Exception ex)
            {
                MsgHelper.Error(ex);
            }
            LoadingHelper.Hide();
        }

        private void TreeList_DoubleClick(object sender, EventArgs e)
        {
            TreeListHitInfo hitInfo = treeList.CalcHitInfo(treeList.PointToClient(MousePosition));

            if (hitInfo.Node != null)
            {
                LoadingHelper.Show();
                string dirName = hitInfo.Node.GetValue(0).ToString();
                this.project = FileHelper.Deserialize<Project>(Path.Combine(this.ProjectPath, dirName, "工程.project"));
                this.OpenProject();
                LoadingHelper.Hide();
            }
        }

        private void EditProjectTemplateBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.project != null)
            {
                DialogEditFileForm form = new DialogEditFileForm(this.project.Name);
                form.ShowDialog();
            }
            else
            {
                MsgHelper.Warn("当前没有打开工程");
            }
        }

        private void EditTemplateBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogEditFileForm form = new DialogEditFileForm();
            form.ShowDialog();
        }

        private void OpenBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.project != null)
            {
                Process.Start("explorer.exe", Path.Combine(this.ProjectPath, this.project.Name));
            }
            else
            {
                MsgHelper.Warn("当前没有打开工程");
            }
        }

        private void OpenProjectItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.project != null)
            {
                Process.Start("explorer.exe", this.project.RootPath);
            }
            else
            {
                MsgHelper.Warn("当前没有打开工程");
            }
        }

        private void OpenOutPutItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.project != null)
            {
                Process.Start("explorer.exe", this.project.PackagePath);
            }
            else
            {
                MsgHelper.Warn("当前没有打开工程");
            }
        }

        private void HelpBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MsgHelper.Show("很懒，不想写……");
        }

        private void OpenProject()
        {
            this.project.SetProgress(this.progress);
            this.projectControl.Enabled = true;
            this.projectControl.LoadProject(this.project);

            this.SetBarItemStatus(true);
        }

        private void ReportProgress(string info)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(ReportProgress), info);
            }
            else
            {
                this.LogEdit.AppendLine(info);
            }
        }
    }
}