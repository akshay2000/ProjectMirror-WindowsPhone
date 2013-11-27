using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ColorsWP8.Model;

namespace ColorsWP8.Pages
{
    public partial class AddToDoPage : PhoneApplicationPage
    {
        public AddToDoPage()
        {
            InitializeComponent();
        }

        private void SaveToDoAppBarButton_Click(object sender, EventArgs e)
        {
            var taskToAdd = new ToDoItem { ItemName = TaskNameTextBox.Text, Deadline = (DateTime)DeadlineDatePicker.Value, ItemIsComplete = false, Priority = PrioityPicker.SelectedIndex };
            App.TasksViewModel.AddToDoItem(taskToAdd);
            NavigationService.GoBack();
        }

        private void CancelAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}