using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardSimulator
{
    class TimeLineAnalyser
    {
        protected static uint __hoursPerDay = Board.__HoursPerDay;

        protected List<BoardWorkReport> _timeline;


        public TimeLineAnalyser(List<BoardWorkReport> timeline)
        {
            _timeline = timeline;
        }


        public int[] SummonsOutDaily { get { return _sumByN(_summonsOut(), Board.__HoursPerDay); } }
        public int[] SummonsOutWeekly { get { return _sumByN(_summonsOut(), Board.__HoursPerDay * Board.__DaysPerWeek); } }
        public int[] SummonsOutYearly { get { return _sumByN(_summonsOut(), Board.__HoursPerDay * Board.__DaysPerWeek * Board.__WeeksPerYear); } }

        public int[] DecisionOutDaily { get { return _sumByN(_decisionOut(), Board.__HoursPerDay); } }
        public int[] DecisionOutWeekly { get { return _sumByN(_decisionOut(), Board.__HoursPerDay * Board.__DaysPerWeek); } }
        public int[] DecisionOutYearly { get { return _sumByN(_decisionOut(), Board.__HoursPerDay * Board.__DaysPerWeek * Board.__WeeksPerYear); } }

        public int[] SummonsQueueSizeDaily { get { return _maxByN(_summonsQueueSize(), Board.__HoursPerDay); } }
        public int[] SummonsQueueSizeWeekly { get { return _maxByN(_summonsQueueSize(), Board.__HoursPerDay * Board.__DaysPerWeek); } }
        public int[] SummonsQueueSizeYearly { get { return _maxByN(_summonsQueueSize(), Board.__HoursPerDay * Board.__DaysPerWeek * Board.__WeeksPerYear); } }

        public int[] SummonsQueueAgeDaily { get { return _maxByN(_summonsQueueAge(), Board.__HoursPerDay); } }
        public int[] SummonsQueueAgeWeekly { get { return _maxByN(_summonsQueueAge(), Board.__HoursPerDay * Board.__DaysPerWeek); } }
        public int[] SummonsQueueAgeYearly { get { return _maxByN(_summonsQueueAge(), Board.__HoursPerDay * Board.__DaysPerWeek * Board.__WeeksPerYear); } }


        #region private data extraction methods
        protected int[] _summonsOut()
        {
            int[] value = new int[_timeline.Count];
            for (int i = 0; i < _timeline.Count; i++)
                if (_timeline[i].ChairReport.SummonsOut)
                    value[i] = 1;
            return value;
        }

        protected int[] _decisionOut()
        {
            int[] value = new int[_timeline.Count];
            for (int i = 0; i < _timeline.Count; i++)
                if (_timeline[i].ChairReport.DecisionOut)
                    value[i] = 1;
            return value;
        }

        protected int[] _summonsQueueSize()
        {
            int[] value = new int[_timeline.Count];
            for (int i = 0; i < _timeline.Count; i++)
                value[i] = (int)_timeline[i].ChairReport.SummonsQueueSize;
            return value;
        }

        protected int[] _summonsQueueAge()
        {
            int[] value = new int[_timeline.Count];
            for (int i = 0; i < _timeline.Count; i++)
                value[i] = (int)_timeline[i].ChairReport.SummonsQueueAge;
            return value;
        }
        #endregion

        #region group-by-N methods
        protected int[] _sumByN(int[] array, uint n)
        {
            if (n <= 0)
                return null;

            if (array.Length <= n)
                return new int[] { array.Sum() };

            int[] value = new int[array.Length / n];

            int index = 0;
            for (int i = 0, j = 0; i < array.Length; i++, j++)
            {
                if (j == n)
                {
                    j = 0;
                    index++;
                }

                value[index] += array[i];
            }

            return value;
        }

        protected int[] _maxByN(int[] array, uint n)
        {
            if (n > array.Length || n <= 0)
                return null;

            int[] value = new int[array.Length / n];

            for (int i = 0; i < value.Length; i++)
                value[i] = int.MinValue;

            int index = 0;
            for (int i = 0, j = 0; i < array.Length; i++, j++)
            {
                if (j == n)
                {
                    j = 0;
                    index++;
                }

                if (value[index] < array[i])
                    value[index] = array[i];
            }

            return value;
        }

        protected int [] _lastOfN(int[] array, uint n)
        {
            if (n > array.Length || n <= 0)
                return null;

            int[] value = new int[array.Length / n];

            int index = 0;
            for (int i = 0, j = 0; i < array.Length; i++, j++)
            {
                if (j == n)
                {
                    j = 0;
                    index++;
                }

                if (j == n - 1)
                    value[index] = array[i];
            }

            return value;
        }

        protected int[] _sumTrueByN(bool[] array, uint n)
        {
            if (n > array.Length || n <= 0)
                return null;

            int[] value = new int[array.Length / __hoursPerDay];
            int index = 0;
            for (int i = 0, j = 0; i < array.Length; i++, j++)
            {
                if (j == n)
                {
                    j = 0;
                    index++;
                }

                if (array[i])
                    value[index] += 1;
            }

            return value;
        }
        #endregion      


        #region helper methods
        protected decimal[] _movingAverage(int[] array, int n)
        {
            int length = array.Length;
            if (length == 0 || n < 1 || n > length)
                return null;

            int newlength = length - n + 1;
            decimal[] value = new decimal[newlength];
            int sum = array.Take(n).Sum();
            for (int i = 0, j = n; i < newlength; i++, j++)
            {
                sum -= array[i];
                sum += array[j];
                value[i] = sum / n;
            }

            return value;
        }
        #endregion
    }




    class ActivityAnalyzer : TimeLineAnalyser
    {
        public enum Activities
        {
            Nothing,
            SummonsWork,
            SummonsOut,
            DecicionWork,
            DecisionOut,
            OPWork,
            Otherwork,
            Unknown
        }

        #region private data fields
        List<Activities[]> _activityTimeLines;
        List<BlockChartData> _activityBlockChartData;
        BlockChartDayData _blockChartDayData;
        #endregion

        #region constructors
        public ActivityAnalyzer(List<BoardWorkReport> timeline)
            : base(timeline)
        {
            _setupActivityData();
            _setupDayData();
        }
        #endregion

        #region public access
        internal List<Activities[]> ActivityTimeLines { get { return _activityTimeLines; } }

        internal List<BlockChartData> ActivityBlockChartData { get { return _activityBlockChartData; } }

        internal BlockChartDayData DayData { get { return _blockChartDayData; } }
        #endregion


        #region private setup methods
        protected void _setupActivityData()
        {
            if (_timeline == null)
            {
                _activityTimeLines = null;
                _activityBlockChartData = null;
                return;
            }

            _activityTimeLines = new List<Activities[]>();
            _activityBlockChartData = new List<BlockChartData>();

            List<Activities> chairActivityList = new List<Activities>();
            List<Activities[]> memberActivityArray = new List<Activities[]>();
            for (int i = 0; i < _timeline.First().TechniclMemberReports.Count; i++)
                memberActivityArray.Add(new Activities[_timeline.Count]);

            int count = 0;
            foreach (BoardWorkReport report in _timeline)
            {
                chairActivityList.Add(_getActivity(report.ChairReport));

                for (int i = 0; i < report.TechniclMemberReports.Count; i++)
                {
                    memberActivityArray[i][count] = _getActivity(report.TechniclMemberReports[i]);
                }

                count++;
            }

            _activityTimeLines.Add(chairActivityList.ToArray());
            _activityBlockChartData.Add(new BlockChartData("Chair", Array.ConvertAll(chairActivityList.ToArray(), value => (int)value)));

            foreach (var array in memberActivityArray)
            {
                _activityTimeLines.Add(array.ToArray());
                _activityBlockChartData.Add(new BlockChartData("Member", Array.ConvertAll(array, value => (int)value)));
            }
        }

        protected Activities _getActivity(BoardMemberReport report)
        {
            if (report.SummonsOut) return Activities.SummonsOut;
            if (report.SummonsWork) return Activities.SummonsWork;
            if (report.DecisionOut) return Activities.DecisionOut;
            if (report.DecisionWork) return Activities.DecicionWork;
            if (report.OPWork) return Activities.OPWork;
            if (report.FreeTime) return Activities.Otherwork;
            return Activities.Unknown;
        }

        protected void _setupDayData()
        {
            int length = _timeline.Count;
            int[] summons = new int[length / __hoursPerDay];
            int[] decisions = new int[length / __hoursPerDay];
            int[] summonsQ = new int[length / __hoursPerDay];
            int[] decisionQ = new int[length / __hoursPerDay];

            bool[] smsArray = new bool[_timeline.Count];
            bool[] decArray = new bool[_timeline.Count];
            int[] sQArray = new int[_timeline.Count];
            int[] dQArray = new int[_timeline.Count];
            for (int i = 0; i < _timeline.Count; i++)
            {
                smsArray[i] = _timeline[i].ChairReport.SummonsOut;
                decArray[i] = _timeline[i].ChairReport.DecisionOut;
                sQArray[i] = (int)_timeline[i].ChairReport.SummonsQueueSize;
                dQArray[i] = (int)_timeline[i].ChairReport.DecisionQueueSize;
            }

            summons = _sumTrueByN(smsArray, __hoursPerDay);
            decisions = _sumTrueByN(decArray, __hoursPerDay);
            summonsQ = _lastOfN(sQArray, __hoursPerDay);
            decisionQ = _lastOfN(dQArray, __hoursPerDay);

            _blockChartDayData = new BlockChartDayData(summons, decisions, summonsQ, decisionQ);
        }
        #endregion





    }
}
