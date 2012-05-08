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
    class DirectionState
    {
        public void change() { }
        public void stop() { }
    }

    /// <summary>
    /// State-Class, nur Methoden, keine Member!
    /// </summary>
    class Upward : DirectionState
    {
        public void change() { }
        public void stop() { }
    }

    /// <summary>
    /// State-Class, nur Methoden, keine Member!
    /// </summary>
    class Downwards : DirectionState
    {
        public void change() { }
        public void stop() { }
    }
}
