using Sendz_Climbing_Journal.Models;
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
    public partial class Dashboard : TabbedPage
    {
        private int selectedUserId;
        public List<Dashboard> feedList = new List<Dashboard>();

        public Dashboard()
        {
            InitializeComponent();
        }

        public Dashboard(int userId)
        {
            InitializeComponent();

            selectedUserId = userId;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //Sets the itemsSource for the collectionViews
            CragCollectionView.ItemsSource = await DatabaseServices.GetCragLogs(selectedUserId);
            GymCollectionView.ItemsSource = await DatabaseServices.GetGymLogs(selectedUserId);


            //Sets the number of logs as a count below the searchbars
            int cragLogCount = await DatabaseServices.GetCragLogCount(selectedUserId);
            CragResultsCount.Text = cragLogCount.ToString() + " Results";

            int gymLogCount = await DatabaseServices.GetGymLogCount(selectedUserId);
            GymResultsCount.Text = gymLogCount.ToString() + " Results";


            //Sets the titlebar Name specific to the user
            var userRecord = await DatabaseServices.GetUserById(selectedUserId);

            if (userRecord != null)
            {
                foreach (User user in userRecord)
                {
                    UserTitle.Text = user.Name + "'s Climbing Journal";
                }
            }
            
        }

        #region CragCollection Methods

        async void CragCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cragLog = (Crag)e.CurrentSelection.FirstOrDefault();

            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new CragView(cragLog));
            }
        }

        async void CragSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var cragLogs = await DatabaseServices.GetCragLogs(selectedUserId);

            CragCollectionView.ItemsSource = 
                cragLogs.Where(i => i.Name.ToLower().Contains(e.NewTextValue.ToLower())
                                    | i.CragName.ToLower().Contains(e.NewTextValue.ToLower()) 
                                    | i.Grade.ToLower().Contains(e.NewTextValue.ToLower())
                                    | i.Type.ToLower().Contains(e.NewTextValue.ToLower())
                                    | i.SendDate.ToString().Contains(e.NewTextValue));

            //Updates results count as user searches
            int cragLogCount = 0;

            foreach (var item in CragCollectionView.ItemsSource)
            {
                cragLogCount++;
            }

            CragResultsCount.Text = cragLogCount.ToString() + " Results";
        }

        async void CragLogButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CragAdd(selectedUserId));
        }

        #endregion

        #region GymCollection Methods

        async void GymCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gymLog = (Gym)e.CurrentSelection.FirstOrDefault();

            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new GymView(gymLog));
            }
        }
        async void GymSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var gymLogs = await DatabaseServices.GetGymLogs(selectedUserId);

            GymCollectionView.ItemsSource = 
                gymLogs.Where(i => i.GymName.ToLower().Contains(e.NewTextValue.ToLower())
                                   | i.Grade.ToLower().Contains(e.NewTextValue.ToLower())
                                   | i.Type.ToLower().Contains(e.NewTextValue.ToLower())
                                   | i.SendDate.ToString().Contains(e.NewTextValue));

            //Updates results count as user searches
            int gymLogCount = 0;

            foreach (var item in GymCollectionView.ItemsSource)
            {
                gymLogCount++;
            }

            GymResultsCount.Text = gymLogCount.ToString() + " Results";

        }

        async void GymLogButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GymAdd(selectedUserId));
        }

        #endregion

        #region Settings Menu Methods

        async void UpdateUser_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserEdit(selectedUserId));
        }

        async void Logout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        #endregion


    }
}