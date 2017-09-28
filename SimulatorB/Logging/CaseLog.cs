using System;

namespace SimulatorB
{
    internal class CaseLog
    {
        #region summons properties
        internal Hour SummonsEnqueuedChair { get; private set; }
        internal Hour SummonsStartedChair { get; private set; }
        internal Hour SummonsFinishedChair { get; private set; }

        internal Hour SummonsEnqueuedRapporteur { get; private set; }
        internal Hour SummonsStartedRapporteur { get; private set; }
        internal Hour SummonsFinishedRapporteur { get; private set; }

        internal Hour SummonsEnqueuedSecondMember { get; private set; }
        internal Hour SummonsStartedSecondMember { get; private set; }
        internal Hour SummonsFinishedSecondMember { get; private set; }
        #endregion


        #region OP properties
        internal Hour OPEnqueuedChair { get; private set; }
        internal Hour OPStartedChair { get; private set; }
        internal Hour OPFinishedChair { get; private set; }

        internal Hour OPEnqueuedRapporteur { get; private set; }
        internal Hour OPStartedRapporteur { get; private set; }
        internal Hour OPFinishedRapporteur { get; private set; }

        internal Hour OPEnqueuedSecondMember { get; private set; }
        internal Hour OPStartedSecondMember { get; private set; }
        internal Hour OPFinishedSecondMember { get; private set; }
        #endregion


        #region Decision properties
        internal Hour DecisionEnqueuedChair { get; private set; }
        internal Hour DecisionStartedChair { get; private set; }
        internal Hour DecisionFinishedChair { get; private set; }

        internal Hour DecisionEnqueuedRapporteur { get; private set; }
        internal Hour DecisionStartedRapporteur { get; private set; }
        internal Hour DecisionFinishedRapporteur { get; private set; }

        internal Hour DecisionEnqueuedSecondMember { get; private set; }
        internal Hour DecisionStartedSecondMember { get; private set; }
        internal Hour DecisionFinishedSecondMember { get; private set; }
        #endregion


        #region Finished properties
        internal Hour Finished { get; private set; }
        #endregion



        #region generic setting methods
        internal void LogEnqueued(Hour hour, WorkCase workCase, CaseWorker worker)
        {  }

        internal void LogStarted(Hour hour, WorkCase workCase, CaseWorker worker)
        { }

        internal void LogFinished(Hour hour, WorkCase workCase, CaseWorker worker)
        { }
        #endregion


        #region summons setting methods
        internal void LogEnqueued(Hour hour, SummonsCase workCase, ChairWorker worker)
        {
            _checkIsNotAlreadyLogged(SummonsEnqueuedChair);
            SummonsEnqueuedChair = hour;
        }

        internal void LogEnqueued(Hour hour, SummonsCase workCase, RapporteurWorker worker)
        {
            _checkIsNotAlreadyLogged(SummonsEnqueuedRapporteur);
            SummonsEnqueuedRapporteur = hour;
        }

        internal void LogEnqueued(Hour hour, SummonsCase workCase, SecondWorker worer)
        {
            _checkIsNotAlreadyLogged(SummonsEnqueuedSecondMember);
            SummonsEnqueuedSecondMember = hour;
        }



        internal void LogStarted(Hour hour, SummonsCase workCase, ChairWorker worker)
        {
            _checkIsNotAlreadyLogged(SummonsStartedChair);
            SummonsStartedChair = hour;
        }

        internal void LogStarted(Hour hour, SummonsCase workCase, RapporteurWorker worker)
        {
            _checkIsNotAlreadyLogged(SummonsStartedRapporteur);
            SummonsStartedRapporteur = hour;
        }

        internal void LogStarted(Hour hour, SummonsCase workCase, SecondWorker worer)
        {
            _checkIsNotAlreadyLogged(SummonsStartedSecondMember);
            SummonsStartedSecondMember = hour;
        }




        internal void LogFinished(Hour hour, SummonsCase workCase, ChairWorker worker)
        {
            _checkIsNotAlreadyLogged(SummonsFinishedChair);
            SummonsFinishedChair = hour;
        }

        internal void LogFinished(Hour hour, SummonsCase workCase, RapporteurWorker worker)
        {
            _checkIsNotAlreadyLogged(SummonsFinishedRapporteur);
            SummonsFinishedRapporteur = hour;
        }
        
