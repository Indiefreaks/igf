/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System.Windows.Forms;

    internal partial class NewEmitterDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewEmitterDialog"/> class.
        /// </summary>
        public NewEmitterDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewEmitterDialog"/> class.
        /// </summary>
        /// <param name="budget">The budget.</param>
        /// <param name="term">The term.</param>
        public NewEmitterDialog(int budget, float term)
            : this()
        {
            this.uxBudget.Value = (decimal)budget;
            this.uxTerm.Value = (decimal)term;
        }

        /// <summary>
        /// Gets the emitter budget.
        /// </summary>
        /// <value>The emitter budget.</value>
        public int EmitterBudget
        {
            get { return (int)this.uxBudget.Value; }
        }

        /// <summary>
        /// Gets the emitter term.
        /// </summary>
        /// <value>The emitter term.</value>
        public float EmitterTerm
        {
            get { return (float)this.uxTerm.Value; }
        }
    }
}
