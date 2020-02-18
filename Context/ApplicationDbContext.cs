using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AdminReciptsDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AdminReciptsDemo.Context
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) { }

        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Currency> MultiCurrency { get; set; }
    }
}