using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Commands;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserChatHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<int>();

            var message = packet.Decode<string>();
            var onlyBalloon = packet.Decode<bool>();

            if (message.StartsWith(CommandManager.Prefix))
            {
                try
                {
                    await user.Service.CommandManager.Process(
                        user,
                        message.Substring(1)
                    );
                }
                catch (Exception)
                {
                    await user.Message("An error has occured while executing that command.");
                }

                return;
            }

            using var p = new Packet(SendPacketOperations.UserChat);
            p.Encode<int>(user.ID);
            p.Encode<bool>(false);
            p.Encode<string>(message);
            p.Encode<bool>(onlyBalloon);
            await user.Field.BroadcastPacket(p);
        }
    }
}