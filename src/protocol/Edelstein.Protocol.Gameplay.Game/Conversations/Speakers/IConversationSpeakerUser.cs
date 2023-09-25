﻿namespace Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

public interface IConversationSpeakerUser : IConversationSpeaker
{
    byte Skin { get; set; }
    int Face { get; set; }
    int Hair { get; set; }

    byte Level { get; set; }
    short Job { get; set; }

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

    IConversationSpeakerUserInventory Inventory { get; }
        
    void IncEXP(int amount);
    void IncPOP(short amount);
    void IncMoney(int amount);

    void TransferField(int fieldID, string portal = "");
}
