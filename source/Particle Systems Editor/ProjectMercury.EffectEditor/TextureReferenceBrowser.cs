/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Threading;
    using System.Diagnostics;
    using System.IO;

    internal partial class TextureReferenceBrowser : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureReferenceBrowser"/> class.
        /// </summary>
        public TextureReferenceBrowser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureReferenceBrowser"/> class.
        /// </summary>
        /// <param name="references">The references.</param>
        public TextureReferenceBrowser(IEnumerable<TextureReference> references, NewTextureReferenceEventHandler newHandler)
            : this()
        {
            this.TextureReferences = references;

            this.NewTextureReferenceHandler = newHandler;

            RefreshTextureDisplay();
        }

        /// <summary>
        /// Refreshes the texture display.
        /// </summary>
        private void RefreshTextureDisplay()
        {
            this.uxTextureImageList.Images.Clear();
            this.uxTextureReferences.Items.Clear();

            foreach (TextureReference reference in this.TextureReferences)
            {
                Image image = Image.FromFile(reference.FilePath, true);

                this.uxTextureImageList.Images.Add(reference.GetAssetName(), image.GetThumbnailImage(128, 128, null, IntPtr.Zero));

                ListViewItem item = new ListViewItem
                {
                    Text = reference.GetAssetName(),
                    ImageIndex = this.uxTextureImageList.Images.IndexOfKey(reference.GetAssetName()),
                    ToolTipText = reference.FilePath,
                    Tag = reference
                };

                this.uxTextureReferences.Items.Add(item);
            }
        }
        public IEnumerable<TextureReference> TextureReferences { get; private set; }

        public NewTextureReferenceEventHandler NewTextureReferenceHandler { get; private set; }

        /// <summary>
        /// Gets the selected reference.
        /// </summary>
        /// <value>The selected reference.</value>
        public TextureReference SelectedReference
        {
            get
            {
                if (this.uxTextureReferences.SelectedItems.Count > 0)
                    if (this.uxTextureReferences.SelectedItems[0] != null)
                        return this.uxTextureReferences.SelectedItems[0].Tag as TextureReference;

                return null;
            }
        }

        /// <summary>
        /// Handles the ItemActivate event of the uxTextureReferences control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxTextureReferences_ItemActivate(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the uxImport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxImport_Click(object sender, EventArgs e)
        {
            if (this.uxImportDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in this.uxImportDialog.FileNames)
                {
                    var args = new NewTextureReferenceEventArgs(filePath);

                    this.NewTextureReferenceHandler(this, args);

                    if (args.AddedTextureReference != null)
                    {
                        var texRef = args.AddedTextureReference;

                        Image image = Image.FromFile(texRef.FilePath, true);

                        this.uxTextureImageList.Images.Add(texRef.GetAssetName(), image.GetThumbnailImage(128, 128, null, IntPtr.Zero));

                        ListViewItem item = new ListViewItem
                        {
                            Text = texRef.GetAssetName(),
                            ImageIndex = this.uxTextureImageList.Images.IndexOfKey(texRef.GetAssetName()),
                            ToolTipText = texRef.FilePath,
                            Tag = texRef
                        };

                        this.uxTextureReferences.Items.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the LinkClicked event of the uxTextureLicenseLabel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void uxTextureLicenseLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                Process.Start(Path.Combine(Application.StartupPath,"Textures") + "\\License.txt");
            });
        }
    }
}
