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
        public virtual void Loop(Elevator Elevator) { }
        public virtual void Finish(Elevator Elevator) { }
    }

    class FixedOpen : ElevatorState
    {
        public override void Loop( Elevator Elevator ) 
        {
            Elevator.DeleteReqired();
            
           
                       
            Syncronize.open_door();
            Syncronize.DoorTimerReset();
            Syncronize.PassengerButtonsEnable(true);           
            
            if (!Syncronize.syncPassengers()) 
                Syncronize.DoorTimerReset(); // Lichtschrake übertreten --> Neustart des Türtimers

            Elevator.loggin();

            if (Elevator.Passengers <= 0)
            {
                Syncronize.PassenderMinusButtonEnable(false);
                Syncronize.enableInnerButton(false);
            }
            else
            {
                Syncronize.PassenderMinusButtonEnable(true);
                Syncronize.enableInnerButton(true);
            }

            if (Elevator.CheckForOverload())
            {
                Elevator.SetState(Defaults.State.Overload);
                Syncronize.DoorTimerStop();
                // return ;
            }            
        } 

        public override void Finish(Elevator Elevator)
        {
            Syncronize.PassengerButtonsEnable( false );
            Syncronize.SetState( Defaults.State.FixedClosed );
            //Elevator.CurrentState.Loop( Elevator );
        }   
    }

    /// <summary>
    /// Eigentlich PseudoZustand, der nur Übergänge einleitet
    /// </summary>
    class FixedClosed : ElevatorState
    {
        public override void Loop(Elevator Elevator)
        {
            Elevator.loggin();
            Syncronize.close_door();
            
            if (Elevator.ThereAreWishesOnThisFloor)
            {
                Syncronize.PassengerButtonsEnable(true);
                Elevator.SetState(Defaults.State.FixedOpen);
            }
            else if (Elevator.ThereAreWishesInMyDirection)
            {
                Elevator.SetState(Defaults.State.Moving);
            }
            else if (Elevator.ThereAreWishesInTheDirectionWhichIsNotMyDirection || Elevator.ReachedHighestOrLowestFloor)
            {
                Elevator.SwitchDirection();
                Elevator.SetState(Defaults.State.Moving);
            }

            else Elevator.TaskStatus = false;
            //else if (Wünsche in der Gegenrichtung ||)
            //  richtungswechesel & go!
        }
    }

    class Moving : ElevatorState 
    {
        public override void Loop(Elevator Elevator) 
        {
            
            if(Elevator.ThereAreWishesOnThisFloor)
            {
                Elevator.SetState(Defaults.State.FixedClosed);
                return;
            }

            else if (Elevator.ThereAreWishesInMyDirection)
            {
                switch (Elevator.Direction)
                {
                    case Defaults.Direction.Upward:
                        {
                            Elevator.CurrentFloor++;
                            Syncronize.SyncCurrentFloor();
                            //Elevator.DeleteReqired();
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            Elevator.CurrentFloor--;
                            Syncronize.SyncCurrentFloor();
                            //Elevator.DeleteReqired();
                        } break;
                }// switch
                Elevator.loggin();
               Syncronize.MoveTimerReset();
              Syncronize.visibleDirection();
               // Elevator.DeleteReqired();
                return;
            }
           
        }
    }

    class Overload : ElevatorState 
    {
        public override void Loop(Elevator Elevator) 
        {
            Elevator.loggin();
            Syncronize.syncPassengers();
            if (!Elevator.CheckForOverload())
            {
                Elevator.SetState(Defaults.State.FixedOpen);
            }
        }
    }
}
