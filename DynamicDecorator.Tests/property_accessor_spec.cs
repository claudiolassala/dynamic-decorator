using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using FluentAssertions;
using NSpec;

namespace DynamicDecorator.Tests
{
    class property_accessor_spec : nspec
    {
        void accessing_property()
        {
            before = () =>
            {
                var propertyName = "SomeProperty";

                var someViewModel = new SomeViewModel { SomeProperty = initialValue };

                var getter = new Func<object>(() => someViewModel.SomeProperty);
                var setter = new Action<object>(value => someViewModel.SomeProperty = value.ToString());
                _propertyAccessor = new PropertyAccessor("SomeProperty", getter, setter, initialValue);
            };

            it["gets and sets the value"] = () =>
            {
                (_propertyAccessor.Get() as string).Should().Be(initialValue);

                _propertyAccessor.Set(newValue);

                (_propertyAccessor.Get() as string).Should().Be(newValue);
            };
        }

        const string initialValue = "Some value";
        const string newValue = "New value";
        PropertyAccessor _propertyAccessor;

        class SomeViewModel
        {
            public string SomeProperty { get; set; }
        }
    }
}