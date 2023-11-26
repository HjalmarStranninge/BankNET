using BankNET.Data;
using BankNET.Models;
using BankNET.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;



namespace BankNET.Utilities
{
    // Static class containing all functions available to the admin. 
    internal static class AdminFunctions
    {
        static bool validAdminPin;

        //Method for viewing list of all users and number of users.
        internal static void ViewUsers(BankContext context, string adminName)
        {
            bool runMenu = true;
            while (runMenu)
            {
                MenuUI.ClearAndPrintFooter();
                List<User> users = DbHelpers.GetAllUsers(context);
                Console.WriteLine($"Number of users in system: {users.Count()-1}");
                Console.WriteLine("---");
                Console.WriteLine("Current users in system:");

                foreach (User user in users)
                {
                    // Doesn't display the admin user.
                    if(user.UserName != adminName)
                    {
                        Console.WriteLine($"{user.UserName}");
                    }
                }
                Console.WriteLine("---");

                runMenu = SelectUser(context, adminName);
            }
        }

        //Selects user and shows accounts connected. Leads admin to user submenu. Returns bool if user not found to print ViewUsers menu again.
        internal static bool SelectUser(BankContext context, string adminName)
        {
                Console.Write("Enter name to view user / 'r' to return: ");
                Console.CursorVisible = true;

                string userSelect = Console.ReadLine();
                Console.CursorVisible = false;
                Console.Beep();

                Console.CursorVisible = false;

                MenuUI.ClearAndPrintFooter();
                Console.WriteLine();

                var userSelection = context.Users
                    .Where(u => u.UserName == userSelect)
                    .Select(u => new
                    {
                        u.Id,
                        u.UserName,
                        u.Pin,
                        AccountCount = u.Accounts.Count(),
                        Accounts = u.Accounts.Select(a => new { a.AccountName, a.AccountNumber, a.Balance }),
                        TotalBalance = u.Accounts.Sum(a => a.Balance)
                    })
                    .ToList();

                // If user viable this will list a summary of user and then the connected accounts
                if (userSelection.Any())
                {
                    foreach (var u in userSelection)
                    {
                        Console.WriteLine(
                            $"Username: {u.UserName}" +
                            $"\tPin: ****" +
                            $"\r\nAccounts: {u.AccountCount} \t        Total balance: {u.TotalBalance,2} SEK");
                    }

                    var userAccounts = context.Users
                        .Where(u => u.UserName == userSelect)
                        .Include(u => u.Accounts)
                        .Single()
                        .Accounts
                        .ToList();

                    foreach (var ua in userAccounts)
                    {
                        Console.WriteLine($"{ua.AccountNumber}\t{ua.AccountName}\t{ua.Balance,2} SEK");
                    }

                    // Lists admin to user submenu of choices
                    AdminUserView(context, userSelect, adminName, userAccounts);
                    return false;
                }
                else if (userSelect == "r")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("\n\t   User not found. Please try again.");
                    Thread.Sleep(2000);
                    return true;
                }
            
        }
        // Admin to user submenu. Brings along selected username and admin username.
        internal static void AdminUserView(BankContext context, string userSelect, string adminName, List<Account> userAccounts)
        {
            ConsoleKeyInfo key;
            bool runMenu = true;
            while (runMenu && !InvalidInputHandling.IsLockedOut())
            {
                int selectedOption = 0;

                // Saving the current cursor position in a variable so we can return to it later.
                int originalTop = Console.CursorTop;
                string[] menuOptions = { "Show pin", "Delete user", "Exit" };

                // Showing menu with the option to move between menu options with arrow keys until user chooses option with ENTER
                do
                {
                    for (int i = 0; i < menuOptions.Length; i++)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop + 1); 
                        if (i == selectedOption)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;

                            Console.Write($"{menuOptions[i]}");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write($"{menuOptions[i]}");
                        }
                    }
                    
                    key = Console.ReadKey();
                    Console.Beep();

                    // Overwrites only the 3 rows with options, so that the information above remains.
                    Console.SetCursorPosition(0, originalTop + 1);
                    Console.Write("                                  ");
                    Console.SetCursorPosition(0, originalTop + 2);
                    Console.Write("                                  ");
                    Console.SetCursorPosition(0, originalTop + 3);
                    Console.Write("                                  ");

