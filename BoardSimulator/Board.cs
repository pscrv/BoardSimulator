using System;
using System.Collections.Generic;
using System.Linq;

namespace BoardSimulator
{
    internal class Board
    {
        #region enums
        public enum OutputTypes { Summons, Decision, Nothing }
        public enum WorkTypes { Summons, Decision, OP, Nothing }
        #endregion

        #region static  - default values
        protected static uint __simulationWeeks = 480; // 12 years

        static uint __chairSummonsHours = 4;
        static uint __chairDecisionHours = 8;
        static uint __chairOPPreparationHours = 4;

        static uint __numberOfMembers = 4;
        static uint __memberSummonsHours = 40;
        static uint __memberDecisionHours = 24;
        static uint __memberOPPreparationHours = 8;

        static uint __opLeadWeeks = 12;
        static uint __opDuration = 6;
        static uint __minDaysBetweenOP = 2;

        static uint __weeksPerYear = 40;
            internal static uint __WeeksPerYear { get { return __weeksPerYear; } }
        static uint __daysPerWeek = 5;
            internal static uint __DaysPerWeek { get { return __daysPerWeek; } }
        static uint __hoursPerDay = 8;
            internal static uint __HoursPerDay { get { return __hoursPerDay; } }
        #endregion

        #region private data fields
        private Chair _chair;
        private List<TechnicalMember> _technicalMembers;
        private uint _numberOfMembers;

        private Queue<OP> _opQueue;
        private uint _opLeadWeeks;
        private uint _opDuration;
        private uint _minDaysBetweenOPs;

        private uint _hour;
        private uint _totalHours;
        private List<BoardWorkReport> _reportLog;

        private uint _chairSummonsHours;
        private uint _chairDecisionHours;
        private uint _chairOPPreparationHours;
        private uint _memberSummonsHours;
        private uint _memberDecisionHours;
        private uint _memberOPPreparationHours;
        #endregion

        #region private derived properties
        private uint _currentDay { get { return _hour / __hoursPerDay; } }
        #endregion


        #region public parameters
        public uint MemberSummonsHours
        {
            get { return _memberSummonsHours; }
            set { _memberSummonsHours = value; }
        }

        public uint MemberDecisionHours
        {
            get { return _memberDecisionHours; }
            set { _memberDecisionHours = value; }
        }

        public uint MemberOPPreparationHours
        {
            get { return _memberOPPreparationHours; }
            set { _memberOPPreparationHours = value; }
        }

        public uint ChairSummonsHours
        {
            get { return _chairSummonsHours; }
            set { _chairSummonsHours = value; }
        }

        public uint ChairDecisionHours
        {
            get { return _chairDecisionHours; }
            set { _chairDecisionHours = value; }

        }

        public uint ChairOPPreparationHours
        {
            get { return _chairOPPreparationHours; }
            set { _chairOPPreparationHours = value; }
        }

        public uint NumberOfMembers
        {
            get { return _numberOfMembers; }
            set { _numberOfMembers = value; }
        }

        public uint OPDuration
        {
            get { return _opDuration; }
            set { _opDuration = value; }
        }
        
        public uint MinDaysBetweenOPs
        {
            get { return _minDaysBetweenOPs; }
            set { _minDaysBetweenOPs = value; }
        }
        #endregion


        #region public access
        internal uint Hour { get { return _hour; } }

        internal bool OPScheduled { get; private set; }  // set in Work()

        internal uint OPScheduledForDay { get; private set; }  // set in Work();

        internal ChairReport ChairReport { get { return new ChairReport(_chair, _hour); } }

        internal List<TechniclMemberReport> TechnichalMemberReports
        {
            get
            {
                List<TechniclMemberReport> list = new List<TechniclMemberReport>();
                foreach (TechnicalMember m in _technicalMembers)
                    list.Add(new TechniclMemberReport(m));
                return list;
            }
        }

        internal List<BoardWorkReport> ReportLogList { get { return _reportLog; } }
        #endregion



        #region constructors
        public Board()
        {
            _hour = 0;
            _totalHours = __simulationWeeks * __daysPerWeek * __hoursPerDay;
            _reportLog = new List<BoardWorkReport>();

            _chairSummonsHours = __chairSummonsHours;
            _chairDecisionHours = __chairDecisionHours;
            _chairOPPreparationHours = __chairOPPreparationHours;
            _memberSummonsHours = __memberSummonsHours;
            _memberDecisionHours = __memberDecisionHours;
            _memberOPPreparationHours = __memberOPPreparationHours;

            _opQueue = new Queue<OP>();
            _opLeadWeeks = __opLeadWeeks;
            _opDuration = __opDuration;
            _minDaysBetweenOPs = __minDaysBetweenOP;

            _chair = new Chair(_chairSummonsHours, _chairDecisionHours, _chairOPPreparationHours);
            _technicalMembers = new List<TechnicalMember>();
            _numberOfMembers = __numberOfMembers;
            for (int i = 0; i < _numberOfMembers; i++)
                _technicalMembers.Add(new TechnicalMember(_memberSummonsHours, _memberDecisionHours, _memberOPPreparationHours));
        }

        public Board(Chair ch, List<TechnicalMember> ms)
        {
            _hour = 0;
            _totalHours = __simulationWeeks * __daysPerWeek * __hoursPerDay;
            _reportLog = new List<BoardWorkReport>();

            _opQueue = new Queue<OP>();
            _opLeadWeeks = __opLeadWeeks;

            _chair = ch;
            _technicalMembers = ms;

            _chairSummonsHours = ch.SummonsHours;
            _chairDecisionHours = ch.DecisionHours;
            _memberSummonsHours = ms.First().SummonsHours;
            _memberDecisionHours = ms.First().DecisionHours;
        }
        
