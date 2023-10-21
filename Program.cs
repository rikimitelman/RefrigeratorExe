using RefrigeratorExercise.Etities;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    private static Refrigerator refrigerator;

    public IEnumerable<Shelf> Shelves { get; private set; }

    public static void Main(string[] args)
    {
        Program program = new Program();
        program.run();
    }
    public void run()
    {
        refrigerator = new Refrigerator();

        Shelf shelf1 = new Shelf( 1, 7);
        Shelf shelf2 = new Shelf( 2, 10);
        Shelf shelf3 = new Shelf( 3, 18);

        refrigerator.Shelves = new List<Shelf> { shelf1, shelf2, shelf3};

        Item item1 = new Item("milk", 2.0, "2023-11-16", TypeOfFood.Drinking, Kosher.Dairy);
        Item item2 = new Item("chocolate",5.0, "2023-10-02", TypeOfFood.Food, Kosher.Dairy);
        Item item3 = new Item("cheeze", 3, "2023-11-11",TypeOfFood.Food, Kosher.Dairy);
        Item item4 = new Item("yugort", 2, "2023-10-24",TypeOfFood.Food, Kosher.Dairy);
        Item item5 = new Item("soup", 4, "2023-11-21",TypeOfFood.Food, Kosher.Meats);
        refrigerator.AddItemToRefrigerator( item1);
        refrigerator.AddItemToRefrigerator( item2);
        refrigerator.AddItemToRefrigerator( item3);
        refrigerator.AddItemToRefrigerator( item4);
        refrigerator.AddItemToRefrigerator( item5);

        bool running = true;

        while (running)
        { 
            Console.WriteLine("Press 1: Print all items in the refrigerator");
            Console.WriteLine("Press 2: Print remaining space in the fridge");
            Console.WriteLine("Press 3: Add an item to the fridge");
            Console.WriteLine("Press 4: Remove an item from the refrigerator");
            Console.WriteLine("Press 5: Clean the refrigerator and print removed items");
            Console.WriteLine("Press 6: Request a product from the fridge");
            Console.WriteLine("Press 7: Print all products sorted by expiration date");
            Console.WriteLine("Press 8: Print shelves arranged by free space");
            Console.WriteLine("Press 9: Print refrigerators arranged by free space");
            Console.WriteLine("Press 10: Prepare the refrigerator for shopping");
            Console.WriteLine("Press 100: Shutdown the system");

            Console.WriteLine("Enter your choice:");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    PrintAllItems();
                    break;
                case "2":
                    PrintRemainingSpace();
                    break;
                case "3":
                    AddItemToRefrigerator();
                    break;
                case "4":
                    RemoveItemFromRefrigerator();
                    break;
                case "5":
                    cleanRefrigerator();
                    break;
                case "6":
                    RequestProduct();
                    break;
                case "7":
                    PrintProductsByExpirationDate();
                    break;
                case "8":
                    PrintShelvesByFreeSpace();
                    break;
                case "9":
                    PrintRefrigeratorsByFreeSpace();
                    break;
                case "10":
                    PrepareForShopping();
                    break;
                case "100":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void PrintAllItems()
    {
        foreach (Shelf shelf in refrigerator.Shelves)
        {
            Console.WriteLine($"Shelf ID: {shelf.ShelfId}");

            foreach (Item item in shelf.Items)
            {
                Console.WriteLine($"Item ID: {item.ItemId}, Name: {item.ItemName},Space:{item.ItemSpace},ExpiryDate:{item.ExpiryDate}");
            }
        }
    }
    private static void PrintRemainingSpace()
    {
        Console.WriteLine("remaining space in the refrigerator");
        Console.WriteLine(refrigerator.CalculateRemainingSpaceInRefrigerator()); 
    }
    private static void AddItemToRefrigerator()
    {
        Console.WriteLine("Enter the ItemName:");
        string itemName = Console.ReadLine();

        Console.WriteLine("Enter the ItemSpace:");
        if (!double.TryParse(Console.ReadLine(), out double itemSpace))
        {
            Console.WriteLine("Invalid input for ItemSpace. Please enter a valid number.");
            return;
        }

        Console.WriteLine("Enter the ItemExpiryDate (yyyy-mm-dd):");
        string itemExpiryDate = Console.ReadLine();
        if (!DateTime.TryParseExact(itemExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expiryDate))
        {
            Console.WriteLine("Invalid input for ItemExpiryDate. Please enter a valid date in the format yyyy-mm-dd.");
            return;
        }

        Console.WriteLine("Enter the kosher (Parrve, Dairy, Meats):");
        if (!Enum.TryParse<Kosher>(Console.ReadLine(), out Kosher itemKosher))
        {
            Console.WriteLine("Invalid input for kosher. Please enter Parrve, Dairy, or Meats.");
            return;
        }

        Console.WriteLine("Enter the ItemType(Food, Drinking):");
        if (!Enum.TryParse<TypeOfFood>(Console.ReadLine(), out TypeOfFood itemType))
        {
            Console.WriteLine("Invalid input for ItemType. Please enter Food or Drinking.");
            return;
        }

        Item item = new Item(itemName, itemSpace, itemExpiryDate, itemType, itemKosher);
        refrigerator.AddItemToRefrigerator(item);
    }


    private static void RemoveItemFromRefrigerator()
    {
        Console.WriteLine("Enter the item ID to remove from the refrigerator:");
        int itemId = Convert.ToInt32(Console.ReadLine());

        Item removedItem = refrigerator.RemoveItemToRefrigerator(itemId);

        if (removedItem != null)
        {
            Console.WriteLine($"Item removed: {removedItem.ItemName}");
        }
    }
    public void cleanRefrigerator()
    {
        try
        {
            foreach (Shelf shelf in Shelves)
            {
                shelf.Items.RemoveAll(item => item.ExpiryDate <= DateTime.Now);
            }
            Console.WriteLine("Refrigerator cleaned successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while cleaning the refrigerator. Error message: " + ex.Message);
        }
    }

    private static void RequestProduct()
    {
        Console.WriteLine("Enter the kosher type (Dairy, Meats, or Parve):");
        Kosher kosher = (Kosher)Enum.Parse(typeof(Kosher), Console.ReadLine());

        Console.WriteLine("Enter the type of food (Food,Drinking):");
        TypeOfFood typeOfFood = (TypeOfFood)Enum.Parse(typeof(TypeOfFood), Console.ReadLine());

        List<Item>requestItems=new List<Item>();
        requestItems= refrigerator.WhatIwantToEat(kosher, typeOfFood);
        foreach (var item in requestItems)
        {
            Console.WriteLine( $"item name:{item.ItemName}");
        }
    }
    private static void PrintProductsByExpirationDate()
    {
        List<Item> sortedItems = new List<Item>(); 
        sortedItems=refrigerator.SortItemsByExpiryDate();
        foreach (var item in sortedItems)
        {
            Console.WriteLine(item.ToString());
        }
    }
    private static void PrintShelvesByFreeSpace()
    {
        List<Shelf> sortedShelves = new List<Shelf>();
        sortedShelves= refrigerator.SortShelvesByFreeSpace(refrigerator.Shelves);
        foreach (var shelf in sortedShelves)
        {
            Console.WriteLine(shelf.ToString());
        }
    }
    private static void PrintRefrigeratorsByFreeSpace()
    {
        //here the user have to add refrigerator in order to sort
        List<Refrigerator>refrigerators=new List<Refrigerator>();
        refrigerators=refrigerator.SorRefrigeratorsByFreeSpace(refrigerators);
        foreach (var item in refrigerators)
        { Console.WriteLine(item.ToString()); }
    }
    public void PrepareForShopping()
    {
        try
        {
            refrigerator.is20Free();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while preparing the refrigerator for shopping. Error message: " + ex.Message);
        }
    }

}
