using System.ComponentModel;

namespace SimulatorUI
{

    public abstract class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }


        protected void SetProperty<T>(ref T backingfield, T value, string propertyName)
        {
            if (Equals(backingfield, value))
                return;

            backingfield = value;
            OnPropertyChanged(propertyName);
        }

    }



}