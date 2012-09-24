/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor.TreeNodes
{
    using System;
    using System.Windows.Forms;
    using ProjectMercury.Emitters;

    public class ParticleEffectTreeNode : TreeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectTreeNode"/> class.
        /// </summary>
        /// <param name="effect">The effect.</param>
        public ParticleEffectTreeNode(ParticleEffect effect)
            : base()
        {
            if (effect == null)
                throw new ArgumentNullException("effect");

            this.ParticleEffect = effect;

            this.Text = effect.Name ?? "Particle Effect";

            this.ParticleEffect.NameChanged += (o, e) => this.Text = this.ParticleEffect.Name;

            this.Tag = effect;

            foreach (AbstractEmitter emitter in effect.Emitters)
            {
                EmitterTreeNode node = new EmitterTreeNode(emitter);

                this.Nodes.Add(node);
            }
        }

        /// <summary>
        /// Gets or sets the particle effect.
        /// </summary>
        /// <value>The particle effect.</value>
        public ParticleEffect ParticleEffect { get; private set; }
    }
}