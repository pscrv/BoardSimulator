using System;

namespace OldSim
{
    internal static class SimulationTime
    {
        internal static Hour Current { get; private set; }

        static SimulationTime() { Current = new Hour(0); }


        internal static void Increment() { Current = Current.Next(); }

        internal static void Reset() { Current = new Hour(0); }

        internal static Hour Future(int increment)
        {
            return Current.Add(increment);
        }
    }


    internal class SimulationTimeSpan
    {
        #region private fields
        private Hour _start;
        private Hour _end;
        #endregion


        #region constructors
        internal SimulationTimeSpan(Hour start, Hour end)
        {
            _start = start;
            _end = end;
        }
        #endregion


        #region internal properties
        internal bool ConatainsCurrent
        {
            get
            {
                return _start.Value <= SimulationTime.Current.Value && SimulationTime.Current.Value <= _end.Value;
            }
        }
        #endregion


        #region overrides
        public override string ToString()
        {
            return string.Format("{0} --- {1}", _start, _end);
        }
        #endregion
    }


    internal class Hour : IEquatable<Hour>
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


        internal Hour Add(int offset)
        {
            return new Hour(Value + offset);
        }


        internal Hour Subtract(int offset)
        {
            return new Hour(Value - offset);
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
            if (obj == null)
                return false;
            return this.Equals(obj as Hour);
            
        }


        public bool Equals(Hour other)
        {
            if (other == null)
                return false;
            return this.Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        #endregion
    }
}