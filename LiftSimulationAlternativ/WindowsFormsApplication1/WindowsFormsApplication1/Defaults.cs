#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
#endregion

namespace LiftSimulationAlternativ
{
    public static class Defaults
    {
        #region Members
        private static int _numberOfFloors = 6;
        private static int _numberOfBasementFloors = 1;
        private static int _maxNumberOfPassengers = 10;
        public  enum Direction { Upward = 1, Downward };
        public enum Door { Open = 1, Closed };

        #endregion

        #region Properties

        /// <summary>
        /// Anzahl Geschosse gesamt (inkl. Kellergeschosse)
        /// </summary>
        public static int Floors
        {
            get { return _numberOfFloors; }
        }

        /// <summary>
        /// Anzahl Kellergeschosse
        /// </summary>
        public static int Basements 
        {
            get { return _numberOfBasementFloors; }
        }

        /// <summary>
        /// Maximal zulässige Anzahl von Fahrgästen in einem Fahrstuhl
        /// </summary>
        public static int MaximumPassengers
        {
            get { return _maxNumberOfPassengers; } 
        }




        #endregion

        #region Methods

        /// <summary>
        /// Ermittelt den Pfad zum Projektverzeichnis
        /// </summary>
        /// <returns>Pfad des VS-Projektes</returns>
        public static string GetProjectPath()
        {
            string projectPath = Environment.CurrentDirectory;

            for (int i = 0; i < 2; i++)
            {
                projectPath = System.IO.Path.GetDirectoryName(projectPath);
            }
            return projectPath;
        }

        /// <summary>
        /// konvertiert anhand Default.Floors und Defaults.Basements 
        /// FloorNr. in nutzbaren List/Array-Index
        /// </summary>
        public static int FloorToIdx( int Floor )
        {
            return ( Floor + Basements );
        }
        #endregion
    }
}
