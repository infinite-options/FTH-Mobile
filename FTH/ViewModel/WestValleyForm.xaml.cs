using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FTH.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FTH.ViewModel
{
    public partial class WestValleyForm : ContentPage
    {
        public ObservableCollection<HouseholdComp> MembersColl = new ObservableCollection<HouseholdComp>();
        public Dictionary<StoreItem, int> itemAmounts = new Dictionary<StoreItem, int>();
        int memberNum;

        public WestValleyForm(Dictionary<StoreItem, int> itmAmts)
        {
            itemAmounts = itmAmts;
            memberNum = 1;
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

        void submitClicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new CheckoutPage(itemAmounts));
        }

        void backClicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new MainPage();
            Navigation.PopAsync();
        }

        void continueClicked(System.Object sender, System.EventArgs e)
        {
            if (pageNum.Text == "Page 1 of 4")
            {
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
            else
            {
                pageNum.Text = "Page 4 of 4";
                thirdPage.IsVisible = false;
                fourthPage.IsVisible = true;
                scroller.ScrollToAsync(0, 0, true);
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
