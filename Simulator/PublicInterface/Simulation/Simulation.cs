using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            _board = boardParameters.MakeBoard(registrar);
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
    }
}