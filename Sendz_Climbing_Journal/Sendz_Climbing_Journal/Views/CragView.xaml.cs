using Sendz_Climbing_Journal.Models;
using Sendz_Climbing_Journal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sendz_Climbing_Journal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CragView : ContentPage
    {
        private int selectedCragLogId;

        public CragView()
        {
            InitializeComponent();
        }

        public CragView(Crag crag)
        {
            InitializeComponent();

            selectedCragLogId = crag.Id;

            string grade = crag.Grade.Remove(crag.Grade.Length - 1, 1);
            char subGrade = crag.Grade.Last();

            ClimbName.Text = crag.Name;
            CragName.Text = crag.CragName;
            StatePicker.SelectedItem = crag.State;
            TypePicker.SelectedItem = crag.Type;
            GradePicker.SelectedItem = grade;
            SubGradePicker.SelectedItem = subGrade.ToString();
            SentSwitch.IsToggled = crag.Sent;
            SendTypePicker.SelectedItem = crag.SendType;
            SendDatePicker.Date = crag.SendDate;
            LogNotes.Text = crag.Notes;
        }

        #region Button Methods

        async void SaveEdit_Clicked(object sender, EventArgs e)
        {
            #region TextBox Validation

            if (string.IsNullOrWhiteSpace(ClimbName.Text))
            {
                await DisplayAlert("Missing Name", "Please Enter the Name of the Climb.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(CragName.Text))
            {
                await DisplayAlert("Missing Crag Name", "Please Enter the Name of the Crag.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(StatePicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing State", "Please Pick a State.", "OK");
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

            if (string.IsNullOrWhiteSpace(SubGradePicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing Sub-Grade", "Please Pick the Climb Sub-Grade.", "OK");
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
                //Combines grade and subgrade to make a single entry
                string completeGrade = GradePicker.SelectedItem.ToString() + SubGradePicker.SelectedItem.ToString();

                try
                {
                    await DatabaseServices.UpdateCragLog(selectedCragLogId, StatePicker.SelectedItem.ToString(),
                        CragName.Text, TypePicker.SelectedItem.ToString(), ClimbName.Text, completeGrade, 
                        SentSwitch.IsToggled, SendTypePicker.SelectedItem.ToString(), SendDatePicker.Date, LogNotes.Text);

                    await DisplayAlert("Climb Log Updated", "Journal Entry Successfully Udpated!", "OK");

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
                    await DatabaseServices.RemoveCragLog(selectedCragLogId);

                    await DisplayAlert("Climb Log Deleted", "Journal Entry Successfully Deleted.", "OK");

                    await Navigation.PopAsync();
                }
                catch (Exception)
                {
                    await DisplayAlert("Error", "Unable to Delete Journal Entry.", "OK");
                }
            }
        }

        async void ShareLog_Clicked(object sender, EventArgs e)
        {
            var climbLog = ClimbName.Text + "\r\n" +
                CragName.Text + "\r\n" +
                StatePicker.SelectedItem.ToString() + "\r\n" +
                TypePicker.SelectedItem.ToString() + "\r\n" +
                GradePicker.SelectedItem.ToString() + SubGradePicker.SelectedItem.ToString() + "\r\n" +
                SendTypePicker.SelectedItem.ToString() + "\r\n" +
                SendDatePicker.Date.ToString() + "\r\n" +
                LogNotes.Text;

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = climbLog,
                Title = "Share Journal Entry"
            });
        }

        #endregion
    }
}