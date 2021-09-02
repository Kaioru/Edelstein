namespace Edelstein.Common.Gameplay.Stages
{
    public interface IServerStageInfo
    {
        public string ID { get; }

        public string Host { get; }
        public short Port { get; }
    }
}
