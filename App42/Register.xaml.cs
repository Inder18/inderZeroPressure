using App42.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace App42
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {

        
      
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public Register()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void cancel_reg_button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(sign_in));
        }

        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }


        private async void signup_button_Click(object sender, RoutedEventArgs e)
        {

             if (signup_email.Text == "" || signup_mobile.Text == "" || signup_name.Text == "" || signup_pswd.Password == "")
            {
                MessageDialog r = new MessageDialog("Please fill out all the fields");

                await r.ShowAsync();
            }
            else if (!isValidEmail(signup_email.Text))
            {
                MessageDialog r = new MessageDialog("Email invalid !");
                await r.ShowAsync();
                signup_email.Focus(FocusState.Keyboard);
            }
            else if (signup_mobile.Text.Length != 10)
            {
                MessageDialog r = new MessageDialog("Mobile number should be of 10 digits !");
                await r.ShowAsync();
                signup_mobile.Focus(FocusState.Keyboard);
            }
            else
                {
                    System.Net.Http.HttpClient clt = new System.Net.Http.HttpClient();
                    Dictionary<string, string> pairs = new Dictionary<string, string>();
                    pairs.Add("name", signup_name.Text);
                    pairs.Add("email", signup_email.Text);
                    pairs.Add("mobile", signup_mobile.Text);
                    pairs.Add("password", signup_pswd.Password);
                    HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(pairs);
                    Windows.Web.Http.HttpClient client = new Windows.Web.Http.HttpClient();
                //Windows.Web.Http.HttpResponseMessage response = await client.PostAsync(new Uri("http://localhost/customer.php?" + DateTime.Now), formContent);
                Windows.Web.Http.HttpResponseMessage response = await client.PostAsync(new Uri("http://158.69.222.199/customer.php?" + DateTime.Now), formContent);                
                //var response = await clt.PostAsync("http://localhost/customer.php",new FormUrlEncodedContent(values));
               // MessageDialog r1 = new MessageDialog(response.Content.ToString());
                 //await r1.ShowAsync();
                if (response.Content.ToString().Equals("Row exists"))
                {

                    MessageDialog r = new MessageDialog("User already registered !");
                    await r.ShowAsync();
                }
                else if (response.Content.ToString().Equals("Connection failed"))
                {

                    MessageDialog r = new MessageDialog("Unable to connect to the internet");
                    await r.ShowAsync();
                }

                else
                {
                    signup_email.Text = "";
                    signup_mobile.Text = "";
                    signup_name.Text = "";
                    signup_pswd.Password = "";
                    MessageDialog r = new MessageDialog("You have been succesfully registered !");
                    await r.ShowAsync();
                    Frame.Navigate(typeof(sign_in));
                }
            }
            
        
        }
        
    }
}
