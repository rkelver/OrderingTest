﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public static class Inventory
    {
        //NOTE: ONLY FOR TESTING ACCESS TO CURRENT INV UNIQUE IDS
        public static List<Guid> ItemIds { get; }
        private const int ItemCount = 10;

        private static IEnumerable<Item> Items => new List<Item>();
        static Inventory()
        {
            //NOTE: ONLY FOR TESTING TO BUILD INVENTORY. I PREFER TO KEEP THE READONLY PROPERTY VS ALLOW THE CLIENT TO BUILD HIS OWN INVENTORY
            ItemIds = new List<Guid>();
            for (var i = 0; i < ItemCount; i++)
            {
                ItemIds.Add(Guid.NewGuid());
            }
        }

        public static bool IsItemInInventory(Guid itemId)
        {
            return Items.Any();
        }

        public static IEnumerable<Item> ItemsNotInInventory(IEnumerable<Guid> itemIds)
        {
            return Items.Where(i => !itemIds.Contains(i.Id));
        }

        public static bool ItemFitsWithinDimensions(Container container, Item item)
        {
            return ThisSpaceIsAvaialable(0,0);
        }

        private static bool ThisSpaceIsAvaialable(decimal holdingSPace, decimal requestingSpace)
        {
            return true;
        }
    }
}
