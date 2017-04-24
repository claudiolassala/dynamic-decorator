using System;
using System.Collections.Generic;

namespace Sample.Wpf
{
    public class CellBackgroundDefinition<T>
    {
        public Func<T, dynamic> Func { get; }
        public string PropertyName { get; }
        public string[] DependsOn { get; }
        public Dictionary<object, object> TriggerDefinitions { get; }

        public CellBackgroundDefinition(string propertyName, string[] dependsOn, Func<T, dynamic> func, Dictionary<object, object> triggerDefinitions)
        {
            Func = func;
            PropertyName = propertyName;
            DependsOn = dependsOn;
            TriggerDefinitions = triggerDefinitions;
        }
    }
}