using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Users.Stats.Modify;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects
{
    public interface IFieldObjUser : IFieldObj, IStageUser<IField, IFieldObjUser>
    {
        IGameStage GameStage { get; }
        IGameStageUser GameStageUser { get; init; }

        bool IsInstantiated { get; set; }

        IFieldSplit[] Watching { get; }
        ICollection<IFieldControlledObj> Controlling { get; }

        IFieldObj GetWatchedObject(int id);
        T GetWatchedObject<T>(int id) where T : IFieldObj;
        IEnumerable<IFieldObj> GetWatchedObjects();
        IEnumerable<T> GetWatchedObjects<T>() where T : IFieldObj;

        bool IsConversing { get; }

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

        Task ModifyStats(Action<IModifyStatContext> action, bool exclRequest = false);
        Task ModifyInventory(Action<IModifyMultiInventoryContext> action, bool exclRequest = false);
    }
}