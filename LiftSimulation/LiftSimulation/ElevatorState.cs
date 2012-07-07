﻿#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace LiftSimulation
{
    #region Zustandsklassen

    interface ElevatorState
    {
        void Loop( Elevator Elevator );
    }

    #region Konkrete Zustände

    class FixedOpen : ElevatorState
    {
        public void Loop( Elevator elevator ) 
        {
            Syncronize.OpenDoor();
            Syncronize.ResetDoorTimer();
            Syncronize.EnablePassengerButtons(true);

            elevator.DeleteRequirementsHere();
            
            if (!Syncronize.SyncPassengers()) 
                Syncronize.ResetDoorTimer();

            if (elevator.Passengers <= 0)
                Syncronize.EnablePassengerMinusButton(false);
            else
                Syncronize.EnablePassengerMinusButton(true);                          
                       
            if (elevator.CheckForOverload())
            {
                elevator.SetState(Defaults.State.Overload);
                Syncronize.StopDoorTimer();
            }            
        }          
    }

    class FixedClosed : ElevatorState
    {
        public void Loop(Elevator elevator)
        {
            Syncronize.CloseDoor();

            if( !elevator.ReachedEndOfShaft )
            {
                if( elevator.FittingWishOnThisFloor )
                {
                    elevator.SetState( Defaults.State.FixedOpen );
                    return;
                }
                else if( !elevator.WishesInMyDirection && elevator.OppositeWishOnThisFloor )
                {
                    elevator.SetState( Defaults.State.FixedOpen );
                    return;
                }
            }
            else
            {
                elevator.SwitchDirection();

                if( elevator.OppositeWishOnThisFloor || elevator.FittingWishOnThisFloor )
                {
                    elevator.SetState(Defaults.State.FixedOpen);
                    return ;
                }
            }

            if( elevator.WishesInMyDirection )
            {
                elevator.SetState( Defaults.State.Moving );
                return;
            }

            //else if( elevator.OppositeWishOnThisFloor )
            //{
            //    elevator.SwitchDirection();
            //    elevator.SetState( Defaults.State.FixedOpen );
            //    return;
            //}

            else if( elevator.WishesInMyOppositeDirection )
            {
                elevator.SwitchDirection();
                elevator.SetState( Defaults.State.Moving );
                return;
            }

            else
            {
                elevator.TaskStatus = false;
            }
        }
    }

    class Moving : ElevatorState 
    {
        public void Loop(Elevator elevator) 
        {
            if( elevator.ReachedEndOfShaft )
            {
                elevator.SwitchDirection();
            }

            if(elevator.FittingWishOnThisFloor)
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
                        } break;
                    case Defaults.Direction.Downward:
                        {
                            elevator.CurrentFloor--;
                            Syncronize.SyncCurrentFloor();
                        } break;
                }

                Log.AddEntry("Aktuelle Etage:" + elevator.CurrentFloor.ToString());
                Syncronize.ResetMoveTimer();
                Syncronize.ShowDirection();
                return;
            }

            else if( elevator.OppositeWishOnThisFloor )
            {
                elevator.SwitchDirection();
                elevator.SetState( Defaults.State.FixedClosed );
                return;
            }
        }
    }

    class Overload : ElevatorState 
    {
        public void Loop(Elevator Elevator) 
        {
            Syncronize.SyncPassengers();
            if ( !Elevator.CheckForOverload() )
                Elevator.SetState( Defaults.State.FixedOpen );
        }
    }

    #endregion

    #endregion
}
