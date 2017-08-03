using System.Collections.Generic;

namespace Simulator
{
    public class BoardParameters
    {
        internal readonly ChairType ChairType;
        internal readonly MemberParameterCollection Chair;
        internal readonly List<MemberParameterCollection> Technicals;
        internal readonly List<MemberParameterCollection> Legals;


        public BoardParameters(
            ChairType chairType, 
            MemberParameterCollection chair, 
            List<MemberParameterCollection> technicals,
            List<MemberParameterCollection> legals)
        {
            ChairType = chairType;
            Chair = chair;
            Technicals = technicals;
            Legals = legals;
        }
    }
}