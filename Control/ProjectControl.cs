using ProjectEnc.Bean;
using System.Windows.Forms;

namespace ProjectEnc.Control
{
    public partial class ProjectControl : DevExpress.XtraEditors.XtraUserControl
    {
        private readonly PBaseControl pBaseControl = new PBaseControl();

        private readonly FileControl mergeControl = new FileControl("merge");

        private readonly FileControl ignoreControl = new FileControl("ignore");

        private Project project;

        public ProjectControl()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            this.groupControl2.Controls.Add(this.pBaseControl);
            this.groupControl3.Controls.Add(this.mergeControl);
            this.groupControl4.Controls.Add(this.ignoreControl);
        }

        internal void LoadProject(Project project)
        {
            this.project = project;

            this.pBaseControl.LoadProject(project, this.RefreshFiles);
            this.mergeControl.LoadProject(project);
            this.ignoreControl.LoadProject(project);
        }

        internal void RefreshFiles()
        {
            this.mergeControl.LoadProject(project);
            this.ignoreControl.LoadProject(project);
        }
    }
}