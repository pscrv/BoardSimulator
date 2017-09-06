using System;
using System.Collections.Generic;

namespace Simulator
{
    public class BoardParameters
    {
        internal readonly ChairType ChairType;
        internal readonly MemberParameterCollection Chair;

        internal readonly List<Tuple<MemberParameterCollection, int>> Technicals;
        internal readonly List<Tuple<MemberParameterCollection, int>> Legals;


        public BoardParameters(
            ChairType chairType, 
            MemberParameterCollection chair, 
            List<MemberParameterCollection> technicals,
            List<MemberParameterCollection> legals)
        {
            ChairType = chairType;
            Chair = chair;

            Technicals = new List<Tuple<MemberParameterCollection, int>>();
            Legals = new List<Tuple<MemberParameterCollection, int>>();

            foreach (var t in technicals)
            {
                Technicals.Add(new Tuple<MemberParameterCollection, int>(t, t.ChairWorkPercentage));
            }

            foreach (var l in legals)
            {
                Legals.Add(new Tuple<MemberParameterCollection, int>(l, l.ChairWorkPercentage));
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
            Technicals = xtechnicals;
            Legals = xlegals;
        }
    }
}