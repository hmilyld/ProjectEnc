namespace ProjectEnc
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.NewBtnItem = new DevExpress.XtraBars.BarButtonItem();
            this.SaveBtnItem = new DevExpress.XtraBars.BarButtonItem();
            this.DelBtnItem = new DevExpress.XtraBars.BarButtonItem();
            this.OpenBtnItem = new DevExpress.XtraBars.BarButtonItem();
            this.EditProjectTemplateBtnItem = new DevExpress.XtraBars.BarButtonItem();
            this.MergeBtnItem = new DevExpress.XtraBars.BarButtonItem();
            this.ProtectBtnItem = new DevExpress.XtraBars.BarButtonItem();
            this.PackageBtnItem = new DevExpress.XtraBars.BarButtonItem();
            this.EditTemplateBtnItem = new DevExpress.XtraBars.BarButtonItem();
            this.HelpBtnItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.treeList = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.tablePanel2 = new DevExpress.Utils.Layout.TablePanel();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.LogEdit = new DevExpress.XtraEditors.MemoEdit();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).BeginInit();
            this.tablePanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.NewBtnItem,
            this.SaveBtnItem,
            this.DelBtnItem,
            this.MergeBtnItem,
            this.ProtectBtnItem,
            this.PackageBtnItem,
            this.EditTemplateBtnItem,
            this.HelpBtnItem,
            this.EditProjectTemplateBtnItem,
            this.OpenBtnItem,
            this.barStaticItem1,
            this.barStaticItem2});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 15;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.NewBtnItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SaveBtnItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.DelBtnItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.OpenBtnItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.EditProjectTemplateBtnItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.MergeBtnItem, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ProtectBtnItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.PackageBtnItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.EditTemplateBtnItem, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.HelpBtnItem, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // NewBtnItem
            // 
            this.NewBtnItem.Caption = "新建工程";
            this.NewBtnItem.Id = 0;
            this.NewBtnItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("NewBtnItem.ImageOptions.SvgImage")));
            this.NewBtnItem.Name = "NewBtnItem";
            this.NewBtnItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NewBtnItem_ItemClick);
            // 
            // SaveBtnItem
            // 
            this.SaveBtnItem.Caption = "保存工程";
            this.SaveBtnItem.Id = 2;
            this.SaveBtnItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("SaveBtnItem.ImageOptions.SvgImage")));
            this.SaveBtnItem.Name = "SaveBtnItem";
            this.SaveBtnItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SaveBtnItem_ItemClick);
            // 
            // DelBtnItem
            // 
            this.DelBtnItem.Caption = "删除工程";
            this.DelBtnItem.Id = 3;
            this.DelBtnItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("DelBtnItem.ImageOptions.SvgImage")));
            this.DelBtnItem.Name = "DelBtnItem";
            this.DelBtnItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DelBtnItem_ItemClick);
            // 
            // OpenBtnItem
            // 
            this.OpenBtnItem.Caption = "打开工程目录";
            this.OpenBtnItem.Id = 11;
            this.OpenBtnItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("OpenBtnItem.ImageOptions.SvgImage")));
            this.OpenBtnItem.Name = "OpenBtnItem";
            this.OpenBtnItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.OpenBtnItem_ItemClick);
            // 
            // EditProjectTemplateBtnItem
            // 
            this.EditProjectTemplateBtnItem.Caption = "修改工程模板";
            this.EditProjectTemplateBtnItem.Id = 10;
            this.EditProjectTemplateBtnItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("EditProjectTemplateBtnItem.ImageOptions.SvgImage")));
            this.EditProjectTemplateBtnItem.Name = "EditProjectTemplateBtnItem";
            this.EditProjectTemplateBtnItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditProjectTemplateBtnItem_ItemClick);
            // 
            // MergeBtnItem
            // 
            this.MergeBtnItem.Caption = "合并文件";
            this.MergeBtnItem.Id = 4;
            this.MergeBtnItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("MergeBtnItem.ImageOptions.SvgImage")));
            this.MergeBtnItem.Name = "MergeBtnItem";
            this.MergeBtnItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.MergeBtnItem_ItemClick);
            // 
            // ProtectBtnItem
            // 
            this.ProtectBtnItem.Caption = "加密主程序";
            this.ProtectBtnItem.Id = 5;
            this.ProtectBtnItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ProtectBtnItem.ImageOptions.SvgImage")));
            this.ProtectBtnItem.Name = "ProtectBtnItem";
            this.ProtectBtnItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ProtectBtnItem_ItemClick);
            // 
            // PackageBtnItem
            // 
            this.PackageBtnItem.Caption = "生成安装包";
            this.PackageBtnItem.Id = 6;
            this.PackageBtnItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("PackageBtnItem.ImageOptions.SvgImage")));
            this.PackageBtnItem.Name = "PackageBtnItem";
            this.PackageBtnItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PackageBtnItem_ItemClick);
            // 
            // EditTemplateBtnItem
            // 
            this.EditTemplateBtnItem.Caption = "编辑模板文件";
            this.EditTemplateBtnItem.Id = 7;
            this.EditTemplateBtnItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("EditTemplateBtnItem.ImageOptions.SvgImage")));
            this.EditTemplateBtnItem.Name = "EditTemplateBtnItem";
            this.EditTemplateBtnItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditTemplateBtnItem_ItemClick);
            // 
            // HelpBtnItem
            // 
            this.HelpBtnItem.Caption = "说明文档";
            this.HelpBtnItem.Id = 8;
            this.HelpBtnItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("HelpBtnItem.ImageOptions.SvgImage")));
            this.HelpBtnItem.Name = "HelpBtnItem";
            this.HelpBtnItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.HelpBtnItem_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem2)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1976, 110);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 1328);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1976, 48);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 110);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 1218);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1976, 110);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 1218);
            // 
            // tablePanel1
            // 
            this.tablePanel2.SetColumn(this.tablePanel1, 0);
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 300F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 100F)});
            this.tablePanel1.Controls.Add(this.groupControl1);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(6, 6);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(6);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel2.SetRow(this.tablePanel1, 0);
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(1964, 1006);
            this.tablePanel1.TabIndex = 4;
            // 
            // groupControl1
            // 
            this.tablePanel1.SetColumn(this.groupControl1, 0);
            this.groupControl1.Controls.Add(this.treeList);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(3, 3);
            this.groupControl1.Name = "groupControl1";
            this.tablePanel1.SetRow(this.groupControl1, 0);
            this.groupControl1.Size = new System.Drawing.Size(294, 1000);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "工程列表";
            // 
            // treeList
            // 
            this.treeList.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.treeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList.FixedLineWidth = 12;
            this.treeList.HorzScrollStep = 24;
            this.treeList.Location = new System.Drawing.Point(3, 45);
            this.treeList.Margin = new System.Windows.Forms.Padding(6);
            this.treeList.MenuManager = this.barManager1;
            this.treeList.MinWidth = 160;
            this.treeList.Name = "treeList";
            this.treeList.OptionsBehavior.Editable = false;
            this.treeList.Size = new System.Drawing.Size(288, 952);
            this.treeList.TabIndex = 0;
            this.treeList.TreeLevelWidth = 24;
            this.treeList.DoubleClick += new System.EventHandler(this.TreeList_DoubleClick);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "工程名称";
            this.treeListColumn1.FieldName = "工程名称";
            this.treeListColumn1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("treeListColumn1.ImageOptions.SvgImage")));
            this.treeListColumn1.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.treeListColumn1.MinWidth = 16;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 600;
            // 
            // tablePanel2
            // 
            this.tablePanel2.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 100F)});
            this.tablePanel2.Controls.Add(this.groupControl2);
            this.tablePanel2.Controls.Add(this.tablePanel1);
            this.tablePanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel2.Location = new System.Drawing.Point(0, 110);
            this.tablePanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tablePanel2.Name = "tablePanel2";
            this.tablePanel2.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 100F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 200F)});
            this.tablePanel2.Size = new System.Drawing.Size(1976, 1218);
            this.tablePanel2.TabIndex = 9;
            // 
            // groupControl2
            // 
            this.tablePanel2.SetColumn(this.groupControl2, 0);
            this.groupControl2.Controls.Add(this.LogEdit);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(9, 1018);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(9, 0, 9, 0);
            this.groupControl2.Name = "groupControl2";
            this.tablePanel2.SetRow(this.groupControl2, 1);
            this.groupControl2.Size = new System.Drawing.Size(1958, 200);
            this.groupControl2.TabIndex = 5;
            this.groupControl2.Text = "日志信息";
            // 
            // LogEdit
            // 
            this.LogEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogEdit.Location = new System.Drawing.Point(3, 45);
            this.LogEdit.Margin = new System.Windows.Forms.Padding(24);
            this.LogEdit.MenuManager = this.barManager1;
            this.LogEdit.Name = "LogEdit";
            this.LogEdit.Properties.ReadOnly = true;
            this.LogEdit.Size = new System.Drawing.Size(1952, 152);
            this.LogEdit.TabIndex = 0;
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem1.Caption = "Copyright © 2024 Hmilyld.com. All rights reserved. ";
            this.barStaticItem1.Id = 12;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "注意：任何操作都有可能导致软件损坏，虽有备份机制，但强烈建议操作前手工备份！！！";
            this.barStaticItem2.Id = 14;
            this.barStaticItem2.Name = "barStaticItem2";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1976, 1376);
            this.Controls.Add(this.tablePanel2);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("MainForm.IconOptions.Image")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "C#程序合并、加密及打包专用工具";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).EndInit();
            this.tablePanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LogEdit.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraBars.BarButtonItem NewBtnItem;
        private DevExpress.XtraBars.BarButtonItem SaveBtnItem;
        private DevExpress.XtraBars.BarButtonItem DelBtnItem;
        private DevExpress.XtraTreeList.TreeList treeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraBars.BarButtonItem MergeBtnItem;
        private DevExpress.XtraBars.BarButtonItem ProtectBtnItem;
        private DevExpress.XtraBars.BarButtonItem PackageBtnItem;
        private DevExpress.XtraBars.BarButtonItem EditTemplateBtnItem;
        private DevExpress.XtraBars.BarButtonItem HelpBtnItem;
        private DevExpress.XtraBars.BarButtonItem EditProjectTemplateBtnItem;
        private DevExpress.Utils.Layout.TablePanel tablePanel2;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.MemoEdit LogEdit;
        private DevExpress.XtraBars.BarButtonItem OpenBtnItem;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
    }
}