                    // Resets the cursor to where the first option is supposed to be.
                    Console.SetCursorPosition(0, originalTop);
                    switch (key.Key)
                    {
                        case ConsoleKey.DownArrow:
                            selectedOption = (selectedOption - 2 + menuOptions.Length) % menuOptions.Length;
                            break;

                        case ConsoleKey.UpArrow:
                            selectedOption = (selectedOption + 2) % menuOptions.Length;
                            break;

                    }
                } while (key.Key != ConsoleKey.Enter);

                if (key.Key == ConsoleKey.Enter)
                {
                    switch (selectedOption)
                    {
                        case 0:
                            ShowPin(context, userSelect, adminName, userAccounts);
                            runMenu = false;
                            break;

                        case 1:
                            DeleteUser(context, userSelect, adminName, userAccounts);
                            runMenu = false;
                            break;

                        case 2:
                            runMenu = false;
                            break;
                    }
                }
            }
        }

        // Method for showing user pin after confirming with admin pin.
        private static void ShowPin(BankContext context, string userSelect, string adminName, List<Account> userAccounts)
        {
            // Declare variable to limit number of attempts admin can enter incorrect pin until lock out
            int attemptsLeft = 3;
            do
            {
                Console.Write("\nPlease enter admin pin to view user pin: ");

                validAdminPin = BankHelpers.PinCheck(context, adminName);

                // If admin pin is correct then same user view is shown but with user pin instead of ****
                if (validAdminPin)
                {
                    Console.Beep();

                    var user = context.Users
                        .FirstOrDefault(u => u.UserName == userSelect);
                    MenuUI.ClearAndPrintFooter();

                    Console.WriteLine($"\nUsername: {user.UserName} " +
                                      $"\tPin: {user.Pin}");

                    foreach (var ua in userAccounts)
                    {
                        Console.WriteLine($"{ua.AccountNumber}\t{ua.AccountName}\t{ua.Balance} SEK");
                    }
                    Console.WriteLine("\n\n\t       Press ENTER to continue");
                    Console.ReadKey();
                    Console.Beep();
                }
                else
                {
                    attemptsLeft--;
                    InvalidInputHandling.IncorrectNameOrPin(attemptsLeft, "\n\t            Incorrect pin.");
                }
            } while (!validAdminPin && !InvalidInputHandling.IsLockedOut());
        }

        //Method for creating user
        internal static void CreateUser(BankContext context)
        {
            List<User> users = DbHelpers.GetAllUsers(context);
            bool usernameIsNull;
            bool usernameAlreadyExists;
            // Loop to make sure created user has valid username (not already existing/not null).
            do
            {
                MenuUI.ClearAndPrintFooter();

                Console.Write("\n\t Enter new username: ");

                Console.CursorVisible = true;

                string newUsername = Console.ReadLine().Trim();
                usernameIsNull = string.IsNullOrWhiteSpace(newUsername);
                usernameAlreadyExists = users.Any(user => user.UserName.ToLower() == newUsername.ToLower());

                Console.CursorVisible = false;
                Console.Beep();
                MenuUI.ClearAndPrintFooter();

                if (usernameIsNull)
                {
                    InvalidInputHandling.InvalidInputName();
                }
                else if (usernameAlreadyExists)
                {
                    Console.WriteLine("\n Username already exists. Please try again.");
                    Thread.Sleep(2000);
                }
                // Valid username tries to create a user with pin.
                else
                {
                    usernameIsNull = false;
                    usernameAlreadyExists = false;

                    string pin = BankHelpers.GeneratePin();

                    User newUser = new User()
                    {
                        UserName = newUsername,
                        Pin = pin
                    };

                    // AddUser sends user newUser to database and print userinfo if success.
                    bool success = DbHelpers.AddUser(context, newUser);

                    MenuUI.ClearAndPrintFooter();

                    if (success)
                    {
                        Console.WriteLine($"\n\t Created user {newUser.UserName} with PIN: {newUser.Pin}");
                        Thread.Sleep(3000);
                    }
                }
            } while (usernameIsNull || usernameAlreadyExists);
        }

        // Method for deleting user by confirming admin pin.
        private static void DeleteUser(BankContext context, string userSelect, string adminName, List<Account> userAccounts)
        {
            int attemptsLeft = 3;
            do
            { 
                if (userSelect != adminName)
                {
                    Console.Write("\n");
                    Console.Write("Confirm user deletion with admin pin: ");
                    bool validAdminPin = BankHelpers.PinCheck(context, adminName);
                    
                    if (validAdminPin)
                    {
                        // If statements states that the sum of the users accounts balance need to be zero to be deleted
                        var userToDelete = context.Users.FirstOrDefault(u => u.UserName == userSelect);
                        if (BankHelpers.CheckAccountBalanceZero(userAccounts))
                        {
                            if (userToDelete != null)
                            {
                                bool success = DbHelpers.DeleteUser(context, userToDelete);
                                if (success)
                                {
                                    MenuUI.ClearAndPrintFooter();
                                    Console.WriteLine($"\n   Deleted user {userToDelete.UserName} and any connected accounts.");
                                    Thread.Sleep(2000);
                                }
                                else
                                {
                                    MenuUI.ClearAndPrintFooter();
                                    Console.WriteLine($"\n      Failed to delete user with username {userToDelete.UserName}");
                                    Thread.Sleep(2000);
                                }
                            }
                        }
                        else
                        {
                            MenuUI.ClearAndPrintFooter();
                            Console.WriteLine($"\n\t     Cannot delete user {userToDelete.UserName}");
                            Console.WriteLine("\n\t Some accounts have remaining balance.");
                            Thread.Sleep(2000);
                        }
                    }
                    // Admin pin is checked and will lock out after three failed attempts
                    else
                    {
                        attemptsLeft--;
                        InvalidInputHandling.IncorrectNameOrPin(attemptsLeft, "\n\t            Incorrect pin.");
                    }
                }

                else
                {
                    MenuUI.ClearAndPrintFooter();
                    Console.WriteLine($"\n\t     Admin user cannot be deleted.");
                    Thread.Sleep(2000);
                }
            } while (!validAdminPin && !InvalidInputHandling.IsLockedOut()) ;
        }

        // Checks if 'admin' user exists, if not, creates one.
        internal static void GenerateAdmin(BankContext context)
        {
            bool adminAccountExists = false;
            foreach (var account in context.Users)
            {
                if (account.UserName.ToLower() == "admin")
                {
                    adminAccountExists = true;
                }
            }
            if (!adminAccountExists)
            {
                User newAdmin = new User();
                newAdmin.UserName = "admin";
                newAdmin.Pin = "1234";

                bool adminCreated = DbHelpers.AddUser(context, newAdmin);

                if (adminCreated)
                {
                    Console.WriteLine("\n\t     Administrator account created");
                    Console.Write("\n\t      Username: admin  Pin: 1234");
                    Thread.Sleep(3000);
                }
            }            
        }
    }
}

