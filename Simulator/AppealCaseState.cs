using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class AppealCaseState
    {
        internal enum Stage
        {
            New,
            SummonsEnqueued,
            SummonsStarted,
            SummonsFinished,
            OPPending,
            OPFinsished,
            DecisionEnqueued,
            DecisionStarted,
            DecisionFinished
        }


        #region private fields
        private Queue<Stage> _stage;
        private Dictionary<Stage, Hour> _log;
        #endregion


        #region internal properties
        internal Stage CurrentStage { get { return _stage.Peek(); } }
        #endregion


        #region constructors
        internal AppealCaseState()
        {
            _stage = new Queue<Stage>();
            _log = new Dictionary<Stage, Hour>();

            _stage.Enqueue(Stage.New);
            _stage.Enqueue(Stage.SummonsEnqueued);
            _stage.Enqueue(Stage.SummonsStarted);
            _stage.Enqueue(Stage.SummonsFinished);
            _stage.Enqueue(Stage.OPPending);
            _stage.Enqueue(Stage.OPFinsished);
            _stage.Enqueue(Stage.DecisionEnqueued);
            _stage.Enqueue(Stage.DecisionStarted);
            _stage.Enqueue(Stage.DecisionFinished);

            _logCurrentStage();
        }
        #endregion


        #region internal methods
        internal void Advance()
        {
            if (CurrentStage != Stage.DecisionFinished)
                _stage.Dequeue();
            _logCurrentStage();        
        }
        #endregion



        #region private methods
        private void _logCurrentStage()
        {
            _log[CurrentStage] = SimulationTime.Current;
        }
        #endregion
    }


    internal class AppealCaseState_old
    {
        internal enum Stage { New, Summons, OP, Decision, Finished }


        #region internal properties
        internal Hour Arrived { get; private set; }
        internal Hour SummonsEnqueued { get; private set; }
        internal Hour SummonsStarted { get; private set; }
        internal Hour SummonsFinished { get; private set; }
        internal Hour OPStarted { get; private set; }
        internal Hour OPFinished { get; private set; }
        internal Hour DecisionEnqueued { get; private set; }
        internal Hour DecisionStarted { get; private set; }
        internal Hour DecisionFinished { get; private set; }
        #endregion


        #region internal properties
        internal Stage AppealStage
        {
            get
            {
                if (Arrived != null && SummonsEnqueued == null)
                    return Stage.New;
                if (SummonsEnqueued != null && OPStarted == null)
                    return Stage.Summons;
                if (OPStarted != null && DecisionStarted != null)
                    return Stage.OP;
                if (DecisionStarted != null && DecisionFinished == null)
                    return Stage.Decision;
                if (DecisionFinished != null)
                    return Stage.Finished;

                throw new InvalidOperationException("AppealStateCase is invalid.");
            }
        }
        #endregion



        #region constructor
        internal AppealCaseState_old()
        {
            Arrived = SimulationTime.Current;
        }
        #endregion


        #region internal methods
        internal void SetSummonsEnqueued()
        {
            _setState(SummonsEnqueued, Arrived);
        }

        internal void SetSummonsStarted()
        {
            _setState(SummonsStarted, SummonsEnqueued);
        }

        internal void SetSummonsFinisheded()
        {
            _setState(SummonsFinished, SummonsStarted);
        }
        
        internal void SetOPStarted()
        {
            _setState(OPStarted, SummonsStarted);
        }

        internal void SetOPFinisheded()
        {
            _setState(OPFinished, OPStarted);
        }

        internal void SetDecisionEnqueued()
        {
            _setState(DecisionEnqueued, OPFinished);
        }

        internal void SetDecisionStarted()
        {
            _setState(DecisionStarted, DecisionEnqueued);
        }

        internal void SetDecisionFinished()
        {
            _setState(DecisionFinished, DecisionStarted);
        }
        #endregion


        #region private methods
        private void _setState(Hour stateToSet, Hour previousState)
        {
            Hour now = SimulationTime.Current;

            if (stateToSet != null)
                throw new InvalidOperationException(
                    string.Format(
                        "The state is already set for {0}. Time is now {1}.",
                        stateToSet, 
                        now));

            if (previousState == null)
                throw new InvalidOperationException(
                        "Cannot set the state because the previous stat has not been set.");

            if (previousState.Value > now.Value)
                throw new InvalidOperationException(
                    string.Format(
                        "The previous state was set at {0} after the current time ({1}).",
                        previousState,
                        now));

            stateToSet = now;
        }
        #endregion
    }
}