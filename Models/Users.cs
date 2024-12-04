using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ExpenseTracker.Models
{
    public class Users : IdentityUser
    {
        public ICollection<Expenses> Expenses {get;set;} = new List<Expenses>();
    }
    
}