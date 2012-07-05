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
        private ElevatorState MOVING = new Moving();                // Quasi - Konstante, da Memberlos
        private ElevatorState FIXED_OPEN = new FixedOpen();         // s.o.
        private ElevatorState FIXED_CLOSED = new FixedClosed();     // s.o.
        private ElevatorState OVERLOAD = new Overload();            // s.o.

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
        public bool TheresAFittingWishOnThisFloor
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
        /// Gibt an, ob auf der gegenwärtigen Etage ein interner Wunsch oder ein Wunsch in Fahrtrichtung vorliegt.
        /// Liegt lediglich ein Wunsch entgegen der Fahrtrichtung vor, lautet das Ergebnis false!
        /// </summary>
        public bool TheresAOppositeWisheOnThisFloor
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

        /* Archiviert: ThereAreWishesInMyDirection
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

        /* Archiviert: ThereAreWishesInTheDirectionWhichIsNotMyDirection
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

        /*Archiviert: OppositeDirectionWishesInMyDirection
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
        }*/

        /// <summary>
        /// Gibt an, ob wünsche in den Geschossen vorliegen, die nicht in Fahrtrichtung liegen.
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


        /* Archiviert: DirectionWishesInMyOppositeDirection
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
        }*/

        /* Archiviert: AnyWishes (verweisfrei)
        public bool AnyWishes
        {
            get
            {
                if (DirectionWishesInMyDirection || DirectionWishesInMyOppositeDirection || OppositeDirectionWishesInMyDirection || OppositeDirectionWishesInMyOppositeDirection)
                    return true;
                return false;

            }
        }      */

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
                case Defaults.State.FixedOpen  : _state = FIXED_OPEN;    break;
                case Defaults.State.FixedClosed : _state = FIXED_CLOSED;  break;
                case Defaults.State.Overload    : _state = OVERLOAD;     break;
            }
            this._state.Loop(this);
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

        /* Archiviert: DeleteReqiredDirection()
         * public void DeleteReqiredDirection()
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
        }*/

        /* Archiviert: DeleteReqiredOppositeDirection()
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
        }*/
        
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

        public void loggin()
        {
            Defaults.Logentry entry;
            if( CurrentState == MOVING )
            {
                entry = new Defaults.Logentry( _direction, _currentFloor, _passengers, Defaults.State.Moving );
            }
            else if( CurrentState == FIXED_CLOSED )
            {
                entry = new Defaults.Logentry( _direction, _currentFloor, _passengers, Defaults.State.FixedClosed );
            }

            else if( CurrentState == FIXED_OPEN )
            {
                entry = new Defaults.Logentry( _direction, _currentFloor, _passengers, Defaults.State.FixedOpen );
            }
            else entry = new Defaults.Logentry( _direction, _currentFloor, _passengers, Defaults.State.Overload );

            Syncronize._logging( entry );
        }         

        #endregion        
    }
}
