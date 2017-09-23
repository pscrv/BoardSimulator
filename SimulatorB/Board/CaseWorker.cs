using System;

namespace SimulatorB
{
    internal abstract class CaseWorker
    {
        #region abstract
        protected abstract string _workerType { get; } 

        internal abstract int HoursForSummons { get; }
        internal abstract int HoursOPPreparation { get; }
        internal abstract int HoursForDecision { get; }
        
        #endregion


        internal readonly Member Member;


        internal CaseWorker(Member member)
        {
            Member = member;
        }


        public override string ToString()
        {
            return $"{_workerType} {Member}";
        }
    }



    internal class ChairWorker : CaseWorker
    {
        public ChairWorker(Member member) 
            : base(member)
        { }


        internal override int HoursForSummons => Member.ChairWorkParameters.HoursForSummons;
        internal override int HoursOPPreparation => Member.ChairWorkParameters.HoursOPPrepration;
        internal override int HoursForDecision => Member.ChairWorkParameters.HoursForDecision;


        protected override string _workerType => "Chair";


        

    }

    internal class RapporteurWorker : CaseWorker
    {
        public RapporteurWorker(Member member) : base(member)
        { }


        internal override int HoursForSummons => Member.RapporteurWorkParameters.HoursForSummons;
        internal override int HoursOPPreparation => Member.RapporteurWorkParameters.HoursOPPrepration;
        internal override int HoursForDecision => Member.RapporteurWorkParameters.HoursForDecision;


        protected override string _workerType => "Rapporteur";


        
    }

    internal class SecondWorker : CaseWorker
    {
        public SecondWorker(Member member) 
            : base(member)
        { }

        internal override int HoursForSummons => Member.SecondMemberWorkParameters.HoursForSummons;
        internal override int HoursOPPreparation => Member.SecondMemberWorkParameters.HoursOPPrepration;
        internal override int HoursForDecision => Member.SecondMemberWorkParameters.HoursForDecision;


        protected override string _workerType => "Second member";

        
    }

}
