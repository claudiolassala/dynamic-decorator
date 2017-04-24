using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSpec;

namespace DynamicDecorator.Tests
{
    internal class change_tracking_spec : nspec
    {
        void property_changed_from_its_original_value()
        {
            before = () =>
            {
                set_up_proxy();

                _proxy.PropertyA = NewValueForPropertyA;
                _changedProperties = (_proxy as DynamicDtoDecorator).GetChanges().ToList();
            };

            it["retrieves information about the properties that were changed"] = () =>
            {
                _changedProperties.Count.Should().Be(1);
                var changedProperty = _changedProperties[0];
                changedProperty.PropertyName.Should().Be("PropertyA");
                (changedProperty.InitialValue as string).Should().Be(InitialValuePropertyA);
                (changedProperty.CurrentValue as string).Should().Be(NewValueForPropertyA);
            };
        }

        const string InitialValuePropertyA = "Initial A";
        const string InitialValuePropertyB = "Initial B";
        const string NewValueForPropertyA = "New Value";
        List<ChangedProperty> _changedProperties;
        DummyDto _dto;
        dynamic _proxy;

        private void set_up_proxy()
        {
            _dto = new DummyDto
            {
                PropertyA = InitialValuePropertyA,
                PropertyB = InitialValuePropertyB
            };
            _proxy = new DynamicDtoDecorator(_dto);
        }

        public class DummyDto
        {
            public string PropertyA { get; set; }
            public string PropertyB { get; set; }
        }
    }
}