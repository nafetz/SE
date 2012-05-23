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
            int previousPassengers = Elevator.Passengers;

            do
            {
                System.Threading.Thread.Sleep(5000);  // geht hier noch was im Elevator (v.A. Ein/Aussteigen)
            } while (previousPassengers != Elevator.Passengers);

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
            while (true)
            {
                if (
                    Elevator.UpwardRequired.Contains(true) ||
                    Elevator.DownwardRequired.Contains(true) ||
                    Elevator.InternRequired.Contains(true)
                    ) break;
            }
            Elevator.SetState(Defaults.State.Moving);
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
