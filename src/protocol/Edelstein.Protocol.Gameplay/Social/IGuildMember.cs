namespace Edelstein.Protocol.Gameplay.Social
{
    public interface IGuildMember
    {
        int ID { get; }
        string Name { get; }

        int Job { get; }
        int Level { get; }
        int Grade { get; }
        bool Online { get; }

        int Commitment { get; }
    }
}
