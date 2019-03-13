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
                        SerializeNeededItemsToFile(neededItems, "ShoppingList.json");
                        break;
                }
            }
        }

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
                string section = CommandLine.Prompt("What section is it in? ");
                if(section == "produce" || section == "bakery" || section == "deli" || section == "cans/jars" || section == "boxes/bags" || section == "drinks" || section == "frozen" || section == "dairy")
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
                groceryItems.Remove(removedItem);
                done = CommandLine.Prompt("Add another item? (y/n) ").ToLower() != "y";
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
                
                done = CommandLine.Prompt("Add another item? (y/n) ").ToLower() != "y";
                if(itemToAdd == null)
                {
                    Console.WriteLine("That item is not available.");
                   
                }
                else
                {
                    neededItems.Add(itemToAdd);
                    foreach (var groceryItem in neededItems)
                    {
                        Console.WriteLine(groceryItem.Item);
                    }

                };
            }
            while (!done);
        }

//serialize needed grocery items list
        public static void SerializeNeededItemsToFile(List<GroceryItem> neededItems, string fileName)
        {
            var serializer = new JsonSerializer();
            using (var writer = new StreamWriter(fileName))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, neededItems);
            }
        }
    }
}
