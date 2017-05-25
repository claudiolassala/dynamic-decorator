using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DynamicDecorator;

namespace Sample.Wpf
{
    public partial class InvoiceView : Window
    {
        public InvoiceView()
        {
            InitializeComponent();
            DataContext = new DynamicDtoDecorator(new InvoiceViewModel());

            ConfigureDataGrid();
        }

        private void ConfigureDataGrid()
        {
            var modelVisuals = new InvoiceItemVisuals();
            ConfigureCellBackgrounds(modelVisuals);
        }

        private void ConfigureCellBackgrounds(InvoiceItemVisuals modelVisuals)
        {
            var cellsBackgrounds = modelVisuals.GetCellBackgrounds();

            cellsBackgrounds.ForEach(cellDefinition =>
            {
                var column = (DataGridTextColumn) ItemsGrid.Columns
                    .Single(c => c.Header.ToString() == cellDefinition.PropertyName);
                var style = new Style(typeof(TextBlock));

                cellDefinition.TriggerDefinitions.ForEach(trigger =>
                {
                    var binding = $"{cellDefinition.PropertyName}_Background";
                    var propertyValue = trigger.Key;
                    var propertyBackground = trigger.Value;
                    var dataTrigger = new DataTrigger
                    {
                        Binding = new Binding(binding),
                        Value = propertyValue
                    };
                    dataTrigger.Setters.Add(new Setter
                    {
                        Property = TextBlock.BackgroundProperty,
                        Value = propertyBackground
                    });
                    style.Triggers.Add(dataTrigger);
                });

                column.ElementStyle = style;
            });
        }

        private void UppercaseAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (dynamic item in (DataContext as DynamicDtoDecorator).GetDto<InvoiceViewModel>().Items)
            {
                item.ProductName = item.ProductName.ToUpper();
                item.Quantity += 1;
            }
        }
    }
}