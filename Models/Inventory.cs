using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class Inventory
    {
        public DateTime CurrentDate { get; set; }

        public List<InventoryItem> InventoryItemList { get; set; }

    }
}
