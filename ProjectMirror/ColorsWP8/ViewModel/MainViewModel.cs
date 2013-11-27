using ColorsWP8.Model;
using Microsoft.Live;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using ProjectMirror;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorsWP8.ViewModel
{
    public class MainViewModel : NotifyBase
    {
        private MirrorSyncService MirrorService = App.MirrorService;
        //Constructor
        public MainViewModel()
        {
            //MirrorService = new MirrorSyncService();
            MirrorService.ConfigureSQLCE<ToDoDataContext>(new ToDoDataContext("Data Source=isostore:/MainDB.sdf"));

            var AzureKeys = System.Windows.Application.GetResourceStream(new Uri("/ColorsWP8;component/AzureKeys.json", UriKind.Relative));
            StreamReader reader = new StreamReader(AzureKeys.Stream);
            string json = reader.ReadToEnd();
            dynamic keyJson = JObject.Parse(json);
            string endPoint = keyJson.endpoint;
            string accessKey = keyJson.accesskey;

            MirrorService.ConfigureMobileService(endPoint, accessKey);
        }

       
        private ObservableCollection<ToDoCategory> _categories = new ObservableCollection<ToDoCategory>();
        public ObservableCollection<ToDoCategory> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                if (_categories != value)
                {
                    NotifyPropertyChanging("Categories");
                    _categories = value;
                    NotifyPropertyChanged("Categories");
                }
            }
        }

        public async Task SyncCategoriesAsync()
        {
            await MirrorService.SynchronizeAsync<ToDoCategory>();
        }

        public void BufferMethod()
        {
            
        }

        #region CRUD operations

        public async void LoadCategories()
        {
            var categories = await MirrorService.LoadItemsAsync<ToDoCategory>();

            Categories.Clear();
            Categories.Add(new ToDoCategory { CategoryColor = "Cyan", CategoryName = "All" });
            foreach (ToDoCategory cat in categories)
            {
                Categories.Add(cat);
            }
        }

        public void AddCategory(ToDoCategory categoryToAdd)
        {
            MirrorService.AddItemAsync<ToDoCategory>(categoryToAdd);
            Categories.Add(categoryToAdd);
        }

        public void UpdateCategory(ToDoCategory categoryToUpdate)
        {
            MirrorService.UpdateItemAsync<ToDoCategory>(categoryToUpdate);
        }

        public void DeleteCategory(ToDoCategory categoryToDelete)
        {
            MirrorService.DeleteItemAsync<ToDoCategory>(categoryToDelete);
            Categories.Remove(categoryToDelete);
        }
        #endregion
    }
}
