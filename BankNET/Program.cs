using BankNET.Utilities;
using System.Net.Http.Headers;
using BankNET.Data;
using BankNET.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;


namespace BankNET
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Hides cursor for a cleaner look. Will be made visible whenever user is asked for input to make it more clear and user friendly.
            Console.CursorVisible = false;

            // Sets encoding to unicode which is needed to display some special characters.
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            // Creates new context for interacting with the database.
            using BankContext context = new BankContext();

            bool runProgram = true;
            while (runProgram)
            {
                // Runs the login method.
                string username = LogInLogOut.Login(context);

                if (username.ToLower() == "admin")
                {
                    MainMenus.AdminMenu(context, username);
                }

                else
                {
                    MainMenus.UserMainMenu(context, username);
                }
            }           
        }
    }
}