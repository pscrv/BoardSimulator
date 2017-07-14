using System.Collections.Generic;

namespace Simulator
{
    internal class OPQueue
    {
        private CaseQueue _opCases = new CaseQueue();
        

        internal  int OPCaseCount { get { return _opCases.Count; } }

        internal  IEnumerable<AllocatedCase> OPCases
        {
            get
            {
                while (_opCases.Count > 0)
                {
                    yield return _opCases.Dequeue();
                }
            }
        }



        internal  void Enqueue(AllocatedCase allocatedCase)
        {
            _opCases.Enqueue(allocatedCase);
        }

        internal  AllocatedCase Dequeue()
        {
            return _opCases.Dequeue();
        }


        internal void Clear()
        {
            while (_opCases.Count > 0)
                _opCases.Dequeue();
        }
    }
}