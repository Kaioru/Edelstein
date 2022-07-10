using Edelstein.Common.Gameplay.Handling;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class WorldInfoRequestHandler : WorldRequestHandler
    {
        public override short Operation => (short)PacketRecvOperations.WorldInfoRequest;
    }
}
