using System.Collections.Generic;

namespace Simulator
{
    internal class CirculationQueue
    {
        private CaseQueue _circulatingCases = new CaseQueue();


        internal int CirculatingCaseCount { get { return _circulatingCases.Count; } }

        internal  IEnumerable<AllocatedCase> CirculatingCases
        {
            get
            {
                while (_circulatingCases.Count > 0)
                {
                    yield return _circulatingCases.Dequeue();
                }
            }
        }



        internal  void Enqueue(AllocatedCase allocatedCase)
        {
            _circulatingCases.Enqueue(allocatedCase);
        }


        internal  void Enqueue()
        {
            foreach (AllocatedCase ac in CirculatingCases)
            {
                ac.EnqueueForWork();
            }
        }



        internal  AllocatedCase Dequeue()
        {
            return _circulatingCases.Dequeue();
        }


        internal  void Clear()
        {
            while (_circulatingCases.Count > 0)
                _circulatingCases.Dequeue();
        }

    }
}