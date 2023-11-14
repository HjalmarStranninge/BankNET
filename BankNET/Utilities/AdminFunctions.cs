using BankNET.Data;
using BankNET.Models;
using BankNET.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BankNET.Utilities
{
    // Static class containing all functions available to the admin. 
    internal static class AdminFunctions
    {
        //Admin menu from where rest of methods will be called from
        internal static void AdminMenu()
        {
            using (BankContext context = new BankContext())
            {
                while (true)
                {
                    Console.WriteLine("1. View all users");
                    Console.WriteLine("2. Create user");
                    Console.WriteLine("e. Exit"); // add pressing escape button to exit?
                    string command = Console.ReadLine();

                    switch (command)
                    {
                        case "1":
                            ViewUsers(context);
                            break;
                        case "2":
                            CreateUser(context);
                            break;
                        case "e":
                            return;
                        default:
                            Console.WriteLine($"Unknown command : {command}");
                            break;
                    }
                }
            }
        }
        //Method for creating user
        private static void CreateUser(BankContext context)
        {
            Console.WriteLine("Create user");
            Console.WriteLine("Enter user name:");
            string userName = Console.ReadLine();

            Random random = new Random();
            string pin = random.Next(0, 10000).ToString();
            while (pin.Length < 4)
            {
                pin = "0" + pin;
            }

            User newUser = new User()
            {
                UserName = userName,
                Pin = pin
            };
            bool success = DbHelpers.AddUser(context, newUser);
            if (success)
            {
                Console.WriteLine($"Created user {userName} with pin {pin}");
            }
            else
            {
                Console.WriteLine($"Failed to create user with username {userName}");
            }
        }
        //Method for viewing list of all users.
        private static void ViewUsers(BankContext context)
        { 
            Console.WriteLine("Current users in system:");
            List<User> users = DbHelpers.GetAllUsers(context);

            foreach (User user in users)
            {
                Console.WriteLine($"{user.UserName}");
            }

            Console.WriteLine("1. View user");
            Console.WriteLine("2. Sort A-Z"); // Is this needed?
            Console.WriteLine("3. Sort Z-A"); // Is this needed?
            Console.WriteLine("e. Go back");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    SelectUser(context);
                    break;
                case "e":
                    return;

            }
        }

        private static void SelectUser(BankContext context)
        {
            Console.WriteLine("Insert name: ");
            string userSelect = Console.ReadLine();
            var UserSelection = context.Users
                .Where(u => u.UserName == userSelect)
                .Select(u => new { u.Id, u.UserName, u.Pin })
                .ToList();
        }

        private static void DeleteUser()
        {

        }

        //* should we allow the function of deleting a user?
        //* should we allow function of reseting customer pin?
        //* adding function to check if user is logging in for first time, forcing them to change pin?
        //* adding function for user to change pin, but not to a pin last used / used within the last 6 months?
        //* should we be able to view all info on a specific user incl. transaction history
        
    }
}
