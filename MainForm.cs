using DevExpress.XtraTreeList;
using ProjectEnc.Bean;
using ProjectEnc.Control;
using ProjectEnc.Form;
using ProjectEnc.Helper;
using ProjectEnc.Service;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectEnc
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        private readonly string projectRootPath = Path.Combine(FileHelper.GetCurrentPath(), AppConstants.ProjectsFolder);

        private readonly ProjectControl projectControl = new ProjectControl();

        private readonly ProjectService projectService = new ProjectService();

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
            if (!Directory.Exists(this.projectRootPath))
            {
                Directory.CreateDirectory(this.projectRootPath);
            }

            this.LoadProjects();
        }

        private void LoadProjects()
        {
            this.treeList.Nodes.Clear();
            string[] dirs = Directory.GetDirectories(this.projectRootPath);

            foreach (var dir in dirs)
            {
                // 使用完整目录名，避免工程名含“.”时被 GetFileNameWithoutExtension 截断
                string dirName = Path.GetFileName(dir);
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
                try
                {
                    this.project.Save();
                    MsgHelper.Show("工程保存成功");
                }
                catch (Exception ex)
                {
                    MsgHelper.Error(ex);
                }
            }
        }

        private void DelBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var selectedNodes = this.treeList.GetNodeList().ToList()
                .Where(node => node.CheckState == CheckState.Checked)
                .ToList();

            if (!selectedNodes.Any())
            {
                MsgHelper.Warn("请先勾选要删除的工程");
                return;
            }

            if (MsgHelper.ConfirmDelete() != DialogResult.Yes)
            {
                return;
            }

            try
            {
                foreach (var node in selectedNodes)
                {
                    string dirName = node.GetValue(0).ToString();
                    FileHelper.Delete(Path.Combine(this.projectRootPath, dirName));
                }

                MsgHelper.Success();
                this.LoadProjects();
            }
            catch (Exception ex)
            {
                MsgHelper.Error(ex);
            }
        }

        private async void MergeBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.project == null)
            {
                MsgHelper.Warn("当前没有打开工程");
                return;
            }

            await this.RunWithLoadingAsync(() => this.projectService.MergeAsync(this.project, this.progress));
        }

        private async void ProtectBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.project == null)
            {
                MsgHelper.Warn("当前没有打开工程");
                return;
            }

            await this.RunWithLoadingAsync(() => this.projectService.ProtectAsync(this.project, this.progress));
        }

        private async void PackageBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.project == null)
            {
                MsgHelper.Warn("当前没有打开工程");
                return;
            }

            await this.RunWithLoadingAsync(() => this.projectService.PackageAsync(this.project, this.progress));
        }

        private void TreeList_DoubleClick(object sender, EventArgs e)
        {
            TreeListHitInfo hitInfo = treeList.CalcHitInfo(treeList.PointToClient(MousePosition));

            if (hitInfo.Node != null)
            {
                string dirName = hitInfo.Node.GetValue(0).ToString();
                this.project = ProjectSerializer.Load(Path.Combine(this.projectRootPath, dirName, AppConstants.ProjectFileName));
                if (this.project == null)
                {
                    MsgHelper.Error("工程文件不存在或已损坏，无法打开。");
                    return;
                }

                if (!File.Exists(this.project.ExeFile))
                {
                    MsgHelper.Error("主程序文件不存在，请检查！");
                    return;
                }

                LoadingHelper.Show();
                try
                {
                    this.OpenProject();
                }
                finally
                {
                    LoadingHelper.Hide();
                }
            }
        }

        private void EditProjectTemplateBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.project != null)
            {
                DialogEditFileForm form = new DialogEditFileForm(this.project.FolderNameOrName);
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
            this.OpenDirectory(() => Path.Combine(this.projectRootPath, this.project.FolderNameOrName));
        }

        private void OpenProjectItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.OpenDirectory(() => this.project.RootPath);
        }

        private void OpenOutPutItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.OpenDirectory(() => this.project.PackagePath);
        }

        private void HelpBtnItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MsgHelper.Show("很懒，不想写……");
        }

        private void OpenProject()
        {
            this.projectControl.Enabled = true;
            this.projectControl.LoadProject(this.project);

            this.SetBarItemStatus(true);
        }

        /// <summary>
        /// 打开资源管理器定位到指定目录；未打开工程或目录不存在时给出提示。
        /// </summary>
        private void OpenDirectory(Func<string> pathSelector)
        {
            if (this.project == null)
            {
                MsgHelper.Warn("当前没有打开工程");
                return;
            }

            string path = pathSelector();
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                MsgHelper.Warn("目录不存在");
                return;
            }

            Process.Start("explorer.exe", ProcessRunner.Quote(path));
        }

        /// <summary>
        /// 在加载遮罩下执行异步操作，统一处理异常并保证遮罩关闭。
        /// </summary>
        private async Task RunWithLoadingAsync(Func<Task> action)
        {
            LoadingHelper.Show();
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                MsgHelper.Error(ex);
            }
            finally
            {
                LoadingHelper.Hide();
            }
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
