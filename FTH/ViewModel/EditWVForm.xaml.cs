using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using FTH.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FTH.ViewModel
{
    public partial class EditWVForm : ContentPage
    {
        public ObservableCollection<HouseholdComp> MembersColl = new ObservableCollection<HouseholdComp>();
        int memberNum;

        public EditWVForm()
        {
            memberNum = 1;
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;
            Console.WriteLine("Width = " + width.ToString());
            Console.WriteLine("Height = " + height.ToString());

            InitializeComponent();
            autofillInfo();
        }

        async void autofillInfo()
        {
            try
            {


                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("https://c1zwsl05s5.execute-api.us-west-1.amazonaws.com/dev/api/v2/households");
                request.Method = HttpMethod.Get;
                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var message = await response.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<FormsGet>(message);
                    var receivedList = data.result;
                    for (int i = receivedList.Count - 1; i >= 0; i--)
                    {
                        if (receivedList[i].customer_uid == (string)Application.Current.Properties["user_id"] &&
                            receivedList[i].english_fluency != null && receivedList[i].english_fluency != "")
                        {
                            Debug.WriteLine("id matched");

                            last_permanent_zip.Text = receivedList[i].last_permanent_zip;
                            last_city.Text = receivedList[i].last_sleep_city;
                            extent_homelessness.Text = receivedList[i].extent_homelessness;
                            gender.Text = receivedList[i].gender;
                            marital_status.Text = receivedList[i].marital_status;
                            education.Text = receivedList[i].education;
                            highest_grade_level.Text = receivedList[i].highest_grade_level;
                            hispanic_origin.Text = receivedList[i].hispanic_origin;
                            primary_ethnicity.Text = receivedList[i].primary_ethnicity;
                            veteran.Text = receivedList[i].veteran;
                            long_disability.Text = receivedList[i].long_disability;
                            long_disability_desc.Text = receivedList[i].long_disability_desc;
                            primary_lang.Text = receivedList[i].primary_lang;
                            english_fluency.Text = receivedList[i].english_fluency;
                            employment_status.Text = receivedList[i].employment_status;
                            medical_insurance.Text = receivedList[i].medical_insurance;
                            special_nutrition.Text = receivedList[i].special_nutrition;
                            if (receivedList[i].calfresh.ToLower() == "yes")
                                calfresh.Text = "Yes, Monthly Amount: $" + receivedList[i].calfresh_amount;
                            else calfresh.Text = "No";
                            emerg_name.Text = receivedList[i].emergency_name;
                            emerg_phone.Text = receivedList[i].emergency_phone;
                            emerg_relationship.Text = receivedList[i].emergency_relationship;
                            emerg_client.Text = receivedList[i].emergency_client;


                            string householdMems = receivedList[i].household_members;
                            JArray membersArray = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(householdMems);

                            foreach (JObject member in membersArray)
                            {
                                MembersColl.Add(new HouseholdComp
                                {
                                    MemberTitle = "Member " + memberNum.ToString() + ":",
                                    MemberName = (string)member["name"],
                                    MemberSSN = (string)member["last4_ss"],
                                    MemberAge = (string)member["age"],
                                    MemberDOB = (string)member["dob"],
                                    MemberRelationship = (string)member["relationship"]
                                });

                                membersCollView.ItemsSource = MembersColl;
                                if (memberNum != 1)
                                    membersCollView.HeightRequest += 300;
                                memberNum++;
                            }


                            return;
                        }
                    }

                }
            }
            catch { }

            if (MembersColl.Count == 0)
            {
                MembersColl.Add(new HouseholdComp
                {
                    MemberTitle = "Member 1 :",
                    MemberName = "First Last",
                    MemberSSN = "####",
                    MemberAge = "##",
                    MemberDOB = "##/##/##",
                    MemberRelationship = "Father"
                });

                membersCollView.ItemsSource = MembersColl;
            }
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

        void orderClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new FoodBanksMap();
            Navigation.PushAsync(new Filter());
        }

        void browseClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new FoodBanksMap();
            Navigation.PushAsync(new FoodBanksMap());
        }

        void loginClicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.Properties["platform"] = "GUEST";
            Application.Current.Properties.Remove("user_id");
            Application.Current.MainPage = new LoginPage();
        }


        //end of menu functions

        void editClicked(System.Object sender, System.EventArgs e)
        {
            FoodBanks emptyFb = new FoodBanks();
            Dictionary<StoreItem, int> emptyDict = new Dictionary<StoreItem, int>();
            Navigation.PushAsync(new WestValleyForm(true, emptyFb, emptyDict));
        }
    }
}
