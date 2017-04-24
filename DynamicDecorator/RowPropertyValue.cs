using System;

namespace DynamicDecorator
{
    public class RowPropertyValue
    {
        readonly Func<string, object> _getter;
        readonly Action<string, object> _setter;

        public RowPropertyValue(string propertyName, 
            Func<string, object> getter, 
            Action<string, object> setter,
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
            return _getter(PropertyName);
        }

        public void Set(dynamic value)
        {
            _setter(PropertyName, value);
            CurrentValue = value;
        }
    }
}