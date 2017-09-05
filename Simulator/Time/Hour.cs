using System;

namespace Simulator
{
    internal class Hour : IEquatable<Hour>, IComparable<Hour>
    {
        #region static
        private static Hour _max;
        private static Hour _min;

        public static Hour MaxHour { get { if (_max == null) _max = new Hour(int.MaxValue); return _max; } }
        public static Hour MinHour { get { if (_min == null) _min = new Hour(int.MinValue); return _min; } }
        #endregion


        #region fields and properties
        internal readonly int Value;


        private bool _isFirstHourOfDay { get => Value % TimeParameters.HoursPerDay == 0; }
        private bool _isFirstHourOfWeek { get => Value % (TimeParameters.HoursPerDay * TimeParameters.DaysPerWeek) == 0; }
        private bool _isFirstHourOfMonth { get => Value % TimeParameters.HoursPerMonth == 0; }
        private bool _isFirstHourOfYear { get => Value % TimeParameters.HoursPerYear == 0; }
        #endregion


        #region construction
        internal Hour(int h)
        {
            Value = h;
        }
        #endregion


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



        internal Hour NextFirstHourOfDay()
        {
            return this._isFirstHourOfDay ? this : this.FirstHourOfNextDay();
        }

        internal Hour NextFirstHourOfWeek()
        {
            return this._isFirstHourOfWeek ? this : this.FirstHourOfNextWeek();
        }

        internal Hour NextFirstHourOfMonth()
        {
            return this._isFirstHourOfMonth ? this : this.FirstHourOfNextMonth();
        }

        internal Hour NextFirstHourOfYear()
        {
            return this._isFirstHourOfYear ? this : this.FirstHourOfNextYear();
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


        #region overrides
        public override string ToString()
        {
            return string.Format("Hour <{0}>", Value);
        }
        #endregion

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


        #region operators etc
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

        internal static Hour Max(Hour a, Hour b)
        {
            if (a.Value > b.Value)
                return a;
            return b;
        }

        internal static Hour Min(Hour a, Hour b)
        {
            if (a.Value < b.Value)
                return a;
            return b;
        }

        #endregion
    }
}