using BankNET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    internal class RunBankNET
    {
        internal static void Start()
        {
            // Hides cursor for a cleaner look. Will be made visible whenever user is asked for input to make it more clear and user friendly.
            Console.CursorVisible = false;

            // Sets encoding to unicode which is needed to display some special characters.
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            // Creates new context for interacting with the database.
            using BankContext context = new BankContext();

            MenuUI.ClearAndPrintFooter();

            AdminFunctions.GenerateAdmin(context);

            bool runProgram = true;
            while (runProgram)
            {
                // Runs the Welcome Screen menu method.
                MainMenus.WelcomeScreenMenu(context);
            }
        }
    }
}
