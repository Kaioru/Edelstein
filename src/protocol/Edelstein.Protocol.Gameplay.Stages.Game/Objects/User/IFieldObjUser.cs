﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
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

        ICollection<IFieldSplit> Watching { get; }
        ICollection<IFieldControlledObj> Controlling { get; }

        ICalculatedRates Rates { get; }
        ICalculatedStats Stats { get; }

        IPacket GetSetFieldPacket();

        Task<T> Prompt<T>(Func<
            IConversationSpeaker,
            T
        > function);
        Task<T> Prompt<T>(Func<
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