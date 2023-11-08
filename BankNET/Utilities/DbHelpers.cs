using BankNET.Data;
using BankNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Static class containing methods for accessing the database in different ways.
    internal static class DbHelpers
    {
        // Returns a list of all users in the database.
        public static List<User> GetAllUsers(BankContext context)
        {
            List<User> users = context.Users.ToList();
            return users;
        }

        // Add method for adding users.

        
    }
}