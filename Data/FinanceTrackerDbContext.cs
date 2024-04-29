// Clayton DeSimone
// Web Services
// Final Project
// 4/29/2024

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
        public DbSet<Goal> Goals { get; set; }

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
                        // Configured as owned property
                        income.WithOwner().HasForeignKey("Id"); 
                        income.ToTable("Incomes");
                        income.Property<decimal>("Amount").HasColumnType("decimal(18,2)");
                    });

                    fs.OwnsMany(f => f.Expenses, expense =>
                    {
                        expense.WithOwner().HasForeignKey("Id");
                        expense.ToTable("Expenses");
                        expense.Property<decimal>("Amount").HasColumnType("decimal(18,2)");
                    });

                    fs.OwnsMany(f => f.Goals, goal =>
                    {
                        goal.WithOwner().HasForeignKey("Id");
                        goal.ToTable("Goals");
                        goal.Property<decimal>("Amount").HasColumnType("decimal(18,2)");
                    });
                });
        }
    }
}
