using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoardSimulator.Resources
{
    /// <summary>
    /// Interaction logic for Label_Int_Pair.xaml
    /// </summary>
    public partial class Label_Int_Pair : UserControl
    {
        #region constructors
        public Label_Int_Pair()
        {
            InitializeComponent();
            { }
        }
        #endregion


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
                typeof(Label_Int_Pair), 
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
                typeof(Label_Int_Pair), 
                new PropertyMetadata(1));



        public int LabelWidth
        {
            get { return (int)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register(
                "LabelWidth", 
                typeof(int), 
                typeof(Label_Int_Pair), 
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
                typeof(Label_Int_Pair), 
                new PropertyMetadata(50));



        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                "Minimum", 
                typeof(int), 
                typeof(Label_Int_Pair), 
                new PropertyMetadata(0));


        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                "Maximum", 
                typeof(int), 
                typeof(Label_Int_Pair), 
                new PropertyMetadata(100));
        #endregion



    }
}
