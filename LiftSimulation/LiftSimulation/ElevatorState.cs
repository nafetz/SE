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
    /// </summary>
    class ElevatorState
    {
        public void Move(Elevator Elevator) { }
    }

    /// <summary>
    /// State-Class, nur Methoden, keine Member!
    /// </summary>
    class FixedOpen : ElevatorState
    {
        public new void Move(Elevator Elevator) { }
    }

    /// <summary>
    /// State-Class, nur Methoden, keine Member!
    /// </summary>
    class FixedClosed : ElevatorState
    {
        public new void Move(Elevator Elevator) { }
    }

    class Moving : ElevatorState 
    {
        public new void Move(Elevator Elevator) { }
    }
}
