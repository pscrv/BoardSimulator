using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BoardSimulator
{
    abstract class BlockChartBase : Canvas
    {
        #region dependency properties
        // --- Padding --- //
        // sets a padding area around the chart
        #region Padding
        public int Padding
        {
            get { return (int)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register("Padding", typeof(int), typeof(BlockChartBase), new PropertyMetadata(0));
        #endregion


        // --- ChartData --- //
        #region ChartData
        public IList<int> ChartData
        {
            get { return (IList<int>)GetValue(ChartDataProperty); }
            set { SetValue(ChartDataProperty, value); }
        }        
        private static void OnChartDataPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            BlockChartBase ch = sender as BlockChartBase;
            ch.CoerceValue(ChartDataProperty);
            ch.CoerceValue(ChartDataSourceProperty);
            ch._reDraw();
        }
        private static object CoerceChartDataProperty(DependencyObject sender, object value)
        {
            if ((sender as BlockChartBase).ChartDataSource == null)
                return value;
            return null;
        }
        public static readonly DependencyProperty ChartDataProperty =
            DependencyProperty.Register(
                "ChartData", 
                typeof(IList<int>), 
                typeof(BlockChartBase),
                new FrameworkPropertyMetadata(
                    new int[0],
                    new PropertyChangedCallback(OnChartDataPropertyChanged),
                    new CoerceValueCallback(CoerceChartDataProperty)));
        #endregion

        #region ChartDataSource
        public IList<IList<int>> ChartDataSource
        {
            get { return (IList<IList<int>>)GetValue(ChartDataSourceProperty); }
            set { SetValue(ChartDataSourceProperty, value); }
        }
        private static void OnChartDataSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            BlockChartBase chart = (sender as BlockChartBase);
            if (chart.ChartDataSource == null)
            {
                List<IList<int>> list = new List<IList<int>>();
                list.Add(chart.ChartData);
            }
            chart._reDraw();
        }
        private static object CoerceChartDataSourceProperty(DependencyObject sender, object value)
        {
            BlockChartBase chart = sender as BlockChartBase;
            if (ChartDataSourceProperty == null)
            {
                IList<IList<int>> list = new List<IList<int>>();
                list.Add(chart.ChartData);
                return list;
            }

            return value;
        }
        public static readonly DependencyProperty ChartDataSourceProperty =
            DependencyProperty.Register(
                "ChartDataSource", 
                typeof(IList<IList<int>>), 
                typeof(BlockChartBase), 
                new PropertyMetadata(
                    null,
                    new PropertyChangedCallback(OnChartDataSourcePropertyChanged)
                    //,
                    //new CoerceValueCallback(CoerceChartDataSourceProperty)
                    ));

        #endregion
        #endregion


        #region abstract methods
        abstract protected void _setParameters();
        abstract protected void _drawChart();
        #endregion

        #region private draw methods
        protected void _reDraw()
        {
            this.Children.Clear();
            _setParameters();
            _drawChart();
        }
        #endregion

    }

    class BlockChart : BlockChartBase
    {
        #region static members  - brushes
        static Brush[] __colourArray =
            {
            Brushes.Transparent,
            Brushes.LightBlue,
            Brushes.MediumBlue,
            Brushes.LightPink,
            Brushes.Magenta,
            Brushes.Yellow,
            Brushes.PaleGreen,
            Brushes.Orange,
            Brushes.Red,
            Brushes.Purple,
            Brushes.Green,
            Brushes.Blue
            };

        static Brush __defaultColour = Brushes.DimGray;

        static Brush __defaultRuleColour = Brushes.Red;
        #endregion

        #region dependency properties
        // --- BlockSize --- //
        // sets the size of blocks which will be shown in a horizontal row
        #region BlockSize
        public int BlockSize
        {
            get { return (int)GetValue(BlockSizeProperty); }
            set { SetValue(BlockSizeProperty, value); }
        }
        public static void OnBlockSizePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        { (sender as BlockChart)._setParameters(); }
        public static readonly DependencyProperty BlockSizeProperty =
            DependencyProperty.Register(
                "BlockSize", 
                typeof(int), 
                typeof(BlockChart), 
                new PropertyMetadata(
                    5,
                    OnBlockSizePropertyChanged));
        #endregion
        
        // --- BlockProportion --- //
        // sets how much of the block is filled with colour
        #region BlockProportion
        public double BlockProportion
        {
            get { return (double)GetValue(BlockProportionProperty); }
            set { SetValue(BlockProportionProperty, value); }
        }
        private static bool OnValidateBlockProportionProperty(object value)
        { return 0 <= (double)value && (double)value <= 1.0; }
        public static readonly DependencyProperty BlockProportionProperty =
            DependencyProperty.Register(
                "BlockProportion", 
                typeof(double), 
                typeof(BlockChart), 
                new PropertyMetadata(1.0),
                new ValidateValueCallback(OnValidateBlockProportionProperty));
        #endregion

        // --- RuleSpacing --- //
        // sets the interval for vertical rulings
        #region RuleSpacing
        public int RuleSpacing
        {
            get { return (int)GetValue(RuleSpacingProperty); }
            set { SetValue(RuleSpacingProperty, value); }
        }
        public static readonly DependencyProperty RuleSpacingProperty =
            DependencyProperty.Register(
                "RuleSpacing", 
                typeof(int), 
                typeof(BlockChart), 
                new PropertyMetadata(1));
        #endregion

        // --- RuleColour --- //
        // sets the colour of vertical rulings
        #region RuleColour
        public Brush RuleColour
        {
            get { return (Brush)GetValue(RuleColourProperty); }
            set { SetValue(RuleColourProperty, value); }
        }
        public static readonly DependencyProperty RuleColourProperty =
            DependencyProperty.Register(
                "RuleColour",
                typeof(Brush),
                typeof(BlockChart),
                new PropertyMetadata(__defaultRuleColour));
        #endregion

        // --- Label --- //
        // sets the contents of a textbox to the left of the chart
        #region Label
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        private static void OnLabelPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as BlockChart).CoerceValue(LabelWidthProperty);
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                "Label", 
                typeof(string), 
                typeof(BlockChart), 
                new PropertyMetadata(
                    string.Empty,
                    new PropertyChangedCallback(OnLabelPropertyChanged)));
        #endregion

        // --- LabelWidth --- //
        // sets the width of the TextBox for Label
        // coerced to 0 if Label is empty
        #region LabelWidth
        public int LabelWidth
        {
            get { return (int)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }
        private static object CoerceLabelWidthProperty(DependencyObject sender, object value)
        {
            if ((sender as BlockChart).Label == string.Empty)
                return 0;
            return value;
        }
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register(
                "LabelWidth", 
                typeof(int), 
                typeof(BlockChart), 
                new PropertyMetadata(
                    50,
                    null,
                    new CoerceValueCallback(CoerceLabelWidthProperty)));
        #endregion

        // --- BlockChartDataSource  --- //
        // an IList of type BlockChartData which will be used to set
        // the values of the base dproperty ChartDataSource
        #region BlockChartDataSource
        public IList<BlockChartData> BlockChartDataSource
        {
            get { return (IList<BlockChartData>)GetValue(BlockChartDataSourceProperty); }
            set { SetValue(BlockChartDataSourceProperty, value); }
        }
        private static void OnBlockChartDataSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            BlockChart chart = (sender as BlockChart);
            if (e.NewValue == null)
            {
                chart.BlockChartDataSource = null;
                chart.ChartDataSource = null;
                return;              
            }

            IList<BlockChartData> list = e.NewValue as IList<BlockChartData>;
            int[][] datalists = new int[list.Count][];
            string[] labels = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                datalists[i] = list[i].Data as int[];
                labels[i] = list[i].Label;
            }

            chart._labels = labels;  // important to do this first, so that the property changed call-back does not trigger too early
            chart.ChartDataSource = datalists;
        }
        public static readonly DependencyProperty BlockChartDataSourceProperty =
            DependencyProperty.Register( 
                "BlockChartDataSource", 
                typeof(IList<BlockChartData>), 
                typeof(BlockChart), 
                new PropertyMetadata(
                    null,
                    new PropertyChangedCallback(OnBlockChartDataSourcePropertyChanged)));
        #endregion
        #endregion


        #region protected data fields
        protected double _chartLeft;
        protected double _chartBottom;
        protected int _maximumLength;
        protected int _numberOfCharts;
        protected string[] _labels;
        #endregion

        #region background draw methods
        protected void _drawRulings()
        {
            if (_maximumLength == 0)
                return;

            double step = (double)BlockSize;
            double ruleStep = step * RuleSpacing;
            double x = _chartLeft;
            double yBottom = _chartBottom;
            double yTop = yBottom - (step * _numberOfCharts);
            Brush brush = RuleColour;
            for (int i = 0; i < _maximumLength; i += RuleSpacing, x += ruleStep)
            {
                Path p = new Path();
                p.StrokeThickness = 1;
                p.Stroke = brush;
                p.Data = new LineGeometry(
                    new Point(x,yBottom),
                    new Point(x, yTop));
                this.Children.Add(p);
            }
        }

        protected void _drawLabel(int index, double yLevel)
        {
            if (LabelWidth == 0 || _labels == null)
                return;

            TextBlock labelTB = new TextBlock();
            labelTB.Text = _labels[index];
            Canvas.SetLeft(labelTB, _chartLeft - LabelWidth);
            Canvas.SetTop(labelTB, yLevel );

            this.Children.Add(labelTB);
        }
        #endregion

        #region draw chart methods
        protected override void _drawChart()
        {
            int numberOfCharts = ChartDataSource.Count;

            _drawRulings();

            double yStep = BlockSize;
            double y = _chartBottom - yStep;
            for (int i = 0; i < numberOfCharts; i++, y -= yStep)
                _drawSingleChart(i, y);

        }


        private void _drawSingleChart(int index, double yLevel)
        {
            if (ChartDataSource.Count == 0)
                return;
            
            _drawLabel(index, yLevel);

            double step = BlockSize;
            double offset = step * (1 - BlockProportion) / 2;
            double y = yLevel + offset;
            double x = _chartLeft + offset;
            double d = step - offset - offset;

            Brush brush;
            IList<int> data = ChartDataSource[index];
            for (int i = 0; i < data.Count; i++, x += step)
            {
                if (data[i] >= __colourArray.Length)
                    brush = __defaultColour;
                else
                    brush = __colourArray[data[i]];

                Path p = new Path();
                p.StrokeThickness = 1;
                p.Data = new RectangleGeometry(new Rect(x, y, d, d));
                p.Fill = brush;
                this.Children.Add(p);
            }
            

        }
        #endregion

        #region helper methods
        protected override void _setParameters()
        {
            if (ChartDataSource == null)
                return;

            _numberOfCharts = ChartDataSource.Count;
            _maximumLength = int.MinValue;
            for (int i = 0; i < _numberOfCharts; i++)
            {
                if (_maximumLength < ChartDataSource[i].Count)
                    _maximumLength = ChartDataSource[i].Count;            
            }

            this.Width = Padding + LabelWidth + (BlockSize * _maximumLength) + Padding;
            this.Height = Padding + BlockSize * _numberOfCharts + Padding;
            if (!(this.Height > 0))
                this.Height = 20;
            _chartLeft = Padding + LabelWidth;
            _chartBottom = this.Height - Padding;
        }
        #endregion

    }

    class BoardBlockChart : BlockChart
    {
        #region dependency properties
        public BlockChartDayData DayData
        {
            get { return (BlockChartDayData)GetValue(DayDataProperty); }
            set { SetValue(DayDataProperty, value); }
        }
        private static void OnDayDataPropertyChanged (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            BoardBlockChart chart = sender as BoardBlockChart;
            chart._setParameters();
            chart._reDraw();
        }
        public static readonly DependencyProperty DayDataProperty =
            DependencyProperty.Register(
                "DayData", 
                typeof(BlockChartDayData), 
                typeof(BoardBlockChart), 
                new PropertyMetadata(
                    null,
                    new PropertyChangedCallback(OnDayDataPropertyChanged)));




        #endregion

        #region protected data fields
        protected double _textBlockHeight;
        #endregion

        #region private data fields
        #endregion

        #region overrides
        protected override void _drawChart()
        {
            base._drawChart();
            if (DayData == null)
                return;

            double xStep = BlockSize * RuleSpacing;
            double yStep = BlockSize;
            double x = _chartLeft;

            for (int i = 0; x < this.Width && i < DayData.Length; i++, x += xStep)
            {
                double y =0;
                TextBlock tbSummons = new TextBlock();
                tbSummons.Text = DayData.Day[i];
                Canvas.SetLeft(tbSummons, x);
                Canvas.SetTop(tbSummons, y);
                this.Children.Add(tbSummons);

                y += _textBlockHeight;
                TextBlock tbDecisions = new TextBlock();
                tbDecisions.Text = DayData.Week[i];
                Canvas.SetLeft(tbDecisions, x);
                Canvas.SetTop(tbDecisions, y);
                this.Children.Add(tbDecisions);

                y += _textBlockHeight;
                TextBlock tbSummonsQueue= new TextBlock();
                tbSummonsQueue.Text = "Summons queue : " + DayData.SummonsQueueData[i];
                Canvas.SetLeft(tbSummonsQueue, x);
                Canvas.SetTop(tbSummonsQueue, y);
                this.Children.Add(tbSummonsQueue);

                y += _textBlockHeight;
                TextBlock tbDecisionQueue = new TextBlock();
                tbDecisionQueue.Text = "Decision queue : " + DayData.DecisionQueueData[i];
                Canvas.SetLeft(tbDecisionQueue, x);
                Canvas.SetTop(tbDecisionQueue, y);
                this.Children.Add(tbDecisionQueue);

            }
        }



        protected override void _setParameters()
        {
            base._setParameters();
            _textBlockHeight = BlockSize;
            this.Height += _textBlockHeight * 4;
            _chartBottom = this.Height - Padding;
        }
        #endregion



    }



    abstract class ChartBase : Canvas
    {
        #region dependency properties
        // --- Padding --- //
        // sets a padding area around the chart
        #region Padding
        public double Padding
        {
            get { return (double)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        public static void OnPaddingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            __setup(sender);
            (sender as ChartBase)._drawChart();
        }
        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register(
                "Padding", 
                typeof(double), 
                typeof(ChartBase), 
                new PropertyMetadata(
                    0.0,
                    new PropertyChangedCallback(OnPaddingChanged)));
        #endregion

        // --- ChartData --- //
        // sets the data for drawing the graph, axes and so on
        #region ChartData
        public ChartData ChartData
        {
            get { return (ChartData)GetValue(ChartDataProperty); }
            set { SetValue(ChartDataProperty, value); }
        }
        public static void OnChartDataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            __setup(sender);
            (sender as ChartBase).Children.Clear();
            (sender as ChartBase)._drawAxes();
        }
        public static readonly DependencyProperty ChartDataProperty =
            DependencyProperty.Register(
                "ChartData", 
                typeof(ChartData), 
                typeof(ChartBase), 
                new PropertyMetadata(
                    null,
                    new PropertyChangedCallback(OnChartDataChanged)));
        #endregion
        #endregion

        #region protected data
        protected double _canvasLeft;
        protected double _canvasRight;
        protected double _canvasTop;
        protected double _canvasBottom;

        protected double _yAxis_X;
        protected double _xAxis_Y;
        protected double _xAxisStep;
        protected double _yAxisStep;
        #endregion

        #region abstract methods
        protected abstract void _drawChart();
        protected abstract void _drawAxes();
        #endregion


        #region static helper methods
        protected static void __setup(DependencyObject sender)
        {
            ChartBase chart = sender as ChartBase;
            chart._canvasLeft = chart.Padding;
            chart._canvasTop = chart.Padding;
            chart._canvasRight = chart.Width - chart.Padding;
            chart._canvasBottom = chart.Height - chart.Padding;
        }
        #endregion
    }

    abstract class IntChart : ChartBase
    { }

    class IntHistogram : IntChart
    {
        #region dependency property overrides
        // --- ChartData ---
        // The data for the chart
        #region ChartData
        new public IntHistogramChartData ChartData
        {
            get { return (IntHistogramChartData)GetValue(ChartDataProperty); }
            set { SetValue(ChartDataProperty, value); }
        }
        new public static void OnChartDataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            __setup(sender);

            IntHistogram chart = sender as IntHistogram;
            chart.Children.Clear();
            chart._drawChart();
            chart._drawAxes();

        }
        new public static readonly DependencyProperty ChartDataProperty =
            DependencyProperty.Register(
                "ChartData",
                typeof(IntHistogramChartData),
                typeof(IntHistogram),
                new PropertyMetadata(
                    null,
                    new PropertyChangedCallback(OnChartDataChanged)));
        #endregion

        // --- __setup ---//
        // overrides the base class __setup
        new protected static void __setup(DependencyObject sender)
        {
            ChartBase.__setup(sender);
            IntHistogram chart = sender as IntHistogram;
            if (chart.ChartData.Data != null)
            {
                chart._xAxis_Y = chart._canvasBottom - chart.ChartData.XAxisPosition * (chart._canvasBottom - chart._canvasTop);
                chart._yAxis_X = chart._canvasLeft + chart.ChartData.YAxisPosition * (chart._canvasRight - chart._canvasLeft);

                if (chart.ChartData.Data.Length > 0)
                {
                    chart._xAxisStep = (chart.Width - chart.Padding - chart.Padding) / chart.ChartData.Data.Length;
                    chart._yAxisStep = (chart.Height - chart.Padding - chart.Padding) / (chart.ChartData.YMax - chart.ChartData.YMin);
                }
                else
                {
                    chart._xAxisStep = 1;
                    chart._yAxisStep = 1;
                }
            }
        }
        #endregion

        #region overrides and implementations of abstract methods
        protected override void _drawAxes()
        {
            if (ChartData.ShowXAxis)
                _drawXAxis();

            if (ChartData.ShowYAxis)
                _drawYAxis();
        }

        protected void _drawXAxis()
        {
            if (ChartData == null)
                return;

            if (!ChartData.ShowXAxis)
                return;

            Path p = new Path();
            p.StrokeThickness = ChartData.XAxisThickness;
            p.Stroke = ChartData.XAxisColour;
            double y = _xAxis_Y;
            p.Data = new LineGeometry(
                new Point(_canvasLeft, y),
                new Point(_canvasRight, y));
            this.Children.Add(p);

            _tickXAxis();
            _labelXAxis();
        }

        protected void _drawYAxis()
        {
            if (ChartData == null)
                return;

            if (!ChartData.ShowYAxis)
                return;

            Path p = new Path();
            p.StrokeThickness = ChartData.YAxisThickness;
            p.Stroke = ChartData.YAxisColour;
            double x = _yAxis_X;
            p.Data = new LineGeometry(
                new Point(x, _canvasBottom),
                new Point(x, _canvasTop));
            this.Children.Add(p);

            _tickYAxis();
            _labelYAxis();
        }

        protected void _tickXAxis()
        {
            if (!ChartData.XAxisTicks)
            {
                if (ChartData.XAxisLabels == null)
                    return;
                else
                    if (!(ChartData.XAxisLabels.Length > 0 && _xAxisStep > 0))
                    return;
            }

            double step = ChartData.XAxisLabelInterval * _xAxisStep;
            double x = _yAxis_X + step;
            double y = _xAxis_Y;
            while (x < _canvasRight)
            {
                Path p = new Path();
                p.StrokeThickness = ChartData.XAxisThickness;
                p.Stroke = ChartData.XAxisColour;
                p.Data = new LineGeometry(
                    new Point(x, y + 5),
                    new Point(x, y));
                this.Children.Add(p);

                x += step;
            }

            x = _yAxis_X - step;
            while (x > _canvasLeft)
            {
                Path p = new Path();
                p.StrokeThickness = ChartData.XAxisThickness;
                p.Stroke = ChartData.XAxisColour;
                p.Data = new LineGeometry(
                    new Point(x, y + 5),
                    new Point(x, y));
                this.Children.Add(p);

                x -= step;
            }


        }

        protected void _tickYAxis()
        {
            if (!ChartData.YAxisTicks)
            {
                if (ChartData.YAxisLabels == null)
                    return;
                else
                    if (!(ChartData.YAxisLabels.Length > 0 && _xAxisStep > 0))
                    return;
            }

            double step = ChartData.YAxisLabelInterval * _yAxisStep;
            double x = _yAxis_X;
            double y = _xAxis_Y - step;
            while (y > _canvasTop)
            {

                Path p = new Path();
                p.StrokeThickness = ChartData.YAxisThickness;
                p.Stroke = ChartData.YAxisColour;
                p.Data = new LineGeometry(
                    new Point(x - 5, y),
                    new Point(x, y));
                this.Children.Add(p);

                y -= step;
            }

            y = _xAxis_Y + step;
            while (y < _canvasBottom)
            {

                Path p = new Path();
                p.StrokeThickness = ChartData.YAxisThickness;
                p.Stroke = ChartData.YAxisColour;
                p.Data = new LineGeometry(
                    new Point(x - 5, y),
                    new Point(x, y));
                this.Children.Add(p);

                y += step;
            }
        }

        protected void _labelXAxis()
        {
            if (ChartData.XAxisLabels == null)
                return;
            if (ChartData.XAxisLabels.Length == 0)
                return;

            double x = _yAxis_X;
            double step = ChartData.XAxisLabelInterval * _xAxisStep;
            while (x > _canvasLeft)
                x -= step;
            if (x < _yAxis_X)
                x += step;

            for (int index = 0; index < ChartData.XAxisLabels.Length && x <= _canvasRight - step; index++, x += step)
            {
                TextBlock tb = new TextBlock();
                tb.Text = ChartData.XAxisLabels[index];
                tb.TextAlignment = TextAlignment.Center;
                tb.Foreground = ChartData.XAxisColour;
                tb.Width = step;
                Canvas.SetLeft(tb, x);
                Canvas.SetTop(tb, _xAxis_Y + 5);
                this.Children.Add(tb);
            }
        }

        protected void _labelYAxis()
        {

            if (ChartData.YAxisLabels == null)
                return;
            if (ChartData.YAxisLabels.Length == 0)
                return;

            double y = _xAxis_Y;
            double step = ChartData.YAxisLabelInterval * _yAxisStep;
            while (y < _canvasBottom)
                y += step;
            if (y > _xAxis_Y)
                y -= step;

            for (int index = 0; index < ChartData.YAxisLabels.Length && y >= _canvasTop; index++, y -= step)
            {
                TextBlock tb = new TextBlock();
                tb.Text = ChartData.YAxisLabels[index];
                tb.TextAlignment = TextAlignment.Right;
                tb.Foreground = ChartData.YAxisColour;
                tb.Width = 20;
                tb.Height = 15;
                Canvas.SetLeft(tb, _yAxis_X - tb.Width - 10);
                Canvas.SetTop(tb, y - tb.Height / 2);
                this.Children.Add(tb);
            }
        }

        protected override void _drawChart()
        {
            if (ChartData == null)
                return;
            if (ChartData.Data == null)
                return;
            if (ChartData.Data.Length == 0)
                return;

            double x = _yAxis_X;
            double offset = _xAxisStep * (1 - ChartData.BarWidth) / 2;
            double barwidth = _xAxisStep * ChartData.BarWidth;
            while (x > _canvasLeft)
                x -= _xAxisStep;
            if (x < _yAxis_X)
                x += _xAxisStep;
            x += offset;

            for (int i = 0; i < ChartData.Data.Length && x < _canvasRight; i++, x += _xAxisStep)
            {
                Path p = new Path();
                p.StrokeThickness = 1;
                p.Stroke = ChartData.BarOutlineColour;
                p.Fill = ChartData.BarColour;
                p.Data = new RectangleGeometry(
                    new Rect(
                        new Point(x, _xAxis_Y),
                        new Point(x + barwidth, _xAxis_Y - (ChartData.Data[i] - ChartData.YMin) * _yAxisStep)));
                this.Children.Add(p);
            }
        }
        #endregion
    }

    class IntLineChart : IntChart
    {
        #region dependency property overrides
        // --- ChartData ---
        // The data for the chart
        #region ChartData
        new public IntLineChartData ChartData
        {
            get { return (IntLineChartData)GetValue(ChartDataProperty); }
            set { SetValue(ChartDataProperty, value); }
        }
        new public static void OnChartDataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            __setup(sender);

            IntLineChart chart = sender as IntLineChart;
            chart.Children.Clear();
            chart._drawAxes();
            chart._drawChart();

        }
        new public static readonly DependencyProperty ChartDataProperty =
            DependencyProperty.Register(
                "ChartData",
                typeof(IntLineChartData),
                typeof(IntLineChart),
                new PropertyMetadata(
                    null,
                    new PropertyChangedCallback(OnChartDataChanged)));
        #endregion

        // --- __setup ---//
        // overrides the base class __setup
        new protected static void __setup(DependencyObject sender)
        {
            ChartBase.__setup(sender);
            IntLineChart chart = sender as IntLineChart;
            if (chart.ChartData.Data != null)
            {
                chart._xAxis_Y = chart._canvasBottom - chart.ChartData.XAxisPosition * (chart._canvasBottom - chart._canvasTop);
                chart._yAxis_X = chart._canvasLeft + chart.ChartData.YAxisPosition * (chart._canvasRight - chart._canvasLeft);

                if (chart.ChartData.Data.Length > 0)
                {
                    chart._xAxisStep = (chart.Width - chart.Padding - chart.Padding) / chart.ChartData.Data.Length;
                    chart._yAxisStep = (chart.Height - chart.Padding - chart.Padding) / (chart.ChartData.YMax - chart.ChartData.YMin);
                }
                else
                {
                    chart._xAxisStep = 1;
                    chart._yAxisStep = 1;
                }
            }
        }
        #endregion



        #region overrides and implementations of abstract methods
        protected override void _drawAxes()
        {
            if (ChartData.ShowXAxis)
                _drawXAxis();

            if (ChartData.ShowYAxis)
                _drawYAxis();
        }

        protected void _drawXAxis()
        {
            if (ChartData == null)
                return;

            if (!ChartData.ShowXAxis)
                return;

            Path p = new Path();
            p.StrokeThickness = ChartData.XAxisThickness;
            p.Stroke = ChartData.XAxisColour;
            double y = _xAxis_Y;
            p.Data = new LineGeometry(
                new Point(_canvasLeft, y),
                new Point(_canvasRight, y));
            this.Children.Add(p);

            _tickXAxis();
            _labelXAxis();
        }

        protected void _drawYAxis()
        {
            if (ChartData == null)
                return;

            if (!ChartData.ShowYAxis)
                return;

            Path p = new Path();
            p.StrokeThickness = ChartData.YAxisThickness;
            p.Stroke = ChartData.YAxisColour;
            double x = _yAxis_X;
            p.Data = new LineGeometry(
                new Point(x, _canvasBottom),
                new Point(x, _canvasTop));
            this.Children.Add(p);

            _tickYAxis();
            _labelYAxis();
        }

        protected void _tickXAxis()
        {
            if (!ChartData.XAxisTicks)
            {
                if (ChartData.XAxisLabels == null)
                    return;
                else
                    if (!(ChartData.XAxisLabels.Length > 0 && _xAxisStep > 0))
                    return;
            }

            double step = ChartData.XAxisLabelInterval * _xAxisStep;
            double x = _yAxis_X + step;
            double y = _xAxis_Y;
            while (x < _canvasRight)
            {
                Path p = new Path();
                p.StrokeThickness = ChartData.XAxisThickness;
                p.Stroke = ChartData.XAxisColour;
                p.Data = new LineGeometry(
                    new Point(x, y + 5),
                    new Point(x, y));
                this.Children.Add(p);

                x += step;
            }

            x = _yAxis_X - step;
            while (x > _canvasLeft)
            {
                Path p = new Path();
                p.StrokeThickness = ChartData.XAxisThickness;
                p.Stroke = ChartData.XAxisColour;
                p.Data = new LineGeometry(
                    new Point(x, y + 5),
                    new Point(x, y));
                this.Children.Add(p);

                x -= step;
            }


        }

        protected void _tickYAxis()
        {
            if (!ChartData.YAxisTicks)
            {
                if (ChartData.YAxisLabels == null)
                    return;
                else
                    if (!(ChartData.YAxisLabels.Length > 0 && _xAxisStep > 0))
                    return;
            }

            double step = ChartData.YAxisLabelInterval * _yAxisStep;
            double x = _yAxis_X;
            double y = _xAxis_Y - step;
            while (y >=_canvasTop)
            {

                Path p = new Path();
                p.StrokeThickness = ChartData.YAxisThickness;
                p.Stroke = ChartData.YAxisColour;
                p.Data = new LineGeometry(
                    new Point(x - 5, y),
                    new Point(x, y));
                this.Children.Add(p);

                y -= step;
            }

            y = _xAxis_Y + step;
            while (y <= _canvasBottom)
            {

                Path p = new Path();
                p.StrokeThickness = ChartData.YAxisThickness;
                p.Stroke = ChartData.YAxisColour;
                p.Data = new LineGeometry(
                    new Point(x - 5, y),
                    new Point(x, y));
                this.Children.Add(p);

                y += step;
            }
        }

        protected void _labelXAxis()
        {
            if (ChartData.XAxisLabels == null)
                return;
            if (ChartData.XAxisLabels.Length == 0)
                return;

            double x = _yAxis_X;
            double step = ChartData.XAxisLabelInterval * _xAxisStep;
            while (x > _canvasLeft)
                x -= step;
            if (x < _yAxis_X)
                x += step;
            

            for (int index = 0; index < ChartData.XAxisLabels.Length && x <= _canvasRight - step; index++, x += step)
            {
                TextBlock tb = new TextBlock();
                tb.Text = ChartData.XAxisLabels[index];
                tb.TextAlignment = TextAlignment.Center;
                tb.Foreground = ChartData.XAxisColour;
                tb.Width = step;
                Canvas.SetLeft(tb, x);
                Canvas.SetTop(tb, _xAxis_Y + 5);
                this.Children.Add(tb);
            }
        }

        protected void _labelYAxis()
        {

            if (ChartData.YAxisLabels == null)
                return;
            if (ChartData.YAxisLabels.Length == 0)
                return;

            double y = _xAxis_Y;
            double step = ChartData.YAxisLabelInterval * _yAxisStep;
            while (y < _canvasBottom)
                y += step;
            if (y > _xAxis_Y)
                y -= step;

            for (int index = 0; index < ChartData.YAxisLabels.Length && y >= _canvasTop; index++, y -= step)
            {
                TextBlock tb = new TextBlock();
                tb.Text = ChartData.YAxisLabels[index];
                tb.TextAlignment = TextAlignment.Right;
                tb.Foreground = ChartData.YAxisColour;
                tb.Width = 100;
                tb.Height = 15;
                Canvas.SetLeft(tb, _yAxis_X - tb.Width - 10);
                Canvas.SetTop(tb, y - tb.Height / 2);
                this.Children.Add(tb);
            }
        }

        protected override void _drawChart()
        {
            if (ChartData == null)
                return;
            if (ChartData.Data == null)
                return;
            if (ChartData.Data.Length == 0)
                return;

            double x = _yAxis_X;
            while (x > _canvasLeft)
                x -= _xAxisStep;
            if (x < _yAxis_X)
                x += _xAxisStep;
            
            switch (ChartData.HorizontalAlignment)
            {
                case IntLineChartData.Position.left:
                    break;
                case IntLineChartData.Position.center:
                    x += _xAxisStep / 2;
                    break;
                case IntLineChartData.Position.right:
                    x += _xAxisStep;
                    break;
            }

            PointCollection points = new PointCollection();
            for (int i = 0; i<ChartData.Data.Length; i++, x += _xAxisStep)
            {
                points.Add(new Point(x, _xAxis_Y - ((ChartData.Data[i] - ChartData.YMin) * _yAxisStep)));
            }

            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 1;
            polyline.Stroke = Brushes.Red;
            polyline.Points = points;

            this.Children.Add(polyline);
        }
        #endregion
        
    }






}
