using System;

namespace DynamicDecorator
{
    public class PropertyAccessor
    {
        readonly Func<object> _getter;
        readonly Action<object> _setter;

        public PropertyAccessor(string propertyName, 
            Func<object> getter, 
            Action<object> setter,
            dynamic initialValue)
        {
            PropertyName = propertyName;
            _getter = getter;
            _setter = setter;
            InitialValue = initialValue;
            CurrentValue = initialValue;
        }

        public string PropertyName { get; }
        public dynamic InitialValue { get; }
        public dynamic CurrentValue { get; private set; }

        public bool Changed => InitialValue != CurrentValue;

        public dynamic Get()
        {
            return _getter.Invoke();
        }

        public void Set(dynamic value)
        {
            _setter.Invoke(value);
            CurrentValue = value;
        }
    }
}