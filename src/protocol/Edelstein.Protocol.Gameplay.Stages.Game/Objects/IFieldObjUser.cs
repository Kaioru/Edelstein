using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects
{
    public interface IFieldObjUser : IFieldObj, IStageUser<IField, IFieldObjUser>
    {
        IGameStageUser GameStageUser { get; init; }

        bool IsInstantiated { get; set; }

        IFieldSplit[] Watching { get; }
        ICollection<IFieldControlledObj> Controlling { get; }

        IFieldObj GetWatchedObject(int id);
        T GetWatchedObject<T>(int id) where T : IFieldObj;
        IEnumerable<IFieldObj> GetWatchedObjects();
        IEnumerable<T> GetWatchedObjects<T>() where T : IFieldObj;
    }
}