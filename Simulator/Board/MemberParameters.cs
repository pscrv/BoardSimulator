namespace Simulator
{
    internal class MemberParameters
    {

        #region temporary static
        private static int _HOURS_SUMMONS = 2;
        private static int _HOURS_OP_PREP = 1;
        private static int _HOURS_DECISION = 2;
        #endregion


        internal readonly int HoursForSummons;
        internal readonly int HoursOPPrepration;
        internal readonly int HoursForDecision;


        internal MemberParameters()
        {
            HoursForSummons = _HOURS_SUMMONS;
            HoursOPPrepration = _HOURS_OP_PREP;
            HoursForDecision = _HOURS_DECISION;
        }
    }
}