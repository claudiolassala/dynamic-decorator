using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using DynamicDecorator;

namespace Sample.Wpf
{
    public class InvoiceViewModel
    {
        public InvoiceViewModel()
        {
            Number = "1234";

            var decoratedItems = GetItems().Select(item =>
            {
                var decoratedDto = new DynamicDtoDecorator(item);
                var visuals = new InvoiceItemVisuals();
                visuals.InjectInto(decoratedDto);
                return decoratedDto;
            });

            Items = CollectionViewSource.GetDefaultView(new ObservableCollection<DynamicDtoDecorator>(decoratedItems));
        }

        private static List<InvoiceItemViewModel> GetItems()
        {
            return new List<InvoiceItemViewModel>
            {
                new InvoiceItemViewModel
                {
                    ProductName = "Banana",
                    Category = "1",
                    Price = 3.50M,
                    Quantity = 3
                },
                new InvoiceItemViewModel
                {
                    ProductName = "Apple",
                    Category = "2",
                    Price = 4.75M,
                    Quantity = 2
                },
                new InvoiceItemViewModel
                {
                    ProductName = "Orange",
                    Category = "2",
                    Price = 2.80M,
                    Quantity = 4
                }
            };
        }

        public string Number { get; set; }

        public ICollectionView Items { get; private set; }
    }
}