#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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
        private ElevatorState Overload = new Overload();

        // Floor-Stuff
        private int _currentFloor;
        List<bool> _upwardRequired = new List<bool>();
        List<bool> _downwardRequired = new List<bool>();
        List<bool> _internRequired = new List<bool>();

        // Riding
        private Defaults.Direction _direction = Defaults.Direction.Upward;
        private int _passengers = 0;

        //User Interface
        private UserInterface _ui;

        #endregion


        #region Properties

        // Für den Zugriff außerhalb des Class-Scope
        // get
        /// <summary>
        /// Nummer der gegenwärtigen Etage.
        /// Achtung!!! Nicht nullbasiert :
        /// EG      => 0;
        /// 1. OG   => 1;
        /// 2. UG   => -2
        /// </summary>
        public int CurrentFloor 
        {
            get { return _currentFloor; }
            set { _currentFloor = value; }
        }

        public List<bool> UpwardRequired 
        {
            get { return _upwardRequired; }
        }
        public List<bool> DownwardRequired 
        {
            get { return _downwardRequired; }
        }
        public List<bool> InternRequired
        {
            get { return _internRequired; }
        }

        public Defaults.Direction Direction
        {
            get { return _direction; }
            set { this._direction = value; }
        }

        public int Passengers
        {
            get { return _passengers; }
            set { _passengers = value; }
        }

        public UserInterface UI
        {
            get { return _ui; }
            set { _ui = value; }
        }
        

        #endregion


        #region Methods

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Elevator(UserInterface myUI)
        {
            this._ui = myUI;
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
            for (int IDX = (Defaults.Floors - 1); IDX >= 0; IDX--)
            {
                _downwardRequired.Add(false);
                _upwardRequired.Add(false);
            }

            State.Move( this );
        }

        public void AddPassengers( int Count ) 
        { 
            _passengers += Count;
            if ( _passengers > Defaults.MaximumPassengers )
            {
                State = Overload;
            }
        }

        public void RemovePassengers( int Count ) 
        {
            if (_passengers - Count < 0)
                _passengers = 0;
            else 
                _passengers -= Count; 
        }
        
        /// <summary>
        /// Löst Zustandsübergang des ElevatorObjektes aus
        /// </summary>
        /// <param name="newState">Zielzustand aus Defaults.State</param>
        public void SetState( Defaults.State newState) 
        {
            switch ( newState )
            {
                case Defaults.State.Moving      : State = Moving;       break;
                case Defaults.State.FixedOpen   : State = FixedOpen;    break;
                case Defaults.State.FixedClosed : State = FixedClosed;  break;
                case Defaults.State.Overload    : State = Overload;     break;
            }          
        }

        /// <summary>
        /// überprüft, ob die Anzahl der Passagiere über dem zulässigen Maximum liegt
        /// </summary>
        /// <returns>true, wenn Fahrstuhl überladen, sonst false</returns>
        public bool CheckForOverload()
        {
            if ( _passengers > Defaults.MaximumPassengers )
                return true;

            return false;
        }

        #endregion
    }
}
