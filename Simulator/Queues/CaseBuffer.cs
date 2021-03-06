﻿using System.Collections.Generic;

namespace Simulator
{
    internal class CaseBuffer
    {
        private CaseQueue _queue = new CaseQueue();


        internal int Count { get { return _queue.Count; } }


        internal IEnumerable<AllocatedCase> Enumeration
        {
            get
            {
                while (_queue.Count > 0)
                {
                    yield return _queue.Dequeue();
                }
            }
        }



        internal void Enqueue(Hour currentHour, AllocatedCase allocatedCase)
        {
            _queue.Enqueue(currentHour, allocatedCase);
        }
        

        internal AllocatedCase Dequeue()
        {
            return _queue.Dequeue();
        }
        
        internal void EnqueueForNextStage(Hour hour)
        {
            foreach (AllocatedCase ac in Enumeration)
            {
                ac.EnqueueForWork(hour);
            }
        }

        internal void Clear()
        {
            while (_queue.Count > 0)
                _queue.Dequeue();
        }
    }


    internal class CirculationQueue : CaseBuffer { }



    internal class OPQueue : CaseBuffer { }


    
    internal class IncomingCaseQueue : CaseBuffer { }
}