using FluentAssertions;
using NSpec;

namespace DynamicDecorator.Tests
{
    internal class indexer_spec : nspec
    {
        private object _dto;

        private DynamicDtoDecorator _decorator;

        private void when_accessing_indexer()
        {
            before = () =>
            {
                _dto = new {FirstName = "Scott", LastName = "Summers"};
                _decorator = new DynamicDtoDecorator(_dto);
            };

            it["allows access to the properties"] = () =>
            {
                (_decorator["FirstName"] as string).Should().Be("Scott");
                (_decorator["LastName"] as string).Should().Be("Summers");
            };
        }
    }
}