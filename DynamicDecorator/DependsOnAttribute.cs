using System;

namespace DynamicDecorator
{
    public class DependsOnAttribute : Attribute
    {
        public string PropertyNames { get; set; }

        public DependsOnAttribute(string propertyNames)
        {
            PropertyNames = propertyNames;
        }
    }
}
