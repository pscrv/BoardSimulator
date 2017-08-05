using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Simulator
{
    public class Simulation
    {
        #region fields and properties
        private SimulationTimeSpan _timeSpan;
        private Board _board;
        private Dictionary<Hour, int> _arrivingCases;
        private HourlyReports _reports;
        #endregion


        #region publlic access
        public ReadOnlyCollection<CompletedCaseReport> FinishedCases
        { get { return _compileCompletedCaseReports(); } }
        

        public HourlyReports Reports { get { return _reports; } }
        #endregion


        #region construction
        public Simulation(
            int lengthInHours, 
            BoardParameters boardParameters, 
            int initialCaseCount,
            Dictionary<int, int> arriving)
        {
            Member chair = new Member(boardParameters.Chair);
            List<Member> technicals = _assembleMemberList(boardParameters.Technicals);
            List<Member> legals = _assembleMemberList(boardParameters.Legals);

            _board = new Board(
                chair,
                boardParameters.ChairType,
                technicals,
                legals
                );

            _timeSpan = new SimulationTimeSpan(new Hour(0), new Hour(lengthInHours - 1));
            _reports = new HourlyReports();
            _assembleInitialCases(initialCaseCount);
            _assembleArrivingCases(arriving);
        }

        public Simulation(
            int lengthInHours,
            BoardParameters boardParameters,
            int initialCaseCount)
            : this (lengthInHours, boardParameters, initialCaseCount, new Dictionary<int, int>())
        { }
        #endregion


        public void Run()
        {
            BoardReport report;

            foreach (Hour hour in _timeSpan)
            {
                if (_arrivingCases.ContainsKey(hour))
                {
                    for (int i = 0; i < _arrivingCases[hour]; i++)
                    {
                        _board.ProcessNewCase(new AppealCase(), hour);
                    }
                }

                report = _board.DoWork(hour);
                _reports.Add(hour, report);
            }
        }



        private ReadOnlyCollection<CompletedCaseReport> _compileCompletedCaseReports()
        {
            List<CompletedCaseReport> result = new List<CompletedCaseReport>();
            foreach (AllocatedCase ac in _board.FinishedCases)
            {
                result.Add(new CompletedCaseReport(ac));
            }
            return new ReadOnlyCollection<CompletedCaseReport>(result);
        }


        private void _assembleArrivingCases(Dictionary<int, int> arriving)
        {
            _arrivingCases = new Dictionary<Hour, int>();
            foreach (int hour in arriving.Keys)
            {
                _arrivingCases[new Hour(hour)] = arriving[hour];
            }
        }

        private void _assembleInitialCases(int initialCaseCount)
        {
            for (int i = 0; i < initialCaseCount; i++)
            {
                _board.ProcessNewCase(
                    new AppealCase(),
                    _timeSpan.Start);
            }
        }

        private static List<Member> _assembleMemberList(List<MemberParameterCollection> parameterList)
        {
            List<Member> memberList = new List<Member>();
            foreach (MemberParameterCollection parameters in parameterList)
            {
                memberList.Add(new Member(parameters));
            }

            return memberList;
        }
    }
}