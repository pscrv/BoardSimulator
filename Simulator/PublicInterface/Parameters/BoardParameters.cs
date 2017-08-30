using System;
using System.Collections.Generic;

namespace Simulator
{
    public class BoardParameters
    {
        internal readonly ChairType ChairType;
        internal readonly MemberParameterCollection Chair;
        //internal readonly List<MemberParameterCollection> Technicals;
        //internal readonly List<MemberParameterCollection> Legals;

        internal readonly List<Tuple<MemberParameterCollection, int>> xTechnicals;
        internal readonly List<Tuple<MemberParameterCollection, int>> xLegals;


        public BoardParameters(
            ChairType chairType, 
            MemberParameterCollection chair, 
            List<MemberParameterCollection> technicals,
            List<MemberParameterCollection> legals)
        {
            ChairType = chairType;
            Chair = chair;
            //Technicals = technicals;
            //Legals = legals;

            xTechnicals = new List<Tuple<MemberParameterCollection, int>>();
            xLegals = new List<Tuple<MemberParameterCollection, int>>();

            foreach (var t in technicals)
            {
                xTechnicals.Add(new Tuple<MemberParameterCollection, int>(t, 0));
            }

            foreach (var l in legals)
            {
                xLegals.Add(new Tuple<MemberParameterCollection, int>(l, 0));
            }
        }

        public BoardParameters(
            ChairType chairType,
            MemberParameterCollection chair,
            List<Tuple<MemberParameterCollection, int>> xtechnicals,
            List<Tuple<MemberParameterCollection, int>> xlegals)
        {
            ChairType = chairType;
            Chair = chair;
            xTechnicals = xtechnicals;
            xLegals = xlegals;
        }
    }
}