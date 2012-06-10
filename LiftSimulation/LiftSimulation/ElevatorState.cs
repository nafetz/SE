﻿#region using
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
        public virtual void finish(Elevator Elevator) { }
    }

    class FixedOpen : ElevatorState
    {
        public override void Loop( Elevator Elevator ) 
        {
            Syncronize.open_door();
            Syncronize.DoorTimerReset();
            
            if (!Syncronize.syncPassengers()) 
                Syncronize.DoorTimerReset(); //Lichtschrake übertreten --> Neustart des Türtimers

            if (Elevator.CheckForOverload())
            {
                Elevator.SetState(Defaults.State.Overload);
                Syncronize.DoorTimerStop();
                // return ;
            }            
        } 

        public override void finish(Elevator Elevator)
        {
            Syncronize.PassengerButtonsEnable(false);
            Elevator.SetState(Defaults.State.FixedClosed);
            //Elevator.CurrentState.Loop(Elevator);
        }   
    }

    /// <summary>
    /// Eigentlich PseudoZustand, der nur Übergänge einleitet
    /// </summary>
    class FixedClosed : ElevatorState
    {
        public override void Loop(Elevator Elevator)
        {
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
            //else if (Wünsche in der Gegenrichtung ||)
            //  richtungswechesel & go!
        }
    }

    class Moving : ElevatorState 
    {
        public override void Loop(Elevator Elevator) 
        {

            //if( Elevator.ReachedHighestOrLowestFloor )
            //{
            //    Elevator.SwitchDirection();
            //}

            if( Elevator.ThereAreWishesOnThisFloor )
            {
                Elevator.SetState(Defaults.State.FixedClosed);
                return ;
            }  
              
            if( Elevator.ThereAreWishesInMyDirection )
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
                Syncronize.MoveTimerReset();
                Syncronize.visibleDirection();
                return ;

            }// if
           

        
        }
    }

    class Overload : ElevatorState 
    {
        public override void Loop(Elevator Elevator) 
        {
            if (!Elevator.CheckForOverload())
            {
                Elevator.SetState(Defaults.State.FixedOpen);
            }
        }
    }
}
