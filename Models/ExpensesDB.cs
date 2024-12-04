using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Models
{
    public class ExpensesDB : IdentityDbContext<IdentityUser>
    {
        public ExpensesDB(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Expenses> Expenses { get; set; } = default!;
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Expenses>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a=>a.UserId);
        }
    }
    
}
