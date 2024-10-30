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
            this.passwdTxt.Text = project.Password;

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

                    this.RefreshFiles();
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

                    this.RefreshFiles();
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

        private void ProjectNameTxt_EditValueChanged(object sender, EventArgs e)
        {
            if (this.project != null)
            {
                this.project.Name = this.ProjectNameTxt.Text;
            }
        }

        private void PasswdTxt_EditValueChanged(object sender, EventArgs e)
        {
            if (this.project != null)
            {
                this.project.Password = this.passwdTxt.Text;
            }
        }

        private void InstallPathTxt_EditValueChanged(object sender, EventArgs e)
        {
            if (this.project != null)
            {
                this.project.PackagePath = this.InstallPathTxt.Text;
            }
        }

        internal Project Save()
        {
            string name = this.ProjectNameTxt.Text;
            string path = this.PathTxt.Text;
            string exePath = this.ExePath.Text;
            string installPath = this.InstallPathTxt.Text;
            string passwd = this.passwdTxt.Text;

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(exePath) && !string.IsNullOrEmpty(installPath))
            {
                Project project = new Project()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    RootPath = path,
                    ExeFile = exePath,
                    PackagePath = installPath,
                    Password = passwd
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