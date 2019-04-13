using Edelstein.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Database
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }
    }
}