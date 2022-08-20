using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.MessageBox;

public interface IFieldMessageBoxFactory
{
    IFieldMessageBox CreateMessageBox(
        IPoint2D position,
        int itemID,
        string hope,
        string name
    );
}
