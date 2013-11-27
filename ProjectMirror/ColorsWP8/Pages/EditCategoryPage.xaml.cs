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
    public partial class EditCategoryPage : PhoneApplicationPage
    {
        ToDoCategory selectedCategory = new ToDoCategory();

        public EditCategoryPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string passedId = string.Empty;

            if (NavigationContext.QueryString.TryGetValue("requestedId", out passedId))
            {
                int reqestedIndex = int.Parse(passedId);
                selectedCategory = App.ViewModel.Categories.FirstOrDefault(cat => cat.LocalId == reqestedIndex);
            }
            base.OnNavigatedTo(e);
        }

        private void SaveCategoryAppBarButton_Click(object sender, EventArgs e)
        {
            selectedCategory.CategoryName = CategoryNameTextBox.Text;
            selectedCategory.CategoryColor = CategoryColorTextBox.Text;
            App.ViewModel.UpdateCategory(selectedCategory);
            NavigationService.GoBack();
        }

        private void CancelAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void CategoryNameTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            CategoryNameTextBox.Text = selectedCategory.CategoryName;
            CategoryColorTextBox.Text = selectedCategory.CategoryColor;
        }

        private void DeleteCategoryAppBarButton_Click(object sender, EventArgs e)
        {
            App.ViewModel.DeleteCategory(selectedCategory);
        }
    }
}