using System;
using System.ComponentModel;

namespace FTH.Model
{
    public class FoodBanks : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string name { get; set; }
        bool hoursVisible;
        public bool HoursVisible
        {
            set
            {
                if (hoursVisible != value)
                {
                    hoursVisible = value;
                    OnPropertyChanged("HoursVisible");
                }
            }
            get
            {
                return hoursVisible;
            }

        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
