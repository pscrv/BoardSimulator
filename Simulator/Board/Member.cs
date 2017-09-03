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

            CaseWorker thisAsCaseWorker = currentCase.Board.GetMemberAsCaseWorker(this);
            if (currentCase.Stage == CaseStage.OP)
            {
                return WorkReport.MakeOPReport(currentCase.Case, thisAsCaseWorker.Role);
            }

            WorkState workState = currentCase.DoWork(thisAsCaseWorker, currentHour);
            if (workState == WorkState.Finished)
            {
                currentCase.RecordFinishedWork(thisAsCaseWorker, currentHour);               
            }

            return WorkReport.MakeReport(
                currentCase.Case,
                currentCase.WorkType,
                thisAsCaseWorker.Role,
                workState);
        }


        internal WorkReport OPWork(AllocatedCase currentCase)
        {
            if (currentCase == null)
                throw new InvalidOperationException("Member.OPWork: currentCase is null.");

            return WorkReport.MakeOPReport(
                currentCase.Case,
                currentCase.Board.GetMemberAsCaseWorker(this).Role);
        }




        #region overrides
        public override string ToString()
        {
            return string.Format("Member <{0}>", ID);
        }
        #endregion

    }



}
