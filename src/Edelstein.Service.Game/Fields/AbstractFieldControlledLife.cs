using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields
{
    public abstract class AbstractFieldControlledLife : AbstractFieldLife, IFieldControlledObj
    {
        private IFieldUser _controller;

        public IFieldUser Controller
        {
            get => _controller;
            set
            {
                if (_controller?.Field == Field)
                {
                    _controller?.Controlled.Remove(this);
                    _controller?.SendPacket(GetChangeControllerPacket(false));
                }

                _controller = value;
                if (value == null) return;
                _controller.Controlled.Add(this);
                _controller.SendPacket(GetChangeControllerPacket(true));
            }
        }

        protected abstract IPacket GetChangeControllerPacket(bool setAsController);
    }
}