        internal void LogFinished(Hour hour, SummonsCase workCase, SecondWorker worer)
        {
            _checkIsNotAlreadyLogged(SummonsFinishedSecondMember);
            SummonsFinishedSecondMember = hour;
        }
        #endregion


        #region OP setting methods
        internal void LogEnqueued(Hour hour, OPCase workCase, CaseWorker worker)
        {
            _checkIsNotAlreadyLogged(OPEnqueuedChair);
            _checkIsNotAlreadyLogged(OPEnqueuedRapporteur);
            _checkIsNotAlreadyLogged(OPEnqueuedSecondMember);
            OPEnqueuedChair = hour;
            OPEnqueuedRapporteur = hour;
            OPEnqueuedSecondMember = hour;
        }        


        internal void LogStarted(Hour hour, OPCase workCase, ChairWorker worker)
        {
            _checkIsNotAlreadyLogged(OPStartedChair);
            OPStartedChair = hour;
        }

        internal void LogStarted(Hour hour, OPCase workCase, RapporteurWorker worker)
        {
            _checkIsNotAlreadyLogged(OPStartedRapporteur);
            OPStartedRapporteur = hour;
        }

        internal void LogStarted(Hour hour, OPCase workCase, SecondWorker worker)
        {
            _checkIsNotAlreadyLogged(OPStartedSecondMember);
            OPStartedSecondMember = hour;
        }


        internal void LogFinished(Hour hour, OPCase workCase, ChairWorker worker)
        {
            _checkIsNotAlreadyLogged(OPFinishedChair);
            OPFinishedChair = hour;
        }

        internal void LogFinished(Hour hour, OPCase workCase, RapporteurWorker worker)
        {
            _checkIsNotAlreadyLogged(OPFinishedRapporteur);
            OPFinishedRapporteur = hour;
        }

        internal void LogFinished(Hour hour, OPCase workCase, SecondWorker worker)
        {
            _checkIsNotAlreadyLogged(OPFinishedSecondMember);
            OPFinishedSecondMember = hour;
        }

        #endregion


        #region Decision setting methods
        internal void LogEnqueued(Hour hour, DecisionCase workCase, ChairWorker worker)
        {
            _checkIsNotAlreadyLogged(DecisionEnqueuedChair);
            DecisionEnqueuedChair = hour;
        }

        internal void LogEnqueued(Hour hour, DecisionCase workCase, RapporteurWorker worker)
        {
            _checkIsNotAlreadyLogged(DecisionEnqueuedRapporteur);
            DecisionEnqueuedRapporteur = hour;
        }

        internal void LogEnqueued(Hour hour, DecisionCase workCase, SecondWorker worer)
        {
            _checkIsNotAlreadyLogged(DecisionEnqueuedSecondMember);
            DecisionEnqueuedSecondMember = hour;
        }



        internal void LogStarted(Hour hour, DecisionCase workCase, ChairWorker worker)
        {
            _checkIsNotAlreadyLogged(DecisionStartedChair);
            DecisionStartedChair = hour;
        }

        internal void LogStarted(Hour hour, DecisionCase workCase, RapporteurWorker worker)
        {
            _checkIsNotAlreadyLogged(DecisionStartedRapporteur);
            DecisionStartedRapporteur = hour;
        }

        internal void LogStarted(Hour hour, DecisionCase workCase, SecondWorker worer)
        {
            _checkIsNotAlreadyLogged(DecisionStartedSecondMember);
            DecisionStartedSecondMember = hour;
        }




        internal void LogFinished(Hour hour, DecisionCase workCase, ChairWorker worker)
        {
            _checkIsNotAlreadyLogged(DecisionFinishedChair);
            DecisionFinishedChair = hour;
        }

        internal void LogFinished(Hour hour, DecisionCase workCase, RapporteurWorker worker)
        {
            _checkIsNotAlreadyLogged(DecisionFinishedRapporteur);
            DecisionFinishedRapporteur = hour;
        }

        internal void LogFinished(Hour hour, DecisionCase workCase, SecondWorker worer)
        {
            _checkIsNotAlreadyLogged(DecisionFinishedSecondMember);
            DecisionFinishedSecondMember = hour;
        }
        #endregion


        #region Finished setting methods
        internal void LogFinished(Hour hour)
        {
            _checkIsNotAlreadyLogged(Finished);
            Finished = hour;
        }
        #endregion






        private void _checkIsNotAlreadyLogged(Hour logEntry)
        {
            if (logEntry != null)
                throw new InvalidOperationException("Entry has already been logged.");
        }
    }
}
