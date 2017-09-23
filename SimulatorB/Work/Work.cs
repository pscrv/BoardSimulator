using System;

namespace SimulatorB
{
    internal class Work
    {
        private int _target;

        internal bool IsStarted { get; private set; }
        internal bool IsFinished { get => _target < 1; }

        internal Work(int target)
        {
            IsStarted = false;
            _target = target;
        }

        internal void DoWork()
        {
            if (_target < 1)
                throw new InvalidOperationException("Work is already finished");

            IsStarted = true;
            _target--;
        }
    }
    
}
