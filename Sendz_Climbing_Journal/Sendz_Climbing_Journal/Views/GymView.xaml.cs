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
    public partial class GymView : ContentPage
    {
        private int selectedGymLogId;

        public GymView()
        {
            InitializeComponent();
        }

        public GymView(Gym gym)
        {
            InitializeComponent();
            selectedGymLogId = gym.Id;

            GymName.Text = gym.GymName;
            TypePicker.SelectedItem = gym.Type;
            GradePicker.SelectedItem = gym.Grade;
            SentSwitch.IsToggled = gym.Sent;
            SendTypePicker.SelectedItem = gym.SendType;
            SendDatePicker.Date = gym.SendDate;
            ClimbDescription.Text = gym.Description;
            LogNotes.Text = gym.Notes;
        }

        #region Button Methods

        async void ShareLog_Clicked(object sender, EventArgs e)
        {
            var climbLog =
                GymName.Text + "\r\n" +
                TypePicker.SelectedItem.ToString() + "\r\n" +
                GradePicker.SelectedItem.ToString() + "\r\n" +
                SendTypePicker.SelectedItem.ToString() + "\r\n" +
                SendDatePicker.Date.ToString() + "\r\n" +
                ClimbDescription.Text + "\r\n" +
                LogNotes.Text;

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = climbLog,
                Title = "Share Journal Entry"
            });
        }

        async void SaveEdit_Clicked(object sender, EventArgs e)
        {
            #region Textbox Validation

            if (string.IsNullOrWhiteSpace(GymName.Text))
            {
                await DisplayAlert("Missing Name", "Please Enter the Name of the Climb.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(TypePicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing Type", "Please Pick the Climb Type.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(GradePicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing Grade", "Please Pick the Climb Grade.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(SendTypePicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing Send Type", "Please Pick the Send Type.", "OK");
                return;
            }

            #endregion

            var answer = await DisplayAlert("Update Climb", "Update this Journal Entry?", "Yes", "No");

            if (answer == true)
            {
                try
                {
                    await DatabaseServices.UpdateGymLog(selectedGymLogId, GymName.Text, TypePicker.SelectedItem.ToString(),
                        GradePicker.SelectedItem.ToString(), SentSwitch.IsToggled, SendTypePicker.SelectedItem.ToString(),
                        SendDatePicker.Date, ClimbDescription.Text, LogNotes.Text);

                    await DisplayAlert("Climb Log Updated", "Journal Entry Successfully Updated!", "OK");

                    await Navigation.PopAsync();
                }
                catch (Exception)
                {
                    await DisplayAlert("Error", "Unable to Update Journal Entry.", "OK");

                    await Navigation.PopAsync();
                }
            }
            else
            {
                return;
            }
        }

        async void DeleteLog_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete Climb", "Delete this Journal Entry?", "Yes", "No");

            if (answer == true)
            {
                try
                {
                    await DatabaseServices.RemoveGymLog(selectedGymLogId);

                    await DisplayAlert("Climb Log Deleted", "Journal Entry Successfully Deleted.", "OK");

                    await Navigation.PopAsync();
                }
                catch (Exception)
                {
                    await DisplayAlert("Error", "Unable to Delete Journal Entry.", "OK");
                }
            }
        }

        #endregion
    }
}