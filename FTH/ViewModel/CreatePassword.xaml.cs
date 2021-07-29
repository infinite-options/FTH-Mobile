using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using FTH.Constants;
using FTH.Model.Login.LoginClasses;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FTH.ViewModel
{
    public partial class CreatePassword : ContentPage
    {
        Dictionary<string, string> signUpInput;

        public CreatePassword(Dictionary<string, string> input)
        {
            signUpInput = input;
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
            /*
            SignUpPost newSignUp = new SignUpPost();
            newSignUp.email = "10";
            newSignUp.first_name = signUpInput["first_name"];
            newSignUp.last_name = signUpInput["last_name"];
            newSignUp.phone_number = signUpInput["phone"];
            newSignUp.address = signUpInput["address"];
            newSignUp.unit = signUpInput["unit"];
            newSignUp.city = signUpInput["city"];
            newSignUp.state = signUpInput["state"];
            newSignUp.zip_code = signUpInput["zip"];
            newSignUp.latitude = signUpInput["latitude"];
            newSignUp.longitude = signUpInput["longitude"];
            newSignUp.referral_source = "";
            newSignUp.role = "CUSTOMER";
            newSignUp.social = "DIRECT";
            newSignUp.password = passEntry.Text;
            newSignUp.mobile_access_token = "";
            newSignUp.mobile_refresh_token = "";
            newSignUp.user_access_token = "";
            newSignUp.user_refresh_token = "";
            newSignUp.social_id = "";
            newSignUp.cust_id = "";

            var directSignUpSerializedObject = JsonConvert.SerializeObject(newSignUp);
            var content = new StringContent(directSignUpSerializedObject, Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine(directSignUpSerializedObject);

            var signUpclient = new HttpClient();
            var RDSResponse = await signUpclient.PostAsync(Constant.SignUpUrl, content);
            Debug.WriteLine("RDSResponse: " + RDSResponse.ToString());
            var RDSMessage = await RDSResponse.Content.ReadAsStringAsync();
            Debug.WriteLine("RDSMessage: " + RDSMessage.ToString());

            // if Sign up is has successfully ie 200 response code
            if (RDSResponse.IsSuccessStatusCode)
            {
                try
                {
                    var RDSData = JsonConvert.DeserializeObject<SignUpResponse>(RDSMessage);
                    Debug.WriteLine("RDSData: " + RDSData.ToString());
                    DateTime today = DateTime.Now;
                    DateTime expDate = today.AddDays(Constant.days);
                    // Local Variables in Xamarin that can be used throughout the App
                    Application.Current.Properties["user_id"] = RDSData.result.customer_uid;
                    Application.Current.Properties["time_stamp"] = expDate;
                    Application.Current.Properties["platform"] = "DIRECT";
                    System.Diagnostics.Debug.WriteLine("UserID is:" + (string)Application.Current.Properties["user_id"]);
                    System.Diagnostics.Debug.WriteLine("Time Stamp is:" + Application.Current.Properties["time_stamp"].ToString());
                    System.Diagnostics.Debug.WriteLine("platform is:" + (string)Application.Current.Properties["platform"]);

                    //string url = "https://ht56vci4v9.execute-api.us-west-1.amazonaws.com/dev/api/v2/Profile/" + (string)Application.Current.Properties["user_id"];
                    //var request3 = new HttpRequestMessage();
                    //request3.RequestUri = new Uri(url);
                    //request3.Method = HttpMethod.Get;
                    //var client2 = new HttpClient();
                    //HttpResponseMessage response = await client2.SendAsync(request3);
                    //HttpContent content2 = response.Content;
                    //Console.WriteLine("content: " + content2);
                    //var userString = await content2.ReadAsStringAsync();
                    //JObject info_obj2 = JObject.Parse(userString);
                    //this.NewMainPage.Clear();
                    //Preferences.Set("profilePicLink", null);
                    //// Go to Subscripton page
                    //// Application.Current.MainPage = new SubscriptionPage();

                    ////send email to verify email
                    //emailVerifyPost emailVer = new emailVerifyPost();
                    //emailVer.email = emailEntry.Text.Trim();
                    //var emailVerSerializedObj = JsonConvert.SerializeObject(emailVer);
                    //var content4 = new StringContent(emailVerSerializedObj, Encoding.UTF8, "application/json");
                    //var client3 = new HttpClient();
                    //var response3 = client3.PostAsync("https://ht56vci4v9.execute-api.us-west-1.amazonaws.com/dev/api/v2/email_verification", content4);
                    //Console.WriteLine("RESPONSE TO CHECKOUT   " + response3.Result);
                    //Console.WriteLine("CHECKOUT JSON OBJECT BEING SENT: " + emailVerSerializedObj);

                    Application.Current.MainPage = new CongratsPage();
                }
                catch
                {
                    var RDSData = JsonConvert.DeserializeObject<SignUpExisted>(RDSMessage);
                    Debug.WriteLine("RDSData: " + RDSData.ToString());
                    if (RDSData.message.Contains("taken"))
                    {
                        DisplayAlert("Error", "email address is already in use", "OK");
                        return;
                    }
                }
                //var RDSData = JsonConvert.DeserializeObject<SignUpResponse>(RDSMessage);
                //Debug.WriteLine("RDSData: " + RDSData.ToString());
                //DateTime today = DateTime.Now;
                //DateTime expDate = today.AddDays(Constant.days);

                //if (RDSData.message.Contains("taken"))
                //{
                //    DisplayAlert("Error", "email address is already in use", "OK");
                //}
                //else
                //{
                //    // Local Variables in Xamarin that can be used throughout the App
                //    //Application.Current.Properties["user_id"] = RDSData.result.customer_uid;
                //    Application.Current.Properties["time_stamp"] = expDate;
                //    Application.Current.Properties["platform"] = "DIRECT";
                //    System.Diagnostics.Debug.WriteLine("UserID is:" + (string)Application.Current.Properties["user_id"]);
                //    System.Diagnostics.Debug.WriteLine("Time Stamp is:" + Application.Current.Properties["time_stamp"].ToString());
                //    System.Diagnostics.Debug.WriteLine("platform is:" + (string)Application.Current.Properties["platform"]);

                //    //string url = "https://ht56vci4v9.execute-api.us-west-1.amazonaws.com/dev/api/v2/Profile/" + (string)Application.Current.Properties["user_id"];
                //    //var request3 = new HttpRequestMessage();
                //    //request3.RequestUri = new Uri(url);
                //    //request3.Method = HttpMethod.Get;
                //    //var client2 = new HttpClient();
                //    //HttpResponseMessage response = await client2.SendAsync(request3);
                //    //HttpContent content2 = response.Content;
                //    //Console.WriteLine("content: " + content2);
                //    //var userString = await content2.ReadAsStringAsync();
                //    //JObject info_obj2 = JObject.Parse(userString);
                //    //this.NewMainPage.Clear();
                //    //Preferences.Set("profilePicLink", null);
                //    //// Go to Subscripton page
                //    //// Application.Current.MainPage = new SubscriptionPage();

                //    ////send email to verify email
                //    //emailVerifyPost emailVer = new emailVerifyPost();
                //    //emailVer.email = emailEntry.Text.Trim();
                //    //var emailVerSerializedObj = JsonConvert.SerializeObject(emailVer);
                //    //var content4 = new StringContent(emailVerSerializedObj, Encoding.UTF8, "application/json");
                //    //var client3 = new HttpClient();
                //    //var response3 = client3.PostAsync("https://ht56vci4v9.execute-api.us-west-1.amazonaws.com/dev/api/v2/email_verification", content4);
                //    //Console.WriteLine("RESPONSE TO CHECKOUT   " + response3.Result);
                //    //Console.WriteLine("CHECKOUT JSON OBJECT BEING SENT: " + emailVerSerializedObj);

                //    Application.Current.MainPage = new CongratsPage();
                //}
            }
            */
            Application.Current.MainPage = new CongratsPage();

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
