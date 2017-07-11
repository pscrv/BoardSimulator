using System;
using System.Collections.Generic;

namespace SimulatorOld
{
    internal class BoardLog
    {
        #region internal properties
        internal readonly Dictionary<Member, WorkReport> Log;
        #endregion


        #region constructors
        internal BoardLog()
        {
            Log = new Dictionary<Member, WorkReport>();
        }
        #endregion


        #region internal methods
        internal void Add(Member m, WorkReport r)
        {
            if (Log.ContainsKey(m))
                throw new InvalidOperationException("Member has already been logged.");

            Log[m] = r;
        }
        #endregion

    }
}