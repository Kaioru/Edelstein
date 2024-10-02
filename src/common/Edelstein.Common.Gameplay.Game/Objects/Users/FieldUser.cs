using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Conversations;
using Edelstein.Common.Gameplay.Game.Conversations.Speakers;
using Edelstein.Common.Gameplay.Game.Objects.Users.Stats;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Dialogs;
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

    public IGameStageSystem System => user.System;

    public Account Account => account;
    public AccountWorldData AccountWorldData => accountWorldData;
    public Character Character => character;

    public ICollection<IFieldSplit> Observing { get; } = new List<IFieldSplit>();
    public ICollection<IFieldObjectControllable> Controlling { get; } = new List<IFieldObjectControllable>();

    public IFieldUserStats Stats { get; private set; } = new FieldUserStats();

    public IDialog? ActiveDialog { get; private set; }

    public bool IsFirstEnter { get; set; } = true;

    private readonly SemaphoreSlim _lock = new(1, 1);

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

    public async Task Initialize()
    {
        await _lock.WaitAsync();

        try
        {
            await UpdateStats();

            character.HP = Math.Min(character.HP, Stats.MaxHP);
            character.MP = Math.Min(character.MP, Stats.MaxMP);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task Modify(Action<IFieldUserModify> action)
    {
        await _lock.WaitAsync();

        try
        {
            var modify = new FieldUserModify(this);

            action.Invoke(modify);

            if (modify.IsRequireUpdate)
                await UpdateStats();
            if (modify.IsRequireUpdateAvatar)
                await UpdateAvatar();

            if (Character.HP > Stats.MaxHP)
                await modify.Stats(s => s.HP = Stats.MaxHP);
            if (Character.MP > Stats.MaxMP)
                await modify.Stats(s => s.MP = Stats.MaxMP);
        }
        finally
        {
            _lock.Release();
        }
    }

    public Task<T?> Prompt<T>(Func<IConversationSpeaker, T> prompt) where T : struct
        => Prompt((s, _) => prompt.Invoke(s));

    public async Task<T?> Prompt<T>(Func<IConversationSpeaker, IConversationSpeaker, T> prompt) where T : struct
    {
        T? result = default;

        await Converse(
            new SystemConversation((self, target)
                => result = prompt.Invoke(self, target)),
            ctx => new ConversationSpeaker(ctx),
            ctx => new ConversationSpeaker(ctx, @params: ConversationSpeakerParam.NPCReplacedByUser)
        );

        return result;
    }

    public async Task Converse<TSelf, TTarget>(
        IConversation<TSelf, TTarget> conversation,
        Func<IConversationContext, TSelf> getSpeakerSelf,
        Func<IConversationContext, TTarget> getSpeakerTarget
    )
        where TSelf : IConversationSpeaker
        where TTarget : IConversationSpeaker
    {
        var ctx = new ConversationContext(this);
        var speakerSelf = getSpeakerSelf.Invoke(ctx);
        var speakerTarget = getSpeakerTarget.Invoke(ctx);

        await Task
            .Run(async () =>
            {
                await Dialog(new ConversationDialog(ctx));
                await conversation.Start(ctx, speakerSelf, speakerTarget);
            }, ctx.Token)
            .ContinueWith(async _ =>
            {
                await EndDialog();
                await this.ModifyStats(exclRequest: true);
            });
    }

    public async Task Dialog(IDialog dialog)
    {
        await _lock.WaitAsync();

        try
        {
            if (ActiveDialog != null) return;
            ActiveDialog = dialog;
            ActiveDialog?.OnOpen(this);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task EndDialog()
    {
        await _lock.WaitAsync();

        try
        {
            if (ActiveDialog == null) return;
            ActiveDialog = null;
            ActiveDialog?.OnClose(this);
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task UpdateStats()
        => Stats = await user.System.Context.Calculators.UserStats.Calculate(this);

    private async Task UpdateAvatar()
    {
        if (FieldSplit != null)
            await FieldSplit.Dispatch(new UserAvatarModified
                {
                    ObjectID = Character.ID,
                    Info = new UserAvatarModifiedInfo
                    {
                        CharacterLook = Character.ToStructuredCharacterLook(),
                        CharacterSpeed = (byte)Stats.Speed
                    }
                },
                this
            );
    }
}
