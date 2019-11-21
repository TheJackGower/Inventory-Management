using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            Inventory i = new Inventory
            {
                CurrentDate = DateTime.Now,
                InventoryItemList = GetInventoryList()
            };

            RefreshInventory(ref i);

            return View(i);
        }

        public List<InventoryItem> GetInventoryList()
        {
            var list = new List<InventoryItem>
            {
                new InventoryItem(0, "Aged Brie", 5, GetRandomDate(3), false),
                new InventoryItem(1, "Christmas Crackers",10 , GetRandomDate(-15), false),
                new InventoryItem(2, "Frozen Item", 20, GetRandomDate(17), false),
                new InventoryItem(3, "Fresh Item", 23, GetRandomDate(20), false),
                new InventoryItem(4, "Soap", 15, null, true)
            };

            return list;
        }

        /// <summary>
        /// Each update call is the equivilant to a day passing
        /// </summary>
        public IActionResult Update(Inventory m)
        {
            // ensure items aren't cached
            ModelState.Clear();

            // add new day for display
            m.CurrentDate = m.CurrentDate.AddDays(1);

            // loop through each item for check
            RefreshInventory(ref m);

            // auto item update for next day
            UpdateInventory(ref m);

            // return view with updated data
            return View("Index", m);
        }

        /// <summary>
        /// Used to update the quality, sellin value etc
        /// </summary>
        void RefreshInventory(ref Inventory m)
        {
            foreach (var item in m.InventoryItemList)
            {
                if (item.IgnoreRules)
                {
                    // don't apply anything
                    continue;
                }

                // are we over sell by date?
                if (DateTime.Now > item.SellByDate)
                {
                    // set bool to reflect
                    item.OverSellBy = true;
                }

                // Adjust total days for sell in
                item.SellInValue = int.Parse(Math.Ceiling((item.SellByDate.Value - m.CurrentDate).TotalDays).ToString());
            }
        }

        /// <summary>
        /// Update values for new day
        /// </summary>
        void UpdateInventory(ref Inventory m)
        {
            foreach (var item in m.InventoryItemList)
            {
                if (item.IgnoreRules)
                {
                    // "Soap" never has to be sold or decreases in Quality
                    continue;
                }

                switch (item.Name)
                {
                    case "Frozen Item":
                        // “Frozen Item” decreases in Quality by 1 
                        item.Quality = !item.OverSellBy ? item.Quality - 1 : item.Quality - 2;
                        break;

                    case "Aged Brie":
                        // "Aged Brie" actually increases in Quality the older it gets 
                        item.Quality++;
                        break;

                    case "Fresh Item":
                        item.Quality = item.OverSellBy ? item.Quality - 4 : item.Quality - 2;
                        break;
                }

                if (item.Quality < 0)
                {
                    // The Quality of an item is never negative
                    item.Quality = 0;
                }

                if (item.Quality > 50)
                {
                    // The Quality of an item is never more than 50
                    item.Quality = 50;
                }
            }
        }

        /// <summary>
        /// Gets a random date
        /// </summary>
        static DateTime GetRandomDate(int DaysInFuture)
        {
            return DateTime.Today.AddDays(DaysInFuture);
        }
    }
}