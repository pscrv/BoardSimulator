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
        private int _workCounter = 0;
        private Dictionary<WorkerRole, MemberParameters> _parameters;

        internal readonly int ID;
        #endregion


        #region construction
        internal Member(MemberParameterCollection parameters)
        {
            ID = __instanceCounter;
            __instanceCounter++;

            _parameters = new Dictionary<WorkerRole, MemberParameters>();
            _parameters[WorkerRole.Chair] = parameters.ChairWorkParameters;
            _parameters[WorkerRole.Rapporteur] = parameters.RapporteurWorkParameters;
            _parameters[WorkerRole.OtherMember] = parameters.OtherWorkParameters;

        }
        #endregion


        internal MemberParameters GetParameters(WorkerRole role)
        {
            return _parameters[role];
        }


        internal WorkState Work(Hour currentHour, AllocatedCase currentCase)
        {

            if (currentCase == null)
            {
                _logWork(); // no work
                return WorkState.None;
            }

            CaseWorker thisAsCaseWorker = currentCase.Board.GetMemberAsCaseWorker(this);
            if (_workCounter == 0)
            {
                currentCase.RecordStartOfWork(thisAsCaseWorker, currentHour);
                _setWorkCounter(currentCase);
            }

            _workCounter--;
            _logWork();

            if (_workCounter == 0)
            {
                currentCase.RecordFinishedWork(thisAsCaseWorker, currentHour);
                return WorkState.Finished;
            }

            return WorkState.Ongoing;
        }



        private void _setWorkCounter(AllocatedCase currentCase)
        {
            if (currentCase == null)
                return;

            WorkerRole currentRole = currentCase.Board.GetRole(this);
            switch (currentCase.WorkType)
            {
                case WorkType.Summons:
                    _workCounter = _parameters[currentRole].HoursForSummons;
                    break;
                case WorkType.Decision:
                    _workCounter = _parameters[currentRole].HoursForDecision;
                    break;
                case WorkType.None:
                    throw new InvalidOperationException("member.Work: no work to do on this case.");
            }
        }



        private void _logWork()
        {
            // TODO: log work
        }





        #region overrides
        public override string ToString()
        {
            return string.Format("Member <{0}>", ID);
        }
        #endregion

    }



}
