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
                        Elevator.Passengers++; break;
                    case Defaults.MoreOrLess.Less:
                        Elevator.Passengers++; break;
                    case Defaults.MoreOrLess.Neither:
                        breakOut = true; break;
                }

                Elevator.UI.ResetPassengerIO();

            } while (!breakOut);

            if( Elevator.CheckForOverload() )
                Elevator.SetState( Defaults.State.Overload );
            else
                Elevator.SetState( Defaults.State.FixedOpen );
        }
    }

    class FixedClosed : ElevatorState
    {
        public new void Move( Elevator Elevator ) 
        {
            bool breakOut = false;

            do
            {
                Defaults.ManualResetEvent.WaitOne( 3000 );


            } while( !breakOut );
        }
    }

    class Moving : ElevatorState 
    {
        public new void Move( Elevator Elevator ) 
        {
            if ( Elevator.Direction == Defaults.Direction.Upward )
            {
                int highestWishIdx = Elevator.UpwardRequired.FindLastIndex ( delegate ( bool wish ) { return wish == true; } );
                if ( highestWishIdx > Defaults.FloorToIdx (Elevator.CurrentFloor) )
                {
                    Elevator.CurrentFloor++;
                    if ( Elevator.UpwardRequired[ Defaults.FloorToIdx( Elevator.CurrentFloor ) ] )
                    {
                        Elevator.SetState ( Defaults.State.FixedOpen );
                    }
                }
                else
                {
                    Elevator.Direction = Defaults.Direction.Downward;
                }

            }
            else
            { }
        }
    }

    class Overload : ElevatorState 
    {
        public new void Move( Elevator Elevator ) 
        {
            while (true)
            {
                if (!Elevator.CheckForOverload()) ; break;
            }
            Elevator.SetState(Defaults.State.FixedClosed);
        }
    }
}
