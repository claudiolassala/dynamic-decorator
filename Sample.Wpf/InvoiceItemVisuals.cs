using System.Collections.Generic;
using System.Windows.Media;
using DynamicDecorator;

namespace Sample.Wpf
{
    public class InvoiceItemVisuals
    {
        public void InjectInto(DynamicDtoDecorator decoratedDto)
        {
            GetCellBackgrounds().ForEach(definition =>
            {
                var backgroundProperty = $"{definition.PropertyName}_Background";
                decoratedDto.RegisterPropertyDependencies(backgroundProperty, definition.DependsOn);
                decoratedDto.Register(backgroundProperty, () => definition.Func(decoratedDto.GetDto<InvoiceItemViewModel>()), newValue => {});
            });
        }

        private List<CellBackgroundDefinition<InvoiceItemViewModel>> _backgroundDefinitions;

        public IEnumerable<CellBackgroundDefinition<InvoiceItemViewModel>> GetCellBackgrounds()
        {
            yield return new CellBackgroundDefinition<InvoiceItemViewModel>("ProductName",
                new [] { "ProductName" }, 
                vm => vm.ProductName == "Banana",
                new Dictionary<object, object>
                {
                    {"true", Brushes.Green},
                    {"false", Brushes.Yellow}
                });
            yield return new CellBackgroundDefinition<InvoiceItemViewModel>("Category",
                new[] { "Category" },
                vm => vm.Category,
                new Dictionary<object, object>
                {
                    {"1", Brushes.LightBlue},
                    {"2", Brushes.LightGray},
                    {"3", Brushes.LightSalmon}
                });
            yield return new CellBackgroundDefinition<InvoiceItemViewModel>("Total",
                new[] { "Category" },
                vm => vm.Category,
                new Dictionary<object, object>
                {
                    {"1", Brushes.Red},
                    {"2", Brushes.Pink},
                    {"3", Brushes.Purple}
                });
        }
    }
}