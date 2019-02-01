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

            modelBuilder.Entity<AccountData>()
                .HasOne(a => a.Locker);
            modelBuilder.Entity<ItemLocker>()
                .HasMany(i => i.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AccountData>()
                .HasOne(a => a.Trunk);
            modelBuilder.Entity<ItemTrunk>()
                .HasMany(i => i.Items)
                .WithOne(i => i.ItemTrunk)
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
                    .Include(c => c.Data)
                    .ThenInclude(c => c.Locker)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.Data)
                    .ThenInclude(c => c.Trunk)
                    .ThenInclude(c => c.Items)
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

                    if (character.Data?.Locker?.Items != null)
                    {
                        var existingLockerItems = existing.Data.Locker.Items.ToList();
                        var currentLockerItems = character.Data.Locker.Items.ToList();

                        foreach (var existingTrunkItem in existingLockerItems)
                        {
                            if (currentLockerItems.All(i => i.ID != existingTrunkItem.ID))
                                Entry(existingTrunkItem).State = EntityState.Deleted;
                        }
                    }

                    if (character.Data?.Trunk?.Items != null)
                    {
                        var existingTrunkItems = existing.Data.Trunk.Items.ToList();
                        var currentTrunkItems = character.Data.Trunk.Items.ToList();

                        foreach (var existingTrunkItem in existingTrunkItems)
                        {
                            if (currentTrunkItems.All(i => i.ID != existingTrunkItem.ID))
                                Entry(existingTrunkItem).State = EntityState.Deleted;
                        }
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