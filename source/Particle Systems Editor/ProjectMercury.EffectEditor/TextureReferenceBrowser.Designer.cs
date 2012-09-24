namespace ProjectMercury.EffectEditor
{
    partial class TextureReferenceBrowser
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
            this.uxCancel = new System.Windows.Forms.Button();
            this.uxOK = new System.Windows.Forms.Button();
            this.uxTextureReferences = new System.Windows.Forms.ListView();
            this.uxTextureImageList = new System.Windows.Forms.ImageList(this.components);
            this.uxImport = new System.Windows.Forms.Button();
            this.uxImportDialog = new System.Windows.Forms.OpenFileDialog();
            this.uxTextureLicenseLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // uxCancel
            // 
            this.uxCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.uxCancel.Location = new System.Drawing.Point(505, 231);
            this.uxCancel.Name = "uxCancel";
            this.uxCancel.Size = new System.Drawing.Size(75, 23);
            this.uxCancel.TabIndex = 0;
            this.uxCancel.Text = "Cancel";
            this.uxCancel.UseVisualStyleBackColor = true;
            // 
            // uxOK
            // 
            this.uxOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.uxOK.Location = new System.Drawing.Point(424, 231);
            this.uxOK.Name = "uxOK";
            this.uxOK.Size = new System.Drawing.Size(75, 23);
            this.uxOK.TabIndex = 1;
            this.uxOK.Text = "OK";
            this.uxOK.UseVisualStyleBackColor = true;
            // 
            // uxTextureReferences
            // 
            this.uxTextureReferences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.uxTextureReferences.BackColor = System.Drawing.SystemColors.Window;
            this.uxTextureReferences.LargeImageList = this.uxTextureImageList;
            this.uxTextureReferences.Location = new System.Drawing.Point(12, 12);
            this.uxTextureReferences.MultiSelect = false;
            this.uxTextureReferences.Name = "uxTextureReferences";
            this.uxTextureReferences.ShowItemToolTips = true;
            this.uxTextureReferences.Size = new System.Drawing.Size(568, 213);
            this.uxTextureReferences.TabIndex = 2;
            this.uxTextureReferences.TileSize = new System.Drawing.Size(128, 128);
            this.uxTextureReferences.UseCompatibleStateImageBehavior = false;
            this.uxTextureReferences.ItemActivate += new System.EventHandler(this.uxTextureReferences_ItemActivate);
            // 
            // uxTextureImageList
            // 
            this.uxTextureImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.uxTextureImageList.ImageSize = new System.Drawing.Size(128, 128);
            this.uxTextureImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // uxImport
            // 
            this.uxImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uxImport.Location = new System.Drawing.Point(12, 231);
            this.uxImport.Name = "uxImport";
            this.uxImport.Size = new System.Drawing.Size(75, 23);
            this.uxImport.TabIndex = 3;
            this.uxImport.Text = "Import...";
            this.uxImport.UseVisualStyleBackColor = true;
            this.uxImport.Click += new System.EventHandler(this.uxImport_Click);
            // 
            // uxImportDialog
            // 
            this.uxImportDialog.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png|Windows Bitmap (*.bmp)|*.bmp|" +
                "JPEG (*.jpg)|*.jpg|Portable Network Graphic (*.png)|*.png";
            this.uxImportDialog.Multiselect = true;
            this.uxImportDialog.Title = "Import Texture";
            // 
            // uxTextureLicenseLabel
            // 
            this.uxTextureLicenseLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uxTextureLicenseLabel.AutoSize = true;
            this.uxTextureLicenseLabel.Location = new System.Drawing.Point(93, 236);
            this.uxTextureLicenseLabel.Name = "uxTextureLicenseLabel";
            this.uxTextureLicenseLabel.Size = new System.Drawing.Size(76, 13);
            this.uxTextureLicenseLabel.TabIndex = 4;
            this.uxTextureLicenseLabel.TabStop = true;
            this.uxTextureLicenseLabel.Text = "Image License";
            this.uxTextureLicenseLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.uxTextureLicenseLabel_LinkClicked);
            // 
            // TextureReferenceBrowser
            // 
            this.AcceptButton = this.uxOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.uxCancel;
            this.ClientSize = new System.Drawing.Size(592, 266);
            this.Controls.Add(this.uxTextureLicenseLabel);
            this.Controls.Add(this.uxImport);
            this.Controls.Add(this.uxTextureReferences);
            this.Controls.Add(this.uxOK);
            this.Controls.Add(this.uxCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextureReferenceBrowser";
            this.Text = "Texture Browser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button uxCancel;
        private System.Windows.Forms.Button uxOK;
        private System.Windows.Forms.ListView uxTextureReferences;
        private System.Windows.Forms.ImageList uxTextureImageList;
        private System.Windows.Forms.Button uxImport;
        private System.Windows.Forms.OpenFileDialog uxImportDialog;
        private System.Windows.Forms.LinkLabel uxTextureLicenseLabel;
    }
}