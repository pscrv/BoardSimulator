using System;
using System.Collections.Generic;

namespace Simulator
{
    public class BoardReport
    {
        #region fields and properties
        private List<Member> _members;
        private Dictionary<int, WorkReport> _publicReports;
        #endregion



        #region construction
        internal BoardReport(IEnumerable<Member> members)
        {
            _members = new List<Member>();
            foreach (Member m in members)
            {
                _members.Add(m);
            }
            
            _publicReports = new Dictionary<int, WorkReport>();
        }
        #endregion


        internal void Add(Member member, WorkReport report)
        {
            if (!_members.Contains(member))
                throw new InvalidOperationException("BoardReport.Add: member is not registered.");

            if (_publicReports.ContainsKey(member.ID))
                throw new InvalidOperationException("BoardReport.Add: a report has already been recorded for member.");
            
            _publicReports[member.ID] = report;
        }


        internal WorkReport Read(Member member)
        {
            if (!_publicReports.ContainsKey(member.ID))
                throw new InvalidOperationException("BoardReport.Read: member has no record.");
            
            return _publicReports[member.ID];
        }



        #region public interface
        public WorkReport Read(int memberID)
        {
            if (!_publicReports.ContainsKey(memberID))
                throw new InvalidOperationException("BoardReport.Read: member has no record.");

            return _publicReports[memberID];
        }
        #endregion
    }
}