using ProjectEnc.Helper;
using System;
using System.IO;

namespace ProjectEnc.Form
{
    public partial class DialogEditFileForm : DevExpress.XtraEditors.XtraForm
    {
        private readonly string folderName;

        private string protectPath, packagePath;

        /// <param name="folderName">工程文件夹名；为 null 时编辑软件默认模板。</param>
        public DialogEditFileForm(string folderName = null)
        {
            InitializeComponent();

            this.folderName = folderName;
        }

        private void DialogEditFileForm_Load(object sender, EventArgs e)
        {
            string basePath = FileHelper.GetCurrentPath();
            string templateDir = string.IsNullOrEmpty(this.folderName)
                ? Path.Combine(basePath, AppConstants.TemplateFolder)
                : Path.Combine(basePath, AppConstants.ProjectsFolder, this.folderName);

            protectPath = Path.Combine(templateDir, AppConstants.ConfuserTemplateFile);
            packagePath = Path.Combine(templateDir, AppConstants.InnoTemplateFile);

            try
            {
                this.ProtectEdit.Text = File.Exists(protectPath)
                    ? File.ReadAllText(protectPath, FileHelper.ConfuserTemplateEncoding)
                    : string.Empty;
                this.PackageEdit.Text = File.Exists(packagePath)
                    ? File.ReadAllText(packagePath, FileHelper.InnoTemplateEncoding)
                    : string.Empty;
            }
            catch (Exception ex)
            {
                MsgHelper.Error(ex);
            }
        }

        private void SaveBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                File.WriteAllText(this.protectPath, this.ProtectEdit.Text, FileHelper.ConfuserTemplateEncoding);
                File.WriteAllText(this.packagePath, this.PackageEdit.Text, FileHelper.InnoTemplateEncoding);
                MsgHelper.Success();
            }
            catch (Exception ex)
            {
                MsgHelper.Error(ex);
            }
        }
    }
}
