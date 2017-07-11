using System;

namespace OldSim
{
    internal class CaseWork
    {
        private int _rapporteurHours = 0;
        private int _chairHours = 0;
        private int _otherHours = 0;

        private readonly int _rapporteurTarget;
        private readonly int _chairTarget;
        private readonly int _otherTarget;


        internal WorkState RapporteurState{ get { return _evaluateState(_rapporteurHours, _rapporteurTarget); } }
        internal WorkState ChairState { get { return _evaluateState(_chairHours, _chairTarget); } }
        internal WorkState OtherState { get { return _evaluateState(_otherHours, _otherTarget); } }



        internal CaseWork(int rapporteurTarget, int chairTarget, int otherTarget)
        {
            _rapporteurTarget = rapporteurTarget;
            _chairTarget = chairTarget;
            _otherTarget = otherTarget;
        }


        

        internal void RecordRapporteurHour()
        {
            if (RapporteurState == WorkState.Finished)
                throw new InvalidOperationException("Attempt to record rapporteur hour, but rapporteur work is finshed");
            
            _rapporteurHours++;
        }


        internal void RecordOtherHour()
        {

            if (RapporteurState != WorkState.Finished)
                throw new InvalidOperationException("Attempt to record other hour, but rapporteur work is not finshed");

            if (OtherState == WorkState.Finished)
                throw new InvalidOperationException("Attempt to record other hour, but other work is finshed");

            _otherHours++;
        }


        internal void RecordChairHour()
        {
            if (RapporteurState != WorkState.Finished)
                throw new InvalidOperationException("Attempt to record chair hour, but rapporteur work is not finshed");


            if (OtherState != WorkState.Finished)
                throw new InvalidOperationException("Attempt to record chair hour, but other work is not finshed");

            if (ChairState == WorkState.Finished)
                throw new InvalidOperationException("Attempt to record chair hour, but chair work is finshed");

            _chairHours++;
        }




        private WorkState _evaluateState(int workHours, int targetHours)
        {
            if (workHours <= 0)
                return WorkState.Pending;

            if (workHours >= targetHours)
                return WorkState.Finished;

            return WorkState.Started;
        }
    }
}