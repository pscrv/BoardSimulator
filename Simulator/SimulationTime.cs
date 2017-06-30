namespace Simulator
{
    internal static class SimulationTime
    {
        internal static Hour Current { get; private set; }

        static SimulationTime() { Current = new Hour(0); }


        internal static void Increment() { Current.Next(); }

        internal static void Reset() { Current = new Hour(0); }
    }


    internal class Hour
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

        public override string ToString()
        {
            return string.Format("Hour <{0}>", Value);
        }
    }
}