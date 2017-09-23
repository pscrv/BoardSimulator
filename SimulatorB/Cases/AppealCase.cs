namespace SimulatorB
{
    internal class AppealCase
    {
        #region static
        private static int __instanceCounter = 0;
        #endregion
        

        #region internal fields and properties
        internal readonly int ID;
        internal readonly CaseLog Log;
        #endregion


        #region construction
        internal AppealCase()
        {
            ID = __instanceCounter;
            __instanceCounter++;
            Log = new CaseLog();
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
