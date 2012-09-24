/*  
 Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    static public class Program
    {
        static internal Core Core { get; private set; }

        [STAThread]
        static public void Main(string[] args)
        {
            Trace.WriteLine("Mercury Particle Engine EffectEditor", "APP");
            
            using (new TraceIndenter())
            {
                Trace.WriteLine("Date: " + DateTime.Now.ToString());
                Trace.WriteLine("Version: " + Application.ProductVersion);
                Trace.WriteLine("Culture: " + Application.CurrentCulture);
                Trace.WriteLine("Path: " + Application.StartupPath);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (Program.Core = new Core())
            {
                Application.Run(Program.Core);
            }
        }
    }
}