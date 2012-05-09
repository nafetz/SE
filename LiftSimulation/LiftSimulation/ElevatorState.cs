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
    }

    /// <summary>
    /// State-Class, nur Methoden, keine Member!
    /// </summary>
    class FixedOpen : ElevatorState
    {
    }

    /// <summary>
    /// State-Class, nur Methoden, keine Member!
    /// </summary>
    class FixedClosed : ElevatorState
    {
    }

    class Moving : ElevatorState 
    {
    }
}
