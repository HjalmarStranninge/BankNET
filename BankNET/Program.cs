using BankNET.Data;
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
            // Add welcome-screen

            // Add login interface. Logging out from an account should bring you back here.

            // Add methods running either admin or user menus.

            using (var context = new BankContext())
            {
                var user = context.Users
                    .Include(u => u.Accounts)
                    .Where(u => u.Id == 1)
                    .First();
                    


                
                UserFunctions.TransferInternal(user);
            };

        } 
    }
}