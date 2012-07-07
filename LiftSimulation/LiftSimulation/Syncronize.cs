using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiftSimulation
{
    class Syncronize
    {
        #region Member

        private static Elevator _elevator;
        private static UserInterface _ui;
        public enum To { UI=1, Elevator }

        #endregion


        #region Konstruktoren

        public Syncronize(ref Elevator Elevator, ref UserInterface UI)
        {
            _elevator = Elevator;
            _ui = UI;
            _ui.CurrentPosition = Elevator.CurrentFloor;
        }

        #endregion


        #region Properties

        public static bool TaskStatus
        {
            get
            {
                return _elevator.TaskStatus;
            }

            set{
               _elevator.TaskStatus = value;
            }

        }

        #endregion


        #region Methoden

        public static int SyncFloor()
        {
            return Defaults.FloorToIdx(_elevator.CurrentFloor);
        }

        public static void SyncUpwardWishes(To who)
        {
            switch (who)
            {
                case To.UI: 
                    {
                        _ui.UpwardRequired = _elevator.UpwardRequired;
                    } break;
                case To.Elevator: 
                    {
                        _elevator.UpwardRequired = _ui.UpwardRequired;                       
                    } break;
            }
        }

        public static void SyncDownwardWishes(To who)
        {
            switch (who)
            {
                case To.UI: 
                    {
                        _ui.DownwardRequired = _elevator.DownwardRequired;
                    } break;
                case To.Elevator: 
                    {
                        _elevator.DownwardRequired = _ui.DownwardRequired;                        
                    } break;
            }
        }

        public static void SyncInnerWishes(To who)
        {            
            switch (who)
            {
                case To.UI: 
                    {
                        _ui.InternRequired = _elevator.InternRequired;
                        
                    } break; 
                case To.Elevator: 
                    {
                        _elevator.InternRequired = _ui.InternRequired;
                    } break;
            }
        }

        public static bool SyncPassengers()
        {
            if (_ui.PassengersIO == Defaults.MoreOrLess.Less)
            {
                _elevator.Passengers--;
                _ui.PassengersCount = _elevator.Passengers;

                Log.AddEntry("Person ausgestiegen, Anzahl der Personen im Fahrstuhl: " + _elevator.Passengers);

                _ui.ResetPassengerIO();
                return false;
            }
                        
            else if (_ui.PassengersIO == Defaults.MoreOrLess.More)
            {
                _elevator.Passengers++;
                _ui.PassengersCount = _elevator.Passengers;

                Log.AddEntry("Person eingestiegen, Anzahl der Personen im Fahrstuhl: " + _elevator.Passengers);

                _ui.ResetPassengerIO();
                return false;
            }
            else
                return true;
        }

        public static void SyncCurrentFloor()
        {
            _ui.CurrentPosition = _elevator.CurrentFloor;            
        }


        public static void EnablePassengerButtons(bool value)
        {
            _ui.PlusPassengerButton.Enabled = value;
            _ui.MinusPassengerButton.Enabled = value;
        }

        public static void EnablePassengerMinusButton(bool value)
        {
            _ui.MinusPassengerButton.Enabled = value;
        }

        public static void SetState(Defaults.State state)
        {
            _elevator.SetState(state);
        }

        public static void ResetDoorTimer()
        {
            _ui.DoorTimer.Stop();
            _ui.DoorTimer.Start();
        }

        public static void StopDoorTimer()
        {
            _ui.DoorTimer.Stop();
        }

        public static void ResetMoveTimer()
        {
            _ui.MoveTimer.Start();
        }

        public static void SwitchDirection()
        {
            _ui.SwitchDirection();
        }        

        public static void ShowDirection()
        {
            _ui.ShowDirection();
        }

        public static void ExecuteLoop()
        {
            _elevator.CurrentState.Loop(_elevator);
        }


        public static void OpenDoor()
        {
            _ui.OpenDoor(Defaults.FloorToIdx(_elevator.CurrentFloor));
        }

        public static void CloseDoor()
        {
            _ui.CloseDoor(Defaults.FloorToIdx(_elevator.CurrentFloor));
        }

        #endregion
    }
}
