using ColorsWP8.Model;
using ProjectMirror;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorsWP8.ViewModel
{
    public class TasksViewModel : NotifyBase
    {
        //Constructor
        public TasksViewModel()
        {
            //BufferMethod();
        }

        private ObservableCollection<ToDoItem> _allToDoItems = new ObservableCollection<ToDoItem>();
        public ObservableCollection<ToDoItem> AllToDoItems
        {
            get
            {
                return _allToDoItems;
            }
            set
            {
                if (_allToDoItems != value)
                {
                    NotifyPropertyChanging("AllToDoItems");
                    _allToDoItems = value;
                    NotifyPropertyChanged("AllToDoItems");
                }
            }
        }

        private ObservableCollection<ToDoItem> _todayToDoItems = new ObservableCollection<ToDoItem>();
        public ObservableCollection<ToDoItem> TodayToDoItems
        {
            get
            {
                return _todayToDoItems;
            }
            set
            {
                if (_todayToDoItems != value)
                {
                    NotifyPropertyChanging("TodayToDoItems");
                    _todayToDoItems = value;
                    NotifyPropertyChanged("TodayToDoItems");
                }
            }
        }

        private ObservableCollection<ToDoItem> _tomToDoItems = new ObservableCollection<ToDoItem>();
        public ObservableCollection<ToDoItem> TomToDoItems
        {
            get
            {
                return _tomToDoItems;
            }
            set
            {
                if (_tomToDoItems != value)
                {
                    NotifyPropertyChanging("TomToDoItems");
                    _tomToDoItems = value;
                    NotifyPropertyChanged("TomToDoItems");
                }
            }
        }

        private ObservableCollection<ToDoItem> _weekToDoItems = new ObservableCollection<ToDoItem>();
        public ObservableCollection<ToDoItem> WeekToDoItems
        {
            get
            {
                return _weekToDoItems;
            }
            set
            {
                if (_weekToDoItems != value)
                {
                    NotifyPropertyChanging("WeekToDoItems");
                    _weekToDoItems = value;
                    NotifyPropertyChanged("WeekToDoItems");
                }
            }
        }

        private ObservableCollection<ToDoItem> _overToDoItems = new ObservableCollection<ToDoItem>();
        public ObservableCollection<ToDoItem> OverToDoItems
        {
            get
            {
                return _overToDoItems;
            }
            set
            {
                if (_overToDoItems != value)
                {
                    NotifyPropertyChanging("OverToDoItems");
                    _overToDoItems = value;
                    NotifyPropertyChanged("OverToDoItems");
                }
            }
        }

        private List<ToDoItem> MasterList;

        public async Task SynchronizeToDosAsync()
        {
            await App.MirrorService.SynchronizeAsync<ToDoItem>();
        }

        private async Task LoadMasterListAsync()
        {
            if (MasterList == null)
                MasterList = new List<ToDoItem>();

            MasterList = await App.MirrorService.LoadItemsAsync<ToDoItem>();
        }

        public async Task LoadToDoItemsAsync(string categoryName)
        {
            await LoadMasterListAsync();
            var filteredAll = categoryName == "All" ? MasterList : MasterList.Where(item => item.ItemCategory == categoryName);
            AllToDoItems.Clear();
            foreach (var p in filteredAll)
            {
                AllToDoItems.Add(p);
            }
        }

        #region CRUD operations
        public void AddToDoItem(ToDoItem itemToAdd)
        {
            App.MirrorService.AddItemAsync<ToDoItem>(itemToAdd);
        }


        #endregion
        public void BufferMethod()
        {

            TodayToDoItems.Add(new ToDoItem { ItemName = "Buy some groundnuts", ItemIsComplete = false });
            TodayToDoItems.Add(new ToDoItem { ItemName = "Clean the table", ItemIsComplete = false });
            TodayToDoItems.Add(new ToDoItem { ItemName = "Send greeting cards", ItemIsComplete = false });
            TodayToDoItems.Add(new ToDoItem { ItemName = "Complete maths homework", ItemIsComplete = false });
        }
    }
}
