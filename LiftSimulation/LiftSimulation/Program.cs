
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
#endregion

namespace LiftSimulation
{    
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UserInterface());

            Elevator Elevator = new Elevator();
        }
    }
}
