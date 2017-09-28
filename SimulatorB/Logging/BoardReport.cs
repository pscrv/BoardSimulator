using System;
using System.Collections.Generic;
using SimulatorB.PublicInterface;

namespace SimulatorB
{
    internal class BoardReport
    {
        #region fields and properties
        private List<Member> _members;
        private Dictionary<int, WorkReport> _reports;
        #endregion



        #region construction
        internal BoardReport(IEnumerable<Member> members)
        {
            _members = new List<Member>();
            foreach (Member m in members)
            {
                _members.Add(m);
            }

            _reports = new Dictionary<int, WorkReport>();
        }
        #endregion


        internal void Add(Member member, WorkReport report)
        {
            if (!_members.Contains(member))
                throw new InvalidOperationException("BoardReport.Add: member is not registered.");

            if (_reports.ContainsKey(member.ID))
                throw new InvalidOperationException("BoardReport.Add: a report has already been recorded for member.");

            _reports[member.ID] = report;
        }


        internal WorkReport Read(Member member)
        {
            if (!_reports.ContainsKey(member.ID))
                throw new InvalidOperationException("BoardReport.Read: member has no record.");

            return _reports[member.ID];
        }        


        internal PublicBoardReport AsPublicBoardReport()
        {
            return new PublicBoardReport(_members, _reports);
        }
    }
}
