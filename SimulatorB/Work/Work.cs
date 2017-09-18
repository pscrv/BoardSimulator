using System;

namespace SimulatorB
{
    internal class Work
    {
        private int _target;

        internal bool IsFinished { get => _target < 1; }

        internal Work(int target)
        {
            _target = target;
        }

        internal void DoWork()
        {
            if (_target < 1)
                throw new InvalidOperationException("Work is already finished");

            _target--;
        }
    }
    
}
