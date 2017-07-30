using System.Collections.Generic;

namespace Simulator
{
    internal class FinishedCaseList
    {
        #region fields and properties
        private HashSet<AllocatedCase> _list = new HashSet<AllocatedCase>();

        internal int Count { get { return _list.Count; } }
        internal List<AllocatedCase> Cases { get { return _asList(); } }
        #endregion



        internal void Add(AllocatedCase ac)
        {
            _list.Add(ac);
        }



        private List<AllocatedCase> _asList()
        {
            List<AllocatedCase> result = new List<AllocatedCase>();
            foreach (AllocatedCase ac in _list)
            {
                result.Add(ac);
            }
            return result;
        }

    }
}