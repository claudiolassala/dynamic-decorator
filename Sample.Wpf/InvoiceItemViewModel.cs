using DynamicDecorator;

namespace Sample.Wpf
{
    public class InvoiceItemViewModel
    {
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        [DependsOn("Price, Quantity")]
        public decimal Total => Price * Quantity;
    }
}