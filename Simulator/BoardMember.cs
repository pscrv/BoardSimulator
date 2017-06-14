using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public abstract class BoardMember
    {
        private MemberWorkParameters _workParameters;

        private WorkQueue _workQueue;
        private Case _currentCase;


        internal MemberState DoOneHourOfWork(WorkQueue outputQueue)
        {
            if (_currentCase == null)
                _currentCase = _workQueue.NextWorkItem;
            _currentCase.DoWork();
            if (_currentCase.StageFinshed(_workParameters))
                outputQueue.AddWorkItem(_currentCase);
        }
    }



    public class Chair : BoardMember { }

    public class TechnicalMember : BoardMember { }

    public class LegalMember : BoardMember { }
}
