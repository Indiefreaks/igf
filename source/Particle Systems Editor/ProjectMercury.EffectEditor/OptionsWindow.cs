/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class OptionsWindow : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsWindow"/> class.
        /// </summary>
        public OptionsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public Color BackgroundColor
        {
            get { return this.uxBackgroundColour.BackColor; }
            set { this.uxBackgroundColour.BackColor = value; }
        }

        /// <summary>
        /// Gets or sets the background colour picked callback.
        /// </summary>
        /// <value>The background colour picked callback.</value>
        public Action<Color> BackgroundColourPickedCallback { get; set; }

        /// <summary>
        /// Gets or sets the background image picked callback.
        /// </summary>
        /// <value>The background image picked callback.</value>
        public Action<String> BackgroundImagePickedCallback { get; set; }

        /// <summary>
        /// Gets or sets the background image options callback.
        /// </summary>
        /// <value>The background image options callback.</value>
        public Action<ImageOptions> BackgroundImageOptionsCallback { get; set; }

        /// <summary>
        /// Gets or sets the background image cleared callback.
        /// </summary>
        /// <value>The background image cleared callback.</value>
        public Action BackgroundImageClearedCallback { get; set; }

        /// <summary>
        /// Handles the Click event of the uxChangeBackgroundColour control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxChangeBackgroundColour_Click(object sender, EventArgs e)
        {
            if (this.uxColourPicker.ShowDialog() == DialogResult.OK)
            {
                this.uxBackgroundColour.BackColor = this.uxColourPicker.Color;

                if (this.BackgroundColourPickedCallback != null)
                    this.BackgroundColourPickedCallback(this.uxColourPicker.Color);
            }
        }

        /// <summary>
        /// Handles the Click event of the uxBrowseBackgroundImage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxBrowseBackgroundImage_Click(object sender, EventArgs e)
        {
            if (this.uxBackgroundImageDialog.ShowDialog() == DialogResult.OK)
            {
                if (this.BackgroundImagePickedCallback != null)
                    this.BackgroundImagePickedCallback(this.uxBackgroundImageDialog.FileName);
            }
        }

        /// <summary>
        /// Handles the Click event of the uxClearBackgroundImage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxClearBackgroundImage_Click(object sender, EventArgs e)
        {
            if (this.BackgroundImageClearedCallback != null)
                this.BackgroundImageClearedCallback();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rbStretch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void rbStretch_CheckedChanged(object sender, EventArgs e)
        {
            if (this.BackgroundImageOptionsCallback != null)
                this.BackgroundImageOptionsCallback(ImageOptions.Stretch);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rbCenter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void rbCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (this.BackgroundImageOptionsCallback != null)
                this.BackgroundImageOptionsCallback(ImageOptions.Center);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rbTile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void rbTile_CheckedChanged(object sender, EventArgs e)
        {
            if (this.BackgroundImageOptionsCallback != null)
                this.BackgroundImageOptionsCallback(ImageOptions.Tile);
        }
    }
}
