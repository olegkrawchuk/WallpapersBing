using System.ComponentModel;

namespace Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected virtual bool SetProperty<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName] System.String propertyName = "")
        {
            if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(System.String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
