using System;

namespace DynamicDecorator
{
    public class DependsOnAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public DependsOnAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
