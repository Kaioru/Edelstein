using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Database.Entities.Characters;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class FuncKeyMappedModifiedHandler : AbstractFieldUserHandler
    {
        public override Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var v3 = packet.Decode<int>();

            if (v3 > 0) return Task.CompletedTask;
            var count = packet.Decode<int>();

            for (var i = 0; i < count; i++)
            {
                var key = packet.Decode<int>();

                user.Character.FunctionKeys[key] = new FunctionKey
                {
                    Type = packet.Decode<byte>(),
                    Action = packet.Decode<int>()
                };
            }

            return Task.CompletedTask;
        }
    }
}