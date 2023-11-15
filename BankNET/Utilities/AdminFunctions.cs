using BankNET.Data;
using BankNET.Models;
using BankNET.Utilities;
using Microsoft.EntityFrameworkCore;
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
        
        //Method for creating user
            internal static void CreateUser(BankContext context)
            {
                string userName;
                while (true)
                {
                    Console.WriteLine("Create user");
                    Console.WriteLine("Enter user name:");
                    userName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        Console.Clear();
                        Console.WriteLine("Cannot be blank");
                    }
                    else
                    {
                        Console.WriteLine();
                        break;

                    }
                }

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
            internal static void ViewUsers(BankContext context)
            {
                List<User> users = DbHelpers.GetAllUsers(context);
                Console.WriteLine($"Total number of users in system: {users.Count()}");
                Console.WriteLine("Current users in system:");

                foreach (User user in users)
                {
                    Console.WriteLine($"{user.UserName}");
                }
                while (true)
                {
                    Console.WriteLine("1. View user");
                    Console.WriteLine("e. Go back");
                    string option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            SelectUser(context);
                            break;
                        case "e":
                            Console.Clear();
                            return;
                        default:
                            Console.Clear();
                            Console.WriteLine("Current users in system:");
                        
                            foreach (User user in users)
                            {
                                Console.WriteLine($"{user.UserName}");
                            }
                            Console.WriteLine("Unknown command");
                            break;

                    }
                }
            }

            //Selects user and shows accounts connected
            internal static void SelectUser(BankContext context)
            {
                Console.WriteLine("Insert name: ");
                string userSelect = Console.ReadLine();
                var UserSelection = context.Users
                    .Where(u => u.UserName == userSelect)
                    .Select(u => new { u.Id, u.UserName, u.Pin, AccountCount = u.Accounts.Count(), u.Accounts })
                    .ToList();

                foreach (var u in UserSelection)
                {
                    Console.WriteLine(
                        $"Name: {u.UserName}," +
                        $"\tPIN: xxxx" +
                        $"\tAccounts: {u.AccountCount}," 
                        );
                }
                var UserAccounts = context.Users
                    .Where (u => u.UserName == userSelect)
                    .Include (u=> u.Accounts)
                    .Single()
                    .Accounts
                    .ToList();
                foreach (var ua in UserAccounts)
                {
                    Console.WriteLine($"AccountName: {ua.AccountName}\tAccountNumber: {ua.AccountNumber}\tBalance: {ua.Balance}");
                }

                //* add password confirmation to view pin and change if wanted

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
