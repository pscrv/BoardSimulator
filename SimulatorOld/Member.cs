using System;
using System.Collections.Generic;

namespace SimulatorOld
{
    internal class Member
    {
        #region TEMPORARY AREA  --- STATIC
        private static int HOURSPERSUMMONS = 2;
        private static int HOURSPERDECISION = 3;
        private static int OPPREPARATIONHOURS = 1;
        #endregion


        #region private fields
        private CaseQueuePair _rapporteurQueue;
        private CaseQueuePair _chairQueue;
        private CaseQueuePair _otherQueue;
        private OPList _opTimes;

        private AppealCase _currentCase;
        private Work.WorkType _currentWorkType;
        private Work.WorkRole _currentRole;
        private int _workCounter;
        #endregion


        #region internal properties
        internal WorkReport Report { get; private set; }
        internal int CaseCount { get { return _rapporteurQueue.Count + _chairQueue.Count + _otherQueue.Count; } }
        #endregion


        #region consctructor
        internal Member()
        {
            _rapporteurQueue = new CaseQueuePair();
            _chairQueue = new CaseQueuePair();
            _otherQueue = new CaseQueuePair();
            _opTimes = new OPList();

            _currentCase = null;
            _workCounter = 0;
        }
        #endregion


        #region internal methods
        internal void EnqueueRapporteurWork(AppealCase appealCase)
        {
            _rapporteurQueue.Enqueue(appealCase);
        }


        internal void EnqueueChairWork(AppealCase appealCase)
        {
            _chairQueue.Enqueue(appealCase);
        }


        internal void EnqueueOtherWork(AppealCase appealCase)
        {
            _otherQueue.Enqueue(appealCase);
        }


        internal void RegisterOP(AppealCase ac, Hour opStartHour, Hour opEndHour)
        {
            Hour start = opStartHour.Subtract(OPPREPARATIONHOURS);
            Hour end = opEndHour;
            _opTimes.Add(ac, new SimulationTimeSpan(start, end));
        }


        internal WorkReport DoWork()
        {
            AppealCase opCase = _opTimes.OPScheduledForCurrentHour;
            if (opCase != null)
                return new OPWorkReport(opCase);


            if (_currentCase == null)
                _getNextCase();
            if (_currentCase == null)
                return new NullWorkReport();
            
            _workCase();

            WorkReport report;  

            if (_workCounter == 0)
            {
                report = new WorkReport(_currentCase, _currentWorkType, _currentRole, Work.WorkState.Finished);
                _currentCase = null;
            }
            else
            {
                report = new WorkReport(_currentCase, _currentWorkType, _currentRole, Work.WorkState.Ongoing);
            }
            return report;
        }

        #endregion


        #region private methods
        private void _getNextCase()
        {
            if (_chairQueue.Count > 0)
            {
                _currentCase = _chairQueue.Dequeue();
                _currentRole = Work.WorkRole.Chair;
                _initialiseCase();
                return;
            }
            if (_otherQueue.Count > 0)
            {
                _currentCase = _otherQueue.Dequeue();
                _currentRole = Work.WorkRole.Other;
                _initialiseCase();
                return;
            }
            if (_rapporteurQueue.Count > 0)
            {
                _currentCase = _rapporteurQueue.Dequeue();
                _currentRole = Work.WorkRole.Rapporteur;
                _initialiseCase();
                return;
            }
            _currentCase = null;
        }

        
        private void _initialiseCase()
        {
            switch (_currentCase?.Stage)
            {
                case AppealCaseState.Stage.SummonsEnqueued:
                case AppealCaseState.Stage.DecisionEnqueued:
                    _currentCase.AdvanceState();
                    break;
            }

            switch (_currentCase?.Stage)
            {
                case AppealCaseState.Stage.SummonsStarted:
                    _workCounter = HOURSPERSUMMONS;
                    _currentWorkType = Work.WorkType.Summons;
                    break;
                case AppealCaseState.Stage.DecisionStarted:
                    _workCounter = HOURSPERDECISION;
                    _currentWorkType = Work.WorkType.Decision;
                    break;

                default:
                    throw new InvalidOperationException("_currentCase is null or in a state that does not allow work.");
            }



        }


        private void _workCase()
        {
            switch (_currentCase.Stage)
            {
                case AppealCaseState.Stage.SummonsStarted:
                case AppealCaseState.Stage.DecisionStarted:
                    _workCounter--;
                    break;
                default:
                    throw new InvalidOperationException("_currentCase is in an invalid state.");
            }
        }
        #endregion
    }
}