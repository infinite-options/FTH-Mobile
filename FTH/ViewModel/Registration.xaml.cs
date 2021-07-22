using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Xml.Linq;
using FTH.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FTH.ViewModel
{
    public class idType
    {
        public string type { get; set; }

    }


    public partial class Registration : ContentPage
    {
        ObservableCollection<idType> idTypes = new ObservableCollection<idType>();
        double origHeight;
        Address addr;

        public Registration()
        {
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;
            Console.WriteLine("Width = " + width.ToString());
            Console.WriteLine("Height = " + height.ToString());
            addr = new Address();
            InitializeComponent();
            origHeight = idTypeFrame.Height;
            
            idTypes.Add(new idType
            {
                type = "Social Security"
            });
            idTypes.Add(new idType
            {
                type = "Driver's License"
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

        async void registrationClicked(System.Object sender, System.EventArgs e)
        {
            if (FNameEntry.Text == null || LNameEntry.Text == null || phoneEntry.Text == null || affilEntry.Text == null ||
                idTypeButton.Text == "ID Type ᐯ" || idNumEntry.Text == null || AddressEntry.Text == null || CityEntry.Text == null || StateEntry.Text == null || ZipEntry.Text == null)
            {
                await DisplayAlert("Oops", "Fill all of the fields before continuing.", "OK");
                return;
            }
            else if (idNumEntry.Text == "invalid" || idNumEntry.Text == "Invalid")
            {
                var result = await DisplayAlert("Sorry!", "You are unable to register. Would you still like to see food banks in your area?", "Yes", "No");
                if (result)
                {
                    await Navigation.PushAsync(new FoodBanksMap());
                    return;
                }
                else return;
            }
            else
            {
                var aptText = AptEntry.Text;
                if (aptText == null)
                    aptText = "";

                // Setting request for USPS API
                XDocument requestDoc = new XDocument(
                    new XElement("AddressValidateRequest",
                    new XAttribute("USERID", "400INFIN1745"),
                    new XElement("Revision", "1"),
                    new XElement("Address",
                    new XAttribute("ID", "0"),
                    new XElement("Address1", AddressEntry.Text.ToString().Trim()),
                    new XElement("Address2", aptText),
                    new XElement("City", CityEntry.Text.ToString().Trim()),
                    new XElement("State", StateEntry.Text.ToString().Trim()),
                    new XElement("Zip5", ZipEntry.Text.ToString().Trim()),
                    new XElement("Zip4", "")
                         )
                     )
                 );
                var url = "https://production.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + requestDoc;
                Console.WriteLine(url);
                var client2 = new WebClient();
                var response2 = client2.DownloadString(url);

                var xdoc = XDocument.Parse(response2.ToString());
                Console.WriteLine("xdoc begin");
                Console.WriteLine(xdoc);


                string latitude = "0";
                string longitude = "0";
                foreach (XElement element in xdoc.Descendants("Address"))
                {
                    if (GetXMLElement(element, "Error").Equals(""))
                    {
                        //  && GetXMLElement(element, "Zip5").Equals(ZipEntry.Text.Trim()) && GetXMLElement(element, "City").Equals(CityEntry.Text.ToUpper().Trim())
                        if (GetXMLElement(element, "DPVConfirmation").Equals("Y") ||
                            GetXMLElement(element, "DPVConfirmation").Equals("S")) // Best case
                        {
                            // Get longitude and latitide because we can make a deliver here. Move on to next page.
                            // Console.WriteLine("The address you entered is valid and deliverable by USPS. We are going to get its latitude & longitude");
                            //GetAddressLatitudeLongitude();
                            Geocoder geoCoder = new Geocoder();

                            //IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(AddressEntry.Text.Trim() + "," + CityEntry.Text.Trim() + "," + StateEntry.Text.Trim());
                            //Position position = approximateLocations.FirstOrDefault();

                            //latitude = $"{position.Latitude}";
                            //longitude = $"{position.Longitude}";

                            //map.MapType = MapType.Street;
                            //var mapSpan = new MapSpan(position, 0.001, 0.001);

                            //Pin address = new Pin();
                            //address.Label = "Delivery Address";
                            //address.Type = PinType.SearchResult;
                            //address.Position = position;

                            //map.MoveToRegion(mapSpan);
                            //map.Pins.Add(address);
                        }
                        else if (GetXMLElement(element, "DPVConfirmation").Equals("D"))
                        {
                            await DisplayAlert("Missing Info", "Please enter your unit/apartment number into the appropriate field.", "OK");
                            return;
                        }
                        else
                        {
                            await DisplayAlert("Invalid Address", "The address you entered couldn't be confirmed. Please enter another one.", "OK");
                            return;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Invalid Address", "The address you entered couldn't be confirmed. Please enter another one.", "OK");
                        return;
                    }
                }
            }
            await Navigation.PushAsync(new CreatePassword()); //Application.Current.MainPage = new CreatePassword();
        }

        public static string GetXMLElement(XElement element, string name)
        {
            var el = element.Element(name);
            if (el != null)
            {
                return el.Value;
            }
            return "";
        }

        void backClicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
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

        //menu functions
        void registerClicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Registration());
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

        void loginClicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }

        //end of menu functions
    }
}
