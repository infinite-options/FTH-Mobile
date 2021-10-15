using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using FTH.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FTH.ViewModel
{
    public partial class FoodBanksMap : ContentPage
    {
        Dictionary<string, KeyValuePair<double, double>> latLongDict;
        public ObservableCollection<MappedFoodBanks> FoodBanks = new ObservableCollection<MappedFoodBanks>();

        public FoodBanksMap()
        {
            latLongDict = new Dictionary<string, KeyValuePair<double, double>>();
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;

            InitializeComponent();

            //dot ct
            Position position1 = new Position(37.236720, -121.887370);
            //corte de la reina

            //use the two most extreme coordinates
            var centerLat = (37.191082 + 37.236720) / 2;
            var centerLong = (-121.887412 - 121.887370) / 2;
            Position center = new Position(Math.Round(centerLat, 6), Math.Round(centerLong,6));
            Debug.WriteLine("center lat and long: " + Math.Round(centerLat, 6).ToString() + ", " + Math.Round(centerLong, 6).ToString());

            Position startPos = new Position(37.236720, -121.887370);

            map2.MapType = MapType.Street;
            var mapSpan = new MapSpan(center, 360 / (Math.Pow(2, 12)), 360 / (Math.Pow(2, 12)));

            Pin address1 = new Pin();
            address1.Label = "Food Bank 1";
            address1.Address = "1408 Dot Ct, San Jose, CA 95120";
            //address.Type = PinType.SearchResult;
            address1.Type = PinType.Place;
            address1.Position = position1;
            map2.MoveToRegion(mapSpan);
            map2.Pins.Add(address1);

            KeyValuePair<double, double> latLong = new KeyValuePair<double, double>(37.236720, -121.887370);
            latLongDict.Add(address1.Label, latLong);

            Pin address2 = new Pin();
            address2.Label = "Food Bank 2";
            address2.Address = "6123 Corte De La Reina, San Jose, CA 95120";
            address2.Type = PinType.Place;
            Position position2 = new Position(37.227140, -121.886930);
            address2.Position = position2;
            map2.Pins.Add(address2);

            latLong = new KeyValuePair<double, double>(37.227140, -121.886930);
            latLongDict.Add(address2.Label, latLong);

            Pin address3 = new Pin();
            address3.Label = "Food Bank 3";
            address3.Address = "6264 Tweedholm Ct";
            address3.Type = PinType.Place;
            Position position3 = new Position(37.191082, -121.887412);
            address3.Position = position3;
            map2.Pins.Add(address3);

            latLong = new KeyValuePair<double, double>(37.191082, -121.887412);
            latLongDict.Add(address3.Label, latLong);

            FoodBanks.Add(new MappedFoodBanks
            {
                name = "Food Bank 1",
                distance = "5 miles away",
                latitude = 37.236720, 
                longitude = -121.887370
            });

            FoodBanks.Add(new MappedFoodBanks
            {
                name = "Food Bank 2",
                distance = "3 miles away",
                latitude = 37.227140,
                longitude = -121.886930
            });

            FoodBanks.Add(new MappedFoodBanks
            {
                name = "Food Bank 3",
                distance = "10 miles away",
                latitude = 37.191082,
                longitude = -121.887412
            });

            foodBanksColl.ItemsSource = FoodBanks;
        }

        void clickedFbGeneral(System.Object sender, System.EventArgs e)
        {
            var title = ((Button)sender).Text;
            var latLongPair = latLongDict[title];
            Position position1 = new Position(latLongPair.Key, latLongPair.Value);
            var mapSpan = new MapSpan(position1, 360 / (Math.Pow(2, 12)), 360 / (Math.Pow(2, 12)));
            map2.MoveToRegion(mapSpan);

            scroller.ScrollToAsync(0, -50, true);
        }

        void backClicked(System.Object sender, System.EventArgs e)
        {
            Dictionary<string, string> holder = new Dictionary<string, string>();
            Application.Current.MainPage = new NavigationPage(new Registration("DIRECT", holder));
            //Navigation.PopAsync();
        }

        //guest menu functions
        void registerClicked(System.Object sender, System.EventArgs e)
        {
            Dictionary<string, string> holder = new Dictionary<string, string>();
            Application.Current.MainPage = new NavigationPage(new Registration("DIRECT", holder));
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
    }
}
