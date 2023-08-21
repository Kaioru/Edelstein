using Edelstein.Common.Database;
using Edelstein.Common.Database.Entities;
using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Inventories.Items;
using Edelstein.Protocol.Gameplay.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile("devsettings.json", true)
    .AddJsonFile("devsettings.Development.json", true)
    .Build();
var connection = configuration.GetConnectionString(GameplayDbContext.ConnectionStringKey);
var builder = new DbContextOptionsBuilder<GameplayDbContext>().UseNpgsql(connection);
var context = new GameplayDbContext(builder.Options);

var newAccount = new AccountEntity
{
    Username = "pooop"
};
var newAccountWorld = new AccountWorldEntity
{
    Account = newAccount,
    WorldID = 1
};
var newCharacter = new CharacterEntity
{
    AccountWorld = newAccountWorld,
    Name = "poopers"
};

newAccountWorld.Trunk.Items[0] = new ItemSlotEquip
{
    Title = "uwunya"
};
newCharacter.Inventories[ItemInventoryType.Install].SlotMax = 96;
newCharacter.Inventories[ItemInventoryType.Install].Items[0] = new ItemSlotEquip();

context.Add(newAccount);
context.Add(newAccountWorld);
context.Add(newCharacter);
context.SaveChanges();
