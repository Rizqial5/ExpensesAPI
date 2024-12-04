using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ExpenseTracker.Models
{
    public class ExpensesDB(DbContextOptions<ExpensesDB> options) : IdentityDbContext(options)
    {
        public DbSet<Expenses> Expenses { get; set; } = default!;
    }
    
}
