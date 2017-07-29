using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulator
{
    internal class SimulationTimeSpan : IEnumerable<Hour>
    {
        #region fields and properties
        internal readonly Hour Start;
        internal readonly Hour End;
        
        internal int DurationHours
        {
            get
            {
                if (End == null)
                    return int.MaxValue;
                return End.Value - Start.Value;
            }
        }
        #endregion


        #region constructors
        internal SimulationTimeSpan(Hour start, Hour end)
        {
            if (start == null)
                throw new InvalidOperationException("Cannot consctruct SimulationTimeSpan with start time == null.");

            Start = start;
            End = end;
        }
        #endregion



        internal bool Contains(Hour h)
        {
            return h >= Start && h <= End;
        }


        #region IEnumerable
        public IEnumerator<Hour> GetEnumerator()
        {
            Hour current = Start;
            while (current <= End)
            {
                yield return current;
                current = current.Next();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion


        #region overrides
        public override string ToString()
        {
            return string.Format("{0} --- {1}", Start, End);
        }
        #endregion

    }
    
}