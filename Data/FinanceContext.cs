using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NETFinalProject.Models;
using System.Net.Sockets;

namespace NETFinalProject.Data
{
    public class FinanceContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);


            modelBuilder.Entity<User>()
                .OwnsOne(u => u.FinancialSummary, fs =>
                {
                    fs.Property<decimal>("TotalIncome").HasColumnType("decimal(18,2)");
                    fs.Property<decimal>("TotalExpense").HasColumnType("decimal(18,2)");

                    fs.Ignore(f => f.Incomes);
                    fs.Ignore(f => f.Expenses);

                    //        fs.OwnsMany(f => f.Incomes, income =>
                    //        {
                    //            income.WithOwner().HasForeignKey("UserId"); // Configure as owned property
                    //            income.ToTable("Incomes"); // Specify the name of the table for Incomes if needed
                    //            income.Property<decimal>("Amount").HasColumnType("decimal(18,2)");
                    //        });

                    //        fs.OwnsMany(f => f.Expenses, expense =>
                    //        {
                    //            expense.WithOwner().HasForeignKey("UserId"); // Configure as owned property
                    //            expense.ToTable("Expenses"); // Specify the name of the table for Expenses if needed
                    //            expense.Property<decimal>("Amount").HasColumnType("decimal(18,2)");
                    //        });
                });


            //modelBuilder.Entity<Income>()
            //    .HasOne(i => i.User);

            //modelBuilder.Entity<Expense>()
            //    .HasOne(e => e.User);


            //modelBuilder.Entity<User>()
            //    .Property(u => u.FinancialSummary.TotalIncome)
            //    .HasColumnType("decimal(18,2)");

            //modelBuilder.Entity<User>()
            //    .Property(u => u.FinancialSummary.TotalExpense)
            //    .HasColumnType("decimal(18,2)");


            IList<User> userList = new List<User>();
            userList.Add(new User() { UserId = 1, FirstName = "Rick", LastName = "Johnson" });
            userList.Add(new User() { UserId = 2, FirstName = "Sally", LastName = "Smith" });
            userList.Add(new User() { UserId = 3, FirstName = "Frank", LastName = "Laslow" });

            modelBuilder.Entity<User>().HasData(userList);

            // Explicitly specifies SQL server column types for Amount properties in Expense and Income entities
            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Income>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");

            //base.OnModelCreating(modelBuilder);
        }
    }
}
