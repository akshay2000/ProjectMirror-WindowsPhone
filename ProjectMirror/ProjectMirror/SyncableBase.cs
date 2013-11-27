using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMirror
{
    [Table]
    public class SyncableBase : NotifyBase, ISyncableBase
    {
        [JsonIgnore]
        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int LocalId { get; set; }

        [Column(CanBeNull = true)]
        [JsonProperty(PropertyName = "id")]
        public int RemoteId { get; set; }

        [Column]
        [JsonProperty(PropertyName = "isdeleted")]
        public bool IsDeleted { get; set; }

        [Column]
        [JsonProperty(PropertyName = "remotelastupdated")]
        public DateTime RemoteLastUpdated { get; set; }

        [Column(CanBeNull = true)]
        [JsonProperty(PropertyName = "locallastupdated")]
        public DateTime? LocalLastUpdated { get; set; }
    }
}
