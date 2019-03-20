using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace GroceryList
{
    class Program
    {
        static List<GroceryItem> groceryItems = new List<GroceryItem>();
        static List<GroceryItem> neededItems = new List<GroceryItem>();
        static List<GroceryItem> organizedNeededItems = new List<GroceryItem>();

        public static void Main(string[] args)
        {
            

//reading the json file

            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            var fileName = Path.Combine(directory.FullName, "GroceryItems.json");
            var fileContents = ReadFile(fileName);
            fileName = Path.Combine(directory.FullName, "GroceryItems.json");
            groceryItems = DeserializeGroceryItems(fileName);

            //application main menu
            CommandLine.DisplayWelcome();

            int option = 0;
            while ((option = Menu.Prompt()) != 5)
            {
                switch (option)
                {
                    case 1:
                        SeeAvailableGroceryItems();
                        break;
                    case 2:
                        AddGroceryItem();
                        SerializeListToFile(groceryItems, "GroceryItems.json");
                        break;
                    case 3:
                        RemoveGroceryItem();
                        SerializeListToFile(groceryItems, "GroceryItems.json");
                        break;
                    case 4:
                        CreateShoppingList();
                        SectionSorter();
                        SerializeNeededItemsToFile(organizedNeededItems, "ShoppingList.txt");
                        break;
                }
            }
        }

//reads json file
        public static string ReadFile(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }
        }

//deserialzing json data
        public static List<GroceryItem> DeserializeGroceryItems(string fileName)
        {
            var groceryItemsList = new List<GroceryItem>();
            var serializer = new JsonSerializer();
            using (var reader = new StreamReader(fileName))
            using (var jsonReader = new JsonTextReader(reader))
            {
                groceryItemsList = serializer.Deserialize<List<GroceryItem>>(jsonReader);
            }
            return groceryItemsList;
        }

//see available grocery items
        public static void SeeAvailableGroceryItems()
        {
            Console.WriteLine("Available Grocery Items");
            Console.WriteLine("-----------------------");
            foreach (var groceryItem in groceryItems)
            {
                Console.WriteLine(groceryItem.Item);
            }
        }

//add grocery item to available grocery items
        public static void AddGroceryItem()
        {
            Console.WriteLine("Add a Grocery Item");
            Console.WriteLine("------------------");
            bool done = false;
            do
            {
                string item = CommandLine.Prompt("What item do you want to add? ");
                if (item == "")
                {
                    Console.WriteLine("Please enter an item");
                }
                else
                {
                    string section = CommandLine.Prompt("What section is it in? ");
                    if (section == "produce" || section == "bakery" || section == "deli" || section == "cans/jars" || section == "boxes/bags" || section == "drinks" || section == "frozen" || section == "dairy")
                    {
                        groceryItems.Add(new GroceryItem { Item = item, Section = section });
                        done = CommandLine.Prompt("Add another item? (y/n) ").ToLower() != "y";
                    }
                    else
                    {
                        Console.WriteLine("Available sections are produce, baker, deli, cans/jars, boxes/bags, drinks, frozen, and dairy.");
                        done = CommandLine.Prompt("Add another item? (y/n) ").ToLower() != "y";
                    }
                }
            }
            while (!done);
        }

//remove grocery item from available grocery items
        public static void RemoveGroceryItem()
        {
            Console.WriteLine("Remove a Grocery Item");
            Console.WriteLine("---------------------");
            bool done = false;
            do
            { 
                string item = CommandLine.Prompt("What item do you want to remove? ");
                var removedItem = groceryItems.Find(i => i.Item == item);
                if (removedItem == null)
                {
                    Console.WriteLine("That is not an available item.");
                    done = CommandLine.Prompt("Do you want to try to remove another item? (y/n) ").ToLower() != "y";
                }
                else
                {
                    groceryItems.Remove(removedItem);
                    done = CommandLine.Prompt("Remove another item? (y/n) ").ToLower() != "y";
                }
            }
            while (!done);
        }

//serialize available grocery items
        public static void SerializeListToFile(List<GroceryItem> groceryItemsList, string fileName)
        {
            var serializer = new JsonSerializer();
            using (var writer = new StreamWriter(fileName))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, groceryItemsList);
            }
        }

//create a shopping list
        public static void CreateShoppingList()
        {
            neededItems = new List<GroceryItem> ();
            Console.WriteLine("Make a Shopping List");
            Console.WriteLine("--------------------");
            bool done = false;
            do
            {
                string itemNeeded = CommandLine.Prompt("What item do you need to get? ");
                var itemToAdd = groceryItems.Find(i => i.Item == itemNeeded);
                if(itemToAdd == null)
                {
                    Console.WriteLine("That item is not available.");
                    done = CommandLine.Prompt("Add another item? (y/n) ").ToLower() != "y";
                }
                else
                {
                    neededItems.Add(itemToAdd);
                    foreach (var groceryItem in neededItems)
                    {
                        Console.WriteLine(groceryItem.Item);
                    }
                    done = CommandLine.Prompt("Add another item? (y/n) ").ToLower() != "y";
                };
            }
            while (!done);
        }

//sorts needed items by grocery store sections
//produce = 1
//bakery = 2
//deli = 3
//cans/jars = 4
//boxes/bags = 5
//drinks = 6
//frozen = 7
//dairy = 8
        public static void SectionSorter()
        {
            organizedNeededItems = neededItems.OrderBy(n => n.Section == "dairy")
                                                  .ThenBy(neededItems => neededItems.Section == "frozen")
                                                  .ThenBy(neededItems => neededItems.Section == "drinks")
                                                  .ThenBy(neededItems => neededItems.Section == "boxes/bags")
                                                  .ThenBy(neededItems => neededItems.Section == "cans/jars")
                                                  .ThenBy(neededItems => neededItems.Section == "deli")
                                                  .ThenBy(neededItems => neededItems.Section == "bakery")
                                                  .ThenBy(neededItems => neededItems.Section == "produce")
                                                  .ToList();

            foreach (var neededItems in organizedNeededItems)
            {
                Console.WriteLine(neededItems.Item);
            }
        }

//serialize needed grocery items list
        public static void SerializeNeededItemsToFile(List<GroceryItem> organizedNeededItems, string fileName)
        {
            var serializer = new JsonSerializer();
            using (var writer = new StreamWriter(fileName))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, organizedNeededItems);
            }
        }
    }
}
