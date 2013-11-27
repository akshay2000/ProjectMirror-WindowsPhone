using Newtonsoft.Json;
using ProjectMirror;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ColorsWP8.Model
{    
    [Table]
    public class ToDoItem : NotifyBase, IMirrorSyncable
    {
        private int _localId;
        [JsonIgnore]
        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int LocalId
        {
            get
            {
                return _localId;
            }
            set
            {
                if (_localId != value)
                {
                    NotifyPropertyChanging("LocalId");
                    _localId = value;
                    NotifyPropertyChanged("LocalId");
                }
            }
        }

        private int _remoteId;
        [Column(CanBeNull = true)]
        [JsonProperty(PropertyName = "id")]
        public int RemoteId
        {
            get
            {
                return _remoteId;
            }
            set
            {
                if (_remoteId != value)
                {
                    NotifyPropertyChanging("RemoteId");
                    _remoteId = value;
                    NotifyPropertyChanged("RemoteId");
                }
            }
        }

        private bool _isDeleted;
        [Column]
        [JsonProperty]
        public bool IsDeleted
        {
            get
            {
                return _isDeleted;
            }
            set
            {
                if (_isDeleted != value)
                {
                    NotifyPropertyChanging("IsDeleted");
                    _isDeleted = value;
                    NotifyPropertyChanged("IsDeleted");
                }
            }
        }

        private DateTime _lastSynchronized;
        [Column]
        [JsonProperty]
        public DateTime LastSynchronized
        {
            get
            {
                return _lastSynchronized;
            }
            set
            {
                if (_lastSynchronized != value)
                {
                    NotifyPropertyChanging("LastSynchronized");
                    _lastSynchronized = value;
                    NotifyPropertyChanged("LastSynchronized");
                }
            }
        }

        private DateTime _lastModified;
        [Column]
        [JsonProperty]
        public DateTime LastModified
        {
            get
            {
                return _lastModified;
            }
            set
            {
                if (_lastModified != value)
                {
                    NotifyPropertyChanging("LastModified");
                    _lastModified = value;
                    NotifyPropertyChanged("LastModified");
                }
            }
        }

        private string _itemName;
        [Column]
        [JsonProperty]
        public string ItemName
        {
            get
            {
                return _itemName;
            }
            set
            {
                if (_itemName != value)
                {
                    NotifyPropertyChanging("ItemName");
                    _itemName = value;
                    NotifyPropertyChanged("ItemName");
                }
            }
        }

        private string _itemDate;
        [Column]
        [JsonProperty]
        public string ItemDate
        {
            get
            {
                return _itemDate;
            }
            set
            {
                if (_itemDate != value)
                {
                    NotifyPropertyChanging("ItemDate");
                    _itemDate = value;
                    NotifyPropertyChanged("ItemDate");
                }
            }
        }

        private string _itemMonth;
        [Column]
        [JsonProperty]
        public string ItemMonth
        {
            get
            {
                return _itemMonth;
            }
            set
            {
                if (_itemMonth != value)
                {
                    NotifyPropertyChanging("ItemMonth");
                    _itemMonth = value;
                    NotifyPropertyChanged("ItemMonth");
                }
            }
        }

        private string _itemYear;
        [Column]
        [JsonProperty]
        public string ItemYear
        {
            get
            {
                return _itemYear;
            }
            set
            {
                if (_itemYear != value)
                {
                    NotifyPropertyChanging("ItemMonth");
                    _itemYear = value;
                    NotifyPropertyChanged("ItemMonth");
                }
            }
        }

        private string _itemCategory;
        [Column]
        [JsonProperty]
        public string ItemCategory
        {
            get
            {
                return _itemCategory;
            }
            set
            {
                if (_itemCategory != value)
                {
                    NotifyPropertyChanging("ItemCategory");
                    _itemCategory = value;
                    NotifyPropertyChanged("ItemCategory");
                }
            }
        }

        private bool _itemIsComplete;
        [Column]
        [JsonProperty]
        public bool ItemIsComplete
        {
            get
            {
                return _itemIsComplete;
            }
            set
            {
                if (_itemIsComplete != value)
                {
                    NotifyPropertyChanging("ItemIsComplete");
                    _itemIsComplete = value;
                    NotifyPropertyChanged("ItemIsComplete");
                }
            }
        }

        private DateTime _deadline;
        [Column]
        [JsonProperty]
        public DateTime Deadline
        {
            get
            {
                return _deadline;
            }
            set
            {
                if (_deadline != value)
                {
                    NotifyPropertyChanging("Deadline");
                    _deadline = value;
                    NotifyPropertyChanged("Deadline");
                }
            }
        }

        private int _totalPomo;
        [Column]
        [JsonProperty]
        public int TotalPomo
        {
            get
            {
                return _totalPomo;
            }
            set
            {
                if (_totalPomo != value)
                {
                    NotifyPropertyChanging("TotalPomo");
                    _totalPomo = value;
                    NotifyPropertyChanged("TotalPomo");
                }
            }
        }

        private int _completePomo;
        [Column]
        [JsonProperty]
        public int CompletePomo
        {
            get
            {
                return _completePomo;
            }
            set
            {
                if (_completePomo != value)
                {
                    NotifyPropertyChanging("CompletePomo");
                    _completePomo = value;
                    NotifyPropertyChanged("CompletePomo");
                }
            }
        }

        private int _priority;
        [Column]
        [JsonProperty]
        public int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                if (_priority != value)
                {
                    NotifyPropertyChanging("Priority");
                    _priority = value;
                    NotifyPropertyChanged("Priority");
                }
            }
        }
    }
}
