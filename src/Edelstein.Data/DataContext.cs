using System.Linq;
using Edelstein.Data.Entities;
using Edelstein.Data.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Edelstein.Data
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Character> Characters { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemSlotEquip>().HasBaseType<ItemSlot>();
            modelBuilder.Entity<ItemSlotBundle>().HasBaseType<ItemSlot>();
            modelBuilder.Entity<ItemSlotPet>().HasBaseType<ItemSlot>();

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Data)
                .WithOne(a => a.Account)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AccountData>()
                .HasMany(a => a.Characters)
                .WithOne(c => c.Data)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Character>()
                .HasMany(c => c.Inventories)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ItemInventory>()
                .HasMany(i => i.Items)
                .WithOne(i => i.ItemInventory)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public void InsertUpdateOrDeleteGraph(object entity)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            if (entity is Character character)
            {
                var existing = Characters
                    .AsNoTracking()
                    .Include(c => c.Inventories)
                    .ThenInclude(c => c.Items)
                    .FirstOrDefault(c => c.ID == character.ID);

                if (existing != null)
                {
                    var existingItems = existing.Inventories.SelectMany(i => i.Items).ToList();
                    var currentItems = character.Inventories.SelectMany(i => i.Items).ToList();

                    foreach (var existingItem in existingItems)
                    {
                        if (currentItems.All(i => i.ID != existingItem.ID))
                            Entry(existingItem).State = EntityState.Deleted;
                    }
                }
            }
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            InsertUpdateOrDeleteGraph(entity);
            return base.Update(entity);
        }
    }
}