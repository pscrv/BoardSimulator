namespace SimulatorB
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


        #region construction
        internal AppealCase()
        {
            ID = __instanceCounter;
            __instanceCounter++;
        }
        #endregion

        


        #region overrides
        public override string ToString()
        {
            return $"CaseNumber <{ID}>";
        }
        #endregion
    }
}
