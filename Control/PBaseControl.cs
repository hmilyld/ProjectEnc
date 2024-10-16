using ProjectEnc.Bean;
using ProjectEnc.Helper;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ProjectEnc.Control
{
    public partial class PBaseControl : DevExpress.XtraEditors.XtraUserControl
    {
        private Project project;

        public Action RefreshFiles { get; private set; }

        public PBaseControl()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
        }

        public void LoadProject(Project project, Action refreshFiles)
        {
            this.project = project;

            this.ProjectNameTxt.Text = project.Name;
            this.PathTxt.Text = project.RootPath;
            this.ExePath.Text = project.ExeFile;
            this.InstallPathTxt.Text = project.PackagePath;

            this.SetVersion(project.ExeFile);

            this.RefreshFiles = refreshFiles;
        }

        private void PathTxt_Click(object sender, EventArgs e)
        {
            string path = this.ShowFoler();
            if (!string.IsNullOrEmpty(path))
            {
                this.PathTxt.Text = path;
                this.ExePath.Text = string.Empty;

                this.SetVersion(null);

                if (this.project != null)
                {
                    this.project.RootPath = path;
                    this.project.ExeFile = string.Empty;

                    this.RefreshFiles.Invoke();
                }
            }
        }

        private void ExePath_Click(object sender, EventArgs e)
        {
            string file = this.ShowFile();
            if (!string.IsNullOrEmpty(file))
            {
                this.PathTxt.Text = Path.GetDirectoryName(file);
                this.ExePath.Text = file;

                this.SetVersion(file);

                if (this.project != null)
                {
                    this.project.RootPath = Path.GetDirectoryName(file);
                    this.project.ExeFile = file;

                    this.RefreshFiles.Invoke();
                }
            }
        }

        private void InstallPathTxt_Click(object sender, EventArgs e)
        {
            string path = this.ShowFoler();
            if (!string.IsNullOrEmpty(path))
            {
                this.InstallPathTxt.Text = path;
            }
        }

        internal Project Save()
        {
            string name = this.ProjectNameTxt.Text;
            string path = this.PathTxt.Text;
            string exePath = this.ExePath.Text;
            string installPath = this.InstallPathTxt.Text;

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(exePath) && !string.IsNullOrEmpty(installPath))
            {
                Project project = new Project()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    RootPath = path,
                    ExeFile = exePath,
                    PackagePath = installPath
                };
                return project;
            }
            else
            {
                MsgHelper.Error("请确认路径是否填写完整");
                return null;
            }
        }

        private string ShowFoler()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "请选择一个文件夹";
                folderDialog.ShowNewFolderButton = true;

                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    return folderDialog.SelectedPath;
                }
                else
                {
                    return "";
                }
            }
        }

        private string ShowFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
                openFileDialog.Title = "请选择一个可执行文件";
                openFileDialog.Multiselect = false;
                if (!string.IsNullOrEmpty(this.PathTxt.Text))
                {
                    openFileDialog.InitialDirectory = this.PathTxt.Text;
                }

                DialogResult result = openFileDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
                {
                    return openFileDialog.FileName;
                }
                else
                {
                    return "";
                }
            }
        }

        private void SetVersion(string file)
        {
            if (file == null)
            {
                this.ExeVerTxt.Text = string.Empty;
                this.InstallVerTxt.Text = string.Empty;
                return;
            }
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(file);
            this.ExeVerTxt.Text = fileVersionInfo.ProductVersion;
            this.InstallVerTxt.Text = fileVersionInfo.ProductVersion;
        }
    }
}