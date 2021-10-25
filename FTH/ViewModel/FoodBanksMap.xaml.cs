using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using FTH.Model;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FTH.ViewModel
{
    public partial class FoodBanksMap : ContentPage
    {
        Dictionary<string, KeyValuePair<double, double>> latLongDict;
        public ObservableCollection<FoodBanks> FoodBanks = new ObservableCollection<FoodBanks>();
        double deviceLat;
        double deviceLong;
        Location deviceLoca;

        public FoodBanksMap()
        {
            deviceLat = 0;
            deviceLong = 0;
            latLongDict = new Dictionary<string, KeyValuePair<double, double>>();
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;

            InitializeComponent();
            _ = GetCurrentLocation();
            getNearbyBanks();

            //dot ct
            Position position1 = new Position(37.236720, -121.887370);
            //corte de la reina

            //use the two most extreme coordinates
            var centerLat = (37.191082 + 37.236720) / 2;
            var centerLong = (-121.887412 - 121.887370) / 2;
            //base it off of the user's location
            Position center = new Position(deviceLat, deviceLong);
            Debug.WriteLine("center lat and long: " + Math.Round(centerLat, 6).ToString() + ", " + Math.Round(centerLong, 6).ToString());

            //Position startPos = new Position(37.236720, -121.887370);

            //map2.MapType = MapType.Street;
            //var mapSpan = new MapSpan(center, 360 / (Math.Pow(2, 12)), 360 / (Math.Pow(2, 12)));

            //Pin address1 = new Pin();
            //address1.Label = "Food Bank 1";
            //address1.Address = "1408 Dot Ct, San Jose, CA 95120";
            ////address.Type = PinType.SearchResult;
            //address1.Type = PinType.Place;
            //address1.Position = position1;
            ////map2.MoveToRegion(mapSpan);
            //map2.Pins.Add(address1);

            //KeyValuePair<double, double> latLong = new KeyValuePair<double, double>(37.236720, -121.887370);
            //latLongDict.Add(address1.Label, latLong);

            //Pin address2 = new Pin();
            //address2.Label = "Food Bank 2";
            //address2.Address = "6123 Corte De La Reina, San Jose, CA 95120";
            //address2.Type = PinType.Place;
            //Position position2 = new Position(37.227140, -121.886930);
            //address2.Position = position2;
            //map2.Pins.Add(address2);

            //latLong = new KeyValuePair<double, double>(37.227140, -121.886930);
            //latLongDict.Add(address2.Label, latLong);

            //Pin address3 = new Pin();
            //address3.Label = "Food Bank 3";
            //address3.Address = "6264 Tweedholm Ct";
            //address3.Type = PinType.Place;
            //Position position3 = new Position(37.191082, -121.887412);
            //address3.Position = position3;
            //map2.Pins.Add(address3);

            //latLong = new KeyValuePair<double, double>(37.191082, -121.887412);
            //latLongDict.Add(address3.Label, latLong);

            //FoodBanks.Add(new MappedFoodBanks
            //{
            //    name = "Food Bank 1",
            //    distance = "5 miles away",
            //    latitude = 37.236720, 
            //    longitude = -121.887370
            //});


            //foodBanksColl.ItemsSource = FoodBanks;
        }

        CancellationTokenSource cts;

        async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Debug.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    //Tuple<double, double> latLong = new Tuple<double, double>(location.Latitude, location.Longitude);
                    deviceLat = location.Latitude;
                    deviceLong = location.Longitude;
                    //process the distance between the food bank and the phone
                    Location deviceLocation = new Location(location.Latitude, location.Longitude);
                    deviceLoca = deviceLocation;
                    //double miles = Location.CalculateDistance(fbLocation, deviceLocation, DistanceUnits.Miles);
                    //miles = Math.Round(miles, 1);
                    //Debug.WriteLine("distance between food bank and device: " + miles.ToString());
                    //bank.distance = miles.ToString() + " miles";

                }
                else
                {
                    await DisplayAlert("Oops", "We ran into a problem when trying to get your current location", "OK");
                    //bank.distance = "-----";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await DisplayAlert("Oops", "Location permissions are not supported on this device.", "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                await DisplayAlert("Oops", "Location permissions aren't enabled on this device.", "OK");
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                await DisplayAlert("Oops", "Please enable location permissions.", "OK");
            }
            catch (Exception ex)
            {
                // Unable to get location
                await DisplayAlert("Oops", "A problem occurred when trying to get this device's location.", "OK");
            }

            

            Position center = new Position(deviceLat, deviceLong);
            map2.MapType = MapType.Street;
            var mapSpan = new MapSpan(center, 360 / (Math.Pow(2, 11)), 360 / (Math.Pow(2, 11)));
            map2.MoveToRegion(mapSpan);

            //Pin address1 = new Pin();
            //address1.Label = "Current Location";
            ////address1.Address = "";
            ////address.Type = PinType.SearchResult;
            //address1.Type = PinType.Place;
            //Position position1 = new Position(deviceLat, deviceLong);
            //address1.Position = position1;
            //map2.Pins.Add(address1);

            //KeyValuePair<double, double> latLong = new KeyValuePair<double, double>(deviceLat, deviceLong);
            //latLongDict.Add(address1.Label, latLong);
        }

        public string GetXMLElement(XElement element, string name)
        {
            var el = element.Element(name);
            if (el != null)
            {
                return el.Value;
            }
            return "";
        }

        public string GetXMLAttribute(XElement element, string name)
        {
            var el = element.Attribute(name);
            if (el != null)
            {
                return el.Value;
            }
            return "";
        }

        async void getNearbyBanks()
        {
            try
            {
                await GetCurrentLocation();
                //new york lat and long: shouldn't have any food banks 42.380380, -76.874480
                string url = "https://c1zwsl05s5.execute-api.us-west-1.amazonaws.com/dev/api/v2/find_food_banks/20,miles," + deviceLat.ToString() + "," + deviceLong.ToString();
                Debug.WriteLine("nearby food banks url: " + url);
                var request3 = new HttpRequestMessage();
                request3.RequestUri = new Uri(url);
                request3.Method = HttpMethod.Get;
                var client2 = new HttpClient();
                HttpResponseMessage response = await client2.SendAsync(request3);
                var message = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("response from get nearby fb endpoint: " + response.ToString());
                Debug.WriteLine("message from get nearby fb endpoint: " + message.ToString());
                var data = JsonConvert.DeserializeObject<NearbyFoodBanks>(message);

                foreach (var foundBank in data.banks_found)
                {
                    FoodBanks newBank = new FoodBanks();
                    try
                    {

                        // Setting request for USPS API

                        Debug.WriteLine("INPUTS: ADDRESS1: {0}, ADDRESS2: {1}, CITY: {2}, STATE: {3}, ZIPCODE: {4}", foundBank.business_address, foundBank.business_unit, foundBank.business_city, foundBank.business_state, foundBank.business_zip);
                        XDocument requestDoc = new XDocument(
                            new XElement("AddressValidateRequest",
                            new XAttribute("USERID", "400INFIN1745"),
                            new XElement("Revision", "1"),
                            new XElement("Address",
                            new XAttribute("ID", "0"),
                            new XElement("Address1", foundBank.business_address),
                            new XElement("Address2", foundBank.business_unit != null ? foundBank.business_unit : ""),
                            new XElement("City", foundBank.business_city),
                            new XElement("State", foundBank.business_state),
                            new XElement("Zip5", foundBank.business_zip),
                            new XElement("Zip4", "")
                                 )
                             )
                         );

                        // This endpoint needs to change
                        var Addurl = "https://production.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + requestDoc;
                        var Addclient = new WebClient();
                        var Addresponse = Addclient.DownloadString(Addurl);
                        var xdoc = XDocument.Parse(Addresponse.ToString());
                        Debug.WriteLine("RESULT FROM USPS: " + xdoc);
                        foreach (XElement element in xdoc.Descendants("Address"))
                        {
                            if (GetXMLElement(element, "Error").Equals(""))
                            {
                                if ((GetXMLElement(element, "DPVConfirmation").Equals("Y") || GetXMLElement(element, "DPVConfirmation").Equals("S") || GetXMLElement(element, "DPVConfirmation").Equals("D")) && GetXMLElement(element, "Zip5").Equals(foundBank.business_zip) && GetXMLElement(element, "City").Equals(foundBank.business_city.ToUpper()))
                                {
                                    Geocoder geoCoder = new Geocoder();

                                    IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(foundBank.business_address + "," + foundBank.business_city + "," + foundBank.business_state);
                                    Position position = approximateLocations.FirstOrDefault();

                                    string bankLatitude = $"{position.Latitude}";
                                    string bankLongitude = $"{position.Longitude}";
                                    Debug.Write("bank latitude: " + bankLatitude);
                                    Debug.Write("bank longitude: " + bankLongitude);

                                    if (deviceLat == 0)
                                    {
                                        await GetCurrentLocation();
                                        Location fbLocation = new Location(Double.Parse(bankLatitude), Double.Parse(bankLongitude));

                                        double miles = Location.CalculateDistance(fbLocation, deviceLoca, DistanceUnits.Miles);
                                        miles = Math.Round(miles, 1);
                                        Debug.WriteLine("(else) distance between food bank and device: " + miles.ToString());
                                        newBank.distance = miles.ToString() + " miles";
                                    }
                                    else
                                    {
                                        Location fbLocation = new Location(Double.Parse(bankLatitude), Double.Parse(bankLongitude));

                                        double miles = Location.CalculateDistance(fbLocation, deviceLoca, DistanceUnits.Miles);
                                        miles = Math.Round(miles, 1);
                                        Debug.WriteLine("(else) distance between food bank and device: " + miles.ToString());
                                        newBank.distance = miles.ToString() + " miles";
                                    }
                                }
                                else
                                {
                                    newBank.distance = "-----";
                                }
                            }
                            else
                            {
                                newBank.distance = "-----";
                            }
                        }
                    }
                    catch
                    {
                        newBank.distance = "-----";
                    }

                    newBank.name = foundBank.business_name;
                    newBank.latitude = double.Parse(foundBank.business_latitude);
                    newBank.longitude = double.Parse(foundBank.business_longitude);
                    newBank.bankImg = foundBank.business_image;
                    newBank.business_uid = foundBank.business_uid;

                    try
                    {
                        newBank.itemLimit = int.Parse(foundBank.limit_per_person);
                    }
                    catch
                    {
                        newBank.itemLimit = 0;
                    }

                    if (foundBank.delivery != 0)
                    {
                        if (foundBank.delivery == 1)
                        {
                            newBank.delivery = true;
                            newBank.desc = "Delivery.";
                        }
                        else newBank.delivery = false;
                    }

                    if (foundBank.pick_up != 0)
                    {
                        if (foundBank.pick_up == 1)
                        {
                            newBank.pickup = true;
                            newBank.desc += " Pick-Up.";
                        }
                        else newBank.pickup = false;
                    }

                    if (foundBank.business_accepting_hours != null)
                    {
                        try
                        {
                            Debug.WriteLine("business hours: " + foundBank.business_accepting_hours);
                            var hoursdata = JsonConvert.DeserializeObject<AcceptingHours>(foundBank.business_accepting_hours);

                            if (hoursdata.Monday[0] == "N/A")
                                newBank.mondayHours = "Closed";
                            else newBank.mondayHours = hoursdata.Monday[0].Substring(0, 5) + " - " + hoursdata.Monday[1].Substring(0, 5);

                            if (hoursdata.Tuesday[0] == "N/A")
                                newBank.tuesdayHours = "Closed";
                            else newBank.tuesdayHours = hoursdata.Tuesday[0].Substring(0, 5) + " - " + hoursdata.Tuesday[1].Substring(0, 5);

                            if (hoursdata.Wednesday[0] == "N/A")
                                newBank.wednesdayHours = "Closed";
                            else newBank.wednesdayHours = hoursdata.Wednesday[0].Substring(0, 5) + " - " + hoursdata.Wednesday[1].Substring(0, 5);

                            if (hoursdata.Thursday[0] == "N/A")
                                newBank.thursdayHours = "Closed";
                            else newBank.thursdayHours = hoursdata.Thursday[0].Substring(0, 5) + " - " + hoursdata.Thursday[1].Substring(0, 5);

                            if (hoursdata.Friday[0] == "N/A")
                                newBank.fridayHours = "Closed";
                            else newBank.fridayHours = hoursdata.Friday[0].Substring(0, 5) + " - " + hoursdata.Friday[1].Substring(0, 5);

                            if (hoursdata.Saturday[0] == "N/A")
                                newBank.saturdayHours = "Closed";
                            else newBank.saturdayHours = hoursdata.Saturday[0].Substring(0, 5) + " - " + hoursdata.Saturday[1].Substring(0, 5);

                            if (hoursdata.Sunday[0] == "N/A")
                                newBank.sundayHours = "Closed";
                            else newBank.sundayHours = hoursdata.Sunday[0].Substring(0, 5) + " - " + hoursdata.Sunday[1].Substring(0, 5);
                        }
                        catch
                        {
                            newBank.mondayHours = "Closed";
                            newBank.tuesdayHours = "Closed";
                            newBank.wednesdayHours = "Closed";
                            newBank.thursdayHours = "Closed";
                            newBank.fridayHours = "Closed";
                            newBank.saturdayHours = "Closed";
                            newBank.sundayHours = "Closed";
                        }
                    }

                    if (foundBank.item_types != null)
                    {
                        Debug.WriteLine("item types: " + foundBank.item_types);
                        try
                        {
                            var typesdata = JsonConvert.DeserializeObject<Types>(foundBank.item_types);
                            newBank.fruits = typesdata.fruits;
                            newBank.vegetables = typesdata.vegetables;
                            newBank.meals = typesdata.meals;
                            newBank.desserts = typesdata.desserts;
                            newBank.beverages = typesdata.beverages;
                            newBank.dairy = typesdata.dairy;
                            newBank.snacks = typesdata.snacks;
                            newBank.cannedFoods = typesdata.cannedFoods;
                        }
                        catch
                        {
                            Debug.WriteLine("didn't work");
                            newBank.fruits = false;
                            newBank.vegetables = false;
                            newBank.meals = false;
                            newBank.desserts = false;
                            newBank.beverages = false;
                            newBank.dairy = false;
                            newBank.snacks = false;
                            newBank.cannedFoods = false;
                        }
                    }




                    FoodBanks.Add(newBank);

                    Pin address1 = new Pin();
                    address1.Label = foundBank.business_name;
                    if (foundBank.business_unit == "")
                        address1.Address = foundBank.business_address + ", " + foundBank.business_city + ", " +
                            foundBank.business_state + " " + foundBank.business_zip;
                    else address1.Address = foundBank.business_address + ", " + foundBank.business_unit + ", " + foundBank.business_city + ", " +
                            foundBank.business_state + " " + foundBank.business_zip;
                    //address.Type = PinType.SearchResult;
                    address1.Type = PinType.Place;
                    Position position1 = new Position(double.Parse(foundBank.business_latitude), double.Parse(foundBank.business_longitude));
                    address1.Position = position1;
                    map2.Pins.Add(address1);
                    KeyValuePair<double, double> latLong = new KeyValuePair<double, double>(double.Parse(foundBank.business_latitude), double.Parse(foundBank.business_longitude));
                    latLongDict.Add(address1.Label, latLong);


                    foodBanksColl.ItemsSource = FoodBanks;
                }
            }
            catch
            {
                if (FoodBanks.Count == 0)
                    await DisplayAlert("Oops", "We couldn't find any food banks around your current location.", "OK");
            }
        }

        void clickedFbGeneral(System.Object sender, System.EventArgs e)
        {
            var title = ((Button)sender).Text;
            var latLongPair = latLongDict[title];
            Position position1 = new Position(latLongPair.Key, latLongPair.Value);
            var mapSpan = new MapSpan(position1, 360 / (Math.Pow(2, 11)), 360 / (Math.Pow(2, 11)));
            map2.MoveToRegion(mapSpan);

            scroller.ScrollToAsync(0, -50, true);
        }

        void backClicked(System.Object sender, System.EventArgs e)
        {
            Dictionary<string, string> holder = new Dictionary<string, string>();
            Application.Current.MainPage = new NavigationPage(new Registration("DIRECT", holder));
            //Navigation.PopAsync();
        }

        void updateCenter(System.Object sender, System.EventArgs e)
        {
            Button button = (Button)sender;
            FoodBanks fb = button.BindingContext as FoodBanks;
            Position center = new Position(fb.latitude, fb.longitude);
            map2.MapType = MapType.Street;
            var mapSpan = new MapSpan(center, 360 / (Math.Pow(2, 11)), 360 / (Math.Pow(2, 11)));
            map2.MoveToRegion(mapSpan);
        }

        void fbClicked(System.Object sender, System.EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            FoodBanks fb = button.BindingContext as FoodBanks;
            Navigation.PushAsync(new FoodBackStore(fb, fb.name, fb.distance, fb.bankImg, fb.itemLimit, fb.business_uid));
        }

        //guest menu functions
        void registerClicked(System.Object sender, System.EventArgs e)
        {
            Dictionary<string, string> holder = new Dictionary<string, string>();
            Application.Current.MainPage = new NavigationPage(new Registration("DIRECT", holder));
        }

        void menuClicked(System.Object sender, System.EventArgs e)
        {
            if ((string)Application.Current.Properties["platform"] == "GUEST")
                openGuestMenuGrid.IsVisible = true;
            else openMenuGrid.IsVisible = true;
            //whiteCover.IsVisible = true;
            menu.IsVisible = false;
        }

        void openedMenuClicked(System.Object sender, System.EventArgs e)
        {
            if ((string)Application.Current.Properties["platform"] == "GUEST")
                openGuestMenuGrid.IsVisible = false;
            else openMenuGrid.IsVisible = false;
            //openMenuGrid.IsVisible = false;
            //whiteCover.IsVisible = false;
            menu.IsVisible = true;
        }

        void orderClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new FoodBanksMap();
            Navigation.PushAsync(new Filter());
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

        //logged in menu functions
        void profileClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new NavigationPage(new UserProfile());
            Navigation.PushAsync(new UserProfile());
        }

        //void menuClicked(System.Object sender, System.EventArgs e)
        //{
        //    openMenuGrid.IsVisible = true;
        //    //whiteCover.IsVisible = true;
        //    menu.IsVisible = false;
        //}

        //void openedMenuClicked(System.Object sender, System.EventArgs e)
        //{
        //    openMenuGrid.IsVisible = false;
        //    //whiteCover.IsVisible = false;
        //    menu.IsVisible = true;
        //}

        //void orderClicked(System.Object sender, System.EventArgs e)
        //{
        //    //Application.Current.MainPage = new FoodBanksMap();
        //    Navigation.PushAsync(new Filter());
        //}

        //void browseClicked(System.Object sender, System.EventArgs e)
        //{
        //    //Application.Current.MainPage = new FoodBanksMap();
        //    Navigation.PushAsync(new FoodBanksMap());
        //}

        void logoutClicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.Properties["platform"] = "GUEST";
            Application.Current.MainPage = new LoginPage();
        }
        //end of menu functions
    }
}
