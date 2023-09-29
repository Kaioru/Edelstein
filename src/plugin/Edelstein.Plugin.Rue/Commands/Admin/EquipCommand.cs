using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class EquipCommand : AbstractCommand
{
    public override string Name => "Equip";
    public override string Description => "Modifies equipment stats";

    public override async Task Execute(IFieldUser user, string[] args)
    {
        var equipped = user.Character.Inventories[ItemInventoryType.Equip]?.Items.ToList() ?? new List<KeyValuePair<short, IItemSlot>>();
        if (equipped.Count == 0) return;

        var slot = await user.Prompt(target => target.AskMenu("Which equipment would you like to modify?", equipped
            .ToDictionary(
                i => (int)i.Key,
                i => $"{i.Value.ID}"
            )), -1);

        if (slot == -1) return;

        var item = user.Character.Inventories[ItemInventoryType.Equip]?.Items[(short)slot];
        if (item is not IItemSlotEquip equip) return;

        var sel = await user.Prompt(target => target.AskMenu("Which equipment stat?", new Dictionary<int, string>
        {
            [0] = "Grade",
            [4] = "Grade (Negative)",
            [1] = "Option 1",
            [2] = "Option 2",
            [3] = "Option 3"
        }), -1);

        if (sel == -1) return;

        switch (sel)
        {
            case 0:
                equip.Grade = (byte)await user.Prompt(target => target.AskNumber("What value?"), 0);
                break;
            case 4:
                equip.Grade = (byte)await user.Prompt(target => target.AskNumber("What value?"), 0);
                break;
            case 1:
                equip.Option1 = (short)await user.Prompt(target => target.AskNumber("What value?"), 0);
                break;
            case 2:
                equip.Option2 = (short)await user.Prompt(target => target.AskNumber("What value?"), 0);
                break;
            case 3:
                equip.Option3 = (short)await user.Prompt(target => target.AskNumber("What value?"), 0);
                break;
        }

        await user.ModifyInventory(i => i[ItemInventoryType.Equip]?.UpdateSlot((short)slot));
    }
}
