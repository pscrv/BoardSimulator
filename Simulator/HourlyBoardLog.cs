using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class HourlyBoardLog
    {
        internal Dictionary<BoardWorker, HourlyworkerLog> Log { get; private set; }

        internal HourlyBoardLog()
        {
            Log = new Dictionary<BoardWorker, HourlyworkerLog>();
        }

        internal void Add(BoardWorker worker, HourlyworkerLog memberlog)
        {
            Log[worker] = memberlog;
        }
    }
}