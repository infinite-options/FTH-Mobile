using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FTH.ViewModel
{
    public partial class FoodBanksMap : ContentPage
    {
        public FoodBanksMap()
        {
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;

            InitializeComponent();

            //dot ct
            Position position1 = new Position(37.236720, -121.887370);
            //corte de la reina
            Position position2 = new Position(37.227140, -121.886930);
            var centerLat = (37.338207 + 37.227140) / 2;
            var centerLong = (-121.886330 + (-121.886930)) / 2;
            Position center = new Position(centerLat, centerLong);
            Debug.WriteLine("center lat and long: " + centerLat.ToString() + ", " + centerLong.ToString());
            
            map2.MapType = MapType.Street;
            var mapSpan = new MapSpan(position1, 360 / (Math.Pow(2, 14)), 360 / (Math.Pow(2, 14)));
            Pin address1 = new Pin();
            address1.Label = "Food Bank 1";
            address1.Address = "1408 Dot Ct, San Jose, CA 95120";
            //address.Type = PinType.SearchResult;
            address1.Type = PinType.Place;
            address1.Position = position1;
            map2.MoveToRegion(mapSpan);
            map2.Pins.Add(address1);

            //Pin address2 = new Pin();
            //address2.Label = "Food Bank 2";
            //address2.Address = "6123 Corte De La Reina, San Jose, CA 95120";
            //address1.Type = PinType.Place;
            //address1.Position = position2;
            //map2.Pins.Add(address2);
        }
    }
}
