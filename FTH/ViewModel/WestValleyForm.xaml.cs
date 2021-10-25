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
    public partial class WestValleyForm : ContentPage
    {
        public ObservableCollection<HouseholdComp> MembersColl = new ObservableCollection<HouseholdComp>();
        public Dictionary<StoreItem, int> itemAmounts = new Dictionary<StoreItem, int>();
        public Dictionary<int, string> checkboxText = new Dictionary<int, string>();
        int memberNum;
        string homelessnessExtent; CheckBox prevHE; //done
        string gender; CheckBox prevG; //done
        string maritalStatus; CheckBox prevMS; //done
        string education; CheckBox prevE; //done
        string highestGrade; CheckBox prevHG; //done
        string hispanicOrigin; CheckBox prevHO; //done
        string primaryEthnicity; CheckBox prevPE; //done
        string veteran; CheckBox prevV; //done
        string disability; CheckBox prevD; //done
        string disabilityDesc; CheckBox prevDD; //done
        string primaryLang; CheckBox prevPL; //done
        string engFluency; CheckBox prevEF; //done
        string employmentStatus; CheckBox prevES; //done
        string medicalInsurance; CheckBox prevMI; //done
        string specialNutrition; CheckBox prevSN; //done
        string calfresh; CheckBox prevC; //done
        string contactClient; CheckBox prevCC;
        FoodBanks chosenFb;

        public WestValleyForm(FoodBanks foodbank, Dictionary<StoreItem, int> itmAmts)
        {
            chosenFb = foodbank;
            homelessnessExtent = ""; gender = ""; maritalStatus = ""; education = ""; highestGrade = "";
            hispanicOrigin = ""; primaryEthnicity = ""; veteran = ""; disability = ""; disabilityDesc = ""; 
            primaryLang = ""; engFluency = ""; employmentStatus = ""; medicalInsurance = ""; specialNutrition = ""; calfresh = "";
            contactClient = "";
            itemAmounts = itmAmts;
            memberNum = 1;
            fillCheckBoxTextDict();
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;
            Console.WriteLine("Width = " + width.ToString());
            Console.WriteLine("Height = " + height.ToString());

            InitializeComponent();

            MembersColl.Add(new HouseholdComp
            {
                MemberTitle = "Member 1:"
            });

            membersCollView.ItemsSource = MembersColl;
        }

        void fillCheckBoxTextDict()
        {
            checkboxText.Add(1, "1 One week or less");
            checkboxText.Add(2, "1 More than a week but less than a month");
            checkboxText.Add(3, "1 1-3 months");
            checkboxText.Add(4, "1 More than 3 months but less than a year");
            checkboxText.Add(5, "1 More than a year");
            checkboxText.Add(6, "2 Female");
            checkboxText.Add(7, "2 Male");
            checkboxText.Add(8, "2 Transgender F-M");
            checkboxText.Add(9, "2 Transgender M-F");
            checkboxText.Add(10, "2 Other");
            checkboxText.Add(11, "2 Unknown");
            checkboxText.Add(12, "3 Single");
            checkboxText.Add(13, "3 Married");
            checkboxText.Add(14, "3 Separated");
            checkboxText.Add(15, "3 Divorced");
            checkboxText.Add(16, "3 Domestic Partner");
            checkboxText.Add(17, "3 Widowed");
            checkboxText.Add(18, "4 Some School");
            checkboxText.Add(19, "4 GED");
            checkboxText.Add(20, "4 AA degree");
            checkboxText.Add(21, "4 College degree");
            checkboxText.Add(22, "4 Graduate degree");
            checkboxText.Add(23, "5 Elementary School");
            checkboxText.Add(24, "5 Middle School");
            checkboxText.Add(25, "5 High School");
            checkboxText.Add(26, "5 Junior College");
            checkboxText.Add(27, "5 Undergraduate School");
            checkboxText.Add(28, "5 Graduate School");
            checkboxText.Add(29, "6 Yes");
            checkboxText.Add(30, "6 No");
            checkboxText.Add(31, "6 Unknown");
            checkboxText.Add(32, "7 American Indian/Alaskan Native");
            checkboxText.Add(33, "7 American Indian/Alaskan Native and White");
            checkboxText.Add(34, "7 Asian");
            checkboxText.Add(35, "7 Asian and White");
            checkboxText.Add(36, "7 Black / African American");
            checkboxText.Add(37, "7 Black / African and White");
            checkboxText.Add(38, "7 Native Hawaiian/ Pacific Islander");
            checkboxText.Add(39, "7 American Indian/ Alaskan Native and Black/ African American");
            checkboxText.Add(40, "7 White");
            checkboxText.Add(41, "7 Unknown");
            checkboxText.Add(42, "7 Other Multi-Racial");
            checkboxText.Add(43, "8 Yes");
            checkboxText.Add(44, "8 No");
            checkboxText.Add(45, "9 Yes");
            checkboxText.Add(46, "9 No");
            checkboxText.Add(47, "9 Unknown");
            checkboxText.Add(48, "10 Alcohol abuse");
            checkboxText.Add(49, "10 Drug abuse");
            checkboxText.Add(50, "10 Mental health problems");
            checkboxText.Add(51, "10 Development disability");
            checkboxText.Add(52, "10 Physical disability");
            checkboxText.Add(53, "10 Chronic health condition");
            checkboxText.Add(54, "10 HIV/AIDS");
            checkboxText.Add(55, "10 Other");
            checkboxText.Add(56, "11 English");
            checkboxText.Add(57, "11 Chinese");
            checkboxText.Add(58, "11 Russian");
            checkboxText.Add(59, "11 Spanish");
            checkboxText.Add(60, "11 Vietnamese");
            checkboxText.Add(61, "11 Other");
            checkboxText.Add(62, "12 Fluent");
            checkboxText.Add(63, "12 Semi-fluent");
            checkboxText.Add(64, "12 Not fluent");
            checkboxText.Add(65, "13 Full time, 35+ hrs/wk");
            checkboxText.Add(66, "13 Part time, less than 35 hrs/wk");
            checkboxText.Add(67, "13 Retired");
            checkboxText.Add(68, "13 Unemployed, seeking work");
            checkboxText.Add(69, "13 Disable, not in labor force");
            checkboxText.Add(70, "13 Full time homemaker");
            checkboxText.Add(71, "13 Student");
            checkboxText.Add(72, "13 Unknown");
            checkboxText.Add(73, "14 Medi-Cal");
            checkboxText.Add(74, "14 Medicare");
            checkboxText.Add(75, "14 Medicaide");
            checkboxText.Add(76, "14 Uninsured");
            checkboxText.Add(77, "14 Private Insurance");
            checkboxText.Add(78, "14 Work Insurance");
            checkboxText.Add(79, "15 Yes");
            checkboxText.Add(80, "15 No");
            checkboxText.Add(81, "16 Yes");
            checkboxText.Add(82, "16 No");
            checkboxText.Add(83, "17 Yes");
            checkboxText.Add(84, "17 No");
        }

        void addMemberClicked(System.Object sender, System.EventArgs e)
        {
            memberNum++;

            MembersColl.Add(new HouseholdComp
            {
                MemberTitle = "Member " + memberNum.ToString() + ":"
            });

            membersCollView.ItemsSource = MembersColl;
            membersCollView.HeightRequest += 300;
        }

        async void submitClicked(System.Object sender, System.EventArgs e)
        {
            if (homelessnessExtent == "" || gender == "" || maritalStatus == "" || education == "" || highestGrade == ""
                || hispanicOrigin == "" || primaryEthnicity == "" || veteran == "" || disability == "" || disabilityDesc == ""
                || primaryLang == "" || engFluency == "" || employmentStatus == "" || medicalInsurance == "" || specialNutrition == ""
                || calfresh == "" || contactClient == "")
            {
                await DisplayAlert("Oops", "fill in all of the required information before submitting", "OK");
                return;
            }

            if (gender == "Other" && (otherGenderEntry.Text == "" || otherGenderEntry.Text == null))
            {
                await DisplayAlert("Oops", "fill in all of the required information before submitting", "OK");
                return;
            }

            if (disabilityDesc == "Other" && (otherDisabilityEntry.Text == "" || otherDisabilityEntry.Text == null))
            {
                await DisplayAlert("Oops", "fill in all of the required information before submitting", "OK");
                return;
            }

            if (primaryLang == "Other" && (otherLangEntry.Text == "" || otherLangEntry.Text == null))
            {
                await DisplayAlert("Oops", "fill in all of the required information before submitting", "OK");
                return;
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
                        if ((entry.Placeholder != null && entry.Placeholder != "") && (entry.Text == "" || entry.Text == null))
                        {
                            await DisplayAlert("Oops", "fill in all of the required information before submitting", "OK");
                            return;
                        }
                    }
                }
            }

            WestValleyPost wvForm = new WestValleyPost();
            wvForm.customer_uid = (string)Application.Current.Properties["user_id"];
            wvForm.name = adultNameEntry.Text;
            wvForm.last_permanent_zip = zipCodeEntry.Text;
            wvForm.last_sleep_city = cityEntry.Text;
            wvForm.extent_homelessness = homelessnessExtent;
            if (gender == "Other")
                wvForm.gender = "Other: " + otherGenderEntry.Text;
            //wvForm.gender = gender;
            wvForm.marital_status = maritalStatus;
            wvForm.education = education;
            wvForm.highest_grade_level = highestGrade;
            wvForm.hispanic_origin = hispanicOrigin;
            wvForm.primary_ethnicity = primaryEthnicity;
            wvForm.veteran = veteran;
            wvForm.long_disability = disability;
            if (disabilityDesc == "Other")
                wvForm.long_disability_desc = "Other: " + otherDisabilityEntry.Text;
            //wvForm.long_disability_desc = disabilityDesc;
            if (primaryLang == "Other")
                wvForm.primary_lang = "Other: " + otherLangEntry.Text;
            //wvForm.primary_lang = primaryLang;
            wvForm.english_fluency = engFluency;
            wvForm.employment_status = employmentStatus;
            wvForm.medical_insurance = medicalInsurance;
            wvForm.special_nutrition = specialNutrition;
            wvForm.calfresh = calfresh;
            if (amtEntry.Text == "" || amtEntry.Text == null)
                wvForm.calfresh_amount = 0;
            else wvForm.calfresh_amount = double.Parse(amtEntry.Text.Trim());
            wvForm.emergency_name = emergencyNameEntry.Text;
            wvForm.emergency_phone = emergencyNumEntry.Text;
            wvForm.emergency_relationship = emergencyRelationEntry.Text;
            wvForm.emergency_client = contactClient;

            List<WV_HouseholdMembers> WVmembers = new List<WV_HouseholdMembers>();

            foreach (HouseholdComp houseMem in MembersColl)
            {
                WVmembers.Add(new WV_HouseholdMembers
                {
                    name = houseMem.MemberName,
                    last4_ss = int.Parse(houseMem.MemberSSN),
                    age = int.Parse(houseMem.MemberAge),
                    relationship = houseMem.MemberRelationship,
                    dob = houseMem.MemberDOB
                });
            }

            wvForm.household_members = WVmembers;
            wvForm.submit_date = dateEntry.Text;

            var submitFormJSONString = JsonConvert.SerializeObject(wvForm);
            var submitFormContent = new StringContent(submitFormJSONString, Encoding.UTF8, "application/json");
            Debug.WriteLine("WV submitForm Content: " + submitFormContent);
            var client = new HttpClient();
            var response = client.PostAsync("https://c1zwsl05s5.execute-api.us-west-1.amazonaws.com/dev/api/v2/households", submitFormContent);
            //await DisplayAlert("Success", "Profile updated!", "OK");
            Debug.WriteLine("RESPONSE TO WV HOUSEHOLDS   " + response.Result);
            Debug.WriteLine("WV HOUSEHOLDS JSON OBJECT BEING SENT: " + submitFormJSONString);

            await Navigation.PushAsync(new CheckoutPage(chosenFb, itemAmounts));
        }

        void backClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new MainPage();
            Navigation.PopAsync();
        }

        void prevClicked(System.Object sender, System.EventArgs e)
        {
            if (pageNum.Text == "Page 4 of 4")
            {
                pageNum.Text = "Page 3 of 4";
                fourthPage.IsVisible = false;
                thirdPage.IsVisible = true;
            }
            else if (pageNum.Text == "Page 3 of 4")
            {
                pageNum.Text = "Page 2 of 4";
                thirdPage.IsVisible = false;
                secondPage.IsVisible = true;
            }
            else if (pageNum.Text == "Page 2 of 4")
            {
                prevButton.IsVisible = false;
                pageNum.Text = "Page 1 of 4";
                secondPage.IsVisible = false;
                firstPage.IsVisible = true;
            }
        }

        void continueClicked(System.Object sender, System.EventArgs e)
        {
            if (pageNum.Text == "Page 1 of 4")
            {
                prevButton.IsVisible = true;
                pageNum.Text = "Page 2 of 4";
                firstPage.IsVisible = false;
                secondPage.IsVisible = true;
                scroller.ScrollToAsync(0, -50, true);
            }
            else if (pageNum.Text == "Page 2 of 4")
            {
                pageNum.Text = "Page 3 of 4";
                secondPage.IsVisible = false;
                thirdPage.IsVisible = true;
                scroller.ScrollToAsync(0, 0, true);
            }
            else //pageNum.Text == Page 3 of 4
            {
                pageNum.Text = "Page 4 of 4";
                thirdPage.IsVisible = false;
                fourthPage.IsVisible = true;
                scroller.ScrollToAsync(0, 0, true);
            }
        }

        void checkboxSelected(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            //Debug.WriteLine("prevHS: " + prevHS.ToString());
            //Debug.WriteLine("prevLS: " + prevLS.ToString());


            var checkbox1 = (CheckBox)sender;
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
            if (value.Substring(0, value.IndexOf(" ")) == "1")
            {
                //Debug.WriteLine("current living situation: " + livingSituation);

                if (prevHE == checkbox1 && checkbox1.IsChecked == false)
                {
                    homelessnessExtent = "";
                    prevHE = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevHE != null)
                {
                    prevHE.IsChecked = false;
                    prevHE = checkbox1;
                    homelessnessExtent = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    homelessnessExtent = value.Substring(value.IndexOf(" ") + 1);
                    prevHE = checkbox1;
                }
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "2")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevG == checkbox1 && checkbox1.IsChecked == false)
                {
                    gender = "";
                    prevG = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevG != null)
                {
                    prevG.IsChecked = false;
                    prevG = checkbox1;
                    gender = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    gender = value.Substring(value.IndexOf(" ") + 1);
                    prevG = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "3")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevMS == checkbox1 && checkbox1.IsChecked == false)
                {
                    maritalStatus = "";
                    prevMS = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevMS != null)
                {
                    prevMS.IsChecked = false;
                    prevMS = checkbox1;
                    maritalStatus = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    maritalStatus = value.Substring(value.IndexOf(" ") + 1);
                    prevMS = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "4")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevE == checkbox1 && checkbox1.IsChecked == false)
                {
                    education = "";
                    prevE = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevE != null)
                {
                    prevE.IsChecked = false;
                    prevE = checkbox1;
                    education = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    education = value.Substring(value.IndexOf(" ") + 1);
                    prevE = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "5")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevHG == checkbox1 && checkbox1.IsChecked == false)
                {
                    highestGrade = "";
                    prevHG = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevHG != null)
                {
                    prevHG.IsChecked = false;
                    prevHG = checkbox1;
                    highestGrade = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    highestGrade = value.Substring(value.IndexOf(" ") + 1);
                    prevHG = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "6")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevHO == checkbox1 && checkbox1.IsChecked == false)
                {
                    hispanicOrigin = "";
                    prevHO = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevHO != null)
                {
                    prevHO.IsChecked = false;
                    prevHO = checkbox1;
                    hispanicOrigin = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    hispanicOrigin = value.Substring(value.IndexOf(" ") + 1);
                    prevHO = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "7")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevPE == checkbox1 && checkbox1.IsChecked == false)
                {
                    primaryEthnicity = "";
                    prevPE = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevPE != null)
                {
                    prevPE.IsChecked = false;
                    prevPE = checkbox1;
                    primaryEthnicity = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    primaryEthnicity = value.Substring(value.IndexOf(" ") + 1);
                    prevPE = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "8")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevV == checkbox1 && checkbox1.IsChecked == false)
                {
                    veteran = "";
                    prevV = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevV != null)
                {
                    prevV.IsChecked = false;
                    prevV = checkbox1;
                    veteran = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    veteran = value.Substring(value.IndexOf(" ") + 1);
                    prevV = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "9")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevD == checkbox1 && checkbox1.IsChecked == false)
                {
                    disability = "";
                    prevD = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevD != null)
                {
                    prevD.IsChecked = false;
                    prevD = checkbox1;
                    disability = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    disability = value.Substring(value.IndexOf(" ") + 1);
                    prevD = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "10")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevDD == checkbox1 && checkbox1.IsChecked == false)
                {
                    disabilityDesc = "";
                    prevDD = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevDD != null)
                {
                    prevDD.IsChecked = false;
                    prevDD = checkbox1;
                    disabilityDesc = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    disabilityDesc = value.Substring(value.IndexOf(" ") + 1);
                    prevDD = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "11")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevPL == checkbox1 && checkbox1.IsChecked == false)
                {
                    primaryLang = "";
                    prevPL = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevPL != null)
                {
                    prevPL.IsChecked = false;
                    prevPL = checkbox1;
                    primaryLang = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    primaryLang = value.Substring(value.IndexOf(" ") + 1);
                    prevPL = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "12")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevEF == checkbox1 && checkbox1.IsChecked == false)
                {
                    engFluency = "";
                    prevEF = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevEF != null)
                {
                    prevEF.IsChecked = false;
                    prevEF = checkbox1;
                    engFluency = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    engFluency = value.Substring(value.IndexOf(" ") + 1);
                    prevEF = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "13")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevES == checkbox1 && checkbox1.IsChecked == false)
                {
                    employmentStatus = "";
                    prevES = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevES != null)
                {
                    prevES.IsChecked = false;
                    prevES = checkbox1;
                    employmentStatus = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    employmentStatus = value.Substring(value.IndexOf(" ") + 1);
                    prevES = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "14")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevMI == checkbox1 && checkbox1.IsChecked == false)
                {
                    medicalInsurance = "";
                    prevMI = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevMI != null)
                {
                    prevMI.IsChecked = false;
                    prevMI = checkbox1;
                    medicalInsurance = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    medicalInsurance = value.Substring(value.IndexOf(" ") + 1);
                    prevMI = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "15")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevSN == checkbox1 && checkbox1.IsChecked == false)
                {
                    specialNutrition = "";
                    prevSN = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevSN != null)
                {
                    prevSN.IsChecked = false;
                    prevSN = checkbox1;
                    specialNutrition = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    specialNutrition = value.Substring(value.IndexOf(" ") + 1);
                    prevSN = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else if (value.Substring(0, value.IndexOf(" ")) == "16")
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevC == checkbox1 && checkbox1.IsChecked == false)
                {
                    calfresh = "";
                    prevC = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevC != null)
                {
                    prevC.IsChecked = false;
                    prevC = checkbox1;
                    calfresh = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    calfresh = value.Substring(value.IndexOf(" ") + 1);
                    prevC = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
            //living situation
            else
            {
                //Debug.WriteLine("current housing status: " + housingStatus);

                if (prevCC == checkbox1 && checkbox1.IsChecked == false)
                {
                    contactClient = "";
                    prevCC = null;
                }
                else if (checkbox1.IsChecked == false)
                { }
                else if (prevCC != null)
                {
                    prevCC.IsChecked = false;
                    prevCC = checkbox1;
                    contactClient = value.Substring(value.IndexOf(" ") + 1);
                }
                else
                {
                    contactClient = value.Substring(value.IndexOf(" ") + 1);
                    prevCC = checkbox1;
                }
                //livingSituation = value.Substring(1);
            }
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
            Application.Current.MainPage = new LoginPage();
        }

        //end of menu functions
    }
}
