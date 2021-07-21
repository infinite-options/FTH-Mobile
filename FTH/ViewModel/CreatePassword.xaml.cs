using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FTH.ViewModel
{
    public partial class CreatePassword : ContentPage
    {
        public CreatePassword()
        {
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;
            Console.WriteLine("Width = " + width.ToString());
            Console.WriteLine("Height = " + height.ToString());

            InitializeComponent();
        }

        async void registrationClicked(System.Object sender, System.EventArgs e)
        {
            if (passEntry.Text == null || confirmPassEntry.Text == null)
            {
                await DisplayAlert("Oops", "Fill all of the fields before continuing.", "OK");
                return;
            }
            else if (passEntry.Text.ToString().Length < 8)
            {
                await DisplayAlert("Oops", "Your password must be at least 8 characters long.", "OK");
                return;
            }
            else if (passEntry.Text != confirmPassEntry.Text)
            {
                await DisplayAlert("Oops", "Your passwords don't match", "OK");
                return;
            }
            else Application.Current.MainPage = new CongratsPage();
        }

        void clickedSeePassword(System.Object sender, System.EventArgs e)
        {
            if (passEntry.IsPassword == true)
                passEntry.IsPassword = false;
            else passEntry.IsPassword = true;
        }

        void clickedSeeConfirmPassword(System.Object sender, System.EventArgs e)
        {
            if (confirmPassEntry.IsPassword == true)
                confirmPassEntry.IsPassword = false;
            else confirmPassEntry.IsPassword = true;
        }

        async void backClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
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
