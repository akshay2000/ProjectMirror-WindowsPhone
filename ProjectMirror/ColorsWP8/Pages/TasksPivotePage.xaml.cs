using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ColorsWP8.ViewModel;

namespace ColorsWP8.Pages
{
    public partial class TasksPivotePage : PhoneApplicationPage
    {
        public TasksPivotePage()
        {
            InitializeComponent();
            DataContext = App.TasksViewModel;
        }

        private void AddToDoAppBarbutton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AddToDoPage.xaml", UriKind.Relative));
        }

        private async void SyncAppBarButton_Click(object sender, EventArgs e)
        {
            await App.TasksViewModel.SynchronizeToDosAsync();
            await App.TasksViewModel.LoadToDoItemsAsync("All");
        }

        private void DummyButton_Click(object sender, EventArgs e)
        {

        }
    }
}