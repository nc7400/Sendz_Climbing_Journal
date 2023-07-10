using Sendz_Climbing_Journal.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sendz_Climbing_Journal
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var loginPage = new LoginPage();
            var navPage = new NavigationPage(loginPage);
            MainPage = navPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
