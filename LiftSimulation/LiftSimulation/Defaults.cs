#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
#endregion

namespace LiftSimulation
{
    public static class Defaults
    {
        #region Members
        private static int _numberOfFloors = 6;
        private static int _numberOfBasementFloors = 1;
        private static int _maxNumberOfPassengers = 10;
        public  enum State { Moving = 1, FixedOpen, FixedClosed, Overload };
        public  enum Direction { Upward = 1, Downward };
        public  enum MoreOrLess { More = 1, Less, Neither };

        private static readonly ManualResetEvent _mre = new ManualResetEvent(false);
        #endregion

        #region Properties

        /// <summary>
        /// Anzahl Geschosse gesamt (inkl. Kellergeschosse)
        /// </summary>
        public static int Floors
        {
            get { return _numberOfFloors; }
            //set { _numberOfFloors = value; }
        }

        /// <summary>
        /// Anzahl Kellergeschosse
        /// </summary>
        public static int Basements 
        {
            get { return _numberOfBasementFloors; }
           // set { _numberOfBasementFloors = value; }
        }

        /// <summary>
        /// Maximal zulässige Anzahl von Fahrgästen in einem Fahrstuhl
        /// </summary>
        public static int MaximumPassengers
        {
            get { return _maxNumberOfPassengers; } 
        }

        public static ManualResetEvent ManualResetEvent
        {
            get { return _mre; }
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

        /// <summary>
        /// konvertiert anhand Default.Floors und Defaults.Basements 
        /// Index in nutzbare FloorNr.
        /// </summary>
        public static int IdxToFloor( int IDX )
        {
               return ( IDX - Basements );
        }
        #endregion
    }
}
