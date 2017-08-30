using System;
using System.ComponentModel;

namespace SimulatorUI
{
    public abstract class MemberParameterCollectionViewModel : ViewModel
    {
        #region fields and properties
        protected MemberParameterCollection _collection;
        protected MemberParametersViewModel _chair;
        protected MemberParametersViewModel _rapporteur;
        protected MemberParametersViewModel _other;
        protected int _chairWorkPercentage;
        

        public MemberParameterCollection Parameters
        { get { return _collection; } }

        public MemberParametersViewModel ChairParameters
        { get { return _chair; } }

        public MemberParametersViewModel RapporteurParameters
        { get { return _rapporteur; } }

        public MemberParametersViewModel OtherMemberParameters
        { get { return _other; } }

        public virtual int ChairWorkPercentage
        {
            get => _chairWorkPercentage;
            set
            {
                _collection.ChairWorkPercentage = value;
                SetProperty(ref _chairWorkPercentage, value, "ChairWorkPercentage");
            }
        }
        #endregion


        #region construction
        public MemberParameterCollectionViewModel(
            MemberParameterCollection collection, 
            int chairWorkPercentage = 0)
        {
            _collection = collection;
            _chairWorkPercentage = chairWorkPercentage;
            _setMembers();

            _chair.PropertyChanged += (s, e) => this.OnPropertyChanged("Chair");
            _rapporteur.PropertyChanged += (s, e) => this.OnPropertyChanged("Rapporteur");
            _other.PropertyChanged += (s, e) => this.OnPropertyChanged("Other");
        }
        
        #endregion


        #region abstract methods
        protected abstract void _setMembers();

        public abstract MemberParameterCollectionViewModel Add(MemberParameterCollectionViewModel other);
        #endregion
    }




    public class MemberParameterCollection_FixedViewModel : MemberParameterCollectionViewModel
    {
        public MemberParameterCollection_FixedViewModel(MemberParameterCollection collection, int chairWorkPercentage = 0) 
            : base (collection, chairWorkPercentage) { }


        #region overrides
        protected override void _setMembers()
        {
            _chair = new MemberParameters_FixedViewModel(_collection.ChairWorkParameters);
            _rapporteur = new MemberParameters_FixedViewModel(_collection.RapporteurWorkParameters);
            _other = new MemberParameters_FixedViewModel(_collection.OtherWorkParameters);
        }
        

        public override MemberParameterCollectionViewModel Add(MemberParameterCollectionViewModel other)
        {
            MemberParameterCollection_FixedViewModel otherFixed = other as MemberParameterCollection_FixedViewModel;

            if (otherFixed == null)
                throw new InvalidOperationException("Can only add _FixedViewModel");

            return new MemberParameterCollection_FixedViewModel(
                new MemberParameterCollection(
                    _chair.Parameters.Add(otherFixed._chair.Parameters),
                    _rapporteur.Parameters.Add(otherFixed._rapporteur.Parameters),
                    _other.Parameters.Add(otherFixed._other.Parameters)),
                    _chairWorkPercentage);
        }
        #endregion
    }




    public class MemberParameterCollection_DynamicViewModel : MemberParameterCollectionViewModel  //, IDataErrorInfo
    {
        #region Delegates
        public delegate int AvailableChairWorkPercentageDelegate(int currentPercentage);
        #endregion

        #region Fields and Properties
        private int _maximumAvailableChairWorkPercentage;


        public int MaximumAvailableChairWorkPercentage
        {
            get => _maximumAvailableChairWorkPercentage;
            set => SetProperty(ref _maximumAvailableChairWorkPercentage, value, "MaximumAvailableChairWorkPercentage");
        }

        #endregion


        #region Construction
        public MemberParameterCollection_DynamicViewModel(
            MemberParameterCollection collection,
            int maximumAvailableChairWorkPercentage,
            int chairWorkPercentage = 0)
            : base(collection, chairWorkPercentage)
        {
            _maximumAvailableChairWorkPercentage = maximumAvailableChairWorkPercentage;
        }
        #endregion



        #region overrides
        protected override void _setMembers()
        {
            _chair = new MemberParameters_DynamicViewModel(_collection.ChairWorkParameters);
            _rapporteur = new MemberParameters_DynamicViewModel(_collection.RapporteurWorkParameters);
            _other = new MemberParameters_DynamicViewModel(_collection.OtherWorkParameters);
        }
        
        public override MemberParameterCollectionViewModel Add(MemberParameterCollectionViewModel other)
        {
            MemberParameterCollection_DynamicViewModel otherDynamic = other as MemberParameterCollection_DynamicViewModel;

            if (otherDynamic == null)
                throw new InvalidOperationException("Can only add _DynamicViewModel");

            return new MemberParameterCollection_FixedViewModel(
                new MemberParameterCollection(
                    _chair.Parameters.Add(otherDynamic._chair.Parameters),
                    _rapporteur.Parameters.Add(otherDynamic._rapporteur.Parameters),
                    _other.Parameters.Add(otherDynamic._other.Parameters)),
                    _chairWorkPercentage);
        }
        #endregion               
    }
}