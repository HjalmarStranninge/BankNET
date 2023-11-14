using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Pin { get; set; }
        public virtual List<Account> Accounts { get; set;}
    }
}
