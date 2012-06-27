#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace LiftSimulation
{
    #region Zustandsklassen

    class ElevatorState
    {
        public virtual void Loop(Elevator Elevator) { }
    }

    #region Konkrete Zustände

    class FixedOpen : ElevatorState
    {
        public override void Loop( Elevator elevator ) 
        {
            Syncronize.OpenDoor();
            Syncronize.ResetDoorTimer();
            Syncronize.EnablePassengerButtons(true);

            elevator.DeleteRequirementsHere();

            //Elevator.DeleteReqiredOppositeDirection();
            //Elevator.DeleteReqiredDirection();
            
            if (!Syncronize.SyncPassengers()) 
                Syncronize.ResetDoorTimer(); // Lichtschrake übertreten --> Neustart des Türtimers

            elevator.loggin(); //keep rollin' rollin' rollin rollin' !!!!!!!! ;)

            if (elevator.Passengers <= 0)
                Syncronize.EnablePassengerMinusButton(false);
            else
                Syncronize.EnablePassengerMinusButton(true);                          
                       
            if (elevator.CheckForOverload())
            {
                elevator.SetState(Defaults.State.Overload);
                Syncronize.StopDoorTimer();
                // return ;
            }            
        }          
    }

    class FixedClosed : ElevatorState
    {
        public override void Loop(Elevator elevator)
        {
            elevator.loggin(); 
            Syncronize.CloseDoor();

            if (!elevator.ReachedHighestOrLowestFloor) //Kein Randstockwerk --> nur Anhalten wenn Haltewunsch in Fahrtrichtung
            {
                if (elevator.ThereAreWishesOnThisFloor)
                {
                    elevator.SetState(Defaults.State.FixedOpen);
                    return ;
                }
                else if (!elevator.DirectionWishesInMyDirection && elevator.ThereAreOppositeWishesOnThisFloor) 
                {
                    elevator.SetState(Defaults.State.FixedOpen);
                    return;
                }
            }
            else //Randstockwerk --> Wenn er dort hält, kann dies als Grund nur einen Wunsch haben
            {
                elevator.SwitchDirection();

                if( elevator.ThereAreOppositeWishesOnThisFloor || elevator.ThereAreWishesOnThisFloor ) //wenn dir Tür geöffnet wurde, ist der Wunsch erloschen -> darf nicht nochmal öffnen
                {
                    elevator.SetState(Defaults.State.FixedOpen);
                    return ;
                }
            }

            if (elevator.DirectionWishesInMyDirection || elevator.OppositeDirectionWishesInMyDirection)
                elevator.SetState(Defaults.State.Moving);

            else if (elevator.ThereAreOppositeWishesOnThisFloor)
            {
                elevator.SwitchDirection();
                elevator.SetState(Defaults.State.FixedOpen);
            }
            else if (elevator.OppositeDirectionWishesInMyOppositeDirection || elevator.DirectionWishesInMyOppositeDirection)
            {
                elevator.SwitchDirection();
                elevator.SetState(Defaults.State.Moving);
            }

            //else if (Elevator.ReachedHighestOrLowestFloor)
            //{
            //    Elevator.SwitchDirection();
            //    Elevator.SetState(Defaults.State.FixedOpen);

            //}

            else
            {
                elevator.TaskStatus = false;
                //Elevator.SetState(Defaults.State.FixedOpen);
            }
        }
    }

    class Moving : ElevatorState 
    {
        public override void Loop(Elevator elevator) 
        {            
            if(elevator.ThereAreWishesOnThisFloor || elevator.ThereAreOppositeWishesOnThisFloor)
            {
                elevator.SetState(Defaults.State.FixedClosed);
                return;
            }

            else if (elevator.DirectionWishesInMyDirection || elevator.OppositeDirectionWishesInMyDirection)
            {
                switch (elevator.Direction)
                {
                    case Defaults.Direction.Upward:
                        {
                            elevator.CurrentFloor++;
                            Syncronize.SyncCurrentFloor();
                            //Elevator.DeleteReqired();
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            elevator.CurrentFloor--;
                            Syncronize.SyncCurrentFloor();
                            //Elevator.DeleteReqired();
                        } break;
                }// switch

                elevator.loggin();
                Syncronize.ResetMoveTimer();
                Syncronize.ShowDirection();
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
            Syncronize.SyncPassengers();
            if ( !Elevator.CheckForOverload() )
                Elevator.SetState( Defaults.State.FixedOpen );
        }
    }

    #endregion

    #endregion
}
