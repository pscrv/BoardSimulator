using System.Windows;
using System.Windows.Controls;

namespace SimulatorUI.AppResources.UserControls
{
    /// <summary>
    /// Interaction logic for TextIconButton.xaml
    /// </summary>
    public partial class TextIconButton : Button
    {
        public TextIconButton()
            : base()
        {
            InitializeComponent();
        }



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }        
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text", 
                typeof(string), 
                typeof(TextIconButton), 
                new PropertyMetadata("DefaultTest"));
        

        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                "Icon", 
                typeof(object), 
                typeof(TextIconButton), 
                new PropertyMetadata(null));


        public enum Position { Left, Right }
        public Position IconPosition
        {
            get { return (Position)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register(
                "IconPosition", 
                typeof(Position), 
                typeof(TextIconButton), 
                new PropertyMetadata(Position.Right));



        
    }
}
