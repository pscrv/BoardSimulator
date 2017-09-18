using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulatorB
{
    public abstract class BoardParameters
    {
        #region abstract
        //internal abstract Board MakeBoard(Registrar registrar);
        #endregion


        #region fields
        internal readonly MemberParameterCollection Chair;

        internal readonly List<Tuple<MemberParameterCollection, int>> Technicals;
        internal readonly List<Tuple<MemberParameterCollection, int>> Legals;

        private Member _chairMember;
        private List<Tuple<Member, int>> _technicalMembers;
        private List<Tuple<Member, int>> _legalMembers;
        //private ChairChooser _chairChooser;
        #endregion



        #region properties
        internal Member ChairMember
        {
            get 
            {
                if (_chairMember == null)
                    _chairMember = new Member(Chair);
                return _chairMember;
            }
        }

        internal List<Member> TechnicalMembers
        {
            get
            {
                if (_technicalMembers == null)
                    _technicalMembers = _assembleMemberList(Technicals);
                return _technicalMembers.Select(x => x.Item1).ToList();
            }
        }

        internal List<Member> LegalMembers
        {
            get
            {
                if (_legalMembers == null)
                    _legalMembers = _assembleMemberList(Legals);
                return _legalMembers.Select(x => x.Item1).ToList();
            }
        }

        //internal ChairChooser ChairChooser
        //{
        //    get
        //    {
        //        if (_chairChooser == null)
        //            _chairChooser = _makeChairChooser();
        //        return _chairChooser;
        //    }
        //}
        #endregion


        #region construction
        protected BoardParameters(
            MemberParameterCollection chair, 
            List<MemberParameterCollection> technicals,
            List<MemberParameterCollection> legals)
        {
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
        #endregion


        #region private methods
        private static List<Tuple<Member, int>> _assembleMemberList(IEnumerable<Tuple<MemberParameterCollection, int>> parameterList)
        {
            List<Tuple<Member, int>> memberList = new List<Tuple<Member, int>>();
            foreach (Tuple<MemberParameterCollection, int> parameters in parameterList)
            {
                memberList.Add(new Tuple<Member, int>(new Member(parameters.Item1), parameters.Item2));
            }

            return memberList;
        }

        //private ChairChooser _makeChairChooser()
        //{
        //    ChairChooser chooser = new ChairChooser(ChairMember);
        //    foreach (var technical in _technicalMembers)
        //    {
        //        if (technical.Item2 > 0)
        //            chooser.AddSecondaryChair(technical.Item1, technical.Item2);
        //    }
        //    foreach (var legal in _legalMembers)
        //    {
        //        if (legal.Item2 > 0)
        //            chooser.AddSecondaryChair(legal.Item1, legal.Item2);
        //    }

        //    return chooser;
        //}
        #endregion
    }



    public class TechnicalBoardParameters : BoardParameters
    {
        public TechnicalBoardParameters(
            MemberParameterCollection chair,
            List<MemberParameterCollection> technicals,
            List<MemberParameterCollection> legals)
            : base(chair, technicals, legals)
        { }
        


        #region overrides
        //internal override Board MakeBoard(Registrar registrar)
        //{

        //    return Board.MakeTechnicalBoard(
        //        ChairMember,
        //        TechnicalMembers,
        //        LegalMembers,
        //        registrar,
        //        ChairChooser);
        //}
        #endregion
    }



    public class LegalBoardParameters : BoardParameters
    {
        public LegalBoardParameters(
            MemberParameterCollection chair,
            List<MemberParameterCollection> technicals,
            List<MemberParameterCollection> legals)
            : base(chair, technicals, legals)
        { }
        

        //internal override Board MakeBoard(Registrar registrar)
        //{
        //    return Board.MakeLegalBoard(
        //        ChairMember,
        //        TechnicalMembers,
        //        LegalMembers,
        //        registrar,
        //        ChairChooser);
        //}
    }


}