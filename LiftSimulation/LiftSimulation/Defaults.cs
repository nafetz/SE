using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;


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

        #endregion


        #region interne Klassen/Structs

        public struct Logentry
        {
            public Direction _direction;
            public int _floor;
            public int _passenger;
            public State _state;

            public Logentry(Direction d, int f, int p, State s)
            {
                _direction = d;
                _floor = f;
                _passenger = p;
                _state = s;
            }
        }
        public static class Log
        {
            #region Member

            private string _path;
            private StreamWriter _logger;

            #endregion


            #region Konstruktor

            public Log()
            {
                _path = System.IO.Path.GetTempPath();

                if( !File.Exists( _path + @"\Elevator_log.txt" ) )
                {
                    File.Create( _path + @"\Elevator_log.txt" );
                }

                _logger = new StreamWriter( _path + @"\Elevator_log.txt" );

                _logger.WriteLine( "\n\n" );
                _logger.WriteLine( "Begin New Log of " + DateTime.Now + "." );
            }

            #endregion


            #region Destruktor

            ~Log()
            {
                _logger.WriteLine( "End of Log." );
                _logger.Close();
            }

            #endregion


            #region Methoden

            /// <summary>
            /// Fügt dem Logfile einen neuen Eintrag hinzu (als neue Zeile)
            /// </summary>
            /// <param name="entry">hinzuzufügender Eintrag</param>
            public void AddEntry( string entry )
            {
                _logger.WriteLine( entry );
            }

            #endregion
        }
        
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


        #region Methoden

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
        public static int FloorToIdx( int floor )
        {
            return ( floor + _numberOfBasementFloors );
        }

        /// <summary>
        /// konvertiert anhand Default.Floors und Defaults.Basements 
        /// Index in nutzbare FloorNr.
        /// </summary>
        public static int IdxToFloor( int idx )
        {
               return ( idx - _numberOfBasementFloors );
        }
        #endregion
    }
}
