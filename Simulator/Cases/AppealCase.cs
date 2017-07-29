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
        internal AppealCase(Hour creation)
        {
            ID = __instanceCounter;
            CreationHour = creation;

            __instanceCounter++;
        }

        internal AppealCase()
            : this(new Hour(0))
        { }

        #endregion

        


        #region overrides
        public override string ToString()
        {
            return string.Format("CaseNumber <{0}>", ID);
        }
        #endregion
    }
}
