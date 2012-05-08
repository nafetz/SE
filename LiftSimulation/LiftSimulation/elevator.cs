#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace LiftSimulation
{
    class Elevator
    {
        #region Member

        // State-Patterns
        private DoorState _door;
        private DirectionState _direction;

        // Floor-Stuff
        private int _currentFloor;
        List<bool> _upwardRequired = new List<bool>( EnvironmentConditions.Floors );
        List<bool> _downwardRequired = new List<bool>( EnvironmentConditions.Floors );

        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion
    }
}
