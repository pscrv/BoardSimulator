using System;

namespace Simulator
{
    internal abstract class CStage
    {
        internal abstract CaseStage Stage { get; }
        internal abstract CStage Next();
    }


    internal class SummonsStage : CStage
    {
        internal override CaseStage Stage => CaseStage.Summons;

        internal override CStage Next()
        {
            return new OPStage();
        }
    }


    internal class OPStage : CStage
    {
        internal override CaseStage Stage => CaseStage.OP;

        internal override CStage Next()
        {
            return new DecisionStage();
        }
    }


    internal class DecisionStage : CStage
    {
        internal override CaseStage Stage => CaseStage.Decision;

        internal override CStage Next()
        {
            return new FinishedStage();
        }
    }



    internal class FinishedStage : CStage
    {
        internal override CaseStage Stage => CaseStage.Finished;

        internal override CStage Next()
        {
            return new FinishedStage();
        }
    }


    internal class CStageMachine : CStage
    {
        internal CStage Current { get; private set; }

        internal override CaseStage Stage => Current.Stage;

        internal CStageMachine()
        {
            Current = new SummonsStage();
        }


        internal override CStage Next()
        {
            Current = Current.Next();
            return Current;
        }
    }

}