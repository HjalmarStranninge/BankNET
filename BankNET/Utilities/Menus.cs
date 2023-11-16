﻿using BankNET.Data;
using BankNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Class containing methods that print the different menus.
    internal static class Menus
    {
        
        public static void Login(BankContext context)
        {
            bool tryAgainUsername = true;
            bool tryAgainPin = true;
            int i = 0;
            
            Console.WriteLine("Log in");

            while (tryAgainUsername && i < 3)
            {
                Console.Write("Enter username: ");
                string username = Console.ReadLine();

                bool validUsername = context.Users.Any(uN => uN.UserName.Equals(username));
            
                if (validUsername)
                {
                    Console.Write("Enter pin: ");
                    string pin = Console.ReadLine();

                    bool validPin = context.Users.Any(p => p.Pin.Equals(pin));
                    int j = 0;

                    while (tryAgainPin && j < 3)
                    {
                        if (validPin)
                        {
                            Console.WriteLine("Login Successful!");
                            tryAgainPin = false;
                            
                            if (username == "admin") AdminMenu(context);
                            else UserMainMenu(context, username);
                        }
                        else Console.WriteLine($"Wrong pin, you have {3 - j} tries left.");
                        j++;
                    }

                    tryAgainUsername = false;
                }
                else Console.WriteLine($"Invalid username, you have {3 - i} tries left.");
                i++;
            }
            Environment.Exit(0);
            
        }

        // Method for user Main Menu and handling menu
        public static void UserMainMenu(BankContext context, string username)
        {
            bool keepTryingUserMenu = true;
            while (keepTryingUserMenu)
            {
                Console.Clear();
                Console.WriteLine("1. See your accounts and balance" +
                "\n2. Transfer between accounts" +
                "\n3. Withdrawal" +
                "\n4. Deposit" +
                "\n5. Open new account" +
                "\n6. Log out");
                Console.Write("What would you like to do: ");
                int userMenuChoice = int.Parse(Console.ReadLine());

                switch (userMenuChoice)
                {
                    case 1:
                        UserFunctions.ViewAccountBalance(context, username);
                        break;
                    case 2:
                        UserFunctions.TransferInternal(context, username); 
                        break;
                    case 3:
                        UserFunctions.Withdraw(context, username);
                        break;
                    case 4:
                        UserFunctions.Deposit(context, username);
                        break;
                    case 5:
                        UserFunctions.CreateNewAccount(context, username); 
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine("You are logged out.");
                        keepTryingUserMenu = false;
                        break;
                    default:
                        Console.Write("Invalid input, try again.");
                        break;
                }
            }
        }
        internal static void AdminMenu(BankContext context)
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
                            AdminFunctions.ViewUsers(context);
                            break;
                        case "2":
                            AdminFunctions.CreateUser(context);
                            break;
                        case "e":
                            return;
                        default:
                            Console.Clear();
                            Console.WriteLine($"Unknown command : {command}");
                            break;
                    }
                }
            
        }
    }
}