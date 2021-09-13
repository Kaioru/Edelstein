namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ISecondaryStat
    {
        SecondaryStatType Type { get; }

        int Value { get; }
        int Reason { get; }

        bool IsExpireAfterMigrate { get; }
        bool IsExpireAfterDisconnect { get; }
    }
}
