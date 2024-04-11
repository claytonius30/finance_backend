using BackendFinance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackendFinance.Data
{
    public class FinanceTrackerDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public FinanceTrackerDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .OwnsOne(u => u.FinancialSummary, fs =>
                {
                    fs.Property<decimal>("TotalIncome").HasColumnType("decimal(18,2)");
                    fs.Property<decimal>("TotalExpense").HasColumnType("decimal(18,2)");

                    fs.OwnsMany(f => f.Incomes, income =>
                    {
                        income.WithOwner().HasForeignKey("Id"); // Configure as owned property
                        income.ToTable("Incomes"); // Specify the name of the table for Incomes if needed
                        income.Property<decimal>("Amount").HasColumnType("decimal(18,2)");
                    });

                    fs.OwnsMany(f => f.Expenses, expense =>
                    {
                        expense.WithOwner().HasForeignKey("Id"); // Configure as owned property
                        expense.ToTable("Expenses"); // Specify the name of the table for Expenses if needed
                        expense.Property<decimal>("Amount").HasColumnType("decimal(18,2)");
                    });
                });

            //IList<User> userList = new List<User>();
            //userList.Add(new User() { Id = 1, FirstName = "Rick", LastName = "Johnson" });
            //userList.Add(new User() { Id = 2, FirstName = "Sally", LastName = "Smith" });
            //userList.Add(new User() { Id = 3, FirstName = "Frank", LastName = "Laslow" });

            //modelBuilder.Entity<User>().HasData(userList);
        }
    }
}
