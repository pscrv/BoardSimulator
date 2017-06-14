using System;

namespace Simulator
{
    internal class Hour
    {
        public int Value { get; private set; }

        public Hour (int value)
        {
            if (value < 0)
                throw new ArgumentException("Cannot assign an Hour < 0.");

            Value = value;
        }
    }
}