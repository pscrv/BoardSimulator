using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulator
{




    internal abstract class WorkQueue<T> where T: Work
    {
        private Queue<T> _workQueue;
        private Dictionary<T, Hour> _hourEnqueued;

        internal WorkQueue()
        {
            _workQueue = new Queue<T>();
            _hourEnqueued = new Dictionary<T, Hour>();        
        }

        internal bool IsNotEmpty { get { return _workQueue.Count > 0; } }

        internal void Enqueue(T t, Hour h)
        {
            _workQueue.Enqueue(t);
            _hourEnqueued[t] = h;
        }
        
        internal T Dequeue()
        {
            T item = _workQueue.Dequeue();
            _hourEnqueued.Remove(item);
            return item;
        }

        internal Hour AgeOfOldest()
        {
            return _hourEnqueued[_workQueue.Peek()];
        }

    }

    internal class SummonsQueue : WorkQueue<SummonsWork> { }

    internal class DecisionQueue : WorkQueue<DecisionWork> { }




}