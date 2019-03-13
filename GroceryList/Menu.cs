using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryList
{
    internal class Menu
    {
        static string[] _options = new string[]
        {
            "See Available Grocery Items",
            "Add Grocery Item",
            "Remove Grocery Item",
            "Make a Shopping List",
            "Quit"
        };

        static void Display()
        {
            for (int i = 0; i < _options.Length; i++)
            {
                Console.WriteLine($"{i + 1}) {_options[i]}");
            }
        }

        internal static int Prompt()
        {
            bool valid = false;
            int parsedOption = 0;
            string option = string.Empty;

            Display();
            do
            {
                option = CommandLine.Prompt($"Please select an option (1-{_options.Length}): ");
                bool canParse = int.TryParse(option, out parsedOption);
                valid = canParse && parsedOption > 0 && parsedOption <= 5;

                if (!valid)
                {
                    Console.WriteLine("'" + option + "' is not a valid option. Please provide a number 1-5");
                }

            }
            while (!valid);

            return parsedOption;
        }
    }
}
