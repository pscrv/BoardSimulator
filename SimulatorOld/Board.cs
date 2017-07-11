using System;
using System.Collections.Generic;

namespace SimulatorOld
{
    internal class Board
    {
        #region TEMPORARY STATIC
        private static int OPLEADTIME = 3;
        private static int OPDURATION = 2;
        #endregion


        #region private fields
        private Member _chair;
        private List<Member> _technical;
        private List<Member> _legal;

        private AppealCaseQueue _incomingCases;
        private AppealCaseQueue _circulatingCases;
        private Dictionary<AppealCase, Allocation> _activeCases;
        private Dictionary<AppealCase, Hour> _scheduledOPs;
        #endregion


        #region internal properties
        //internal BoardLog Log { get; private set; }
        internal int IncomingCaseCount { get { return _incomingCases.Count; } }
        #endregion


        #region constructors
        internal Board(Member chair, List<Member> technical, List<Member> legal)
        {
            _chair = chair;
            _technical = technical;
            _legal = legal;

            _incomingCases = new AppealCaseQueue();
            _circulatingCases = new AppealCaseQueue();
            _activeCases = new Dictionary<AppealCase, Allocation>();

            _scheduledOPs = new Dictionary<AppealCase, Hour>();
        }
        #endregion

        #region internal methods
        internal void EnqueueNewCase(AppealCase appealCase)
        {
            _incomingCases.Enqueue(appealCase);
        }

        internal BoardLog DoWork()
        {
            _processCirculatingCases();
            _processIncomingCases();

            BoardLog log = new BoardLog();
            foreach (Member m in _members())
            {
                WorkReport report = m.DoWork();
                log.Add(m, report);

                if (report.State == Work.WorkState.Finished)
                {
                    AppealCase appealCase = report.Case;
                    _circulatingCases.Enqueue(appealCase);
                }
            }

            return log;   
        }
        #endregion



        #region private methods
        private void _processIncomingCases()
        {
            while (_incomingCases.Count > 0)
            {
                AppealCase appealCase = _incomingCases.Dequeue();
                Allocation allocation = _allocate(appealCase);
                Member firstWorker = allocation.Enqueue(appealCase);
                _activeCases[appealCase] = allocation;
            }
        }


        private void _processCirculatingCases()
        {
            while (_circulatingCases.Count > 0)
            {
                AppealCase appealCase = _circulatingCases.Dequeue();
                Allocation allocation = _activeCases[appealCase];
                Member nextWorker = allocation.Enqueue(appealCase);

                if (nextWorker == null)
                {
                    appealCase.AdvanceState();
                    Hour opStartHour =  _scheduleOP(appealCase);
                    Hour opEndHour = opStartHour.Add(OPDURATION);
                    allocation.NotifyOPSchedule(appealCase, opStartHour, opEndHour);
                }
            }
        }

        private Hour _scheduleOP(AppealCase appealCase)
        {
            // TODO: make a proper scheduler
            Hour startHour = SimulationTime.Future(OPLEADTIME);
            _scheduledOPs[appealCase] = startHour;
            return startHour;
        }

        private Allocation _allocate(AppealCase appealCase)
        {
            // TODO: make a proper allocation
            return new Allocation(_chair, _technical[0], _legal[0]);
        }

        private IEnumerable<Member> _members()
        {
            yield return _chair;
            foreach (Member tm in _technical) yield return tm;
            foreach (Member lm in _legal) yield return lm;
        }
        #endregion
    }
}
