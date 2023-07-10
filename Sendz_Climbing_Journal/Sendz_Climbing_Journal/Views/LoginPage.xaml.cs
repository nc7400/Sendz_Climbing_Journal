using Sendz_Climbing_Journal.Models;
using Sendz_Climbing_Journal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sendz_Climbing_Journal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        //TODO Forgot password
        //TODO Add new user

        public LoginPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //Initializes and creates the tables, then gets stored users
            var users = await DatabaseServices.GetUsers();

            //TODO check if works
            LoginEmail.Text = null;
            LoginPassword.Text = null;
        }

        async void LoginSettings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppSettings());
        }

        async void LoginButton_Clicked(object sender, EventArgs e)
        {
            #region Textbox Validation

            if (string.IsNullOrWhiteSpace(LoginEmail.Text))
            {
                await DisplayAlert("Missing Email", "Please Enter User's Email.", "OK");
                return;
            }

            if (!EmailValidation(LoginEmail.Text))
            {
                await DisplayAlert("Incorrect Email", "Please Enter a Correctly Formatted Email.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(LoginPassword.Text))
            {
                await DisplayAlert("Missing Password", "Please Enter User's Password.", "OK");
                return;
            }

            #endregion

            //Adds all users to a list then checks list to validate user exists in the database
            var userList = await DatabaseServices.GetAllUsers();
            int userExists = 0;

            foreach (User user in userList)
            {
                if (user.Email == LoginEmail.Text && user.Password == LoginPassword.Text)
                {
                    userExists++;
                    await Navigation.PushAsync(new Dashboard(user.Id));
                }
            }

            if (userExists == 0)
            {
                await DisplayAlert("Incorrect Email/Password", "No user found with Email/Password combination.", "OK");
                return;
            }
        }

        async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserAdd());
        }

        //Validates that email is correctly
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