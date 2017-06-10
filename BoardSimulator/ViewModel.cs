using BoardSimulator.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BoardSimulator
{
    class ViewModel : ObservableObject
    { }

    #region reports
    class BoardWorkReport_VM : ViewModel
    {
        private BoardWorkReport _report;
        private ChairReport_VM _chairReportVM;
        private List<TechnichMemberReport_VM> _technicalMemberReportsVM;
        public BoardWorkReport_VM(BoardWorkReport report)
        {
            _report = report;
            _chairReportVM = new ChairReport_VM(_report.ChairReport);
            _technicalMemberReportsVM = new List<TechnichMemberReport_VM>();
            foreach (TechniclMemberReport r in _report.TechniclMemberReports)
                _technicalMemberReportsVM.Add(new TechnichMemberReport_VM(r));
        }


        public uint Hour { get { return _report.Hour; } }
        public uint Day { get { return _report.Day; } }
        public uint Week { get { return _report.Week; } }
        public uint OPScheduledForDay { get { return _report.OPScheduledForDay; } }
        public ChairReport_VM ChairReportVM { get { return _chairReportVM; } }
        public List<ChairReport_VM> ChairReportListVM
        {
            get
            {
                List<ChairReport_VM> list = new List<ChairReport_VM>();
                list.Add(_chairReportVM);
                return list;
            }
        }
        public List<TechnichMemberReport_VM> MemberReportsVM { get { return _technicalMemberReportsVM; } }

    }

    class BoardMemberReport_VM : ViewModel
    {
        protected BoardMemberReport _report;
        public BoardMemberReport_VM(BoardMemberReport report) { _report = report; }

        public bool DecisionOut { get { return _report.DecisionOut; } }
        public bool SummonsOut { get { return _report.SummonsOut; } }

        public bool DecisionWork { get { return _report.DecisionWork; } }
        public bool SummonsWork { get { return _report.SummonsWork; } }
        public bool OPWork { get { return _report.OPWork; } }
        public bool OtherWork { get { return _report.FreeTime; } }
    }

    class ChairReport_VM : BoardMemberReport_VM
    {
        private new ChairReport _report;
        public ChairReport_VM(ChairReport report)
            : base(report)
        {
            _report = report;
        }

        public uint SummonsQueueSize { get { return _report.SummonsQueueSize; } }
        public uint SummonsQueueAge { get { return _report.SummonsQueueAge; } }
        public uint DecisionQueueSize { get { return _report.DecisionQueueSize; } }
        public uint DecisionQueueAge { get { return _report.DecisionQueueAge; } }
        
        public int SummonsQS { get { return (int)SummonsQueueSize; } }
    }

    class TechnichMemberReport_VM : BoardMemberReport_VM
    {
        public TechnichMemberReport_VM(TechniclMemberReport report)
            : base(report)
        { }
    }
    #endregion

    #region timelines
    class ReportLogList_VM : List<BoardWorkReport_VM>
    {

        public ReportLogList_VM(List<BoardWorkReport> list)
        {
            foreach (BoardWorkReport r in list)
                this.Add(new BoardWorkReport_VM(r));
        }

        public List<BoardWorkReport_VM> BoardReportList { get { return this; } }
    }

    class ActivityTimeLines_VM : ViewModel
    {
        private ActivityAnalyzer _analyser;

        public List<ActivityAnalyzer.Activities[]> ActivityTimeLines { get; private set; }
        public List<BlockChartData> ActivityData { get; private set; }
        public BlockChartDayData DayData { get; private set; }

        public ActivityTimeLines_VM(Board board)
        {
            if (board.ReportLogList.Count == 0)
                board.RunSimulation();

            _analyser = new ActivityAnalyzer(board.ReportLogList);

            ActivityTimeLines = _analyser.ActivityTimeLines;
            ActivityData = _analyser.ActivityBlockChartData;
            DayData = _analyser.DayData;
        }       

    }
    #endregion


    #region board 
    class BoardParameters_VM : ViewModel
    {
        protected Board _board;

        public uint MemberSummonsHours
        {
            get { return _board.MemberSummonsHours; }
            set {
                if (_board.MemberSummonsHours != value)
                {
                    _board.MemberSummonsHours = value;
                    OnPropertyChanged("MemberSummonsHours");
                }
            }
        }

        public uint MemberDecisionHours
        {
            get { return _board.MemberDecisionHours; }
            set
            {
                if (_board.MemberDecisionHours != value)
                {
                    _board.MemberDecisionHours = value;
                    OnPropertyChanged("MemberDecisionHours");
                }
            }
        }

        public uint MemberOPPreparationHours
        {
            get { return _board.MemberOPPreparationHours ; }
            set
            {
                if (_board.MemberOPPreparationHours != value)
                {
                    _board.MemberOPPreparationHours = value;
                    OnPropertyChanged("MemberOPPreparationHours");
                }
            }
        }

        public uint ChairSummonsHours
        {
            get { return _board.ChairSummonsHours; }
            set
            {
                if (_board.ChairSummonsHours != value)
                {
                    _board.ChairSummonsHours = value;
                    OnPropertyChanged("ChairSummonsHours");
                }
            }
        }

        public uint ChairDecisionHours
        {
            get { return _board.ChairDecisionHours; }
            set
            {
                if (_board.ChairDecisionHours != value)
                {
                    _board.ChairDecisionHours = value;
                    OnPropertyChanged("ChairDecisionHours");
                }
            }
        }

        public uint ChairOPPreparationHours
        {
            get { return _board.ChairOPPreparationHours; }
            set
            {
                if (_board.ChairOPPreparationHours != value)
                {
                    _board.ChairOPPreparationHours = value;
                    OnPropertyChanged("ChairOPPreparationHours");
                }
            }
        }

        public uint OPDuration
        {
            get { return _board.OPDuration; }
            set
            {
                if (_board.OPDuration != value)
                {
                    _board.OPDuration = value;
                    OnPropertyChanged("OPDuration");
                }
            }
        }

        public uint DaysBetweenOPs
        {
            get { return _board.MinDaysBetweenOPs; }
            set
            {
                if (_board.MinDaysBetweenOPs != value)
                {
                    _board.MinDaysBetweenOPs = value;
                    OnPropertyChanged("MinDaysBetweenOPs");
                }
            }
        }

        public uint NumberOfMembers
        {
            get { return _board.NumberOfMembers; }
            set
            {
                if (_board.NumberOfMembers != value)
                {
                    _board.NumberOfMembers = value;
                    OnPropertyChanged("NumberOfMembers");
                }
            }
        }

        #region constructors
        public BoardParameters_VM(Board b)
        {
            _board = b;
        }
        #endregion
    }


    class BoardOutput_VM
    {
        protected Board _board;
        protected TimeLineAnalyser _analyzer;

        #region constructors
        public BoardOutput_VM(Board b)
        {
            _board = b;
            _analyzer = new TimeLineAnalyser(_board.ReportLogList);
        }

        public int[] SummonsOut_Yearly { get { return _analyzer.SummonsOutYearly; } }

        public int SummonsOut_Year1 { get { return _analyzer.SummonsOutYearly[0]; } }
        public int SummonsOut_Year2 { get { return _analyzer.SummonsOutYearly[1]; } }
        public int SummonsOut_Year3 { get { return _analyzer.SummonsOutYearly[2]; } }
        public int SummonsOut_Year4 { get { return _analyzer.SummonsOutYearly[3]; } }


        public int[] DecisionsOut_Yearly { get { return _analyzer.DecisionOutYearly; } }

        public int DecisionsOut_Year1 { get { return _analyzer.DecisionOutYearly[0]; } }
        public int DecisionsOut_Year2 { get { return _analyzer.DecisionOutYearly[1]; } }
        public int DecisionsOut_Year3 { get { return _analyzer.DecisionOutYearly[2]; } }
        public int DecisionsOut_Year4 { get { return _analyzer.DecisionOutYearly[3]; } }

        #endregion

    }

    class BoardOutputGraphs_VM
    {
        static protected Brush __summonsFill = Brushes.LightBlue;
        static protected Brush __summonsBorder = Brushes.MediumBlue;
        static protected Brush __decisionFill = Brushes.LightPink;
        static protected Brush __decisionBorder = Brushes.Magenta;
        static protected Brush __opFill = Brushes.Yellow;
        static protected Brush __opBorder = Brushes.Yellow;
        static protected Brush __otherWorkFill = Brushes.PaleGreen;
        static protected Brush __otherWorkBorder = Brushes.PaleGreen;
        static protected Brush __summonsQueueLine = Brushes.BlueViolet;
        static protected Brush __decisionQueueLine = Brushes.Crimson;


        protected Board _board;
        protected TimeLineAnalyser _analyzer;

        protected IntHistogramChartData _summonsOut_Yearly_ChartData;
        protected IntHistogramChartData _decisionOut_Yearly_ChartData;
        protected IntLineChartData _summonsQueueSize_Yearly_ChartData;
        protected IntLineChartData _summonsQueueAge_Yearly_ChartData;


        #region constructors
        public BoardOutputGraphs_VM(Board b)
        {
            _board = b;
            _analyzer = new TimeLineAnalyser(_board.ReportLogList);

            _summonsOut_Yearly_ChartData = new IntHistogramChartData();
            _summonsOut_Yearly_ChartData.Data = _analyzer.SummonsOutYearly;
            _summonsOut_Yearly_ChartData.BarWidth = 0.8;
            _summonsOut_Yearly_ChartData.BarColour = __summonsFill;
            _summonsOut_Yearly_ChartData.BarOutlineColour = __summonsBorder;
            _summonsOut_Yearly_ChartData.XAxisLabels = new string[_summonsOut_Yearly_ChartData.Data.Length];
            for (int i = 0; i < _summonsOut_Yearly_ChartData.Data.Length; i++)
                _summonsOut_Yearly_ChartData.XAxisLabels[i] = "Year " + (i + 1).ToString();
            _summonsOut_Yearly_ChartData.YAxisLabelInterval = 5;
            _summonsOut_Yearly_ChartData.AutoGenerateYMinMax();
            _summonsOut_Yearly_ChartData.AutoGenerateYLabels(5);
            
            _decisionOut_Yearly_ChartData = new IntHistogramChartData();
            _decisionOut_Yearly_ChartData.Data = _analyzer.DecisionOutYearly;
            _decisionOut_Yearly_ChartData.BarWidth = 0.8;
            _decisionOut_Yearly_ChartData.BarColour = __decisionFill;
            _decisionOut_Yearly_ChartData.BarOutlineColour = __decisionBorder;
            _decisionOut_Yearly_ChartData.XAxisLabels = new string[_decisionOut_Yearly_ChartData.Data.Length];
            for (int i = 0; i < _decisionOut_Yearly_ChartData.Data.Length; i++)
                _decisionOut_Yearly_ChartData.XAxisLabels[i] = "Year " + (i + 1).ToString();
            _decisionOut_Yearly_ChartData.YAxisLabelInterval = 5;
            _decisionOut_Yearly_ChartData.AutoGenerateYMinMax();
            _decisionOut_Yearly_ChartData.AutoGenerateYLabels(5);

            _summonsQueueSize_Yearly_ChartData = new IntLineChartData();
            _summonsQueueSize_Yearly_ChartData.Data = _analyzer.SummonsQueueSizeYearly;
            _summonsQueueSize_Yearly_ChartData.LineColour = Brushes.Orange;
            _summonsQueueSize_Yearly_ChartData.XAxisLabels = new string[_summonsQueueSize_Yearly_ChartData.Data.Length];
            for (int i = 0; i < _summonsQueueSize_Yearly_ChartData.Data.Length; i++)
                _summonsQueueSize_Yearly_ChartData.XAxisLabels[i] = "Year " + (i + 1).ToString();
            _summonsQueueSize_Yearly_ChartData.YAxisLabelInterval = 5;
            _summonsQueueSize_Yearly_ChartData.AutoGenerateYMinMax();
            _summonsQueueSize_Yearly_ChartData.AutoGenerateYLabels(5);

            _summonsQueueAge_Yearly_ChartData = new IntLineChartData();
            _summonsQueueAge_Yearly_ChartData.Data = _analyzer.SummonsQueueAgeYearly;
            _summonsQueueAge_Yearly_ChartData.LineColour = Brushes.Orange;
            _summonsQueueAge_Yearly_ChartData.XAxisLabels = new string[_summonsQueueAge_Yearly_ChartData.Data.Length];
            for (int i = 0; i < _summonsQueueAge_Yearly_ChartData.Data.Length; i++)
                _summonsQueueAge_Yearly_ChartData.XAxisLabels[i] = "Year " + (i + 1).ToString();
            _summonsQueueAge_Yearly_ChartData.YAxisLabelInterval = 5;
            _summonsQueueAge_Yearly_ChartData.AutoGenerateYMinMax();
            _summonsQueueAge_Yearly_ChartData.AutoGenerateYLabels(5);
        }
        #endregion


        public IntHistogramChartData SummonsOut_Yearly_ChartData
        { get { return _summonsOut_Yearly_ChartData; } }

        public IntHistogramChartData DecisionOut_Yearly_ChartData
        { get { return _decisionOut_Yearly_ChartData; } }

        public IntLineChartData SummonsQueueSize_Yearly_ChartData
        { get { return _summonsQueueSize_Yearly_ChartData; } }

        public IntLineChartData SummonsQueueAge_Yearly_ChartData
        { get { return _summonsQueueAge_Yearly_ChartData; } }

    }


    class BoardOutputGraphTest_VM
    {

    }

    class Board_VM : ViewModel
    {
        private Board _board;

        public BoardParameters_VM Parameters_VM { get; private set; }
        public Information_VM Information_VM { get; private set; }
        public BoardOutputGraphs_VM BoardOutputGraphs_VM { get; private set; }
        public ActivityTimeLines_VM ActivityTimeLines_VM { get; private set; }


        #region commands
        public ICommand Run_Command
        { get { return new DelegateCommand(_runSimulation); } }

        public ICommand ShowActivities_Command
        { get { return new DelegateCommand(_showActivities); } }



        private void _runSimulation()
        {
            _board.Remake();
            _board.RunSimulation();
            Information_VM = new Information_VM();
            ActivityTimeLines_VM = new ActivityTimeLines_VM(_board);
            BoardOutputGraphs_VM = new BoardOutputGraphs_VM(_board);
            OnPropertyChanged("ActivityTimeLines_VM");
            OnPropertyChanged("BoardOutput_VM");
            OnPropertyChanged("BoardOutputGraphs_VM");
        }

        private void _showActivities()
        {
            Window w = new ActivityTimeLineWindow();
            w.Width = 1050;
            w.DataContext = this;
            w.Show();
        }
        #endregion


        #region constructors
        public Board_VM(Board b)
        {
            _board = b;
            Parameters_VM = new BoardParameters_VM(_board);
            _runSimulation();
        }
        #endregion
    }


    class Information_VM
    {
        public string Info { get { return Information.__Info; } }
    }
    #endregion



}
