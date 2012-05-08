#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace LiftSimulation
{
    static class EnvironmentConditions
    {
        #region Members
        private static int _numberOfFloors = 6;
        private static int _numberOfBasementFloors = 1;
        #endregion

        #region Properties

        public static int Floors
        {
            get { return _numberOfFloors; }
            //set { _numberOfFloors = value; }
        }
        public static int Basements 
        {
            get { return _numberOfBasementFloors; }
           // set { _numberOfBasementFloors = value; }
        }

        #endregion

        #region Methods
        //none
        #endregion
    }
}
