using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class Member
    {
        #region static
        private static int __instanceCounter = 0;
        #endregion



        #region fields and properties     
        private Dictionary<WorkerRole, MemberParameters> _parameters;

        internal readonly int ID;
        #endregion


        #region construction
        internal Member(MemberParameterCollection parameters)
        {
            ID = __instanceCounter;
            __instanceCounter++;

            _parameters = new Dictionary<WorkerRole, MemberParameters>
            {
                [WorkerRole.Chair] = parameters.ChairWorkParameters,
                [WorkerRole.Rapporteur] = parameters.RapporteurWorkParameters,
                [WorkerRole.OtherMember] = parameters.OtherWorkParameters
            };
        }
        #endregion


        internal MemberParameters GetParameters(WorkerRole role)
        {
            return _parameters[role];
        }


        internal WorkReport Work(Hour currentHour, AllocatedCase currentCase)
        {
            if (currentCase == null)
                return WorkReport.MakeNullReport();

            return currentCase.DoWorkAndMakeReport(this, currentHour);
        }
        


        #region overrides
        public override string ToString()
        {
            return string.Format("Member <{0}>", ID);
        }
        #endregion

    }



}
