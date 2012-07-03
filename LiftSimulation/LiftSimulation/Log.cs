using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LiftSimulation
{
    public static class Log
    {
        #region Member

        private static string _path;
        private static StreamWriter _logger;

        #endregion


        #region Konstruktor

        //public Log()
        //{
        //    _path = System.IO.Path.GetTempPath();

        //    if( !File.Exists( _path + @"\Elevator_log.txt" ) )
        //    {
        //        File.Create( _path + @"\Elevator_log.txt" );
        //    }

        //    _logger = new StreamWriter( _path + @"\Elevator_log.txt" );

        //    _logger.WriteLine( "\n\n" );
        //    _logger.WriteLine( "Begin New Log of " + DateTime.Now + "." );
        //}

        #endregion


        #region Destruktor

        //~Log()
        //{
        //    _logger.WriteLine( "End of Log." );
        //    _logger.Close();
        //}

        #endregion


        #region Methoden

        /// <summary>
        /// Fügt dem Logfile einen neuen Eintrag hinzu (als neue Zeile)
        /// </summary>
        /// <param name="entry">hinzuzufügender Eintrag</param>
        public static void AddEntry( string entry )
        {
            _logger.WriteLine( entry );
        }

        #endregion
    }
}
