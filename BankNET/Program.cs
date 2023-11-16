using BankNET.Utilities;
using System.Net.Http.Headers;
﻿using BankNET.Data;
using BankNET.Models;
using BankNET.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;


namespace BankNET
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using BankContext context = new BankContext();

            while (true)
            {
                Console.WriteLine("Welcome to BankNET!");
                Console.WriteLine("Would you like to log in (y/n)?");
                string wantToLogin = Console.ReadLine().ToLower();

                if (wantToLogin == "y")
                {
                    LogInLogOut.Login(context);
                }
            }
            
           
            // Add welcome-screen

            // Add login interface. Logging out from an account should bring you back here.

            // Add methods running either admin or user menus.

            // Add methods running either admin or user menus.

        }
    }
}