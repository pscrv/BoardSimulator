using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    internal class Case
    {
        private enum WorkStage { Summons, Decision }

        private WorkStage _workStage;
        private int SummonsCounter { get; set; }
        private int DecisionCounter { get; set; }


        internal CaseLog Log { get; private set; }
        internal CaseState State { get; }

        internal void DoWork()
        {
            throw new NotImplementedException();
        }

        internal bool StageFinshed(MemberWorkParameters workParameters)
        {
            switch (_workStage)
            {
                case WorkStage.Summons:
                    return SummonsCounter >= workParameters.HoursPerSummons;
                case WorkStage.Decision:
                    return DecisionCounter >= workParameters.HoursPerDecision;
            }
            throw new InvalidOperationException("Invalide state for Case: _workStage not set to Summons or Decision.");
        }
    }
}
