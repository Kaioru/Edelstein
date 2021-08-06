using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Gameplay.Users;
using Edelstein.Common.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Interop.Contracts;
using Edelstein.Protocol.Network;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class CreateNewCharacterHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.CreateNewCharacter;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.SelectCharacter);

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {
            var name = packet.ReadString();
            var race = packet.ReadInt();
            var subJob = packet.ReadShort();
            var face = packet.ReadInt();
            var hair = packet.ReadInt();
            var hairColor = packet.ReadInt();
            var skin = packet.ReadInt();
            var coat = packet.ReadInt();
            var pants = packet.ReadInt();
            var shoes = packet.ReadInt();
            var weapon = packet.ReadInt();
            var gender = packet.ReadByte();

            var result = LoginResultCode.Success;
            var response = new UnstructuredOutgoingPacket(PacketSendOperations.CreateNewCharacterResult);

            var world = await user.Stage.WorldTemplates.Retrieve((int)user.SelectedWorldID);

            if (world.BlockCharCreation) result = LoginResultCode.NotConnectableWorld;

            var serverRequest = new DescribeServersRequest();

            serverRequest.Tags.Add("Type", Enum.GetName(ServerStageType.Game));
            serverRequest.Tags.Add("WorldID", user.SelectedWorldID.ToString());
            serverRequest.Tags.Add("ChannelID", user.SelectedChannelID.ToString());

            var server = (await user.Stage.ServerRegistryService.DescribeServers(serverRequest)).Servers.FirstOrDefault();

            if (server == null) result = LoginResultCode.NotConnectableWorld;

            response.WriteByte((byte)result);

            if (result == LoginResultCode.Success)
            {
                var character = new Character
                {
                    AccountWorldID = user.AccountWorld.ID,
                    Name = name,
                    Job = 0, // TODO: race -> job
                    Face = face,
                    Hair = hair + hairColor,
                    Skin = (byte)skin,
                    Gender = gender,
                    FieldID = 310000000, // TODO: start maps
                    FieldPortal = 0,
                    SubJob = 0 // TODO: race -> subjob
                };
                var context = new ModifyMultiInventoryContext(character.Inventories, user.Stage.ItemTemplates);

                context.Set(BodyPart.Clothes, coat);
                context.Set(BodyPart.Shoes, shoes);
                context.Set(BodyPart.Weapon, weapon);
                if (pants > 0)
                    context.Set(BodyPart.Pants, pants);

                await user.Stage.CharacterRepository.Insert(character);

                user.Character = character;
                user.Stage.Logger.LogDebug($"Created new {race} character: {name} (ID: {character.ID})");

                response.WriteCharacterStats(character);
                response.WriteCharacterLook(character);
                response.WriteBool(false);
                response.WriteBool(false);
            }
            else response.WriteInt(0);

            await user.Dispatch(response);
        }
    }
}
