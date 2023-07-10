using Sendz_Climbing_Journal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sendz_Climbing_Journal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CragAdd : ContentPage
    {
        private int selectedUserId;

        public CragAdd()
        {
            InitializeComponent();
        }

        public CragAdd(int userId)
        {
            InitializeComponent();

            selectedUserId = userId;
        }

        async void SaveClimbButton_Clicked(object sender, EventArgs e)
        {
            #region Textbox Validation

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

            var answer = await DisplayAlert("Add Climb", "Add this Journal Entry?", "Yes", "No");

            if (answer == true)
            {
                //Combines grade and subgrade to make a single entry
                string completeGrade = GradePicker.SelectedItem.ToString() + SubGradePicker.SelectedItem.ToString();

                try
                {
                    await DatabaseServices.AddCragLog(selectedUserId, StatePicker.SelectedItem.ToString(),
                        CragName.Text, TypePicker.SelectedItem.ToString(), ClimbName.Text, completeGrade,
                        SentSwitch.IsToggled, SendTypePicker.SelectedItem.ToString(), SendDatePicker.Date, LogNotes.Text);

                    await DisplayAlert("Climb Log Added", "Journal Entry Successfully Added!", "OK");

                    await Navigation.PopAsync();
                }
                catch (Exception)
                {
                    await DisplayAlert("Error", "Unable to Add Journal Entry.", "OK");

                    await Navigation.PopAsync();
                }
            }
            else
            {
                return;
            }
        }

        async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}