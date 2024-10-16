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
            if (this.projectName == null)
            {
                // 读取软件默认配置文件
                protectPath = Path.Combine(basePath, "Template", "工程加密.crproj");
                packagePath = Path.Combine(basePath, "Template", "工程打包.iss");
            }
            else
            {
                // 读取工程配置文件
                protectPath = Path.Combine(basePath, "Projects", this.projectName, "工程加密.crproj");
                packagePath = Path.Combine(basePath, "Projects", this.projectName, "工程打包.iss");
            }
            this.ProtectEdit.Text = File.ReadAllText(protectPath);
            this.PackageEdit.Text = File.ReadAllText(packagePath, Encoding.GetEncoding("GB2312"));
        }

        private void SaveBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            File.WriteAllText(this.protectPath, this.ProtectEdit.Text);
            File.WriteAllText(this.packagePath, this.PackageEdit.Text, Encoding.GetEncoding("GB2312"));
            MsgHelper.Success();
        }
    }
}