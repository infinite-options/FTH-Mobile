using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using FTH.Model;
using Xamarin.Forms;

namespace FTH.ViewModel
{
    public partial class FoodBackStore : ContentPage
    {
        public ObservableCollection<StoreItem> itemSource = new ObservableCollection<StoreItem>();
        public ObservableCollection<FilterItem> filterSource = new ObservableCollection<FilterItem>();
        public static Dictionary<string, StoreItem> cart = new Dictionary<string, StoreItem>();

        public Color filterBackgroundColorSelected = Color.FromHex("#E7404A");
        public Color filterBackgroundColorNonselected = Color.White;
        public Color filterTextColorSelected = Color.White;
        public Color filterTextColorNonselected = Color.FromHex("#E7404A");

        public static int totalQuantity = 0;
        public static int threshold = 5;

        public FoodBackStore()
        {
            InitializeComponent();

            SetFoodBank("Feeding Orange County", "5.3 miles away", "businessImage");
            SetFilters();
            SetItems();
            SetCartQuantity();
        }

        void SetFoodBank(string name, string distance, string picture)
        {
            foodBankName.Text = name;
            foodBankDistance.Text = distance;
            foodBankPicture.Source = picture;
        }

        void SetFilters()
        {
            try
            {
                filterList.ItemsSource = filterSource;

                var filterArray = new string[] { "Fruits", "Vegetables", "Meals", "Desserts" };

                foreach (var type in filterArray)
                {
                    filterSource.Add(new FilterItem()
                    {
                        filterName = type,
                        filterColor = filterBackgroundColorNonselected,
                        filterTextColor = filterTextColorNonselected,
                    });
                }

                if (filterSource.Count > 0)
                {
                    filterSource[0].filterColor = filterBackgroundColorSelected;
                    filterSource[0].filterTextColor = filterTextColorSelected;
                }

            }
            catch (Exception issue)
            {
                Debug.WriteLine(issue.Message);
            }
        }

        void SetItems()
        {
            itemsList.ItemsSource = itemSource;

            var items = new Dictionary<string, string>();

            items.Add("item1", "Fruits");
            items.Add("item2", "Fruits");
            items.Add("item3", "Fruits");
            items.Add("item4", "Vegetables");
            items.Add("item5", "Vegetables");
            items.Add("item6", "Meals");
            items.Add("item7", "Meals");
            items.Add("item8", "Meals");
            items.Add("item9", "Meals");
            items.Add("item10", "Desserts");

            foreach (string name in items.Keys)
            {
                var item = new StoreItem()
                {
                    image = "itemImage",
                    name = name,
                    quantity = 0,
                    type = items[name],
                };

                if (cart.Count != 0)
                {
                    if (cart.ContainsKey(item.name))
                    {
                        item.quantity = cart[item.name].quantity;
                    }
                }

                itemSource.Add(item);
            }
        }

        void AddRemoveFilter(System.Object sender, System.EventArgs e)
        {
            var frame = (Frame)sender;
            var recognizer = (TapGestureRecognizer)frame.GestureRecognizers[0];
            var filter = (FilterItem)recognizer.CommandParameter;

            if (filter.filterColor == filterBackgroundColorSelected)
            {
                filter.filterColorUpdate = filterBackgroundColorNonselected;
                filter.filterTextColorUpdate = filterTextColorNonselected;
            }
            else
            {
                filter.filterColorUpdate = filterBackgroundColorSelected;
                filter.filterTextColorUpdate = filterTextColorSelected;
            }

            var allFilterTypes = new List<string>();

            foreach (FilterItem item in filterSource)
            {
                if (item.filterColor == filterBackgroundColorSelected)
                {
                    allFilterTypes.Add(item.filterName);
                }
            }

            if (allFilterTypes.Count != 0)
            {
                var tempList = new ObservableCollection<StoreItem>();
                foreach (string type in allFilterTypes)
                {
                    foreach (StoreItem item in itemSource)
                    {
                        if (type == item.type)
                        {
                            tempList.Add(item);
                        }
                    }
                }
                itemsList.ItemsSource = tempList;
            }
            else
            {
                itemsList.ItemsSource = itemSource;
            }
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
            }

            SetCartQuantity();
        }

        void RemoveItemFromCart(System.Object sender, System.EventArgs e)
        {
            var label = (Label)sender;
            var recognizer = (TapGestureRecognizer)label.GestureRecognizers[0];
            var item = (StoreItem)recognizer.CommandParameter;

            if (item.quantity != 0)
            {

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

            SetCartQuantity();
        }

        void SetCartQuantity()
        {
            cartQuantity.Text = totalQuantity.ToString();

            if (totalQuantity == threshold)
            {
                DisplayAlert("Oops", "You have reached the maximum number of items", "OK");
            }
        }

        void NavigateToCartPage(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new CartPage(), false);
        }
    }
}
