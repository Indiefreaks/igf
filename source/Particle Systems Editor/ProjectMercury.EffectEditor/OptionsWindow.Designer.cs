namespace ProjectMercury.EffectEditor
{
    partial class OptionsWindow
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsWindow));
            this.uxColourPicker = new System.Windows.Forms.ColorDialog();
            this.uxEditorOptionsGroup = new System.Windows.Forms.GroupBox();
            this.pbTile = new System.Windows.Forms.PictureBox();
            this.pbCenter = new System.Windows.Forms.PictureBox();
            this.pbStretch = new System.Windows.Forms.PictureBox();
            this.rbTile = new System.Windows.Forms.RadioButton();
            this.rbCenter = new System.Windows.Forms.RadioButton();
            this.rbStretch = new System.Windows.Forms.RadioButton();
            this.uxClearBackgroundImage = new System.Windows.Forms.Button();
            this.uxBrowseBackgroundImage = new System.Windows.Forms.Button();
            this.uxChangeBackgroundColour = new System.Windows.Forms.Button();
            this.uxBackgroundColour = new System.Windows.Forms.Panel();
            this.uxBackgroundImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.uxToolTipProvider = new System.Windows.Forms.ToolTip(this.components);
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.uxEditorOptionsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCenter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStretch)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 45);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(101, 13);
            label1.TabIndex = 0;
            label1.Text = "Background Colour:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 94);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(100, 13);
            label2.TabIndex = 3;
            label2.Text = "Background Image:";
            // 
            // uxColourPicker
            // 
            this.uxColourPicker.FullOpen = true;
            // 
            // uxEditorOptionsGroup
            // 
            this.uxEditorOptionsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.uxEditorOptionsGroup.Controls.Add(this.pbTile);
            this.uxEditorOptionsGroup.Controls.Add(this.pbCenter);
            this.uxEditorOptionsGroup.Controls.Add(this.pbStretch);
            this.uxEditorOptionsGroup.Controls.Add(this.rbTile);
            this.uxEditorOptionsGroup.Controls.Add(this.rbCenter);
            this.uxEditorOptionsGroup.Controls.Add(this.rbStretch);
            this.uxEditorOptionsGroup.Controls.Add(this.uxClearBackgroundImage);
            this.uxEditorOptionsGroup.Controls.Add(this.uxBrowseBackgroundImage);
            this.uxEditorOptionsGroup.Controls.Add(label2);
            this.uxEditorOptionsGroup.Controls.Add(this.uxChangeBackgroundColour);
            this.uxEditorOptionsGroup.Controls.Add(this.uxBackgroundColour);
            this.uxEditorOptionsGroup.Controls.Add(label1);
            this.uxEditorOptionsGroup.Location = new System.Drawing.Point(12, 12);
            this.uxEditorOptionsGroup.Name = "uxEditorOptionsGroup";
            this.uxEditorOptionsGroup.Size = new System.Drawing.Size(475, 242);
            this.uxEditorOptionsGroup.TabIndex = 0;
            this.uxEditorOptionsGroup.TabStop = false;
            this.uxEditorOptionsGroup.Text = "Particle window";
            // 
            // pbTile
            // 
            this.pbTile.Image = ((System.Drawing.Image)(resources.GetObject("pbTile.Image")));
            this.pbTile.Location = new System.Drawing.Point(285, 137);
            this.pbTile.Name = "pbTile";
            this.pbTile.Size = new System.Drawing.Size(100, 79);
            this.pbTile.TabIndex = 12;
            this.pbTile.TabStop = false;
            this.uxToolTipProvider.SetToolTip(this.pbTile, "Tiled");
            // 
            // pbCenter
            // 
            this.pbCenter.Image = ((System.Drawing.Image)(resources.GetObject("pbCenter.Image")));
            this.pbCenter.Location = new System.Drawing.Point(159, 137);
            this.pbCenter.Name = "pbCenter";
            this.pbCenter.Size = new System.Drawing.Size(100, 79);
            this.pbCenter.TabIndex = 11;
            this.pbCenter.TabStop = false;
            this.uxToolTipProvider.SetToolTip(this.pbCenter, "Centred");
            // 
            // pbStretch
            // 
            this.pbStretch.Image = ((System.Drawing.Image)(resources.GetObject("pbStretch.Image")));
            this.pbStretch.Location = new System.Drawing.Point(30, 137);
            this.pbStretch.Name = "pbStretch";
            this.pbStretch.Size = new System.Drawing.Size(100, 79);
            this.pbStretch.TabIndex = 10;
            this.pbStretch.TabStop = false;
            this.uxToolTipProvider.SetToolTip(this.pbStretch, "Stretched");
            // 
            // rbTile
            // 
            this.rbTile.AutoSize = true;
            this.rbTile.Location = new System.Drawing.Point(265, 168);
            this.rbTile.Name = "rbTile";
            this.rbTile.Size = new System.Drawing.Size(14, 13);
            this.rbTile.TabIndex = 9;
            this.rbTile.TabStop = true;
            this.rbTile.UseVisualStyleBackColor = true;
            this.rbTile.CheckedChanged += new System.EventHandler(this.rbTile_CheckedChanged);
            // 
            // rbCenter
            // 
            this.rbCenter.AutoSize = true;
            this.rbCenter.Location = new System.Drawing.Point(136, 168);
            this.rbCenter.Name = "rbCenter";
            this.rbCenter.Size = new System.Drawing.Size(14, 13);
            this.rbCenter.TabIndex = 8;
            this.rbCenter.TabStop = true;
            this.rbCenter.UseVisualStyleBackColor = true;
            this.rbCenter.CheckedChanged += new System.EventHandler(this.rbCenter_CheckedChanged);
            // 
            // rbStretch
            // 
            this.rbStretch.AutoSize = true;
            this.rbStretch.Checked = true;
            this.rbStretch.Location = new System.Drawing.Point(9, 168);
            this.rbStretch.Name = "rbStretch";
            this.rbStretch.Size = new System.Drawing.Size(14, 13);
            this.rbStretch.TabIndex = 7;
            this.rbStretch.TabStop = true;
            this.rbStretch.UseVisualStyleBackColor = true;
            this.rbStretch.CheckedChanged += new System.EventHandler(this.rbStretch_CheckedChanged);
            // 
            // uxClearBackgroundImage
            // 
            this.uxClearBackgroundImage.Location = new System.Drawing.Point(170, 89);
            this.uxClearBackgroundImage.Name = "uxClearBackgroundImage";
            this.uxClearBackgroundImage.Size = new System.Drawing.Size(64, 23);
            this.uxClearBackgroundImage.TabIndex = 5;
            this.uxClearBackgroundImage.Text = "Clear";
            this.uxClearBackgroundImage.UseVisualStyleBackColor = true;
            this.uxClearBackgroundImage.Click += new System.EventHandler(this.uxClearBackgroundImage_Click);
            // 
            // uxBrowseBackgroundImage
            // 
            this.uxBrowseBackgroundImage.Location = new System.Drawing.Point(132, 89);
            this.uxBrowseBackgroundImage.Name = "uxBrowseBackgroundImage";
            this.uxBrowseBackgroundImage.Size = new System.Drawing.Size(32, 23);
            this.uxBrowseBackgroundImage.TabIndex = 4;
            this.uxBrowseBackgroundImage.Text = "...";
            this.uxBrowseBackgroundImage.UseVisualStyleBackColor = true;
            this.uxBrowseBackgroundImage.Click += new System.EventHandler(this.uxBrowseBackgroundImage_Click);
            // 
            // uxChangeBackgroundColour
            // 
            this.uxChangeBackgroundColour.Location = new System.Drawing.Point(132, 40);
            this.uxChangeBackgroundColour.Name = "uxChangeBackgroundColour";
            this.uxChangeBackgroundColour.Size = new System.Drawing.Size(32, 23);
            this.uxChangeBackgroundColour.TabIndex = 2;
            this.uxChangeBackgroundColour.Text = "...";
            this.uxChangeBackgroundColour.UseVisualStyleBackColor = true;
            this.uxChangeBackgroundColour.Click += new System.EventHandler(this.uxChangeBackgroundColour_Click);
            // 
            // uxBackgroundColour
            // 
            this.uxBackgroundColour.BackColor = System.Drawing.Color.Black;
            this.uxBackgroundColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uxBackgroundColour.Location = new System.Drawing.Point(170, 40);
            this.uxBackgroundColour.Name = "uxBackgroundColour";
            this.uxBackgroundColour.Size = new System.Drawing.Size(64, 23);
            this.uxBackgroundColour.TabIndex = 1;
            // 
            // uxBackgroundImageDialog
            // 
            this.uxBackgroundImageDialog.Filter = "Image Files|*.bmp;*.jpg;*.png;*.jpeg|Windows Bitmap|*.bmp|Portable Network Graphi" +
                "c|*.png|JPEG|*.jpg;*.jpeg|Truevision TARGA|*.tga|All Files|*.*";
            this.uxBackgroundImageDialog.Title = "Select Backgound Image";
            // 
            // OptionsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 266);
            this.Controls.Add(this.uxEditorOptionsGroup);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OptionsWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.TopMost = true;
            this.uxEditorOptionsGroup.ResumeLayout(false);
            this.uxEditorOptionsGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCenter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStretch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog uxColourPicker;
        private System.Windows.Forms.GroupBox uxEditorOptionsGroup;
        private System.Windows.Forms.Button uxChangeBackgroundColour;
        private System.Windows.Forms.Panel uxBackgroundColour;
        private System.Windows.Forms.Button uxClearBackgroundImage;
        private System.Windows.Forms.Button uxBrowseBackgroundImage;
        private System.Windows.Forms.OpenFileDialog uxBackgroundImageDialog;
        private System.Windows.Forms.RadioButton rbTile;
        private System.Windows.Forms.RadioButton rbCenter;
        private System.Windows.Forms.RadioButton rbStretch;
        private System.Windows.Forms.PictureBox pbTile;
        private System.Windows.Forms.PictureBox pbCenter;
        private System.Windows.Forms.PictureBox pbStretch;
        private System.Windows.Forms.ToolTip uxToolTipProvider;
    }
}