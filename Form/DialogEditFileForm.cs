using ProjectEnc.Helper;
using System;
using System.IO;
using System.Text;

namespace ProjectEnc.Form
{
    public partial class DialogEditFileForm : DevExpress.XtraEditors.XtraForm
    {
        private readonly string projectName;

        private string protectPath, packagePath;

        public DialogEditFileForm(string projectName = null)
        {
            InitializeComponent();

            this.projectName = projectName;
        }

        private void DialogEditFileForm_Load(object sender, EventArgs e)
        {
            string basePath = FileHelper.GetCurrentPath();
            string templateDir = this.projectName == null
                ? Path.Combine(basePath, "Template")
                : Path.Combine(basePath, "Projects", this.projectName);

            protectPath = Path.Combine(templateDir, "工程加密.crproj");
            packagePath = Path.Combine(templateDir, "工程打包.iss");

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