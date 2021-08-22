using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Social.Guilds;
using Edelstein.Protocol.Gameplay.Social.Parties;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Users.Stats.Modify;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User
{
    public interface IFieldObjUser : IFieldLife, IStageUser<IField, IFieldObjUser>, IPacketDispatcher
    {
        new int ID { get; }

        bool IsInstantiated { get; set; }
        bool IsConversing { get; }

        IConversationContext ConversationContext { get; }

        ICollection<IFieldSplit> Watching { get; }
        ICollection<IFieldControlledObj> Controlling { get; }

        ICalculatedRates Rates { get; }
        ICalculatedStats Stats { get; }

        IGuild Guild { get; set; }
        IGuildMember GuildMember => Guild?.Members.FirstOrDefault(m => m.CharacterID == ID);

        IParty Party { get; set; }
        IPartyMember PartyMember => Party?.Members.FirstOrDefault(m => m.CharacterID == ID);

        IPacket GetSetFieldPacket();

        Task Message(string message);
        Task Message(IMessage message);

        Task<T> Prompt<T>(Func<
            IConversationSpeaker,
            T
        > function);
        Task<T?> Prompt<T>(Func<
            IConversationSpeaker,
            IConversationSpeaker,
            T
        > function);

        Task Converse(IConversation conversation);
        Task EndConversation();

        Task ModifyStats(Action<IModifyStatContext> action = null, bool exclRequest = false);
        Task ModifyInventory(Action<IModifyMultiInventoryContext> action = null, bool exclRequest = false);
    }
}