using System;
using System.Collections;
using System.Collections.Generic;

namespace SimulatorB
{
    internal class CaseBoard : IEnumerable<CaseWorker>
    {
        internal readonly CaseWorker Chair;
        internal readonly CaseWorker Rapporteur;
        internal readonly CaseWorker SecondWorker;


        internal CaseBoard(ChairWorker chair, RapporteurWorker rapporteur, SecondWorker second)
        {
            Chair = chair;
            Rapporteur = rapporteur;
            SecondWorker = second;
        }

        #region IEnumerable
        public IEnumerator<CaseWorker> GetEnumerator()
        {
            yield return Rapporteur;
            yield return SecondWorker;
            yield return Chair;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
