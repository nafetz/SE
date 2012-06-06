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
    interface ElevatorState
    {
        void Loop(Elevator Elevator);
    }

    class Fixed : ElevatorState
    {
        public void Loop( Elevator Elevator ) 
        {
           // Syncronize.PassengerButtonsEnable(true);

            bool breakOut = false;

            do
            {
                //Syncronize.TimerReset();
                breakOut = Syncronize.syncPassengers();

            } while (!breakOut);

            if( Elevator.CheckForOverload() )
                Elevator.SetState( Defaults.State.Overload );

            //Syncronize.PassengerButtonsEnable(false);

            if (Elevator.ReachedHighestOrLowestFloor || !Elevator.ThereAreWishesInMyDirection)
                Elevator.SwitchDirection();

            if (Elevator.ThereAreWishesInMyDirection)
                Elevator.SetState(Defaults.State.Moving);

            //else
            //    Elevator.SetState( Defaults.State.FixedClosed );
        }
    }

    /// <summary>
    /// Eigentlich PseudoZustand, der nur Übergänge einleitet
    /// </summary>
    //class FixedClosed : ElevatorState
    //{
    //    public void Move( Elevator Elevator ) 
    //    {
    //        if( Elevator.ThereAreWishesOnThisFloor )
    //            Elevator.SetState( Defaults.State.FixedOpen );
    //        else
    //            Elevator.SetState( Defaults.State.Moving );
    //    }
    //}

    class Moving : ElevatorState 
    {
        public void Loop( Elevator Elevator ) 
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

                else if( Elevator.ThereAreWishesInMyDirection )
                {
                    switch( Elevator.Direction )
                    {
                        case Defaults.Direction.Upward:
                            {
                                Elevator.CurrentFloor++;
                                Syncronize.SyncCurrentFloor();
                                Elevator.delete_requireds();
                            } break;
                        case Defaults.Direction.Downward:
                            {
                                Elevator.CurrentFloor--;
                                Syncronize.SyncCurrentFloor();
                                Elevator.delete_requireds();
                                
                            } break;
                    }// switch

                }// if
            }// while

            Elevator.SetState( Defaults.State.Fixed );
        }
    }

    class Overload : ElevatorState 
    {
        public void Loop( Elevator Elevator ) 
        {
            bool breakOut = false;

            do
            {
                // 3 sec warten auf Button
                /*
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
                */

                

                if( !Elevator.CheckForOverload() )
                    breakOut = true;

            } while( !breakOut );

            Elevator.SetState(Defaults.State.Fixed);
        }
    }
}
