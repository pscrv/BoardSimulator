using System.Windows;
using System.Windows.Controls;

namespace SimulatorUI.AppResources.CustomControls
{
    public class TextIconButton : Button
    {
        static TextIconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TextIconButton),
                new FrameworkPropertyMetadata(typeof(TextIconButton)));
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
                new PropertyMetadata("DefaultText"));



        public enum Position { Left, Right }
        public Position ContentPosition
        {
            get { return (Position)GetValue(ContentPositionProperty); }
            set { SetValue(ContentPositionProperty, value); }
        }
        public static readonly DependencyProperty ContentPositionProperty =
            DependencyProperty.Register(
                "ContentPosition",
                typeof(Position),
                typeof(TextIconButton),
                new PropertyMetadata(Position.Right));
    }
}

