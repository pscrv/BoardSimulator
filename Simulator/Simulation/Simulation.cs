using System.Collections.Generic;

namespace Simulator
{
    internal class Simulation
    {
        #region fields and properties
        private SimulationTimeSpan _timeSpan;
        private SimulationLog _log;
        private Board _board;
        private Dictionary<Hour, List<AppealCase>> _arrivingCases;
        private HourlyReports _reports;   
        #endregion


        #region construction
        internal Simulation(
            int lengthInHours, 
            BoardParameters boardParameters, 
            IEnumerable<AppealCase> initialCases, 
            Dictionary<Hour, List<AppealCase>> arriving)
        {
            _timeSpan = new SimulationTimeSpan(new Hour(0), new Hour(lengthInHours - 1));
            _log = new SimulationLog();
            _reports = new HourlyReports();
            _arrivingCases = arriving;

            Member chair = new Member(boardParameters.Chair);

            List<Member> technicals = new List<Member>();
            foreach (MemberParameterCollection parameters in boardParameters.Technicals)
            {
                technicals.Add( new Member(parameters));
            }

            List<Member> legals = new List<Member>();
            foreach (MemberParameterCollection  parameters in boardParameters.Legals)
            {
                legals.Add(new Member(parameters));
            }            

            _board = new Board(
                chair, 
                boardParameters.ChairType, 
                technicals, 
                legals               
                );

            foreach (AppealCase ac in initialCases)
            {
                _board.ProcessNewCase(ac, _timeSpan.Start);
            }
        }

        internal Simulation(
            int lengthInHours,
            BoardParameters boardParameters,
            IEnumerable<AppealCase> initialCases)
            : this (lengthInHours, boardParameters, initialCases, new Dictionary<Hour, List<AppealCase>>())
        { }
        #endregion


        internal void Run()
        {
            BoardReport report;

            foreach (Hour hour in _timeSpan)
            {
                if (_arrivingCases.ContainsKey(hour))
                {
                    _board.ProcessNewCaseList(_arrivingCases[hour], hour);
                }

                report = _board.DoWork(hour);
                _reports.Add(hour, report);
            }
        }        
    }
}