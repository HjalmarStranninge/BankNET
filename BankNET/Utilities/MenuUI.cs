using BankNET.Data;
using BankNET.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Class that handles the UI elements of the application. 
    internal static class MenuUI
    {

        // Prints a header with logo.
        public static void PrintHeader()
        {
            Console.Write($"------------------- <> Bank");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("NET");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\u2122 <> ------------------\n\n\n");
        }

        // Clears console and prints footer at the bottom before resetting cursor position.
        public static void ClearAndPrintFooter()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 10);
            Console.WriteLine("\n\n\n-----------------------------------------------------");
            Console.SetCursorPosition(0, 0);
            PrintHeader();

        }                   
    }
}
