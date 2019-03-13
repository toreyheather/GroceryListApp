using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryList
{
    class CommandLine
    {
        internal static void DisplayWelcome()
        {
            Console.WriteLine("------------------Grocery Shopping-----------------");
            Console.WriteLine("A Shopping List Organized by Grocery Store Sections");
            Console.WriteLine("---------------------------------------------------");
        }

        internal static string Prompt(string message)
        {
            Console.Write(message);
            string userInput = Console.ReadLine();
            Console.WriteLine();

            return userInput.Trim();
        }
    }
}
