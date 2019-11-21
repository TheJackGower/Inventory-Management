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
        /// <summary>
        /// Landing page. Displays inventory items. Also set current date for 
        /// button click (day change)
        /// </summary>
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

        /// <summary>
        /// List used to first load inventory.
        /// Would obviously come from database via Stored Procedure or Context etc.
        /// Just for quick demo purpose.
        /// </summary>
        public List<InventoryItem> GetInventoryList()
        {
            var list = new List<InventoryItem>
            {
                new InventoryItem(0, "Aged Brie", 5, GetRandomDate(3), false, null,null,true),
                new InventoryItem(1, "Christmas Crackers",10 , new DateTime(2019, 12, 25), false,null,null,null,true),
                new InventoryItem(2, "Frozen Item", 20, GetRandomDate(14), false, null,true),
                new InventoryItem(3, "Fresh Item", 23, GetRandomDate(19), false, true),
                new InventoryItem(4, "Soap", 30, null, true)
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

                #region General Rules

                if (item.IsFresh.HasValue)
                {
                    item.Quality = item.OverSellBy ? item.Quality - 4 : item.Quality - 2;
                }

                if (item.IsFrozen.HasValue)
                {
                    // “Frozen Item” decreases in Quality by 1 
                    item.Quality = !item.OverSellBy ? item.Quality - 1 : item.Quality - 2;
                }

                if (item.IsCheese.HasValue)
                {
                    // "Aged Brie" actually increases in Quality the older it gets 
                    item.Quality += 1;
                }

                if (item.IsChristmasItem.HasValue)
                {
                    // "Christmas Crackers", like “Aged Brie”, increases in Quality as its SellIn value approaches
                    item.Quality += 1;

                    if (item.SellInValue <= 10)
                    {
                        // Its quality increases by 2 when there are 10 days or less
                        item.Quality += 1;

                        if (item.SellInValue <= 5)
                        {
                            // by 3 when there are 5 days or less
                            item.Quality += 1;
                        }
                    }

                    if (m.CurrentDate > new DateTime(2019, 12, 25))
                    {
                        // quality drops to 0 after Christmas
                        item.Quality = 0;
                    }
                }

                #endregion

                #region Never less than 0 and more than 50

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

                #endregion
            }
        }

        /// <summary>
        /// Gets a random date for sellby
        /// </summary>
        static DateTime GetRandomDate(int DaysInFuture)
        {
            return DateTime.Today.AddDays(DaysInFuture);
        }
    }
}