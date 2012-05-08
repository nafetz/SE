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
    class DoorState
    {
        public void open() { }
        public void close() { }
    }

    /// <summary>
    /// State-Class, nur Methoden, keine Member!
    /// </summary>
    class Open : DoorState
    {
        public new void open() { }
        public new void close() { }
    }

    /// <summary>
    /// State-Class, nur Methoden, keine Member!
    /// </summary>
    class Closed : DoorState
    {
        public new void open() { }
        public new void close() { }
    }
}
