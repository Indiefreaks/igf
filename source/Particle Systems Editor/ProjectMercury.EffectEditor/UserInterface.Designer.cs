namespace ProjectMercury.EffectEditor
{
    partial class UserInterface
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
            System.Windows.Forms.ToolStripSeparator toolStripSeparator;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
            System.Windows.Forms.ToolStripStatusLabel uxSpring;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInterface));
            this.uxEffectTreeContextMenuSeperator = new System.Windows.Forms.ToolStripSeparator();
            this.uxMainMenu = new System.Windows.Forms.MenuStrip();
            this.uxMainMenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.uxImportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuView = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuToggleEffectTree = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuTogglePropertyBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuViewTextures = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuHomepage = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuDocumentation = new System.Windows.Forms.ToolStripMenuItem();
            this.uxAPIReferenceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMainMenuLog = new System.Windows.Forms.ToolStripMenuItem();
            this.uxStatusBar = new System.Windows.Forms.StatusStrip();
            this.uxStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.uxActiveParticlesLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.uxUpdateTimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.uxFrameRateProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.uxOuterSplitContainer = new System.Windows.Forms.SplitContainer();
            this.uxTreeLibrarySplitContainer = new System.Windows.Forms.SplitContainer();
            this.uxEffectTree = new System.Windows.Forms.TreeView();
            this.uxEffectTreeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.uxCloneMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxToggleEmitterEnabledMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxAddEmitterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxAddModifierMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxAddControllerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxReinitialiseEmitterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxSelectTextureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxDeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxLibraryListView = new System.Windows.Forms.ListView();
            this.uxLibraryImageList = new System.Windows.Forms.ImageList(this.components);
            this.uxInnerSplitContainer = new System.Windows.Forms.SplitContainer();
            this.uxEffectPreview = new ProjectMercury.EffectEditor.ParticleEffectPreviewControl();
            this.uxPropertyBrowser = new System.Windows.Forms.PropertyGrid();
            this.uxImportEffectDialog = new System.Windows.Forms.OpenFileDialog();
            this.uxExportEffectDialog = new System.Windows.Forms.SaveFileDialog();
            toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            uxSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.uxMainMenu.SuspendLayout();
            this.uxStatusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxOuterSplitContainer)).BeginInit();
            this.uxOuterSplitContainer.Panel1.SuspendLayout();
            this.uxOuterSplitContainer.Panel2.SuspendLayout();
            this.uxOuterSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxTreeLibrarySplitContainer)).BeginInit();
            this.uxTreeLibrarySplitContainer.Panel1.SuspendLayout();
            this.uxTreeLibrarySplitContainer.Panel2.SuspendLayout();
            this.uxTreeLibrarySplitContainer.SuspendLayout();
            this.uxEffectTreeContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxInnerSplitContainer)).BeginInit();
            this.uxInnerSplitContainer.Panel1.SuspendLayout();
            this.uxInnerSplitContainer.Panel2.SuspendLayout();
            this.uxInnerSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new System.Drawing.Size(138, 6);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(138, 6);
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(200, 6);
            // 
            // uxSpring
            // 
            uxSpring.Name = "uxSpring";
            uxSpring.Size = new System.Drawing.Size(470, 17);
            uxSpring.Spring = true;
            // 
            // uxEffectTreeContextMenuSeperator
            // 
            this.uxEffectTreeContextMenuSeperator.Name = "uxEffectTreeContextMenuSeperator";
            this.uxEffectTreeContextMenuSeperator.Size = new System.Drawing.Size(164, 6);
            // 
            // uxMainMenu
            // 
            this.uxMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxMainMenuFile,
            this.uxMainMenuTools,
            this.uxMainMenuView,
            this.uxMainMenuHelp});
            this.uxMainMenu.Location = new System.Drawing.Point(0, 0);
            this.uxMainMenu.Name = "uxMainMenu";
            this.uxMainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.uxMainMenu.Size = new System.Drawing.Size(750, 24);
            this.uxMainMenu.TabIndex = 0;
            // 
            // uxMainMenuFile
            // 
            this.uxMainMenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxMainMenuNew,
            toolStripSeparator,
            this.uxImportMenuItem,
            this.uxExportMenuItem,
            toolStripSeparator1,
            this.uxMainMenuExit});
            this.uxMainMenuFile.Name = "uxMainMenuFile";
            this.uxMainMenuFile.Size = new System.Drawing.Size(37, 20);
            this.uxMainMenuFile.Text = "&File";
            // 
            // uxMainMenuNew
            // 
            this.uxMainMenuNew.Image = ((System.Drawing.Image)(resources.GetObject("uxMainMenuNew.Image")));
            this.uxMainMenuNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uxMainMenuNew.Name = "uxMainMenuNew";
            this.uxMainMenuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.uxMainMenuNew.Size = new System.Drawing.Size(141, 22);
            this.uxMainMenuNew.Text = "&New";
            this.uxMainMenuNew.Click += new System.EventHandler(this.uxMainMenuNew_Click);
            // 
            // uxImportMenuItem
            // 
            this.uxImportMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("uxImportMenuItem.Image")));
            this.uxImportMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uxImportMenuItem.Name = "uxImportMenuItem";
            this.uxImportMenuItem.Size = new System.Drawing.Size(141, 22);
            this.uxImportMenuItem.Text = "Import";
            this.uxImportMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uxImportMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.uxImportMenuItem_DropDownItemClicked);
            // 
            // uxExportMenuItem
            // 
            this.uxExportMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("uxExportMenuItem.Image")));
            this.uxExportMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uxExportMenuItem.Name = "uxExportMenuItem";
            this.uxExportMenuItem.Size = new System.Drawing.Size(141, 22);
            this.uxExportMenuItem.Text = "Export";
            this.uxExportMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.uxExportMenuItem_DropDownItemClicked);
            // 
            // uxMainMenuExit
            // 
            this.uxMainMenuExit.Name = "uxMainMenuExit";
            this.uxMainMenuExit.Size = new System.Drawing.Size(141, 22);
            this.uxMainMenuExit.Text = "E&xit";
            this.uxMainMenuExit.Click += new System.EventHandler(this.uxMainMenuExit_Click);
            // 
            // uxMainMenuTools
            // 
            this.uxMainMenuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxMainMenuOptions});
            this.uxMainMenuTools.Name = "uxMainMenuTools";
            this.uxMainMenuTools.Size = new System.Drawing.Size(48, 20);
            this.uxMainMenuTools.Text = "&Tools";
            // 
            // uxMainMenuOptions
            // 
            this.uxMainMenuOptions.Name = "uxMainMenuOptions";
            this.uxMainMenuOptions.Size = new System.Drawing.Size(116, 22);
            this.uxMainMenuOptions.Text = "&Options";
            this.uxMainMenuOptions.Click += new System.EventHandler(this.uxMainMenuOptions_Click);
            // 
            // uxMainMenuView
            // 
            this.uxMainMenuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxMainMenuToggleEffectTree,
            this.uxMainMenuTogglePropertyBrowser,
            this.uxMainMenuViewTextures});
            this.uxMainMenuView.Name = "uxMainMenuView";
            this.uxMainMenuView.Size = new System.Drawing.Size(44, 20);
            this.uxMainMenuView.Text = "&View";
            // 
            // uxMainMenuToggleEffectTree
            // 
            this.uxMainMenuToggleEffectTree.Checked = true;
            this.uxMainMenuToggleEffectTree.CheckOnClick = true;
            this.uxMainMenuToggleEffectTree.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uxMainMenuToggleEffectTree.Name = "uxMainMenuToggleEffectTree";
            this.uxMainMenuToggleEffectTree.Size = new System.Drawing.Size(202, 22);
            this.uxMainMenuToggleEffectTree.Text = "Effect Composition Tree";
            this.uxMainMenuToggleEffectTree.Click += new System.EventHandler(this.uxMainMenuToggleEffectTree_Click);
            // 
            // uxMainMenuTogglePropertyBrowser
            // 
            this.uxMainMenuTogglePropertyBrowser.Checked = true;
            this.uxMainMenuTogglePropertyBrowser.CheckOnClick = true;
            this.uxMainMenuTogglePropertyBrowser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uxMainMenuTogglePropertyBrowser.Name = "uxMainMenuTogglePropertyBrowser";
            this.uxMainMenuTogglePropertyBrowser.Size = new System.Drawing.Size(202, 22);
            this.uxMainMenuTogglePropertyBrowser.Text = "Property Browser";
            this.uxMainMenuTogglePropertyBrowser.Click += new System.EventHandler(this.uxMainMenuTogglePropertyBrowser_Click);
            // 
            // uxMainMenuViewTextures
            // 
            this.uxMainMenuViewTextures.Name = "uxMainMenuViewTextures";
            this.uxMainMenuViewTextures.Size = new System.Drawing.Size(202, 22);
            this.uxMainMenuViewTextures.Text = "Textures";
            this.uxMainMenuViewTextures.Click += new System.EventHandler(this.uxMainMenuViewTextures_Click);
            // 
            // uxMainMenuHelp
            // 
            this.uxMainMenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxMainMenuHomepage,
            this.uxMainMenuDocumentation,
            this.uxAPIReferenceMenuItem,
            toolStripSeparator5,
            this.uxMainMenuAbout,
            this.uxMainMenuLog});
            this.uxMainMenuHelp.Name = "uxMainMenuHelp";
            this.uxMainMenuHelp.Size = new System.Drawing.Size(44, 20);
            this.uxMainMenuHelp.Text = "&Help";
            // 
            // uxMainMenuHomepage
            // 
            this.uxMainMenuHomepage.Name = "uxMainMenuHomepage";
            this.uxMainMenuHomepage.Size = new System.Drawing.Size(203, 22);
            this.uxMainMenuHomepage.Text = "Visit Homepage";
            this.uxMainMenuHomepage.Click += new System.EventHandler(this.uxMainMenuHomepage_Click);
            // 
            // uxMainMenuDocumentation
            // 
            this.uxMainMenuDocumentation.Name = "uxMainMenuDocumentation";
            this.uxMainMenuDocumentation.Size = new System.Drawing.Size(203, 22);
            this.uxMainMenuDocumentation.Text = "Documentation (Online)";
            this.uxMainMenuDocumentation.Click += new System.EventHandler(this.uxMainMenuDocumentation_Click);
            // 
            // uxAPIReferenceMenuItem
            // 
            this.uxAPIReferenceMenuItem.Name = "uxAPIReferenceMenuItem";
            this.uxAPIReferenceMenuItem.Size = new System.Drawing.Size(203, 22);
            this.uxAPIReferenceMenuItem.Text = "API Reference";
            this.uxAPIReferenceMenuItem.Click += new System.EventHandler(this.uxAPIReferenceMenuItem_Click);
            // 
            // uxMainMenuAbout
            // 
            this.uxMainMenuAbout.Name = "uxMainMenuAbout";
            this.uxMainMenuAbout.Size = new System.Drawing.Size(203, 22);
            this.uxMainMenuAbout.Text = "&About...";
            this.uxMainMenuAbout.Click += new System.EventHandler(this.uxMainMenuAbout_Click);
            // 
            // uxMainMenuLog
            // 
            this.uxMainMenuLog.Name = "uxMainMenuLog";
            this.uxMainMenuLog.Size = new System.Drawing.Size(203, 22);
            this.uxMainMenuLog.Text = "Log";
            this.uxMainMenuLog.Click += new System.EventHandler(this.uxMainMenuLog_Click);
            // 
            // uxStatusBar
            // 
            this.uxStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxStatusLabel,
            this.uxActiveParticlesLabel,
            uxSpring,
            this.uxUpdateTimeLabel,
            this.uxFrameRateProgress});
            this.uxStatusBar.Location = new System.Drawing.Point(0, 534);
            this.uxStatusBar.Name = "uxStatusBar";
            this.uxStatusBar.Size = new System.Drawing.Size(750, 22);
            this.uxStatusBar.TabIndex = 5;
            // 
            // uxStatusLabel
            // 
            this.uxStatusLabel.Name = "uxStatusLabel";
            this.uxStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.uxStatusLabel.Text = "Ready";
            // 
            // uxActiveParticlesLabel
            // 
            this.uxActiveParticlesLabel.Name = "uxActiveParticlesLabel";
            this.uxActiveParticlesLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // uxUpdateTimeLabel
            // 
            this.uxUpdateTimeLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uxUpdateTimeLabel.Name = "uxUpdateTimeLabel";
            this.uxUpdateTimeLabel.Size = new System.Drawing.Size(124, 17);
            this.uxUpdateTimeLabel.Text = "Update: 0.016 seconds";
            // 
            // uxFrameRateProgress
            // 
            this.uxFrameRateProgress.Name = "uxFrameRateProgress";
            this.uxFrameRateProgress.Size = new System.Drawing.Size(100, 16);
            this.uxFrameRateProgress.Step = 1;
            // 
            // uxOuterSplitContainer
            // 
            this.uxOuterSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxOuterSplitContainer.Location = new System.Drawing.Point(0, 24);
            this.uxOuterSplitContainer.Name = "uxOuterSplitContainer";
            // 
            // uxOuterSplitContainer.Panel1
            // 
            this.uxOuterSplitContainer.Panel1.Controls.Add(this.uxTreeLibrarySplitContainer);
            this.uxOuterSplitContainer.Panel1MinSize = 192;
            // 
            // uxOuterSplitContainer.Panel2
            // 
            this.uxOuterSplitContainer.Panel2.Controls.Add(this.uxInnerSplitContainer);
            this.uxOuterSplitContainer.Size = new System.Drawing.Size(750, 510);
            this.uxOuterSplitContainer.SplitterDistance = 192;
            this.uxOuterSplitContainer.TabIndex = 6;
            // 
            // uxTreeLibrarySplitContainer
            // 
            this.uxTreeLibrarySplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxTreeLibrarySplitContainer.Location = new System.Drawing.Point(0, 0);
            this.uxTreeLibrarySplitContainer.Name = "uxTreeLibrarySplitContainer";
            this.uxTreeLibrarySplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // uxTreeLibrarySplitContainer.Panel1
            // 
            this.uxTreeLibrarySplitContainer.Panel1.Controls.Add(this.uxEffectTree);
            // 
            // uxTreeLibrarySplitContainer.Panel2
            // 
            this.uxTreeLibrarySplitContainer.Panel2.Controls.Add(this.uxLibraryListView);
            this.uxTreeLibrarySplitContainer.Size = new System.Drawing.Size(192, 510);
            this.uxTreeLibrarySplitContainer.SplitterDistance = 255;
            this.uxTreeLibrarySplitContainer.TabIndex = 1;
            // 
            // uxEffectTree
            // 
            this.uxEffectTree.BackColor = System.Drawing.Color.LightSteelBlue;
            this.uxEffectTree.ContextMenuStrip = this.uxEffectTreeContextMenu;
            this.uxEffectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxEffectTree.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uxEffectTree.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.uxEffectTree.HideSelection = false;
            this.uxEffectTree.Indent = 32;
            this.uxEffectTree.ItemHeight = 24;
            this.uxEffectTree.LineColor = System.Drawing.Color.SteelBlue;
            this.uxEffectTree.Location = new System.Drawing.Point(0, 0);
            this.uxEffectTree.Name = "uxEffectTree";
            this.uxEffectTree.Size = new System.Drawing.Size(192, 255);
            this.uxEffectTree.TabIndex = 0;
            this.uxEffectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.uxEffectTree_AfterSelect);
            this.uxEffectTree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.uxEffectTree_KeyUp);
            // 
            // uxEffectTreeContextMenu
            // 
            this.uxEffectTreeContextMenu.BackColor = System.Drawing.Color.LightSteelBlue;
            this.uxEffectTreeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxCloneMenuItem,
            this.uxToggleEmitterEnabledMenuItem,
            this.uxAddEmitterMenuItem,
            this.uxAddModifierMenuItem,
            this.uxAddControllerMenuItem,
            this.uxReinitialiseEmitterMenuItem,
            this.uxSelectTextureMenuItem,
            this.uxEffectTreeContextMenuSeperator,
            this.uxDeleteMenuItem});
            this.uxEffectTreeContextMenu.Name = "uxEffectTreeContextMenu";
            this.uxEffectTreeContextMenu.Size = new System.Drawing.Size(168, 208);
            this.uxEffectTreeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.uxEffectTreeContextMenu_Opening);
            // 
            // uxCloneMenuItem
            // 
            this.uxCloneMenuItem.Name = "uxCloneMenuItem";
            this.uxCloneMenuItem.Size = new System.Drawing.Size(167, 22);
            this.uxCloneMenuItem.Text = "Clone";
            this.uxCloneMenuItem.Click += new System.EventHandler(this.uxCloneMenuItem_Click);
            // 
            // uxToggleEmitterEnabledMenuItem
            // 
            this.uxToggleEmitterEnabledMenuItem.Name = "uxToggleEmitterEnabledMenuItem";
            this.uxToggleEmitterEnabledMenuItem.Size = new System.Drawing.Size(167, 22);
            this.uxToggleEmitterEnabledMenuItem.Text = "Enabled (Disable)";
            this.uxToggleEmitterEnabledMenuItem.Click += new System.EventHandler(this.uxToggleEmitterEnabledMenuItem_CheckedChanged);
            // 
            // uxAddEmitterMenuItem
            // 
            this.uxAddEmitterMenuItem.Name = "uxAddEmitterMenuItem";
            this.uxAddEmitterMenuItem.Size = new System.Drawing.Size(167, 22);
            this.uxAddEmitterMenuItem.Text = "Add Emitter...";
            this.uxAddEmitterMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.uxAddEmitterMenuItem_DropDownItemClicked);
            // 
            // uxAddModifierMenuItem
            // 
            this.uxAddModifierMenuItem.Name = "uxAddModifierMenuItem";
            this.uxAddModifierMenuItem.Size = new System.Drawing.Size(167, 22);
            this.uxAddModifierMenuItem.Text = "Add Modifier...";
            // 
            // uxAddControllerMenuItem
            // 
            this.uxAddControllerMenuItem.Name = "uxAddControllerMenuItem";
            this.uxAddControllerMenuItem.Size = new System.Drawing.Size(167, 22);
            this.uxAddControllerMenuItem.Text = "Add Controller...";
            this.uxAddControllerMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.uxAddControllerMenuItem_DropDownItemClicked);
            // 
            // uxReinitialiseEmitterMenuItem
            // 
            this.uxReinitialiseEmitterMenuItem.Name = "uxReinitialiseEmitterMenuItem";
            this.uxReinitialiseEmitterMenuItem.Size = new System.Drawing.Size(167, 22);
            this.uxReinitialiseEmitterMenuItem.Text = "Reinitialise Emiter";
            this.uxReinitialiseEmitterMenuItem.Click += new System.EventHandler(this.uxReinitialiseEmitter_Click);
            // 
            // uxSelectTextureMenuItem
            // 
            this.uxSelectTextureMenuItem.Name = "uxSelectTextureMenuItem";
            this.uxSelectTextureMenuItem.Size = new System.Drawing.Size(167, 22);
            this.uxSelectTextureMenuItem.Text = "Select Texture...";
            this.uxSelectTextureMenuItem.Click += new System.EventHandler(this.uxSelectTexture_Click);
            // 
            // uxDeleteMenuItem
            // 
            this.uxDeleteMenuItem.Name = "uxDeleteMenuItem";
            this.uxDeleteMenuItem.Size = new System.Drawing.Size(167, 22);
            this.uxDeleteMenuItem.Text = "Delete";
            this.uxDeleteMenuItem.Click += new System.EventHandler(this.uxDeleteMenuItem_Click);
            // 
            // uxLibraryListView
            // 
            this.uxLibraryListView.BackColor = System.Drawing.Color.LightSteelBlue;
            this.uxLibraryListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxLibraryListView.LargeImageList = this.uxLibraryImageList;
            this.uxLibraryListView.Location = new System.Drawing.Point(0, 0);
            this.uxLibraryListView.Name = "uxLibraryListView";
            this.uxLibraryListView.Size = new System.Drawing.Size(192, 251);
            this.uxLibraryListView.TabIndex = 0;
            this.uxLibraryListView.UseCompatibleStateImageBehavior = false;
            this.uxLibraryListView.View = System.Windows.Forms.View.Tile;
            this.uxLibraryListView.ItemActivate += new System.EventHandler(this.uxLibraryListView_ItemActivate);
            // 
            // uxLibraryImageList
            // 
            this.uxLibraryImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.uxLibraryImageList.ImageSize = new System.Drawing.Size(32, 32);
            this.uxLibraryImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // uxInnerSplitContainer
            // 
            this.uxInnerSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxInnerSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.uxInnerSplitContainer.Name = "uxInnerSplitContainer";
            this.uxInnerSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // uxInnerSplitContainer.Panel1
            // 
            this.uxInnerSplitContainer.Panel1.Controls.Add(this.uxEffectPreview);
            // 
            // uxInnerSplitContainer.Panel2
            // 
            this.uxInnerSplitContainer.Panel2.Controls.Add(this.uxPropertyBrowser);
            this.uxInnerSplitContainer.Panel2MinSize = 128;
            this.uxInnerSplitContainer.Size = new System.Drawing.Size(554, 510);
            this.uxInnerSplitContainer.SplitterDistance = 306;
            this.uxInnerSplitContainer.TabIndex = 0;
            // 
            // uxEffectPreview
            // 
            this.uxEffectPreview.Cursor = System.Windows.Forms.Cursors.Cross;
            this.uxEffectPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxEffectPreview.Location = new System.Drawing.Point(0, 0);
            this.uxEffectPreview.Name = "uxEffectPreview";
            this.uxEffectPreview.ParticleEffect = null;
            this.uxEffectPreview.Renderer = null;
            this.uxEffectPreview.Size = new System.Drawing.Size(554, 306);
            this.uxEffectPreview.TabIndex = 0;
            this.uxEffectPreview.Text = "Particle Effect Preview";
            this.uxEffectPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uxEffectPreview_MouseDown);
            this.uxEffectPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.uxEffectPreview_MouseMove);
            this.uxEffectPreview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uxEffectPreview_MouseUp);
            // 
            // uxPropertyBrowser
            // 
            this.uxPropertyBrowser.BackColor = System.Drawing.SystemColors.Control;
            this.uxPropertyBrowser.CategoryForeColor = System.Drawing.Color.WhiteSmoke;
            this.uxPropertyBrowser.CommandsBackColor = System.Drawing.SystemColors.Control;
            this.uxPropertyBrowser.CommandsVisibleIfAvailable = false;
            this.uxPropertyBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxPropertyBrowser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uxPropertyBrowser.HelpBackColor = System.Drawing.Color.LightSteelBlue;
            this.uxPropertyBrowser.HelpForeColor = System.Drawing.Color.Black;
            this.uxPropertyBrowser.LineColor = System.Drawing.Color.LightSlateGray;
            this.uxPropertyBrowser.Location = new System.Drawing.Point(0, 0);
            this.uxPropertyBrowser.Name = "uxPropertyBrowser";
            this.uxPropertyBrowser.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.uxPropertyBrowser.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.uxPropertyBrowser.Size = new System.Drawing.Size(554, 200);
            this.uxPropertyBrowser.TabIndex = 0;
            this.uxPropertyBrowser.ToolbarVisible = false;
            this.uxPropertyBrowser.ViewBackColor = System.Drawing.Color.LightSteelBlue;
            // 
            // uxImportEffectDialog
            // 
            this.uxImportEffectDialog.Filter = "Particle Effect Files|*.pfx|All Files|*.*";
            this.uxImportEffectDialog.Title = "Open Particle Effect";
            // 
            // uxExportEffectDialog
            // 
            this.uxExportEffectDialog.Title = "Save Particle Effect";
            // 
            // UserInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(750, 556);
            this.Controls.Add(this.uxOuterSplitContainer);
            this.Controls.Add(this.uxStatusBar);
            this.Controls.Add(this.uxMainMenu);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.uxMainMenu;
            this.MinimumSize = new System.Drawing.Size(480, 320);
            this.Name = "UserInterface";
            this.Text = "Mercury Particle Engine";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.uxMainMenu.ResumeLayout(false);
            this.uxMainMenu.PerformLayout();
            this.uxStatusBar.ResumeLayout(false);
            this.uxStatusBar.PerformLayout();
            this.uxOuterSplitContainer.Panel1.ResumeLayout(false);
            this.uxOuterSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uxOuterSplitContainer)).EndInit();
            this.uxOuterSplitContainer.ResumeLayout(false);
            this.uxTreeLibrarySplitContainer.Panel1.ResumeLayout(false);
            this.uxTreeLibrarySplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uxTreeLibrarySplitContainer)).EndInit();
            this.uxTreeLibrarySplitContainer.ResumeLayout(false);
            this.uxEffectTreeContextMenu.ResumeLayout(false);
            this.uxInnerSplitContainer.Panel1.ResumeLayout(false);
            this.uxInnerSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uxInnerSplitContainer)).EndInit();
            this.uxInnerSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip uxMainMenu;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuFile;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuNew;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuExit;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuTools;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuOptions;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuHelp;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuAbout;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuHomepage;
        private System.Windows.Forms.StatusStrip uxStatusBar;
        private System.Windows.Forms.ToolStripStatusLabel uxStatusLabel;
        private System.Windows.Forms.SplitContainer uxOuterSplitContainer;
        private System.Windows.Forms.TreeView uxEffectTree;
        private System.Windows.Forms.SplitContainer uxInnerSplitContainer;
        private System.Windows.Forms.PropertyGrid uxPropertyBrowser;
        private System.Windows.Forms.OpenFileDialog uxImportEffectDialog;
        private System.Windows.Forms.SaveFileDialog uxExportEffectDialog;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuView;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuToggleEffectTree;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuTogglePropertyBrowser;
        private ParticleEffectPreviewControl uxEffectPreview;
        private System.Windows.Forms.ToolStripMenuItem uxImportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uxExportMenuItem;
        private System.Windows.Forms.ContextMenuStrip uxEffectTreeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem uxDeleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uxAddEmitterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uxAddModifierMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uxReinitialiseEmitterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuViewTextures;
        private System.Windows.Forms.ToolStripMenuItem uxSelectTextureMenuItem;
        private System.Windows.Forms.SplitContainer uxTreeLibrarySplitContainer;
        private System.Windows.Forms.ListView uxLibraryListView;
        private System.Windows.Forms.ImageList uxLibraryImageList;
        private System.Windows.Forms.ToolStripStatusLabel uxActiveParticlesLabel;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuDocumentation;
        private System.Windows.Forms.ToolStripMenuItem uxCloneMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uxToggleEmitterEnabledMenuItem;
        private System.Windows.Forms.ToolStripSeparator uxEffectTreeContextMenuSeperator;
        private System.Windows.Forms.ToolStripMenuItem uxAPIReferenceMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel uxUpdateTimeLabel;
        private System.Windows.Forms.ToolStripProgressBar uxFrameRateProgress;
        private System.Windows.Forms.ToolStripMenuItem uxMainMenuLog;
        private System.Windows.Forms.ToolStripMenuItem uxAddControllerMenuItem;
    }
}