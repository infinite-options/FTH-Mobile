using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FTH.ViewModel
{
    public partial class LoginPage : ContentPage
    {
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
    }
}
