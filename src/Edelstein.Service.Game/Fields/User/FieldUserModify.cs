using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.User.Stats.Modify;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser
    {
        public void AvatarModified()
        {
            using (var p = new Packet(SendPacketOperations.UserAvatarModified))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(0x1); // Flag
                Character.EncodeLook(p);

                p.Encode<bool>(false);
                p.Encode<bool>(false);
                p.Encode<bool>(false);
                p.Encode<int>(0); // Completed Set ID

                Field.BroadcastPacket(this, p);
            }
        }

        public async Task ModifyStats(Action<ModifyStatContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyStatContext(Character);

            action?.Invoke(context);
            // TODO: validate stats

            if (!IsInstantiated) return;

            if (context.Flag.HasFlag(ModifyStatType.Skin) ||
                context.Flag.HasFlag(ModifyStatType.Face) ||
                context.Flag.HasFlag(ModifyStatType.Hair))
                AvatarModified();

            using (var p = new Packet(SendPacketOperations.StatChanged))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(false);
                p.Encode<bool>(false);
                await SendPacket(p);
            }
        }
    }
}