using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimulatorB.PublicInterface
{
    public class PublicCasesReportList : ReadOnlyCollection<PublicCaseReport>
    {
        public PublicCasesReportList(IList<PublicCaseReport> list) 
            : base(list)
        { }
    }
}
