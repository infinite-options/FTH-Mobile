using System;
using System.ComponentModel;

namespace FTH.Model
{
    public class StoreItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string image { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public string type { get; set; }

        public int quantityUpdate
        {
            set
            {
                quantity = value;
                PropertyChanged(this, new PropertyChangedEventArgs("quantity"));
            }
        }
    }
}
