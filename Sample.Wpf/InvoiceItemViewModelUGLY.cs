using System.ComponentModel;

namespace Sample.Wpf
{
    public class InvoiceItemViewModelUGLY : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _productName;

        public string ProductName
        {
            get { return _productName; }
            set
            {
                if (_productName != value)
                {
                    _productName = value;
                    OnPropertyChanged("ProductName");
                }
            }
        }
    }

    public class InvoiceItemViewModelBETTER
    {
        public string ProductName { get; set; }
    }
}