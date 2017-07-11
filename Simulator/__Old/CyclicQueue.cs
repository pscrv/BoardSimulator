using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldSim
{
    class CyclicQueue<T>
    {
        #region private fields
        private Queue<T> _queue;
        #endregion


        #region internal properties
        internal T Next
        {
            get
            {
                T next = _queue.Dequeue();
                _queue.Enqueue(next);
                return next;
            }
        }
        #endregion


        #region constructors
        internal CyclicQueue(IEnumerable<T> contents)
        {
            _queue = new Queue<T>(contents);
        }
        #endregion



        #region internal methods
        internal IEnumerable<T> Enumerate()
        {
            foreach(T x in _queue) { yield return x; }
        }
        #endregion
    }
}
