using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Continent
{
    public interface IContiMoveInfo : IRepositoryEntry<int>
    {
        public string Name { get; }

        public int Term { get; }
        public int Delay { get; }

        public bool Event { get; }

        public int Wait { get; }
        public int EventEnd { get; }
        public int Required { get; }
    }
}
