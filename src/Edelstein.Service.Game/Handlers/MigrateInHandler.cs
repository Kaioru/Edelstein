using System;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Guild;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities.Social;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers
{
    public class MigrateInHandler : AbstractPacketHandler<GameServiceAdapter>
    {
        protected override async Task Handle(
            GameServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var characterID = packet.Decode<int>();

            packet.Decode<long>(); // MachineID 1
            packet.Decode<long>(); // MachineID 2

            packet.Decode<bool>(); // isUserGM
            packet.Decode<byte>(); // Unk

            var clientKey = packet.Decode<long>();

            try
            {
                await adapter.TryMigrateFrom(characterID, clientKey);

                var field = adapter.Service.FieldManager.Get(adapter.Character.FieldID);
                var fieldUser = new FieldUser(adapter)
                {
                    Party = await adapter.Service.PartyManager.Load(adapter.Character)
                    // TODO: guild
                };

                adapter.User = fieldUser;

                await fieldUser.UpdateStats();
                await field.Enter(fieldUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await adapter.Close();
            }
        }
    }
}