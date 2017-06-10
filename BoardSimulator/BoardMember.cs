using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardSimulator
{
    abstract class BoardMember
    {
        #region private data fields
        private uint _summonsHours;
        private uint _decisionHours;
        private uint _opPreparationHours;

        private SummonsQueue _summonsQueue;
        private DecisionQueue _decisionQueue;

        private Summons _currentSummons;
        private Decision _currentDecision;
        #endregion

        #region constructors
        public BoardMember() { throw new NotImplementedException("Boardmember paramterless constructor"); }

        public BoardMember(uint s_hours, uint d_hours, uint op_prep_hours)
        {
            _summonsHours = s_hours;
            _decisionHours = d_hours;
            _opPreparationHours = op_prep_hours;
            _summonsQueue = new SummonsQueue();
            _decisionQueue = new DecisionQueue();
        }
        #endregion

        #region public access
        public uint OPPreparationHours
        {
            get { return _opPreparationHours; }
            set { _opPreparationHours = value; }
        }
        public uint DecisionHours
        {
            get { return _decisionHours; }
            set { _decisionHours = value; }
        }
        public uint SummonsHours
        {
            get { return _summonsHours; }
            set { _summonsHours = value; }
        }

        public bool SummonsQueueIsEmpty { get { return _summonsQueue.Count == 0; } }
        public uint SummonsQueueSize { get { return (uint)_summonsQueue.Count; } }
        public uint DecisionQueueSize { get { return (uint)_decisionQueue.Count; } }


        public Board.WorkTypes WorkType { get; private set; }
        public Board.OutputTypes OutputType { get; private set; }
        public Work Output { get; private set; }
        #endregion


        #region public methods
        public uint SummonsQueueAgeAtHour(uint hour)
        { return _summonsQueue.AgeAtHour(hour); }

        public uint DecisionQueueAgeAtHour(uint hour)
        { return _decisionQueue.AgeAtHour(hour); }

        internal void EnqueueSummons(Summons s, uint hour)
        {
            s.Reset();
            _summonsQueue.Enqueue(s, hour);
        }

        internal void EnqueueDecision(Decision d, uint hour)
        {
            d.Reset();
            _decisionQueue.Enqueue(d, hour);
        }

        internal void Work()
        {
            WorkType = Board.WorkTypes.Nothing;
            OutputType = Board.OutputTypes.Nothing;
            Output = null;

            if (_currentDecision == null && _decisionQueue.Count > 0)
                _currentDecision = _decisionQueue.Dequeue();
            if (_currentSummons == null && _summonsQueue.Count > 0)
                _currentSummons = _summonsQueue.Dequeue();

            if (_currentDecision != null)
            {
                WorkType = Board.WorkTypes.Decision;
                _currentDecision.DoWork();
                if (_currentDecision.WorkHours == _decisionHours)
                {
                    OutputType = Board.OutputTypes.Decision;
                    Output = _currentDecision.Copy();
                    _currentDecision = null;
                }
            }
            else if (_currentSummons != null)
            {
                WorkType = Board.WorkTypes.Summons;
                _currentSummons.DoWork();
                if (_currentSummons.WorkHours == _summonsHours)
                {
                    OutputType = Board.OutputTypes.Summons;
                    Output = _currentSummons.Copy();
                    _currentSummons = null;
                }
            }        
        }

        internal void DoOPWork()
        {
            OutputType = Board.OutputTypes.Nothing;
            Output = null;
            WorkType = Board.WorkTypes.OP;
        }

        internal void Reset()
        {
            _summonsQueue.Clear();
            _decisionQueue.Clear();
            _currentSummons = null;
            _currentDecision = null;
        }
        #endregion
    }

    class Chair : BoardMember
    {
        #region constructors
        public Chair()
            : base()
        { }

        public Chair(uint s_hours, uint d_hours, uint op_prep_hours)
            : base(s_hours, d_hours, op_prep_hours)
        { }

        #endregion
    }

    class TechnicalMember : BoardMember
    {
        #region constructors
        public TechnicalMember()
            : base()
        { }

        public TechnicalMember(uint s_hours, uint d_hours, uint op_prep_hours)
            : base(s_hours, d_hours, op_prep_hours)
        { }

        #endregion
    }




    class BoardMemberReport
    {
        internal bool SummonsWork { get; private set; }
        internal bool SummonsOut { get; private set; }
        internal bool DecisionWork { get; private set; }
        internal bool DecisionOut { get; private set; }
        internal bool OPWork { get; private set; }
        internal bool FreeTime { get; private set; }
        
        public BoardMemberReport(BoardMember m)
        {
            SummonsWork = m.WorkType == Board.WorkTypes.Summons;
            DecisionWork = m.WorkType == Board.WorkTypes.Decision;
            OPWork = m.WorkType == Board.WorkTypes.OP;
            FreeTime = m.WorkType == Board.WorkTypes.Nothing;

            SummonsOut = m.OutputType == Board.OutputTypes.Summons;
            DecisionOut = m.OutputType == Board.OutputTypes.Decision;
        }
    }

    class ChairReport : BoardMemberReport
    {
        internal uint SummonsQueueSize { get; private set; }
        internal uint DecisionQueueSize { get; private set; }
        internal uint SummonsQueueAge { get; private set; }
        internal uint DecisionQueueAge { get; private set; }

        public ChairReport(Chair m, uint hour)
            : base(m)
        {
            SummonsQueueSize = m.SummonsQueueSize;
            SummonsQueueAge = m.SummonsQueueAgeAtHour(hour);
            DecisionQueueSize = m.DecisionQueueSize;
            DecisionQueueAge = m.DecisionQueueAgeAtHour(hour);
        }
    }

    class TechniclMemberReport : BoardMemberReport
    {
        public TechniclMemberReport(TechnicalMember m)
            : base(m)
        { }
    }

}
