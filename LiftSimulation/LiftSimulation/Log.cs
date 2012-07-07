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

        private static string _path = System.IO.Path.GetTempPath();
        private static StreamWriter _logger = null;

        private static bool _firstUsage = true;

        #endregion


        #region Methoden

        /// <summary>
        /// Fügt dem Logfile einen neuen Eintrag hinzu (als neue Zeile)
        /// </summary>
        /// <param name="entry">hinzuzufügender Eintrag</param>
        public static void AddEntry( string entry )
        {
            if (_firstUsage)
            {
                _logger = new StreamWriter(_path + @"\Elevator_log_" + DateTime.Now.ToString().Replace(".","_").Replace(" ", "_").Replace(":","_") + ".txt");
                _logger.WriteLine("Logfile vom " + DateTime.Now);
                _logger.WriteLine("Tester: " + System.Environment.UserName);
                _logger.WriteLine("");
                _firstUsage = false;
            }

            _logger.WriteLine( DateTime.Now.ToString().Substring(11) + " Uhr: " + entry );
        }

        /// <summary>
        /// Schließt _logger, sofern dieser initialisiert wurde
        /// </summary>
        public static void Close()
        {
            if(_logger!=null)
                _logger.Close();
        }
        #endregion
    }
}
