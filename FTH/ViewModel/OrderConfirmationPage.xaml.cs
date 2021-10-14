using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace FTH.ViewModel
{
    public partial class OrderConfirmationPage : ContentPage
    {
        public OrderConfirmationPage()
        {
            InitializeComponent();

            var orderType = "DELIVERY";

            if (orderType == "DELIVERY")
            {
                SetFoodBankNameTimeAddressDelivery("Feeding Orange County", "10:00 AM - 12:00 PM", "123 Any Street Santa Clara, CA 95120.");
            }
            else if (orderType == "PICKUP")
            {
                SetFoodBankNameTimeAddressPickUp("Feeding Orange County", "12:00 PM", "July 27th.");
            }
        }

        void SetFoodBankNameTimeAddressDelivery(string name, string time, string fullAddress)
        {
            foodBankName.Text = "Your order from " + name + " will be delivered\nat ";
            deliveryTime.Text = time + " \n";
            deliveryAddress.Text = fullAddress;
        }

        void SetFoodBankNameTimeAddressPickUp(string name, string time, string date)
        {
            foodBankName.Text = "Your order from " + name + " will be ready for pickup\nat ";
            deliveryTime.Text = time + " ";
            deliveryAddress.Text = date;
        }

        void NavigateBackToCheckoutPage(System.Object sender, System.EventArgs e)
        {
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

        void filterClicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Filter());
        }

        void browseClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new FoodBanksMap();
            Navigation.PushAsync(new FoodBanksMap());
        }

        void loginClicked(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("logout clicked");
            Application.Current.MainPage = new LoginPage();
        }

        //end of menu functions
    }
}
