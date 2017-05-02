using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using FluentAssertions;
using NSpec;

namespace DynamicDecorator.Tests
{
    internal class property_changed_spec : nspec
    {
        void changing_property()
        {
            before = () =>
            {
                set_up_proxy();

                (_proxy as INotifyPropertyChanged).PropertyChanged += (o, ea) => _propertyChangedEventWasRaised = true;

                _proxy.PropertyA = NewValueForPropertyA;
            };

            it["raises the PropertyChanged event"] = () =>
            {
                _propertyChangedEventWasRaised.Should().BeTrue();
            };
        }

        const string NewValueForPropertyA = "New Value";
        bool _propertyChangedEventWasRaised = false;
        dynamic _proxy;

        private void set_up_proxy()
        {
            _proxy = new DynamicDtoDecorator(new DummyDto());
        }

        public class DummyDto
        {
            public string PropertyA { get; set; }
        }
    }
}