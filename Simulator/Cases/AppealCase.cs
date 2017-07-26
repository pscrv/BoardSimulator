namespace Simulator
{
    internal class AppealCase
    {
        #region static
        private static int __instanceCounter = 0;
        #endregion

        

        #region internal fields and properties
        internal readonly int ID;
        internal readonly Hour CreationHour;
        #endregion


        #region constructors
        internal AppealCase()
        {
            ID = __instanceCounter;
            CreationHour = SimulationTime.CurrentHour;

            __instanceCounter++;
        }
        #endregion

        


        #region overrides
        public override string ToString()
        {
            return string.Format("CaseNumber <{0}>", ID);
        }
        #endregion
    }
}
