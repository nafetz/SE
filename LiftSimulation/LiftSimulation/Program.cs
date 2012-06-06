
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
#endregion

namespace LiftSimulation
{    
    class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            UserInterface UI = new UserInterface();

            Elevator Elevator = new Elevator();

            Syncronize Sync = new Syncronize(ref Elevator, ref UI);

            Application.Run(UI);         

            //while(true)
            //{
            //    Elevator.CurrentState.Move( Elevator );
            //}
        }
    }
}
