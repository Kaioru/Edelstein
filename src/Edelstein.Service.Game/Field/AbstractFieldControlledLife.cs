using Edelstein.Network.Packet;
using Edelstein.Service.Game.Field.User;

namespace Edelstein.Service.Game.Field
{
    public abstract class AbstractFieldControlledLife : AbstractFieldLife
    {
        public FieldUser Controller { get; private set; }

        public void ChangeController(FieldUser controller)
        {
            if (Controller?.Field == Field)
                Controller?.SendPacket(GetChangeControllerPacket(false));
            Controller = controller;

            if (controller == null) return;
            Controller.SendPacket(GetChangeControllerPacket(true));
        }

        public abstract IPacket GetChangeControllerPacket(bool setAsController);
    }
}