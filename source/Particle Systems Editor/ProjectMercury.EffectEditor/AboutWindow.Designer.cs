namespace ProjectMercury.EffectEditor
{
    partial class AboutWindow
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
            this.uxLogo = new System.Windows.Forms.Panel();
            this.uxAboutBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // uxLogo
            // 
            this.uxLogo.BackColor = System.Drawing.Color.White;
            this.uxLogo.BackgroundImage = global::ProjectMercury.EffectEditor.Properties.Resources.MercuryLogoWhiteBG;
            this.uxLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.uxLogo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uxLogo.Location = new System.Drawing.Point(12, 12);
            this.uxLogo.Name = "uxLogo";
            this.uxLogo.Size = new System.Drawing.Size(576, 174);
            this.uxLogo.TabIndex = 1;
            // 
            // uxAboutBrowser
            // 
            this.uxAboutBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.uxAboutBrowser.Location = new System.Drawing.Point(12, 192);
            this.uxAboutBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.uxAboutBrowser.Name = "uxAboutBrowser";
            this.uxAboutBrowser.Size = new System.Drawing.Size(576, 199);
            this.uxAboutBrowser.TabIndex = 2;
            this.uxAboutBrowser.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // AboutWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 403);
            this.Controls.Add(this.uxAboutBrowser);
            this.Controls.Add(this.uxLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "AboutWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About Mercury Particle Engine";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel uxLogo;
        private System.Windows.Forms.WebBrowser uxAboutBrowser;
    }
}