using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class BoardReport
    {
        #region fields and properties
        private List<Member> _members;
        private Dictionary<Member, WorkReport> _reports;
        #endregion



        #region construction
        internal BoardReport(IEnumerable<Member> members)
        {
            _members = new List<Member>();
            foreach (Member m in members)
            {
                _members.Add(m);
            }

            _reports = new Dictionary<Member, WorkReport>();
        }
        #endregion


        internal void Add(Member member, WorkReport report)
        {
            if (!_members.Contains(member))
                throw new InvalidOperationException("BoardReport.Add: member is not registered.");

            if (_reports.ContainsKey(member))
                throw new InvalidOperationException("BoardReport.Add: a report has already been recorded for member.");

            _reports[member] = report;
        }


        internal WorkReport Read(Member member)
        {
            return _reports[member];
        }
    }
}