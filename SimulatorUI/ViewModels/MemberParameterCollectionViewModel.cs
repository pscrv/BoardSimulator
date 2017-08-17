using System;

namespace SimulatorUI
{
    public abstract class MemberParameterCollectionViewModel : ViewModel
    {
        #region fields and properties
        protected MemberParameterCollection _collection;
        protected MemberParametersViewModel _chair;
        protected MemberParametersViewModel _rapporteur;
        protected MemberParametersViewModel _other;


        public MemberParameterCollection Parameters
        { get { return _collection; } }

        public MemberParametersViewModel ChairParameters
        { get { return _chair; } }

        public MemberParametersViewModel RapporteurParameters
        { get { return _rapporteur; } }

        public MemberParametersViewModel OtherMemberParameters
        { get { return _other; } }
        #endregion


        #region construction

        public MemberParameterCollectionViewModel(MemberParameterCollection collection)
        {
            _collection = collection;
            _setMembers();

            //_chair = new MemberParametersViewModel(collection.ChairWorkParameters);
            //_rapporteur = new MemberParametersViewModel(collection.RapporteurWorkParameters);
            //_other = new MemberParametersViewModel(collection.OtherWorkParameters);

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
        public MemberParameterCollection_FixedViewModel(MemberParameterCollection collection) 
            : base (collection) { }


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
                    _other.Parameters.Add(otherFixed._other.Parameters)));
        }
        #endregion
    }

    public class MemberParameterCollection_DynamicViewModel : MemberParameterCollectionViewModel
    {
        public MemberParameterCollection_DynamicViewModel(MemberParameterCollection collection)
            : base(collection)
        { }


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
                    _other.Parameters.Add(otherDynamic._other.Parameters)));
        }
        #endregion
    }
}