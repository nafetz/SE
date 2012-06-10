using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiftSimulation
{
    class Syncronize
    {
        #region member

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

        #region Property

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

        public static int syncFloor()
        {
            return Defaults.FloorToIdx(_elevator.CurrentFloor);

        }

        public static void syncUpwardWishes(To who)
        {
            switch (who)
            {
                case To.UI: {
                    _ui.UpwardRequired = _elevator.UpwardRequired;
                } break;
                case To.Elevator: 
                    {
                        _elevator.UpwardRequired = _ui.UpwardRequired;
                        _elevator.CurrentState.Loop(_elevator);
                    } break;
            }
        }

        public static void syncDownwardWishes(To who)
        {
            switch (who)
            {
                case To.UI: {
                    _ui.DownwardRequired = _elevator.DownwardRequired;
                } break;
                case To.Elevator: 
                    {
                        _elevator.DownwardRequired = _ui.DownwardRequired;
                        _elevator.CurrentState.Loop(_elevator);
                    } break;
            }
        }

        public static void syncinnerWishes(To who)
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

        public static bool syncPassengers()
        {
            if (_ui.PassengersIO == Defaults.MoreOrLess.Less)
            {
                _elevator.Passengers--;
                _ui.PassengersCount = _elevator.Passengers;
                return false;
            }
                        
            else if (_ui.PassengersIO == Defaults.MoreOrLess.More)
            {
                _elevator.Passengers++;
                _ui.PassengersCount = _elevator.Passengers;
                return false;
            }
            else
                return true;
        }


        public static void PassengerButtonsEnable(bool value)
        {
            _ui.PlusPassengersButton.Enabled = value;
            _ui.MinusPassengersButton.Enabled = value;
        }

        public static void PassenderMinusButtonEnable(bool value)
        {
            _ui.MinusPassengersButton.Enabled = value;
        }

        public static void SetState(Defaults.State state)
        {
            //_ui.Doortimer.Start();

            _elevator.SetState(state);
        
        }

        public static void DoorTimerReset()
        {
            _ui.Doortimer.Stop();
            _ui.Doortimer.Start();
        }

        public static void DoorTimerStop()
        {
            _ui.Doortimer.Stop();
        }

        public static void MoveTimerReset()
        {
            _ui.Movetimer.Start();
        }

        public static void SwitchDirection()
        {
            _ui.ChangeDirection();
        }

        public static void SyncCurrentFloor()
        {
            _ui.CurrentPosition = _elevator.CurrentFloor;
            
        }

        public static void visibleDirection()
        {
            _ui.show_direction();
        }

        public static void executeLoop()
        {
            _elevator.CurrentState.Loop(_elevator);
        }

        public static void executeFinish()
        {
            _elevator.CurrentState.Finish(_elevator);
        }

        public static void open_door()
        {
            _ui.open_door(Defaults.FloorToIdx(_elevator.CurrentFloor));
        }

        public static void close_door()
        {
            _ui.close_door(Defaults.FloorToIdx(_elevator.CurrentFloor));
        }

        public static void enableInnerButton(bool value)
        {
            _ui.enableInnerButtons(true);
        }

        






        #endregion
    }
}
