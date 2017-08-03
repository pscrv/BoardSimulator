using System;

namespace Simulator
{
    internal class CaseWorker
    {
        #region fields and properties
        internal readonly Member Member;
        internal readonly WorkerRole Role;

        internal int ID { get { return Member.ID; } }

        internal int HoursForSummons { get { return Member.GetParameters(Role).HoursForSummons; } }
        internal int HoursForDecision { get { return Member.GetParameters(Role).HoursForDecision; } }
        internal int HoursOPPreparation { get { return Member.GetParameters(Role).HoursOPPrepration; } }
        #endregion


        #region construction
        internal CaseWorker(Member member, WorkerRole role)
        {
            Member = member;
            Role = role;
        }
        #endregion     
    }
}