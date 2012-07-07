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
        private ElevatorState _state;
        private ElevatorState MOVING = new Moving();                // Quasi - Konstante, da memberlos
        private ElevatorState FIXED_OPEN = new FixedOpen();         // s.o.
        private ElevatorState FIXED_CLOSED = new FixedClosed();     // s.o.
        private ElevatorState OVERLOAD = new Overload();            // s.o.

        // Geschossumgebung
        private int _currentFloor;
        List<bool> _upwardRequired = new List<bool>();
        List<bool> _downwardRequired = new List<bool>();
        List<bool> _internRequired = new List<bool>();

        // Was man zu Fahren braucht
        private Defaults.Direction _direction = Defaults.Direction.Upward;
        private int _passengers = 0;

        private bool _hasTask = false;
        #endregion


        #region Konstruktoren

        public Elevator()
        {              
            _currentFloor = 0;           
                     
            for (int IDX = (Defaults.Floors - 1); IDX >= 0; IDX--)
            {
                _downwardRequired.Add(false);
                _upwardRequired.Add(false);
                _internRequired.Add(false);
            }
            
            _state = FIXED_CLOSED;
        }

        #endregion


        #region Properties

        public bool TaskStatus
        {
            get { return _hasTask; }
            set { _hasTask = value; }
        }
        
        public ElevatorState CurrentState
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// Nummer der gegenwärtigen Etage.
        /// Achtung!!! Nicht nullbasiert
        /// Erdgeschoss         => 0;
        /// 1. Obergeschoss     => 1;
        /// 2. Untergeschoss    => -2
        /// </summary>       
        public int CurrentFloor 
        {
            get { return _currentFloor; }
            set { _currentFloor = value; }
        }

        // Wunschlisten
        public List<bool> UpwardRequired 
        {
            get { return _upwardRequired; }
            set { _upwardRequired = value; }
        }
        public List<bool> DownwardRequired 
        {
            get { return _downwardRequired; }
            set { _downwardRequired = value; }
        }
        public List<bool> InternRequired
        {
            get { return _internRequired; }
            set { _internRequired = value; }
        }

        /// <summary>
        /// Gibt an, ob der Fahrstuhl das Ende des Fahrstuhlschachtes in Fahrtrichtung erreicht hat.
        /// </summary>
        public bool ReachedEndOfShaft
        {
            get
            { 
                switch (_direction)
                {
                    case Defaults.Direction.Downward:
                        {
                            if( _currentFloor == ( 0 - Defaults.Basements ) )
                                return true;
                        }break;
                    case Defaults.Direction.Upward:
                        {
                            if( _currentFloor == Defaults.Floors - Defaults.Basements - 1 )
                                return true;
                        } break;
                }
                return false;
            }
        }

        /// <summary>
        /// Gibt an, ob auf der gegenwärtigen Etage ein interner Wunsch oder ein Wunsch in Fahrtrichtung vorliegt.
        /// Liegt lediglich ein Wunsch entgegen der Fahrtrichtung vor, lautet das Ergebnis false!
        /// </summary>
        public bool FittingWishOnThisFloor
        {
            get
            {
               if (_internRequired[Defaults.FloorToIdx(_currentFloor)])
                {
                    return true;
                }

                switch( _direction ) 
                {
                    case Defaults.Direction.Upward:
                        {
                            if( _upwardRequired[ Defaults.FloorToIdx( _currentFloor ) ] )
                                return true;
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            if( _downwardRequired[ Defaults.FloorToIdx( _currentFloor ) ] )
                                return true;
                        } break;
                }
                return false;
            }
        }

        /// <summary>
        /// Gibt an, ob auf der gegenwärtigen Etage ein Wunsch entgegen der Fahrtrichtung vorliegt.
        /// Liegt kein Wunsch entgegen der Fahrtrichtung vor, lautet das Ergebnis false!
        /// </summary>
        public bool OppositeWishOnThisFloor
        {
            get
            {
                switch (_direction)
                {
                    case Defaults.Direction.Upward:
                        {
                            if (_downwardRequired[Defaults.FloorToIdx(_currentFloor)])
                                return true;
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            if (_upwardRequired[Defaults.FloorToIdx(_currentFloor)])
                                return true;
                        } break;
                }

                return false;
            }
        }

        /// <summary>
        /// Gibt an, ob Wünsche in den folgenen Etagen entlang der Fahrtrichtung vorliegen
        /// Bsp: Hochfahren und weiter oben der Wunsch nach oben zu fahren
        /// </summary>
        public bool WishesInMyDirection
        {
            get
            {
                switch (_direction)
                {
                    case Defaults.Direction.Upward:
                        {
                            if( !ReachedEndOfShaft )
                            {
                                for( int i = Defaults.FloorToIdx( _currentFloor ) + 1; i < Defaults.Floors; i++ )
                                {
                                    if( _internRequired[ i ] || _upwardRequired[ i ] || _downwardRequired[ i ] )
                                        return true;
                                }
                            }
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            if( !ReachedEndOfShaft )
                            {
                                for( int i = Defaults.FloorToIdx( _currentFloor ) -1; i >= 0; i-- )
                                {
                                    if( _internRequired[ i ] || _downwardRequired[ i ] || _upwardRequired[ i ] )
                                        return true;
                                }
                            }
                        } break;
                }
                return false;
            }
        }

        /// <summary>
        /// Gibt an, ob Wünsche in den Geschossen vorliegen, die nicht in Fahrtrichtung liegen.
        /// </summary>
        public bool WishesInMyOppositeDirection
        {
            get
            {
                switch (_direction)
                {
                    case Defaults.Direction.Upward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i >= 0; i--)
                            {
                                if( _internRequired[ i ] || _downwardRequired[ i ] || _upwardRequired[ i ] )
                                    return true;
                            }
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i < Defaults.Floors; i++)
                            {
                                if( _internRequired[ i ] || _upwardRequired[ i ] || _downwardRequired[ i ] )
                                    return true;
                            }
                        } break;
                }
                return false;
            }
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

        #endregion     


        #region Methoden

        /// <summary>
        /// Löst Zustandsübergang des ElevatorObjektes aus
        /// </summary>
        /// <param name="newState">Zielzustand aus Defaults.State</param>
        public void SetState( Defaults.State newState) 
        {
            switch ( newState )
            {
                case Defaults.State.Moving      : _state = MOVING;       break;
                case Defaults.State.FixedOpen   : _state = FIXED_OPEN;    break;
                case Defaults.State.FixedClosed : _state = FIXED_CLOSED;  break;
                case Defaults.State.Overload    : _state = OVERLOAD;     break;
            }

            Log.AddEntry("Neuer Zustand: " + newState.ToString());
            this._state.Loop(this);
        }

        /// <summary>
        /// Überprüft, ob die Anzahl der Passagiere über dem zulässigen Maximum liegt
        /// </summary>
        /// <returns>true, wenn Fahrstuhl überladen, sonst false</returns>
        public bool CheckForOverload()
        {
            if ( _passengers > Defaults.MaximumPassengers )
                return true;

            return false;
        }

        /// <summary>
        /// Ändert die Fahrtrichtung.
        /// </summary>
        public void SwitchDirection()
        {
            switch( _direction )
            {
                case Defaults.Direction.Downward:
                    {
                        if ( _currentFloor != ( Defaults.Floors - Defaults.Basements - 1))
                            _direction = Defaults.Direction.Upward; 
                    } break;
                case Defaults.Direction.Upward:
                    {
                        if (_currentFloor != (0 - Defaults.Basements)) 
                            _direction = Defaults.Direction.Downward; 
                    } break;
            }

            Syncronize.SwitchDirection();
            Log.AddEntry("Richtungswechsel, neue Richtung: " + _direction.ToString());
        }

        /// <summary>
        /// Löscht die Wünsche auf der aktuellen Etage
        /// </summary>
        public void DeleteRequirementsHere()
        {
            int i = Defaults.FloorToIdx( _currentFloor );

            _upwardRequired[ i ] = false;
            _downwardRequired[ i ] = false;
            _internRequired[ i ] = false;

            Syncronize.SyncUpwardWishes( Syncronize.To.UI );
            Syncronize.SyncDownwardWishes( Syncronize.To.UI );
            Syncronize.SyncInnerWishes( Syncronize.To.UI );
        }
      

        #endregion        
    }
}
