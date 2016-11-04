using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.ViewModels.Main
{
    public class Item
    {
        public string Item1 { get; set; }
        public string Item2 { get; set; }
        public string Item3 { get; set; }
    }

    public class MenuItemViewModel
    {
        public string Category { get; set; }
        public List<Item> Items { get; set; }
    }
}
