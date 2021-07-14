using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

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

        public Registration()
        {
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;
            Console.WriteLine("Width = " + width.ToString());
            Console.WriteLine("Height = " + height.ToString());

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

        async void registerClicked(System.Object sender, System.EventArgs e)
        {
            if (FNameEntry.Text == null || LNameEntry.Text == null || phoneEntry.Text == null || affilEntry.Text == null ||
                idTypeButton.Text == "ID Type ᐯ" || idNumEntry.Text == null || addressEntry.Text == null || cityEntry.Text == null || zipEntry.Text == null)
            {
                await DisplayAlert("Oops", "Fill all of the fields before continuing.", "OK");
                return;
            }
            else if (idNumEntry.Text == "invalid" || idNumEntry.Text == "Invalid")
            {
                var result = await DisplayAlert("Sorry!", "You are unable to register. Would you still like to see food banks in your area?", "Yes", "No");
                if (result)
                {
                    Application.Current.MainPage = new FoodBanksMap();
                    return;
                }
                else return;
            }
            else Application.Current.MainPage = new CongratsPage();
        }

        void backClicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }

        void browseClicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new FoodBanksMap();
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
    }
}
