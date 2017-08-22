namespace SimulatorUI
{
    internal class FullSimulationViewModel : ViewModel
    {
        private BoardParametersViewModel boardParametersVM;

        public FullSimulationViewModel()
        {
        }

        public FullSimulationViewModel(BoardParametersViewModel boardParametersVM)
        {
            this.boardParametersVM = boardParametersVM;
        }
    }
}