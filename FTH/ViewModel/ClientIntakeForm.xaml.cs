using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using FTH.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FTH.ViewModel
{
    public partial class ClientIntakeForm : ContentPage
    {
        Address addr;
        public ObservableCollection<HouseholdMembers> MembersColl = new ObservableCollection<HouseholdMembers>();
        int memberNum;

        public ClientIntakeForm()
        {
            memberNum = 1;
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;
            Console.WriteLine("Width = " + width.ToString());
            Console.WriteLine("Height = " + height.ToString());
            addr = new Address();

            InitializeComponent();

            MembersColl.Add(new HouseholdMembers
            {
                MemberTitle = "Member 1:"
            });

            membersCollView.ItemsSource = MembersColl;

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

        void backClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new MainPage();
            Navigation.PopAsync();
        }

        //menu functions
        void profileClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new NavigationPage(new UserProfile());
            Navigation.PushAsync(new UserProfile());
        }

        void menuClicked(System.Object sender, System.EventArgs e)
        {
            openMenuGrid.IsVisible = true;
            menu.IsVisible = false;
        }

        void openedMenuClicked(System.Object sender, System.EventArgs e)
        {
            openMenuGrid.IsVisible = false;
            menu.IsVisible = true;
        }

        void browseClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new FoodBanksMap();
            Navigation.PushAsync(new FoodBanksMap());
        }

        void loginClicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }

        //end of menu functions

        void submitButton_Clicked(System.Object sender, System.EventArgs e)
        {
            foreach (HouseholdMembers houseMem in MembersColl)
            {
                Debug.WriteLine("title: " + houseMem.MemberTitle);
                Debug.WriteLine("name: " + houseMem.MemberName);
                Debug.WriteLine("age: " + houseMem.MemberAge);
                Debug.WriteLine("relationship: " + houseMem.MemberRelationship);
            }

            Navigation.PushAsync(new CheckoutPage());
        }

        void addMemberClicked(System.Object sender, System.EventArgs e)
        {
            memberNum++;
            
            MembersColl.Add(new HouseholdMembers
            {
                MemberTitle = "Member " + memberNum.ToString() + ":"
            });

            membersCollView.ItemsSource = MembersColl;
            membersCollView.HeightRequest += 180;
        }
    }
}
