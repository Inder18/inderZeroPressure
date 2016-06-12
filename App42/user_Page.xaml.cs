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
using System.Net.Http;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace App42
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class user_Page : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        string response;
        string user_name = "";
        public user_Page()
        {
            
            this.InitializeComponent();            
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            DrawerLayout.InitializeDrawerLayout();
            BackgroundTasks.Task1.Register();

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
           user_name= e.NavigationParameter.ToString();
            user_name_txtblock.Text = "Hello " + user_name.ToUpper();
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
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string resp = localSettings.Values["Position"].ToString();
            pos_image.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + resp + ".png"));
            pos_txt.Text = "Position : On the " + resp;

            Frame.BackStack.Clear();
            

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
            

        }

        #endregion
        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            else
                DrawerLayout.OpenDrawer();
        }
        private void about_us_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(about_us));
        }

       
        private void reach_us_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(reach_us));
        }

        private async void btn_Click(object sender, RoutedEventArgs e)
        {
            //if (await Task.Run(() => NetworkInterface.GetIsNetworkAvailable()))
            //  {
            HttpClient clt = new HttpClient();

            try
            {
                //response = await clt.GetStringAsync(new Uri("http://localhost/inder.php?" + DateTime.Now));

                response = await clt.GetStringAsync(new Uri("http://158.69.222.199/inder.php?" + DateTime.Now)); 
            }

#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {

                MessageDialog r = new MessageDialog("Please check your internet connection");

                await r.ShowAsync();
            }
            string x = response;
            string pos = x.Substring(0,x.IndexOf("-"));
            string id = x.Substring(x.IndexOf("-") + 1, x.Length - x.IndexOf("-") - 1);
            pos_image.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + pos + ".png"));
            pos_txt.Text = "Position : On the " + pos;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["Position"] = pos;
            localSettings.Values["id"] = id;
       
            //}
            //else
            //{
            //  MessageDialog r = new MessageDialog("Please check your internet connection");
            //await r.ShowAsync();
            //}
        }

        private void learn_more_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            
            Frame.Navigate(typeof(learn_more));
        }

        private void log_out_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            
            Frame.Navigate(typeof(sign_in));
        }
    }
}
