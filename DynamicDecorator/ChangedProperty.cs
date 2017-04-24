namespace DynamicDecorator
{
    public class ChangedProperty
    {
        public string PropertyName { get; private set; }
        public dynamic InitialValue { get; private set; }
        public dynamic CurrentValue { get; private set; }

        public ChangedProperty(string propertyName, dynamic initialValue, dynamic currentValue)
        {
            PropertyName = propertyName;
            InitialValue = initialValue;
            CurrentValue = currentValue;
        }
    }
}