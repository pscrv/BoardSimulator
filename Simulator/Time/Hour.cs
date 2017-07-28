using System;

namespace Simulator
{
    internal class Hour : IEquatable<Hour>, IComparable<Hour>
    {
        internal readonly int Value;

        internal Hour(int h)
        {
            Value = h;
        }


        internal Hour Next()
        {
            return new Hour(Value + 1);
        }

        internal Hour Previous()
        {
            return new Hour(Value - 1);
        }

        internal Hour FirstHourOfNextDay()
        {
            return _firstHourOfNextUnit(TimeParameters.HoursPerDay);
        }

        internal Hour FirstHourOfNextWeek()
        {
            return _firstHourOfNextUnit(TimeParameters.HoursPerDay * TimeParameters.DaysPerWeek);
        }

        internal Hour FirstHourOfNextMonth()
        {
            return _firstHourOfNextUnit(TimeParameters.HoursPerDay * TimeParameters.DaysPerMonth);
        }

        internal Hour FirstHourOfNextYear()
        {
            return _firstHourOfNextUnit(TimeParameters.HoursPerDay * TimeParameters.DaysPerWeek * TimeParameters.WeeksPerYear);
        }

        private Hour _firstHourOfNextUnit(int unit)
        {
            int hourOfUnit = Value % unit;
            return new Hour(Value + unit - hourOfUnit);
        }


        internal Hour AddHours(int offset)
        {
            return new Hour(Value + offset);
        }

        internal Hour AddDays(int offset)
        {
            return AddHours(offset * TimeParameters.HoursPerDay);
        }

        internal Hour AddMonths(int offset)
        {
            return AddHours(offset * TimeParameters.HoursPerDay * TimeParameters.DaysPerMonth);
        }


        internal Hour SubtractHours(int offset)
        {
            return new Hour(Value - offset);
        }

        internal Hour SubtractDays(int offset)
        {
            return SubtractHours(offset * TimeParameters.HoursPerDay);
        }

        internal Hour SubtractMonths(int offset)
        {
            return SubtractHours(offset * TimeParameters.HoursPerDay * TimeParameters.DaysPerMonth);
        }

        public override string ToString()
        {
            return string.Format("Hour <{0}>", Value);
        }


        #region IEquatable
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (ReferenceEquals(obj, null))
                return false;
            return this.Equals(obj as Hour);

        }

        public bool Equals(Hour other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return this.Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        #endregion


        #region IComparable
        public int CompareTo(Hour other)
        {
            if (other == null)
                throw new NullReferenceException("Hour.CompareTo: attempt to compare to null.");

            return Value.CompareTo(other.Value);
        }
        #endregion


        #region operators
        public static bool operator < (Hour a, Hour b)
        {
            return a.Value < b.Value;
        }

        public static bool operator <= (Hour a, Hour b)
        {
            return a.Value <= b.Value;
        }

        public static bool operator > (Hour a, Hour b)
        {
            return a.Value > b.Value;
        }

        public static bool operator >= (Hour a, Hour b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator == (Hour a, Hour b)
        {
            if (ReferenceEquals(a, null))
                return ReferenceEquals(b, null);
            return a.Equals(b);             
        }

        public static bool operator != (Hour a, Hour b)
        {
            return ! (a == b);
        }

        #endregion
    }
}