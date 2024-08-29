using System;
using System.Text.Json;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Entities.Inventories.Modifiers;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketCreateNewCharacter(
    ICharacterRepository characters,
    ITemplateManager<IItemTemplate> itemTemplates
) : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CreateNewCharacter>>
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CreateNewCharacter> message)
    {
        if (message.User.State != LoginState.SelectCharacter) return;

        try
        {
            var character = new Character
            {
                AccountWorldDataID = message.User.AccountWorldData!.ID,
                Name = message.Packet.Name.Value,
                Job = 0,
                Face = message.Packet.Face,
                Hair = message.Packet.Hair + message.Packet.HairColor,
                Skin = (byte)message.Packet.Skin,
                Gender = message.Packet.Gender,
                FieldID = 0,
                FieldPortal = 0,
                SubJob = 0
            };
            var context = new ModifyInventoryContextGroup(character.Inventories, itemTemplates);
            
            context.SetEquipped(BodyPart.Clothes, message.Packet.Coat);
            context.SetEquipped(BodyPart.Shoes, message.Packet.Shoes);
            context.SetEquipped(BodyPart.Weapon, message.Packet.Weapon);
            if (message.Packet.Pants > 0)
                context.SetEquipped(BodyPart.Pants, message.Packet.Pants);
            
            character = await characters.Insert(character);
            
            await message.User.Dispatch(new CreateNewCharacterResult
            {
                Result = LoginResultCode.Success,
                Info = new CreateNewCharacterResultInfo
                {
                    CharacterStat = character.ToStructuredCharacterStat(),
                    CharacterLook = character.ToStructuredCharacterLook()
                }
            });
        }
        catch
        {
            await message.User.Dispatch(new CreateNewCharacterResult
            {
                Result = LoginResultCode.Unknown
            });
        }
    }
}
