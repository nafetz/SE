#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace LiftSimulation
{
    /// <summary>
    /// State-Class, nur Methoden, keine Member!
    /// Konkrete States bitte in Defaults.State ergänzen
    /// </summary>
    class ElevatorState
    {
        public void Move(Elevator Elevator) { }
    }

    class FixedOpen : ElevatorState
    {
        public new void Move( Elevator Elevator ) 
        {
            bool breakOut = false;

            do
            {
                // 3 sec warten auf Button5
                Defaults.ManualResetEvent.WaitOne( 3000 );

                switch( Elevator.UI.PassengersIO )
                {
                    case Defaults.MoreOrLess.More:
                        Elevator.Passengers++;
                        Elevator.UI.PassengersCount = Elevator.Passengers;
                        break;
                    case Defaults.MoreOrLess.Less:
                        Elevator.Passengers--;
                        Elevator.UI.PassengersCount = Elevator.Passengers;
                        break;
                    case Defaults.MoreOrLess.Neither:
                        breakOut = true; break;
                }

                Elevator.UI.ResetPassengerIO();     // wieder Neither

            } while (!breakOut);

            if( Elevator.CheckForOverload() )
                Elevator.SetState( Defaults.State.Overload );
            
            if( Elevator.ReachedHighestOrLowestFloor || !Elevator.ThereAreWishesInMyDirection )
                Elevator.SwitchDirection();

            else
                Elevator.SetState( Defaults.State.FixedClosed );
        }
    }

    /// <summary>
    /// Eigentlich PseudoZustand, der nur Übergänge einleitet
    /// </summary>
    class FixedClosed : ElevatorState
    {
        public new void Move( Elevator Elevator ) 
        {
            if( Elevator.ThereAreWishesOnThisFloor )
                Elevator.SetState( Defaults.State.FixedOpen );
            else
                Elevator.SetState( Defaults.State.Moving );
        }
    }

    class Moving : ElevatorState 
    {
        public new void Move( Elevator Elevator ) 
        {
            bool breakout = false;

            while( !breakout )
            {
                if( Elevator.ReachedHighestOrLowestFloor )
                {
                    Elevator.SwitchDirection();
                }

                if( Elevator.ThereAreWishesOnThisFloor )
                {
                    breakout = true;
                }

                if( Elevator.ThereAreWishesInMyDirection )
                {
                    switch( Elevator.Direction )
                    {
                        case Defaults.Direction.Upward:
                            {
                                Elevator.CurrentFloor++;
                            } break;
                        case Defaults.Direction.Downward:
                            {
                                Elevator.CurrentFloor--;
                            } break;
                    }// switch
                }// if
            }// while

            Elevator.SetState( Defaults.State.FixedClosed );
        }
    }

    class Overload : ElevatorState 
    {
        public new void Move( Elevator Elevator ) 
        {
            bool breakOut = false;

            do
            {
                // 3 sec warten auf Button
                Defaults.ManualResetEvent.WaitOne( 3000 );

                switch( Elevator.UI.PassengersIO )
                {
                    case Defaults.MoreOrLess.More:
                        Elevator.Passengers++;
                        Elevator.UI.PassengersCount = Elevator.Passengers;
                        break;
                    case Defaults.MoreOrLess.Less:
                        Elevator.Passengers--;
                        Elevator.UI.PassengersCount = Elevator.Passengers;
                        break;
                    case Defaults.MoreOrLess.Neither:
                        breakOut = true; break;
                }

                Elevator.UI.ResetPassengerIO();     // wieder Neither

                if( !Elevator.CheckForOverload() )
                    breakOut = true;

            } while( !breakOut );

            Elevator.SetState(Defaults.State.FixedClosed);
        }
    }
}
