using System;
using System.Collections.Generic;

namespace Simulator
{
    internal static class WorkQueues
    {
        internal static BoardQueue Members = new BoardQueue();
        internal static IncomingCaseQueue Incoming = new IncomingCaseQueue();
        internal static CirculationQueue Circulation = new CirculationQueue();
 
        internal static OPSchedule OPSchedule = new OPSchedule();

        
        internal static void ClearAllQueues()
        {
            Members.ClearAll();
            Circulation.Clear();
            OPSchedule = new OPSchedule();
        }
    }
}