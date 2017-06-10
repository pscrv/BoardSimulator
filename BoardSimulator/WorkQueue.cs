using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardSimulator
{
    class WorkQueue<T> : Queue where T : Work
    {
        #region private data fields
        protected Queue<uint> _loggedAtHour;
        #endregion

        #region constructors
        public WorkQueue()
            : base()
        {
            _loggedAtHour = new Queue<uint>();
        }
        #endregion

        #region public methods
        public uint AgeAtHour(uint hour)
        {
            if (_loggedAtHour.Count == 0)
                return 0;

            return hour - _loggedAtHour.Peek();
        }
        #endregion

        #region new, overriding, and extension methods
        public override void Clear()
        {
            base.Clear();
            _loggedAtHour.Clear();
        }

        public void Enqueue(T t)
        {
            base.Enqueue(t);
            _loggedAtHour.Enqueue(0);
        }

        public void Enqueue(T t, uint hour)
        {
            base.Enqueue(t);
            _loggedAtHour.Enqueue(hour);
        }        

        public new T Dequeue()
        {
            _loggedAtHour.Dequeue();
            return base.Dequeue() as T;
        }
        #endregion
    }

    class SummonsQueue : WorkQueue<Summons>
    {

    }

    class DecisionQueue : WorkQueue<Decision>
    {
        
    }


}
