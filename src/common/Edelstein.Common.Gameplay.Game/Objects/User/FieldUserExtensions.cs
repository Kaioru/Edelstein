using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Objects.User;

public static class FieldUserExtensions
{
    public static Task DispatchSetDirectionMode(this IFieldUser user, bool enable, int delay = 0)
    {
        var p = new PacketWriter(PacketSendOperations.SetDirectionMode);
        p.WriteBool(enable);
        p.WriteInt(delay);
        return user.Dispatch(p.Build());
    }
    
    public static Task DispatchSetStandAloneMode(this IFieldUser user, bool enable)
    {
        var p = new PacketWriter(PacketSendOperations.SetStandAloneMode);
        p.WriteBool(enable);
        return user.Dispatch(p.Build());
    }
}
