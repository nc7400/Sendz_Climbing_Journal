using Sendz_Climbing_Journal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sendz_Climbing_Journal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserAdd : ContentPage
    {
        public UserAdd()
        {
            InitializeComponent();
        }

        async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            #region Textbox Validation

            if (string.IsNullOrWhiteSpace(UserName.Text))
            {
                await DisplayAlert("Missing Name", "Please Enter User's Name.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(UserEmail.Text))
            {
                await DisplayAlert("Missing Email", "Please Enter User's Email.", "OK");
                return;
            }

            if (!EmailValidation(UserEmail.Text))
            {
                await DisplayAlert("Incorrect Email", "Please Enter a Correctly Formatted Email.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(UserPassword.Text))
            {
                await DisplayAlert("Missing Password", "Please Enter User's Password.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(MatchingPassword.Text))
            {
                await DisplayAlert("Missing Re-Entered Password", "Please Enter User's Password Again.", "OK");
                return;
            }

            if (UserPassword.Text != MatchingPassword.Text)
            {
                await DisplayAlert("Passwords Do Not Match", "Please Enter Matching Passwords.", "OK");
                return;
            }

            #endregion

            var answer = await DisplayAlert("New User", "Add New User With This Info: \r\n\r\n" +
               "Name: " + UserName.Text + "\r\n" + "Email: " + UserEmail.Text + "\r\n" +
               "Password: " + UserPassword.Text, "Yes", "No");

            if (answer == true)
            {
                try
                {
                    await DatabaseServices.AddUser(UserName.Text, UserEmail.Text, UserPassword.Text);

                    await DisplayAlert("User Added", "Successfully added User!", "OK");
                }
                catch (Exception)
                {
                    await DisplayAlert("Error", "Unable to Add User", "OK");
                }
            }
            else
            {
                return;
            }

            await Navigation.PopAsync();
        }

        //Validates that email is correctly formatted
        private bool EmailValidation(string emailAddress)
        {
            try
            {
                MailAddress mail = new MailAddress(emailAddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }   
}