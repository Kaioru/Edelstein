namespace Edelstein.Protocol.Gameplay.Social
{
    public interface IPartyMember
    {
        int ID { get; }
        string Name { get; }

        int Job { get; }
        int Level { get; }
        int Channel { get; }
        int Field { get; }
    }
}
