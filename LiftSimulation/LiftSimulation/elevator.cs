#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace LiftSimulation
{
    class Elevator
    {
        #region Member

        // State-Patterns
        private ElevatorState State;
        private ElevatorState Moving = new Moving();
        private ElevatorState FixedOpen = new FixedOpen();
        private ElevatorState FixedClosed = new FixedClosed();

        // Floor-Stuff
        private int _currentFloor;
        List<bool> _upwardRequired = new List<bool>( EnvironmentConditions.Floors );
        List<bool> _downwardRequired = new List<bool>( EnvironmentConditions.Floors );

        // Riding
        /// <summary>
        /// Upward => TRUE  |  Downward => FALSE
        /// </summary>
        private bool Direction;

        #endregion


        #region Properties

        // Für den Zugriff außerhalb des Class-Scope
        // get
        public int CurrentFloor 
        {
            get { return _currentFloor; }
        }
        public List<bool> UpwardRequired 
        {
            get { return _upwardRequired; }
        }
        public List<bool> DownwardRequired 
        {
            get { return _downwardRequired; }
        }

        #endregion


        #region Methods

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Elevator()
        {
            InitOrReset();
        }

        /// <summary>
        /// Setzt fahrstuhl in Ausgangszustand
        /// </summary>
        public void InitOrReset()
        {
            State = FixedClosed;
            _currentFloor = 0;
            
            // ANGEBLICH ARRAY-OUT-OF-BOUND   BITTE PRÜFEN, ICH HAB KEINE AHNUNG
            //
            //for ( int IDX = ( EnvironmentConditions.Floors - 1 ); IDX >= 0; IDX-- ) 
            //{ 
            //    _downwardRequired[ IDX ] = false;
            //    _upwardRequired[ IDX ] = false;
            //}

            State.Move( this );
        }

        #endregion
    }
}
