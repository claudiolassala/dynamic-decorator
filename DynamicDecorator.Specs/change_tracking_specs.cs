using System.Collections.Generic;
using FluentAssertions;
using SubSpec;

namespace DynamicProxy.Specs
{
    public class change_tracking_specs
    {
        dynamic _proxy;
        DummyDto _dto;
        string _initialValuePropertyA = "Initial A";
        string _initialValuePropertyB = "Initial B";
        List<ChangedProperty> _changedProperties;
            
        [Specification]
        public void change_tracking()
        {
            "Given proxy instance with two properties And one of the properties is set to a value different its initial value"
                .Context(() =>
                {
                    _dto = new DummyDto
                    {
                        PropertyA = _initialValuePropertyA, PropertyB = _initialValuePropertyB
                    };
                    _proxy = new DynamicNotifyPropertyChangedProxy(_dto);
                    _proxy.PropertyA = "New Value";
                });
            "When asked to retrieve properties that have changed from their initial value"
                .Do(() =>
                {
                    _changedProperties = _proxy.GetChanges();
                });
            "Then it should receive information about each property that has changed"
                .Assert(() =>
                {
                    _changedProperties.Count.Should().Be(1);
                    _changedProperties[0].PropertyName.Should().Be("PropertyA");
                    _changedProperties[0].InitialValue.Should().Be(_initialValuePropertyA);
                    _changedProperties[0].CurrentValue.Should().Be("New Value");
                });
        }
    }

    public class DummyDto
    {
        public string PropertyA { get; set; }
        public string PropertyB { get; set; }
    }
}