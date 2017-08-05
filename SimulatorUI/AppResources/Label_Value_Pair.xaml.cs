using System.Windows;
using System.Windows.Controls;

namespace SimulatorUI.Resources
{
    /// <summary>
    /// Interaction logic for Label_Value_Pair.xaml
    /// </summary>
    public partial class Label_Value_Pair : UserControl
    {
        public Label_Value_Pair()
        {
            InitializeComponent();
        }


        #region dependency properties
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                "Label",
                typeof(string),
                typeof(Label_Value_Pair),
                new PropertyMetadata(""));


        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(int),
                typeof(Label_Value_Pair),
                new PropertyMetadata(0));



        public int LabelWidth
        {
            get { return (int)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register(
                "LabelWidth",
                typeof(int),
                typeof(Label_Value_Pair),
                new PropertyMetadata(200));



        public int ValueWidth
        {
            get { return (int)GetValue(ValueWidthProperty); }
            set { SetValue(ValueWidthProperty, value); }
        }
        public static readonly DependencyProperty ValueWidthProperty =
            DependencyProperty.Register(
                "ValueWidth",
                typeof(int),
                typeof(Label_Value_Pair),
                new PropertyMetadata(50));
        #endregion



    }
}
