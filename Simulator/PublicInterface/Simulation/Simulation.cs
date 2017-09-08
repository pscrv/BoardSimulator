using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Simulator
{
    public class Simulation
    {
        #region static  
        private static Dictionary<Hour, int> __scheduleArrivals(Dictionary<int, int> arriving)
        {
            Dictionary<Hour, int> arrivingCases = new Dictionary<Hour, int>();
            foreach (int hour in arriving.Keys)
            {
                arrivingCases[new Hour(hour)] = arriving[hour];
            }

            return arrivingCases;
        }

        private static Dictionary<Hour, int> __scheduleArrivals(int arrivalsPerMonth, int lengthInHours)
        {
            Dictionary<Hour, int> schedule = new Dictionary<Hour, int>();
            SimulationTimeSpan timespan = new SimulationTimeSpan(new Hour(0), new Hour(lengthInHours - 1));

            Hour hour = new Hour(0);
            while (hour <= timespan.End)
            {
                schedule[hour] = arrivalsPerMonth;
                hour = hour.FirstHourOfNextMonth();
            }

            return schedule;
        }
#endregion


        #region fields and properties
        private SimulationTimeSpan _timeSpan;
        private Board _board;
        private Dictionary<Hour, int> _arrivingCases;
        private HourlyReports _reports;
        #endregion


        #region public access
        public static Simulation MakeSimulation(
            int years,
            BoardParameters boardParameters,
            int minimumDaysBetweenOP,
            int initialCaseCount,
            int arrivalsPerMonth = 0)
        {
            return new Simulation(
                years * TimeParameters.HoursPerYear,
                boardParameters,
                minimumDaysBetweenOP,
                initialCaseCount,
                arrivalsPerMonth);
        }


        public SimulationReport SimulationReport
        {
            get
            {
                return new SimulationReport(
                    _compileCompletedCaseReports(),
                    _reports);
            }
        }
        #endregion


        #region construction
        internal Simulation(
            int lengthInHours, 
            BoardParameters boardParameters, 
            int minimumDaysBetweenOP,
            int initialCaseCount,
            Dictionary<Hour, int> arriving)
        {
            OPSchedule opSchedule = new SimpleOPScheduler(minimumDaysBetweenOP);
            Registrar registrar = new Registrar(opSchedule);
            _makeBoard(boardParameters, registrar);

            _timeSpan = new SimulationTimeSpan(new Hour(0), new Hour(lengthInHours - 1));
            _reports = new HourlyReports();
            _arrivingCases = arriving;

            _assembleInitialCases(initialCaseCount);
        }

        internal Simulation(
            int lengthInHours,
            BoardParameters boardParameters,
            int minimumDaysBetweenOP,
            int initialCaseCount,
            Dictionary<int, int> arriving)
            : this (
                  lengthInHours,
                  boardParameters,
                  minimumDaysBetweenOP,
                  initialCaseCount,
                  __scheduleArrivals(arriving))
        { }

        public Simulation(
            int lengthInHours,
            BoardParameters boardParameters,
            int minimumDaysBetweenOP,
            int initialCaseCount)
            : this (
                  lengthInHours, 
                  boardParameters,
                  minimumDaysBetweenOP,
                  initialCaseCount, 
                  new Dictionary<Hour, int>())
        { }

        public Simulation(
            int lengthInHours,
            BoardParameters boardParameters,
            int initialCaseCount)
            : this(
                  lengthInHours,
                  boardParameters,
                  0,
                  initialCaseCount,
                  new Dictionary<Hour, int>())
        { }


        public Simulation(
            int lengthInHours,
            BoardParameters boardParameters,
            int minimumDaysBetweenOP,
            int initialCaseCount,
            int arrivalsPerMonth)
            : this (
                  lengthInHours, 
                  boardParameters,
                  minimumDaysBetweenOP,
                  initialCaseCount, 
                  __scheduleArrivals(arrivalsPerMonth, lengthInHours))
        { }




        private void _makeBoard(BoardParameters boardParameters, Registrar registrar)
        {
            Member chair = new Member(boardParameters.Chair);
            List<Tuple<Member, int>> technicals = _assembleMemberList(boardParameters.Technicals);
            List<Tuple<Member, int>> legals = _assembleMemberList(boardParameters.Legals);
            ChairChooser chairChooser = _makeChairChooser(chair, technicals, legals);

            // TODO: refactor BoardParameters to get rid of this switch
            switch (boardParameters.ChairType)
            {
                case ChairType.Technical:
                    _board = Board.MakeTechnicalBoard(
                        chair,
                        technicals.Select(x => x.Item1).ToList(),
                        legals.Select(x => x.Item1).ToList(),
                        registrar,
                        chairChooser
                        );
                    break;
                case ChairType.Legal:
                    _board = Board.MakeLegalBoard(
                        chair,
                        technicals.Select(x => x.Item1).ToList(),
                        legals.Select(x => x.Item1).ToList(),
                        registrar,
                        chairChooser
                        );
                    break;
            }
        }


        private ChairChooser _makeChairChooser(
            Member chair, 
            IEnumerable<Tuple<Member, int>> technicals,
            IEnumerable<Tuple<Member, int>> legals)
        {
            ChairChooser chooser = new ChairChooser(chair);
            foreach (var technical in technicals)
            {
                if (technical.Item2 > 0)
                    chooser.AddSecondaryChair(technical.Item1, technical.Item2);
            }
            foreach (var legal in legals)
            {
                if (legal.Item2 > 0)
                    chooser.AddSecondaryChair(legal.Item1, legal.Item2);
            }

            return chooser;
        }

        #endregion


        public void Run()
        {
            foreach (Hour hour in _timeSpan)
            {
                if (_arrivingCases.ContainsKey(hour))
                {
                    for (int i = 0; i < _arrivingCases[hour]; i++)
                    {
                        _board.ProcessNewCase(new AppealCase(), hour);
                    }
                }

                BoardReport report = _board.DoWork(hour);
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

        private void _assembleInitialCases(int initialCaseCount)
        {
            for (int i = 0; i < initialCaseCount; i++)
            {
                _board.ProcessNewCase(
                    new AppealCase(),
                    _timeSpan.Start);
            }
        }




        private static List<Member> _assembleMemberList(IEnumerable<MemberParameterCollection> parameterList)
        {
            List<Member> memberList = new List<Member>();
            foreach (MemberParameterCollection parameters in parameterList)
            {
                memberList.Add(new Member(parameters));
            }

            return memberList;
        }

        private static List<Tuple<Member, int>> _assembleMemberList(IEnumerable<Tuple<MemberParameterCollection, int>> parameterList)
        {
            List<Tuple<Member, int>> memberList = new List<Tuple<Member, int>>();
            foreach (Tuple<MemberParameterCollection, int> parameters in parameterList)
            {
                memberList.Add(new Tuple<Member, int>(new Member(parameters.Item1), parameters.Item2));
            }

            return memberList;
        }
    }
}