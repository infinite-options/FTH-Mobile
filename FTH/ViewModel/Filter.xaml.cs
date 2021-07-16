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
        List<ImageButton> selectedTypes;
        public ObservableCollection<FoodBanks> Banks = new ObservableCollection<FoodBanks>();

        public Filter()
        {
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
            //if the item is unfilled
            if (imgButton.Source.ToString().IndexOf("Unfilled") != -1)
            {
                string source = imgButton.Source.ToString().Substring(6);
                source = source.Substring(0, source.IndexOf("Unfilled")) + "Filled.png";
                //Debug.WriteLine("source to change to: $" + source + "$");
                imgButton.Source = source;
                selectedTypes.Add(imgButton);

                pickTypeFrame.BackgroundColor = Color.FromHex("#E7404A");
                pickTypeButton.TextColor = Color.White;
            }
            else
            {
                string source = imgButton.Source.ToString().Substring(6);
                source = source.Substring(0, source.IndexOf("Filled")) + "Unfilled.png";
                imgButton.Source = source;
                selectedTypes.Remove(imgButton);

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
    }
}
