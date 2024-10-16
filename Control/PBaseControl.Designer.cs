namespace ProjectEnc.Control
{
    partial class PBaseControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.ProjectNameTxt = new DevExpress.XtraEditors.TextEdit();
            this.ExeVerTxt = new DevExpress.XtraEditors.TextEdit();
            this.InstallVerTxt = new DevExpress.XtraEditors.TextEdit();
            this.InstallPathTxt = new DevExpress.XtraEditors.TextEdit();
            this.ExePath = new DevExpress.XtraEditors.TextEdit();
            this.PathTxt = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProjectNameTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExeVerTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InstallVerTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InstallPathTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExePath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PathTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.ProjectNameTxt);
            this.layoutControl1.Controls.Add(this.ExeVerTxt);
            this.layoutControl1.Controls.Add(this.InstallVerTxt);
            this.layoutControl1.Controls.Add(this.InstallPathTxt);
            this.layoutControl1.Controls.Add(this.ExePath);
            this.layoutControl1.Controls.Add(this.PathTxt);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1388, 130, 1300, 800);
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1294, 320);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // ProjectNameTxt
            // 
            this.ProjectNameTxt.Location = new System.Drawing.Point(192, 12);
            this.ProjectNameTxt.Name = "ProjectNameTxt";
            this.ProjectNameTxt.Size = new System.Drawing.Size(1090, 46);
            this.ProjectNameTxt.StyleController = this.layoutControl1;
            this.ProjectNameTxt.TabIndex = 10;
            // 
            // ExeVerTxt
            // 
            this.ExeVerTxt.Location = new System.Drawing.Point(192, 162);
            this.ExeVerTxt.Name = "ExeVerTxt";
            this.ExeVerTxt.Properties.ReadOnly = true;
            this.ExeVerTxt.Size = new System.Drawing.Size(1090, 46);
            this.ExeVerTxt.StyleController = this.layoutControl1;
            this.ExeVerTxt.TabIndex = 9;
            // 
            // InstallVerTxt
            // 
            this.InstallVerTxt.EditValue = "";
            this.InstallVerTxt.Location = new System.Drawing.Point(192, 262);
            this.InstallVerTxt.Name = "InstallVerTxt";
            this.InstallVerTxt.Properties.ReadOnly = true;
            this.InstallVerTxt.Size = new System.Drawing.Size(1090, 46);
            this.InstallVerTxt.StyleController = this.layoutControl1;
            this.InstallVerTxt.TabIndex = 8;
            // 
            // InstallPathTxt
            // 
            this.InstallPathTxt.Location = new System.Drawing.Point(192, 212);
            this.InstallPathTxt.Name = "InstallPathTxt";
            this.InstallPathTxt.Properties.ReadOnly = true;
            this.InstallPathTxt.Size = new System.Drawing.Size(1090, 46);
            this.InstallPathTxt.StyleController = this.layoutControl1;
            this.InstallPathTxt.TabIndex = 6;
            this.InstallPathTxt.Click += new System.EventHandler(this.InstallPathTxt_Click);
            // 
            // ExePath
            // 
            this.ExePath.Location = new System.Drawing.Point(192, 112);
            this.ExePath.Name = "ExePath";
            this.ExePath.Properties.ReadOnly = true;
            this.ExePath.Size = new System.Drawing.Size(1090, 46);
            this.ExePath.StyleController = this.layoutControl1;
            this.ExePath.TabIndex = 5;
            this.ExePath.Click += new System.EventHandler(this.ExePath_Click);
            // 
            // PathTxt
            // 
            this.PathTxt.Location = new System.Drawing.Point(192, 62);
            this.PathTxt.Name = "PathTxt";
            this.PathTxt.Properties.ReadOnly = true;
            this.PathTxt.Size = new System.Drawing.Size(1090, 46);
            this.PathTxt.StyleController = this.layoutControl1;
            this.PathTxt.TabIndex = 4;
            this.PathTxt.Click += new System.EventHandler(this.PathTxt_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem5,
            this.layoutControlItem4,
            this.layoutControlItem6});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1294, 320);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.PathTxt;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 50);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1274, 50);
            this.layoutControlItem1.Text = "程序路径";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(168, 31);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.ExePath;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 100);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(1274, 50);
            this.layoutControlItem2.Text = "主程序文件";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(168, 31);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.InstallPathTxt;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 200);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(1274, 50);
            this.layoutControlItem3.Text = "安装包保存路径";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(168, 31);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.InstallVerTxt;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 250);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(1274, 50);
            this.layoutControlItem5.Text = "安装包版本号";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(168, 31);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.ExeVerTxt;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 150);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(1274, 50);
            this.layoutControlItem4.Text = "主程序版本号";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(168, 31);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.ProjectNameTxt;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(1274, 50);
            this.layoutControlItem6.Text = "工程名称";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(168, 31);
            // 
            // PBaseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "PBaseControl";
            this.Size = new System.Drawing.Size(1294, 320);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProjectNameTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExeVerTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InstallVerTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InstallPathTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExePath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PathTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.TextEdit InstallVerTxt;
        private DevExpress.XtraEditors.TextEdit InstallPathTxt;
        private DevExpress.XtraEditors.TextEdit ExePath;
        private DevExpress.XtraEditors.TextEdit PathTxt;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.TextEdit ExeVerTxt;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.TextEdit ProjectNameTxt;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}
