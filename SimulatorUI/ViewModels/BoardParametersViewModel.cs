using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

using Simulator;

namespace SimulatorUI
{
    public class BoardParametersViewModel : ViewModel
    {
        #region enumm
        public enum ViewType { Summary, Details }
        #endregion

        #region static
        private static ChairType __type = ChairType.Technical;
        private static MemberParameterCollection __chair = new MemberParameterCollection(
            new MemberParameters(6, 6, 12),
            new MemberParameters(40, 8, 24),
            new MemberParameters(3, 4, 8));


        private static MemberParameterCollection __defaultTechnical()
        {
            return new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9));
        }

        private static MemberParameterCollection __defaultLegal()
        {
            return new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9));
        }


        private static List<MemberParameterCollection> __technicals = new List<MemberParameterCollection>
            {
                new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9)),

                new MemberParameterCollection(
                    new MemberParameters(8, 8, 14),
                    new MemberParameters(42, 10, 26),
                    new MemberParameters(5, 6, 10))
            };

        private static List<MemberParameterCollection> __legals = new List<MemberParameterCollection>
            {
                new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9)),

                new MemberParameterCollection(
                    new MemberParameters(8, 8, 14),
                    new MemberParameters(42, 10, 26),
                    new MemberParameters(5, 6, 10)),

                new MemberParameterCollection(
                    new MemberParameters(9, 9, 15),
                    new MemberParameters(43, 11, 27),
                    new MemberParameters(6, 7, 11))
            };


        private static BoardParameters __DefaultParameters()
        {
            return new BoardParameters(
                __type,
                __chair,
                __technicals,
                __legals);
        }
        
        #endregion


        #region fields and properties
        private BoardParameters _parameters;
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



        #region construction
        public BoardParametersViewModel(BoardParameters parameters)
        {
            _parameters = parameters;
            _detailsVM = new BoardDetailsViewModel(parameters);
            _summaryVM = new BoardSummaryViewModel(parameters);
            _viewtype = ViewType.Summary;

            _detailsVM.PropertyChanged += _update;
        }

        public BoardParametersViewModel()
            : this(__DefaultParameters()) { }
        #endregion


        private void _update(object sender, PropertyChangedEventArgs e)
        {
            _summaryVM = new BoardSummaryViewModel(_detailsVM.Parameters);
            OnPropertyChanged("DetailsVM");
        }
    }
}