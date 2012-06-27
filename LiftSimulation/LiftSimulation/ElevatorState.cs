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
    }

    class FixedOpen : ElevatorState
    {
        public override void Loop( Elevator Elevator ) 
        {
            Syncronize.open_door();
            Syncronize.DoorTimerReset();
            Syncronize.PassengerButtonsEnable(true);

            Elevator.DeleteRequirementsHere();

            //Elevator.DeleteReqiredOppositeDirection();
            //Elevator.DeleteReqiredDirection();
            
            if (!Syncronize.syncPassengers()) 
                Syncronize.DoorTimerReset(); // Lichtschrake übertreten --> Neustart des Türtimers

            Elevator.loggin();

            if (Elevator.Passengers <= 0)
            {
                Syncronize.PassenderMinusButtonEnable(false);
            }
            else{
                Syncronize.PassenderMinusButtonEnable(true);
            }
                          
                       
            if (Elevator.CheckForOverload())
            {
                Elevator.SetState(Defaults.State.Overload);
                Syncronize.DoorTimerStop();
                // return ;
            }            
        } 
         
    }

    /// <summary>
    /// 
    /// </summary>
    class FixedClosed : ElevatorState
    {
        public override void Loop(Elevator Elevator)
        {
            Elevator.loggin(); 
            Syncronize.close_door();

            if (!Elevator.ReachedHighestOrLowestFloor) //Kein Randstockwerk --> nur Anhalten wenn Haltewunsch in Fahrtrichtung
            {
                if (Elevator.ThereAreWishesOnThisFloor)
                {
                    Elevator.SetState(Defaults.State.FixedOpen);
                    return ;
                }
                else if (!Elevator.DirectionWishesInMyDirection && Elevator.ThereAreOppositeWishesOnThisFloor) 
                {
                    Elevator.SetState(Defaults.State.FixedOpen);
                    return;
                }
            }
            else //Randstockwerk --> Wenn er dort hält, kann dies als Grund nur einen Wunsch haben
            {
                Elevator.SwitchDirection();

                if( Elevator.ThereAreOppositeWishesOnThisFloor || Elevator.ThereAreWishesOnThisFloor ) //wenn dir Tür geöffnet wurde, ist der Wunsch erloschen -> darf nicht nochmal öffnen
                {
                    Elevator.SetState(Defaults.State.FixedOpen);
                    return ;
                }
            }

            if (Elevator.DirectionWishesInMyDirection || Elevator.OppositeDirectionWishesInMyDirection)
            {
                Elevator.SetState(Defaults.State.Moving);
            }

            else if (Elevator.ThereAreOppositeWishesOnThisFloor)
            {

                Elevator.SwitchDirection();

                Elevator.SetState(Defaults.State.FixedOpen);
            }
            else if (Elevator.OppositeDirectionWishesInMyOppositeDirection || Elevator.DirectionWishesInMyOppositeDirection)
            {
                Elevator.SwitchDirection();
                Elevator.SetState(Defaults.State.Moving);
            }

            //else if (Elevator.ReachedHighestOrLowestFloor)
            //{
            //    Elevator.SwitchDirection();
            //    Elevator.SetState(Defaults.State.FixedOpen);

            //}

            else
            {
                Elevator.TaskStatus = false;
                //Elevator.SetState(Defaults.State.FixedOpen);
            }

        }
    }

    class Moving : ElevatorState 
    {
        public override void Loop(Elevator Elevator) 
        {
            
            if(Elevator.ThereAreWishesOnThisFloor || Elevator.ThereAreOppositeWishesOnThisFloor)
            {
                Elevator.SetState(Defaults.State.FixedClosed);
                return;
            }

            else if (Elevator.DirectionWishesInMyDirection || Elevator.OppositeDirectionWishesInMyDirection)
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
