﻿using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class Member
    {
        #region static
        private static int __instanceCounter = 0;

        internal static Member DefaultMember()
        {
            MemberParameters chair = new MemberParameters(16, 4, 8);
            MemberParameters rapporteur = new MemberParameters(40, 8, 24);
            MemberParameters other = new MemberParameters(8, 4, 8);

            MemberParameterCollection parameters =
                new MemberParameterCollection(chair, rapporteur, other);

            return new Member(parameters);
        }
        #endregion



        #region fields and properties        
        private int _workCounter = 0;
        private BoardQueue _boardQueues = WorkQueues.Members;
        private CirculationQueue _circulationQueue = WorkQueues.Circulation;
        private Dictionary<WorkerRole, MemberParameters> _parameters;

        private AllocatedCase _currentCase { get { return _boardQueues.Peek(this); } }
        private WorkerRole _currentRole { get { return _currentCase.Board.GetRole(this); } }
        private CaseWorker _thisAsCaseWorker { get { return _currentCase.Board.GetMemberAsCaseWorker(this); } }

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

            _boardQueues.Register(this);
        }
        #endregion


        internal MemberParameters GetParameters(WorkerRole role)
        {
            return _parameters[role];
        }


        internal void Work(Hour currentHour)
        {

            if (_currentCase == null)
            {
                _logWork(); // no work
                return;
            }

            if (_workCounter == 0)
            {
                _currentCase.RecordStartOfWork(_thisAsCaseWorker, currentHour);
                _setWorkCounter();
            }

            _workCounter--;
            _logWork();

            if (_workCounter == 0)
            {
                _finishCase(currentHour);
            }
        }



        private void _setWorkCounter()
        {
            if (_currentCase == null)
                return;

            switch (_currentCase.WorkType)
            {
                case WorkType.Summons:
                    _workCounter = _parameters[_currentRole].HoursForSummons;
                    break;
                case WorkType.Decision:
                    _workCounter = _parameters[_currentRole].HoursForDecision;
                    break;
                case WorkType.None:
                    throw new InvalidOperationException("member.Work: no work to do on this case.");
            }
        }

        private void _finishCase(Hour currentHour)
        {
            _currentCase.RecordFinishedWork(_thisAsCaseWorker, currentHour);
            if (_currentCase.Stage != CaseStage.Finished)
            {
                _circulationQueue.Enqueue(_currentCase);
            }
            _boardQueues.Dequeue(this);
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
