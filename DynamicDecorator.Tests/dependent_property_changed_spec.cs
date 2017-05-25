using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using FluentAssertions;
using NSpec;

namespace DynamicDecorator.Tests
{
    internal class dependent_property_changed_spec : nspec
    {
        void changing_property_dependent_upon()
        {
            before = () =>
            {
                set_up_proxy();

                (_proxy as INotifyPropertyChanged).PropertyChanged += (o, ea) =>
                {
                    if (ea.PropertyName == "PropertyC")
                        _propertyChangedEventCount++;
                };
             
                _proxy.PropertyA = NewValueForPropertyA;
                _proxy.PropertyB = NewValueForPropertyB;
            };

            it["raises the PropertyChanged event"] = () =>
            {
                _propertyChangedEventCount.Should().Be(2);
            };
        }

        const string NewValueForPropertyA = "New Value A";
        const string NewValueForPropertyB = "New Value B";
        int _propertyChangedEventCount = 0;
        dynamic _proxy;

        private void set_up_proxy()
        {
            _proxy = new DynamicDtoDecorator(new DummyDto());
        }

        public class DummyDto
        {
            public string PropertyA { get; set; }
            public string PropertyB { get; set; }

            [DependsOn("PropertyA, PropertyB")]
            public string PropertyC { get; set; }
        }
    }
}