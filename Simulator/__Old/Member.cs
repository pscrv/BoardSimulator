using System;
using System.Collections.Generic;

namespace OldSim
{
    internal class Member
    {
        #region TEMPORARY STATIC ATTRIBUTES
        private static int __HOURPERSUMMONS = 2;
        private static int __HOURSOPPREPARATION = 1;
        private static int __HOURSPERDECISION = 3;
        #endregion


        #region private fields
        private AppealCaseQueuePair _incomingQueue;
        private AppealCaseQueuePair _activeQueue;
        private AppealCase _currentCase;
        private int _workCounter;        
        #endregion


        #region internal properties
        internal readonly int HoursPerSummons;
        internal readonly int HoursOPPreparation;
        internal readonly int HoursPerDecision;
        #endregion


        #region consctructor
        internal Member()
        {
            HoursPerSummons = __HOURPERSUMMONS;
            HoursOPPreparation = __HOURSOPPREPARATION;
            HoursPerDecision = __HOURSPERDECISION;

            _incomingQueue = new AppealCaseQueuePair();
            _activeQueue = new AppealCaseQueuePair();
            _currentCase = null;
            _workCounter = 0;
        }
        #endregion


        #region internal methods
        internal void Enqueue(AppealCase ac)
        {
            _incomingQueue.Enqueue(ac);
        }


        internal void Enqueue(AllocatedCase ac)
        {

        }




        internal void DoAndLogWork(SimulationLog log)
        {
            Work.WorkType worktype;
            Work.WorkRole workrole = Work.WorkRole.Rapporteur;  //TODO: fix this
            WorkReport report;


            if (_opWorkToDo())
            {
                report = WorkReport.MakeOPReport(null, Work.WorkRole.None, Work.WorkState.None);
                //log.Add(this, report);
                return;
            }

            if (_currentCase == null)
                _setCurrentCase();
            if (_currentCase == null)
            {
                _logNoWork(log);
                return;
            }

            _workCounter++;
            int target;
            switch (_currentCase.Stage)
            {
                case AppealCaseState.Stage.SummonsStarted:
                    target = __HOURPERSUMMONS;
                    worktype = Work.WorkType.Summons;
                    break;
                case AppealCaseState.Stage.DecisionStarted:
                    target = __HOURSPERDECISION;
                    worktype = Work.WorkType.Decision;
                    break;

                default:
                    throw new InvalidOperationException("<DoAndLogWork>: _currentCase in in invalid stage " + _currentCase.Stage);
            }

            if (_workCounter == target)
            {
                report = WorkReport.MakeReport(_currentCase, worktype, workrole, Work.WorkState.Finished);
                _currentCase = null;
                _workCounter = 0;
            }
            else
            {
                report = WorkReport.MakeReport(_currentCase, worktype, workrole, Work.WorkState.Ongoing);
            }
            
            //log.Add(this, report);

        }

        #endregion


        #region private methods
        private void _setCurrentCase()
        {
            if (_activeQueue.Count > 0)
                _currentCase = _activeQueue.Dequeue();
            else if (_incomingQueue.Count > 0)
                _currentCase = _incomingQueue.Dequeue();
            else
                return;

            _workCounter = 0;
            switch (_currentCase.Stage)
            {
                case AppealCaseState.Stage.SummonsEnqueued:
                case AppealCaseState.Stage.DecisionEnqueued:
                    _currentCase.AdvanceState();
                    break;

                case AppealCaseState.Stage.SummonsStarted:
                case AppealCaseState.Stage.DecisionStarted:
                    break;

                default:
                    throw new InvalidOperationException("<_setCurrentCase>: _currentCase is in invalid stage " + _currentCase.Stage);
            }


        }

        private void _logNoWork(SimulationLog log)
        {
            //log.Add(this, WorkReport.MakeNullReport());
        }
        #endregion

        

        private bool _opWorkToDo()
        {
            // TODO: make a real check
            return false;
        }
    }
}