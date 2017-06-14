using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class WorkQueue
    {
        private Queue<Case> _summonsQueue;
        private Queue<Case> _decisionQueue;
        private Queue<Case> _opQueue;

        public Case NextWorkItem
        {
            get
            {
                if (_opWorkDue())
                    return _opQueue.Dequeue();
                if (_decisionWorkDue())
                    return _decisionQueue.Dequeue();
                if (_summonsWorkDue())
                    return _summonsQueue.Dequeue();
                return null;
            }
        }

        private bool _opWorkDue()
        {
            return _opQueue.Count > 0;
        }

        private bool _decisionWorkDue()
        {
            return _decisionQueue.Count > 0;
        }

        private bool _summonsWorkDue()
        {
            return _summonsQueue.Count > 0;
        }
        


    }

    internal class InitialWorkQueue : WorkQueue { }
}