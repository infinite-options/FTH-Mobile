using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using FTH.Model;
using FTH.Model.Login.LoginClasses;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using FTH.Constants;
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Xamarin.Forms.Maps;
using FTH.ViewModel;

namespace FTH.ViewModel
{
    public partial class CongratsPage : ContentPage
    {

        public CongratsPage()
        {
            try
            {
                var width = DeviceDisplay.MainDisplayInfo.Width;
                var height = DeviceDisplay.MainDisplayInfo.Height;
                InitializeComponent();

            }
            catch (Exception ex)
            {
                //Generic gen = new Generic();
                //gen.parseException(ex.ToString());
            }
        }

        async void clickedFilter(System.Object sender, System.EventArgs e)
        {
            //await Navigation.PushAsync(new NavigationPage(new Filter()));
            Application.Current.MainPage = new NavigationPage(new Filter());
        }

        //async void clickedMenu(System.Object sender, System.EventArgs e)
        //{
        //    await Navigation.PushAsync(new Menu(cust_firstName, cust_lastName, cust_email));
        //}


    }
}
