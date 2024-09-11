using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Objects.Users.Stats;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects.Users;

public class FieldUser(
    IGameStageSystemUser user,
    Account account,
    AccountWorldData accountWorldData,
    Character character,
    IPoint2D position
) : AbstractFieldLife<IFieldUserMovePath, IFieldUserMoveAction>(
        position, 
        null, 
        new FieldUserMoveAction(0)),
    IFieldUser
{
    public override FieldObjectType Type => FieldObjectType.User;
    public ISocket Socket => user.Socket;

    public Account Account => account;
    public AccountWorldData AccountWorldData => accountWorldData;
    public Character Character => character;

    public ICollection<IFieldSplit> Observing { get; } = new List<IFieldSplit>();

    public IFieldUserStats Stats { get; private set; } = new FieldUserStats();

    public bool IsFirstEnter { get; set; } = true;

    public IDispatchable GetDispatchSetField()
        => new SetField
        {
            ChannelID = user.System.Options.ChannelID,
            IsInitialize = IsFirstEnter,
            Info = IsFirstEnter
                ? new SetFieldInfoCharacterInit
                {
                    Seed1 = 0,
                    Seed2 = 0,
                    Seed3 = 0,
                    Data = character.ToStructuredCharacterData()
                }
                : new SetFieldInfoCharacter
                {
                    PosMap = character.FieldID,
                    Portal = character.FieldPortal,
                    HP = character.HP
                },
            DateServer = new FDateTime(user.System.Context.DateTime.Now)
        };
    
    public override IDispatchable GetDispatchEnterField(bool isEnterField = false)
        => new UserEnterField
        {
            ObjectID = ObjectID ?? 0,
            CharacterName = new LPString(character.Name),
            CharacterLook = character.ToStructuredCharacterLook(),
            X = (short)Position.X,
            Y = (short)Position.Y,
            MoveAction = Action.Value,
            Foothold = (short)(Foothold?.ID ?? 0)
        };

    public override IDispatchable GetDispatchLeaveField(bool isLeaveField = false)
        => new UserLeaveField
        {
            ObjectID = ObjectID ?? 0
        };
    
    public Task Initialize()
        => UpdateStats();

    private async Task UpdateStats()
        => Stats = await user.System.Context.Calculators.UserStats.Calculate(this);
}
