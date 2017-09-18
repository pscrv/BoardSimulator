using System;
using System.Collections;
using System.Collections.Generic;

namespace SimulatorB
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

        internal IEnumerable<Hour> FirstHoursOfDays
        {
            get
            {
                Hour hour = Start.NextFirstHourOfDay();
                while (hour <= End)
                {
                    yield return hour;
                    hour = hour.AddDays(1);
                }
            }
        }
        #endregion


        #region constructors
        internal SimulationTimeSpan(Hour start, Hour end)
        {
            if (start == null)
                throw new InvalidOperationException("Cannot consctruct SimulationTimeSpan with start time == null.");
            
            Start = start ?? Hour.MinHour;
            End = end ?? Hour.MaxHour;
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



        #region static operators
        internal static SimulationTimeSpan Intersection(SimulationTimeSpan a, SimulationTimeSpan b)
        {
            Hour start = Hour.Max(a.Start, b.Start);
            Hour end = Hour.Min(a.End, b.End);
            if (start > end)
                return null;
            return new SimulationTimeSpan(start, end);
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