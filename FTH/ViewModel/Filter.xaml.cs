using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using FTH.Model;
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
        public ObservableCollection<FoodBanks> Banks = new ObservableCollection<FoodBanks>();

        public Filter()
        {
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
            
        }

        void getFoodBanks()
        {
            for (int i = 0; i < 11; i++)
            {
                Banks.Add(new FoodBanks
                {
                    name = "Feeding Orange County",
                    HoursVisible = false
                });
            }

            foodBankColl.ItemsSource = Banks;
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

        void clickedShowDates(System.Object sender, System.EventArgs e)
        {
            if (dateCarousel.IsVisible == true)
            {
                dateCarousel.IsVisible = false;
                clearDatesGrid.IsVisible = false;
            }
            else
            {
                dateCarousel.IsVisible = true;
                clearDatesGrid.IsVisible = true;
            }
        }

        void clickedClearDates(System.Object sender, System.EventArgs e)
        {
            selectedDate.BackgroundImg = "dateUnselected.png";
            selectedDate.TextColor = Color.Black;

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

        void clickedShowHours(System.Object sender, System.EventArgs e)
        {
            ImageButton button1 = (ImageButton)sender;
            FoodBanks fbChosen = button1.BindingContext as FoodBanks;
            if (fbChosen.HoursVisible == true)
                fbChosen.HoursVisible = false;
            else fbChosen.HoursVisible = true;
        }
    }
}
