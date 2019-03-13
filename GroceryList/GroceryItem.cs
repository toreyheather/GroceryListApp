using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GroceryList
{
    public class RootObject
    {
        public GroceryItem[] GroceryItem { get; set; }
    }

    public class GroceryItem
    {
        public string Item { get; set; }
        public string Section { get; set; }

        
    }
}
