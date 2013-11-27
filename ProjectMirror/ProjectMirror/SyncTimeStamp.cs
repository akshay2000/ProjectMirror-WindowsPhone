using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMirror
{
    [Table]
    public class SyncTimeStamp
    {
        [Column(IsPrimaryKey = true)]
        public string EntityName { get; set; }
        [Column]
        public DateTime LastSynced { get; set; }
    }
}
