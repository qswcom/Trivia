using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            temp?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetValue<TProperty>(ref TProperty property, TProperty value,
            [CallerMemberName] string propertyName = null)
        {
            if (Equals(property, value))
            {
                return false;
            }

            property = value;
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}