﻿using Edelstein.Common.Gameplay.Game.Objects.User;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Common.Gameplay.Game;

public class GameStage : AbstractStage<IGameStageUser>, IGameStage
{
    private readonly IFieldManager _fieldManager;

    public GameStage(string id, IFieldManager fieldManager)
    {
        ID = id;
        _fieldManager = fieldManager;
    }

    public override string ID { get; }

    public new async Task Enter(IGameStageUser user)
    {
        if (user.Account == null || user.AccountWorld == null || user.Character == null)
        {
            await user.Disconnect();
            return;
        }

        var field = await _fieldManager.Retrieve(user.Character.FieldID);
        var fieldUser = new FieldUser(user, user.Account, user.AccountWorld, user.Character);

        if (field == null)
        {
            await user.Disconnect();
            return;
        }

        user.FieldUser = fieldUser;

        await field.Enter(fieldUser);
        await base.Enter(user);

        var funcKeyMappedInitPacket = new PacketWriter(PacketSendOperations.FuncKeyMappedInit);

        funcKeyMappedInitPacket.WriteBool(user.Character.FuncKeys.Records.Count == 0);
        if (user.Character.FuncKeys.Records.Count > 0)
        {
            for (byte i = 0; i < 90; i++)
            {
                if (user.Character.FuncKeys.Records.TryGetValue(i, out var value))
                {
                    funcKeyMappedInitPacket.WriteByte(value.Type);
                    funcKeyMappedInitPacket.WriteInt(value.Action);
                }
                else
                {
                    funcKeyMappedInitPacket.WriteByte(0);
                    funcKeyMappedInitPacket.WriteInt(0);
                }
            }
        }
        await fieldUser.Dispatch(funcKeyMappedInitPacket.Build());
    }

    public new async Task Leave(IGameStageUser user)
    {
        if (user.Field != null && user.FieldUser != null)
            await user.Field.Leave(user.FieldUser);
        await base.Leave(user);
    }
}
