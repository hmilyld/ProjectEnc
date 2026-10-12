using ProjectEnc.Bean;
using ProjectEnc.Control;
using ProjectEnc.Helper;
using System;
using System.Windows.Forms;

namespace ProjectEnc.Form
{
    public partial class DialogNewForm : DevExpress.XtraEditors.XtraForm
    {
        private readonly PBaseControl pBaseControl = new PBaseControl();
        public Project Project { get; private set; }

        public DialogNewForm()
        {
            InitializeComponent();

            this.Controls.Add(pBaseControl);
        }

        private void SaveBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Project = this.pBaseControl.Save();
            if (this.Project != null)
            {
                try
                {
                    this.Project.Init();
                }
                catch (Exception ex)
                {
                    MsgHelper.Error(ex);
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}