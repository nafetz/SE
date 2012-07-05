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

            if (!elevator.ReachedEndOfShaft) //Kein Randstockwerk --> nur Anhalten wenn Haltewunsch in Fahrtrichtung
            {
                if (elevator.TheresAFittingWishOnThisFloor)
                {
                    elevator.SetState(Defaults.State.FixedOpen);
                    return ;
                }
                else if (!elevator.WishesInMyDirection && elevator.TheresAOppositeWisheOnThisFloor) 
                {
                    elevator.SetState(Defaults.State.FixedOpen);
                    return;
                }
            }
            else //Randstockwerk --> Wenn er dort hält, kann dies als Grund nur einen Wunsch haben
            {
                elevator.SwitchDirection();

                // wenn man pokern will, lässt man die abfrage weg, da er nur dann ganz hoch oder runter fährt, wenn es dort wünsche gibt.
                if( elevator.TheresAOppositeWisheOnThisFloor || elevator.TheresAFittingWishOnThisFloor ) //wenn die Tür geöffnet wurde, ist der Wunsch erloschen -> darf nicht nochmal öffnen
                {
                    elevator.SetState(Defaults.State.FixedOpen);
                    return ;
                }
            }

            if( elevator.WishesInMyDirection )
            {
                elevator.SetState( Defaults.State.Moving );
                return; //3.7.2012
            }

            else if( elevator.TheresAOppositeWisheOnThisFloor )
            {
                elevator.SwitchDirection();
                elevator.SetState( Defaults.State.FixedOpen );
                return; //3.7.2012
            }
            else if( elevator.WishesInMyOppositeDirection )
            {
                elevator.SwitchDirection();
                elevator.SetState( Defaults.State.Moving );
                return; //3.7.2012
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
            if( elevator.ReachedEndOfShaft )
            {
                elevator.SwitchDirection();
            }

            if(elevator.TheresAFittingWishOnThisFloor /*|| elevator.ThereAreOppositeWishesOnThisFloor*/)
            {
                elevator.SetState(Defaults.State.FixedClosed);
                return;
            }

            else if( elevator.WishesInMyDirection )
            {
                switch( elevator.Direction )
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

            else if( elevator.TheresAOppositeWisheOnThisFloor )
            {
                elevator.SwitchDirection();
                elevator.SetState( Defaults.State.FixedClosed );
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
