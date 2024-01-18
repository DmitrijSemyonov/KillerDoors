using System;

namespace KillerDoors.Data.Management
{
    public class ObservableVariable<T>
    {
        public event Action<T> Changed;

        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Changed?.Invoke(_value);
            }
        }
        public ObservableVariable(T value) => 
            _value = value;

        public void Dispose() =>
            Changed = null;
    }
}