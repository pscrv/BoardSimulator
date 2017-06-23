using System;

namespace Simulator
{

    internal abstract class Work
    {
        #region protected fields
        protected int _counter;
        #endregion

        #region internal properties
        internal int Counter { get { return _counter; } }
        internal bool IsFinished { get { return Counter <= 0; } }
        #endregion

        #region constructors
        protected Work() { }

        internal Work(Case c, int hoursneeded)
        {
            _counter = hoursneeded;
        }
        #endregion

        #region internal methods
        internal void DoWork(Hour hour)
        {
            _counter--;
        }
        #endregion
    }


    internal class SummonsWork : Work

    {
        internal SummonsWork(SummonsCase sc, int hoursneeded)
            : base (sc, hoursneeded) { }
    }


    internal class DecisionWork : Work
    {
        internal DecisionWork(DecisionCase dc, int hoursneeded)
            : base (dc, hoursneeded) { }
    }


    internal class OPWork : Work { }


    internal class NullWork : Work
    {
        internal NullWork()
        {
            _counter = 0;
        }
    }

}