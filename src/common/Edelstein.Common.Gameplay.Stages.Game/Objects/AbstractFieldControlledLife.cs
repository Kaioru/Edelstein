using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects
{
    public abstract class AbstractFieldControlledLife : AbstractFieldLife, IFieldControlledObj
    {
        private IFieldObjUser _controller;

        public IFieldObjUser Controller
        {
            get => _controller;
            set
            {
                if (_controller == value) return;

                _controller?.Controlling.Remove(this);

                if (_controller?.Field == Field)
                    _controller?.Dispatch(GetChangeControllerPacket(false));

                _controller = value;
                if (value == null) return;
                _controller.Controlling.Add(this);
                _controller.Dispatch(GetChangeControllerPacket(true));
            }
        }

        protected abstract IPacket GetChangeControllerPacket(bool setAsController);
    }
}
