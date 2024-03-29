﻿using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers.Facades;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

public interface IConversationSpeakerUser : IConversationSpeaker
{
    byte Skin { get; set; }
    int Face { get; set; }
    int Hair { get; set; }

    byte Level { get; set; }
    short Job { get; set; }
    int JobRace { get; }
    int JobType { get; }
    int JobLevel { get; }
    int JobBranch { get; }

    short STR { get; set; }
    short DEX { get; set; }
    short INT { get; set; }
    short LUK { get; set; }

    int HP { get; set; }
    int MaxHP { get; set; }
    int MP { get; set; }
    int MaxMP { get; set; }

    short AP { get; set; }
    short SP { get; set; }

    int EXP { get; set; }
    short POP { get; set; }

    int Money { get; set; }
    
    int Gender { get; }
    
    ISpeakerUserInventory Inventory { get; }
    ISpeakerUserQuests Quests { get; }
    ISpeakerField? Field { get; }
        
    void IncEXP(int amount);
    void IncPOP(short amount);
    void IncMoney(int amount);

    void TransferField(int fieldID, string portal = "");

    void SetDirectionMode(bool enable, int delay = 0);
    void SetStandAloneMode(bool enable);

    void Message(string message);
    void MessageScriptProgress(string message);
    void MessageBalloon(string message, short? width = null, short? duration = null, IPoint2D? position = null);
    
    void EffectPlayPortalSE();
    void EffectSquib(string path);
    void EffectReserved(string path);
    void EffectAvatarOriented(string path);
    
    void EffectFieldScreen(string path);
    void EffectFieldTremble(bool isHeavyAndShort, int delay);
}
