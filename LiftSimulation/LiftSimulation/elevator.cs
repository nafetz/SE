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

        private bool _hasTask = false;
        #endregion


        #region Properties

        // Für den Zugriff außerhalb des Class-Scope
        // get

        public bool TaskStatus
        {
            get { return _hasTask; }
            set { _hasTask = value; }
        }
        
        public ElevatorState CurrentState
        {
            get { return State; }
            set { State = value; }
        }

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

        public bool ReachedHighestOrLowestFloor
        {
            get
            { 
                if( _currentFloor == ( 0 - Defaults.Basements ) )
                    return true;
                if( _currentFloor == Defaults.Floors - Defaults.Basements -1  )
                    return true;

                return false;
            }
        }

        public bool ThereAreWishesOnThisFloor
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

        public bool ThereAreOppositeWishesOnThisFloor
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
/*
        public bool ThereAreWishesInMyDirection
        {
            get
            {
                switch (_direction)
                {
                    case Defaults.Direction.Upward:
                        {
                            for ( int i = Defaults.FloorToIdx(_currentFloor); i < Defaults.Floors; i++ )
                            {
                                if ( _internRequired[i]  || _upwardRequired[i] ) 
                                    return true;
                            }
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i >= 0; i--)
                            {
                                if( _internRequired[ i ] || _downwardRequired[ i ] )
                                    return true;
                            }
                        } break;
                }
                return false;
            }
        }
*/

        /*
        public bool ThereAreWishesInTheDirectionWhichIsNotMyDirection
        {
            get
            {
                switch (_direction)
                {
                    case Defaults.Direction.Upward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i >= 0; i--)
                            {
                                if (_internRequired[i] || _downwardRequired[i])
                                    return true;
                            }
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i < Defaults.Floors; i++)
                            {
                                if (_internRequired[i] || _upwardRequired[i] )
                                    return true;
                            }
                        } break;
                }
                return false;
            }
        }
         * */

        /// <summary>
        /// Wünsche in folgenen Etagen entlang der Fahrtrichtung
        /// Bsp: Hochfahren und weiter oben der Wunsch nach oben zu fahren
        /// </summary>
        public bool DirectionWishesInMyDirection
        {
            get
            {
                switch (_direction)
                {
                    case Defaults.Direction.Upward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i < Defaults.Floors; i++)
                            {
                                if (_internRequired[i] || _upwardRequired[i])
                                    return true;
                            }
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i >= 0; i--)
                            {
                                if (_internRequired[i] || _downwardRequired[i])
                                    return true;
                            }
                        } break;
                }
                return false;
            }
        }

        /// <summary>
        /// Wünsche  in folgenden Etagen entgegen der Fahrtrichtung
        ///  Bsp: Hochfahren und weiter oben der Wunsch nach unten zu fahren
        /// </summary>
        public bool OppositeDirectionWishesInMyDirection
        {
            get
            {
                switch (_direction)
                {
                    case Defaults.Direction.Upward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i < Defaults.Floors; i++)
                            {
                                if (_downwardRequired[i])
                                    return true;
                            }
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i >= 0; i--)
                            {
                                if (_upwardRequired[i])
                                    return true;
                            }
                        } break;
                }
                return false;
            }
        }

        /// <summary>
        /// Wünsche in vorherigen Etagen entgegen der Fahrtrichtung
        /// Bsp: Hochfahren und in unterer Etage Wunsch nach unten zu fahren
        /// </summary>

        public bool OppositeDirectionWishesInMyOppositeDirection
        {
            get
            {
                switch (_direction)
                {
                    case Defaults.Direction.Upward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i >= 0; i--)
                            {
                                if (_internRequired[i] || _downwardRequired[i])
                                    return true;
                            }
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i < Defaults.Floors; i++)
                            {
                                if (_internRequired[i] || _upwardRequired[i])
                                    return true;
                            }
                        } break;
                }
                return false;
            }
        }
        /// <summary>
        /// Wünsche in vorherigen Etagen entlang der Fahrtrichtung
        /// Bsp: Hochfahren und in unterer Etage Wunsch nach oben zu fahren
        /// </summary>

        public bool DirectionWishesInMyOppositeDirection
        {
            get
            {
                switch (_direction)
                {
                    case Defaults.Direction.Upward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i >= 0; i--)
                            {
                                if (_upwardRequired[i])
                                    return true;
                            }
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            for (int i = Defaults.FloorToIdx(_currentFloor); i < Defaults.Floors; i++)
                            {
                                if (_downwardRequired[i])
                                    return true;
                            }
                        } break;
                }
                return false;
            }
        }

        public bool AnyWishes
        {
            get
            {
                if (DirectionWishesInMyDirection || DirectionWishesInMyOppositeDirection || OppositeDirectionWishesInMyDirection || OppositeDirectionWishesInMyOppositeDirection)
                    return true;
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


        #region Methods

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Elevator()
        {              
            _currentFloor = 0;           
                     
            for (int IDX = (Defaults.Floors - 1); IDX >= 0; IDX--)
            {
                _downwardRequired.Add(false);
                _upwardRequired.Add(false);
                _internRequired.Add(false);
            }
            
            State = FixedClosed;
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
                case Defaults.State.FixedOpen  : State = FixedOpen;    break;
                case Defaults.State.FixedClosed : State = FixedClosed;  break;
                case Defaults.State.Overload    : State = Overload;     break;
            }
            this.CurrentState.Loop(this);
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

        /// <summary>
        /// ändert Fahrtrichtung ins entgegengesetzte
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
        }

        public void DeleteReqiredDirection()
        {
            int i = Defaults.FloorToIdx( _currentFloor );

            switch (_direction)
            {
                case Defaults.Direction.Upward:
                    {
                        _upwardRequired[ i ] = false;
                        Syncronize.syncUpwardWishes(Syncronize.To.UI);
                        break;
                    }
                case Defaults.Direction.Downward:
                    {
                        _downwardRequired[ i ] = false;
                        Syncronize.syncDownwardWishes( Syncronize.To.UI );
                        break;
                    }
            }
            _internRequired[ i ] = false;
            Syncronize.syncinnerWishes( Syncronize.To.UI );            
        }

        public void DeleteReqiredOppositeDirection()
        {
            int i = Defaults.FloorToIdx(_currentFloor);

            switch (_direction)
            {
                case Defaults.Direction.Downward:
                    {
                        _upwardRequired[i] = false;
                         Syncronize.syncDownwardWishes(Syncronize.To.UI);
                        break;
                    }
                case Defaults.Direction.Upward:
                    {
                        _downwardRequired[i] = false;
                         Syncronize.syncUpwardWishes( Syncronize.To.UI );
                        break;
                    }
            }
            _internRequired[i] = false;
           Syncronize.syncinnerWishes( Syncronize.To.UI );            
        }

        public void loggin()
        {
            Defaults._logentry entry;
           if (CurrentState  == Moving)            {
                   entry = new Defaults._logentry(_direction, CurrentFloor, Passengers, Defaults.State.Moving);
            }
           else if (CurrentState == FixedClosed)
           {
                entry = new Defaults._logentry(_direction, CurrentFloor, Passengers, Defaults.State.FixedClosed);
           }

           else if (CurrentState == FixedOpen)
           {
                entry = new Defaults._logentry(_direction, CurrentFloor, Passengers, Defaults.State.FixedOpen);
           }
           else   entry = new Defaults._logentry(_direction, CurrentFloor, Passengers, Defaults.State.Overload);

           Syncronize._logging(entry);
        }

        #endregion
    }
}
