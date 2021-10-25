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
    public partial class EditClientIntakeForm : ContentPage
    {
        public ObservableCollection<HouseholdMembers> MembersColl = new ObservableCollection<HouseholdMembers>();
        int memberNum;

        public EditClientIntakeForm()
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
                        if (receivedList[i].customer_uid == (string)Application.Current.Properties["user_id"])
                        {
                            Debug.WriteLine("id matched");

                            name.Text = receivedList[i].name;
                            socialSec.Text = receivedList[i].last4_ss;
                            dob.Text = receivedList[i].dob;
                            address.Text = receivedList[i].address + "\n" + receivedList[i].city
                                + ", " + receivedList[i].state + " " + receivedList[i].zip;
                            home_phone.Text = receivedList[i].home_phone;

                            string householdMems = receivedList[i].household_members;
                            JArray membersArray = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(householdMems);

                            foreach (JObject member in membersArray)
                            {
                                MembersColl.Add(new HouseholdMembers
                                {
                                    MemberTitle = "Member " + memberNum.ToString() + ":",
                                    MemberName = (string)member["name"],
                                    MemberAge = (string)member["age"],
                                    MemberRelationship = (string)member["relationship"]
                                });

                                membersCollView.ItemsSource = MembersColl;
                                if (memberNum != 1)
                                    membersCollView.HeightRequest += 200;
                                memberNum++;
                            }

                            under18.Text = receivedList[i].under_18;
                            over18.Text = receivedList[i].over_18;
                            over65.Text = receivedList[i].over_65;
                            housingStatus.Text = receivedList[i].housing_status;
                            livingSituation.Text = receivedList[i].living_situation;
                            submitDate.Text = receivedList[i].submit_date;

                            return;
                        }
                    }
                    
                }
            }
            catch { }

            if (MembersColl.Count == 0)
            {
                MembersColl.Add(new HouseholdMembers
                {
                    MemberTitle = "Member 1 :",
                    MemberName = "First Last",
                    MemberAge = "##",
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
    }
}
