using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace BoardSimulator
{
    public abstract class ChartData
    {
        #region axes
        private double _xAxisPosition;
        public double XAxisPosition
        {
            get
            { return _xAxisPosition; }
            set
            {
                if (value < 0) _xAxisPosition = 0;
                else if (value > 1) _xAxisPosition = 1;
                else _xAxisPosition = value;
            }
        }

        private double _yAxisPosition;
        public double YAxisPosition
        {
            get
            { return _yAxisPosition; }
            set
            {
                if (value < 0) _yAxisPosition = 0;
                else if (value > 1) _yAxisPosition = 1;
                else _yAxisPosition = value;
            }
        }

        public bool ShowXAxis { get; set; }
        public bool XAxisTicks { get; set; }
        public string[] XAxisLabels { get; set; }
        public int XAxisLabelInterval { get; set; }
        public Brush XAxisColour { get; set; }
        public int XAxisThickness { get; set; }

        public bool ShowYAxis { get; set; }
        public bool YAxisTicks { get; set; }
        public string[] YAxisLabels { get; set; }
        public int YAxisLabelInterval { get; set; }
        public Brush YAxisColour { get; set; }
        public int YAxisThickness { get; set; }
        #endregion

        #region the chart
        #endregion


        #region other properties
        public string Title { get; set; }
        #endregion

        #region constructors
        public ChartData()
        {
            ShowXAxis = true;
            XAxisPosition = 0;
            XAxisTicks = false;
            XAxisLabels = null;
            XAxisLabelInterval = 1;
            XAxisColour = Brushes.Black;
            XAxisThickness = 1;
            ShowYAxis = true;
            YAxisPosition = 0;
            YAxisTicks = false;
            YAxisLabels = null;
            YAxisLabelInterval = 1;
            YAxisColour = Brushes.Black;
            YAxisThickness = 1;
            Title = "Title";
        }
        #endregion


        #region public methods
        public abstract void MakeDefault();
        #endregion

        #region private methods
        #endregion
    }


    public abstract class IntChartData : ChartData
    {
        public int[] Data { get; set; }

        private int _yMax;
        public int YMax
        {
            get { return _yMax; }
            set
            {
                if (value < _yMin)
                    _yMax = _yMin;
                else
                    _yMax = value;
            }
        }

        private int _yMin;
        public int YMin
        {
            get { return _yMin; }
            set
            {
                if (value > _yMax)
                    _yMin = _yMax;
                else
                    _yMin = value;
            }
        }

        public IntChartData()
            : base()
        {
            Data = null;
            YMin = 0;
            YMax = 100;
        }


        #region public methods
        public void AutoGenerateYLabels(int maxLabels)
        {
            double range = _yMax - _yMin;
            YAxisLabelInterval = (int)Math.Ceiling(range / maxLabels);
            if (YAxisLabelInterval < 1)
                YAxisLabelInterval = 1;


            List<string> labelList = new List<string>();
            for (int y = _yMin; y <= YMax; y += YAxisLabelInterval)
                labelList.Add(y.ToString());
            YAxisLabels = labelList.ToArray();
        }

        public void AutoGenerateYMinMax()
        {
            if (Data == null)
                return;
            if (Data.Length == 0)
                return;

            _yMin = int.MaxValue;
            _yMax = int.MinValue;

            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i] < _yMin)
                    _yMin = Data[i];
                if (Data[i] > _yMax)
                    _yMax = Data[i];
            }

            _yMin -= 1 + (_yMax - _yMin) / 10;

        }

        #endregion



    }
    

    public class IntHistogramChartData : IntChartData
    {

        #region the chart
        private double _barWidth;
        public double BarWidth
        {
            get { return _barWidth; }
            set
            {
                if (value < 0)
                    _barWidth = 0;
                else if (value > 1)
                    _barWidth = 1;
                else
                    _barWidth = value;
            }
        }
        public Brush BarColour { get; set; }
        public Brush BarOutlineColour { get; set; }
        #endregion


        #region constructors
        public IntHistogramChartData()
            : base()
        {
            BarWidth = 1;
            BarColour = Brushes.PaleVioletRed;
            BarOutlineColour = Brushes.Black;
        }
        #endregion


        #region implementations of abstract methods
        public override void MakeDefault()
        {
            Data = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 17, 0 };
            Title = "Default chart";
        }

        #endregion
    }


    public class IntLineChartData : IntChartData
    {
        #region enum
        public enum Position { left, center, right}
        #endregion

        #region the chart
        public Brush LineColour { get; set; }
        public Position HorizontalAlignment { get; set; }
        #endregion


        #region constructors
        public IntLineChartData()
            : base()
        {
            LineColour = Brushes.Blue;
            HorizontalAlignment = Position.center;
        }
        #endregion


        public override void MakeDefault()
        {
            Data = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 17, 0, 3 };
            Title = "Default Chart";
        }
    }





    public class BlockChartData
    {
        #region private data fields
        int[] _data;
        string _label;
        #endregion

        #region constructors
        public BlockChartData(string label, int[] data)
        {
            _data = data;
            _label = label;
        }

        public BlockChartData(int[] data)
            : this("", data)
        { }
        #endregion

        #region public access
        public IList<int> Data { get { return _data; } }
        public string Label { get { return _label; } }
        #endregion
    }

    public class BlockChartDayData
    {
        #region private data fields
        private int _length;
        private string[] _day;
        private string[] _week;
        //private string[] _summonsOut;
        //private string[] _decisionsOut;
        private string[] _summonsQueueSize;
        private string[] _decisionQueueSize;
        #endregion

        #region constructors
        public BlockChartDayData(int[] summons, int[] decision, int[] summonsQueue , int[] decisionQueue)
        {
            _length = Math.Max(summons.Length, 
                Math.Max(decision.Length, 
                Math.Max(summonsQueue.Length, decisionQueue.Length)));

            _day = new string[_length];
            _week = new string[_length];
            //_summonsOut = new string[_length];
            //_decisionsOut = new string[_length];
            _summonsQueueSize = new string[_length];
            _decisionQueueSize = new string[_length];

            //for (int i = 0; i < summons.Length; i++)
            //    _summonsOut[i] = summons[i].ToString();
            //for (int i = 0; i < decision.Length; i++)
            //    _decisionsOut[i] = decision[i].ToString();
            for (int i = 0; i < summonsQueue.Length; i++)
                _summonsQueueSize[i] = summonsQueue[i].ToString();
            for (int i = 0; i < decisionQueue.Length; i++)
                _decisionQueueSize[i] = decisionQueue[i].ToString();

            //for (int i = summons.Length; i < _summonsOut.Length; i++)
            //    _summonsOut[i] = "unkonwn";
            //for (int i = decision.Length; i < _decisionsOut.Length; i++)
            //    _decisionsOut[i] = "unkonwn";
            for (int i = summonsQueue.Length; i < _summonsQueueSize.Length; i++)
                _summonsQueueSize[i] = "unkonwn";
            for (int i = decisionQueue.Length; i < _decisionQueueSize.Length; i++)
                _decisionQueueSize[i] = "unkonwn";

            for (int i = 0; i < _length; i++)
            {
                _day[i] = "Day " + i.ToString();
                _week[i] = "Week " + (i / Board.__DaysPerWeek).ToString();
            }

        }
        #endregion

        #region public access
        public int Length { get { return _length; } }
        public string[] Day { get { return _day; } }
        public string[] Week { get { return _week; } }
        //public string[] SummonsData { get { return _summonsOut; } }
        //public string[] DecisionData { get { return _decisionsOut; } }
        public string[] SummonsQueueData { get { return _summonsQueueSize; } }
        public string[] DecisionQueueData { get { return _decisionQueueSize; } }
        #endregion

    }







}
