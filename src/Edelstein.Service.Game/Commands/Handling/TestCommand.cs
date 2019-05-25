using System;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class TestCommand : AbstractCommand<object>
    {
        public override string Name => "Test";
        public override string Description => "A test command";

        public TestCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task Execute(FieldUser sender, object option)
        {
            var petItem = new ItemSlotPet
            {
                TemplateID = 5000000,
                CashItemSN = DateTime.Now.Ticks,
                PetName = "Da Cong"
            };
            var petObj = new FieldUserPet(sender, petItem, 0);

            sender.Pets.Add(petObj);
            await sender.ModifyInventory(i => i.Add(petItem));
            await sender.Field.BroadcastPacket(petObj.GetEnterFieldPacket());
        }
    }
}