using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using FTH.Model;
using Newtonsoft.Json;
using Xamarin.Forms;
using static FTH.ViewModel.EditAddressPage;
using static FTH.ViewModel.FoodBackStore;

namespace FTH.ViewModel
{
    public partial class CheckoutPage : ContentPage
    {
        public ObservableCollection<StoreItem> itemsSource = new ObservableCollection<StoreItem>();
        public Dictionary<StoreItem, int> itemAmounts = new Dictionary<StoreItem, int>();
        CheckoutPost checkoutobj = new CheckoutPost();

        public CheckoutPage(Dictionary<StoreItem, int> itmAmts)
        {
            itemAmounts = itmAmts;
            InitializeComponent();

            //pass the displayed info into the checkout obj
            checkoutobj.delivery_first_name = "Carlos";
            checkoutobj.delivery_last_name = "Torres";
            checkoutobj.delivery_phone_num = "1231231234";
            checkoutobj.delivery_email = "j12345l54321@gmail.com";
            checkoutobj.delivery_address = "6123 Corte De La Reina";
            checkoutobj.delivery_unit = "";
            checkoutobj.delivery_city = "San Jose";
            checkoutobj.delivery_state = "CA";
            checkoutobj.delivery_zip = "95120";
            checkoutobj.delivery_latitude = "37.227124";
            checkoutobj.delivery_longitude = "-121.886943";


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

        async void NavigateToConfirmationPage(System.Object sender, System.EventArgs e)
        {
            //CheckoutPost checkoutobj = new CheckoutPost();
            Item[] itmList = new Item[itemAmounts.Count];
            int index = 0;
            double price = 0;
            foreach(var item in itemAmounts.Keys)
            {
                Item item1 = new Item();
                item1.img = item.image;
                item1.qty = item.quantity;
                item1.name = item.name;
                item1.unit = item.unit;
                item1.price = item.price;
                item1.item_uid = item.item_uid;
                item1.itm_business_uid = item.itm_business_uid;
                itmList[index] = item1;
                price += item.quantity * item.price;
                index++;
            }
            checkoutobj.pur_customer_uid = Application.Current.Properties["user_id"].ToString();
            checkoutobj.pur_business_uid = Application.Current.Properties["chosen_business_uid"].ToString();
            checkoutobj.items = itmList;
            checkoutobj.order_instructions = "";
            checkoutobj.delivery_instructions = "";
            checkoutobj.order_type = "food";
            checkoutobj.purchase_notes = "good";
            checkoutobj.start_delivery_date = "";
            checkoutobj.pay_coupon_id = "";
            checkoutobj.amount_due = price.ToString();
            checkoutobj.amount_discount = "0";
            checkoutobj.amount_paid = "0";
            checkoutobj.info_is_Addon = "FALSE";
            checkoutobj.cc_num = "4242424242424242";
            checkoutobj.cc_exp_date = "2028-07-01 00:00:00";
            checkoutobj.cc_cvv = "222";
            checkoutobj.cc_zip = "95132";
            checkoutobj.charge_id = "";
            checkoutobj.payment_type = "STRIPE";
            checkoutobj.delivery_status = "FALSE";
            checkoutobj.subtotal = price.ToString();
            checkoutobj.service_fee = "3";
            checkoutobj.delivery_fee = "2";
            checkoutobj.driver_tip = "2";
            checkoutobj.taxes = "1.5";
            checkoutobj.ambassador_code = "2";

            var getItemsSerializedObject = JsonConvert.SerializeObject(checkoutobj);
            var content = new StringContent(getItemsSerializedObject, Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine("checkout obj: " + getItemsSerializedObject);

            var getItemsClient = new HttpClient();
            var RDSResponse = await getItemsClient.PostAsync("https://c1zwsl05s5.execute-api.us-west-1.amazonaws.com/dev/api/v2/checkout_SN", content);
            var RDSMessage = await RDSResponse.Content.ReadAsStringAsync();
            Debug.WriteLine("RDSResponse from checkout endpoint: " + RDSResponse.ToString());
            Debug.WriteLine("RDSMessage from checkout endpoint: " + RDSMessage.ToString());

            await Navigation.PushAsync(new OrderConfirmationPage(), false);
        }

        void NavigateToEditAddressPage(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new EditAddressPage(itemAmounts), false);
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
