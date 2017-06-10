using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class CaseState
    {
        public enum States
        {
            Arrived,
            SummonsStarted,
            SummonsWritten,
            SummonsSent,
            OPPreparation,
            OPRunning,
            OPFinished,
            DecisionStarted,
            DecisionWritten,
            Finished
        }

        public States CurrentState { get; private set; }

        public CaseState()
        {
            CurrentState = States.Arrived;
        }

        public void AdvanceState()
        {
            switch (CurrentState)
            {
                case States.Arrived:
                    CurrentState = States.SummonsStarted;
                    break;
                case States.SummonsStarted:
                    CurrentState = States.SummonsWritten;
                    break;
                case States.SummonsWritten:
                    CurrentState = States.SummonsSent;
                    break;
                case States.SummonsSent:
                    CurrentState = States.OPPreparation;
                    break;
                case States.OPPreparation:
                    CurrentState = States.OPRunning;
                    break;
                case States.OPRunning:
                    CurrentState = States.OPFinished;
                    break;
                case States.OPFinished:
                    CurrentState = States.DecisionStarted;
                    break;
                case States.DecisionStarted:
                    CurrentState = States.DecisionWritten;
                    break;
                case States.DecisionWritten:
                    CurrentState = States.Finished;
                    break;
                case States.Finished:
                    break;
            }
        }

        public void Finish()
        {
            CurrentState = States.Finished;
        }

    }
}
