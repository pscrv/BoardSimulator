namespace SimulatorUI
{
    internal class MemberParameterCollectionViewModel : ObservableObject
    {
        private MemberParameterCollection _collection;
        private MemberParametersViewModel _chair;
        private MemberParametersViewModel _rapporteur;
        private MemberParametersViewModel _other;


        public MemberParameterCollectionViewModel(MemberParameterCollection collection)
        {
            _collection = collection;
            _chair = new MemberParametersViewModel(collection.ChairWorkParameters);
            _rapporteur = new MemberParametersViewModel(collection.RapporteurWorkParameters);
            _other = new MemberParametersViewModel(collection.OtherWorkParameters);

            _chair.PropertyChanged += (s, e) => this.OnPropertyChanged("Chair");
            _rapporteur.PropertyChanged += (s, e) => this.OnPropertyChanged("Rapporteur");
            _other.PropertyChanged += (s, e) => this.OnPropertyChanged("Other");
        }


        public MemberParametersViewModel ChairParameters
        { get { return _chair; } }

        public MemberParametersViewModel RapporteurParameters
        { get { return _rapporteur; } }

        public MemberParametersViewModel OtherMemberParameters
        { get { return _other; } }
    }
}