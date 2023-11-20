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
            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            using BankContext context = new BankContext();
            
            MenuUI.LoginScreen(context);
               
            // Add welcome-screen

            // Add login interface. Logging out from an account should bring you back here.

            // Add methods running either admin or user menus.

            // Add methods running either admin or user menus.

        }
    }
}