namespace SimulatorUI
{
    internal class FullSimulationViewModel : ViewModel
    {
        private BoardParameters _boardParameters;
        

        public FullSimulationViewModel(BoardParameters boardParameters)
        {
            _boardParameters = boardParameters;
        }
    }
}