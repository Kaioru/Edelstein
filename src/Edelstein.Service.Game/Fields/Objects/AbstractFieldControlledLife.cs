using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects
{
    public abstract class AbstractFieldControlledLife : AbstractFieldLife, IFieldControlled
    {
        private IFieldUser _controller;

        public IFieldUser Controller
        {
            get => _controller;
            set
            {
                if (_controller == value) return;

                _controller?.Controlling.Remove(this);

                if (_controller?.Field == Field)
                    _controller?.SendPacket(GetChangeControllerPacket(false));

                _controller = value;
                if (value == null) return;
                _controller.Controlling.Add(this);
                _controller.SendPacket(GetChangeControllerPacket(true));
            }
        }

        protected abstract IPacket GetChangeControllerPacket(bool setAsController);
    }
}