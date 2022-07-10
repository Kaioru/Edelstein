using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Users.Keys;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class FuncKeyMappedModifiedHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.FuncKeyMappedModified;

        protected override Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            var v3 = packet.ReadInt();

            if (v3 > 0) return Task.CompletedTask;
            var count = packet.ReadInt();

            for (var i = 0; i < count; i++)
            {
                var key = packet.ReadInt();

                user.Character.FunctionKeys[key] = new CharacterFunctionKey
                {
                    Type = packet.ReadByte(),
                    Action = packet.ReadInt()
                };
            }

            return Task.CompletedTask;
        }
    }
}
