using System;

namespace SimulatorB
{
    internal class Member
    {
        #region static
        private static int __instanceCounter = 0;
        #endregion


        #region fields and properties     
        internal readonly int ID;        
        internal readonly MemberParameters ChairWorkParameters;
        internal readonly MemberParameters RapporteurWorkParameters;
        internal readonly MemberParameters SecondMemberWorkParameters;       
        
        #endregion


        #region construction
        internal Member(MemberParameterCollection parameters)
        {
            ID = __instanceCounter;
            __instanceCounter++;

            ChairWorkParameters = parameters.ChairWorkParameters;
            RapporteurWorkParameters = parameters.RapporteurWorkParameters;
            SecondMemberWorkParameters = parameters.SecondWorkParameters;
        }
        #endregion

        
        

        #region overrides
        public override string ToString()
        {
            return $"Member <{ID}>";
        }
        #endregion
        
    }



}