        public Board(Chair ch, TechnicalMember m)
        {
            _hour = 0;
            _totalHours = __simulationWeeks * __daysPerWeek * __hoursPerDay;
            _reportLog = new List<BoardWorkReport>();

            _opQueue = new Queue<OP>();
            _chair = ch;
            _technicalMembers = new List<TechnicalMember>();
            _technicalMembers.Add(m);

            _chairSummonsHours = ch.SummonsHours;
            _chairDecisionHours = ch.DecisionHours;
            _memberSummonsHours = m.SummonsHours;
            _memberDecisionHours = m.DecisionHours;
        }
        #endregion


        #region Work methods
        public void RunSimulation()
        {
            _reset();
            for (uint i = 0; i < _totalHours; i++)
            {
                _hour = i;
                _work();
                _logWork();
            }
        }

        public void Remake()
        {
            _chair = new Chair(_chairSummonsHours, _chairDecisionHours, _chairOPPreparationHours);
            _technicalMembers.Clear();
            for (int i = 0; i < _numberOfMembers; i++)
                _technicalMembers.Add(new TechnicalMember(_memberSummonsHours, _memberDecisionHours, _memberOPPreparationHours));
        }

        private void _reset()
        {
            _hour = 0;
            _totalHours = __simulationWeeks * __daysPerWeek * __hoursPerDay;
            _reportLog.Clear();
            _reportLog = new List<BoardWorkReport>();
            _opQueue = new Queue<OP>();
            _chair.Reset();
            foreach (TechnicalMember m in _technicalMembers)
            {
                m.Reset();
            }
        }

        private void _work()
        {
            OPScheduled = false;

            // see if anyone is busy with op prepration of the op itself
            bool chairBusyWithOP;
            bool rapporteurBusyWithOP;
            TechnicalMember opRapporteur;
            if (_opQueue.Count > 0)
            {
                OP op = _opQueue.Peek();
                chairBusyWithOP = op.ChairIsBusy(_hour);
                rapporteurBusyWithOP = op.RapporteurIsbusy(_hour);
                if (rapporteurBusyWithOP)
                    opRapporteur = op.Rapporteur;
                else
                    opRapporteur = null;
            }
            else
            {
                chairBusyWithOP = false;
                rapporteurBusyWithOP = false;
                opRapporteur = null;
            }

            // make sure each tech member has a summons to work on
            foreach (TechnicalMember m in _technicalMembers)
            {
                if (m.SummonsQueueIsEmpty)
                    m.EnqueueSummons(new Summons(m), _hour);
            }

            // chair and each tech member works
            if (chairBusyWithOP)
                _chair.DoOPWork();
            else
                _chair.Work();
            foreach (TechnicalMember m in _technicalMembers)
                if (opRapporteur == m)
                    m.DoOPWork();
            else
                    m.Work();

            // process chair output
            if (! chairBusyWithOP)
                switch (_chair.OutputType)
                {
                    case OutputTypes.Summons:
                        _scheduleOP(_chair.Output.Rapporteur);
                        OPScheduled = true;
                        break;

                    case OutputTypes.Decision:
                        break;

                    case OutputTypes.Nothing:
                        break;
                }

            //process member outputs
            foreach (TechnicalMember m in _technicalMembers)
            {
                if (! (opRapporteur == m))
                    switch (m.OutputType)
                    {
                        case Board.OutputTypes.Summons:
                            _chair.EnqueueSummons(m.Output as Summons, _hour);
                            break;

                        case Board.OutputTypes.Decision:
                            _chair.EnqueueDecision(m.Output as Decision, _hour);
                            break;

                        case Board.OutputTypes.Nothing:
                            break;
                    }
            }

            // if OP is finished, dequeue it and push a decision onto the member's queue
            if (_opQueue.Count > 0)
            {
                if (_opQueue.Peek().IsOver(_hour))
                {
                    TechnicalMember member = _opQueue.Peek().Rapporteur;
                    member.EnqueueDecision(new Decision(member), _hour);
                    _opQueue.Dequeue();
                }
            }

            // if OP has been scheduled, keep track of the time it is scheduled for
            if (OPScheduled)
                OPScheduledForDay = _opQueue.Last().StartDay;
            else
                OPScheduledForDay = 0;

        }

        private void _scheduleOP(TechnicalMember rapporteur)        
        {
            uint startDay = _getOPStartDay();
            OP op = new OP(_chair, rapporteur, startDay, _opDuration);
            _opQueue.Enqueue(op);
        }

        private uint _getOPStartDay()
        {
            uint earliestDay = _currentDay + 1 + (_opLeadWeeks * __daysPerWeek);

            // Get the last op end time
            uint lastEndDay;
            if (_opQueue.Count > 0)
                lastEndDay = _opQueue.Last().EndDay;
            else
                lastEndDay = 0;

            return Math.Max(lastEndDay + _minDaysBetweenOPs + 1, earliestDay);
        }

        private void _logWork()
        {
            _reportLog.Add(new BoardWorkReport(this));
        }
        #endregion
    }


    internal class BoardWorkReport
    {
        #region data fields
        public uint Hour { get; private set; }
        public uint Day { get; private set; }
        public uint Week { get; private set; }

        public uint OPScheduledForDay { get; private set; }

        public ChairReport ChairReport { get; private set; }
        public List<TechniclMemberReport> TechniclMemberReports{ get; private set; }
        #endregion

        public BoardWorkReport(Board b)
        {
            Hour = b.Hour;
            Day = b.Hour / Board.__HoursPerDay;
            Week = Day / Board.__DaysPerWeek;

            OPScheduledForDay = b.OPScheduledForDay;

            ChairReport = b.ChairReport;
            TechniclMemberReports = b.TechnichalMemberReports;
        }
    }


}
