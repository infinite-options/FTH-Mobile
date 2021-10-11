using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FTH.Model;
using Xamarin.Forms;

namespace FTH.ViewModel
{
    public partial class UpdateProfile : ContentPage
    {
        Address addr;
        ObservableCollection<idType> idTypes = new ObservableCollection<idType>();

        public UpdateProfile()
        {
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
            addr = new Address();

            idTypes.Add(new idType
            {
                type = "Social Security"
            });
            idTypes.Add(new idType
            {
                type = "Driver License"
            });
            idTypes.Add(new idType
            {
                type = "Passport"
            });
            idTypes.Add(new idType
            {
                type = "Real ID"
            });
            idList.ItemsSource = idTypes;
        }

        void idTypeClicked(System.Object sender, System.EventArgs e)
        {
            if (idList.IsVisible == true)
                idList.IsVisible = false;
            else idList.IsVisible = true;

            idTypeFrame.HeightRequest = 32;
            idNum.HeightRequest = 32;
        }

        void idListItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            idList.IsVisible = false;
            idTypeButton.Text = ((idType)idList.SelectedItem).type;

            idTypeFrame.HeightRequest = 32;
            idNum.HeightRequest = 32;
        }

        //address autocomplete start
        // Auto-complete
        private ObservableCollection<AddressAutocomplete> _addresses;
        public ObservableCollection<AddressAutocomplete> Addresses
        {
            get => _addresses ?? (_addresses = new ObservableCollection<AddressAutocomplete>());
            set
            {
                if (_addresses != value)
                {
                    _addresses = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _addressText;
        public string AddressText
        {
            get => _addressText;
            set
            {
                if (_addressText != value)
                {
                    _addressText = value;
                    OnPropertyChanged();
                }
            }
        }

        private async void OnAddressChanged(object sender, TextChangedEventArgs eventArgs)
        {
            addressList.IsVisible = true;
            UnitCity.IsVisible = false;
            StateZip.IsVisible = false;
            //UnitCityState.IsVisible = false;
            //ZipPhone.IsVisible = false;
            addressList.ItemsSource = await addr.GetPlacesPredictionsAsync(AddressEntry.Text);
            //addr.OnAddressChanged(addressList, Addresses, _addressText);
        }

        private void addressEntryFocused(object sender, EventArgs eventArgs)
        {
            //addr.addressEntryFocused(addressList, new Grid[] { UnitCityState, ZipPhone });
        }

        private void addressEntryUnfocused(object sender, EventArgs eventArgs)
        {
            addr.addressEntryUnfocused(addressList, new Grid[] { UnitCity, StateZip });
        }

        private void addressSelected(System.Object sender, System.EventArgs e)
        {
            addr.addressSelected(addressList, new Grid[] { UnitCity, StateZip }, AddressEntry, CityEntry, StateEntry, ZipEntry);
            addressList.IsVisible = false;
            UnitCity.IsVisible = true;
            StateZip.IsVisible = true;
        }
        //address autocomplete end

        //menu functions
        void profileClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new NavigationPage(new UserProfile());
            Navigation.PushAsync(new UserProfile());
        }

        void menuClicked(System.Object sender, System.EventArgs e)
        {
            openMenuGrid.IsVisible = true;
            //whiteCover.IsVisible = true;
            menu.IsVisible = false;
        }

        void openedMenuClicked(System.Object sender, System.EventArgs e)
        {
            openMenuGrid.IsVisible = false;
            //whiteCover.IsVisible = false;
            menu.IsVisible = true;
        }

        void browseClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new FoodBanksMap();
            Navigation.PushAsync(new FoodBanksMap());
        }

        void logoutClicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }
        //end of menu functions
    }
}
