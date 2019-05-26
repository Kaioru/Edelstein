namespace Edelstein.Service.Game.Interactions.Miniroom
{
    public interface IMiniroomDialog : IDialog
    {
        IMiniroom Miniroom { get; }
    }
}