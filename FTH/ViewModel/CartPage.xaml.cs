using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FTH.Model;
using Xamarin.Forms;
using static FTH.ViewModel.FoodBackStore;

namespace FTH.ViewModel
{
    public partial class CartPage : ContentPage
    {
        public ObservableCollection<StoreItem> itemsSource = new ObservableCollection<StoreItem>();
        public Dictionary<StoreItem, int> itemAmounts = new Dictionary<StoreItem, int>();
        int threshold;

        public CartPage(int storeThreshold, Dictionary<StoreItem, int> itmAmts)
        {
            threshold = storeThreshold;
            itemAmounts = itmAmts;
            InitializeComponent();

            SetFoodBank("Feeding Orange County", "5.3 miles away", "businessImage");
            SetCartItems();

        }

        void SetCartItems()
        {
            itemsSource.Clear();

            foreach (string itemName in cart.Keys)
            {
                itemsSource.Add(cart[itemName]);
            }

            cartItemList.ItemsSource = itemsSource;
        }

        void SetFoodBank(string name, string distance, string picture)
        {
            foodBankName.Text = name;
            foodBankDistance.Text = distance;
            foodBankPicture.Source = picture;
        }

        void NavigateToFoodBankStore(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new NavigationPage(new FoodBackStore());
            Navigation.PopAsync();
        }

        void DeleteItem(System.Object sender, System.EventArgs e)
        {
            var swipe = (SwipeItem)sender;
            var recognizer = swipe.CommandParameter;
            var item = (StoreItem)recognizer;

            totalQuantity -= item.quantity;
            item.quantityUpdate = 0;

            if (cart.ContainsKey(item.name))
            {
                cart.Remove(item.name);
            }

            if (itemAmounts.ContainsKey(item))
                itemAmounts.Remove(item);

            SetCartItems();
        }

        void AddItemToCart(System.Object sender, System.EventArgs e)
        {
            var label = (Label)sender;
            var recognizer = (TapGestureRecognizer)label.GestureRecognizers[0];
            var item = (StoreItem)recognizer.CommandParameter;

            if (totalQuantity < threshold)
            {
                item.quantityUpdate = item.quantity + 1;

                totalQuantity = totalQuantity + 1;

                if (!cart.ContainsKey(item.name))
                {
                    cart.Add(item.name, item);
                }
                else
                {
                    cart[item.name] = item;
                }

                if (itemAmounts.ContainsKey(item))
                    itemAmounts[item]++;
                else itemAmounts[item] = 1;
            }

            CheckTotalQuantity();
        }

        void RemoveItemFromCart(System.Object sender, System.EventArgs e)
        {
            var label = (Label)sender;
            var recognizer = (TapGestureRecognizer)label.GestureRecognizers[0];
            var item = (StoreItem)recognizer.CommandParameter;

            if (item.quantity != 0)
            {
                if (itemAmounts.ContainsKey(item) && itemAmounts[item] == 1)
                    itemAmounts.Remove(item);
                else itemAmounts[item]--;

                item.quantityUpdate = item.quantity - 1;

                totalQuantity = totalQuantity - 1;

                if (item.quantity != 0)
                {
                    if (!cart.ContainsKey(item.name))
                    {
                        cart.Add(item.name, item);
                    }
                    else
                    {
                        cart[item.name] = item;
                    }
                }
                else
                {
                    if (cart.ContainsKey(item.name))
                    {
                        cart.Remove(item.name);
                    }
                }
            }

            CheckTotalQuantity();
        }

        void CheckTotalQuantity()
        {
            if (totalQuantity == threshold)
            {
                DisplayAlert("Oops", "You have reached the maximum number of items", "OK");
            }
        }

        void NavigateToCheckoutPage(System.Object sender, System.EventArgs e)
        {
            if (itemAmounts.Count == 0)
                DisplayAlert("Oops", "Choose items before checking out", "OK");
            else Navigation.PushAsync(new ClientIntakeForm(itemAmounts), false);
        }

        void backClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new MainPage();
            Navigation.PopAsync(false);
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
