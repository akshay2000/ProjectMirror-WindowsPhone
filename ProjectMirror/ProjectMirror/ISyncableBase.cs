using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjectMirror
{
    /// <summary>
    /// The interface provides skeleton for properties absolutely required by
    /// the class to be synced.
    /// </summary>
    public interface IMirrorSyncable
    {
        int LocalId { get; set; }

        int RemoteId { get; set; }

        bool IsDeleted { get; set; }

        DateTime LastSynchronized { get; set; }

        DateTime LastModified { get; set; }
    }
}
