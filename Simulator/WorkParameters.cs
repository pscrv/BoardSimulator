namespace Simulator
{
    internal class WorkParameters
    {
        internal int HoursForsummons { get; private set; }
        internal int HoursForDecision { get; private set; }
        internal int HoursforOPPreparation { get; private set; }

        internal WorkParameters(int summons, int decision, int op)
        {
            HoursForsummons = summons;
            HoursForDecision = decision;
            HoursforOPPreparation = op;
        }
    }
}