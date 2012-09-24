/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor.TreeNodes
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using ProjectMercury.Controllers;
    using ProjectMercury.Emitters;
    using ProjectMercury.Modifiers;

    public class EmitterTreeNode : TreeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmitterTreeNode"/> class.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        public EmitterTreeNode(AbstractEmitter emitter) : base()
        {
            this.Emitter = emitter;

            this.ForeColor = emitter.Enabled ? SystemColors.WindowText : Color.Gray;

            this.Text = emitter.Enabled ? emitter.Name : String.Format("{0} (Disabled)", emitter.Name);

            this.Emitter.NameChanged += (o, s) => this.Text = this.Emitter.Name;

            this.Tag = emitter;

            foreach (AbstractModifier modifier in emitter.Modifiers)
            {
                ModifierTreeNode node = new ModifierTreeNode(modifier);

                this.Nodes.Add(node);
            }

            foreach (AbstractController controller in emitter.Controllers)
            {
                ControllerTreeNode node = new ControllerTreeNode(controller);

                this.Nodes.Add(node);
            }

            if (emitter.Enabled)
            {
                this.Expand();
            }
        }

        /// <summary>
        /// Gets or sets the emitter.
        /// </summary>
        /// <value>The emitter.</value>
        public AbstractEmitter Emitter { get; private set; }
    }
}