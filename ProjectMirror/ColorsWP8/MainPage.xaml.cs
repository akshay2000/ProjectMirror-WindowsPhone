using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ColorsWP8.Resources;
using ColorsWP8.Model;
using System.Threading.Tasks;
using Microsoft.Live;
using Microsoft.WindowsAzure.MobileServices;

namespace ColorsWP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        private AuthenticationManager authMan;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            App.ViewModel.LoadCategories();
            this.Loaded += MainPage_Loaded;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (authMan == null)
            {
                ManageSystemTray("Signing in...", true);
                authMan = new AuthenticationManager();
                authMan.AuthenticationCompleted += authMan_AuthenticationCompleted;
            }
        }


        private async void authMan_AuthenticationCompleted(object sender, EventArgs e)
        {
            ManageSystemTray("Synchronizing...", true);
            await App.ViewModel.SyncCategoriesAsync();
            App.ViewModel.LoadCategories();
            ManageSystemTray("", false);
        }

        private void ManageSystemTray(string text, bool isVisible)
        {
            ProgressIndicator progressIndicator = new ProgressIndicator
            {
                IsVisible = isVisible,
                IsIndeterminate = true,
                Text = text
            };
            SystemTray.SetProgressIndicator(this, progressIndicator);
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        private async Task AuthenticateAsync()
        {
            LiveAuthClient authClient = new LiveAuthClient("000000004010B920");
            LiveLoginResult result = await authClient.InitializeAsync(new[] { "wl.signin", "wl.offline_access" });
            LiveConnectSession session = result.Session;
            MobileServiceUser user = null;

            if (session == null)
            {
                NavigationService.Navigate(new Uri("/SplashPage.xaml", UriKind.Relative));
                return;
            }
            if (result.Status == LiveConnectSessionStatus.Connected)
            {
                string authToken = session.AuthenticationToken;
                user = await App.MirrorService.AuthenticateWithMicrosoftAsync(authToken);
            }
            else
            {
                MessageBox.Show("Something went wrong with the authentication", "Sorry", MessageBoxButton.OK);
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void AddCatgegoryAppBarbutton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AddCategoryPage.xaml", UriKind.Relative));
        }       

        private void CategoryWrapperGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ToDoCategory selectedCategory = (sender as Grid).DataContext as ToDoCategory;
            int requestedId = selectedCategory.LocalId;
            string UriString = "/Pages/EditCategoryPage.xaml?requestedId=" + requestedId;
            NavigationService.Navigate(new Uri(UriString, UriKind.Relative));
        }

        private async void SyncAppBarButton_Click(object sender, EventArgs e)
        {
            await App.ViewModel.SyncCategoriesAsync();
            App.ViewModel.LoadCategories();
        }     
    }
}