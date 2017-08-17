using System;
using System.ComponentModel;
using System.Windows.Input;

namespace SimulatorUI
{
    public class BoardParametersViewModel : ViewModel
    {
        #region enumm
        public enum ViewType { Summary, Details }
        #endregion

        #region static
        public static BoardParametersViewModel MakeDefaultBoard()
        {
            BoardParameters parameters = BoardDetailsViewModel.MakeDefaultBoard().Parameters;
            return new BoardParametersViewModel(parameters);
        }
        #endregion


        #region fields and properties
        private BoardDetailsViewModel _detailsVM;
        private BoardSummaryViewModel _summaryVM;
        private ViewType _viewtype;


        public BoardDetailsViewModel DetailsVM { get => _detailsVM; }
        public BoardSummaryViewModel SummaryVM { get => _summaryVM; }
        public ViewType ActiveViewType { get => _viewtype; }
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
            _viewtype = ViewType.Summary;
            OnPropertyChanged("ActiveViewType");
        }

        private void _showDetails(object parameter)
        {
            _viewtype = ViewType.Details;
            OnPropertyChanged("ActiveViewType");
        }
        #endregion



        #region consctruction
        public BoardParametersViewModel(BoardParameters parameters)
        {
            _detailsVM = new BoardDetailsViewModel(parameters);
            _summaryVM = new BoardSummaryViewModel(parameters);
            _viewtype = ViewType.Summary;

            _detailsVM.PropertyChanged += _update;
        }
        #endregion


        private void _update(object sender, PropertyChangedEventArgs e)
        {
            _summaryVM = new BoardSummaryViewModel(_detailsVM.Parameters);
            OnPropertyChanged("DetailsVM");
        }
    }
}