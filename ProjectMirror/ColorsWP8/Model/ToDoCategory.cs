using ProjectMirror;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
//using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ColorsWP8.Model
{
    [Table]
    public class ToDoCategory : NotifyBase, IMirrorSyncable
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

        private string _remoteId;
        [Column(CanBeNull = true)]
        [JsonProperty(PropertyName = "id")]
        public string RemoteId
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

        private string _categoryName;
        [Column]
        [JsonProperty]
        public string CategoryName
        {
            get
            {
                return _categoryName;
            }
            set
            {
                if (_categoryName != value)
                {
                    NotifyPropertyChanging("CategoryName");
                    _categoryName = value;
                    NotifyPropertyChanged("CategoryName");
                }
            }
        }

        private string _categoryColor;
        [Column]
        [JsonProperty]
        public string CategoryColor
        {
            get
            {
                return _categoryColor;
            }
            set
            {
                if (_categoryColor != value)
                {
                    NotifyPropertyChanging("CategoryColor");
                    _categoryColor = value;
                    NotifyPropertyChanged("CategoryColor");
                }
            }
        }
    }
}
