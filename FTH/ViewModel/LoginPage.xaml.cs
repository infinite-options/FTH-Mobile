using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FTH.Constants;
using FTH.LogInClasses;
using FTH.Model.Login.LoginClasses;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FTH.ViewModel
{
    public partial class LoginPage : ContentPage
    {
        public bool createAccount = false;
        string deviceId;
        int directEmailVerified = 0;
        public HttpClient client = new HttpClient();

        public LoginPage()
        {
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;
            Console.WriteLine("Width = " + width.ToString());
            Console.WriteLine("Height = " + height.ToString());

            InitializeComponent();
        }

        void backClicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
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
            Application.Current.MainPage = new FoodBanksMap();
            //Navigation.PushAsync(new FoodBanksMap());
        }

        void loginClicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }

        //end of menu functions

        void clickedSeePassword(System.Object sender, System.EventArgs e)
        {
            if (passEntry.IsPassword == true)
                passEntry.IsPassword = false;
            else passEntry.IsPassword = true;
        }

        async void clickedLogin(System.Object sender, System.EventArgs e)
        {
            var accountSalt = await retrieveAccountSalt(nameEntry.Text.ToLower().Trim());

            if (accountSalt != null)
            {
                var loginAttempt = await LogInUser(nameEntry.Text.ToLower(), passEntry.Text, accountSalt);

                if (directEmailVerified == 0)
                {
                    DisplayAlert("Please Verify Email", "Please click the link in the email sent to " + nameEntry.Text + ". Check inbox and spam folders.", "OK");

                    //send email to verify email
                    emailVerifyPost emailVer = new emailVerifyPost();
                    emailVer.email = nameEntry.Text.Trim();
                    var emailVerSerializedObj = JsonConvert.SerializeObject(emailVer);
                    var content4 = new StringContent(emailVerSerializedObj, Encoding.UTF8, "application/json");
                    var client3 = new HttpClient();
                    var response3 = client3.PostAsync("https://ht56vci4v9.execute-api.us-west-1.amazonaws.com/dev/api/v2/email_verification", content4);
                    Console.WriteLine("RESPONSE TO CHECKOUT   " + response3.Result);
                    Console.WriteLine("CHECKOUT JSON OBJECT BEING SENT: " + emailVerSerializedObj);

                    loginButton.IsEnabled = true;
                }

                if (loginAttempt != null && loginAttempt.message != "Request failed, wrong password.")
                {
                    System.Diagnostics.Debug.WriteLine("USER'S DATA");
                    System.Diagnostics.Debug.WriteLine("USER CUSTOMER_UID: " + loginAttempt.result[0].customer_uid);
                    System.Diagnostics.Debug.WriteLine("USER FIRST NAME: " + loginAttempt.result[0].customer_first_name);
                    System.Diagnostics.Debug.WriteLine("USER LAST NAME: " + loginAttempt.result[0].customer_last_name);
                    System.Diagnostics.Debug.WriteLine("USER EMAIL: " + loginAttempt.result[0].customer_email);

                    DateTime today = DateTime.Now;
                    DateTime expDate = today.AddDays(Constant.days);

                    Application.Current.Properties["user_id"] = loginAttempt.result[0].customer_uid;
                    Application.Current.Properties["time_stamp"] = expDate;
                    Application.Current.Properties["platform"] = "DIRECT";

                    // Application.Current.MainPage = new CarlosHomePage();
                    // This statement initializes the stack to Subscription Page
                    //check to see if user has already selected a meal plan before
                    var request = new HttpRequestMessage();
                    Console.WriteLine("user_id: " + (string)Application.Current.Properties["user_id"]);
                    string url = "https://ht56vci4v9.execute-api.us-west-1.amazonaws.com/dev/api/v2/customer_lplp?customer_uid=" + (string)Application.Current.Properties["user_id"];
                    //old db
                    //string url = "https://ht56vci4v9.execute-api.us-west-1.amazonaws.com/dev/api/v2/customer_lplp?customer_uid=" + (string)Application.Current.Properties["user_id"];
                    //string url = "https://ht56vci4v9.execute-api.us-west-1.amazonaws.com/dev/api/v2/meals_selected?customer_uid=" + (string)Application.Current.Properties["user_id"];
                    //string url = "https://ht56vci4v9.execute-api.us-west-1.amazonaws.com/dev/api/v2/meals_selected?customer_uid=" + "100-000256";
                    Console.WriteLine("url: " + url);
                    request.RequestUri = new Uri(url);
                    //request.RequestUri = new Uri("https://ht56vci4v9.execute-api.us-west-1.amazonaws.com/dev/api/v2/get_delivery_info/400-000453");
                    request.Method = HttpMethod.Get;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContent content = response.Content;
                        Console.WriteLine("content: " + content);
                        var userString = await content.ReadAsStringAsync();
                        Console.WriteLine(userString);

                        //writing guid to db
                        if (Preferences.Get("setGuid" + (string)Application.Current.Properties["user_id"], false) == false)
                        {
                            if (Device.RuntimePlatform == Device.iOS)
                            {
                                deviceId = Preferences.Get("guid", null);
                                if (deviceId != null) { Debug.WriteLine("This is the iOS GUID from Log in: " + deviceId); }
                            }
                            else
                            {
                                deviceId = Preferences.Get("guid", null);
                                if (deviceId != null) { Debug.WriteLine("This is the Android GUID from Log in " + deviceId); }
                            }

                            if (deviceId != null)
                            {
                                GuidPost notificationPost = new GuidPost();

                                notificationPost.uid = (string)Application.Current.Properties["user_id"];
                                notificationPost.guid = deviceId.Substring(5);
                                Application.Current.Properties["guid"] = deviceId.Substring(5);
                                notificationPost.notification = "TRUE";

                                var notificationSerializedObject = JsonConvert.SerializeObject(notificationPost);
                                Debug.WriteLine("Notification JSON Object to send: " + notificationSerializedObject);

                                var notificationContent = new StringContent(notificationSerializedObject, Encoding.UTF8, "application/json");

                                var clientResponse = await client.PostAsync(Constant.GuidUrl, notificationContent);

                                Debug.WriteLine("Status code: " + clientResponse.IsSuccessStatusCode);

                                if (clientResponse.IsSuccessStatusCode)
                                {
                                    System.Diagnostics.Debug.WriteLine("We have post the guid to the database");
                                    Preferences.Set("setGuid" + (string)Application.Current.Properties["user_id"], true);
                                }
                                else
                                {
                                    await DisplayAlert("Ooops!", "Something went wrong. We are not able to send you notification at this moment", "OK");
                                }
                            }
                        }
                        //written


                    }

                    Application.Current.MainPage = new NavigationPage(new Filter());
                }
                else
                {
                    await DisplayAlert("Error", "Wrong password was entered", "OK");
                    loginButton.IsEnabled = true;
                }
            }
            loginButton.IsEnabled = true;
        }

        public async Task<LogInResponse> LogInUser(string userEmail, string userPassword, AccountSalt accountSalt)
        {
                SHA512 sHA512 = new SHA512Managed();
                byte[] data = sHA512.ComputeHash(Encoding.UTF8.GetBytes(userPassword + accountSalt.password_salt)); // take the password and account salt to generate hash
                string hashedPassword = BitConverter.ToString(data).Replace("-", string.Empty).ToLower(); // convert hash to hex

                LogInPost loginPostContent = new LogInPost();
                loginPostContent.email = userEmail;
                loginPostContent.password = hashedPassword;
                loginPostContent.social_id = "";
                loginPostContent.signup_platform = "";
                Preferences.Set("hashed_password", hashedPassword);
                Preferences.Set("user_password", userPassword);
                Console.WriteLine("accountSalt: " + accountSalt.password_salt);
                Console.WriteLine("userPassword: " + userPassword);

                string loginPostContentJson = JsonConvert.SerializeObject(loginPostContent); // make orderContent into json

                var httpContent = new StringContent(loginPostContentJson, Encoding.UTF8, "application/json"); // encode orderContentJson into format to send to database
                var response = await client.PostAsync(Constant.LogInUrl, httpContent); // try to post to database
                var message = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("LogInUser message: " + message);
                string emailVerify = message.Substring(message.IndexOf("email_verified") + 18, 1);
                Debug.WriteLine("emailVerify: " + emailVerify);
                if (emailVerify == "1")
                    directEmailVerified = 1;
                if (message.IndexOf("email_verified") == -1)
                    directEmailVerified = -1;


                if (message.Contains(Constant.AutheticatedSuccesful))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LogInResponse>(responseContent);
                    return loginResponse;
                }
                else
                {
                    return null;
                }
        }

        private async Task<AccountSalt> retrieveAccountSalt(string userEmail)
        {
            System.Diagnostics.Debug.WriteLine(userEmail);

            SaltPost saltPost = new SaltPost();
            saltPost.email = userEmail;

            var saltPostSerilizedObject = JsonConvert.SerializeObject(saltPost);
            var saltPostContent = new StringContent(saltPostSerilizedObject, Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine(saltPostSerilizedObject);

            var client = new HttpClient();
            var DRSResponse = await client.PostAsync(Constant.AccountSaltUrl, saltPostContent);
            var DRSMessage = await DRSResponse.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(DRSMessage);

            AccountSalt userInformation = null;

            if (DRSResponse.IsSuccessStatusCode)
            {
                var result = await DRSResponse.Content.ReadAsStringAsync();

                AcountSaltCredentials data = new AcountSaltCredentials();
                data = JsonConvert.DeserializeObject<AcountSaltCredentials>(result);

                if (DRSMessage.Contains(Constant.UseSocialMediaLogin))
                {
                    Debug.WriteLine("check if social login already exists for this email");
                    createAccount = true;
                    System.Diagnostics.Debug.WriteLine(DRSMessage);
                    await DisplayAlert("Oops!", data.message, "OK");
                }
                else if (DRSMessage.Contains(Constant.EmailNotFound))
                {
                    await DisplayAlert("Oops!", "Our records show that you don't have an accout. Please sign up!", "OK");
                }
                else
                {
                    userInformation = new AccountSalt
                    {
                        password_algorithm = data.result[0].password_algorithm,
                        password_salt = data.result[0].password_salt
                    };
                }
            }

            return userInformation;
        }
    }
}
