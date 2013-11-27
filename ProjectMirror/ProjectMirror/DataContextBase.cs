using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMirror
{
    public class DataContextBase : DataContext
    {
        public DataContextBase(string connectionString) : base(connectionString)
        { }

        public Table<SyncTimeStamp> SyncTimeStamps;
    }
}
