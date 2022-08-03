using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Objects;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Common.Util.Spatial;
using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game;

public class FieldUser : AbstractFieldLife, IFieldUser
{
    private readonly IGameStageUser _user;

    public FieldUser(
        IGameStageUser user,
        IAccount account,
        IAccountWorld accountWorld,
        ICharacter character
    ) : base(new Point2D(0, 0))
    {
        _user = user;
        Account = account;
        AccountWorld = accountWorld;
        Character = character;
        Observing = new List<IFieldSplit>();
        Controlled = new List<IFieldControllable>();
    }

    public override FieldObjectType Type => FieldObjectType.User;

    public ISocket Socket => _user.Socket;

    public IAccount Account { get; }
    public IAccountWorld AccountWorld { get; }
    public ICharacter Character { get; }

    public bool IsInstantiated { get; set; }

    public ICollection<IFieldSplit> Observing { get; }
    public ICollection<IFieldControllable> Controlled { get; }

    public IPacket GetSetFieldPacket()
    {
        var packet = new PacketWriter(PacketSendOperations.SetField);

        packet.WriteShort(0); // ClientOpt

        packet.WriteInt(_user.Context.Options.ChannelID);
        packet.WriteInt(_user.Context.Options.WorldID);

        packet.WriteBool(true); // sNotifierMessage._m_pStr
        packet.WriteBool(!IsInstantiated);
        packet.WriteShort(0); // nNotifierCheck, loops

        if (!IsInstantiated)
        {
            packet.WriteUInt(0);
            packet.WriteUInt(0);
            packet.WriteUInt(0);

            packet.WriteCharacterData(Character);

            packet.WriteInt(0);
            for (var i = 0; i < 3; i++) packet.WriteInt(0);
        }
        else
        {
            packet.WriteByte(0);
            packet.WriteInt(Character.FieldID);
            packet.WriteByte(Character.FieldPortal);
            packet.WriteInt(Character.HP);
            packet.WriteBool(false);
        }

        packet.WriteDateTime(DateTime.Now);

        return packet;
    }

    public override IPacket GetEnterFieldPacket()
    {
        var packet = new PacketWriter(PacketSendOperations.UserEnterField);

        packet.WriteInt(Character.ID);

        packet.WriteByte(Character.Level);
        packet.WriteString(Character.Name);

        packet.WriteString("");
        packet.WriteShort(0);
        packet.WriteByte(0);
        packet.WriteShort(0);
        packet.WriteByte(0);

        packet.WriteLong(0); // secondary stats
        packet.WriteLong(0);
        packet.WriteByte(0);
        packet.WriteByte(0);

        packet.WriteShort(Character.Job);
        packet.WriteCharacterLooks(Character);

        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);

        packet.WritePoint2D(Position);
        packet.WriteByte(0); // Action?
        packet.WriteShort((short)(Foothold?.ID ?? 0));
        packet.WriteByte(0);

        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteBool(false);

        packet.WriteBool(false);

        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);

        packet.WriteByte(0);

        packet.WriteBool(false);

        packet.WriteBool(false);
        packet.WriteBool(false);
        packet.WriteBool(false);

        packet.WriteByte(0);

        packet.WriteByte(0);
        packet.WriteInt(0);

        return packet;
    }

    public override IPacket GetLeaveFieldPacket() =>
        new PacketWriter(PacketSendOperations.UserLeaveField)
            .WriteInt(Character.ID);

    public Task OnPacket(IPacket packet) => _user.OnPacket(packet);
    public Task OnException(Exception exception) => _user.OnException(exception);
    public Task OnDisconnect() => _user.OnDisconnect();
    public Task Dispatch(IPacket packet) => _user.Dispatch(packet);
    public Task Disconnect() => _user.Disconnect();

    protected override IPacket GetMovePacket(IMovePath ctx)
    {
        var packet = new PacketWriter(PacketSendOperations.UserMove);

        packet.WriteInt(Character.ID);
        packet.Write(ctx);

        return packet;
    }
}
