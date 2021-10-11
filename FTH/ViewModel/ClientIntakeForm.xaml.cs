using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using FTH.Model;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FTH.ViewModel
{
    public partial class ClientIntakeForm : ContentPage
    {
        Address addr;
        public ObservableCollection<HouseholdMembers> MembersColl = new ObservableCollection<HouseholdMembers>();
        public Dictionary<StoreItem, int> itemAmounts = new Dictionary<StoreItem, int>();
        public Dictionary<int, string> checkboxText = new Dictionary<int, string>();
        int memberNum;
        string housingStatus;
        string livingSituation;
        CheckBox prevHS;
        CheckBox prevLS;

        public ClientIntakeForm(Dictionary<StoreItem, int> itmAmts)
        {
            housingStatus = "";
            livingSituation = "";
            itemAmounts = itmAmts;
            memberNum = 1;
            fillCheckBoxTextDict();
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;
            Console.WriteLine("Width = " + width.ToString());
            Console.WriteLine("Height = " + height.ToString());
            addr = new Address();

            InitializeComponent();

            MembersColl.Add(new HouseholdMembers
            {
                MemberTitle = "Member 1:"
            });

            membersCollView.ItemsSource = MembersColl;

        }

        void fillCheckBoxTextDict()
        {
            checkboxText.Add(1, "1Stably Housed");
            checkboxText.Add(2, "1Literally homeless");
            checkboxText.Add(3, "1Unstably housed / imminently losing housing");
            checkboxText.Add(4, "2Rental by client (no subsidy)");
            checkboxText.Add(5, "2Rental by client with Section 8");
            checkboxText.Add(6, "2Rental by client with subsidy");
            checkboxText.Add(7, "2Staying with family/friends");
            checkboxText.Add(8, "2Emergency Shelter");
            checkboxText.Add(9, "2Place not meant for human habitation (street, park, etc.)");
            checkboxText.Add(10, "2Hotel/motel (no voucher)");
            checkboxText.Add(11, "2Transitional housing /Safe Haven");
            checkboxText.Add(12, "2Other");
        }

        //address autocomplete start
        // Auto-complete
        private ObservableCollection<AddressAutocomplete> _addresses;
        public ObservableCollection<AddressAutocomplete> Addresses
        {
            get => _addresses ?? (_addresses = new ObservableCollection<AddressAutocomplete>());
            set
            {
                if (_addresses != value)
                {
                    _addresses = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _addressText;
        public string AddressText
        {
            get => _addressText;
            set
            {
                if (_addressText != value)
                {
                    _addressText = value;
                    OnPropertyChanged();
                }
            }
        }

        private async void OnAddressChanged(object sender, TextChangedEventArgs eventArgs)
        {
            addressList.IsVisible = true;
            UnitCity.IsVisible = false;
            StateZip.IsVisible = false;
            //UnitCityState.IsVisible = false;
            //ZipPhone.IsVisible = false;
            addressList.ItemsSource = await addr.GetPlacesPredictionsAsync(AddressEntry.Text);
            //addr.OnAddressChanged(addressList, Addresses, _addressText);
        }

        private void addressEntryFocused(object sender, EventArgs eventArgs)
        {
            //addr.addressEntryFocused(addressList, new Grid[] { UnitCityState, ZipPhone });
        }

        private void addressEntryUnfocused(object sender, EventArgs eventArgs)
        {
            addr.addressEntryUnfocused(addressList, new Grid[] { UnitCity, StateZip });
        }

        private void addressSelected(System.Object sender, System.EventArgs e)
        {
            addr.addressSelected(addressList, new Grid[] { UnitCity, StateZip }, AddressEntry, CityEntry, StateEntry, ZipEntry);
            addressList.IsVisible = false;
            UnitCity.IsVisible = true;
            StateZip.IsVisible = true;
        }
        //address autocomplete end

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

        async void submitButton_Clicked(System.Object sender, System.EventArgs e)
        {
            foreach (HouseholdMembers houseMem in MembersColl)
            {
                Debug.WriteLine("title: " + houseMem.MemberTitle);
                Debug.WriteLine("name: " + houseMem.MemberName);
                Debug.WriteLine("age: " + houseMem.MemberAge);
                Debug.WriteLine("relationship: " + houseMem.MemberRelationship);
            }

            foreach (var element in mainStack.Children)
            {
                //Debug.WriteLine("element class id:" + element.ClassId);
                //Debug.WriteLine("element: " + element.ToString());
                if (element.ToString() == "Xamarin.Forms.Frame")
                {
                    var frame = (Frame)element;
                    if (frame.Content.ToString() == "Xamarin.Forms.Entry")
                    {
                        var entry = (Entry)frame.Content;
                        Debug.WriteLine("placeholder: " + entry.Placeholder);
                        if ((entry.Placeholder != null && entry.Placeholder != "" && entry.Placeholder.Contains("*")) && (entry.Text == "" || entry.Text == null))
                        {
                            await DisplayAlert("Oops", "fill in all of the required information before submitting", "OK");
                            return;
                        }
                    }
                }

                if (livingSituation == "" || housingStatus == "")
                {
                    await DisplayAlert("Oops", "fill in all of the required information before submitting", "OK");
                    return;
                }

                if (livingSituation == "Other" && (otherEntry.Text == "" || otherEntry.Text == null))
                {
                    await DisplayAlert("Oops", "fill in all of the required information before submitting", "OK");
                    return;
                }



                //try
                //{
                //    Debug.WriteLine("frame");
                //    var frame = (Frame)element;
                //    Debug.WriteLine("frame child: " + frame.Content.ToString());
                //    //Debug.WriteLine("frame child: " + frame.Content.ToString());
                //}
                //catch { }
            }

            if (AddressEntry.Text == null || CityEntry.Text == null || StateEntry.Text == null || ZipEntry.Text == null)
            {
                await DisplayAlert("Oops", "Fill all of the fields before continuing.", "OK");
                return;
            }

            string unit;
            if (AptEntry.Text == null)
                unit = "";
            else unit = AptEntry.Text;

            AddressValidation addValid = new AddressValidation();
            var addressValidationCode = addValid.ValidateAddressString(AddressEntry.Text, unit, CityEntry.Text, StateEntry.Text, ZipEntry.Text);
            if (addressValidationCode == null)
            {
                await DisplayAlert("Invalid Address", "The address you entered couldn't be confirmed. Please enter another one.", "OK");
                return;
            }
            else if (addressValidationCode == "D")
            {
                await DisplayAlert("Missing Info", "Please enter your unit/apartment number into the appropriate field.", "OK");
                return;
            }

            ClientIntakePost clientform = new ClientIntakePost();
            clientform.customer_uid = (string)Application.Current.Properties["user_id"];
            clientform.name = FNameEntry.Text;
            clientform.last4_ss = int.Parse(Last4Entry.Text.Trim());
            clientform.dob = dobEntry.Text;
            clientform.address = AddressEntry.Text;
            //clientform.unit = unit;
            clientform.city = CityEntry.Text;
            clientform.county = countyEntry.Text;
            clientform.state = StateEntry.Text;
            clientform.zip = ZipEntry.Text;
            clientform.home_phone = phoneHomeEntry.Text;
            clientform.cell_phone = phoneCellEntry.Text;
            List<CI_HouseholdMembers> CImembers = new List<CI_HouseholdMembers>();

            foreach (HouseholdMembers houseMem in MembersColl)
            {
                CImembers.Add(new CI_HouseholdMembers
                {
                    name = houseMem.MemberName,
                    age = int.Parse(houseMem.MemberAge),
                    relationship = houseMem.MemberRelationship
                });
                //Debug.WriteLine("title: " + houseMem.MemberTitle);
                //Debug.WriteLine("name: " + houseMem.MemberName);
                //Debug.WriteLine("age: " + houseMem.MemberAge);
                //Debug.WriteLine("relationship: " + houseMem.MemberRelationship);
            }
            clientform.household_members = CImembers;
            clientform.under_18 = int.Parse(under18Entry.Text);
            clientform.over_18 = int.Parse(over18Entry.Text);
            clientform.over_65 = int.Parse(over65Entry.Text);
            clientform.housing_status = housingStatus;
            if (livingSituation == "Other")
                clientform.living_situation = "Other: " + otherEntry.Text;
            else clientform.living_situation = livingSituation;
            clientform.submit_date = dateEntry.Text;

            var submitFormJSONString = JsonConvert.SerializeObject(clientform);
            // Console.WriteLine("newPaymentJSONString" + newPaymentJSONString);
            var submitFormContent = new StringContent(submitFormJSONString, Encoding.UTF8, "application/json");
            Console.WriteLine("submitForm Content: " + submitFormContent);
            var client = new HttpClient();
            var response = client.PostAsync("https://c1zwsl05s5.execute-api.us-west-1.amazonaws.com/dev/api/v2/households", submitFormContent);
            //await DisplayAlert("Success", "Profile updated!", "OK");
            Console.WriteLine("RESPONSE TO HOUSEHOLDS   " + response.Result);
            Console.WriteLine("HOUSEHOLDS JSON OBJECT BEING SENT: " + submitFormJSONString);

            //Navigation.PushAsync(new CheckoutPage());
            await Navigation.PushAsync(new WestValleyForm(itemAmounts));
        }

        void addMemberClicked(System.Object sender, System.EventArgs e)
        {
            memberNum++;
            
            MembersColl.Add(new HouseholdMembers
            {
                MemberTitle = "Member " + memberNum.ToString() + ":"
            });

            membersCollView.ItemsSource = MembersColl;
            membersCollView.HeightRequest += 180;
        }

        void checkboxSelected(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            //Debug.WriteLine("prevHS: " + prevHS.ToString());
            //Debug.WriteLine("prevLS: " + prevLS.ToString());


            var checkbox1 = (CheckBox) sender;
            //Debug.WriteLine("checked: " + checkbox1.IsChecked.ToString());

            //the status of the checkbox is whatever it is after the click
            //if (checkbox1.IsChecked == false)
            //{
            //    return;
            //}

            Debug.WriteLine("sender anchor: " + checkbox1.AnchorX.ToString());
            Debug.WriteLine("value for key: " + checkboxText[(int)checkbox1.AnchorX]);
            var value = checkboxText[(int)checkbox1.AnchorX];
            //housing situation
            if (value.Substring(0,1) == "1")
            {
                Debug.WriteLine("current living situation: " + livingSituation);

                if (prevHS == checkbox1 && checkbox1.IsChecked == false)
                {
                    housingStatus = "";
                    prevHS = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevHS != null)
                {
                    prevHS.IsChecked = false;
                    prevHS = checkbox1;
                    housingStatus = value.Substring(1);
                }
                else
                {
                    housingStatus = value.Substring(1);
                    prevHS = checkbox1;
                }
            }
            //living situation
            else
            {
                Debug.WriteLine("current housing status: " + housingStatus);

                if (prevLS == checkbox1 && checkbox1.IsChecked == false)
                {
                    livingSituation = "";
                    prevLS = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevLS != null)
                {
                    prevLS.IsChecked = false;
                    prevLS = checkbox1;
                    livingSituation = value.Substring(1);
                }
                else
                {
                    livingSituation = value.Substring(1);
                    prevLS = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
        }
    }
}
