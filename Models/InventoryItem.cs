using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class InventoryItem
    {
        public InventoryItem()
        { }

        public InventoryItem(int _id, string _Name, int _Quality, DateTime? _SellByDate, bool _IgnoreRules)
        {
            ID = _id;
            Name = _Name;
            Quality = _Quality;
            SellByDate = _SellByDate;
            IgnoreRules = _IgnoreRules;
        }

        public int ID { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Denotes the number of days we have to sell the item
        /// </summary>
        [DisplayName("Days To Sell")]
        public int SellInValue { get; set; }

        public bool OverSellBy { get; set; } = false;

        /// <summary>
        /// The Quality of an item is never more than 50
        /// </summary>

        [Range(int.MinValue, 50)]
        public int Quality { get; set; }

        [DisplayName("Sell By")]
        public DateTime? SellByDate { get; set; }

        /// <summary>
        /// For items where quality and sell-in value doesn't decrease
        /// i.e. Soap never has to be sold or decreases in Quality
        /// </summary>
        public bool IgnoreRules { get; set; }

    }
}
