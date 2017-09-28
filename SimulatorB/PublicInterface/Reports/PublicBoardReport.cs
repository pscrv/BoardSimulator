using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimulatorB.PublicInterface
{
    public class PublicBoardReport
    {
        private ReadOnlyCollection<int> _memberIDs;
        private Dictionary<int, PublicWorkReport> _reports;
        

        internal PublicBoardReport(IEnumerable<Member> members, Dictionary<int, WorkReport> reports)
        {
            _memberIDs = members.Select(x => x.ID).ToList().AsReadOnly();

            _reports = new Dictionary<int, PublicWorkReport>();
            foreach (int memberID in reports.Keys)
            {
                _reports[memberID] = new PublicWorkReport(reports[memberID]);
            }
        }

        public PublicWorkReport Read(int id)
        {
            if (!_reports.ContainsKey(id))
                throw new InvalidOperationException("BoardReport.Read: member has no record.");

            return _reports[id];
        }
    }
    
}