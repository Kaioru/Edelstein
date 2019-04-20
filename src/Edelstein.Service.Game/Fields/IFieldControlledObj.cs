namespace Edelstein.Service.Game.Fields
{
    public interface IFieldControlledObj : IFieldObj
    {
        void SetController(IFieldUser user);
    }
}