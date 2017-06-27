using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulator
{




    internal abstract class WorkQueue<T> where T: Work
    {
        #region private fields
        private Queue<T> _workQueue;
        private Dictionary<T, Hour> _hourEnqueued;
        #endregion


        #region constructorx
        internal WorkQueue()
        {
            _workQueue = new Queue<T>();
            _hourEnqueued = new Dictionary<T, Hour>();        
        }
        #endregion

        #region properties
        internal bool IsNotEmpty { get { return _workQueue.Count > 0; } }

        internal int Count { get { return _workQueue.Count; } }

        internal Hour AgeOfOldest { get { return _hourEnqueued[_workQueue.Peek()]; } }
        #endregion

        #region internal methods
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
        #endregion

    }

    internal class SummonsQueue : WorkQueue<SummonsWork> { }

    internal class DecisionQueue : WorkQueue<DecisionWork> { }




}