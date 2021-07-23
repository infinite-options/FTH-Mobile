using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FTH.Model;
using Xamarin.Forms;
using static FTH.ViewModel.EditAddressPage;
using static FTH.ViewModel.FoodBackStore;

namespace FTH.ViewModel
{
    public partial class CheckoutPage : ContentPage
    {
        public ObservableCollection<StoreItem> itemsSource = new ObservableCollection<StoreItem>();

        public CheckoutPage()
        {
            InitializeComponent();

            SetFoodBank("Feeding Orange County", totalQuantity.ToString(), "businessImage");
            SetCartItems();
            SetPersonalInfo("Carlos", "Torres", "4158329643");
            SetFullAddress("1658 Sacramento Street", "San Francisco", "CA", "94109");
            SetFullDeliveryInfo("June 27, 2021", "10:00 AM - 12:00 PM");
        }

        void SetFoodBank(string name, string totalQuantity, string picture)
        {
            foodBankName.Text = name;
            totalCartItems.Text = totalQuantity + " Items";
            foodBankPicture.Source = picture;
        }

        void SetPersonalInfo(string firstName, string lastName, string phone)
        {
            userName.Text = firstName + " " + lastName;
            userPhone.Text = phone;
        }

        void SetFullAddress(string address, string city, string state, string zipcode)
        {
            if (addressToValidate != null && addressToValidate.isValidated)
            {
                userAddress.Text = addressToValidate.Street;
                userCityStateZipcode.Text = addressToValidate.City + ", " + addressToValidate.State + " " + addressToValidate.ZipCode;
            }
            else
            {
                userAddress.Text = address;
                userCityStateZipcode.Text = city + ", " + state + " " + zipcode;
            }
        }

        void SetFullDeliveryInfo(string date, string time)
        {
            deliveryDate.Text = date;
            deliveryTime.Text = time;
        }

        void SetCartItems()
        {
            itemsSource.Clear();

            foreach (string itemName in cart.Keys)
            {
                itemsSource.Add(cart[itemName]);
            }

            itemList.ItemsSource = itemsSource;

            float size = itemsSource.Count;
            float rows = 0;
            float d = 3;

            if (size != 0)
            {
                if (size > 0 && size <= 3)
                {
                    rows = 1;
                }
                else
                {
                    rows = size / d;
                }
            }

            double height = (double)(Math.Ceiling(rows) * 155);

            itemList.HeightRequest = height;
        }

        void NavigateToCartPage(System.Object sender, System.EventArgs e)
        {
            Navigation.PopAsync(false);
        }

        void NavigateToConfirmationPage(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new OrderConfirmationPage(), false);
        }

        void NavigateToEditAddressPage(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new EditAddressPage(), false);
        }

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
    }
}
