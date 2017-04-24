using FluentAssertions;
using NSpec;

namespace DynamicDecorator.Tests
{
    internal class value_converter_spec : nspec
    {
        void string_to_int()
        {
            context["Given dto has integer property"] = () =>
            {
                before = () =>
                {
                    _dto = new {Age = 21};
                    _proxy = new DynamicDtoDecorator(_dto);
                };

                context["When setting value as valid string"] = () =>
                {
                    before = () => _proxy.Age = "30";

                    it["should automatically convert it into integer"] = () =>
                    {
                        ((int)_proxy.Age).Should().Be(30);
                    };
                };

                context["When setting value as invalid string"] = () =>
                {
                    before = () => _proxy.Age = "something invalid";

                    it["should indicate issue"] = () =>
                    {
                        // How to indicate it? Throw exception? Revert it to previous value?
                        "not implemented".Should().Be("Implemented");
                    };
                };
            };
        }

        object _dto;
        dynamic _proxy;
    }
}