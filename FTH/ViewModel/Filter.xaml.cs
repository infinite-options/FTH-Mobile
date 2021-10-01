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
    public partial class Filter : ContentPage
    {
        List<Date1> availableDates;
        Date1 selectedDate;
        //allowing multiple date selections
        //List<Date1> selectedDates;
        List<ImageButton> selectedTypes;
        //holds all of the banks (doesn't get changed)
        public ObservableCollection<FoodBanks> totalBanksColl = new ObservableCollection<FoodBanks>();
        //holds all of the banks, the values store the amount of filters applied to hide a certain foodbank (ex: not delivery and not fruit)
        Dictionary<FoodBanks, int> totalBanks = new Dictionary<FoodBanks, int>();
        //the current list of banks showing on the screen
        ObservableCollection<FoodBanks> currentList = new ObservableCollection<FoodBanks>();

        public Filter()
        {
            Debug.WriteLine("user id: " + Application.Current.Properties["user_id"]);

            selectedTypes = new List<ImageButton>();
            availableDates = new List<Date1>();
            //allowing multiple date selections
            //selectedDates = new List<Date1>();
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;
            Console.WriteLine("Width = " + width.ToString());
            Console.WriteLine("Height = " + height.ToString());

            InitializeComponent();

            getDates();
            getFoodBanks();
            Debug.WriteLine("availableDates size: " + availableDates.Count);

            Debug.WriteLine("main grid height: " + mainGrid.HeightRequest);
            //Debug.WriteLine("topstack height: " +;
            Debug.WriteLine("foodbankcoll height: " + foodBankColl.HeightRequest);
            //foodBankColl.HeightRequest = height - 500;
            Debug.WriteLine("foodbankcoll height after: " + foodBankColl.HeightRequest);
        }

        async void getFoodBanks()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://c1zwsl05s5.execute-api.us-west-1.amazonaws.com/dev/api/v2/businesses");
            Debug.WriteLine("BUSINESSES ENDPOINT TRYING TO BE REACHED: " + "https://c1zwsl05s5.execute-api.us-west-1.amazonaws.com/dev/api/v2/businesses");
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            var RDSMessage = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("RDSMessage from businesses endpoint: " + RDSMessage.ToString());
            var data = JsonConvert.DeserializeObject<BusinessesResponse>(RDSMessage);

            foreach (var bus in data.result.result)
            {
                FoodBanks bank = new FoodBanks();
                bank.name = bus.business_name;
                bank.business_uid = bus.business_uid;
                bank.bankImg = bus.business_image;
                bank.TotalVisible = true;
                bank.HoursVisible = false;

                if (bus.delivery != null)
                {
                    if (bus.delivery == "1")
                    {
                        bank.delivery = true;
                        bank.desc = "Delivery.";
                    }
                    else bank.delivery = false;
                }

                if (bus.pick_up != null)
                {
                    if (bus.pick_up == "1")
                    {
                        bank.pickup = true;
                        bank.desc += " Pick-Up.";
                    }
                    else bank.pickup = false;
                }

                //bank.pickup = true;

                try
                {
                    bank.itemLimit = int.Parse(bus.limit_per_person);
                    bank.desc += " Limited to " + bus.limit_per_person + " items.";
                }
                catch
                {
                    bank.itemLimit = 0;
                    bank.desc += " Limited to 0 items.";
                }

                if (bus.business_accepting_hours != null)
                {
                    try
                    {
                        Debug.WriteLine("business hours: " + bus.business_accepting_hours);
                        var hoursdata = JsonConvert.DeserializeObject<AcceptingHours>(bus.business_accepting_hours);

                        if (hoursdata.Monday[0] == "N/A")
                            bank.mondayHours = "Closed";
                        else bank.mondayHours = hoursdata.Monday[0].Substring(0, 5) + " - " + hoursdata.Monday[1].Substring(0, 5);

                        if (hoursdata.Tuesday[0] == "N/A")
                            bank.tuesdayHours = "Closed";
                        else bank.tuesdayHours = hoursdata.Tuesday[0].Substring(0, 5) + " - " + hoursdata.Tuesday[1].Substring(0, 5);

                        if (hoursdata.Wednesday[0] == "N/A")
                            bank.wednesdayHours = "Closed";
                        else bank.wednesdayHours = hoursdata.Wednesday[0].Substring(0, 5) + " - " + hoursdata.Wednesday[1].Substring(0, 5);

                        if (hoursdata.Thursday[0] == "N/A")
                            bank.thursdayHours = "Closed";
                        else bank.thursdayHours = hoursdata.Thursday[0].Substring(0, 5) + " - " + hoursdata.Thursday[1].Substring(0, 5);

                        if (hoursdata.Friday[0] == "N/A")
                            bank.fridayHours = "Closed";
                        else bank.fridayHours = hoursdata.Friday[0].Substring(0, 5) + " - " + hoursdata.Friday[1].Substring(0, 5);

                        if (hoursdata.Saturday[0] == "N/A")
                            bank.saturdayHours = "Closed";
                        else bank.saturdayHours = hoursdata.Saturday[0].Substring(0, 5) + " - " + hoursdata.Saturday[1].Substring(0, 5);

                        if (hoursdata.Sunday[0] == "N/A")
                            bank.sundayHours = "Closed";
                        else bank.sundayHours = hoursdata.Sunday[0].Substring(0, 5) + " - " + hoursdata.Sunday[1].Substring(0, 5);
                    }
                    catch
                    {
                        bank.mondayHours = "Closed";
                        bank.tuesdayHours = "Closed";
                        bank.wednesdayHours = "Closed";
                        bank.thursdayHours = "Closed";
                        bank.fridayHours = "Closed";
                        bank.saturdayHours = "Closed";
                        bank.sundayHours = "Closed";
                    }
                }

                if (bus.item_types != null)
                {
                    Debug.WriteLine("item types: " + bus.item_types);
                    try
                    {
                        var typesdata = JsonConvert.DeserializeObject<Types>(bus.item_types);
                        bank.fruits = typesdata.fruits;
                        bank.vegetables = typesdata.vegetables;
                        bank.meals = typesdata.meals;
                        bank.desserts = typesdata.desserts;
                        bank.beverages = typesdata.beverages;
                        bank.dairy = typesdata.dairy;
                        bank.snacks = typesdata.snacks;
                        bank.cannedFoods = typesdata.cannedFoods;
                    }
                    catch
                    {
                        Debug.WriteLine("didn't work");
                        bank.fruits = false;
                        bank.vegetables = false;
                        bank.meals = false;
                        bank.desserts = false;
                        bank.beverages = false;
                        bank.dairy = false;
                        bank.snacks = false;
                        bank.cannedFoods = false;
                    }
                }

                bank.FilterCount = 0;
                bank.Height = 90;
                bank.distance = "1.5 Miles";

                totalBanksColl.Add(bank);
                totalBanks.Add(bank, 0);
            }

            //for (int i = 0; i < 11; i++)
            //{
            //    Banks.Add(new FoodBanks
            //    {
            //        name = "Feeding Orange County",
            //        HoursVisible = false
            //    });
            //}

            foodBankColl.ItemsSource = totalBanksColl;
            currentList = totalBanksColl;
        }

        void getDates()
        {
            Date1 newDate = new Date1();
            newDate.BackgroundImg = "dateUnselected.png";
            newDate.dotw = "S";
            newDate.day = "27";
            newDate.month = "Jun";
            newDate.TextColor = Color.Black;
            availableDates.Add(newDate);

            for (int i = 0; i < 11; i++)
            {
                availableDates.Add(new Date1
                {
                    BackgroundImg = "dateUnselected.png",
                    dotw = "S",
                    day = "27",
                    month = "Jun",
                    TextColor = Color.Black

                });
            }

            dateCarousel.ItemsSource = availableDates;
        }

        private void dateChange(object sender, EventArgs e)
        {
            Button button1 = (Button)sender;
            Date1 dateChosen = button1.BindingContext as Date1;

            if (dateChosen.BackgroundImg == "dateUnselected.png")
            {
                if (selectedDate != null)
                {
                    selectedDate.BackgroundImg = "dateUnselected.png";
                    selectedDate.TextColor = Color.Black;
                }
                selectedDate = dateChosen;
                dateChosen.BackgroundImg = "dateSelected.png";
                dateChosen.TextColor = Color.White;
                pickDateButton.Text = dateChosen.month + " " + dateChosen.day;
                //allowing multiple date selections
                //dateChosen.BackgroundImg = "dateSelected.png";
                //dateChosen.TextColor = Color.White;
                //selectedDates.Add(dateChosen);

                pickDateFrame.BackgroundColor = Color.FromHex("#E7404A"); 
                pickDateButton.TextColor = Color.White;
            }
            else
            {
                selectedDate = null;
                pickDateFrame.BackgroundColor = Color.White;
                pickDateButton.TextColor = Color.FromHex("#E7404A");
                pickDateButton.Text = "Pick a date";
                //allowing multiple date selections
                dateChosen.BackgroundImg = "dateUnselected.png";
                dateChosen.TextColor = Color.Black;
                //selectedDates.Remove(dateChosen);

                //if (selectedDates.Count == 0)
                //{
                //    pickDateFrame.BackgroundColor = Color.White;
                //    pickDateButton.TextColor = Color.FromHex("#E7404A");
                //}
            }
        }

        void clickedDelivery(System.Object sender, System.EventArgs e)
        {
            foodBankColl.IsVisible = false;
            foodBankColl.ScrollTo(0);
            //if the button is already selected, selected --> unselected
            if (deliveryButton.TextColor == Color.White)
            {
                deliveryFrame.BackgroundColor = Color.White;
                deliveryButton.TextColor = Color.FromHex("#E7404A");
                ObservableCollection<FoodBanks> newBanks = new ObservableCollection<FoodBanks>();

                foreach (var bank in totalBanksColl)
                {
                    //only show it if there are no more filters on it
                    if (!bank.delivery && totalBanks[bank] > 1)
                    {
                        totalBanks[bank]--;
                    }
                    else if (!bank.delivery && totalBanks[bank] == 1)
                    {
                        totalBanks[bank]--;
                        newBanks.Add(bank);
                    }
                    else if (totalBanks[bank] == 0)
                        newBanks.Add(bank);
                }

                currentList = newBanks;
                foodBankColl.ItemsSource = newBanks;

                //foreach (var bank in totalBanksColl)
                //{
                //    //only show it if there are no more filters on it
                //    if (!bank.desc.Contains("Delivery") && bank.FilterCount == 1)
                //    {
                //        bank.TotalVisible = true;
                //        //bank.Height = 90;
                //        bank.FilterCount--;
                //    }
                //}
                //foodBankColl.ItemsSource = Banks;
            }
            else //unselected --> selected
            {
                deliveryFrame.BackgroundColor = Color.FromHex("#E7404A");
                deliveryButton.TextColor = Color.White;
                ObservableCollection<FoodBanks> newBanks = new ObservableCollection<FoodBanks>();

                //foreach(var bank in totalBanksColl)
                //{
                //    newBanks.Add(bank);
                //}

                foreach (var bank in totalBanksColl)
                {
                    if (!bank.delivery)
                    {
                        totalBanks[bank] += 1;
                    }
                    else if (currentList.Contains(bank))
                        newBanks.Add(bank);
                }

                currentList = newBanks;
                foodBankColl.ItemsSource = newBanks;
                //for (int i = 0; i < totalBanks.Keys.Count; i++)
                //{
                //    if (totalBanks.Keys)
                //}

                //foreach (var bank in Banks)
                //{
                //    //bank.FilterCount++;

                //    if (!bank.delivery)
                //    {
                //        bank.TotalVisible = false;
                //        bank.FilterCount++;
                //    }

                //}
            }

            foodBankColl.IsVisible = true;
        }

        void clickedShowDates(System.Object sender, System.EventArgs e)
        {
            if (dateCarousel.IsVisible == true)
            {
                dateCarousel.IsVisible = false;
                clearDatesGrid.IsVisible = false;
            }
            else
            {
                if (TypesGrid.IsVisible == true)
                {
                    TypesGrid.IsVisible = false;
                    clearTypesGrid.IsVisible = false;
                }

                dateCarousel.IsVisible = true;
                clearDatesGrid.IsVisible = true;
            }
        }

        void clickedClearDates(System.Object sender, System.EventArgs e)
        {
            selectedDate.BackgroundImg = "dateUnselected.png";
            selectedDate.TextColor = Color.Black;
            pickDateButton.Text = "Pick a date";
            //allowing multiple date selections
            //foreach (Date1 date in selectedDates)
            //{
            //    date.BackgroundImg = "dateUnselected.png";
            //    date.TextColor = Color.Black;
            //}
            //selectedDates.Clear();
            pickDateFrame.BackgroundColor = Color.White;
            pickDateButton.TextColor = Color.FromHex("#E7404A");
        }

        void clickedShowTypes(System.Object sender, System.EventArgs e)
        {
            if (TypesGrid.IsVisible == true)
            {
                TypesGrid.IsVisible = false;
                clearTypesGrid.IsVisible = false;
            }
            else
            {
                if (dateCarousel.IsVisible == true)
                {
                    dateCarousel.IsVisible = false;
                    clearDatesGrid.IsVisible = false;
                }

                TypesGrid.IsVisible = true;
                clearTypesGrid.IsVisible = true;
            }
        }

        void clickedChooseType(System.Object sender, System.EventArgs e)
        {
            ImageButton imgButton = (ImageButton)sender;
            Debug.WriteLine("source of imgbutton: " + imgButton.Source.ToString());
            //if the item is unfilled, unselected --> selected
            if (imgButton.Source.ToString().IndexOf("Unfilled") != -1)
            {
                string source = imgButton.Source.ToString().Substring(6);
                source = source.Substring(0, source.IndexOf("Unfilled")) + "Filled.png";
                //Debug.WriteLine("source to change to: $" + source + "$");
                imgButton.Source = source;
                selectedTypes.Add(imgButton);

                pickTypeFrame.BackgroundColor = Color.FromHex("#E7404A");
                pickTypeButton.TextColor = Color.White;
                ObservableCollection<FoodBanks> newBanks = new ObservableCollection<FoodBanks>();

                if (imgButton.Source.ToString().IndexOf("fruits") != -1)
                {
                    foreach(var bank in totalBanksColl)
                    {
                        if (!bank.fruits)
                        {
                            totalBanks[bank] += 1;
                        }
                        else if (currentList.Contains(bank))
                            newBanks.Add(bank);
                    }

                    //foreach (var bank in currentList)
                    //{
                    //    if (!bank.fruits)
                    //    {
                    //        totalBanks[bank] += 1;
                    //    }
                    //    else newBanks.Add(bank);
                    //}

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("vegetables") != -1)
                {
                    //foreach (var bank in currentList)
                    //{
                    //    if (!bank.vegetables)
                    //    {
                    //        totalBanks[bank] += 1;
                    //    }
                    //    else newBanks.Add(bank);
                    //}

                    foreach (var bank in totalBanksColl)
                    {
                        if (!bank.vegetables)
                        {
                            totalBanks[bank] += 1;
                        }
                        else if (currentList.Contains(bank))
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("meals") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        if (!bank.meals)
                        {
                            totalBanks[bank] += 1;
                        }
                        else if (currentList.Contains(bank))
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("desserts") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        if (!bank.desserts)
                        {
                            totalBanks[bank] += 1;
                        }
                        else if (currentList.Contains(bank))
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("beverages") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        if (!bank.beverages)
                        {
                            totalBanks[bank] += 1;
                        }
                        else if (currentList.Contains(bank))
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("dairy") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        if (!bank.dairy)
                        {
                            totalBanks[bank] += 1;
                        }
                        else if (currentList.Contains(bank))
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("snacks") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        if (!bank.snacks)
                        {
                            totalBanks[bank] += 1;
                        }
                        else if (currentList.Contains(bank))
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("cannedFoods") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        if (!bank.cannedFoods)
                        {
                            totalBanks[bank] += 1;
                        }
                        else if (currentList.Contains(bank))
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
            }
            else
            {
                string source = imgButton.Source.ToString().Substring(6);
                source = source.Substring(0, source.IndexOf("Filled")) + "Unfilled.png";
                imgButton.Source = source;
                selectedTypes.Remove(imgButton);
                ObservableCollection<FoodBanks> newBanks = new ObservableCollection<FoodBanks>();

                if (imgButton.Source.ToString().IndexOf("fruits") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        //only show it if there are no more filters on it
                        if (!bank.fruits && totalBanks[bank] > 1)
                        {
                            totalBanks[bank]--;
                        }
                        else if (!bank.fruits && totalBanks[bank] == 1)
                        {
                            totalBanks[bank]--;
                            newBanks.Add(bank);
                        }
                        else if (totalBanks[bank] == 0)
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("vegetables") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        //only show it if there are no more filters on it
                        if (!bank.fruits && totalBanks[bank] > 1)
                        {
                            totalBanks[bank]--;
                        }
                        else if (!bank.vegetables && totalBanks[bank] == 1)
                        {
                            totalBanks[bank]--;
                            newBanks.Add(bank);
                        }
                        else if (totalBanks[bank] == 0)
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("meals") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        //only show it if there are no more filters on it
                        if (!bank.meals && totalBanks[bank] > 1)
                        {
                            totalBanks[bank]--;
                        }
                        else if (!bank.meals && totalBanks[bank] == 1)
                        {
                            totalBanks[bank]--;
                            newBanks.Add(bank);
                        }
                        else if (totalBanks[bank] == 0)
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("desserts") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        //only show it if there are no more filters on it
                        if (!bank.desserts && totalBanks[bank] > 1)
                        {
                            totalBanks[bank]--;
                        }
                        else if (!bank.desserts && totalBanks[bank] == 1)
                        {
                            totalBanks[bank]--;
                            newBanks.Add(bank);
                        }
                        else if (totalBanks[bank] == 0)
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("beverages") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        //only show it if there are no more filters on it
                        if (!bank.beverages && totalBanks[bank] > 1)
                        {
                            totalBanks[bank]--;
                        }
                        else if (!bank.beverages && totalBanks[bank] == 1)
                        {
                            totalBanks[bank]--;
                            newBanks.Add(bank);
                        }
                        else if (totalBanks[bank] == 0)
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("dairy") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        //only show it if there are no more filters on it
                        if (!bank.dairy && totalBanks[bank] > 1)
                        {
                            totalBanks[bank]--;
                        }
                        else if (!bank.dairy && totalBanks[bank] == 1)
                        {
                            totalBanks[bank]--;
                            newBanks.Add(bank);
                        }
                        else if (totalBanks[bank] == 0)
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("snacks") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        //only show it if there are no more filters on it
                        if (!bank.snacks && totalBanks[bank] > 1)
                        {
                            totalBanks[bank]--;
                        }
                        else if (!bank.snacks && totalBanks[bank] == 1)
                        {
                            totalBanks[bank]--;
                            newBanks.Add(bank);
                        }
                        else if (totalBanks[bank] == 0)
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }
                else if (imgButton.Source.ToString().IndexOf("cannedFoods") != -1)
                {
                    foreach (var bank in totalBanksColl)
                    {
                        //only show it if there are no more filters on it
                        if (!bank.cannedFoods && totalBanks[bank] > 1)
                        {
                            totalBanks[bank]--;
                        }
                        else if (!bank.cannedFoods && totalBanks[bank] == 1)
                        {
                            totalBanks[bank]--;
                            newBanks.Add(bank);
                        }
                        else if (totalBanks[bank] == 0)
                            newBanks.Add(bank);
                    }

                    currentList = newBanks;
                    foodBankColl.ItemsSource = newBanks;
                }

                if (selectedTypes.Count == 0)
                {
                    pickTypeFrame.BackgroundColor = Color.White;
                    pickTypeButton.TextColor = Color.FromHex("#E7404A");
                }
            }
        }

        void clickedClearTypes(System.Object sender, System.EventArgs e)
        {
            foreach (ImageButton imgButton in selectedTypes)
            {
                string source = imgButton.Source.ToString().Substring(6);
                source = source.Substring(0, source.IndexOf("Filled")) + "Unfilled.png";
                imgButton.Source = source;
            }
            selectedTypes.Clear();
            pickTypeFrame.BackgroundColor = Color.White;
            pickTypeButton.TextColor = Color.FromHex("#E7404A");
        }

        

        void clickedShowHours(System.Object sender, System.EventArgs e)
        {
            ImageButton button1 = (ImageButton)sender;
            FoodBanks fbChosen = button1.BindingContext as FoodBanks;
            if (fbChosen.HoursVisible == true)
                fbChosen.HoursVisible = false;
            else fbChosen.HoursVisible = true;
        }
        
        async void clickedFoodBank(System.Object sender, System.EventArgs e)
        {
            Button button1 = (Button)sender;
            FoodBanks bankChosen = button1.BindingContext as FoodBanks;
            Application.Current.Properties["chosen_business_uid"] = bankChosen.business_uid;
            Debug.WriteLine("sending name: " + bankChosen.name);
            Debug.WriteLine("sending distance: " + bankChosen.distance);
            Debug.WriteLine("sending bankImg: " + bankChosen.bankImg);
            Debug.WriteLine("sending itemLimit: " + bankChosen.itemLimit);
            Debug.WriteLine("sending business_uid: " + bankChosen.business_uid);
            await Navigation.PushAsync(new FoodBackStore(bankChosen.name, bankChosen.distance, bankChosen.bankImg, bankChosen.itemLimit, bankChosen.business_uid));
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
            whiteCover.IsVisible = true;
        }

        void openedMenuClicked(System.Object sender, System.EventArgs e)
        {
            openMenuGrid.IsVisible = false;
            whiteCover.IsVisible = false;
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
