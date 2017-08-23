using System.ComponentModel;
using System.Windows.Input;

namespace SimulatorUI
{
    public class BoardParametersViewModel : ViewModel
    {
        #region enumm
        public enum ViewType { Summary, Details }
        #endregion



        #region fields and properties
        public BoardDetailsViewModel DetailsVM { get; private set; }
        public BoardSummaryViewModel SummaryVM { get; private set; }
        public ViewType ActiveViewType { get; private set; }
        #endregion



        #region commands
        public ICommand ShowDetailsCommand
        {
            get => new DelegateParamterisedCommand(_showDetails);
        }

        public ICommand ShowSummaryCommand
        {
            get => new DelegateParamterisedCommand(_showSummary);
        }

        private void _showSummary(object parameter)
        {
            if (ActiveViewType == ViewType.Summary)
                return;

            ActiveViewType = ViewType.Summary;
            OnPropertyChanged("ActiveViewType");
        }

        private void _showDetails(object parameter)
        {
            if (ActiveViewType == ViewType.Details)
                return;

            ActiveViewType = ViewType.Details;
            OnPropertyChanged("ActiveViewType");
        }
        #endregion



        #region construction
        public BoardParametersViewModel(BoardParameters parameters)
        {
            DetailsVM = new BoardDetailsViewModel(parameters);
            SummaryVM = new BoardSummaryViewModel(parameters);
            ActiveViewType = ViewType.Summary;

            DetailsVM.PropertyChanged += _update;
        }

        internal void Reset()
        {
            ActiveViewType = ViewType.Summary;
            OnPropertyChanged("ActiveViewType");
        }
        #endregion



        private void _update(object sender, PropertyChangedEventArgs e)
        {
            SummaryVM = new BoardSummaryViewModel(DetailsVM.Parameters);
            OnPropertyChanged("DetailsVM");
        }
    }
}