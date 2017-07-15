using System;
using System.Collections.Generic;

namespace Simulator
{
    internal static class WorkQueues
    {
        internal static BoardQueues Members = new BoardQueues();
        internal static IncomingCaseQueue Incoming = new IncomingCaseQueue();
        internal static CirculationQueue Circulation = new CirculationQueue();
        internal static OPQueue OP = new OPQueue();
 

        
        internal static void ClearAllQueues()
        {
            Members.ClearAll();
            Circulation.Clear();
            OP.Clear();
        }
    }
}