using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using FTH.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
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
            
            getInfo();
            SetFoodBank(Preferences.Get("chosenBankName", ""), totalQuantity.ToString(), Preferences.Get("chosenBankImg", ""));
            SetCartItems();
            //SetPersonalInfo("Carlos", "Torres", "4158329643");
            //SetFullAddress("1658 Sacramento Street", "San Francisco", "CA", "94109");
            SetFullDeliveryInfo("June 27, 2021", "10:00 AM - 12:00 PM");
        }

        async void getInfo()
        {
            var request = new HttpRequestMessage();
            Console.WriteLine("user_id: " + (string)Application.Current.Properties["user_id"]);
            string url = "https://c1zwsl05s5.execute-api.us-west-1.amazonaws.com/dev/api/v2/Profile/" + (string)Application.Current.Properties["user_id"];
            Debug.WriteLine("profile endpoint: " + url);
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                Debug.WriteLine("content: " + content);
                var userString = await content.ReadAsStringAsync();
                Debug.WriteLine("userString: " + userString);
                var info_obj = JObject.Parse(userString);

                //pass the displayed info into the checkout obj
                checkoutobj.delivery_first_name = (info_obj["result"])[0]["customer_first_name"].ToString();
                checkoutobj.delivery_last_name = (info_obj["result"])[0]["customer_last_name"].ToString();
                checkoutobj.delivery_phone_num = (info_obj["result"])[0]["customer_phone_num"].ToString();
                checkoutobj.delivery_email = (info_obj["result"])[0]["customer_email"].ToString();
                checkoutobj.delivery_address = (info_obj["result"])[0]["customer_address"].ToString();
                checkoutobj.delivery_unit = (info_obj["result"])[0]["customer_unit"].ToString();
                checkoutobj.delivery_city = (info_obj["result"])[0]["customer_city"].ToString();
                checkoutobj.delivery_state = (info_obj["result"])[0]["customer_state"].ToString();
                checkoutobj.delivery_zip = (info_obj["result"])[0]["customer_zip"].ToString();
                checkoutobj.delivery_latitude = (info_obj["result"])[0]["customer_lat"].ToString();
                checkoutobj.delivery_longitude = (info_obj["result"])[0]["customer_long"].ToString();

                SetPersonalInfo((info_obj["result"])[0]["customer_first_name"].ToString(), (info_obj["result"])[0]["customer_last_name"].ToString(), (info_obj["result"])[0]["customer_phone_num"].ToString());
                SetFullAddress((info_obj["result"])[0]["customer_address"].ToString(), (info_obj["result"])[0]["customer_city"].ToString(),
                    (info_obj["result"])[0]["customer_state"].ToString(), (info_obj["result"])[0]["customer_zip"].ToString());
            }
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
            checkoutobj.delivery_instructions = "deliv instructions";
            checkoutobj.order_type = "food";
            checkoutobj.purchase_notes = "purch notes";
            checkoutobj.start_delivery_date = "";
            checkoutobj.pay_coupon_id = "";
            //checkoutobj.amount_due = price.ToString();
            checkoutobj.amount_due = "0";
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
            checkoutobj.subtotal = "0";
            checkoutobj.service_fee = "0";
            checkoutobj.delivery_fee = "0";
            checkoutobj.driver_tip = "0";
            checkoutobj.taxes = "0";
            checkoutobj.ambassador_code = "0";

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
