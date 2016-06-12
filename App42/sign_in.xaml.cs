using App42.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using System.Text.RegularExpressions;
using Windows.Web.Http;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace App42
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class sign_in : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        string user_name="";
        public sign_in()
        {
            this.InitializeComponent();
            
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Application.Current.Exit();
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
            
            HardwareButtons.BackPressed += this.HardwareButtons_BackPressed;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
            HardwareButtons.BackPressed -= this.HardwareButtons_BackPressed;

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

        #endregion

        private async void signin_button_Click(object sender, RoutedEventArgs e)
        {
            if (signin_email.Text == "" || signin_pswd.Password == "")
            {
                MessageDialog r = new MessageDialog("Please enter email and password.");
                await r.ShowAsync();
                signin_email.Focus(FocusState.Keyboard);
            }
            else if (!isValidEmail(signin_email.Text.ToString()))
            {
                MessageDialog r = new MessageDialog("Invalid email !");
                await r.ShowAsync();
                signin_email.Focus(FocusState.Keyboard);
            }
            else
            {

                //System.Net.Http.HttpClient clt = new System.Net.Http.HttpClient();
                Dictionary<string, string> pairs = new Dictionary<string, string>();
                pairs.Add("email", signin_email.Text);
                pairs.Add("password", signin_pswd.Password);
                HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(pairs);
                Windows.Web.Http.HttpClient client = new Windows.Web.Http.HttpClient();
                //Windows.Web.Http.HttpResponseMessage response = await client.PostAsync(new Uri("http://localhost/login.php?" + DateTime.Now), formContent);
                Windows.Web.Http.HttpResponseMessage response = await client.PostAsync(new Uri("http://158.69.222.199/login.php?" + DateTime.Now), formContent);
               // MessageDialog r1 = new MessageDialog(response.Content.ToString());
                //await r1.ShowAsync();
                  if (response.Content.ToString().Equals("Connection failed"))
                {

                    MessageDialog r = new MessageDialog("Unable to connect to the internet.");
                    await r.ShowAsync();
                }
                else if (!response.Content.ToString().Equals("check") )
                {
                    user_name = response.Content.ToString();
                    Frame.Navigate(typeof(user_Page),user_name);
                    HttpClient clt = new HttpClient();
                    string msg = await clt.GetStringAsync(new Uri("http://158.69.222.199/reminder.php?" + DateTime.Now));
                    if(msg.Equals("skin"))
                    {
                        ShowToast();
                    }

                }

                else if(response.Content.ToString().Equals("check"))
                {
                    MessageDialog r = new MessageDialog("Email or password incorrect !");
                    await r.ShowAsync();
                }

            }

            
        }
        public static ToastNotification ShowToast()
        {

            var template = ToastTemplateType.ToastText02;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            var toastElements = xml.GetElementsByTagName("text");
            toastElements[0].AppendChild(xml.CreateTextNode("Its Skin Inspection Day !"));
            toastElements[1].AppendChild(xml.CreateTextNode("Check the skin for any problems"));
            var toast = new ToastNotification(xml);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);            
            return toast;

        }
        private void sign_up_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Register));
        }
    }
}
