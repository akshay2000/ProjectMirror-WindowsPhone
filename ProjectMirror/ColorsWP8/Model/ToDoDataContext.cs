using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMirror;
using System.Data.Linq;

namespace ColorsWP8.Model
{
    class ToDoDataContext : DataContextBase
    {
        public ToDoDataContext(string connectionString) :base(connectionString)
        { }

        public Table<ToDoCategory> ToDoCategories;

        public Table<ToDoItem> ToDoItems;
    }
}
