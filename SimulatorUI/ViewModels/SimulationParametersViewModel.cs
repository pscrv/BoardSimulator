namespace SimulatorUI
{
    public class SimulationParametersViewModel : ViewModel
    {
        private int _miniRunLength;
        private int _fullRunLength;
        private int _initialCaseCount;
        private int _arrivalsPerMonth;
        private int _minimumDaysBetweenOP;
        

        public int MiniRunLength
        {
            get => _miniRunLength;
            set => SetProperty(ref _miniRunLength, value, "MiniRunLength");
        }

        public int FullRunLength
        {
            get => _fullRunLength;
            set => SetProperty(ref _fullRunLength, value, "FullRunLength");
        }

        public int InitialCaseCount
        {
            get => _initialCaseCount;
            set => SetProperty(ref _initialCaseCount, value, "InitialCaseCount");
        }

        public int ArrivalsPerMonth
        {
            get => _arrivalsPerMonth;
            set => SetProperty(ref _arrivalsPerMonth, value, "ArrivalsPerMonth");
        }

        public int MinimumDaysBetweenOP
        {
            get => _minimumDaysBetweenOP;
            set => SetProperty(ref _minimumDaysBetweenOP, value, "MinimumDaysBetweenOP");
        }
    }

}