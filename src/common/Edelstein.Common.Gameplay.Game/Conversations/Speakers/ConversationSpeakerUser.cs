using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Game.Conversations.Speakers.Facades;
using Edelstein.Common.Gameplay.Game.Objects.User.Effects;
using Edelstein.Common.Gameplay.Game.Objects.User.Effects.Field;
using Edelstein.Common.Gameplay.Game.Objects.User.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers.Facades;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers;

public class ConversationSpeakerUser : ConversationSpeaker, IConversationSpeakerUser
{
    private readonly IFieldUser _user;

    public ConversationSpeakerUser(
        IFieldUser user,
        IConversationContext context, 
        int id = 9010000, 
        ConversationSpeakerFlags flags = 0
    ) : base(context, id, flags) 
        => _user = user;

    public byte Skin
    {
        get => _user.Character.Skin;
        set => _user.ModifyStats(s => s.Skin = value).Wait();
    }

    public int Face
    {
        get => _user.Character.Face;
        set => _user.ModifyStats(s => s.Face = value).Wait();
    }
    
    public int Hair
    {
        get => _user.Character.Hair;
        set => _user.ModifyStats(s => s.Hair = value).Wait();
    }
    
    public byte Level
    {
        get => _user.Character.Level;
        set => _user.ModifyStats(s => s.Level = value).Wait();
    }
    
    public short Job
    {
        get => _user.Character.Job;
        set => _user.ModifyStats(s => s.Job = value).Wait();
    }

    public int JobRace => JobConstants.GetJobRace(_user.Character.Job);
    public int JobType => JobConstants.GetJobType(_user.Character.Job);
    public int JobLevel => JobConstants.GetJobLevel(_user.Character.Job);
    public int JobBranch => JobConstants.GetJobBranch(_user.Character.Job);

    public short STR
    {
        get => _user.Character.STR;
        set => _user.ModifyStats(s => s.STR = value).Wait();
    }
    
    public short DEX
    {
        get => _user.Character.DEX;
        set => _user.ModifyStats(s => s.DEX = value).Wait();
    }

    public short INT
    {
        get => _user.Character.INT;
        set => _user.ModifyStats(s => s.INT = value).Wait();
    }

    public short LUK
    {
        get => _user.Character.LUK;
        set => _user.ModifyStats(s => s.LUK = value).Wait();
    }
    
    public int HP
    {
        get => _user.Character.HP;
        set => _user.ModifyStats(s => s.HP = value).Wait();
    }

    public int MaxHP
    {
        get => _user.Character.MaxHP;
        set => _user.ModifyStats(s => s.MaxHP = value).Wait();
    }

    public int MP
    {
        get => _user.Character.MP;
        set => _user.ModifyStats(s => s.MP = value).Wait();
    }

    public int MaxMP
    {
        get => _user.Character.MaxMP;
        set => _user.ModifyStats(s => s.MaxMP = value).Wait();
    }
    
    public short AP
    {
        get => _user.Character.AP;
        set => _user.ModifyStats(s => s.AP = value).Wait();
    }

    public short SP
    {
        get => _user.Character.SP;
        set => _user.ModifyStats(s => s.SP = value).Wait();
    }
    
    public int EXP
    {
        get => _user.Character.EXP;
        set => _user.ModifyStats(s => s.EXP = value).Wait();
    }

    public short POP
    {
        get => _user.Character.POP;
        set => _user.ModifyStats(s => s.POP = value).Wait();
    }
    
    public int Money
    {
        get => _user.Character.Money;
        set => _user.ModifyStats(s => s.Money = value).Wait();
    }

    public int Gender => _user.Character.Gender;

    public ISpeakerUserInventory Inventory => new SpeakerUserInventory(_user);
    public ISpeakerUserQuests Quests => new SpeakerUserQuests(_user);
    public ISpeakerField? Field => _user.Field == null ? null : new SpeakerField(_user.Field);

    public void IncEXP(int amount)
    {
        EXP += amount;
        _user.Message(new IncEXPMessage(amount, true));
    }
    
    public void IncPOP(short amount)
    {
        POP += amount;
        _user.Message(new IncPOPMessage(amount));
    }
    
    public void IncMoney(int amount)
    {
        Money += amount;
        _user.Message(new IncMoneyMessage(amount));
    }
    
    public void TransferField(int fieldID, string portal = "")
        => _user.StageUser.Context.Managers.Field.Retrieve(fieldID).Result?.Enter(_user, portal);

    public void SetDirectionMode(bool enable, int delay = 0)
        => _user.SetDirectionMode(enable, delay).Wait();
    
    public void SetStandAloneMode(bool enable)
        => _user.SetStandAloneMode(enable).Wait();

    public void Message(string message)
        => _user.Message(message);
    
    public void MessageScriptProgress(string message)
        => _user.MessageScriptProgress(message);

    public void MessageBalloon(string message, short? width = null, short? duration = null, IPoint2D? position = null)
        => _user.MessageBalloon(message, width, duration, position);

    public void EffectPlayPortalSE()
        => _user.Effect(new PlayPortalSEEffect(), isRemote: false);

    public void EffectReserved(string path)
        => _user.Effect(new ReservedEffect(path), isRemote: false);
    
    public void EffectAvatarOriented(string path)
        => _user.Effect(new AvatarOrientedEffect(path), isRemote: false);

    public void EffectFieldScreen(string path)
        => _user.EffectField(new ScreenFieldEffect(path));
    
    public void EffectFieldTremble(bool isHeavyAndShort, int delay)
        => _user.EffectField(new TrembleFieldEffect(isHeavyAndShort, delay));
}
