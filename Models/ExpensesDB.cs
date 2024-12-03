using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Models
{
    public class ExpensesDB(DbContextOptions<ExpensesDB> options) : DbContext(options)
    {
        public DbSet<ExpenseTracker.Models.Expenses> Expenses { get; set; } = default!;
    }
    
}
