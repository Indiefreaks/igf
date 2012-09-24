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
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;
    using ProjectMercury.PluginContracts;
    using ProjectMercury.EffectEditor.TreeNodes;
    using ProjectMercury.Controllers;
    using ProjectMercury.Emitters;
    using ProjectMercury.Modifiers;
    using ProjectMercury.Renderers;

    [Export(typeof(IInterfaceProvider))]
    internal partial class UserInterface : Form, IInterfaceProvider
    {
        private readonly object PadLock = new object();

        public event EventHandler Ready;

        protected virtual void OnReady(EventArgs e)
        {
            Trace.WriteLine("UserInterface reporting ready...", "UI");

            var handler = Interlocked.CompareExchange(ref this.Ready, null, null);

            if (handler != null)
                handler.Invoke(this, e);
        }

        public event SerializeEventHandler Serialize;

        protected virtual void OnSerialize(SerializeEventArgs e)
        {
            Trace.WriteLine("User requires serialize of particle effect...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Filepath: " + e.FilePath);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.Serialize, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event SerializeEventHandler Deserialize;

        protected virtual void OnDeserialize(SerializeEventArgs e)
        {
            Trace.WriteLine("User requires deserialize of particle effect...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Filepath:" + e.FilePath);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.Deserialize, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event NewEmitterEventHandler EmitterAdded;

        protected virtual void OnEmitterAdded(NewEmitterEventArgs e)
        {
            Trace.WriteLine("User requires adding an emitter...", "UI");

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.EmitterAdded, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event CloneEmitterEventHandler EmitterCloned;

        protected virtual void OnEmitterCloned(CloneEmitterEventArgs e)
        {
            Trace.WriteLine("User requires cloning an emitter...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Prototype: " + e.Prototype.Name);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.EmitterCloned, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event EmitterEventHandler EmitterRemoved;

        protected virtual void OnEmitterRemoved(EmitterEventArgs e)
        {
            Trace.WriteLine("User requires removing an emitter...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Emitter: " + e.Emitter.Name);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.EmitterRemoved, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event NewModifierEventHandler ModifierAdded;

        protected virtual void OnModifierAdded(NewModifierEventArgs e)
        {
            Trace.WriteLine("User requires adding a modifier...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Emitter: " + e.ParentEmitter.Name);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.ModifierAdded, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event CloneModifierEventHandler ModifierCloned;

        protected virtual void OnModifierCloned(CloneModifierEventArgs e)
        {
            Trace.WriteLine("User requires cloning a modifier...", "UI");

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.ModifierCloned, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event ModifierEventHandler ModifierRemoved;

        protected virtual void OnModifierRemoved(ModifierEventArgs e)
        {
            Trace.WriteLine("User requires removing a modifier...");

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.ModifierRemoved, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event NewControllerEventHandler ControllerAdded;

        protected virtual void OnControllerAdded(NewControllerEventArgs e)
        {
            Trace.WriteLine("User requires adding a controller...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Emitter: " + e.ParentEmitter.Name);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.ControllerAdded, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event CloneControllerEventHandler ControllerCloned;

        protected virtual void OnControllerCloned(CloneControllerEventArgs e)
        {
            Trace.WriteLine("User requires cloning a controller...", "UI");

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.ControllerCloned, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event ControllerEventHandler ControllerRemoved;

        protected virtual void OnControllerRemoved(ControllerEventArgs e)
        {
            Trace.WriteLine("User requires removing a controller...");

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.ControllerRemoved, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event EmitterReinitialisedEventHandler EmitterReinitialised;

        protected virtual void OnEmitterReinitialised(EmitterReinitialisedEventArgs e)
        {
            Trace.WriteLine("User requires reinitialising emitter...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Budget: " + e.Budget);
                Trace.WriteLine("Term: " + e.Term);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.EmitterReinitialised, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event NewTextureReferenceEventHandler TextureReferenceAdded;

        protected virtual void OnTextureReferenceAdded(NewTextureReferenceEventArgs e)
        {
            Trace.WriteLine("User requires adding a texture reference...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Filepath: " + e.FilePath);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.TextureReferenceAdded, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event TextureReferenceChangedEventHandler TextureReferenceChanged;

        protected virtual void OnTextureReferenceChanged(TextureReferenceChangedEventArgs e)
        {
            Trace.WriteLine("User requires assigning texture reference to emitter...", "UI");

            using (new TraceIndenter())
            {
                Trace.WriteLine("Emitter: " + e.Emitter.Name);
                Trace.WriteLine("Texture: " + e.TextureReference.FilePath);
            }

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.TextureReferenceChanged, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }

            this.AssertOperationOK(e.Result);
        }

        public event EventHandler NewParticleEffect;

        protected virtual void OnNewParticleEffect(EventArgs e)
        {
            Trace.WriteLine("User requires new particle effect...", "UI");

            using (new HourglassCursor())
            {
                var handler = Interlocked.CompareExchange(ref this.NewParticleEffect, null, null);

                if (handler != null)
                    handler.Invoke(this, e);
            }
        }

        public IEnumerable<TextureReference> TextureReferences { get; set; }

        public void SetEmitterPlugins(IEnumerable<IEmitterPlugin> plugins)
        {
            foreach (IEmitterPlugin plugin in plugins)
                this.AddEmitterPlugin(plugin);
        }

        public void SetModifierPlugins(IEnumerable<IModifierPlugin> plugins)
        {
            foreach (IModifierPlugin plugin in plugins)
                this.AddModifierPlugin(plugin);
        }

        public void SetControllerPlugins(IEnumerable<IControllerPlugin> plugins)
        {
            foreach (var plugin in plugins)
                this.AddControllerPlugin(plugin);
        }

        public void SetSerializationPlugins(IEnumerable<ISerializerPlugin> plugins)
        {
            foreach (ISerializerPlugin plugin in plugins)
                this.AddSerializationPlugin(plugin);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectEditorWindow"/> class.
        /// </summary>
        public UserInterface()
        {
            this.InitializeComponent();

            this.TriggerTimer = new Stopwatch();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.OnReady(EventArgs.Empty);

            this.DisplayLibraryEffects();
        }

        private bool MouseButtonPressed;

        private Point LocalMousePosition;

        private Color PreviewBackgroundColor = Color.Black;

        /// <summary>
        /// Displays the library effects.
        /// </summary>
        private void DisplayLibraryEffects()
        {
            Trace.WriteLine("Searching for library particle effects...", "UI");

            DirectoryInfo effectsDir = new DirectoryInfo(Application.StartupPath + "\\EffectLibrary");

            foreach (FileInfo file in effectsDir.GetFiles())
            {
                using (new TraceIndenter())
                {
                    Trace.WriteLine("File: " + file.FullName);
                }

                bool pluginFound = false;

                foreach (ISerializerPlugin plugin in (from ToolStripItem menuItem in this.uxImportMenuItem.DropDownItems
                                                               where menuItem.Tag is ISerializerPlugin
                                                               select menuItem.Tag as ISerializerPlugin))
                {
                    if (plugin.FileFilter.Contains(file.Extension))
                    {
                        //this.uxLibraryImageList.Images.Add(file.Name, plugin.DisplayIcon);

                        ListViewItem item = new ListViewItem
                        {
                            Text = file.Name,
                            Tag = plugin,
                            //ImageIndex = this.uxLibraryImageList.Images.IndexOfKey(file.Name),
                            //StateImageIndex = this.uxLibraryImageList.Images.IndexOfKey(file.Name)
                        };

                        this.uxLibraryListView.Items.Add(item);

                        pluginFound = true;

                        break;
                    }
                }

                if (!pluginFound)
                    Trace.TraceWarning("Could not find serialization plugin for effect library file: " + file.FullName);
            }
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuToggleEffectTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuToggleEffectTree_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.uxOuterSplitContainer.Panel1Collapsed = !this.uxMainMenuToggleEffectTree.Checked;
                });
            });
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuTogglePropertyBrowser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuTogglePropertyBrowser_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.uxInnerSplitContainer.Panel2Collapsed = !this.uxMainMenuTogglePropertyBrowser.Checked;
                });
            });
        }

        /// <summary>
        /// Handles the AfterSelect event of the uxEffectTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        private void uxEffectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.uxPropertyBrowser.SelectedObject = e.Node.Tag;
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuHomepage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuHomepage_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("User requires home page link...", "UI");

            ThreadPool.QueueUserWorkItem(s => Process.Start("http://mpe.codeplex.com/"));
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuAbout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuAbout_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("User requires about box...", "UI");

            using (AboutWindow about = new AboutWindow())
            {
                about.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuOptions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuOptions_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("User requires options window...", "UI");

            using (OptionsWindow options = new OptionsWindow
                {
                    BackgroundColor = this.PreviewBackgroundColor,
                    BackgroundColourPickedCallback = new Action<Color>(delegate(Color color)
                        {
                            Trace.WriteLine("User changes background color...", "UI");

                            this.PreviewBackgroundColor = color;

                            this.uxEffectPreview.SetBackgroundColor(color.R, color.G, color.B);
                        }),
                    BackgroundImagePickedCallback = new Action<string>(delegate(string filePath)
                        {
                            Trace.WriteLine("User changes background image...", "UI");

                            this.uxEffectPreview.LoadBackgroundImage(filePath);
                        }),
                    BackgroundImageClearedCallback = new Action(delegate
                        {
                            Trace.WriteLine("User clears background image...", "UI");

                            this.uxEffectPreview.ClearBackgroundImage();
                        }),
                    BackgroundImageOptionsCallback = new Action<ImageOptions>(delegate(ImageOptions imageOptions)
                        {
                            Trace.WriteLine("User changes background image layout...", "UI");

                            this.uxEffectPreview.ImageOptionsChanged(imageOptions);
                        }),
                })
            {
                options.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the KeyUp event of the uxEffectTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void uxEffectTree_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (this.uxEffectTree.SelectedNode is EmitterTreeNode)
                {
                    AbstractEmitter emitter = (this.uxEffectTree.SelectedNode as EmitterTreeNode).Emitter;

                    this.OnEmitterRemoved(new EmitterEventArgs(emitter));
                }

                if (this.uxEffectTree.SelectedNode is ModifierTreeNode)
                {
                    AbstractModifier modifier = (this.uxEffectTree.SelectedNode as ModifierTreeNode).Modifier;

                    this.OnModifierRemoved(new ModifierEventArgs(modifier));
                }

                this.uxEffectTree.SelectedNode.Remove();
            }
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuExit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuExit_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("User requires program exit...", "UI");

            Application.Exit();
        }

        /// <summary>
        /// Adds the copy plugin to the interface.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        private void AddEmitterPlugin(IEmitterPlugin plugin)
        {
            Trace.WriteLine("Adding menu item for '" + plugin.Name + "' plugin...", "UI");

            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = plugin.Name,
                ToolTipText = plugin.Description,
                Image = Icons.Emitter.ToBitmap(), //plugin.DisplayIcon,
                Tag = plugin
            };

            this.uxAddEmitterMenuItem.DropDownItems.Add(item);
        }

        /// <summary>
        /// Adds the modifier plugin to the interface.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        private void AddModifierPlugin(IModifierPlugin plugin)
        {
            Trace.WriteLine("Adding menu item for '" + plugin.Name + "' plugin...", "UI");

            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = plugin.Name,
                ToolTipText = plugin.Description,
                Image = Icons.Modifier.ToBitmap(), //plugin.DisplayIcon,
                Tag = plugin
            };

            bool foundCategory = false;

            foreach (ToolStripMenuItem categoryItem in this.uxAddModifierMenuItem.DropDownItems)
            {
                if (categoryItem.Text.Equals(plugin.Category))
                {
                    categoryItem.DropDownItems.Add(item);

                    foundCategory = true;
                }
            }

            if (!foundCategory)
            {
                ToolStripMenuItem newCategoryItem = new ToolStripMenuItem(plugin.Category);

                newCategoryItem.DropDownItemClicked += this.uxAddModifierMenuItem_DropDownItemClicked;

                this.uxAddModifierMenuItem.DropDownItems.Add(newCategoryItem);

                newCategoryItem.DropDownItems.Add(item);
            }

            //this.uxAddModifierMenuItem.DropDownItems.Add(item);
        }

        private void AddControllerPlugin(IControllerPlugin plugin)
        {
            Trace.WriteLine("Adding menu item for '" + plugin.Name + "' plugin...", "UI");

            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = plugin.Name,
                ToolTipText = plugin.Description,
                Image = Icons.ParticleEffect.ToBitmap(), //plugin.DisplayIcon,
                Tag = plugin
            };

            this.uxAddControllerMenuItem.DropDownItems.Add(item);
        }

        /// <summary>
        /// Adds the serialization plugin to the interface.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        private void AddSerializationPlugin(ISerializerPlugin plugin)
        {
            Trace.WriteLine("Adding menu item for '" + plugin.Name + "' plugin...", "UI");

            //TODO: Perhaps it would be better to load the plugins and don't add them to the uxImportMenuItem
            //Instead we have a simple import button, and it has the filetypes with the supported plugins.
            //Example: .em and .pe (or something similar) are the only visible types in the import dialog.
            //Doing it this way will prevent loading of dynamic menu items and fix the bug where the menu items is
            //still displayed even tho the load dialog has been displayed.
            if (plugin.CanDeserialize)
            {
                ToolStripMenuItem item = new ToolStripMenuItem
                {
                    //Image = Image.FromFile(plugin.DisplayIcon.AbsolutePath),
                    Text = plugin.Name,
                    ToolTipText = plugin.Description,
                    Tag = plugin
                };

                this.uxImportMenuItem.DropDownItems.Add(item);
            }

            if (plugin.CanSerialize)
            {
                ToolStripMenuItem item = new ToolStripMenuItem
                {
                    //Image = Image.FromFile(plugin.DisplayIcon.AbsolutePath),
                    Text = plugin.Name,
                    ToolTipText = plugin.Description,
                    Tag = plugin
                };

                this.uxExportMenuItem.DropDownItems.Add(item);
            }
        }

        /// <summary>
        /// Sets the particle effect.
        /// </summary>
        /// <param name="effect">The effect.</param>
        public void SetParticleEffect(ParticleEffect effect)
        {
            this.uxEffectTree.Nodes.Clear();

            ParticleEffectTreeNode node = new ParticleEffectTreeNode(effect);

            this.uxEffectTree.Nodes.Add(node);

            this.uxEffectTree.SelectedNode = node;

            node.Expand();
        }

        /// <summary>
        /// Sets the amount of time it took to update the particle effect.
        /// </summary>
        /// <param name="totalSeconds"></param>
        public void SetUpdateTime(float totalSeconds)
        {
            this.uxUpdateTimeLabel.Text = String.Format("Update: {0:F5} seconds", totalSeconds);

            float framePercent = (totalSeconds / 0.016f) * 100f;

            framePercent = framePercent >= 100f ? 100f : framePercent;

            this.uxFrameRateProgress.Value = (int)framePercent;
        }

        /// <summary>
        /// Handles the DropDownItemClicked event of the uxImportMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        private void uxImportMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var plugin = e.ClickedItem.Tag as ISerializerPlugin;

            this.uxImportEffectDialog.Filter = plugin.FileFilter;

            if (this.uxImportEffectDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var filePath = this.uxImportEffectDialog.FileName;

                    var args = new SerializeEventArgs(plugin, filePath);

                    this.OnDeserialize(args);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Handles the DropDownItemClicked event of the uxExportMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        private void uxExportMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var plugin = e.ClickedItem.Tag as ISerializerPlugin;

            this.uxExportEffectDialog.Filter = plugin.FileFilter;

            if (this.uxExportEffectDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var filePath = this.uxExportEffectDialog.FileName;

                    var args = new SerializeEventArgs(plugin, filePath);

                    this.OnSerialize(args);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Draws the specified effect.
        /// </summary>
        /// <param name="effect">The effect.</param>
        /// <param name="renderer">The renderer.</param>
        public void Draw(ParticleEffect effect, AbstractRenderer renderer)
        {
            this.uxActiveParticlesLabel.Text = String.Format("{0} Active Particles", effect.ActiveParticlesCount);

            this.uxEffectPreview.ParticleEffect = effect;
            this.uxEffectPreview.Renderer = renderer;

            this.uxEffectPreview.Invalidate();
        }

        /// <summary>
        /// Handles the MouseDown event of the uxEffectPreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void uxEffectPreview_MouseDown(object sender, MouseEventArgs e)
        {
            this.MouseButtonPressed = true;

            this.LocalMousePosition = new Point
            {
                X = e.Location.X - (this.uxEffectPreview.Width / 2),
                Y = e.Location.Y - (this.uxEffectPreview.Height / 2),
            };

            this.uxStatusLabel.Text = this.LocalMousePosition.ToString();

            this.TriggerTimer.Start();
        }

        /// <summary>
        /// Handles the MouseUp event of the uxEffectPreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void uxEffectPreview_MouseUp(object sender, MouseEventArgs e)
        {
            this.MouseButtonPressed = false;

            this.uxStatusLabel.Text = "Ready";

            this.TriggerTimer.Stop();
        }

        /// <summary>
        /// Handles the MouseMove event of the uxEffectPreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void uxEffectPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.MouseButtonPressed)
            {
                this.LocalMousePosition = new Point
                {
                    X = e.Location.X - (this.uxEffectPreview.Width / 2),
                    Y = e.Location.Y - (this.uxEffectPreview.Height / 2),
                };

                this.uxStatusLabel.Text = this.LocalMousePosition.ToString();
            }
        }

        private Stopwatch TriggerTimer { get; set; }

        /// <summary>
        /// Gets a value indicating wether or not a trigger is required.
        /// </summary>
        /// <param name="x">The x location of the trigger.</param>
        /// <param name="y">The y location of the trigger.</param>
        public bool TriggerRequired(out float x, out float y)
        {
            x = this.LocalMousePosition.X;
            y = this.LocalMousePosition.Y;

            if (this.MouseButtonPressed)
            {
                if (this.TriggerTimer.Elapsed.TotalSeconds > (1f / 60f))
                {
                    this.TriggerTimer.Reset();
                    this.TriggerTimer.Start();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Handles the Opening event of the uxEffectTreeContextMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void uxEffectTreeContextMenu_Opening(object sender, CancelEventArgs e)
        {
            TreeNode selectedNode = this.uxEffectTree.SelectedNode;

            this.uxAddEmitterMenuItem.Visible = selectedNode is ParticleEffectTreeNode;
            this.uxAddModifierMenuItem.Visible = selectedNode is EmitterTreeNode;
            this.uxAddControllerMenuItem.Visible = selectedNode is EmitterTreeNode;
            this.uxReinitialiseEmitterMenuItem.Visible = selectedNode is EmitterTreeNode;
            this.uxSelectTextureMenuItem.Visible = selectedNode is EmitterTreeNode;
            this.uxToggleEmitterEnabledMenuItem.Visible = selectedNode is EmitterTreeNode;

            this.uxEffectTreeContextMenuSeperator.Visible = (selectedNode is ParticleEffectTreeNode) == false;
            this.uxDeleteMenuItem.Visible = (selectedNode is ParticleEffectTreeNode) == false;
            this.uxCloneMenuItem.Visible = (selectedNode is ParticleEffectTreeNode) == false;

            if (selectedNode is EmitterTreeNode)
            {
                AbstractEmitter emitter = (selectedNode as EmitterTreeNode).Emitter;

                this.uxToggleEmitterEnabledMenuItem.Text = emitter.Enabled ? "Disable" : "Enable";
            }
        }

        /// <summary>
        /// Handles the DropDownItemClicked event of the uxAddEmitterMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        private void uxAddEmitterMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                using (NewEmitterDialog dialog = new NewEmitterDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        IEmitterPlugin plugin = e.ClickedItem.Tag as IEmitterPlugin;

                        var args = new NewEmitterEventArgs(plugin, dialog.EmitterBudget, dialog.EmitterTerm);

                        this.OnEmitterAdded(args);

                        if (args.AddedEmitter != null)
                        {
                            AbstractEmitter emitter = args.AddedEmitter;

                            EmitterTreeNode node = new EmitterTreeNode(emitter);

                            this.uxEffectTree.Nodes[0].Nodes.Add(node);

                            node.EnsureVisible();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handles the DropDownItemClicked event of the uxAddModifierMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        private void uxAddModifierMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                EmitterTreeNode parentNode = this.uxEffectTree.SelectedNode as EmitterTreeNode;

                IModifierPlugin plugin = e.ClickedItem.Tag as IModifierPlugin;

                AbstractEmitter parent = parentNode.Emitter;

                var args = new NewModifierEventArgs(parent, plugin);

                this.OnModifierAdded(args);

                if (args.AddedModifier != null)
                {
                    AbstractModifier modifier = args.AddedModifier;

                    ModifierTreeNode node = new ModifierTreeNode(modifier);

                    parentNode.Nodes.Add(node);

                    node.EnsureVisible();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handles the DropDownItemClicked event of the uxAddControllerMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        private void uxAddControllerMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                EmitterTreeNode parentNode = this.uxEffectTree.SelectedNode as EmitterTreeNode;

                IControllerPlugin plugin = e.ClickedItem.Tag as IControllerPlugin;

                AbstractEmitter parent = parentNode.Emitter;

                var args = new NewControllerEventArgs(parent, plugin);

                this.OnControllerAdded(args);

                if (args.AddedController!= null)
                {
                    AbstractController controller = args.AddedController;

                    ControllerTreeNode node = new ControllerTreeNode(controller);

                    parentNode.Nodes.Add(node);

                    node.EnsureVisible();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handles the Click event of the uxCloneMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxCloneMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = this.uxEffectTree.SelectedNode;

            if (selectedNode is EmitterTreeNode)
            {
                CloneEmitterEventArgs args = new CloneEmitterEventArgs((selectedNode as EmitterTreeNode).Emitter);

                this.OnEmitterCloned(args);

                if (args.AddedEmitter != null)
                {
                    EmitterTreeNode newNode = new EmitterTreeNode(args.AddedEmitter);

                    selectedNode.Parent.Nodes.Add(newNode);

                    newNode.Expand();

                    newNode.EnsureVisible();
                }
            }
            else if (selectedNode is ModifierTreeNode)
            {
                CloneModifierEventArgs args = new CloneModifierEventArgs((selectedNode as ModifierTreeNode).Modifier);

                this.OnModifierCloned(args);

                if (args.AddedModifier != null)
                {
                    ModifierTreeNode newNode = new ModifierTreeNode(args.AddedModifier);

                    selectedNode.Parent.Nodes.Add(newNode);

                    newNode.EnsureVisible();
                }
            }
            else if (selectedNode is ControllerTreeNode)
            {
                var args = new CloneControllerEventArgs((selectedNode as ControllerTreeNode).Controller);

                this.OnControllerCloned(args);

                if (args.AddedController != null)
                {
                    ControllerTreeNode newNode = new ControllerTreeNode(args.AddedController);

                    selectedNode.Parent.Nodes.Add(newNode);

                    newNode.Expand();

                    newNode.EnsureVisible();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the uxDeleteMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxDeleteMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = this.uxEffectTree.SelectedNode;

            if (selectedNode is EmitterTreeNode)
            {
                try
                {
                    AbstractEmitter emitter = (selectedNode as EmitterTreeNode).Emitter;

                    this.OnEmitterRemoved(new EmitterEventArgs(emitter));

                    selectedNode.Remove();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else if (selectedNode is ModifierTreeNode)
            {
                try
                {
                    AbstractModifier modifier = (selectedNode as ModifierTreeNode).Modifier;

                    this.OnModifierRemoved(new ModifierEventArgs(modifier));

                    selectedNode.Remove();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (selectedNode is ControllerTreeNode)
            {
                try
                {
                    var controller = (selectedNode as ControllerTreeNode).Controller;

                    this.OnControllerRemoved(new ControllerEventArgs(controller));

                    selectedNode.Remove();
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the uxReinitialiseEmitter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxReinitialiseEmitter_Click(object sender, EventArgs e)
        {
            EmitterTreeNode node = this.uxEffectTree.SelectedNode as EmitterTreeNode;

            using (NewEmitterDialog dialog = new NewEmitterDialog(node.Emitter.Budget, node.Emitter.Term))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var args = new EmitterReinitialisedEventArgs(node.Emitter, dialog.EmitterBudget, dialog.EmitterTerm);

                    this.OnEmitterReinitialised(args);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuViewTextures control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuViewTextures_Click(object sender, EventArgs e)
        {
            using (TextureReferenceBrowser browser = new TextureReferenceBrowser(this.TextureReferences, this.TextureReferenceAdded))
            {
                browser.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the Click event of the uxSelectTexture control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxSelectTexture_Click(object sender, EventArgs e)
        {
            using (TextureReferenceBrowser browser = new TextureReferenceBrowser(this.TextureReferences, this.TextureReferenceAdded))
            {
                if (browser.ShowDialog() == DialogResult.OK)
                {
                    EmitterTreeNode node = this.uxEffectTree.SelectedNode as EmitterTreeNode;

                    var args = new TextureReferenceChangedEventArgs(node.Emitter, browser.SelectedReference);

                    this.OnTextureReferenceChanged(args);

                    this.uxPropertyBrowser.Refresh();
                }
            }
        }

        /// <summary>
        /// Handles the ItemActivate event of the uxLibraryListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxLibraryListView_ItemActivate(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you wish to open this effect? Unsaved changes will be lost.",
                                "Confirm",
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ListViewItem item = uxLibraryListView.SelectedItems[0];

                ISerializerPlugin plugin = item.Tag as ISerializerPlugin;

                string filePath = Application.StartupPath + "\\EffectLibrary\\" + item.Text;

                this.OnDeserialize(new SerializeEventArgs(plugin, filePath));
            }
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuNew_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you wish to start a new effect? Unsaved changed will be lost.",
                                "Confirm",
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.OnNewParticleEffect(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the Click event of the uxMainMenuDocumentation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuDocumentation_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("User requires documentation link...", "UI");

            ThreadPool.QueueUserWorkItem(s => Process.Start("http://mpe.codeplex.com/documentation"));
        }

        /// <summary>
        /// Handles the click event of the uxMainMenuLog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxMainMenuLog_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("User requires log file...", "UI");

            ThreadPool.QueueUserWorkItem(s => Process.Start("Trace.log"));
        }

        /// <summary>
        /// Handles the Click event of the uxAPIReferenceMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxAPIReferenceMenuItem_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("User requires API reference file...", "UI");

            ThreadPool.QueueUserWorkItem(s => Process.Start(".\\Reference\\Documentation.chm"));
        }

        /// <summary>
        /// Handles the CheckedChanged event of the uxToggleEmitterEnabledMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uxToggleEmitterEnabledMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            AbstractEmitter emitter = (this.uxEffectTree.SelectedNode as EmitterTreeNode).Emitter;

            if (emitter.Enabled == false)
            {
                this.uxEffectTree.SelectedNode.ForeColor = SystemColors.WindowText;
                
                this.uxEffectTree.SelectedNode.Text = this.uxEffectTree.SelectedNode.Text.Replace(" (Disabled)", "");
                
                this.uxEffectTree.SelectedNode.Expand();

                emitter.Enabled = true;
            }
            else
            {
                this.uxEffectTree.SelectedNode.Collapse();

                this.uxEffectTree.SelectedNode.ForeColor = Color.Gray;
                
                this.uxEffectTree.SelectedNode.Text = String.Format("{0} (Disabled)", this.uxEffectTree.SelectedNode.Text);

                emitter.Enabled = false;
            }
        }

        private void AssertOperationOK(CoreOperationResult operationResult)
        {
            if (operationResult == CoreOperationResult.OK)
                return;

            MessageBox.Show("An error occured, the error message will now be written to Trace.log",
                            "Program Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

            this.LogError(operationResult.Exception);
        }

        private void LogError(Exception error)
        {
            Trace.TraceError(error.ToString());
        }
    }
}
