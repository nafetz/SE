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

            //_elevator.SetState(Defaults.State.Fixed);

            _ui.CurrentPosition = Elevator.CurrentFloor;
            _ui.enableInnerButtons(false);
        }

        #endregion


        #region Properties

        public static bool TaskStatus
        {
            get{
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
                _ui.ResetPassengerIO();
                return false;
            }
                        
            else if (_ui.PassengersIO == Defaults.MoreOrLess.More)
            {
                _elevator.Passengers++;
                _ui.PassengersCount = _elevator.Passengers;
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
            _ui.PlusPassengersButton.Enabled = value;
            _ui.MinusPassengersButton.Enabled = value;
        }

        public static void EnablePassengerMinusButton(bool value)
        {
            _ui.MinusPassengersButton.Enabled = value;
        }

        public static void SetState(Defaults.State state)
        {
            //_ui.Doortimer.Start();

            _elevator.SetState(state);        
        }

        public static void ResetDoorTimer()
        {
            _ui.Doortimer.Stop();
            _ui.Doortimer.Start();
        }

        public static void StopDoorTimer()
        {
            _ui.Doortimer.Stop();
        }

        public static void ResetMoveTimer()
        {
            _ui.Movetimer.Start();
        }

        public static void SwitchDirection()
        {
            _ui.ChangeDirection();
        }        

        public static void ShowDirection()
        {
            _ui.show_direction();
        }

        public static void ExecuteLoop()
        {
            _elevator.CurrentState.Loop(_elevator);
        }


        public static void OpenDoor()
        {
            _ui.open_door(Defaults.FloorToIdx(_elevator.CurrentFloor));
        }

        public static void CloseDoor()
        {
            _ui.close_door(Defaults.FloorToIdx(_elevator.CurrentFloor));
        }

        public static void EnableInnerButtons(bool value)
        {
            _ui.enableInnerButtons(true);
        }

        public static void _logging(Defaults.Logentry entry)
        {
            _ui.logging = entry;
        }

        #endregion
    }
}
