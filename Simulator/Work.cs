using System;

namespace Simulator
{

    internal abstract class Work
    {
        protected int _counter;

        internal int Counter { get { return _counter; } }
        internal bool IsFinished { get { return Counter <= 0; } }

        internal Work()
        { }

        internal void SetCounter(int hoursForsummons)
        {
            _counter = hoursForsummons;
        }

        internal void DoWork(Hour hour)
        {
            _counter--;
        }
    }

    internal class SummonsWork : Work { }

    internal class DecisionWork : Work { }

    internal class OPWork : Work { }

    internal class Nullwork : Work
    {
        internal Nullwork()
        {
            _counter = 0;
        }
    }

}