using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimulatorUI.AppResources.UserControls
{
    /// <summary>
    /// Interaction logic for IndexedTabControl.xaml
    /// </summary>
    public partial class IndexedTabControl : UserControl
    {
        public IndexedTabControl()
            : base()
        {
            InitializeComponent();
        }


        public ObservableCollection<MemberParameterCollection_DynamicViewModel> Collection
        {
            get { return (ObservableCollection<MemberParameterCollection_DynamicViewModel>)GetValue(CollectionProperty); }
            set { SetValue(CollectionProperty, value); }
        }
        public static readonly DependencyProperty CollectionProperty =
            DependencyProperty.Register(
                "Collection", 
                typeof(ObservableCollection<MemberParameterCollection_DynamicViewModel>), 
                typeof(IndexedTabControl), 
                new PropertyMetadata(null));





        public ICommand RemoveItemCommand
        {
            get { return (ICommand)GetValue(RemoveItemCommandProperty); }
            set { SetValue(RemoveItemCommandProperty, value); }
        }
        public static readonly DependencyProperty RemoveItemCommandProperty =
            DependencyProperty.Register(
                "RemoveItemCommand", 
                typeof(ICommand), 
                typeof(IndexedTabControl), 
                new PropertyMetadata(null));




    }
}
