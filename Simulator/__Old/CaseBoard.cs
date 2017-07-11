using System;
using System.Collections.Generic;

namespace OldSim
{
    internal class CaseBoard
    {
        private CyclicQueue<Member> _workers;


        internal readonly Member Chair;
        internal readonly Member Rapporteur;
        internal readonly Member Other;



        #region constructors
        internal CaseBoard(Member chair, Member rapporteur, Member other)
        {
            Chair = chair;
            Rapporteur = rapporteur;
            Other = other;
            _workers = new CyclicQueue<Member>(new List<Member> { Rapporteur, Other, Chair, null });
        }
        #endregion


        #region internal methods
        internal Member GetNextWorker()
        {
            return _workers.Next;
        }

        internal Member Enqueue(AppealCase appealCase)
        {

            return null;
        }

        
        #endregion
    }
}