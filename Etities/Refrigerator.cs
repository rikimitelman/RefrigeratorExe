using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RefrigeratorExercise.Etities
{
    public class Refrigerator
    {
        private static int nextRefrigeratorId = 0;
        private List<Refrigerator> refrigerators;

        public int RefrigeratorId { get; }
        public string RefrigeratorModel { get; set; }
        public string RefrigeratorColor { get; set; }
        public int NumOfShelves { get; set; }
        public List<Shelf> Shelves { get; set; }

        public Refrigerator()
        {
            RefrigeratorId = ++nextRefrigeratorId;
            refrigerators = new List<Refrigerator>();
        }
        public override string ToString()
        {
            return $"Refrigerator ID: {RefrigeratorId}\nModel: {RefrigeratorModel}\nColor: {RefrigeratorColor}\nNumber of Shelves: {NumOfShelves}";
        }
        public double CalculateRemainingSpaceInRefrigerator()
        {
            double totalSpace = 0;
            foreach (Shelf shelf in Shelves)
            {
                totalSpace += shelf.SpaceInTheShelf();
            }
            Console.WriteLine( $"total:{totalSpace}");
            return totalSpace;
        }
        public void AddItemToRefrigerator(Item item)
        {
            foreach (Shelf shelf in Shelves)
            {
                if (shelf.SpaceInTheShelf() >= item.ItemSpace)
                { 
                    shelf.Items.Add(item);
                    item.IdOfShelf = shelf.ShelfId;                 
                    Console.WriteLine("item added successfuly");
                    return;
                }
            }
            Console.WriteLine("there is no space in the refrigeratot");
        }
        public Item RemoveItemToRefrigerator(int id)
        {
            foreach (Shelf shelf in Shelves)
            {
                foreach(Item item in shelf.Items)
                {
                    if (item.ItemId == id)
                    {
                        shelf.Items.Remove(item);
                        item.IdOfShelf = 0;
                        return item;
                    }
                }
            }
            Console.WriteLine("item not found");
            return null;

        }
        public void cleanRefrigerator()
        {
            foreach (Shelf shelf in Shelves)
            {
                shelf.Items.RemoveAll(item => item.ExpiryDate <= DateTime.Now);
            }
                Console.WriteLine("Refrigerator cleaned successfully.");
        }
        public List<Item> WhatIwantToEat(Kosher kosher, TypeOfFood typeOfFood)
        {
            List<Item> availbleFoods = new List<Item>();
            foreach (Shelf shelf in Shelves)
            {
                foreach (Item item in shelf.Items)
                {
                    if (item.ExpiryDate >= DateTime.Now && item.TypeOfFood == typeOfFood && item.Kosher == kosher)
                    {
                       availbleFoods.Add(item);
                    } 
                }
            }
            return availbleFoods;
        }
        public List<Item> SortItemsByExpiryDate()
        {
            List<Item> sortedItems = new List<Item>();
            foreach(Shelf shelf in Shelves)
                sortedItems.AddRange(shelf.Items);
            sortedItems.Sort((x, y) => DateTime.Compare(x.ExpiryDate, y.ExpiryDate));
            return sortedItems;
        }
        public List<Shelf> SortShelvesByFreeSpace(List<Shelf> shelves)
        {
            List<Shelf> sortedShelves = new List<Shelf>();
            foreach (Shelf shelf in shelves)
            {
                sortedShelves.Add(shelf);
            }
            sortedShelves.Sort((x, y) => x.SpaceInTheShelf().CompareTo(y.SpaceInTheShelf()));
            return sortedShelves;
        }

        public List<Refrigerator> SorRefrigeratorsByFreeSpace(List<Refrigerator>refrigerators)
        {
            List<Refrigerator>freeSpaces = new List<Refrigerator>();
            foreach (Refrigerator refrigerator in refrigerators)
            {
                freeSpaces.Add(refrigerator);
            }
            freeSpaces.Sort((x,y)=>x.CalculateRemainingSpaceInRefrigerator().CompareTo(y.CalculateRemainingSpaceInRefrigerator()));
            return freeSpaces;
        }
        public void is20Free()
        {
            double freeSpace = CalculateRemainingSpaceInRefrigerator();

            if (freeSpace >= 29)
            {
                Console.WriteLine("There is enough space in the refrigerator.");
                return;
            }

            Console.WriteLine("There is not enough space in the refrigerator.");

            //Throw all expired products
            cleanRefrigerator();

            freeSpace = CalculateRemainingSpaceInRefrigerator();

            if (freeSpace >= 29)
            {
                Console.WriteLine("After removing expired items, there is now enough space in the refrigerator.");
                return;
            }

            Console.WriteLine("Removing items based on priority...");

            //Remove dairy products that expire in less than three days
            bool removedItems = RemoveItemsByPriority(Kosher.Dairy, DateTime.Now.AddDays(3));
            if (removedItems)
            {
                freeSpace = CalculateRemainingSpaceInRefrigerator();
                if (freeSpace >= 29)
                {
                    Console.WriteLine("After removing dairy products, there is now enough space in the refrigerator.");
                    return;
                }
            }

            //Remove meat products that expire in less than a week
            removedItems = RemoveItemsByPriority(Kosher.Meats, DateTime.Now.AddDays(7));
            if (removedItems)
            {
                freeSpace = CalculateRemainingSpaceInRefrigerator();
                if (freeSpace >= 29)
                {
                    Console.WriteLine("After removing meat products, there is now enough space in the refrigerator.");
                    return;
                }
            }

            // Remove fur products that are valid for less than a day
            removedItems = RemoveItemsByPriority(Kosher.Parrve, DateTime.Now.AddDays(1));
            if (removedItems)
            {
                freeSpace = CalculateRemainingSpaceInRefrigerator();
                if (freeSpace >= 29)
                {
                    Console.WriteLine("After removing fur products, there is now enough space in the refrigerator.");
                    return;
                }
            }

            // Leave products that have not expired and notify the user
            Console.WriteLine("There is still not enough space in the refrigerator. It is not time to shop.");
            Console.WriteLine("Please remove some items from the refrigerator or wait until there is enough space.");
        }
        private bool RemoveItemsByPriority(Kosher kosher, DateTime expiryDate)
        {
            bool removeItems = false;
            foreach (Shelf shelf in Shelves)
            {
                List<Item> itemToRemove = shelf.Items.FindAll(x=>x.Kosher==kosher&&x.ExpiryDate<=expiryDate);
                foreach (Item item in itemToRemove)
                {
                    shelf.Items.Remove(item);
                    item.IdOfShelf = 0;
                    Console.WriteLine($"Removed item: {item.ItemName}");
                    removeItems = true;
                }
            }
            return removeItems;
        }
    }
}
