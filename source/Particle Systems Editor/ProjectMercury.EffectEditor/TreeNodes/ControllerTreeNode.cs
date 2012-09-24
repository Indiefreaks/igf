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
    using ProjectMercury.Emitters;
    using ProjectMercury.Controllers;

    public class ControllerTreeNode : TreeNode
    {
        public ControllerTreeNode(AbstractController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            this.Controller = controller;

            this.Text = controller.GetType().Name;

            this.Tag = controller;
        }

        public AbstractController Controller { get; private set; }
    }
}