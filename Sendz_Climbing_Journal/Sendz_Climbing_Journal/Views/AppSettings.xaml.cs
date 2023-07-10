using Sendz_Climbing_Journal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sendz_Climbing_Journal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppSettings : ContentPage
    {
        public AppSettings()
        {
            InitializeComponent();
        }

        private void ClearPreferences_Clicked(object sender, EventArgs e)
        {
            Preferences.Clear();
        }

        async void LoadSampleData_Clicked(object sender, EventArgs e)
        {
            if (Settings.FirstRun)
            {
                DatabaseServices.LoadSampleData();
                Settings.FirstRun = false;

                await Navigation.PopAsync();
            }
        }

        async void ClearSampleData_Clicked(object sender, EventArgs e)
        {
            await DatabaseServices.ClearSampleData();

            await Navigation.PopAsync();
        }
    }
}