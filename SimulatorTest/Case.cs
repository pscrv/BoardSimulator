using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simulator;

namespace SimulatorTests
{
    [TestClass]
    public class Case
    {
        [TestMethod]
        public void TestCaseState_Normal()
        {
            CaseState state = new CaseState();
            Assert.AreEqual(CaseState.States.Arrived, state.CurrentState);
            state.AdvanceState();
            Assert.AreEqual(CaseState.States.SummonsStarted, state.CurrentState);
            state.AdvanceState();
            Assert.AreEqual(CaseState.States.SummonsWritten, state.CurrentState);
            state.AdvanceState();
            Assert.AreEqual(CaseState.States.SummonsSent, state.CurrentState);
            state.AdvanceState();
            Assert.AreEqual(CaseState.States.OPPreparation, state.CurrentState);
            state.AdvanceState();
            Assert.AreEqual(CaseState.States.OPRunning, state.CurrentState);
            state.AdvanceState();
            Assert.AreEqual(CaseState.States.OPFinished, state.CurrentState);
            state.AdvanceState();
            Assert.AreEqual(CaseState.States.DecisionStarted, state.CurrentState);
            state.AdvanceState();
            Assert.AreEqual(CaseState.States.DecisionWritten, state.CurrentState);
            state.AdvanceState();
            Assert.AreEqual(CaseState.States.Finished, state.CurrentState);
            state.AdvanceState();
            Assert.AreEqual(CaseState.States.Finished, state.CurrentState);
        }

        [TestMethod]
        public void CaseState_FinishEarly()
        {
            CaseState state = new CaseState();
            Assert.AreEqual(CaseState.States.Arrived, state.CurrentState);
            state.AdvanceState();
            Assert.AreEqual(CaseState.States.SummonsStarted, state.CurrentState);
            state.Finish();
            Assert.AreEqual(CaseState.States.Finished, state.CurrentState);
            state.AdvanceState();
            Assert.AreEqual(CaseState.States.Finished, state.CurrentState);
        }
    }
}
