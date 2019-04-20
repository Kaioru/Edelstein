using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields
{
    public abstract class AbstractFieldControlledLife : AbstractFieldLife, IFieldControlledObj
    {
        public IFieldUser Controller { get; private set; }

        public void SetController(IFieldUser user)
        {
            if (Controller?.Field == Field)
                Controller?.SendPacket(GetChangeControllerPacket(false));
            Controller = user;

            if (user == null) return;
            Controller.SendPacket(GetChangeControllerPacket(true));
        }

        protected abstract IPacket GetChangeControllerPacket(bool setAsController);
    }
}